using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace PlugY_Configurator_Avalonia.Pages
{
    public partial class MainPage : UserControl
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
