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
            //this.AttachDevTools();
#endif
            AddHandler(DragDrop.DropEvent, Drop);
        }


        [STAThread]
        private void Drop(object _, DragEventArgs e)
        {
            if (e.Data.Contains(DataFormats.Files))
            {
                IEnumerator<Avalonia.Platform.Storage.IStorageItem>? tmpEnum = e.Data.GetFiles()?.GetEnumerator();
                if (tmpEnum != null)
                {
                    List<string> result = new();
                    while (tmpEnum.MoveNext())   // пока не будет возвращено false
                    {
                        result.Add(tmpEnum.Current.Path.LocalPath);
                    }
                    DragDropEnum = result;
                }
            }
        }

        public List<string> DragDropEnum
        {
            get { return GetValue(DragDropEnumProperty); }
            set { SetValue(DragDropEnumProperty, value); }
        }

        public static readonly StyledProperty<List<string>> DragDropEnumProperty = AvaloniaProperty.Register<Window, List<string>>(nameof(DragDropEnum));


    }
}
