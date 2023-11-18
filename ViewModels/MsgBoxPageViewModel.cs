using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Threading;
using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.Generic;

namespace PlugY_Configurator_Avalonia.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        #region MsgBox

        private bool _msgBox_Visab = false;
        public bool MsgBox_Visab
        {
            get => _msgBox_Visab;
            set => SetProperty(ref _msgBox_Visab, value);
        }

        public async Task<int> ShowMsgBox(string message, string title, MsgBoxIcon icon = MsgBoxIcon.Information, string btn1 = "Ok", string btn2 = "", string btn3 = "")
        {
            ShowMsgBoxAsync(message, title, icon, btn1, btn2, btn3, false, null);

            var wait = new ManualResetEvent(false);

            int result = 0;
            MsgClick += (s) =>
            {
                result = s;
                wait.Set();
            };

            await Task.Run(() => wait.WaitOne());
            return result;
        }

        public async Task<(int btnNum, int cmbBxIndex, string cmbBxItem)> ShowMsgBox(string message, string title, string[] comboBox, MsgBoxIcon icon = MsgBoxIcon.Information, string btn1 = "Ok", string btn2 = "", string btn3 = "")
        {
            ShowMsgBoxAsync(message, title, icon, btn1, btn2, btn3, false, comboBox);

            var wait = new ManualResetEvent(false);

            int result = 0;
            MsgClick += (s) =>
            {
                result = s;
                wait.Set();
            };

            await Task.Run(() => wait.WaitOne());
            return (result, MsgBox_ComboBox_Index, MsgBox_ComboBox_Item);
        }

        private void ShowMsgBoxAsync(string message, string title = "", MsgBoxIcon icon = MsgBoxIcon.none, string btn1 = "Ok", string btn2 = "", string btn3 = "", bool showTxtBox = false, string[]? comboBox = null)
        {
            if (MsgClick != null)
                foreach (Delegate d in MsgClick.GetInvocationList())
                    MsgClick -= (MsgBoxClick)d;

            MsgBox_Title_Txt = title;
            MsgBox_Txt = message;
            MsgBox_Btn1_Txt = btn1;
            MsgBox_Btn2_Txt = btn2;
            MsgBox_Btn3_Txt = btn3;
            MsgBox_TxtBox_Visb = showTxtBox;

            if (comboBox != null)
            {
                MsgBox_ComboBox_Item = string.Empty;
                MsgBox_ComboBox_Items = comboBox;
                MsgBox_ComboBox_Index = -1;
                MsgBox_ComboBox_Visab = true;
            }
            else MsgBox_ComboBox_Visab = false;

            Console.WriteLine();
            if (!string.IsNullOrEmpty(title))
                Console.WriteLine(title);
            if (!string.IsNullOrEmpty(message))
                Console.WriteLine(message);

            GeometryDrawing geometryDrawing = new();

            switch (icon)
            {
                case MsgBoxIcon.Information:
                    geometryDrawing.Geometry = Geometry.Parse("M7.2,5.6 L8.8,5.6 L8.8,4 L7.2,4  M8,14.4 C4.472,14.4 1.6,11.53 1.6,8 C1.6,4.472 4.472,1.6 8,1.6 C11.53,1.6 14.4,4.472 14.4,8 C14.4,11.53 11.53,14.4 8,14.4  M8,0 A8,8 0 0 0 0,8 A8,8 0 0 0 8,16 A8,8 0 0 0 16,8 A8,8 0 0 0 8,0  M7.2,12 L8.8,12 L8.8,7.2 L7.2,7.2 L7.2,12 z");
                    geometryDrawing.Brush = ThemeInvertColor;
                    break;

                case MsgBoxIcon.Good:
                    geometryDrawing.Geometry = Geometry.Parse("M8,0 C3.582,0 0,3.582 0,8 C0,12.42 3.582,16 8,16 C12.42,16 16,12.42 16,8 C16,3.582 12.42,0 8,0 z M8,1.548 C11.57,1.548 14.45,4.434 14.45,8 C14.45,11.57 11.57,14.45 8,14.45 C4.434,14.45 1.548,11.57 1.548,8 C1.548,4.434 4.434,1.548 8,1.548  M12.52,5.751 L11.8,5.018 C11.65,4.866 11.4,4.865 11.25,5.016 L6.689,9.539 L4.76,7.594 C4.609,7.442 4.364,7.441 4.212,7.592 L3.48,8.319 C3.328,8.469 3.327,8.715 3.477,8.866 L6.406,11.82 C6.556,11.97 6.801,11.97 6.953,11.82 L12.52,6.298 C12.67,6.147 12.67,5.902 12.52,5.751 z");
                    geometryDrawing.Brush = new SolidColorBrush(Colors.Green);
                    break;

                case MsgBoxIcon.Error:
                    geometryDrawing.Geometry = Geometry.Parse("M8,16 Q5.84,16 4,14.92 Q2.16,13.84 1.08,12 Q0,10.16 0,8 Q0,5.84 1.08,4 Q2.16,2.16 4,1.08 Q5.84,0 8,0 Q10.16,0 12,1.08 Q13.84,2.16 14.92,4 Q16,5.84 16,8 Q16,10.16 14.92,12 Q13.84,13.84 12,14.92 Q10.16,16 8,16 z M8,9.28 L10.56,11.84 L11.84,10.56 L9.28,8 L11.84,5.44 L10.56,4.16 L8,6.72 L5.44,4.16 L4.16,5.44 L6.72,8 L4.16,10.56 L5.44,11.84 L8,9.28 z");
                    geometryDrawing.Brush = new SolidColorBrush(Colors.Red);
                    break;

                case MsgBoxIcon.Help:
                    geometryDrawing.Geometry = Geometry.Parse("M7.2,12.8 L8.8,12.8 L8.8,11.2 L7.2,11.2 L7.2,12.8  M8,0 A8,8 0 0 0 0,8 A8,8 0 0 0 8,16 A8,8 0 0 0 16,8 A8,8 0 0 0 8,0  M8,14.4 C4.472,14.4 1.6,11.53 1.6,8 C1.6,4.472 4.472,1.6 8,1.6 C11.53,1.6 14.4,4.472 14.4,8 C14.4,11.53 11.53,14.4 8,14.4  M8,3.2 A3.2,3.2 0 0 0 4.8,6.4 L6.4,6.4 A1.6,1.6 0 0 1 8,4.8 A1.6,1.6 0 0 1 9.6,6.4 C9.6,8 7.2,7.8 7.2,10.4 L8.8,10.4 C8.8,8.6 11.2,8.4 11.2,6.4 A3.2,3.2 0 0 0 8,3.2 z");
                    geometryDrawing.Brush = ThemeInvertColor;
                    break;

                case MsgBoxIcon.Warning:
                    geometryDrawing.Geometry = Geometry.Parse("M4.684,0 L0,4.684 L0,11.32 L4.684,16 L11.32,16 C12.89,14.44 16,11.32 16,11.32 L16,4.684 L11.32,0  M5.422,1.778 L10.58,1.778 L14.22,5.422 L14.22,10.58 L10.58,14.22 L5.422,14.22 L1.778,10.58 L1.778,5.422  M7.111,10.67 L8.889,10.67 L8.889,12.44 L7.111,12.44 L7.111,10.67  M7.111,3.556 L8.889,3.556 L8.889,8.889 L7.111,8.889 L7.111,3.556");
                    geometryDrawing.Brush = new SolidColorBrush(Colors.Orange);
                    break;

                default:
                    MsgBox_Icon_Visab = false;
                    geometryDrawing.Geometry = default;
                    break;
            }
            if (geometryDrawing.Geometry != default)
            {
                MsgBox_Icon_Visab = true;
                MsgBox_Icon.Drawing = geometryDrawing;
            }

            MsgBox_Visab = true;
        }

        public enum MsgBoxIcon
        {
            Information,
            Good,
            Error,
            Help,
            Warning,
            none
        }

        private bool _msgBox_IconInformation_Visab = false;
        public bool MsgBox_IconInformation_Visab
        {
            get => _msgBox_IconInformation_Visab;
            set => SetProperty(ref _msgBox_IconInformation_Visab, value);
        }

        private bool _msgBox_IconGood_Visab = false;
        public bool MsgBox_IconGood_Visab
        {
            get => _msgBox_IconGood_Visab;
            set => SetProperty(ref _msgBox_IconGood_Visab, value);
        }

        private bool _msgBox_IconError_Visab = false;
        public bool MsgBox_IconError_Visab
        {
            get => _msgBox_IconError_Visab;
            set => SetProperty(ref _msgBox_IconError_Visab, value);
        }

        private bool _msgBox_IconHelp_Visab = false;
        public bool MsgBox_IconHelp_Visab
        {
            get => _msgBox_IconHelp_Visab;
            set => SetProperty(ref _msgBox_IconHelp_Visab, value);
        }

        private bool _msgBox_IconWarning_Visab = false;
        public bool MsgBox_IconWarning_Visab
        {
            get => _msgBox_IconWarning_Visab;
            set => SetProperty(ref _msgBox_IconWarning_Visab, value);
        }

        private DrawingImage _msgBox_Icon = new();
        public DrawingImage MsgBox_Icon
        {
            get => _msgBox_Icon;
            set => SetProperty(ref _msgBox_Icon, value);
        }

        private bool _msgBox_Icon_Visab = false;
        public bool MsgBox_Icon_Visab
        {
            get => _msgBox_Icon_Visab;
            set => SetProperty(ref _msgBox_Icon_Visab, value);
        }



        private bool _msgBox_Title_Visab = false;
        public bool MsgBox_Title_Visab
        {
            get => _msgBox_Title_Visab;
            set => SetProperty(ref _msgBox_Title_Visab, value);
        }

        private string _msgBox_Title_Txt = string.Empty;
        public string MsgBox_Title_Txt
        {
            get => _msgBox_Title_Txt;
            set
            {
                MsgBox_Title_Visab = !string.IsNullOrEmpty(value);
                SetProperty(ref _msgBox_Title_Txt, value);
            }
        }

        private string msgBox_Txt = string.Empty;
        public string MsgBox_Txt
        {
            get => msgBox_Txt;
            set => SetProperty(ref msgBox_Txt, value);
        }


        private bool _msgBox_TxtBox_Visb = false;
        public bool MsgBox_TxtBox_Visb
        {
            get => _msgBox_TxtBox_Visb;
            set
            {
                if (value) MsgBox_TxtBox_Txt = string.Empty;
                SetProperty(ref _msgBox_TxtBox_Visb, value);
            }
        }

        private string _msgBox_TxtBox_Txt = string.Empty;
        public string MsgBox_TxtBox_Txt
        {
            get => _msgBox_TxtBox_Txt;
            set => SetProperty(ref _msgBox_TxtBox_Txt, value);
        }

        private string _msgBox_ComboBox_Item = string.Empty;
        public string MsgBox_ComboBox_Item
        {
            get => _msgBox_ComboBox_Item;
            set => SetProperty(ref _msgBox_ComboBox_Item, value);
        }

        private IEnumerable<string>? _msgBox_ComboBox_Items;
        public IEnumerable<string>? MsgBox_ComboBox_Items 
        {
            get => _msgBox_ComboBox_Items;
            set => SetProperty(ref _msgBox_ComboBox_Items, value);
        }

        private bool _msgBox_ComboBox_Visab;
        public bool MsgBox_ComboBox_Visab
        {
            get => _msgBox_ComboBox_Visab;
            set => SetProperty(ref _msgBox_ComboBox_Visab, value);
        }

        private int _msgBox_ComboBox_Index;
        public int MsgBox_ComboBox_Index
        {
            get => _msgBox_ComboBox_Index;
            set => SetProperty(ref _msgBox_ComboBox_Index, value);
        }


        delegate void MsgBoxClick(int btnNum);
        event MsgBoxClick? MsgClick;
        public void MsgBox_Btn1_Click()
        {
            if (MsgClick != null)
            {
                MsgBox_Visab = false;
                MsgClick.Invoke(1);
            }
        }
        public void MsgBox_Btn2_Click()
        {
            if (MsgClick != null)
            {
                MsgBox_Visab = false;
                MsgClick.Invoke(2);
            }
        }
        public void MsgBox_Btn3_Click()
        {
            if (MsgClick != null)
            {
                MsgBox_Visab = false;
                MsgClick.Invoke(3);
            }
        }

        private bool _msgBox_Btn1_Visab = false;
        public bool MsgBox_Btn1_Visab
        {
            get => _msgBox_Btn1_Visab;
            set => SetProperty(ref _msgBox_Btn1_Visab, value);
        }
        private bool _msgBox_Btn2_Visab = false;
        public bool MsgBox_Btn2_Visab
        {
            get => _msgBox_Btn2_Visab;
            set => SetProperty(ref _msgBox_Btn2_Visab, value);
        }
        private bool _msgBox_Btn3_Visab = false;
        public bool MsgBox_Btn3_Visab
        {
            get => _msgBox_Btn3_Visab;
            set => SetProperty(ref _msgBox_Btn3_Visab, value);
        }

        private string _msgBox_Btn1_Txt = string.Empty;
        public string MsgBox_Btn1_Txt
        {
            get => _msgBox_Btn1_Txt;
            set
            {
                MsgBox_Btn1_Visab = !string.IsNullOrEmpty(value);
                SetProperty(ref _msgBox_Btn1_Txt, value);
            }
        }

        private string _msgBox_Btn2_Txt = string.Empty;
        public string MsgBox_Btn2_Txt
        {
            get => _msgBox_Btn2_Txt;
            set
            {
                MsgBox_Btn2_Visab = !string.IsNullOrEmpty(value);
                SetProperty(ref _msgBox_Btn2_Txt, value);
            }
        }

        private string _msgBox_Btn3_Txt = string.Empty;
        public string MsgBox_Btn3_Txt
        {
            get => _msgBox_Btn3_Txt;
            set
            {
                MsgBox_Btn3_Visab = !string.IsNullOrEmpty(value);
                SetProperty(ref _msgBox_Btn3_Txt, value);
            }
        }
        /*
        private double _msgBox_Width;
        public double MsgBox_Width
        {
            get => _msgBox_Width;
            set
            {
                //MsgBox_Btn3_Visab = !string.IsNullOrEmpty(value);
                SetProperty(ref _msgBox_Width, value);
            }
        }*/

        private SolidColorBrush _msgBox_BackgroundColor = SolidColorBrush.Parse("#fff5f5f5");
        public SolidColorBrush MsgBox_BackgroundColor
        {
            get => _msgBox_BackgroundColor;
            set => SetProperty(ref _msgBox_BackgroundColor, value);
        }

        private IBrush _msgBox_BorderColor = Brushes.Black;
        public IBrush MsgBox_BorderColor
        {
            get => _msgBox_BorderColor;
            set => SetProperty(ref _msgBox_BorderColor, value);
        }

        #endregion

    }
}
