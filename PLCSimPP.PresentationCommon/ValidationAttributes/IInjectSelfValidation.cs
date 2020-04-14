namespace PLCSimPP.PresentationCommon.ValidationAttributes
{
    /// <summary>
    /// Inject Self Validation interface
    /// </summary>
    public interface IInjectSelfValidation
    {
        /// <summary>
        /// Self object
        /// </summary>
        object Self { get; set; }
    }
}
