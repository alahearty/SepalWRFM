using System.Collections.Generic;
using SepalWRFM.Core.Models;

namespace SepalWRFM.Services.Interfaces
{
    /// <summary>
    /// Interface for WRFM module data management.
    /// </summary>
    public interface IWRFMModuleService
    {
        /// <summary>
        /// Gets all available WRFM modules.
        /// </summary>
        /// <returns>Collection of WRFM modules.</returns>
        IEnumerable<WRFMModuleData> GetWRFMModules();
    }
}