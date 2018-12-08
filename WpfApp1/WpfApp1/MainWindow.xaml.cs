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
using Microsoft.Win32;
using System.IO;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);
        ImageProcessing imageProcessing;
        TimerProcessing timer = null;
        TimerProcessing timerNativ = null;
        public MainWindow()
        {
            InitializeComponent();
            imageProcessing = new ImageProcessing();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (colorImage.Source != null)
            {
                MessageBox.Show("The image has been selected");
                return;
            }
            string imagePath = loadPicture();
            if (imagePath.Length == 0)
            {
                MessageBox.Show("File path cannot be empty");
                return;
            }
            colorImage.Source = new BitmapImage(new Uri(imagePath));
        }

        private void Load_grey_image_Click(object sender, RoutedEventArgs e)
        {
            if (greyImage.Source != null)
            {
                MessageBox.Show("The image has been selected");
                return;
            }
           Bitmap image = imageProcessing.loadImageFromPath(loadPicture());
            if (image == null)
            {
                MessageBox.Show("File path cannot be empty");
                return;
            }
            timer = new TimerProcessing();
            Bitmap bitmapGrey = timer.imageProcessingTime(imageProcessing.grayScale, image);
            BitmapImage greyBitMapImage = toBitmapImage(bitmapGrey);
            greyImage.Source = greyBitMapImage;
            TimeLabel.Content = "Time: " + timer.Time + "ms";
            if (timerNativ != null)
                TimeCompare.Content = "Time compare: " + (timer.Time - timerNativ.Time) + "ms";
        }

        async private void Grey_asyn_button_Click(object sender, RoutedEventArgs e)
        {
            if (imageAsyn.Source != null)
            {
                MessageBox.Show("The image has been selected");
                return;
            }
            BitmapImage greyBitMapImage = null;
            await Task.Run(() =>
             {
                 Bitmap image = imageProcessing.loadImageFromPath(loadPicture());
                 if (image == null)
                 {
                     MessageBox.Show("File path cannot be empty");
                     return;
                 }
                 greyBitMapImage = toBitmapImage(imageProcessing.greyScaleAsyn(image));
             });
            imageAsyn.Source = greyBitMapImage;
        }

        private void Nativ_cpp_grey_Click(object sender, RoutedEventArgs e)
        {
            if (imageGreyNative.Source != null)
            {
                MessageBox.Show("The image has been selected");
                return;
            }
            Bitmap image = imageProcessing.loadImageFromPath(loadPicture());
            if (image == null)
            {
                MessageBox.Show("File path cannot be empty");
                return;
            }
            timerNativ = new TimerProcessing();
            Bitmap bitmapGrey = timerNativ.imageProcessingTime(imageProcessing.nativCppGreyScale, image);
            BitmapImage greyBitMapImage = toBitmapImage(bitmapGrey);
            imageGreyNative.Source = greyBitMapImage;
            TimeNativLabel.Content = "Time: " + timerNativ.Time + "ms";
            if (timer != null)
                TimeCompare.Content = "Time compare: " + (timer.Time - timerNativ.Time) + "ms";
        }

        private string loadPicture()
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = "Select a picture";
            op.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
              "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
              "Portable Network Graphic (*.png)|*.png";
            if (op.ShowDialog() == true)
            {
                return op.FileName;
            }

            return "";
        }

        public static BitmapImage toBitmapImage(Bitmap bitmap)
        {
            using (var memory = new MemoryStream())
            {
                bitmap.Save(memory, ImageFormat.Jpeg);
                memory.Position = 0;
                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                bitmapImage.Freeze();
                return bitmapImage;
            }
        }
    }
}