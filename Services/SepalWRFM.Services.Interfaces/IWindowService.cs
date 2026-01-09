using System.Windows;

namespace SepalWRFM.Services.Interfaces
{
    public interface IWindowService
    {
        void OpenSepalWRFMWindow();
        void OpenModuleWindow(string moduleName);
    }
}
