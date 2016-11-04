using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Iap.Keyboards
{
    /// <summary>
    /// Interaction logic for Keyboard.xaml
    /// </summary>
    public partial class Keyboard : UserControl, INotifyPropertyChanged
    {
        public Keyboard()
        {
            InitializeComponent();
        }

        #region Windows API Functions

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, UIntPtr dwExtraInfo);

        #endregion Windows API Functions

        #region Keyboard Constants

        private const uint KEYEVENTF_KEYUP = 0x2;  // Release key
        private const byte VK_BACK = 0x8;          // back space
        private const byte VK_LEFT = 0x25;
        private const byte VK_RIGHT = 0x27;
        private const byte VK_CAPS = 0xF9;
        private const byte VK_0 = 0x30;
        private const byte VK_1 = 0x31;
        private const byte VK_2 = 0x32;
        private const byte VK_3 = 0x33;
        private const byte VK_4 = 0x34;
        private const byte VK_5 = 0x35;
        private const byte VK_6 = 0x36;
        private const byte VK_7 = 0x37;
        private const byte VK_8 = 0x38;
        private const byte VK_9 = 0x39;

        #endregion

        #region charsEnglishConstrands
        private const byte VK_A = 0X41;
        private const byte VK_B = 0X42;
        private const byte VK_C = 0X43;
        private const byte VK_D = 0X44;
        private const byte VK_E = 0X45;
        private const byte VK_F = 0X46;
        private const byte VK_G = 0X47;
        private const byte VK_H = 0X48;
        private const byte VK_I = 0X49;
        private const byte VK_J = 0x4A;
        private const byte VK_K = 0x4B;
        private const byte VK_L = 0x4C;
        private const byte VK_M = 0X4D;
        private const byte VK_N = 0X4E;
        private const byte VK_O = 0x4F;
        private const byte VK_P = 0X50;
        private const byte VK_Q = 0x51;
        private const byte VK_R = 0x52;
        private const byte VK_S = 0x53;
        private const byte VK_T = 0x54;
        private const byte VK_U = 0x55;
        private const byte VK_V = 0x56;
        private const byte VK_W = 0x57;
        private const byte VK_X = 0x58;
        private const byte VK_Y = 0x59;
        private const byte VK_Z = 0x5A;
        #endregion

        #region symbolConstarnts
        private const byte VK_LEFT_BRACKET = 0xDB;
        private const byte VK_RIGHT_BRACKET = 0xDD;
        private const byte VK_SLASH = 0xDC;
        private const byte VK_SEMICOLON = 0xBA;
        private const byte VK_COMMA = 0xBC;
        private const byte VK_DOT = 0xBE;
        private const byte VK_FORSLASH = 0xBF;
        #endregion

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                this.PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
        }

        private void cmdNumericButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void cmdCharButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Button key = (Button)sender;
                switch(key.Name)
                {
                    case "A":
                        keybd_event(VK_A, 0, 0, (UIntPtr)0);
                        keybd_event(VK_A, 0, KEYEVENTF_KEYUP, (UIntPtr)0);
                        e.Handled = true;
                        break;
                    case "B":
                        keybd_event(VK_B, 0, 0, (UIntPtr)0);
                        keybd_event(VK_B, 0, KEYEVENTF_KEYUP, (UIntPtr)0);
                        e.Handled = true;
                        break;
                    case "C":
                        keybd_event(VK_C, 0, 0, (UIntPtr)0);
                        keybd_event(VK_C, 0, KEYEVENTF_KEYUP, (UIntPtr)0);
                        e.Handled = true;
                        break;
                    case "D":
                        keybd_event(VK_D, 0, 0, (UIntPtr)0);
                        keybd_event(VK_D, 0, KEYEVENTF_KEYUP, (UIntPtr)0);
                        e.Handled = true;
                        break;
                    case "E":
                        break;
                    case "F":
                        break;
                    case "G":
                        break;
                    case "H":
                        break;
                    case "I":
                        break;
                    case "J":
                        break;
                    case "K":
                        break;
                    case "L":
                        break;
                    case "M":
                        break;
                    case "N":
                        break;
                    case "O":
                        break;
                    case "P":
                        break;
                    case "Q":
                        break;
                    case "R":
                        break;
                    case "S":
                        break;
                    case "T":
                        break;
                    case "U":
                        break;
                    case "V":
                        break;
                    case "W":
                        break;
                    case "X":
                        break;
                    case "Y":
                        break;
                    case "Z":
                        break;
                }
            }

            catch { }
        }

        private void cmdSymbolButtonClick(object sender, RoutedEventArgs e)
        {

        }

        private void changelang_Click(object sender, RoutedEventArgs e)
        {
            ImageBrush myBrush = new ImageBrush();
            if (isEnghlish)
            {
                myBrush.ImageSource = new BitmapImage
                (new Uri(@"pack://application:,,,/Images/AIA_FOR PNG-42.png"));
                isEnghlish = false;
            }
            else
            {
                myBrush.ImageSource = new BitmapImage
                (new Uri(@"pack://application:,,,/Images/AIA_FOR PNG-22.png"));
                isEnghlish = true;
            }
            this.KeyboardGrid.Background = myBrush;
        }


        public bool isEnghlish = true;

        private string GridBackground
        {
            get
            {
                if (isEnghlish)
                {
                    return "/Images/AIA_FOR PNG-22.png";
                }
                else
                {
                    return "/Images/AIA_FOR PNG-42.png";
                }
            }
        }

        public bool isCaps = true;

        private void Caps_Click(object sender, RoutedEventArgs e)
        {
            ImageBrush myBrush = new ImageBrush();
            if (isCaps)
            {
                if (isEnghlish)
                {
                    myBrush.ImageSource = new BitmapImage
                (new Uri(@"pack://application:,,,/Images/AIA_FOR PNG-23.png"));
                }
                else
                {
                    myBrush.ImageSource = new BitmapImage
                (new Uri(@"pack://application:,,,/Images/AIA_FOR PNG-43.png"));
                }
                
                isCaps = false;
            }
            else
            {
                keybd_event(VK_CAPS, 0, 0, (UIntPtr)0);
                keybd_event(VK_CAPS, 0, KEYEVENTF_KEYUP, (UIntPtr)0);
                if (isEnghlish)
                {
                    myBrush.ImageSource = new BitmapImage
                    (new Uri(@"pack://application:,,,/Images/AIA_FOR PNG-22.png"));
                }
                else
                {
                    myBrush.ImageSource = new BitmapImage
                    (new Uri(@"pack://application:,,,/Images/AIA_FOR PNG-42.png"));
                }
                isCaps = true;
            }
            this.KeyboardGrid.Background = myBrush;
        }
    }
}
