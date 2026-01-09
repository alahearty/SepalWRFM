namespace SepalWRFM.Core.Models
{
    /// <summary>
    /// Data model for WRFM module information.
    /// </summary>
    public class WRFMModuleData
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string IconPath { get; set; }
        public string IconKind { get; set; }
        public string GradientStartColor { get; set; }
        public string GradientEndColor { get; set; }
        public string IconBackgroundColor { get; set; }
        public string Category { get; set; }
    }
}