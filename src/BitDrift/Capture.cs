using System.Collections.Generic;

using Microsoft.Extensions.Logging;

namespace BitDrift;

public static partial class Capture
{
    /// <summary>
    /// Gets or sets a value indicating whether Capture is configured.
    /// </summary>
    public static bool IsConfigured { get; internal set; } = false;

    /// <summary>
    /// Configures Capture with the specified API key and URL.
    /// </summary>
    /// <param name="apiKey"></param>
    /// <param name="url"></param>
    /// <param name="sessionStrategy"></param>
    public static void Configure(
            string apiKey,
            string url = "https://api.bitdrift.io",
            SessionStrategy sessionStrategy = SessionStrategy.ActivityBased)
    {
        if (IsConfigured)
        {
            return;
        }

        ConfigureNative(apiKey, url, sessionStrategy.ToNativeString());

        IsConfigured = true;
    }
    /// <summary>
    /// Adds a field to Capture with the specified key and value.
    /// </summary>
    /// <param name="key">The key of the field.</param>
    /// <param name="value">The value of the field.</param>
    public static void AddField(string key, string value) => AddFieldNative(key, value);

    /// <summary>
    /// Removes the field with the specified key from Capture.
    /// </summary>
    /// <param name="key">The key of the field to remove.</param>
    public static void RemoveField(string key) => RemoveFieldNative(key);

    /// <summary>
    /// Gets the session ID associated with Capture.
    /// </summary>
    /// <returns>The session ID.</returns>
    public static string SessionId() => SessionIdNative();

    /// <summary>
    /// Gets the URL of the session associated with Capture.
    /// </summary>
    /// <returns>The session URL.</returns>
    public static string SessionUrl() => SessionUrlNative();

    /// <summary>
    /// Starts a new session for Capture.
    /// </summary>
    public static void StartNewSession() => StartNewSessionNative();

    /// <summary>
    /// Logs a message with the specified log level.
    /// </summary>
    /// <param name="level">The log level.</param>
    /// <param name="message">The log message.</param>
    /// <param name="arguments">Optional dictionary of additional arguments.</param>
    public static void Log(double level, string message, IDictionary<string, string> arguments = null) => LogNative(level, message, arguments);

    /// <summary>
    /// Logs a message with the specified log level.
    /// </summary>
    /// <param name="level">The log level.</param>
    /// <param name="message">The log message.</param>
    /// <param name="arguments">Optional dictionary of additional arguments.</param>
    public static void Log(LogLevel level, string message, IDictionary<string, string> arguments = null) => LogNative(BitDriftLogger.ToDouble(level), message, arguments);

    /// <summary>
    /// Logs a message with the trace log level.
    /// </summary>
    /// <param name="message">The log message.</param>
    /// <param name="arguments">Optional dictionary of additional arguments.</param>
    public static void LogTrace(string message, IDictionary<string, string> arguments = null) => LogNative(BitDriftLogger.ToDouble(LogLevel.Trace), message, arguments);

    /// <summary>
    /// Logs a message with the debug log level.
    /// </summary>
    /// <param name="message">The log message.</param>
    /// <param name="arguments">Optional dictionary of additional arguments.</param>
    public static void LogDebug(string message, IDictionary<string, string> arguments = null) => LogNative(BitDriftLogger.ToDouble(LogLevel.Debug), message, arguments);

    /// <summary>
    /// Logs a message with the information log level.
    /// </summary>
    /// <param name="message">The log message.</param>
    /// <param name="arguments">Optional dictionary of additional arguments.</param>
    public static void LogInformation(string message, IDictionary<string, string> arguments = null) => LogNative(BitDriftLogger.ToDouble(LogLevel.Information), message, arguments);

    /// <summary>
    /// Logs a message with the trace warning level.
    /// </summary>
    /// <param name="message">The log message.</param>
    /// <param name="arguments">Optional dictionary of additional arguments.</param>
    public static void LogWarning(string message, IDictionary<string, string> arguments = null) => LogNative(BitDriftLogger.ToDouble(LogLevel.Warning), message, arguments);

    /// <summary>
    /// Logs a message with the error log level.
    /// </summary>
    /// <param name="message">The log message.</param>
    /// <param name="arguments">Optional dictionary of additional arguments.</param>
    public static void LogError(string message, IDictionary<string, string> arguments = null) => LogNative(BitDriftLogger.ToDouble(LogLevel.Error), message, arguments);
}