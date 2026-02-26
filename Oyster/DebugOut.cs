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

        // Public Methods
        /// <summary>
        /// Logs an informational error message.
        /// </summary>
        public static void Log(string message)
        {
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
            // Given not static, raise event
            if (OnDebugMessage != null)
            {
                OnDebugMessage(message, Severity.Error);
            }
        }
    }
}
