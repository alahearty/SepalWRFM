using System.Windows;
using System.Windows.Input;
using FullApp.Modules.Copilot.ViewModels;

namespace FullApp.Modules.Copilot.Views
{
    public partial class CopilotView
    {
        public CopilotView()
        {
            InitializeComponent();
            Loaded += CopilotView_Loaded;
            
            // Close popup when clicking outside
            this.Loaded += (s, e) =>
            {
                if (Window.GetWindow(this) is Window window)
                {
                    window.MouseDown += (s2, e2) =>
                    {
                        if (MoreOptionsPopup.IsOpen && !MoreOptionsPopup.IsMouseOver && !MoreOptionsButton.IsMouseOver)
                        {
                            MoreOptionsPopup.IsOpen = false;
                        }
                    };
                }
            };
        }

        private void CopilotView_Loaded(object sender, RoutedEventArgs e)
        {
            // Scroll to bottom when view is loaded
            if (MessageScrollViewer != null)
            {
                MessageScrollViewer.ScrollToEnd();
            }
        }

        private void MoreOptionsButton_Click(object sender, RoutedEventArgs e)
        {
            MoreOptionsPopup.IsOpen = !MoreOptionsPopup.IsOpen;
            e.Handled = true;
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            // Close the popup when any menu item is clicked
            MoreOptionsPopup.IsOpen = false;
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && (Keyboard.Modifiers & ModifierKeys.Control) == 0)
            {
                if (DataContext is CopilotViewModel viewModel)
                {
                    var command = viewModel.SendMessageCommand;
                    if (command?.CanExecute(null) == true)
                    {
                        command.Execute(null);
                        e.Handled = true;
                        // Scroll to bottom after sending
                        MessageScrollViewer?.ScrollToEnd();
                    }
                }
            }
        }
    }
}
