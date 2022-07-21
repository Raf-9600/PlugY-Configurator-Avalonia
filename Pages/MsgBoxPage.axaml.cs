using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using ReactiveUI;


namespace PlugY_Configurator_Avalonia.Pages
{
    public partial class MsgBoxPage : UserControl
    {
        public MsgBoxPage()
        {
            InitializeComponent();
            //this.DataContext = new MsgPage();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
