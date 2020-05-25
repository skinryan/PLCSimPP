using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCI.PLCSimPP.PresentationControls.ValidationAttributes
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
