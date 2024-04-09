using System;
using System.Collections.Generic;
using System.Diagnostics;

using Microsoft.Extensions.Logging;

namespace BitDrift;

/// <summary>
/// Represents a logger implementation for BitDrift.
/// </summary>
public partial class BitDriftLogger : ILogger
{
    private readonly string categoryName;
    private readonly LogLevel configLogLevel;

    /// <summary>
    /// Initializes a new instance of the <see cref="BitDriftLogger"/> class.
    /// </summary>
    /// <param name="categoryName">The name of the category for the logger.</param>
    /// <param name="logLevel">The log level for the logger.</param>
    public BitDriftLogger(string categoryName, LogLevel logLevel)
    {
        this.categoryName = categoryName;
        this.configLogLevel = logLevel;
    }

    /// <inheritdoc/>
    public IDisposable BeginScope<TState>(TState state) => NullScope.Instance;

    /// <inheritdoc/>
    public bool IsEnabled(LogLevel logLevel)
    {
        return logLevel >= this.configLogLevel;
    }

    /// <summary>
    /// Converts a <see cref="LogLevel"/> value to a double representation.
    /// </summary>
    /// <param name="logLevel">The log level to convert.</param>
    /// <returns>The double representation of the log level.</returns>
    public static double ToDouble(LogLevel logLevel) => logLevel switch
    {
        LogLevel.Trace => 0,
        LogLevel.Debug => 1,
        LogLevel.Information => 2,
        LogLevel.Warning => 3,
        LogLevel.Error => 4,
        LogLevel.Critical => 4,
        LogLevel.None => throw new ArgumentOutOfRangeException(nameof(logLevel), $"Not expected logLevel value: {logLevel}"),
        _ => throw new ArgumentOutOfRangeException(nameof(logLevel), $"Not expected logLevel value: {logLevel}"),
    };

    /// <inheritdoc/>
    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
    {
        double lvl = ToDouble(logLevel);
        var message = formatter(state, exception);
        var logProperties = new Dictionary<string, string>();
        if (typeof(IDictionary<string, object>).IsAssignableFrom(state.GetType()))
        {
            foreach (var value in (IDictionary<string, object>)state)
            {
                try
                {
                    logProperties[value.Key] = value.Value.ToString();
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                }
            }
        }
        
        Capture.LogNative(lvl, message, logProperties);
    }

    /// <summary>
    /// Represents a null scope for the logger.
    /// </summary>
    internal class NullScope : IDisposable
    {
        /// <summary>
        /// Gets the instance of the null scope.
        /// </summary>
        public static IDisposable Instance { get; } = new NullScope();

        /// <inheritdoc/>
        public void Dispose() { }
    }
}
