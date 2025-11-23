namespace Oyster.Core.Interfaces.Commands
{
    public interface ISpeechCommand
    {
        // Methods
        /// <summary>
        /// Called once per frame when speech system is running.
        /// </summary>
        /// <returns>True when we need to move to the next line, false otherwise.</returns>
        public bool Run();
        /// <summary>
        /// Called to make an instance of the relevant command.
        /// </summary>
        /// <param name="rawParameters">An array of raw parameters.</param>
        /// <returns>A valid object that implements ISpeechCommand.</returns>
        public ISpeechCommand MakeSelf(string[] rawParameters);
    }
}
