namespace Oyster.Core.Interfaces.Commands
{
    public interface ITakesTime
    {
        // Methods
        /// <summary>
        /// Called when Oyster requests the current command goes faster.
        /// </summary>
        public void MakeItGoFaster();
    }
}