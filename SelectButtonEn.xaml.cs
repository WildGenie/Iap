using System;
using System.Collections.Generic;
using System.Linq;
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

namespace Iap
{
    /// <summary>
    /// Interaction logic for SelectButtonEn.xaml
    /// </summary>
    public partial class SelectButtonEn : Button
    {

        public static readonly DependencyProperty EnabledUncheckedProperty =
           DependencyProperty.Register(
           "EnabledUnchecked",
           typeof(ImageSource),
           typeof(SelectButtonEn),
           new PropertyMetadata(HandleEnabledUncheckedChangedEvent));

        public static readonly DependencyProperty EnabledCheckedProperty =
           DependencyProperty.Register(
           "EnabledChecked",
           typeof(ImageSource),
           typeof(SelectButtonEn),
           new PropertyMetadata(HandleEnabledCheckedChangedEvent));

        public SelectButtonEn()
        {
            InitializeComponent();
        }

        public ImageSource EnabledUnchecked
        {
            get { return (ImageSource)GetValue(EnabledUncheckedProperty); }
            set { SetValue(EnabledUncheckedProperty, value); }
        }

        public ImageSource EnabledChecked
        {
            get { return (ImageSource)GetValue(EnabledCheckedProperty); }
            set { SetValue(EnabledCheckedProperty, value); }
        }

        private static void HandleEnabledUncheckedChangedEvent(
           DependencyObject dobj,
           DependencyPropertyChangedEventArgs args)
        {
        }

        private static void HandleEnabledCheckedChangedEvent(
           DependencyObject dobj,
           DependencyPropertyChangedEventArgs args)
        {
        }

        private void Button_MouseDown(object sender, MouseButtonEventArgs e)
        {
          //  this.ChangeImage();
        }

        private void ChangeImage()
        {
            if (base.IsMouseOver)
            {
                this.ButtonImage.Source = this.EnabledChecked;
            }
            else
            {
                this.ButtonImage.Source = this.EnabledUnchecked;
            }
        }

        private void Button_Loaded(object sender, RoutedEventArgs e)
        {
            this.ButtonImage.Source = this.EnabledUnchecked;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.ChangeImage();
        }
    }
}
