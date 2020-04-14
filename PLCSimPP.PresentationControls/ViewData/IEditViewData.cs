namespace PLCSimPP.PresentationControls.ViewData
{
    /// <summary>
    /// Interface of setup edit data
    /// </summary>
    public interface IEditViewData : IViewData
    {
        /// <summary>
        /// Is data loaded
        /// </summary>
        bool IsLoaded { get; set; }
        /// <summary>
        /// Is data value changed
        /// </summary>
        bool IsValueChanged { get; }
        /// <summary>
        /// Save data
        /// </summary>
        void Save();
        /// <summary>
        /// If has any error
        /// </summary>
        bool HaveError { get; }
        /// <summary>
        /// Initialize
        /// </summary>
        void Initialize();
        /// <summary>
        /// Loaded
        /// </summary>
        void Loaded();
    }
    /// <summary>
    /// Interface for data
    /// </summary>
    public interface IViewData { }
}
