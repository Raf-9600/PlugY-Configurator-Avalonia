using Avalonia;
using Avalonia.Controls;
using Avalonia.Platform;
using Avalonia.Threading;
using Newtonsoft.Json;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Threading;
using AvaloniaLocalizationExample.Localizer;
using Avalonia.Markup.Xaml;
using System.Linq;
using Avalonia.Themes.Fluent;
using Avalonia.Media;
using Microsoft.Win32;
using DynamicData;
using System.Reflection;
using Avalonia.Markup.Xaml.Styling;

namespace PlugY_Configurator_Avalonia.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        readonly Models.MainModel _model = new Models.MainModel();
        private Models.IniWork _ini = new Models.IniWork();
        private string _plugyDir = "";

        [DataMember]
        [JsonProperty("Path to PlugY")]
        private string _plugyFullPathJson;
        public string PlugyFullPathJson
        {
            get => _plugyFullPathJson;
            set { this.RaiseAndSetIfChanged(ref _plugyFullPathJson, value); }
        }

        private string _plugyFullPath;
        public string PlugyFullPath
        {
            get { return _plugyFullPath; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    _plugyDir = Path.GetDirectoryName(value);

                    _ini.NotSet = true;
                    _ini.Initialization(value);
                    Upd();
                    _ini.NotSet = false;
                    PlugyFullPathJson = value;
                }
                this.RaiseAndSetIfChanged(ref _plugyFullPath, value);
            }
        }

        //        private void Upd(Models.IniWork ini)
        private async void Upd()
        {
            // LAUNCHING
            Param = _ini.GetVal("LAUNCHING", "Param", string.Empty);
            Library = _ini.GetVal("LAUNCHING", "Library", string.Empty);

            // GENERAL
            ActivePlugin = _ini.GetVal("GENERAL", "ActivePlugin", false);
            DisableBattleNet = _ini.GetVal("GENERAL", "DisableBattleNet", false);
            ActiveLogFile = _ini.GetVal("GENERAL", "ActiveLogFile", false);
            DllToLoad = _ini.GetVal("GENERAL", "DllToLoad", string.Empty);
            DllToLoad2 = _ini.GetVal("GENERAL", "DllToLoad2", string.Empty);
            ActiveCommands = _ini.GetVal("GENERAL", "ActiveCommands", false);
            ActiveCheckMemory = _ini.GetVal("GENERAL", "ActiveCheckMemory", false);
            ActiveAllOthersFeatures = _ini.GetVal("GENERAL", "ActiveAllOthersFeatures", false);

            // WINDOWED
            ActiveWindowed = _ini.GetVal("WINDOWED", "ActiveWindowed", false);
            RemoveBorder = _ini.GetVal("WINDOWED", "RemoveBorder", false);
            WindowOnTop = _ini.GetVal("WINDOWED", "WindowOnTop", false);
            Maximized = _ini.GetVal("WINDOWED", "Maximized", false);
            SetWindowPos = _ini.GetVal("WINDOWED", "SetWindowPos", false);
            WindowedX = _ini.GetVal("WINDOWED", "X", 0);
            WindowedY = _ini.GetVal("WINDOWED", "Y", 0);
            WindowedWidth = _ini.GetVal("WINDOWED", "Width", 0);
            WindowedHeight = _ini.GetVal("WINDOWED", "Height", 0);
            LockMouseOnStartup = _ini.GetVal("WINDOWED", "LockMouseOnStartup", false);

            // LANGUAGE
            //ActiveChangeLanguage = _ini.GetVal("LANGUAGE", "ActiveChangeLanguage", false);
            ActiveChangeLanguage = await ShowMsgBaseModConflict("Language", "Enabled", _ini.GetVal("LANGUAGE", "ActiveChangeLanguage", false));
            ActiveLanguageManagement = _ini.GetVal("LANGUAGE", "ActiveLanguageManagement", false);

            var selectedLanguage_ini = _ini.GetVal("LANGUAGE", "SelectedLanguage", string.Empty);
            SelectedLanguage = Array.IndexOf(_languageListWrite, selectedLanguage_ini);

            var defaultLanguage_ini = _ini.GetVal("LANGUAGE", "DefaultLanguage", string.Empty);
            DefaultLanguage = Array.IndexOf(_languageListWrite, defaultLanguage_ini);

            AvlblLngs_ENG = _ini.GetAvailableLanguages("ENG");
            AvlblLngs_ESP = _ini.GetAvailableLanguages("ESP");
            AvlblLngs_DEU = _ini.GetAvailableLanguages("DEU");
            AvlblLngs_FRA = _ini.GetAvailableLanguages("FRA");
            AvlblLngs_POR = _ini.GetAvailableLanguages("POR");
            AvlblLngs_ITA = _ini.GetAvailableLanguages("ITA");
            AvlblLngs_JPN = _ini.GetAvailableLanguages("JPN");
            AvlblLngs_KOR = _ini.GetAvailableLanguages("KOR");
            AvlblLngs_SIN = _ini.GetAvailableLanguages("SIN");
            AvlblLngs_CHI = _ini.GetAvailableLanguages("CHI");
            AvlblLngs_POL = _ini.GetAvailableLanguages("POL");
            AvlblLngs_RUS = _ini.GetAvailableLanguages("RUS");


            // SAVEPATH
            ActiveSavePathChange = _ini.GetVal("SAVEPATH", "ActiveSavePathChange", false);
            SavePath = _ini.GetVal("SAVEPATH", "SavePath", string.Empty);

            // MAIN SCREEN
            ActiveVersionTextChange = _ini.GetVal("MAIN SCREEN", "ActiveVersionTextChange", false);
            VersionText = _ini.GetVal("MAIN SCREEN", "VersionText", string.Empty);
            ColorOfVersionText = _ini.GetVal("MAIN SCREEN", "ColorOfVersionText", 0);
            //ColorOfVersionText = ColorToNum(_ini.GetVal("MAIN SCREEN", "ColorOfVersionText", 0));

            ActivePrintPlugYVersion = _ini.GetVal("MAIN SCREEN", "ActivePrintPlugYVersion", false);
            ColorOfPlugYVersion = _ini.GetVal("MAIN SCREEN", "ColorOfPlugYVersion", 0);

            // STASH
            ActiveBigStash = _ini.GetVal("STASH", "ActiveBigStash", false);
            ActiveMultiPageStash = _ini.GetVal("STASH", "ActiveMultiPageStash", false);
            NbPagesPerIndex = _ini.GetVal("STASH", "NbPagesPerIndex", 0);
            NbPagesPerIndex2 = _ini.GetVal("STASH", "NbPagesPerIndex2", 0);
            MaxPersonnalPages = _ini.GetVal("STASH", "MaxPersonnalPages", 0);
            AutoRenameStashPage = _ini.GetVal("STASH", "AutoRenameStashPage", false);
            PersonalNormalPageColor = _ini.GetVal("STASH", "PersonalNormalPageColor", 0);
            PersonalIndexPageColor = _ini.GetVal("STASH", "PersonalIndexPageColor", 12);
            PersonalMainIndexPageColor = _ini.GetVal("STASH", "PersonalMainIndexPageColor", 9);
            SharedNormalPageColor = _ini.GetVal("STASH", "SharedNormalPageColor", 4);
            SharedIndexPageColor = _ini.GetVal("STASH", "SharedIndexPageColor", 8);
            SharedMainIndexPageColor = _ini.GetVal("STASH", "SharedMainIndexPageColor", 1);
            ActiveSharedStash = _ini.GetVal("STASH", "ActiveSharedStash", false);
            ActiveSharedStashInMultiPlayer = _ini.GetVal("STASH", "ActiveSharedStashInMultiPlayer", false);
            SeparateHardcoreStash = _ini.GetVal("STASH", "SeparateHardcoreStash", false);
            OpenSharedStashOnLoading = _ini.GetVal("STASH", "OpenSharedStashOnLoading", false);
            SharedStashFilename = _ini.GetVal("STASH", "SharedStashFilename", string.Empty);
            DisplaySharedSetItemNameInGreen = _ini.GetVal("STASH", "DisplaySharedSetItemNameInGreen", false);
            MaxSharedPages = _ini.GetVal("STASH", "MaxSharedPages", 0);
            ActiveSharedGold = _ini.GetVal("STASH", "ActiveSharedGold", false);
            PosXPreviousBtn = _ini.GetVal("STASH", "PosXPreviousBtn", -1);
            PosYPreviousBtn = _ini.GetVal("STASH", "PosYPreviousBtn", -1);
            PosWPreviousBtn = _ini.GetVal("STASH", "PosWPreviousBtn", 32);
            PosHPreviousBtn = _ini.GetVal("STASH", "PosHPreviousBtn", 32);
            PosXNextBtn = _ini.GetVal("STASH", "PosXNextBtn", -1);
            PosYNextBtn = _ini.GetVal("STASH", "PosYNextBtn", -1);
            PosWNextBtn = _ini.GetVal("STASH", "PosWNextBtn", 32);
            PosHNextBtn = _ini.GetVal("STASH", "PosHNextBtn", 32);
            PosXSharedBtn = _ini.GetVal("STASH", "PosXSharedBtn", -1);
            PosYSharedBtn = _ini.GetVal("STASH", "PosYSharedBtn", -1);
            PosWSharedBtn = _ini.GetVal("STASH", "PosWSharedBtn", 32);
            PosHSharedBtn = _ini.GetVal("STASH", "PosHSharedBtn", 32);
            PosXPreviousIndexBtn = _ini.GetVal("STASH", "PosXPreviousIndexBtn", -1);
            PosYPreviousIndexBtn = _ini.GetVal("STASH", "PosYPreviousIndexBtn", -1);
            PosWPreviousIndexBtn = _ini.GetVal("STASH", "PosWPreviousIndexBtn", 32);
            PosHPreviousIndexBtn = _ini.GetVal("STASH", "PosHPreviousIndexBtn", 32);
            PosXNextIndexBtn = _ini.GetVal("STASH", "PosXNextIndexBtn", -1);
            PosYNextIndexBtn = _ini.GetVal("STASH", "PosYNextIndexBtn", -1);
            PosWNextIndexBtn = _ini.GetVal("STASH", "PosWNextIndexBtn", 32);
            PosHNextIndexBtn = _ini.GetVal("STASH", "PosHNextIndexBtn", 32);
            PosXPutGoldBtn = _ini.GetVal("STASH", "PosXPutGoldBtn", -1);
            PosYPutGoldBtn = _ini.GetVal("STASH", "PosYPutGoldBtn", -1);
            PosWPutGoldBtn = _ini.GetVal("STASH", "PosWPutGoldBtn", 32);
            PosHPutGoldBtn = _ini.GetVal("STASH", "PosHPutGoldBtn", 32);
            PosXTakeGoldBtn = _ini.GetVal("STASH", "PosXTakeGoldBtn", -1);
            PosYTakeGoldBtn = _ini.GetVal("STASH", "PosYTakeGoldBtn", -1);
            PosWTakeGoldBtn = _ini.GetVal("STASH", "PosWTakeGoldBtn", 32);
            PosHTakeGoldBtn = _ini.GetVal("STASH", "PosHTakeGoldBtn", 32);
            PosXStashNameField = _ini.GetVal("STASH", "PosXStashNameField", -1);
            PosYStashNameField = _ini.GetVal("STASH", "PosYStashNameField", -1);
            PosWStashNameField = _ini.GetVal("STASH", "PosWStashNameField", 175);
            PosHStashNameField = _ini.GetVal("STASH", "PosHStashNameField", 20);
            PosXStashGoldField = _ini.GetVal("STASH", "PosXStashGoldField", -1);
            PosYStashGoldField = _ini.GetVal("STASH", "PosYStashGoldField", -1);
            PosWStashGoldField = _ini.GetVal("STASH", "PosWStashGoldField", 152);
            PosHStashGoldField = _ini.GetVal("STASH", "PosHStashGoldField", 18);

            // STATS POINTS
            ActiveStatsUnassignment = _ini.GetVal("STATS POINTS", "ActiveStatsUnassignment", false);
            KeyUsed = _ini.GetVal("STATS POINTS", "KeyUsed", 18);

            ActiveShiftClickLimit = _ini.GetVal("STATS POINTS", "ActiveShiftClickLimit", false);
            LimitValueToShiftClick = _ini.GetVal("STATS POINTS", "LimitValueToShiftClick", 5);

            // STAT ON LEVEL UP
            ActiveStatPerLevelUp = _ini.GetVal("STAT ON LEVEL UP", "ActiveStatPerLevelUp", false);
            StatPerLevelUp = _ini.GetVal("STAT ON LEVEL UP", "StatPerLevelUp", 5);

            // SKILLS POINTS
            ActiveSkillsUnassignment = _ini.GetVal("SKILLS POINTS", "ActiveSkillsUnassignment", false);
            ActiveSkillsUnassignmentOneForOne = _ini.GetVal("SKILLS POINTS", "ActiveSkillsUnassignmentOneByOne", false);
            PosXUnassignSkillBtn = _ini.GetVal("SKILLS POINTS", "PosXUnassignSkillBtn", -1);
            PosYUnassignSkillBtn = _ini.GetVal("SKILLS POINTS", "PosYUnassignSkillBtn", -1);

            // SKILL ON LEVEL UP
            ActiveSkillPerLevelUp = _ini.GetVal("SKILL ON LEVEL UP", "ActiveSkillPerLevelUp", false);
            SkillPerLevelUp = _ini.GetVal("SKILL ON LEVEL UP", "SkillPerLevelUp", 1);

            // WORLD EVENT
            // ActiveWorldEvent = _ini.GetVal("WORLD EVENT", "ActiveWorldEvent", false);
            ActiveWorldEvent = await ShowMsgBaseModConflict("WorldEvent", "Enabled", _ini.GetVal("WORLD EVENT", "ActiveWorldEvent", false));
            ShowCounterInAllDifficulty = _ini.GetVal("WORLD EVENT", "ShowCounterInAllDifficulty", false);
            ItemsToSell = _ini.GetVal("WORLD EVENT", "ItemsToSell", "The Stone of Jordan");
            MonsterID = _ini.GetVal("WORLD EVENT", "MonsterID", 333);
            OwnSOJSoldChargeFor = _ini.GetVal("WORLD EVENT", "OwnSOJSoldChargeFor", 100);
            InititalSOJSoldMin = _ini.GetVal("WORLD EVENT", "InititalSOJSoldMin", 200);
            InititalSOJSoldMax = _ini.GetVal("WORLD EVENT", "InititalSOJSoldMax", 3000);
            TriggerAtEachSOJSoldMin = _ini.GetVal("WORLD EVENT", "TriggerAtEachSOJSoldMin", 75);
            TriggerAtEachSOJSoldMax = _ini.GetVal("WORLD EVENT", "TriggerAtEachSOJSoldMax", 125);
            ActiveAutoSell = _ini.GetVal("WORLD EVENT", "ActiveAutoSell", false);
            TimeBeforeAutoSellMin = _ini.GetVal("WORLD EVENT", "TimeBeforeAutoSellMin", 0);
            TimeBeforeAutoSellMax = _ini.GetVal("WORLD EVENT", "TimeBeforeAutoSellMax", 1200);

            // UBER QUEST
            // ActiveUberQuest = _ini.GetVal("UBER QUEST", "ActiveUberQuest", false);
            ActiveUberQuest = await ShowMsgBaseModConflict("RedPortals", "Enabled", _ini.GetVal("UBER QUEST", "ActiveUberQuest", true));
            UberMephistoX = _ini.GetVal("UBER QUEST", "UberMephistoX", 25130);
            UberMephistoY = _ini.GetVal("UBER QUEST", "UberMephistoY", 5143);
            UberDiabloX = _ini.GetVal("UBER QUEST", "UberDiabloX", 25139);
            UberDiabloY = _ini.GetVal("UBER QUEST", "UberDiabloY", 5139);
            UberBaalX = _ini.GetVal("UBER QUEST", "UberBaalX", 25139);
            UberBaalY = _ini.GetVal("UBER QUEST", "UberBaalY", 5135);
            // ActiveUberMinions = _ini.GetVal("UBER QUEST", "ActiveUberMinions", true);
            ActiveUberMinions = await ShowMsgBaseModConflict("UberMinions", "Enabled", _ini.GetVal("UBER QUEST", "ActiveUberMinions", true));
            UberMephistoSpawnPercent = _ini.GetVal("UBER QUEST", "UberMephistoSpawnPercent", 80);
            UberMephistoSpawnRadius = _ini.GetVal("UBER QUEST", "UberMephistoSpawnRadius", 30);
            UberBaalSpawnPercent = _ini.GetVal("UBER QUEST", "UberBaalSpawnPercent", 30);
            UberBaalSpawnRadius = _ini.GetVal("UBER QUEST", "UberBaalSpawnRadius", 30);
            UberDiabloSpawnPercent = _ini.GetVal("UBER QUEST", "UberDiabloSpawnPercent", 30);
            UberDiabloSpawnRadius = _ini.GetVal("UBER QUEST", "UberDiabloSpawnRadius", 30);
            ActiveUberDiabloRushTweekAI = _ini.GetVal("UBER QUEST", "ActiveUberDiabloRushTweekAI", true);
            ActiveUberBaalTeleportTweekAI = _ini.GetVal("UBER QUEST", "ActiveUberBaalTeleportTweekAI", true);
            ActiveUberBaalChillingArmorTweekAI = _ini.GetVal("UBER QUEST", "ActiveUberBaalChillingArmorTweekAI", true);
            UberBaalChillingArmorTimer = _ini.GetVal("UBER QUEST", "UberBaalChillingArmorTimer", 6000);

            // INTERFACE
            ActiveNewStatsInterface = _ini.GetVal("INTERFACE", "ActiveNewStatsInterface", false);
            SelectMainPageOnOpenning = _ini.GetVal("INTERFACE", "SelectMainPageOnOpenning", false);
            PrintButtonsBackgroundOnMainStatsPage = _ini.GetVal("INTERFACE", "PrintButtonsBackgroundOnMainStatsPage", false);

            // EXTRA
            ActiveLaunchAnyNumberOfLOD = _ini.GetVal("EXTRA", "ActiveLaunchAnyNumberOfLOD", false);
            // AlwaysRegenMapInSP = _ini.GetVal("EXTRA", "AlwaysRegenMapInSP", false);
            AlwaysRegenMapInSP = await ShowMsgBaseModConflict("MapAutoRegen", "Enabled", _ini.GetVal("EXTRA", "AlwaysRegenMapInSP", false));
            // NBPlayersByDefault = _ini.GetVal("EXTRA", "NBPlayersByDefault", 0);
            NBPlayersByDefault = await ShowMsgBaseModConflict("PlayersX", "Enabled", _ini.GetVal("EXTRA", "NBPlayersByDefault", 0));
            // ActiveDisplayItemLevel = _ini.GetVal("EXTRA", "ActiveDisplayItemLevel", false);
            ActiveDisplayItemLevel = await ShowMsgBaseModConflict("IlvlDisplay", "Enabled", _ini.GetVal("EXTRA", "ActiveDisplayItemLevel", false));
            AlwaysDisplayLifeAndManaValues = _ini.GetVal("EXTRA", "AlwaysDisplayLifeAndManaValues", 0);
            EnabledTXTFilesWhenMSExcelOpenIt = _ini.GetVal("EXTRA", "EnabledTXTFilesWhenMSExcelOpenIt", false);
            ActiveDisplayBaseStatsValue = _ini.GetVal("EXTRA", "ActiveDisplayBaseStatsValue", false);
            // ActiveLadderRunewords = _ini.GetVal("EXTRA", "ActiveLadderRunewords", false);
            ActiveLadderRunewords = await ShowMsgBaseModConflict("LadderRuneWords", "Enabled", _ini.GetVal("EXTRA", "ActiveLadderRunewords", false));
            ActiveCowPortalWhenCowKingWasKilled = _ini.GetVal("EXTRA", "ActiveCowPortalWhenCowKingWasKilled", false);
            // ActiveDoNotCloseNihlathakPortal = _ini.GetVal("EXTRA", "ActiveDoNotCloseNihlathakPortal", false);
            ActiveDoNotCloseNihlathakPortal = await ShowMsgBaseModConflict("NilPortalFix", "Enabled", _ini.GetVal("EXTRA", "ActiveDoNotCloseNihlathakPortal", false));
            MoveCainNearHarrogathWaypoint = _ini.GetVal("EXTRA", "MoveCainNearHarrogathWaypoint", false);
        }


        private async Task<bool> ShowMsgBaseModConflict(string baseIniSection, string baseIniKey, bool plugyParam)
        {
            if (!plugyParam) return false;

            if (_ini.IniBaseMod?.GetValue(baseIniSection, baseIniKey, 0) == 1)
            {
                var msgResult = await ShowMsgBox(Localizer.Instance["PlugYconflictBaseMod_Core"].Replace("%PYparam%", Localizer.Instance["ActiveDisplayItemLevel"]).Replace("%BMparam%", $"[{baseIniSection}] {baseIniKey}=1"),
                    Localizer.Instance["PlugYconflictBaseMod_Title"], MsgBoxIcon.Warning, Localizer.Instance["PlugYconflictBaseMod_Btn1"], Localizer.Instance["PlugYconflictBaseMod_Btn2"]);

                if (msgResult == 1)
                    _ini.IniBaseMod.SetValue(baseIniSection, baseIniKey, 0);
                else return false;
            }
            return true;
        }

        private async Task<int> ShowMsgBaseModConflict(string baseIniSection, string baseIniKey, int plugyParam)
        {
            if (plugyParam == 0) return 0;

            if (_ini.IniBaseMod?.GetValue(baseIniSection, baseIniKey, 0) == 1)
            {
                var msgResult = await ShowMsgBox(Localizer.Instance["PlugYconflictBaseMod_Core"].Replace("%PYparam%", Localizer.Instance["ActiveDisplayItemLevel"]).Replace("%BMparam%", $"[{baseIniSection}] {baseIniKey}=1"),
                    Localizer.Instance["PlugYconflictBaseMod_Title"], MsgBoxIcon.Warning, Localizer.Instance["PlugYconflictBaseMod_Btn1"], Localizer.Instance["PlugYconflictBaseMod_Btn2"]);

                if (msgResult == 1)
                    _ini.IniBaseMod.SetValue(baseIniSection, baseIniKey, 0);
                else return 0;
            }
            return plugyParam;
        }

        private void ShowMsgBaseModConflict(string baseIniSection, string baseIniKey, string nameProp)
        {
            if (_ini.IniBaseMod?.GetValue(baseIniSection, baseIniKey, 0) == 1)
            {
                ShowMsgBoxAsync(Localizer.Instance["PlugYconflictBaseMod_Core"].Replace("%PYparam%", Localizer.Instance["ActiveDisplayItemLevel"]).Replace("%BMparam%", $"[{baseIniSection}] {baseIniKey}=1"),
                    Localizer.Instance["PlugYconflictBaseMod_Title"], MsgBoxIcon.Warning, Localizer.Instance["PlugYconflictBaseMod_Btn1"], Localizer.Instance["PlugYconflictBaseMod_Btn2"]);
                MsgClick += (s) =>
                {
                    if (s == 1)
                        _ini.IniBaseMod.SetValue(baseIniSection, baseIniKey, 0);
                    else
                        this.GetType()?.GetProperty(nameProp)?.SetValue(this, false);
                };
            }
        }

        private void ShowMsgBaseModConflict(string baseIniSection, string baseIniKey, string nameProp, int disableValue)
        {
            if (_ini.IniBaseMod?.GetValue(baseIniSection, baseIniKey, 0) == 1)
            {
                ShowMsgBoxAsync(Localizer.Instance["PlugYconflictBaseMod_Core"].Replace("%PYparam%", Localizer.Instance["ActiveDisplayItemLevel"]).Replace("%BMparam%", $"[{baseIniSection}] {baseIniKey}=1"),
                    Localizer.Instance["PlugYconflictBaseMod_Title"], MsgBoxIcon.Warning, Localizer.Instance["PlugYconflictBaseMod_Btn1"], Localizer.Instance["PlugYconflictBaseMod_Btn2"]);
                MsgClick += (s) =>
                {
                    if (s == 1)
                        _ini.IniBaseMod.SetValue(baseIniSection, baseIniKey, 0);
                    else
                        this.GetType()?.GetProperty(nameProp)?.SetValue(this, disableValue);
                };
            }
        }

        public MainWindowViewModel()
        {
            if (App.FirstStart)
            {
                EnabDarkThemeGUI(_model.DetectDarkTheme() ?? true);
            }

            MainWindowViewModelAsync();
        }

        private async void MainWindowViewModelAsync()
        {
            if (App.FirstStart)
            {
                EnabDarkThemeGUI(_model.DetectDarkTheme() ?? true);
                switch (Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName)
                {
                    case "en":
                        LngGui = "English";
                        break;
                    case "ru":
                        LngGui = "�������";
                        break;
                    case "de":
                        LngGui = "Deutsch";
                        break;
                    //case "uk":
                    //SelfLng_ukr = true;
                    //break;
                    default:
                       await SelectLng();
                        break;
                }

                async Task SelectLng()
                {
                    var (btnNum, cmbBxIndex) = await ShowMsgBox(message: "Select the language to use during the installation.", title: "Select Setup Language", icon: MsgBoxIcon.Help, comboBox: CmbBoxLng_Items);
                        if (btnNum == 1)
                        {
                            switch (cmbBxIndex)
                            {
                                case -1:
                                await SelectLng();
                                    break;
                                case 0:
                                    LngGui = "English";
                                    break;
                                case 1:
                                    LngGui = "�������";
                                    break;
                                case 2:
                                    LngGui = "Deutsch";
                                    break;
                                    //case 2:
                                    //SelfLng_ukr = true;
                                    //break;
                            }
                        }
                }
            }
            else
            {
                using var fs = new FileStream(App.MainSettingsFile, FileMode.Open, FileAccess.Read);
                JsonDocument? post = JsonDocument.Parse(fs);

                LngGui = post.RootElement.GetProperty("Language").GetString() ?? string.Empty;
            }
            //LngGui = "English";


            try
            {
                string[] args = Environment.GetCommandLineArgs();
                string workFile = string.Empty;

                if (args.Length > 1)
                    workFile = _model.FindPlugyIni(args);



                if (!File.Exists(workFile))
                {
                    workFile = _model.FindWorkDir("PlugY.ini");
                    if (!string.IsNullOrEmpty(workFile))
                        workFile = Path.Combine(workFile, "PlugY.ini");
                }

                if (!File.Exists(workFile) && !App.FirstStart)
                {
                    using var fs = new FileStream(App.MainSettingsFile, FileMode.Open, FileAccess.Read);
                    JsonDocument? post = JsonDocument.Parse(fs);
                    try
                    {
                        workFile = post.RootElement.GetProperty("Path to PlugY").GetString() ?? string.Empty;
                    }
                    catch (System.Exception)
                    { workFile = ""; }
                }
                //workFile = App._mainSettings.Get().installationPath;

                if (!File.Exists(workFile))
                {
                    workFile = _model.FindInstalledDiablo2();
                    if (!string.IsNullOrEmpty(workFile))
                        workFile = Path.Combine(workFile, "PlugY.ini");
                }

                if (File.Exists(workFile))
                    PlugyFullPath = workFile;
                else
                {
                    var msgResult = await ShowMsgBox(Localizer.Instance["PlugYiniNotFound_Question"], Localizer.Instance["PlugYiniNotFound_Heading"], MsgBoxIcon.Error);
                    if (msgResult == 1)
                    {
                        await PlugYRefresh_ClickAsync(); //Dispatcher.UIThread.InvokeAsync(() => ActivePlugin = true);
                        if (!File.Exists(PlugyFullPath))
                            App.desktop.Shutdown();
                        //Environment.Exit(0);
                    };
                }
            }
            catch (Exception e)
            {
                ShowMsgBoxAsync(e.Message, Localizer.Instance["Error"], MsgBoxIcon.Error);
            }
        }

        private IEnumerable<string>? _eventDrop;
        public IEnumerable<string>? EventDrop
        {
            get => _eventDrop;
            set
            {
                PlugyFullPath = _model.FindPlugyIni(value);
                this.RaiseAndSetIfChanged(ref _eventDrop, value);
            }
        }

        [DataMember]
        [JsonProperty("WindowWidth")]
        private double _windowWidth = 1240;
        public double WindowWidth
        {
            get =>_windowWidth;
            set => this.RaiseAndSetIfChanged(ref _windowWidth, value);
        }

        [DataMember]
        [JsonProperty("WindowHeight")]
        private double _windowHeight = 640;
        public double WindowHeight
        {
            get => _windowHeight;
            set => this.RaiseAndSetIfChanged(ref _windowHeight, value);
        }

        public ObservableCollection<string> CmbBoxLng_Items { get; set; } = new ObservableCollection<string>() { "English", "�������", "Deutsch" };


        private string cmbBoxLng_Slctd;
        public string CmbBoxLng_Slctd
        {
            get => cmbBoxLng_Slctd;
            set
            {
                LngGui = value;
                this.RaiseAndSetIfChanged(ref cmbBoxLng_Slctd, value);
            }
        }

        [DataMember]
        [JsonProperty("Language")]
        private string _lngGui;
        public string LngGui
        {
            get => cmbBoxLng_Slctd;
            set
            {
                switch (value)
                {
                    case "English":
                        Localizer.Instance.LoadLanguage("en");
                        SelfLng_rus = false;
                        break;
                    case "�������":
                        Localizer.Instance.LoadLanguage("ru");
                        SelfLng_rus = true;
                        break;
                    case "Deutsch":
                        Localizer.Instance.LoadLanguage("de");
                        SelfLng_rus = false;
                        break;
                    /* case "���������":
                        Localizer.Instance.LoadLanguage("uk");
                        SelfLng_ukr = false;
                        break; */
                }

                this.RaiseAndSetIfChanged(ref _lngGui, value);
            }
        }

        private bool _selfLng_rus;
        public bool SelfLng_rus
        {
            get => _selfLng_rus;
            set => this.RaiseAndSetIfChanged(ref _selfLng_rus, value);
        }


        public void BtnClick(string parameter)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = parameter,
                UseShellExecute = true
            });
        }

        public void BtnClipboardClick(string parameter)
        {
            Application.Current?.Clipboard?.SetTextAsync(parameter);
        }

        public void BtnDefaultClick(string parameter)
        {
            if (_ini._iniDefault == null) return;

            var pArray = parameter.Split("__");
            //Type thisType = typeof(MainWindowViewModel);
            //PropertyInfo? myPropInfo = thisType.GetProperty(pArray[1], BindingFlags.Public | BindingFlags.Instance);

            var defVal = _ini._iniDefault?.GetValue(pArray[0].Replace('_', ' '), pArray[1], "");

            switch (this.GetType()?.GetProperty(pArray[1])?.PropertyType.Name)
            {
                case "Boolean":
                    this.GetType()?.GetProperty(pArray[1])?.SetValue(this, (defVal == "1"));
                    break;
                case "String":
                    this.GetType()?.GetProperty(pArray[1])?.SetValue(this, defVal);
                    break;
                case "Int32":
                    this.GetType()?.GetProperty(pArray[1])?.SetValue(this, Convert.ToInt32(defVal));
                    break;
            }


            
        }


        #region ThemeGUI
        private SolidColorBrush _themeBackgroundColor = SolidColorBrush.Parse("#FFFFFFFF");
        public SolidColorBrush ThemeBackgroundColor
        {
            get => _themeBackgroundColor;
            set => this.RaiseAndSetIfChanged(ref _themeBackgroundColor, value);
        }

        private SolidColorBrush _themeInvertColor = SolidColorBrush.Parse("#3F3F3F");
        public SolidColorBrush ThemeInvertColor
        {
            get => _themeInvertColor;
            set => this.RaiseAndSetIfChanged(ref _themeInvertColor, value);
        }

        [DataMember]
        [JsonProperty("DarkTheme")]
        private bool _darkThemeGUI;
        public bool DarkThemeGUI
        {
            get
            {
                EnabDarkThemeGUI(_darkThemeGUI);
                return _darkThemeGUI;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref _darkThemeGUI, value);
            }
        }

        // https://github.com/wieslawsoltes/Avalonia.ThemeManager
        // https://github.com/AvaloniaUI/Avalonia/blob/4cd6ff6a8923982b666c3a7bbeaa000a1a90ec85/samples/ControlCatalog/MainView.xaml.cs
        private void EnabDarkThemeGUI(bool enab)
        {
            if (Application.Current != null)
            {
                Uri? uri = null;

                if (enab)
                {
                    if (App.Fluent.Mode != FluentThemeMode.Dark)
                    {
                        App.Fluent.Mode = FluentThemeMode.Dark;
                    }
                    Application.Current.Styles[0] = App.Fluent;
                    //Application.Current.Styles[1] = App.DataGridFluent;


                    ThemeBackgroundColor = SolidColorBrush.Parse("#252525");
                    MsgBox_BackgroundColor = SolidColorBrush.Parse("#3F3F3F");
                    ThemeInvertColor = SolidColorBrush.Parse("#fff5f5f5");

                    uri = new Uri("avares://PlugY Configurator Avalonia/Styles/AccentColorsDark.axaml");
                }
                else
                {
                    if (App.Fluent.Mode != FluentThemeMode.Light)
                    {
                        App.Fluent.Mode = FluentThemeMode.Light;
                    }
                    Application.Current.Styles[0] = App.Fluent;
                    //Application.Current.Styles[1] = App.DataGridFluent;


                    ThemeBackgroundColor = SolidColorBrush.Parse("#FFFFFFFF");
                    MsgBox_BackgroundColor = SolidColorBrush.Parse("#fff5f5f5");
                    ThemeInvertColor = SolidColorBrush.Parse("#3F3F3F");

                    uri = new Uri("avares://PlugY Configurator Avalonia/Styles/AccentColorsLight.axaml");
                }

                if (uri != null)
                {
                    var baseAppUru = new Uri("avares://ControlCatalog/Styles");
                    var styleInclude = new StyleInclude(baseAppUru)
                    {
                        Source = uri
                    };
                    Application.Current.Styles.Add(styleInclude);
                }
            }



            /*
           var uri = new Uri("avares://PlugY Configurator Avalonia/Styles/AccentColors.axaml");
           StyleInclude styleInclude = new StyleInclude(uri)
           {
               Source = uri
           };
           //Avalonia.Application.Current.Styles[0] = styleInclude;
           Application.Current.Styles.Add(styleInclude); */

            /*
            SetStyles();

            StyleInclude CreateStyle(string url)
            {
                var self = new Uri("resm:Styles?assembly=PlugY Configurator Avalonia");
                return new StyleInclude(self)
                {
                    Source = new Uri(url)
                };
            }

            void SetStyles()
            {
                var newStyles = CreateStyle("avares://PlugY Configurator Avalonia/Styles/AccentColors.axaml");
                Avalonia.Application.Current.Styles[0] = newStyles;
            }
            */



            //Avalonia.Application.Current.Styles[0] = styleInclude;
            //Application.Current.Styles[0] = styleInclude;

            SharedMainIndexPageColor = SharedMainIndexPageColor;
            SharedIndexPageColor = SharedIndexPageColor;
            SharedNormalPageColor = SharedNormalPageColor;
            PersonalMainIndexPageColor = PersonalMainIndexPageColor;
            PersonalIndexPageColor = PersonalIndexPageColor;
            PersonalNormalPageColor = PersonalNormalPageColor;
            ColorOfPlugYVersion = ColorOfPlugYVersion;
            ColorOfVersionText = ColorOfVersionText;
        }
        #endregion

        public async Task PlugYRefresh_ClickAsync()
        {
            OpenFileDialog dlg = new()
            {
                Title = Localizer.Instance["PlugYiniNotFound_Question"],
                Directory = _model.FindInstalledDiablo2(), // Environment.GetFolderPath(Environment.SpecialFolder.MyComputer) // Name = Localizer.Instance["DlgFolderPlugyIni_AllIni"], Extensions = new List<string>()
                AllowMultiple = false,
                InitialFileName = "PlugY.ini"
            };
            dlg.Filters?.Add(new FileDialogFilter() { Name = Localizer.Instance["DlgFolderPlugyIni_AllIni"], Extensions = { "ini" } });
            dlg.Filters?.Add(new FileDialogFilter() { Name = Localizer.Instance["DlgFolderPlugyIni_AllFiles"], Extensions = { "*" } });

            string[]? result = await dlg.ShowAsync(App.desktop.MainWindow);

            if (result != null)
                PlugyFullPath = result[0];
        }

        public void PlugYDefault_BtnClick()
        {
            ShowMsgBoxAsync(Localizer.Instance["DlgDifaultPlugYIni_Core"], Localizer.Instance["DlgDifaultPlugYIni_Title"], MsgBoxIcon.Help, Localizer.Instance["Yes"], Localizer.Instance["No"]);
            MsgClick += (s) =>
            {
                if (s == 1)
                {
                    //var i1 = new TIniFile.INIFile(Path.Combine(_plugyDir, "PlugY.ini"), false, true);
                    var i2 = new TIniFile.INIFile(Path.Combine(_plugyDir, "PlugY", "PlugYDefault.ini"));

                    //i1.SetValue("LAUNCHING", "Param", "");
                    //_ini._iniPlugy.m_Sections = i2.m_Sections;

                    _ini._iniPlugy.m_Modified = i2.m_Sections;
                    _ini._iniPlugy.m_CacheModified = true;
                    _ini._iniPlugy.Flush();

                    PlugyFullPath = PlugyFullPath;
                }
            };
        }

        #region LaunchParam_Flyout
        private bool _param_Flyout_Open;
        public bool Param_Flyout_Open
        {
            get { return _param_Flyout_Open; }
            set
            {
                if (value)
                {
                    Act1 = Param.Contains("-act 1");
                    Act2 = Param.Contains("-act 2");
                    Act3 = Param.Contains("-act 3");
                    Act4 = Param.Contains("-act 4");
                    Act5 = Param.Contains("-act 5");

                    GameParam_WindowMode = Param.Contains("-w");
                    GameParam_nofixaspect = Param.Contains("-nofixaspect");
                    GameParam_Direct = Param.Contains("-direct");
                    GameParam_Txt = Param.Contains("-txt");
                    GameParam_Ns = Param.Contains("-ns");
                    GameParam_sndbkg = Param.Contains("-sndbkg");
                    GameParam_skiptobnet = Param.Contains("-skiptobnet");
                    GameParam_nosave = Param.Contains("-nosave");
                    GameParam_3dfx = Param.Contains("-3dfx");
                }

                this.RaiseAndSetIfChanged(ref _param_Flyout_Open, value);
            }
        }

        private bool _act1;
        public bool Act1
        {
            get { return _act1; }
            set
            {
                if (value) Param = _model.ActAddRemove(Param, "-act 1");
                this.RaiseAndSetIfChanged(ref _act1, value);
            }
        }

        private bool _act2;
        public bool Act2
        {
            get { return _act2; }
            set
            {
                if (value) Param = _model.ActAddRemove(Param, "-act 2");
                this.RaiseAndSetIfChanged(ref _act2, value);
            }
        }

        private bool _act3;
        public bool Act3
        {
            get { return _act3; }
            set
            {
                if (value) Param = _model.ActAddRemove(Param, "-act 3");
                this.RaiseAndSetIfChanged(ref _act3, value);
            }
        }

        private bool _act4;
        public bool Act4
        {
            get { return _act4; }
            set
            {
                if (value) Param = _model.ActAddRemove(Param, "-act 4");
                this.RaiseAndSetIfChanged(ref _act4, value);
            }
        }

        private bool _act5;
        public bool Act5
        {
            get { return _act5; }
            set
            {
                if (value) Param = _model.ActAddRemove(Param, "-act 5");
                this.RaiseAndSetIfChanged(ref _act5, value);
            }
        }

        private bool _gameParam_WindowMode;
        public bool GameParam_WindowMode
        {
            get { return _gameParam_WindowMode; }
            set
            {
                if (value)
                    Param = _model.AddParam(Param, "-w");
                else if (!string.IsNullOrEmpty(Param))
                    Param = Param.Replace("-w", "");

                this.RaiseAndSetIfChanged(ref _gameParam_WindowMode, value);

                if (ActiveWindowed != value)
                    ActiveWindowed = value;
                
                _model.GlideWindowed((value || ActiveWindowed) && Param.Contains("-3dfx"));

            }
        }

        private bool _gameParam_nofixaspect;
        public bool GameParam_nofixaspect
        {
            get { return _gameParam_nofixaspect; }
            set
            {
                if (value)
                    Param = _model.AddParam(Param, "-nofixaspect");
                else if (!string.IsNullOrEmpty(Param))
                    Param = Param.Replace("-nofixaspect", "");

                this.RaiseAndSetIfChanged(ref _gameParam_nofixaspect, value);
            }
        }

        private bool _gameParam_Direct;
        public bool GameParam_Direct
        {
            get { return _gameParam_Direct; }
            set
            {
                if (value)
                    Param = _model.AddParam(Param, "-direct");
                else if (!string.IsNullOrEmpty(Param))
                    Param = Param.Replace("-direct", "");

                this.RaiseAndSetIfChanged(ref _gameParam_Direct, value);
            }
        }

        private bool _gameParam_Txt;
        public bool GameParam_Txt
        {
            get { return _gameParam_Txt; }
            set
            {
                if (value)
                    Param = _model.AddParam(Param, "-txt");
                else if (!string.IsNullOrEmpty(Param))
                     Param = Param.Replace("-txt", "");

                this.RaiseAndSetIfChanged(ref _gameParam_Txt, value);
            }
        }

        private bool _gameParam_Ns;
        public bool GameParam_Ns
        {
            get { return _gameParam_Ns; }
            set
            {
                if (value)
                    Param = _model.AddParam(Param, "-ns");
                else if (!string.IsNullOrEmpty(Param))
                    Param = Param.Replace("-ns", "");

                this.RaiseAndSetIfChanged(ref _gameParam_Ns, value);
            }
        }

        private bool _gameParam_sndbkg;
        public bool GameParam_sndbkg
        {
            get { return _gameParam_sndbkg; }
            set
            {
                if (value)
                    Param = _model.AddParam(Param, "-sndbkg");
                else if (!string.IsNullOrEmpty(Param))
                    Param = Param.Replace("-sndbkg", "");

                this.RaiseAndSetIfChanged(ref _gameParam_sndbkg, value);
            }
        }

        private bool _gameParam_skiptobnet;
        public bool GameParam_skiptobnet
        {
            get { return _gameParam_skiptobnet; }
            set
            {
                if (value)
                    Param = _model.AddParam(Param, "-skiptobnet");
                else if (!string.IsNullOrEmpty(Param))
                    Param = Param.Replace("-skiptobnet", "");

                this.RaiseAndSetIfChanged(ref _gameParam_skiptobnet, value);
            }
        }

        private bool _gameParam_nosave;
        public bool GameParam_nosave
        {
            get { return _gameParam_nosave; }
            set
            {
                if (value) Param = _model.AddParam(Param, "-nosave");
                else if (!string.IsNullOrEmpty(Param))
                    Param = Param.Replace("-nosave", "");

                this.RaiseAndSetIfChanged(ref _gameParam_nosave, value);
            }
        }

        private bool _gameParam_3dfx;
        public bool GameParam_3dfx
        {
            get { return _gameParam_3dfx; }
            set
            {
                if (value) Param = _model.AddParam(Param, "-3dfx");
                else if (!string.IsNullOrEmpty(Param))
                    Param = Param.Replace("-3dfx", "");

                this.RaiseAndSetIfChanged(ref _gameParam_3dfx, value);
                _model.GlideWindowed((GameParam_WindowMode || ActiveWindowed) && value);
            }
        }
        #endregion

        #region [LAUNCHING]
        public void Param_Open()
        {
            Param_Flyout_Open = true;
        }

        private string _param = "";
        public string Param
        {
            get { return _param; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    value = value.ToLower().Trim();
                    value = value.Replace("  ", " ");
                }

                _ini.SetVal("LAUNCHING", "Param", value);
                this.RaiseAndSetIfChanged(ref _param, value);
            }
        }

        private string _library = "";
        public string Library
        {
            get { return _library; }
            set
            {
                _ini.SetVal("LAUNCHING", "Library", value);
                this.RaiseAndSetIfChanged(ref _library, value);
            }
        }
        #endregion

        #region [GENERAL]
        private bool _activePlugin;
        public bool ActivePlugin
        {
            get { return _activePlugin; }
            set
            {
                _ini.SetVal("GENERAL", "ActivePlugin", value);

                //Dispatcher.UIThread.InvokeAsync(() => this.RaiseAndSetIfChanged(ref _activePlugin, value));
                this.RaiseAndSetIfChanged(ref _activePlugin, value);
            }
        }

        private bool _disableBattleNet;
        public bool DisableBattleNet
        {
            get { return _disableBattleNet; }
            set
            {
                _ini.SetVal("GENERAL", "DisableBattleNet", value);
                this.RaiseAndSetIfChanged(ref _disableBattleNet, value);
            }
        }

        private bool _activeLogFile;
        public bool ActiveLogFile
        {
            get { return _activeLogFile; }
            set
            {
                _ini.SetVal("GENERAL", "ActiveLogFile", value);
                this.RaiseAndSetIfChanged(ref _activeLogFile, value);
            }
        }

        private string _dllToLoad;
        public string DllToLoad
        {
            get { return _dllToLoad; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                    value.Replace('/', '\\');

                _ini.SetVal("GENERAL", "DllToLoad", value);
                this.RaiseAndSetIfChanged(ref _dllToLoad, value);
            }
        }

        private string _dllToLoad2;
        public string DllToLoad2
        {
            get { return _dllToLoad2; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                    value.Replace('/', '\\');

                _ini.SetVal("GENERAL", "DllToLoad2", value);
                this.RaiseAndSetIfChanged(ref _dllToLoad2, value);
            }
        }

        private string _dllToLoad_Flyout;
        public string DllToLoad_Flyout
        {
            get { return _dllToLoad_Flyout; }
            set { this.RaiseAndSetIfChanged(ref _dllToLoad_Flyout, value); }
        }

        private string _dllToLoad2_Flyout;
        public string DllToLoad2_Flyout
        {
            get { return _dllToLoad2_Flyout; }
            set { this.RaiseAndSetIfChanged(ref _dllToLoad2_Flyout, value); }
        }

        private bool _dllToLoad_Flyout_Open;
        public bool DllToLoad_Flyout_Open
        {
            get { return _dllToLoad_Flyout_Open; }
            set
            {
                if (value)
                    DllToLoad_Flyout = DllToLoad.Replace("|", Environment.NewLine);
                else
                    DllToLoad = DllToLoad_Flyout.Trim().Replace(Environment.NewLine, "|").Replace("||", "|");

                this.RaiseAndSetIfChanged(ref _dllToLoad_Flyout_Open, value);
            }
        }

        private bool _dllToLoad2_Flyout_Open;
        public bool DllToLoad2_Flyout_Open
        {
            get { return _dllToLoad2_Flyout_Open; }
            set
            {
                if (value)
                    DllToLoad2_Flyout = DllToLoad2.Replace("|", Environment.NewLine);
                else
                    DllToLoad2 = DllToLoad2_Flyout.Trim().Replace(Environment.NewLine, "|").Replace("||", "|");

                this.RaiseAndSetIfChanged(ref _dllToLoad2_Flyout_Open, value);
            }
        }

        public async void DllToLoad_BtnClick(string param)
        {
            string title = Localizer.Instance["SetDllForPlugy"];
            bool multiFile = true;
            if (param == "Library")
            {
                title = Localizer.Instance["SetPlugyDll"];
                multiFile = false;
            }

            OpenFileDialog dlg = new()
            {
                Title = title,
                Directory = _plugyDir,
                AllowMultiple = multiFile
            };
            dlg.Filters?.Add(new FileDialogFilter() { Name = Localizer.Instance["DlgFolderDllForPlugy"], Extensions = { "dll" } });
            dlg.Filters?.Add(new FileDialogFilter() { Name = Localizer.Instance["DlgFolderPlugyIni_AllFiles"], Extensions = { "*" } });

            string[]? result = await dlg.ShowAsync(App.desktop.MainWindow);

            if (result != null)
            {
                if (!result[0].StartsWith(_plugyDir))
                    await ShowMsgBox(Localizer.Instance["SetPlugyDll_DirIncorrect"], Localizer.Instance["DirIncorrect"], MsgBoxIcon.Help);
                else
                {
                    var prop = this.GetType()?.GetProperty(param);

                    string oldValue = "";
                    if (param != "Library")
                        oldValue = (string?)(prop?.GetValue(this)) ?? "";

                    var r = _model.DllToLoad(oldValue, result, _plugyDir);
                    prop?.SetValue(this, r);
                }
            }
        }

        private bool _activeCommands;
        public bool ActiveCommands
        {
            get { return _activeCommands; }
            set
            {
                _ini.SetVal("GENERAL", "ActiveCommands", value);
                this.RaiseAndSetIfChanged(ref _activeCommands, value);
            }
        }

        private string _activeCommands_Hint;
        public string ActiveCommands_Hint
        {
            get { return _activeCommands_Hint; }
            set { this.RaiseAndSetIfChanged(ref _activeCommands_Hint, value); }
        }

        private bool _activeCommands_Hint_Open;
        public bool ActiveCommands_Hint_Open
        {
            get
            {
                ActiveCommands_Hint = Localizer.Instance["ActiveCommands_Hint"]
                   .Replace("%AlwaysDisplayLifeAndManaValues%", Localizer.Instance["AlwaysDisplayLifeAndManaValues"].Replace(":", ""))
                   .Replace("%Display%", Localizer.Instance["Display"])
                   .Replace("%OnCommandOnly%", Localizer.Instance["OnCommandOnly"])
                   //.Replace(":", "")
                   .Replace("\n", Environment.NewLine + Environment.NewLine) + Environment.NewLine;

                return _activeCommands_Hint_Open;
            }
            set { this.RaiseAndSetIfChanged(ref _activeCommands_Hint_Open, value); }
        }

        public void ActiveCommands_Flyout_Open()
        {
            ActiveCommands_Hint = Localizer.Instance["ActiveCommands_Hint"]
                .Replace("%AlwaysDisplayLifeAndManaValues%", Localizer.Instance["AlwaysDisplayLifeAndManaValues"])
                .Replace("%Display%", Localizer.Instance["Display"])
                .Replace("%OnCommandOnly%", Localizer.Instance["OnCommandOnly"])
                .Replace(":", "")
                .Replace("\n", Environment.NewLine + Environment.NewLine);
        }

        private bool _activeCheckMemory;
        public bool ActiveCheckMemory
        {
            get { return _activeCheckMemory; }
            set
            {
                _ini.SetVal("GENERAL", "ActiveCheckMemory", value);
                this.RaiseAndSetIfChanged(ref _activeCheckMemory, value);
            }
        }

        private bool _activeAllOthersFeatures;
        public bool ActiveAllOthersFeatures
        {
            get { return _activeAllOthersFeatures; }
            set
            {
                _ini.SetVal("GENERAL", "ActiveAllOthersFeatures", value);
                this.RaiseAndSetIfChanged(ref _activeAllOthersFeatures, value);
            }
        }
        #endregion

        #region [WINDOWED]
        private bool _activeWindowed;
        public bool ActiveWindowed
        {
            get { return _activeWindowed; }
            set
            {
                _ini.SetVal("WINDOWED", "ActiveWindowed", value);
                this.RaiseAndSetIfChanged(ref _activeWindowed, value);

                GameParam_WindowMode = value;
            }
        }

        private bool _removeBorder;
        public bool RemoveBorder
        {
            get { return _removeBorder; }
            set
            {
                _ini.SetVal("WINDOWED", "RemoveBorder", value);
                this.RaiseAndSetIfChanged(ref _removeBorder, value);
            }
        }

        private bool _windowOnTop;
        public bool WindowOnTop
        {
            get { return _windowOnTop; }
            set
            {
                _ini.SetVal("WINDOWED", "WindowOnTop", value);
                this.RaiseAndSetIfChanged(ref _windowOnTop, value);
            }
        }

        private bool _maximized;
        public bool Maximized
        {
            get { return _maximized; }
            set
            {
                _ini.SetVal("WINDOWED", "Maximized", value);
                this.RaiseAndSetIfChanged(ref _maximized, value);
            }
        }

        private bool _setWindowPos;
        public bool SetWindowPos
        {
            get { return _setWindowPos; }
            set
            {
                _ini.SetVal("WINDOWED", "SetWindowPos", value);
                this.RaiseAndSetIfChanged(ref _setWindowPos, value);
            }
        }

        private int _windowedX;
        public int WindowedX
        {
            get { return _windowedX; }
            set
            {
                _ini.SetVal("WINDOWED", "X", value);
                this.RaiseAndSetIfChanged(ref _windowedX, value);
            }
        }

        private int _windowedY;
        public int WindowedY
        {
            get { return _windowedY; }
            set
            {
                _ini.SetVal("WINDOWED", "Y", value);
                this.RaiseAndSetIfChanged(ref _windowedY, value);
            }
        }

        private int _windowedWidth;
        public int WindowedWidth
        {
            get { return _windowedWidth; }
            set
            {
                _ini.SetVal("WINDOWED", "Width", value);
                this.RaiseAndSetIfChanged(ref _windowedWidth, value);
            }
        }

        private int _windowedHeight;
        public int WindowedHeight
        {
            get { return _windowedHeight; }
            set
            {
                _ini.SetVal("WINDOWED", "Height", value);
                this.RaiseAndSetIfChanged(ref _windowedHeight, value);
            }
        }

        private bool _lockMouseOnStartup;
        public bool LockMouseOnStartup
        {
            get { return _lockMouseOnStartup; }
            set
            {
                _ini.SetVal("WINDOWED", "LockMouseOnStartup", value);
                this.RaiseAndSetIfChanged(ref _lockMouseOnStartup, value);
            }
        }
        #endregion

        #region [LANGUAGE]
        private bool _activeChangeLanguage;
        public bool ActiveChangeLanguage
        {
            get { return _activeChangeLanguage; }
            set
            {
                if (value) ShowMsgBaseModConflict("Language", "Enabled", "ActiveChangeLanguage");

                _ini.SetVal("LANGUAGE", "ActiveChangeLanguage", value);
                this.RaiseAndSetIfChanged(ref _activeChangeLanguage, value);
            }
        }

        private readonly string[] _languageListWrite = new string[] { "ENG", "ESP", "DEU", "FRA", "POR", "ITA", "JPN", "KOR", "SIN", "CHI", "POL", "RUS" };

        private int _selectedLanguage;
        public int SelectedLanguage
        {
            get { return _selectedLanguage; }
            set
            {
                var result = _languageListWrite[value];
                _ini.SetVal("LANGUAGE", "SelectedLanguage", result);

                this.RaiseAndSetIfChanged(ref _selectedLanguage, value);
            }
        }

        private bool _activeLanguageManagement;
        public bool ActiveLanguageManagement
        {
            get { return _activeLanguageManagement; }
            set
            {
                _ini.SetVal("LANGUAGE", "ActiveLanguageManagement", value);
                this.RaiseAndSetIfChanged(ref _activeLanguageManagement, value);
            }
        }

        private int _defaultLanguage;
        public int DefaultLanguage
        {
            get { return _defaultLanguage; }
            set
            {
                if (value > -1)
                {
                    var result = _languageListWrite[value];
                    _ini.SetVal("LANGUAGE", "DefaultLanguage", result);
                }

                this.RaiseAndSetIfChanged(ref _defaultLanguage, value);
            }
        }

        private bool _avlblLngs_ENG;
        public bool AvlblLngs_ENG
        {
            get { return _avlblLngs_ENG; }
            set
            {
                if (value)
                    _ini.SetAvailableLanguages("ENG");
                else _ini.DelAvailableLanguages("ENG");

                this.RaiseAndSetIfChanged(ref _avlblLngs_ENG, value);
            }
        }

        private bool _avlblLngs_ESP;
        public bool AvlblLngs_ESP
        {
            get { return _avlblLngs_ESP; }
            set
            {
                if (value)
                    _ini.SetAvailableLanguages("ESP");
                else _ini.DelAvailableLanguages("ESP");

                this.RaiseAndSetIfChanged(ref _avlblLngs_ESP, value);
            }
        }

        private bool _avlblLngs_DEU;
        public bool AvlblLngs_DEU
        {
            get { return _avlblLngs_DEU; }
            set
            {
                if (value)
                    _ini.SetAvailableLanguages("DEU");
                else _ini.DelAvailableLanguages("DEU");

                this.RaiseAndSetIfChanged(ref _avlblLngs_DEU, value);
            }
        }

        private bool _avlblLngs_FRA;
        public bool AvlblLngs_FRA
        {
            get { return _avlblLngs_FRA; }
            set
            {
                if (value)
                    _ini.SetAvailableLanguages("FRA");
                else _ini.DelAvailableLanguages("FRA");

                this.RaiseAndSetIfChanged(ref _avlblLngs_FRA, value);
            }
        }

        private bool _avlblLngs_POR;
        public bool AvlblLngs_POR
        {
            get { return _avlblLngs_POR; }
            set
            {
                if (value)
                    _ini.SetAvailableLanguages("POR");
                else _ini.DelAvailableLanguages("POR");

                this.RaiseAndSetIfChanged(ref _avlblLngs_POR, value);
            }
        }

        private bool _avlblLngs_ITA;
        public bool AvlblLngs_ITA
        {
            get { return _avlblLngs_ITA; }
            set
            {
                if (value)
                    _ini.SetAvailableLanguages("ITA");
                else _ini.DelAvailableLanguages("ITA");

                this.RaiseAndSetIfChanged(ref _avlblLngs_ITA, value);
            }
        }

        private bool _avlblLngs_JPN;
        public bool AvlblLngs_JPN
        {
            get { return _avlblLngs_JPN; }
            set
            {
                if (value)
                    _ini.SetAvailableLanguages("JPN");
                else _ini.DelAvailableLanguages("JPN");

                this.RaiseAndSetIfChanged(ref _avlblLngs_JPN, value);
            }
        }

        private bool _avlblLngs_KOR;
        public bool AvlblLngs_KOR
        {
            get { return _avlblLngs_KOR; }
            set
            {
                if (value)
                    _ini.SetAvailableLanguages("KOR");
                else _ini.DelAvailableLanguages("KOR");

                this.RaiseAndSetIfChanged(ref _avlblLngs_KOR, value);
            }
        }

        private bool _avlblLngs_SIN;
        public bool AvlblLngs_SIN
        {
            get { return _avlblLngs_SIN; }
            set
            {
                if (value)
                    _ini.SetAvailableLanguages("SIN");
                else _ini.DelAvailableLanguages("SIN");

                this.RaiseAndSetIfChanged(ref _avlblLngs_SIN, value);
            }
        }

        private bool _avlblLngs_CHI;
        public bool AvlblLngs_CHI
        {
            get { return _avlblLngs_CHI; }
            set
            {
                if (value)
                    _ini.SetAvailableLanguages("CHI");
                else _ini.DelAvailableLanguages("CHI");

                this.RaiseAndSetIfChanged(ref _avlblLngs_CHI, value);
            }
        }

        private bool _avlblLngs_POL;
        public bool AvlblLngs_POL
        {
            get { return _avlblLngs_POL; }
            set
            {
                if (value)
                    _ini.SetAvailableLanguages("POL");
                else _ini.DelAvailableLanguages("POL");

                this.RaiseAndSetIfChanged(ref _avlblLngs_POL, value);
            }
        }

        private bool _avlblLngs_RUS;
        public bool AvlblLngs_RUS
        {
            get { return _avlblLngs_RUS; }
            set
            {
                if (value)
                    _ini.SetAvailableLanguages("RUS");
                else _ini.DelAvailableLanguages("RUS");

                this.RaiseAndSetIfChanged(ref _avlblLngs_RUS, value);
            }
        }
        #endregion

        #region [SAVEPATH]
        private bool _activeSavePathChange;
        public bool ActiveSavePathChange
        {
            get { return _activeSavePathChange; }
            set
            {
                _ini.SetVal("SAVEPATH", "ActiveSavePathChange", value);
                this.RaiseAndSetIfChanged(ref _activeSavePathChange, value);
            }
        }

        private string _savePath;
        public string SavePath
        {
            get { return _savePath; }
            set
            {
                _ini.SetVal("SAVEPATH", "SavePath", value);
                this.RaiseAndSetIfChanged(ref _savePath, value);
            }
        }

        private bool _activeAutoBackup;
        public bool ActiveAutoBackup
        {
            get { return _activeAutoBackup; }
            set
            {
                _ini.SetVal("SAVEPATH", "ActiveAutoBackup", value);
                this.RaiseAndSetIfChanged(ref _activeAutoBackup, value);
            }
        }

        private int _maxBackupPerCharacter;
        public int MaxBackupPerCharacter
        {
            get { return _maxBackupPerCharacter; }
            set
            {
                _ini.SetVal("SAVEPATH", "MaxBackupPerCharacter", value);
                this.RaiseAndSetIfChanged(ref _maxBackupPerCharacter, value);
            }
        }
        #endregion

        private int ColorFromNum(int color)
        {
            switch (color)
            {
                case 13:
                    return 15;
                case 14:
                    return 16;
                case 15:
                    return 18;
                case 16:
                    return 19;
                default:
                    return color;
            }
        }

        private int ColorToNum(int color)
        {
            switch (color)
            {
                case 15:
                    return 13;
                case 16:
                    return 14;
                case 18:
                    return 15;
                case 19:
                    return 16;
                default:
                    return color;
            }
        }

        #region [MAIN_SCREEN]
        private bool _activeVersionTextChange;
        public bool ActiveVersionTextChange
        {
            get { return _activeVersionTextChange; }
            set
            {
                _ini.SetVal("MAIN SCREEN", "ActiveVersionTextChange", value);
                this.RaiseAndSetIfChanged(ref _activeVersionTextChange, value);
            }
        }

        private string _versionText;
        public string VersionText
        {
            get { return _versionText; }
            set
            {
                _ini.SetVal("MAIN SCREEN", "VersionText", value);
                this.RaiseAndSetIfChanged(ref _versionText, value);
            }
        }

        private int _�olorOfVersionText;
        public int ColorOfVersionText
        {
            get { return _�olorOfVersionText; }
            set
            {
                if (((value == 0) || (value == 15) || (value == 16) || (value == 18))
                    && (App.Fluent.Mode == FluentThemeMode.Light))

                    ColorOfVersionText_Background = new SolidColorBrush(Colors.Gray);
                else 
                    ColorOfVersionText_Background = new SolidColorBrush(Colors.Transparent);


                _ini.SetVal("MAIN SCREEN", "ColorOfVersionText", value); //_ini.SetVal("MAIN SCREEN", "ColorOfVersionText", ColorFromNum(value));
                this.RaiseAndSetIfChanged(ref _�olorOfVersionText, value);
            }
        }

        private SolidColorBrush _�olorOfVersionText_Background;
        public SolidColorBrush ColorOfVersionText_Background
        {
            get { return _�olorOfVersionText_Background; }
            set { this.RaiseAndSetIfChanged(ref _�olorOfVersionText_Background, value); }
        }

        private bool _activePrintPlugYVersion;
        public bool ActivePrintPlugYVersion
        {
            get { return _activePrintPlugYVersion; }
            set
            {
                _ini.SetVal("MAIN SCREEN", "ActivePrintPlugYVersion", value);
                this.RaiseAndSetIfChanged(ref _activePrintPlugYVersion, value);
            }
        }

        private int _colorOfPlugYVersion;
        public int ColorOfPlugYVersion
        {
            get { return _colorOfPlugYVersion; }
            set
            {
                if (((value == 0) || (value == 15) || (value == 16) || (value == 18))
                    && (App.Fluent.Mode == FluentThemeMode.Light))

                    ColorOfPlugYVersion_Background = new SolidColorBrush(Colors.Gray);
                else
                    ColorOfPlugYVersion_Background = new SolidColorBrush(Colors.Transparent);

                _ini.SetVal("MAIN SCREEN", "ColorOfPlugYVersion", value);
                this.RaiseAndSetIfChanged(ref _colorOfPlugYVersion, value);
            }
        }

        private SolidColorBrush _�olorOfPlugYVersion_Background;
        public SolidColorBrush ColorOfPlugYVersion_Background
        {
            get { return _�olorOfPlugYVersion_Background; }
            set { this.RaiseAndSetIfChanged(ref _�olorOfPlugYVersion_Background, value); }
        }
        #endregion

        #region [STASH]
        private bool _activeBigStash;
        public bool ActiveBigStash
        {
            get { return _activeBigStash; }
            set
            {
                _ini.SetVal("STASH", "ActiveBigStash", value);
                this.RaiseAndSetIfChanged(ref _activeBigStash, value);
            }
        }

        private bool _activeMultiPageStash;
        public bool ActiveMultiPageStash
        {
            get { return _activeMultiPageStash; }
            set
            {
                _ini.SetVal("STASH", "ActiveMultiPageStash", value);
                this.RaiseAndSetIfChanged(ref _activeMultiPageStash, value);
            }
        }

        private int _nbPagesPerIndex;
        public int NbPagesPerIndex
        {
            get { return _nbPagesPerIndex; }
            set
            {
                _ini.SetVal("STASH", "NbPagesPerIndex", value);
                this.RaiseAndSetIfChanged(ref _nbPagesPerIndex, value);
            }
        }

        private int _nbPagesPerIndex2;
        public int NbPagesPerIndex2
        {
            get { return _nbPagesPerIndex2; }
            set
            {
                _ini.SetVal("STASH", "NbPagesPerIndex2", value);
                this.RaiseAndSetIfChanged(ref _nbPagesPerIndex2, value);
            }
        }

        private int _maxPersonnalPages;
        public int MaxPersonnalPages
        {
            get { return _maxPersonnalPages; }
            set
            {
                _ini.SetVal("STASH", "MaxPersonnalPages", value);
                this.RaiseAndSetIfChanged(ref _maxPersonnalPages, value);
            }
        }

        private bool _autoRenameStashPage;
        public bool AutoRenameStashPage
        {
            get { return _autoRenameStashPage; }
            set
            {
                _ini.SetVal("STASH", "AutoRenameStashPage", value);
                this.RaiseAndSetIfChanged(ref _autoRenameStashPage, value);
            }
        }

        private int _personalNormalPageColor;
        public int PersonalNormalPageColor
        {
            get { return _personalNormalPageColor; }
            set
            {
                if (((value == 0) || (value == 15) || (value == 16) || (value == 18))
                    && (App.Fluent.Mode == FluentThemeMode.Light))

                    PersonalNormalPageColor_Background = new SolidColorBrush(Colors.Gray);
                else
                    PersonalNormalPageColor_Background = new SolidColorBrush(Colors.Transparent);


                _ini.SetVal("STASH", "PersonalNormalPageColor", value);
                this.RaiseAndSetIfChanged(ref _personalNormalPageColor, value);
            }
        }

        private SolidColorBrush _personalNormalPageColor_Background;
        public SolidColorBrush PersonalNormalPageColor_Background
        {
            get { return _personalNormalPageColor_Background; }
            set { this.RaiseAndSetIfChanged(ref _personalNormalPageColor_Background, value); }
        }

        private int _personalIndexPageColor;
        public int PersonalIndexPageColor
        {
            get { return _personalIndexPageColor; }
            set
            {
                if (((value == 0) || (value == 15) || (value == 16) || (value == 18))
                    && (App.Fluent.Mode == FluentThemeMode.Light))

                    PersonalIndexPageColor_Background = new SolidColorBrush(Colors.Gray);
                else
                    PersonalIndexPageColor_Background = new SolidColorBrush(Colors.Transparent);


                _ini.SetVal("STASH", "PersonalIndexPageColor", value);
                this.RaiseAndSetIfChanged(ref _personalIndexPageColor, value);
            }
        }

        private SolidColorBrush _personalIndexPageColor_Background;
        public SolidColorBrush PersonalIndexPageColor_Background
        {
            get { return _personalIndexPageColor_Background; }
            set { this.RaiseAndSetIfChanged(ref _personalIndexPageColor_Background, value); }
        }

        private int _personalMainIndexPageColor;
        public int PersonalMainIndexPageColor
        {
            get { return _personalMainIndexPageColor; }
            set
            {
                if (((value == 0) || (value == 15) || (value == 16) || (value == 18))
                    && (App.Fluent.Mode == FluentThemeMode.Light))

                    PersonalMainIndexPageColor_Background = new SolidColorBrush(Colors.Gray);
                else
                    PersonalMainIndexPageColor_Background = new SolidColorBrush(Colors.Transparent);


                _ini.SetVal("STASH", "PersonalMainIndexPageColor", value);
                this.RaiseAndSetIfChanged(ref _personalMainIndexPageColor, value);
            }
        }

        private SolidColorBrush _personalMainIndexPageColor_Background;
        public SolidColorBrush PersonalMainIndexPageColor_Background
        {
            get { return _personalMainIndexPageColor_Background; }
            set { this.RaiseAndSetIfChanged(ref _personalMainIndexPageColor_Background, value); }
        }

        private int _sharedNormalPageColor;
        public int SharedNormalPageColor
        {
            get { return _sharedNormalPageColor; }
            set
            {
                if (((value == 0) || (value == 15) || (value == 16) || (value == 18))
                    && (App.Fluent.Mode == FluentThemeMode.Light))

                    SharedNormalPageColor_Background = new SolidColorBrush(Colors.Gray);
                else
                    SharedNormalPageColor_Background = new SolidColorBrush(Colors.Transparent);


                _ini.SetVal("STASH", "SharedNormalPageColor", value);
                this.RaiseAndSetIfChanged(ref _sharedNormalPageColor, value);
            }
        }

        private SolidColorBrush _sharedNormalPageColor_Background;
        public SolidColorBrush SharedNormalPageColor_Background
        {
            get { return _sharedNormalPageColor_Background; }
            set { this.RaiseAndSetIfChanged(ref _sharedNormalPageColor_Background, value); }
        }

        private int _sharedIndexPageColor;
        public int SharedIndexPageColor
        {
            get { return _sharedIndexPageColor; }
            set
            {
                if (((value == 0) || (value == 15) || (value == 16) || (value == 18))
                    && (App.Fluent.Mode == FluentThemeMode.Light))

                    SharedIndexPageColor_Background = new SolidColorBrush(Colors.Gray);
                else
                    SharedIndexPageColor_Background = new SolidColorBrush(Colors.Transparent);


                _ini.SetVal("STASH", "SharedIndexPageColor", value);
                this.RaiseAndSetIfChanged(ref _sharedIndexPageColor, value);
            }
        }

        private SolidColorBrush _sharedIndexPageColor_Background;
        public SolidColorBrush SharedIndexPageColor_Background
        {
            get { return _sharedIndexPageColor_Background; }
            set { this.RaiseAndSetIfChanged(ref _sharedIndexPageColor_Background, value); }
        }

        private int _sharedMainIndexPageColor;
        public int SharedMainIndexPageColor
        {
            get { return _sharedMainIndexPageColor; }
            set
            {
                if (((value == 0) || (value == 15) || (value == 16) || (value == 18))
                    && (App.Fluent.Mode == FluentThemeMode.Light))

                    SharedMainIndexPageColor_Background = new SolidColorBrush(Colors.Gray);
                else
                    SharedMainIndexPageColor_Background = new SolidColorBrush(Colors.Transparent);


                _ini.SetVal("STASH", "SharedMainIndexPageColor", value);
                this.RaiseAndSetIfChanged(ref _sharedMainIndexPageColor, value);
            }
        }

        private SolidColorBrush _sharedMainIndexPageColor_Background;
        public SolidColorBrush SharedMainIndexPageColor_Background
        {
            get { return _sharedMainIndexPageColor_Background; }
            set { this.RaiseAndSetIfChanged(ref _sharedMainIndexPageColor_Background, value); }
        }

        private bool _activeSharedStash;
        public bool ActiveSharedStash
        {
            get { return _activeSharedStash; }
            set
            {
                _ini.SetVal("STASH", "ActiveSharedStash", value);
                this.RaiseAndSetIfChanged(ref _activeSharedStash, value);
            }
        }

        private bool _activeSharedStashInMultiPlayer;
        public bool ActiveSharedStashInMultiPlayer
        {
            get { return _activeSharedStashInMultiPlayer; }
            set
            {
                _ini.SetVal("STASH", "ActiveSharedStashInMultiPlayer", value);
                this.RaiseAndSetIfChanged(ref _activeSharedStashInMultiPlayer, value);
            }
        }

        private bool _separateHardcoreStash;
        public bool SeparateHardcoreStash
        {
            get { return _separateHardcoreStash; }
            set
            {
                _ini.SetVal("STASH", "SeparateHardcoreStash", value);
                this.RaiseAndSetIfChanged(ref _separateHardcoreStash, value);
            }
        }

        private string _sharedStashFilename;
        public string SharedStashFilename
        {
            get { return _sharedStashFilename; }
            set
            {
                _ini.SetVal("STASH", "SharedStashFilename", value);
                this.RaiseAndSetIfChanged(ref _sharedStashFilename, value);
            }
        }


        private bool _displaySharedSetItemNameInGreen;
        public bool DisplaySharedSetItemNameInGreen
        {
            get { return _displaySharedSetItemNameInGreen; }
            set
            {
                _ini.SetVal("STASH", "DisplaySharedSetItemNameInGreen", value);
                this.RaiseAndSetIfChanged(ref _displaySharedSetItemNameInGreen, value);
            }
        }

        private int _maxSharedPages;
        public int MaxSharedPages
        {
            get { return _maxSharedPages; }
            set
            {
                _ini.SetVal("STASH", "MaxSharedPages", value);
                this.RaiseAndSetIfChanged(ref _maxSharedPages, value);
            }
        }

        private bool _activeSharedGold;
        public bool ActiveSharedGold
        {
            get { return _activeSharedGold; }
            set
            {
                _ini.SetVal("STASH", "ActiveSharedGold", value);
                this.RaiseAndSetIfChanged(ref _activeSharedGold, value);
            }
        }

        private bool _openSharedStashOnLoading;
        public bool OpenSharedStashOnLoading
        {
            get { return _openSharedStashOnLoading; }
            set
            {
                _ini.SetVal("STASH", "OpenSharedStashOnLoading", value);
                this.RaiseAndSetIfChanged(ref _openSharedStashOnLoading, value);
            }
        }

        private int _posXPreviousBtn;
        public int PosXPreviousBtn
        {
            get { return _posXPreviousBtn; }
            set
            {
                _ini.SetVal("STASH", "PosXPreviousBtn", value);
                this.RaiseAndSetIfChanged(ref _posXPreviousBtn, value);
            }
        }

        private int _posYPreviousBtn;
        public int PosYPreviousBtn
        {
            get { return _posYPreviousBtn; }
            set
            {
                _ini.SetVal("STASH", "PosYPreviousBtn", value);
                this.RaiseAndSetIfChanged(ref _posYPreviousBtn, value);
            }
        }

        private int _posWPreviousBtn;
        public int PosWPreviousBtn
        {
            get { return _posWPreviousBtn; }
            set
            {
                _ini.SetVal("STASH", "PosWPreviousBtn", value);
                this.RaiseAndSetIfChanged(ref _posWPreviousBtn, value);
            }
        }

        private int _posHPreviousBtn;
        public int PosHPreviousBtn
        {
            get { return _posHPreviousBtn; }
            set
            {
                _ini.SetVal("STASH", "PosHPreviousBtn", value);
                this.RaiseAndSetIfChanged(ref _posHPreviousBtn, value);
            }
        }

        private int _posXNextBtn;
        public int PosXNextBtn
        {
            get { return _posXNextBtn; }
            set
            {
                _ini.SetVal("STASH", "PosXNextBtn", value);
                this.RaiseAndSetIfChanged(ref _posXNextBtn, value);
            }
        }

        private int _posYNextBtn;
        public int PosYNextBtn
        {
            get { return _posYNextBtn; }
            set
            {
                _ini.SetVal("STASH", "PosYNextBtn", value);
                this.RaiseAndSetIfChanged(ref _posYNextBtn, value);
            }
        }

        private int _posWNextBtn;
        public int PosWNextBtn
        {
            get { return _posWNextBtn; }
            set
            {
                _ini.SetVal("STASH", "PosWNextBtn", value);
                this.RaiseAndSetIfChanged(ref _posWNextBtn, value);
            }
        }

        private int _posHNextBtn;
        public int PosHNextBtn
        {
            get { return _posHNextBtn; }
            set
            {
                _ini.SetVal("STASH", "PosHNextBtn", value);
                this.RaiseAndSetIfChanged(ref _posHNextBtn, value);
            }
        }

        private int _posXSharedBtn;
        public int PosXSharedBtn
        {
            get { return _posXSharedBtn; }
            set
            {
                _ini.SetVal("STASH", "PosXSharedBtn", value);
                this.RaiseAndSetIfChanged(ref _posXSharedBtn, value);
            }
        }

        private int _posYSharedBtn;
        public int PosYSharedBtn
        {
            get { return _posYSharedBtn; }
            set
            {
                _ini.SetVal("STASH", "PosYSharedBtn", value);
                this.RaiseAndSetIfChanged(ref _posYSharedBtn, value);
            }
        }

        private int _posWSharedBtn;
        public int PosWSharedBtn
        {
            get { return _posWSharedBtn; }
            set
            {
                _ini.SetVal("STASH", "PosWSharedBtn", value);
                this.RaiseAndSetIfChanged(ref _posWSharedBtn, value);
            }
        }

        private int _posHSharedBtn;
        public int PosHSharedBtn
        {
            get { return _posHSharedBtn; }
            set
            {
                _ini.SetVal("STASH", "PosHSharedBtn", value);
                this.RaiseAndSetIfChanged(ref _posHSharedBtn, value);
            }
        }

        private int _posXPreviousIndexBtn;
        public int PosXPreviousIndexBtn
        {
            get { return _posXPreviousIndexBtn; }
            set
            {
                _ini.SetVal("STASH", "PosXPreviousIndexBtn", value);
                this.RaiseAndSetIfChanged(ref _posXPreviousIndexBtn, value);
            }
        }

        private int _posYPreviousIndexBtn;
        public int PosYPreviousIndexBtn
        {
            get { return _posYPreviousIndexBtn; }
            set
            {
                _ini.SetVal("STASH", "PosYPreviousIndexBtn", value);
                this.RaiseAndSetIfChanged(ref _posYPreviousIndexBtn, value);
            }
        }

        private int _posWPreviousIndexBtn;
        public int PosWPreviousIndexBtn
        {
            get { return _posWPreviousIndexBtn; }
            set
            {
                _ini.SetVal("STASH", "PosWPreviousIndexBtn", value);
                this.RaiseAndSetIfChanged(ref _posWPreviousIndexBtn, value);
            }
        }

        private int _posHPreviousIndexBtn;
        public int PosHPreviousIndexBtn
        {
            get { return _posHPreviousIndexBtn; }
            set
            {
                _ini.SetVal("STASH", "PosHPreviousIndexBtn", value);
                this.RaiseAndSetIfChanged(ref _posHPreviousIndexBtn, value);
            }
        }

        private int _posXNextIndexBtn;
        public int PosXNextIndexBtn
        {
            get { return _posXNextIndexBtn; }
            set
            {
                _ini.SetVal("STASH", "PosXNextIndexBtn", value);
                this.RaiseAndSetIfChanged(ref _posXNextIndexBtn, value);
            }
        }

        private int _posYNextIndexBtn;
        public int PosYNextIndexBtn
        {
            get { return _posYNextIndexBtn; }
            set
            {
                _ini.SetVal("STASH", "PosYNextIndexBtn", value);
                this.RaiseAndSetIfChanged(ref _posYNextIndexBtn, value);
            }
        }

        private int _posWNextIndexBtn;
        public int PosWNextIndexBtn
        {
            get { return _posWNextIndexBtn; }
            set
            {
                _ini.SetVal("STASH", "PosWNextIndexBtn", value);
                this.RaiseAndSetIfChanged(ref _posWNextIndexBtn, value);
            }
        }

        private int _posHNextIndexBtn;
        public int PosHNextIndexBtn
        {
            get { return _posHNextIndexBtn; }
            set
            {
                _ini.SetVal("STASH", "PosHNextIndexBtn", value);
                this.RaiseAndSetIfChanged(ref _posHNextIndexBtn, value);
            }
        }

        private int _posXPutGoldBtn;
        public int PosXPutGoldBtn
        {
            get { return _posXPutGoldBtn; }
            set
            {
                _ini.SetVal("STASH", "PosXPutGoldBtn", value);
                this.RaiseAndSetIfChanged(ref _posXPutGoldBtn, value);
            }
        }

        private int _posYPutGoldBtn;
        public int PosYPutGoldBtn
        {
            get { return _posYPutGoldBtn; }
            set
            {
                _ini.SetVal("STASH", "PosYPutGoldBtn", value);
                this.RaiseAndSetIfChanged(ref _posYPutGoldBtn, value);
            }
        }

        private int _posWPutGoldBtn;
        public int PosWPutGoldBtn
        {
            get { return _posWPutGoldBtn; }
            set
            {
                _ini.SetVal("STASH", "PosWPutGoldBtn", value);
                this.RaiseAndSetIfChanged(ref _posWPutGoldBtn, value);
            }
        }

        private int _posHPutGoldBtn;
        public int PosHPutGoldBtn
        {
            get { return _posHPutGoldBtn; }
            set
            {
                _ini.SetVal("STASH", "PosHPutGoldBtn", value);
                this.RaiseAndSetIfChanged(ref _posHPutGoldBtn, value);
            }
        }

        private int _posXTakeGoldBtn;
        public int PosXTakeGoldBtn
        {
            get { return _posXTakeGoldBtn; }
            set
            {
                _ini.SetVal("STASH", "PosXTakeGoldBtn", value);
                this.RaiseAndSetIfChanged(ref _posXTakeGoldBtn, value);
            }
        }

        private int _posYTakeGoldBtn;
        public int PosYTakeGoldBtn
        {
            get { return _posYTakeGoldBtn; }
            set
            {
                _ini.SetVal("STASH", "PosYTakeGoldBtn", value);
                this.RaiseAndSetIfChanged(ref _posYTakeGoldBtn, value);
            }
        }

        private int _posWTakeGoldBtn;
        public int PosWTakeGoldBtn
        {
            get { return _posWTakeGoldBtn; }
            set
            {
                _ini.SetVal("STASH", "PosWTakeGoldBtn", value);
                this.RaiseAndSetIfChanged(ref _posWTakeGoldBtn, value);
            }
        }

        private int _posHTakeGoldBtn;
        public int PosHTakeGoldBtn
        {
            get { return _posHTakeGoldBtn; }
            set
            {
                _ini.SetVal("STASH", "PosHTakeGoldBtn", value);
                this.RaiseAndSetIfChanged(ref _posHTakeGoldBtn, value);
            }
        }

        private int _posXStashNameField;
        public int PosXStashNameField
        {
            get { return _posXStashNameField; }
            set
            {
                _ini.SetVal("STASH", "PosXStashNameField", value);
                this.RaiseAndSetIfChanged(ref _posXStashNameField, value);
            }
        }

        private int _posYStashNameField;
        public int PosYStashNameField
        {
            get { return _posYStashNameField; }
            set
            {
                _ini.SetVal("STASH", "PosYStashNameField", value);
                this.RaiseAndSetIfChanged(ref _posYStashNameField, value);
            }
        }

        private int _posWStashNameField;
        public int PosWStashNameField
        {
            get { return _posWStashNameField; }
            set
            {
                _ini.SetVal("STASH", "PosWStashNameField", value);
                this.RaiseAndSetIfChanged(ref _posWStashNameField, value);
            }
        }

        private int _posHStashNameField;
        public int PosHStashNameField
        {
            get { return _posHStashNameField; }
            set
            {
                _ini.SetVal("STASH", "PosHStashNameField", value);
                this.RaiseAndSetIfChanged(ref _posHStashNameField, value);
            }
        }

        private int _posXStashGoldField;
        public int PosXStashGoldField
        {
            get { return _posXStashGoldField; }
            set
            {
                _ini.SetVal("STASH", "PosXStashGoldField", value);
                this.RaiseAndSetIfChanged(ref _posXStashGoldField, value);
            }
        }

        private int _posYStashGoldField;
        public int PosYStashGoldField
        {
            get { return _posYStashGoldField; }
            set
            {
                _ini.SetVal("STASH", "PosYStashGoldField", value);
                this.RaiseAndSetIfChanged(ref _posYStashGoldField, value);
            }
        }

        private int _posWStashGoldField;
        public int PosWStashGoldField
        {
            get { return _posWStashGoldField; }
            set
            {
                _ini.SetVal("STASH", "PosWStashGoldField", value);
                this.RaiseAndSetIfChanged(ref _posWStashGoldField, value);
            }
        }

        private int _posHStashGoldField;
        public int PosHStashGoldField
        {
            get { return _posHStashGoldField; }
            set
            {
                _ini.SetVal("STASH", "PosHStashGoldField", value);
                this.RaiseAndSetIfChanged(ref _posHStashGoldField, value);
            }
        }
        #endregion

        #region [STATS POINTS]
        private bool _activeStatsUnassignment;
        public bool ActiveStatsUnassignment
        {
            get { return _activeStatsUnassignment; }
            set
            {
                _ini.SetVal("STATS POINTS", "ActiveStatsUnassignment", value);
                this.RaiseAndSetIfChanged(ref _activeStatsUnassignment, value);
            }
        }

        private int _keyUsed;
        public int KeyUsed
        {
            get { return _keyUsed; }
            set
            {
                int result = 18;
                if (value == 0) result = 17;
                else if (value == 1) result = 18;

                if (value == 17) value = 0;
                else if (value == 18) value = 1;

                _ini.SetVal("STATS POINTS", "KeyUsed", result);
                this.RaiseAndSetIfChanged(ref _keyUsed, value);
            }
        }

        private bool _activeShiftClickLimit;
        public bool ActiveShiftClickLimit
        {
            get { return _activeShiftClickLimit; }
            set
            {
                _ini.SetVal("STATS POINTS", "ActiveShiftClickLimit", value);
                this.RaiseAndSetIfChanged(ref _activeShiftClickLimit, value);
            }
        }

        private int _limitValueToShiftClick;
        public int LimitValueToShiftClick
        {
            get { return _limitValueToShiftClick; }
            set
            {
                _ini.SetVal("STATS POINTS", "LimitValueToShiftClick", value);
                this.RaiseAndSetIfChanged(ref _limitValueToShiftClick, value);
            }
        }
        #endregion

        #region [STAT ON LEVEL UP]
        private bool _activeStatPerLevelUp;
        public bool ActiveStatPerLevelUp
        {
            get { return _activeStatPerLevelUp; }
            set
            {
                _ini.SetVal("STAT ON LEVEL UP", "ActiveStatPerLevelUp", value);
                this.RaiseAndSetIfChanged(ref _activeStatPerLevelUp, value);
            }
        }

        private int _statPerLevelUp;
        public int StatPerLevelUp
        {
            get { return _statPerLevelUp; }
            set
            {
                _ini.SetVal("STAT ON LEVEL UP", "StatPerLevelUp", value);
                this.RaiseAndSetIfChanged(ref _statPerLevelUp, value);
            }
        }
        #endregion

        #region [SKILLS POINTS]

        private bool _activeSkillsUnassignment;
        public bool ActiveSkillsUnassignment
        {
            get { return _activeSkillsUnassignment; }
            set
            {
                _ini.SetVal("SKILLS POINTS", "ActiveSkillsUnassignment", value);
                this.RaiseAndSetIfChanged(ref _activeSkillsUnassignment, value);
            }
        }

        private bool _activeSkillsUnassignmentOneForOne;
        public bool ActiveSkillsUnassignmentOneForOne
        {
            get { return _activeSkillsUnassignmentOneForOne; }
            set
            {
                _ini.SetVal("SKILLS POINTS", "ActiveSkillsUnassignmentOneByOne", value);
                this.RaiseAndSetIfChanged(ref _activeSkillsUnassignmentOneForOne, value);
            }
        }

        private int _posXUnassignSkillBtn;
        public int PosXUnassignSkillBtn
        {
            get { return _posXUnassignSkillBtn; }
            set
            {
                _ini.SetVal("SKILLS POINTS", "PosXUnassignSkillBtn", value);
                this.RaiseAndSetIfChanged(ref _posXUnassignSkillBtn, value);
            }
        }

        private int _posYUnassignSkillBtn;
        public int PosYUnassignSkillBtn
        {
            get { return _posYUnassignSkillBtn; }
            set
            {
                _ini.SetVal("SKILLS POINTS", "PosYUnassignSkillBtn", value);
                this.RaiseAndSetIfChanged(ref _posYUnassignSkillBtn, value);
            }
        }
        #endregion

        #region [SKILL ON LEVEL UP]
        private bool _activeSkillPerLevelUp;
        public bool ActiveSkillPerLevelUp
        {
            get { return _activeSkillPerLevelUp; }
            set
            {
                _ini.SetVal("SKILL ON LEVEL UP", "ActiveSkillPerLevelUp", value);
                this.RaiseAndSetIfChanged(ref _activeSkillPerLevelUp, value);
            }
        }

        private int _skillPerLevelUp;
        public int SkillPerLevelUp
        {
            get { return _skillPerLevelUp; }
            set
            {
                _ini.SetVal("SKILL ON LEVEL UP", "SkillPerLevelUp", value);
                this.RaiseAndSetIfChanged(ref _skillPerLevelUp, value);
            }
        }
        #endregion

        #region [WORLD EVENT]
        private bool _activeWorldEvent;
        public bool ActiveWorldEvent
        {
            get { return _activeWorldEvent; }
            set
            {
                if (value) ShowMsgBaseModConflict("LadderRuneWords", "Enabled", "ActiveLadderRunewords");

                _ini.SetVal("WORLD EVENT", "ActiveWorldEvent", value);
                this.RaiseAndSetIfChanged(ref _activeWorldEvent, value);
            }
        }

        private bool _showCounterInAllDifficulty;
        public bool ShowCounterInAllDifficulty
        {
            get { return _showCounterInAllDifficulty; }
            set
            {
                _ini.SetVal("WORLD EVENT", "ShowCounterInAllDifficulty", value);
                this.RaiseAndSetIfChanged(ref _showCounterInAllDifficulty, value);
            }
        }

        private string _itemsToSell;
        public string ItemsToSell
        {
            get { return _itemsToSell; }
            set
            {
                _ini.SetVal("WORLD EVENT", "ItemsToSell", value);
                this.RaiseAndSetIfChanged(ref _itemsToSell, value);
            }
        }


        private int _monsterID;
        public int MonsterID
        {
            get { return _monsterID; }
            set
            {
                _ini.SetVal("WORLD EVENT", "MonsterID", value);
                this.RaiseAndSetIfChanged(ref _monsterID, value);
            }
        }

        private int _ownSOJSoldChargeFor;
        public int OwnSOJSoldChargeFor
        {
            get { return _ownSOJSoldChargeFor; }
            set
            {
                _ini.SetVal("WORLD EVENT", "OwnSOJSoldChargeFor", value);
                this.RaiseAndSetIfChanged(ref _ownSOJSoldChargeFor, value);
            }
        }

        private int _inititalSOJSoldMin;
        public int InititalSOJSoldMin
        {
            get { return _inititalSOJSoldMin; }
            set
            {
                _ini.SetVal("WORLD EVENT", "InititalSOJSoldMin", value);
                this.RaiseAndSetIfChanged(ref _inititalSOJSoldMin, value);
            }
        }

        private int _inititalSOJSoldMax;
        public int InititalSOJSoldMax
        {
            get { return _inititalSOJSoldMax; }
            set
            {
                _ini.SetVal("WORLD EVENT", "InititalSOJSoldMax", value);
                this.RaiseAndSetIfChanged(ref _inititalSOJSoldMax, value);
            }
        }

        private int _triggerAtEachSOJSoldMin;
        public int TriggerAtEachSOJSoldMin
        {
            get { return _triggerAtEachSOJSoldMin; }
            set
            {
                _ini.SetVal("WORLD EVENT", "TriggerAtEachSOJSoldMin", value);
                this.RaiseAndSetIfChanged(ref _triggerAtEachSOJSoldMin, value);
            }
        }

        private int _triggerAtEachSOJSoldMax;
        public int TriggerAtEachSOJSoldMax
        {
            get { return _triggerAtEachSOJSoldMax; }
            set
            {
                _ini.SetVal("WORLD EVENT", "TriggerAtEachSOJSoldMax", value);
                this.RaiseAndSetIfChanged(ref _triggerAtEachSOJSoldMax, value);
            }
        }

        private bool _activeAutoSell;
        public bool ActiveAutoSell
        {
            get { return _activeAutoSell; }
            set
            {
                _ini.SetVal("WORLD EVENT", "ActiveAutoSell", value);
                this.RaiseAndSetIfChanged(ref _activeAutoSell, value);
            }
        }

        private int _timeBeforeAutoSellMin;
        public int TimeBeforeAutoSellMin
        {
            get { return _timeBeforeAutoSellMin; }
            set
            {
                _ini.SetVal("WORLD EVENT", "TimeBeforeAutoSellMin", value);
                this.RaiseAndSetIfChanged(ref _timeBeforeAutoSellMin, value);
            }
        }

        private int _timeBeforeAutoSellMax;
        public int TimeBeforeAutoSellMax
        {
            get { return _timeBeforeAutoSellMax; }
            set
            {
                _ini.SetVal("WORLD EVENT", "TimeBeforeAutoSellMax", value);
                this.RaiseAndSetIfChanged(ref _timeBeforeAutoSellMax, value);
            }
        }
        #endregion

        #region [UBER QUEST]
        private bool _activeUberQuest;
        public bool ActiveUberQuest
        {
            get { return _activeUberQuest; }
            set
            {
                if (value) ShowMsgBaseModConflict("RedPortals", "Enabled", "ActiveUberQuest");

                _ini.SetVal("UBER QUEST", "ActiveUberQuest", value);
                this.RaiseAndSetIfChanged(ref _activeUberQuest, value);
            }
        }

        private int _uberMephistoX;
        public int UberMephistoX
        {
            get { return _uberMephistoX; }
            set
            {
                _ini.SetVal("UBER QUEST", "UberMephistoX", value);
                this.RaiseAndSetIfChanged(ref _uberMephistoX, value);
            }
        }

        private int _uberMephistoY;
        public int UberMephistoY
        {
            get { return _uberMephistoY; }
            set
            {
                _ini.SetVal("UBER QUEST", "UberMephistoY", value);
                this.RaiseAndSetIfChanged(ref _uberMephistoY, value);
            }
        }

        private int _uberDiabloX;
        public int UberDiabloX
        {
            get { return _uberDiabloX; }
            set
            {
                _ini.SetVal("UBER QUEST", "UberDiabloX", value);
                this.RaiseAndSetIfChanged(ref _uberDiabloX, value);
            }
        }

        private int _uberDiabloY;
        public int UberDiabloY
        {
            get { return _uberDiabloY; }
            set
            {
                _ini.SetVal("UBER QUEST", "UberDiabloY", value);
                this.RaiseAndSetIfChanged(ref _uberDiabloY, value);
            }
        }

        private int _uberBaalX;
        public int UberBaalX
        {
            get { return _uberBaalX; }
            set
            {
                _ini.SetVal("UBER QUEST", "UberBaalX", value);
                this.RaiseAndSetIfChanged(ref _uberBaalX, value);
            }
        }

        private int _uberBaalY;
        public int UberBaalY
        {
            get { return _uberBaalY; }
            set
            {
                _ini.SetVal("UBER QUEST", "UberBaalY", value);
                this.RaiseAndSetIfChanged(ref _uberBaalY, value);
            }
        }

        private bool _activeUberMinions;
        public bool ActiveUberMinions
        {
            get { return _activeUberMinions; }
            set
            {
                if (value) ShowMsgBaseModConflict("UberMinions", "Enabled", "ActiveUberMinions");

                _ini.SetVal("UBER QUEST", "ActiveUberMinions", value);
                this.RaiseAndSetIfChanged(ref _activeUberMinions, value);
            }
        }

        private int _uberMephistoSpawnPercent;
        public int UberMephistoSpawnPercent
        {
            get { return _uberMephistoSpawnPercent; }
            set
            {
                _ini.SetVal("UBER QUEST", "UberMephistoSpawnPercent", value);
                this.RaiseAndSetIfChanged(ref _uberMephistoSpawnPercent, value);
            }
        }

        private int _uberMephistoSpawnRadius;
        public int UberMephistoSpawnRadius
        {
            get { return _uberMephistoSpawnRadius; }
            set
            {
                _ini.SetVal("UBER QUEST", "UberMephistoSpawnRadius", value);
                this.RaiseAndSetIfChanged(ref _uberMephistoSpawnRadius, value);
            }
        }

        private int _uberBaalSpawnPercent;
        public int UberBaalSpawnPercent
        {
            get { return _uberBaalSpawnPercent; }
            set
            {
                _ini.SetVal("UBER QUEST", "UberBaalSpawnPercent", value);
                this.RaiseAndSetIfChanged(ref _uberBaalSpawnPercent, value);
            }
        }

        private int _uberBaalSpawnRadius;
        public int UberBaalSpawnRadius
        {
            get { return _uberBaalSpawnRadius; }
            set
            {
                _ini.SetVal("UBER QUEST", "UberBaalSpawnRadius", value);
                this.RaiseAndSetIfChanged(ref _uberBaalSpawnRadius, value);
            }
        }

        private int _uberDiabloSpawnPercent;
        public int UberDiabloSpawnPercent
        {
            get { return _uberDiabloSpawnPercent; }
            set
            {
                _ini.SetVal("UBER QUEST", "UberDiabloSpawnPercent", value);
                this.RaiseAndSetIfChanged(ref _uberDiabloSpawnPercent, value);
            }
        }

        private int _uberDiabloSpawnRadius;
        public int UberDiabloSpawnRadius
        {
            get { return _uberDiabloSpawnRadius; }
            set
            {
                _ini.SetVal("UBER QUEST", "UberDiabloSpawnRadius", value);
                this.RaiseAndSetIfChanged(ref _uberDiabloSpawnRadius, value);
            }
        }

        private bool _activeUberDiabloRushTweekAI;
        public bool ActiveUberDiabloRushTweekAI
        {
            get { return _activeUberDiabloRushTweekAI; }
            set
            {
                _ini.SetVal("UBER QUEST", "ActiveUberDiabloRushTweekAI", value);
                this.RaiseAndSetIfChanged(ref _activeUberDiabloRushTweekAI, value);
            }
        }

        private bool _activeUberBaalTeleportTweekAI;
        public bool ActiveUberBaalTeleportTweekAI
        {
            get { return _activeUberBaalTeleportTweekAI; }
            set
            {
                _ini.SetVal("UBER QUEST", "ActiveUberBaalTeleportTweekAI", value);
                this.RaiseAndSetIfChanged(ref _activeUberBaalTeleportTweekAI, value);
            }
        }

        private bool _activeUberBaalChillingArmorTweekAI;
        public bool ActiveUberBaalChillingArmorTweekAI
        {
            get { return _activeUberBaalChillingArmorTweekAI; }
            set
            {
                _ini.SetVal("UBER QUEST", "ActiveUberBaalChillingArmorTweekAI", value);
                this.RaiseAndSetIfChanged(ref _activeUberBaalChillingArmorTweekAI, value);
            }
        }

        private int _uberBaalChillingArmorTimer;
        public int UberBaalChillingArmorTimer
        {
            get { return _uberBaalChillingArmorTimer; }
            set
            {
                _ini.SetVal("UBER QUEST", "UberBaalChillingArmorTimer", value);
                this.RaiseAndSetIfChanged(ref _uberBaalChillingArmorTimer, value);
            }
        }
        #endregion

        #region [INTERFACE]
        private bool _activeNewStatsInterface;
        public bool ActiveNewStatsInterface
        {
            get { return _activeNewStatsInterface; }
            set
            {
                _ini.SetVal("INTERFACE", "ActiveNewStatsInterface", value);
                this.RaiseAndSetIfChanged(ref _activeNewStatsInterface, value);
            }
        }

        private bool _selectMainPageOnOpenning;
        public bool SelectMainPageOnOpenning
        {
            get { return _selectMainPageOnOpenning; }
            set
            {
                _ini.SetVal("INTERFACE", "SelectMainPageOnOpenning", value);
                this.RaiseAndSetIfChanged(ref _selectMainPageOnOpenning, value);
            }
        }

        private bool _printButtonsBackgroundOnMainStatsPage;
        public bool PrintButtonsBackgroundOnMainStatsPage
        {
            get { return _printButtonsBackgroundOnMainStatsPage; }
            set
            {
                _ini.SetVal("INTERFACE", "PrintButtonsBackgroundOnMainStatsPage", value);
                this.RaiseAndSetIfChanged(ref _printButtonsBackgroundOnMainStatsPage, value);
            }
        }
        #endregion

        #region [EXTRA]
        private bool _activeLaunchAnyNumberOfLOD;
        public bool ActiveLaunchAnyNumberOfLOD
        {
            get { return _activeLaunchAnyNumberOfLOD; }
            set
            {
                _ini.SetVal("EXTRA", "ActiveLaunchAnyNumberOfLOD", value);
                this.RaiseAndSetIfChanged(ref _activeLaunchAnyNumberOfLOD, value);
            }
        }

        private bool _alwaysRegenMapInSP;
        public bool AlwaysRegenMapInSP
        {
            get { return _alwaysRegenMapInSP; }
            set
            {
                if (value) ShowMsgBaseModConflict("MapAutoRegen", "Enabled", "AlwaysRegenMapInSP");

                _ini.SetVal("EXTRA", "AlwaysRegenMapInSP", value);
                this.RaiseAndSetIfChanged(ref _alwaysRegenMapInSP, value);
            }
        }

        private int _nBPlayersByDefault;
        public int NBPlayersByDefault
        {
            get { return _nBPlayersByDefault; }
            set
            {
                if (value != 0) ShowMsgBaseModConflict("PlayersX", "Enabled", "NBPlayersByDefault", 0);

                _ini.SetVal("EXTRA", "NBPlayersByDefault", value);
                this.RaiseAndSetIfChanged(ref _nBPlayersByDefault, value);
            }
        }

        private bool _activeDisplayItemLevel;
        public bool ActiveDisplayItemLevel
        {
            get { return _activeDisplayItemLevel; }
            set
            {
                if (value) ShowMsgBaseModConflict("IlvlDisplay", "Enabled", "ActiveDisplayItemLevel");

                _ini.SetVal("EXTRA", "ActiveDisplayItemLevel", value);
                this.RaiseAndSetIfChanged(ref _activeDisplayItemLevel, value);
            }
        }

        private int _alwaysDisplayLifeAndManaValues;
        public int AlwaysDisplayLifeAndManaValues
        {
            get { return _alwaysDisplayLifeAndManaValues; }
            set
            {
                _ini.SetVal("EXTRA", "AlwaysDisplayLifeAndManaValues", value);
                this.RaiseAndSetIfChanged(ref _alwaysDisplayLifeAndManaValues, value);
            }
        }

        private bool _enabledTXTFilesWhenMSExcelOpenIt;
        public bool EnabledTXTFilesWhenMSExcelOpenIt
        {
            get { return _enabledTXTFilesWhenMSExcelOpenIt; }
            set
            {
                _ini.SetVal("EXTRA", "EnabledTXTFilesWhenMSExcelOpenIt", value);
                this.RaiseAndSetIfChanged(ref _enabledTXTFilesWhenMSExcelOpenIt, value);
            }
        }

        private bool _activeDisplayBaseStatsValue;
        public bool ActiveDisplayBaseStatsValue
        {
            get { return _activeDisplayBaseStatsValue; }
            set
            {
                _ini.SetVal("EXTRA", "ActiveDisplayBaseStatsValue", value);
                this.RaiseAndSetIfChanged(ref _activeDisplayBaseStatsValue, value);
            }
        }

        private bool _activeLadderRunewords;
        public bool ActiveLadderRunewords
        {
            get { return _activeLadderRunewords; }
            set
            {
                if (value) ShowMsgBaseModConflict("LadderRuneWords", "Enabled", "ActiveLadderRunewords");

                _ini.SetVal("EXTRA", "ActiveLadderRunewords", value);
                this.RaiseAndSetIfChanged(ref _activeLadderRunewords, value);
            }
        }

        private bool _activeCowPortalWhenCowKingWasKilled;
        public bool ActiveCowPortalWhenCowKingWasKilled
        {
            get { return _activeCowPortalWhenCowKingWasKilled; }
            set
            {
                _ini.SetVal("EXTRA", "ActiveCowPortalWhenCowKingWasKilled", value);
                this.RaiseAndSetIfChanged(ref _activeCowPortalWhenCowKingWasKilled, value);
            }
        }

        private bool _activeDoNotCloseNihlathakPortal;
        public bool ActiveDoNotCloseNihlathakPortal
        {
            get { return _activeDoNotCloseNihlathakPortal; }
            set
            {
                if (value) ShowMsgBaseModConflict("NilPortalFix", "Enabled", "ActiveDoNotCloseNihlathakPortal");

                _ini.SetVal("EXTRA", "ActiveDoNotCloseNihlathakPortal", value);
                this.RaiseAndSetIfChanged(ref _activeDoNotCloseNihlathakPortal, value);
            }
        }

        private bool _moveCainNearHarrogathWaypoint;
        public bool MoveCainNearHarrogathWaypoint
        {
            get { return _moveCainNearHarrogathWaypoint; }
            set
            {
                _ini.SetVal("EXTRA", "MoveCainNearHarrogathWaypoint", value);
                this.RaiseAndSetIfChanged(ref _moveCainNearHarrogathWaypoint, value);
            }
        }
        #endregion


    }
}
