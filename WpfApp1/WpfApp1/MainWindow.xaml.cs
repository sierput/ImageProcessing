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
using System.Drawing;
using ImageProcessingLibrary;
using System.Threading;
using Rectangle = System.Drawing.Rectangle;
using System.Drawing.Imaging;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);
        public static string IMAGE = System.IO.Directory.GetCurrentDirectory() + "\\..\\..\\" + "Desert.jpg";
        ImageProcessing imageProcessing;
        public MainWindow()
        {
            InitializeComponent();
            imageProcessing = new ImageProcessing(IMAGE);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (colorImage.Source == null)
                colorImage.Source = new BitmapImage(new Uri(IMAGE));
        }

        private void Load_grey_image_Click(object sender, RoutedEventArgs e)
        {
            if (greyImage.Source != null)
                return;
            imageProcessing.grayScale();
            greyImage.Source = new BitmapImage(new Uri(path + "\\" + ImageProcessing.IMAGEGREY));
            TimeLabel.Content = "Time: " + imageProcessing.Time + "ms";
            if (imageProcessing.TimeNativ != 0)
                TimeCompare.Content = "Time compare: " + (imageProcessing.Time - imageProcessing.TimeNativ) + "ms";
        }

        async private void Grey_asyn_button_Click(object sender, RoutedEventArgs e)
        {
            if (imageAsyn.Source != null)
                return;
            await Task.Run(() =>
             {
                 Bitmap greyBitMap = imageProcessing.greyScaleAsyn();
                 Thread.Sleep(2000);
                 greyBitMap.Save(ImageProcessing.IMAGEGREYASYN);
             });
            imageAsyn.Source = new BitmapImage(new Uri(path + "\\" + ImageProcessing.IMAGEGREYASYN));
        }

        private void Nativ_cpp_grey_Click(object sender, RoutedEventArgs e)
        {
            if (imageGreyNative.Source != null)
                return;
            
            imageProcessing.nativCppGreyScale();
            imageGreyNative.Source = new BitmapImage(new Uri(path + "\\" + ImageProcessing.IMAGENATIV));
            TimeNativLabel.Content = "Time: " + imageProcessing.TimeNativ + "ms";
            if (imageProcessing.Time != 0)
                TimeCompare.Content = "Time compare: " + (imageProcessing.Time - imageProcessing.TimeNativ) + "ms";
        }
    }
}