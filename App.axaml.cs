using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Text.Json;
using System.Threading;
using Avalonia.Markup.Xaml.Styling;
using Avalonia.Themes.Fluent;
using PlugY_Configurator_Avalonia.ViewModels;
using PlugY_Configurator_Avalonia.Views;

namespace PlugY_Configurator_Avalonia
{
    public partial class App : Application
    {
        public static readonly string MainSettingsDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "PlugY Configurator");
        public static readonly string MainSettingsFile = Path.Combine(MainSettingsDir, "Settings.json");
        public static readonly bool FirstStart = !File.Exists(MainSettingsFile);

        public static readonly System.Text.Json.JsonSerializerOptions jsonOptions = new System.Text.Json.JsonSerializerOptions
        {
            ReadCommentHandling = System.Text.Json.JsonCommentHandling.Skip,
            WriteIndented = true,
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };

        public static IClassicDesktopStyleApplicationLifetime desktop;


        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop1)
            {
                desktop = desktop1;
                desktop.ShutdownMode = ShutdownMode.OnMainWindowClose;

                // Инициализируем хуки приостановки. 
                var suspension = new AutoSuspendHelper(ApplicationLifetime);
                RxApp.SuspensionHost.CreateNewAppState = () => new MainWindowViewModel();
                RxApp.SuspensionHost.SetupDefaultSuspendResume(new NewtonsoftJsonSuspensionDriver(MainSettingsFile));
                suspension.OnFrameworkInitializationCompleted();

                desktop.MainWindow = new MainWindow
                {
                    DataContext = RxApp.SuspensionHost.GetAppState<MainWindowViewModel>()
                };
            }

            base.OnFrameworkInitializationCompleted();
        }


        // https://github.com/AvaloniaUI/Avalonia/blob/4cd6ff6a8923982b666c3a7bbeaa000a1a90ec85/samples/ControlCatalog/App.xaml.cs
        public static FluentTheme Fluent = new FluentTheme(new Uri("avares://ControlCatalog/Styles"));

    }
}
