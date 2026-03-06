namespace Oyster
{
    public static class DebugOut
    {
        // Enum
        public enum Severity
        {
            Log,
            Warn,
            Error
        }

        // Events
        public delegate void OnDebugMessageDelegate(string message, Severity severity);
        public static OnDebugMessageDelegate? OnDebugMessage;

        // Private Variables
        private static bool _enabled = true;

        // Public Methods
        /// <summary>
        /// Logs an informational error message.
        /// </summary>
        public static void Log(string message)
        {
            // Skip if disabled
            if (!_enabled) return;

            // Given not static, raise event
            if (OnDebugMessage != null)
            {
                OnDebugMessage(message, Severity.Log);
            }
        }
        /// <summary>
        /// Logs a potentially scary error message.
        /// </summary>
        public static void Warn(string message)
        {
            // Skip if disabled
            if (!_enabled) return;

            // Given not static, raise event
            if (OnDebugMessage != null)
            {
                OnDebugMessage(message, Severity.Warn);
            }
        }
        /// <summary>
        /// Logs a proper scary error message.
        /// </summary>
        public static void Error(string message)
        {
            // Skip if disabled
            if (!_enabled) return;

            // Given not static, raise event
            if (OnDebugMessage != null)
            {
                OnDebugMessage(message, Severity.Error);
            }
        }

        // Accessors
        /// <summary>
        /// Gets or sets whether this class will actually raise events for messages.
        /// </summary>
        public static bool Enabled { get { return _enabled; } set { _enabled = value; } }
    }
}
