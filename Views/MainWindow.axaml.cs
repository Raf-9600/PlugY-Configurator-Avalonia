using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using System;
using System.Diagnostics;
using System.Collections.Generic;

namespace PlugY_Configurator_Avalonia.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
            AddHandler(DragDrop.DropEvent, Drop);
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void Drop(object _, DragEventArgs e)
        {
            if (e.Data.Contains(DataFormats.FileNames))
                DragDropEnum = e.Data.GetFileNames();
            //Console.WriteLine(e.Data.GetFileNames());

            //foreach (string file in e.Data.GetFileNames())
            //Console.WriteLine(file);
        }

        public IEnumerable<string>? DragDropEnum
        {
            get { return GetValue(DragDropEnumProperty); }
            set { SetValue(DragDropEnumProperty, value); }
        }

        public static readonly StyledProperty<IEnumerable<string>?> DragDropEnumProperty = AvaloniaProperty.Register<Window, IEnumerable<string>?>(nameof(DragDropEnum));


    }
}
