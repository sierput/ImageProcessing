using System;
using System.Collections.Generic;
using Prism.Commands;
using Prism.Mvvm;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using ImageProcessingLibrary;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;

namespace WpfApp1MVVM.ViewModel
{
    class ImageProcessingViewModel : BindableBase
    {
        public ICommand buttonImage { get; set; }
        public ICommand buttonGreyImage { get; set; }
        public ICommand buttonGreyAsync { get; set; }
        public ICommand buttonNativCppGrey { get; set; }
        private BitmapImage colorImage { get; set; }
        private BitmapImage greyImage { get; set; }
        private BitmapImage imageAsync { get; set; }
        private BitmapImage imageGreyNative { get; set; }
        private string timeLabel { get; set; }
        private string timeNativLabel { get; set; }
        private string timeCompare { get; set; }
        ImageProcessing imageProcessing;
        TimerProcessing timer = null;
        TimerProcessing timerNativ = null;

        public BitmapImage ColorImage
        {
            get { return colorImage; }
            set { colorImage = value; OnPropertyChanged(() => ColorImage); }
        }

        public BitmapImage GreyImage
        {
            get { return greyImage; }
            set { greyImage = value; OnPropertyChanged(() => GreyImage); }
        }

        public BitmapImage ImageAsync
        {
            get { return imageAsync; }
            set { imageAsync = value; OnPropertyChanged(() => ImageAsync); }
        }

        public BitmapImage ImageGreyNative
        {
            get { return imageGreyNative; }
            set { imageGreyNative = value; OnPropertyChanged(() => ImageGreyNative); }
        }

        public string TimeLabel
        {
            get { return timeLabel; }
            set { timeLabel = value; OnPropertyChanged(() => TimeLabel); }
        }

        public string TimeNativLabel
        {
            get { return timeNativLabel; }
            set { timeNativLabel = value; OnPropertyChanged(() => TimeNativLabel); }
        }

        public string TimeCompare
        {
            get { return timeCompare; }
            set { timeCompare = value; OnPropertyChanged(() => TimeCompare); }
        }

        public ImageProcessingViewModel()
        {
            buttonImage = new DelegateCommand(ClickButtonImage);
            buttonGreyImage = new DelegateCommand(ClickLoadGreyImage);
            buttonGreyAsync = new DelegateCommand(ClickbuttonGreyAsync);
            buttonNativCppGrey = new DelegateCommand(ClickbuttonNativCppGrey);
            imageProcessing = new ImageProcessing();
        }

        private void ClickButtonImage()
        {

            if (colorImage != null)
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
            ColorImage = new BitmapImage(new Uri(imagePath));
        }

        private void ClickLoadGreyImage()
        {
            if (greyImage != null)
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
            Bitmap bitmapGrey = timer.imageProcessingTime(imageProcessing.greyScale, image);
            BitmapImage greyBitMapImage = toBitmapImage(bitmapGrey);
            GreyImage = greyBitMapImage;
            TimeLabel = "Time: " + timer.Time + "ms";
            if (timerNativ != null)
                TimeCompare = "Time compare: " + (timer.Time - timerNativ.Time) + "ms";
        }

       async private void ClickbuttonGreyAsync()
        {

            if (imageAsync != null)
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
                greyBitMapImage = toBitmapImage(imageProcessing.greyScaleAsync(image));
            });
           ImageAsync = greyBitMapImage;
        }

        private void ClickbuttonNativCppGrey()
        {
            if (imageGreyNative != null)
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
            ImageGreyNative = greyBitMapImage;
            TimeNativLabel = "Time: " + timerNativ.Time + "ms";
            if (timer != null)
                TimeCompare = "Time compare: " + (timer.Time - timerNativ.Time) + "ms";
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
