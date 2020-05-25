using BCI.PLCSimPP.PresentationControls.ViewData;

namespace BCI.PLCSimPP.PresentationControls
{
    /// <summary>
    /// Handle setup configuration save event
    /// </summary>
    public delegate void ControllerSaveHandle();
    /// <summary>
    /// Handle loading data event
    /// </summary>
    public delegate void ControllerLoadViewDatasHandle();
    /// <summary>
    /// Handle validation event
    /// </summary>
    public delegate void ControllerValidationErrorHandle();
    /// <summary>
    /// Interface of controller
    /// </summary>
    public interface IController
    {
        /// <summary>
        /// view data of controller
        /// </summary>
        IEditViewData Data { get; set; }
        /// <summary>
        /// Save data
        /// </summary>
        /// <returns></returns>
        bool Save();

        /// <summary>
        /// Loading data
        /// </summary>
        void LoadViewDatas();
        /// <summary>
        /// Validation
        /// </summary>
        /// <returns></returns>
        bool CheckOut();
        /// <summary>
        /// Saving event
        /// </summary>
        event ControllerSaveHandle Saving;

        /// <summary>
        /// Loading data event
        /// </summary>
        event ControllerLoadViewDatasHandle LoadViewDatasing;
        /// <summary>
        /// Validation event
        /// </summary>
        event ControllerValidationErrorHandle ValidationError;
    }
}
