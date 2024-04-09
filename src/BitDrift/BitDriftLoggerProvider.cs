using Microsoft.Extensions.Logging;

namespace BitDrift
{
    /// <summary>
    /// Represents a logger provider for BitDrift.
    /// </summary>
    public partial class BitDriftLoggerProvider : ILoggerProvider
    {

        private readonly LogLevel logLevel;

        /// <summary>
        /// Initializes a new instance of the <see cref="BitDriftLoggerProvider"/> class.
        /// </summary>
        /// <param name="logLevel">The minimum log level to be logged.</param>
        /// <param name="apiKey">The API key for BitDrift.</param>
        /// <param name="url">The URL for BitDrift.</param>
        /// <param name="sessionStrategy">The session strategy for BitDrift.</param>
        public BitDriftLoggerProvider(
            LogLevel logLevel, 
            string apiKey, 
            string url,
            SessionStrategy sessionStrategy)
        {
            this.logLevel = logLevel;

            Capture.Configure(apiKey, url, sessionStrategy);

            AddPropertiesNative();
        }

        /// <summary>
        /// Creates a new instance of the <see cref="BitDriftLogger"/> class.
        /// </summary>
        /// <param name="categoryName">The name of the logger category.</param>
        /// <returns>A new instance of the <see cref="BitDriftLogger"/> class.</returns>
        public ILogger CreateLogger(string categoryName)
        {
            return new BitDriftLogger(categoryName, this.logLevel);
        }

        /// <summary>
        /// Disposes the BitDrift logger provider.
        /// </summary>
        public void Dispose() { }
    }
}
