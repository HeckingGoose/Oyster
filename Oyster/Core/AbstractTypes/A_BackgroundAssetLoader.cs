namespace Oyster.Core.AbstractTypes
{
    public abstract class A_BackgroundAssetLoader<AssetType> where AssetType : class
    {
        // Const
        protected const string DEFAULT_LOG = "No message supplied.";

        // Enum
        public enum LoadResult
        {
            Failed,
            Succeeded
        }

        // Delegates
        public delegate void AssetLoadedDelegate(LoadResult loadResult, string log);

        // Events
        public AssetLoadedDelegate? OnLoadFinished;

        // Protected
        protected AssetType? _asset = default;

        // Constructor
        public A_BackgroundAssetLoader() { }

        // Protected Methods
        /// <summary>
        /// Should be called when the asset has completed loading.
        /// </summary>
        /// <param name="asset">The resulting asset that was loaded.</param>
        /// <param name="loadResult">Whether the load was successful or not.</param>
        protected void InvokeOnAssetLoad(LoadResult loadResult, string log = DEFAULT_LOG)
        {
            // Call load completed event
            if (OnLoadFinished != null) { OnLoadFinished(loadResult, log); }
        }

        // Public Method
        /// <summary>
        /// Begins loading this asset.
        /// </summary>
        public abstract void BeginAssetLoad();

        // Accessors
        /// <summary>
        /// Returns the asset being loaded. Returns the default value for this type if the asset is not loaded.
        /// </summary>
        public AssetType? Asset { get { return _asset; } }
    }
}