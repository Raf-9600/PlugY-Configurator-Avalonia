using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using System;
using System.IO;
using Avalonia.Data.Core.Plugins;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Threading;
using PlugY_Configurator_Avalonia.ViewModels;
using PlugY_Configurator_Avalonia.Views;

namespace PlugY_Configurator_Avalonia
{
    public class App : Application
    {
        public static readonly string MainSettingsDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "PlugY Configurator");
        public static readonly string MainSettingsFile = Path.Combine(MainSettingsDir, "Settings.json");
        public static readonly bool FirstStart = AppSettings.TryReadAppSettings(MainSettingsFile);
        public static Window? MainWindow;
        public static IClassicDesktopStyleApplicationLifetime? Desktop;

        public static readonly System.Text.Json.JsonSerializerOptions jsonOptions = new System.Text.Json.JsonSerializerOptions
        {
            ReadCommentHandling = System.Text.Json.JsonCommentHandling.Skip,
            WriteIndented = true,
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };


        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public delegate void InfoHandler();
        public static event InfoHandler? LoadedSettings;

        public override void OnFrameworkInitializationCompleted()
        {
            BindingPlugins.DataValidators.RemoveAt(0);

            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                Desktop = desktop;
                Desktop.ShutdownMode = ShutdownMode.OnMainWindowClose;
                //Desktop.ShutdownRequested += ClosingEvent;
                MainWindow = Desktop.MainWindow = new MainWindow
                {
                    DataContext = AppSettings.LoadSettings(new MainWindowViewModel()) //DataContext = new MainViewModel()
                };
                LoadedSettings?.Invoke();
            }

            base.OnFrameworkInitializationCompleted();

            if (OperatingSystem.IsWindows())
            {
                //Thread.CurrentThread.SetApartmentState(ApartmentState.Unknown);
                Thread.CurrentThread.SetApartmentState(ApartmentState.STA);
            }
        }
    }

}

[JsonSerializable(typeof(Dictionary<string, string>))]
internal partial class DictionaryTypeInfoResolver : JsonSerializerContext
{ }

public enum OperatingSystemType
{
    Unknown,
    WinNT,
    Linux,
    OSX,
    Android,
    iOS
}