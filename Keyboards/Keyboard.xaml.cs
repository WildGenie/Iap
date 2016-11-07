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
        private const int WM_KEYDOWN = 0x0100;
        private const byte VK_BACK = 0x8;          // back space
        private const byte VK_LEFT = 0x25;
        private const byte VK_RIGHT = 0x27;
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
        private const byte VK_CAPITAL = 0x14;
        private const byte VK_LSHIFT = 0xA0;
        private const byte VK_LALT = 0xA4;

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
        private const byte VK_SPACE = 0x20;
        private const byte VK_ENTER = 0x0D;
        private const byte VK_APOSTROPHE = 0xDE;
        #endregion

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                this.PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
        }

        private void cmdNumericButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Button key = (Button)sender;
                switch (key.Name)
                {
                    case "One":
                        keybd_event(VK_1, 0, 0, (UIntPtr)0);
                        keybd_event(VK_1, 0, KEYEVENTF_KEYUP, (UIntPtr)0);
                        e.Handled = true;
                        break;
                    case "Two":
                        keybd_event(VK_2, 0, 0, (UIntPtr)0);
                        keybd_event(VK_2, 0, KEYEVENTF_KEYUP, (UIntPtr)0);
                        e.Handled = true;
                        break;
                    case "Three":
                        keybd_event(VK_3, 0, 0, (UIntPtr)0);
                        keybd_event(VK_3, 0, KEYEVENTF_KEYUP, (UIntPtr)0);
                        e.Handled = true;
                        break;
                    case "Four":
                        keybd_event(VK_4, 0, 0, (UIntPtr)0);
                        keybd_event(VK_4, 0, KEYEVENTF_KEYUP, (UIntPtr)0);
                        e.Handled = true;
                        break;
                    case "Five":
                        keybd_event(VK_5, 0, 0, (UIntPtr)0);
                        keybd_event(VK_5, 0, KEYEVENTF_KEYUP, (UIntPtr)0);
                        e.Handled = true;
                        break;
                    case "Six":
                        keybd_event(VK_6, 0, 0, (UIntPtr)0);
                        keybd_event(VK_6, 0, KEYEVENTF_KEYUP, (UIntPtr)0);
                        e.Handled = true;
                        break;
                    case "Seven":
                        keybd_event(VK_7, 0, 0, (UIntPtr)0);
                        keybd_event(VK_7, 0, KEYEVENTF_KEYUP, (UIntPtr)0);
                        e.Handled = true;
                        break;
                    case "Eight":
                        keybd_event(VK_8, 0, 0, (UIntPtr)0);
                        keybd_event(VK_8, 0, KEYEVENTF_KEYUP, (UIntPtr)0);
                        e.Handled = true;
                        break;
                    case "Nine":
                        keybd_event(VK_9, 0, 0, (UIntPtr)0);
                        keybd_event(VK_9, 0, KEYEVENTF_KEYUP, (UIntPtr)0);
                        e.Handled = true;
                        break;
                    case "Zero":
                        keybd_event(VK_0, 0, 0, (UIntPtr)0);
                        keybd_event(VK_0, 0, KEYEVENTF_KEYUP, (UIntPtr)0);
                        e.Handled = true;
                        break;
                }
            }
            catch { }
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
                        keybd_event(VK_E, 0, 0, (UIntPtr)0);
                        keybd_event(VK_E, 0, KEYEVENTF_KEYUP, (UIntPtr)0);
                        e.Handled = true;
                        break;
                    case "F":
                        keybd_event(VK_F, 0, 0, (UIntPtr)0);
                        keybd_event(VK_F, 0, KEYEVENTF_KEYUP, (UIntPtr)0);
                        e.Handled = true;
                        break;
                    case "G":
                        keybd_event(VK_G, 0, 0, (UIntPtr)0);
                        keybd_event(VK_G, 0, KEYEVENTF_KEYUP, (UIntPtr)0);
                        e.Handled = true;
                        break;
                    case "H":
                        keybd_event(VK_H, 0, 0, (UIntPtr)0);
                        keybd_event(VK_H, 0, KEYEVENTF_KEYUP, (UIntPtr)0);
                        e.Handled = true;
                        break;
                    case "I":
                        keybd_event(VK_I, 0, 0, (UIntPtr)0);
                        keybd_event(VK_I, 0, KEYEVENTF_KEYUP, (UIntPtr)0);
                        e.Handled = true;
                        break;
                    case "J":
                        keybd_event(VK_J, 0, 0, (UIntPtr)0);
                        keybd_event(VK_J, 0, KEYEVENTF_KEYUP, (UIntPtr)0);
                        e.Handled = true;
                        break;
                    case "K":
                        keybd_event(VK_K, 0, 0, (UIntPtr)0);
                        keybd_event(VK_K, 0, KEYEVENTF_KEYUP, (UIntPtr)0);
                        e.Handled = true;
                        break;
                    case "L":
                        keybd_event(VK_L, 0, 0, (UIntPtr)0);
                        keybd_event(VK_L, 0, KEYEVENTF_KEYUP, (UIntPtr)0);
                        e.Handled = true;
                        break;
                    case "M":
                        keybd_event(VK_M, 0, 0, (UIntPtr)0);
                        keybd_event(VK_M, 0, KEYEVENTF_KEYUP, (UIntPtr)0);
                        e.Handled = true;
                        break;
                    case "N":
                        keybd_event(VK_N, 0, 0, (UIntPtr)0);
                        keybd_event(VK_N, 0, KEYEVENTF_KEYUP, (UIntPtr)0);
                        e.Handled = true;
                        break;
                    case "O":
                        keybd_event(VK_O, 0, 0, (UIntPtr)0);
                        keybd_event(VK_O, 0, KEYEVENTF_KEYUP, (UIntPtr)0);
                        e.Handled = true;
                        break;
                    case "P":
                        keybd_event(VK_P, 0, 0, (UIntPtr)0);
                        keybd_event(VK_P, 0, KEYEVENTF_KEYUP, (UIntPtr)0);
                        e.Handled = true;
                        break;
                    case "Q":
                        keybd_event(VK_Q, 0, 0, (UIntPtr)0);
                        keybd_event(VK_Q, 0, KEYEVENTF_KEYUP, (UIntPtr)0);
                        e.Handled = true;
                        break;
                    case "R":
                        keybd_event(VK_R, 0, 0, (UIntPtr)0);
                        keybd_event(VK_R, 0, KEYEVENTF_KEYUP, (UIntPtr)0);
                        e.Handled = true;
                        break;
                    case "S":
                        keybd_event(VK_S, 0, 0, (UIntPtr)0);
                        keybd_event(VK_S, 0, KEYEVENTF_KEYUP, (UIntPtr)0);
                        e.Handled = true;
                        break;
                    case "T":
                        keybd_event(VK_T, 0, 0, (UIntPtr)0);
                        keybd_event(VK_T, 0, KEYEVENTF_KEYUP, (UIntPtr)0);
                        e.Handled = true;
                        break;
                    case "U":
                        keybd_event(VK_U, 0, 0, (UIntPtr)0);
                        keybd_event(VK_U, 0, KEYEVENTF_KEYUP, (UIntPtr)0);
                        e.Handled = true;
                        break;
                    case "V":
                        keybd_event(VK_V, 0, 0, (UIntPtr)0);
                        keybd_event(VK_V, 0, KEYEVENTF_KEYUP, (UIntPtr)0);
                        e.Handled = true;
                        break;
                    case "W":
                        keybd_event(VK_W, 0, 0, (UIntPtr)0);
                        keybd_event(VK_W, 0, KEYEVENTF_KEYUP, (UIntPtr)0);
                        e.Handled = true;
                        break;
                    case "X":
                        keybd_event(VK_X, 0, 0, (UIntPtr)0);
                        keybd_event(VK_X, 0, KEYEVENTF_KEYUP, (UIntPtr)0);
                        e.Handled = true;
                        break;
                    case "Y":
                        keybd_event(VK_Y, 0, 0, (UIntPtr)0);
                        keybd_event(VK_Y, 0, KEYEVENTF_KEYUP, (UIntPtr)0);
                        e.Handled = true;
                        break;
                    case "Z":
                        keybd_event(VK_Z, 0, 0, (UIntPtr)0);
                        keybd_event(VK_Z, 0, KEYEVENTF_KEYUP, (UIntPtr)0);
                        e.Handled = true;
                        break;
                }
            }

            catch { }
        }

        private void cmdSymbolButtonClick(object sender, RoutedEventArgs e)
        {
            try
            {
                Button key = (Button)sender;
                switch (key.Name)
                {
                    case "Backspace":
                        keybd_event(VK_BACK, 0, 0, (UIntPtr)0);
                        keybd_event(VK_BACK, 0, KEYEVENTF_KEYUP, (UIntPtr)0);
                        e.Handled = true;
                        break;
                    case "LeftBracket":
                        keybd_event(VK_LEFT_BRACKET, 0, 0, (UIntPtr)0);
                        keybd_event(VK_LEFT_BRACKET, 0, KEYEVENTF_KEYUP, (UIntPtr)0);
                        e.Handled = true;
                        break;
                    case "RightBracket":
                        keybd_event(VK_RIGHT_BRACKET, 0, 0, (UIntPtr)0);
                        keybd_event(VK_RIGHT_BRACKET, 0, KEYEVENTF_KEYUP, (UIntPtr)0);
                        e.Handled = true;
                        break;
                    case "Slash":
                        keybd_event(VK_SLASH, 0, 0, (UIntPtr)0);
                        keybd_event(VK_SLASH, 0, KEYEVENTF_KEYUP, (UIntPtr)0);
                        e.Handled = true;
                        break;
                    case "semicolon":
                        keybd_event(VK_SEMICOLON, 0, 0, (UIntPtr)0);
                        keybd_event(VK_SEMICOLON, 0, KEYEVENTF_KEYUP, (UIntPtr)0);
                        e.Handled = true;
                        break;
                    case "apostrophe":
                        keybd_event(VK_APOSTROPHE, 0, 0, (UIntPtr)0);
                        keybd_event(VK_APOSTROPHE, 0, KEYEVENTF_KEYUP, (UIntPtr)0);
                        e.Handled = true;
                        break;
                    case "enter":
                        keybd_event(VK_ENTER, 0, 0, (UIntPtr)0);
                        keybd_event(VK_ENTER, 0, KEYEVENTF_KEYUP, (UIntPtr)0);
                        e.Handled = true;
                        break;
                    case "comma":
                        keybd_event(VK_COMMA, 0, 0, (UIntPtr)0);
                        keybd_event(VK_COMMA, 0, KEYEVENTF_KEYUP, (UIntPtr)0);
                        e.Handled = true;
                        break;
                    case "Dot":
                        keybd_event(VK_DOT, 0, 0, (UIntPtr)0);
                        keybd_event(VK_DOT, 0, KEYEVENTF_KEYUP, (UIntPtr)0);
                        e.Handled = true;
                        break;
                    case "forwslash":
                        keybd_event(VK_FORSLASH, 0, 0, (UIntPtr)0);
                        keybd_event(VK_FORSLASH, 0, KEYEVENTF_KEYUP, (UIntPtr)0);
                        e.Handled = true;
                        break;
                    case "Space":
                        keybd_event(VK_SPACE, 0, 0, (UIntPtr)0);
                        keybd_event(VK_SPACE, 0, KEYEVENTF_KEYUP, (UIntPtr)0);
                        e.Handled = true;
                        break;
                }
            }
            catch { }
        }

        private void changelang_Click(object sender, RoutedEventArgs e)
        {
            keybd_event(VK_LSHIFT, 0, 0, (UIntPtr)0);
            keybd_event(VK_LSHIFT, 0, WM_KEYDOWN, (UIntPtr)0);

            keybd_event(VK_LALT, 0, 0, (UIntPtr)0);
            keybd_event(VK_LALT, 0, KEYEVENTF_KEYUP, (UIntPtr)0);

            keybd_event(VK_LSHIFT, 0, KEYEVENTF_KEYUP, (UIntPtr)0);

            ImageBrush myBrush = new ImageBrush();
            if (isEnghlish)
            {
                if (isCaps)
                {
                    myBrush.ImageSource = new BitmapImage
                (new Uri(@"pack://application:,,,/Images/AIA_FOR PNG-42.png"));
                }
                else
                {
                    myBrush.ImageSource = new BitmapImage
                (new Uri(@"pack://application:,,,/Images/AIA_FOR PNG-43.png"));
                }

                isEnghlish = false;
                
            }
            else
            {
                if (isCaps)
                {
                    myBrush.ImageSource = new BitmapImage
                    (new Uri(@"pack://application:,,,/Images/AIA_FOR PNG-22.png"));
                }
                else
                {
                    myBrush.ImageSource = new BitmapImage
                    (new Uri(@"pack://application:,,,/Images/AIA_FOR PNG-23.png"));
                }
                isEnghlish = true;
            }

            this.KeyboardGrid.Background = myBrush;
        }


        public bool isEnghlish = true;
        public bool isCaps = false;

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
            keybd_event(VK_CAPITAL, 0, 0, (UIntPtr)0);
            keybd_event(VK_CAPITAL, 0, KEYEVENTF_KEYUP, (UIntPtr)0);
            this.KeyboardGrid.Background = myBrush;
        }

        private void Caps2_Click(object sender, RoutedEventArgs e)
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
            keybd_event(VK_CAPITAL, 0, 0, (UIntPtr)0);
            keybd_event(VK_CAPITAL, 0, KEYEVENTF_KEYUP, (UIntPtr)0);
            this.KeyboardGrid.Background = myBrush;
        }
    }
}
