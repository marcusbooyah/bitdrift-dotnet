using Microsoft.Extensions.Logging;

namespace BitDrift
{
    /// <summary>
    /// Provides extension methods for configuring BitDrift logging in an <see cref="ILoggingBuilder"/>.
    /// </summary>
    public static class LoggingExensions
    {
        /// <summary>
        /// Adds BitDrift logging to the specified <see cref="ILoggingBuilder"/>.
        /// </summary>
        /// <param name="builder">The <see cref="ILoggingBuilder"/> to configure.</param>
        /// <param name="apiKey">The API key for authenticating with the BitDrift service.</param>
        /// <param name="url">The URL of the BitDrift service. Defaults to "https://api.bitdrift.io".</param>
        /// <param name="minimumLogLevel">The minimum log level to be logged. Defaults to <see cref="LogLevel.Information"/>.</param>
        /// <param name="sessionStrategy">The session strategy to be used. Defaults to <see cref="SessionStrategy.ActivityBased"/>.</param>
        public static void AddBitDrift(
            this ILoggingBuilder builder,
            string apiKey,
            string url = "https://api.bitdrift.io",
            LogLevel minimumLogLevel = LogLevel.Information,
            SessionStrategy sessionStrategy = SessionStrategy.ActivityBased)
        {
            builder.AddProvider(new BitDriftLoggerProvider(minimumLogLevel, apiKey, url, sessionStrategy));
        }
    }
}