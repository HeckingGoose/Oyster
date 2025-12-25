using Oyster.Core.AbstractTypes;

namespace UnitTests.OysterImplementation.Object
{
    internal class StringAsAssetLoader : A_BackgroundAssetLoader<string>
    {
        // Constructor
        public StringAsAssetLoader(string script)
        {
            // Pass Value
            _asset = script;
        }

        // Public Methods
        public override void BeginAssetLoad()
        {
            // Raise event
            InvokeOnAssetLoad(LoadResult.Succeeded);
        }
    }
}
