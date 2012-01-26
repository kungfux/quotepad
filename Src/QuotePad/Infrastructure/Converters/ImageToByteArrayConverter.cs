using System;
using System.IO;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace QuotePad.Infrastructure.Converters
{
    [ValueConversion(typeof(byte[]), typeof(BitmapImage))]
    public class ImageToByteArrayConverter : IValueConverter
    {
        public Uri DefaultImageUri { get; set; }
        public Uri InvalidImageUri { get; set; }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            BitmapImage bitmapResult;
            if (value == null && DefaultImageUri != null)
            {
                bitmapResult = new BitmapImage(DefaultImageUri);
            }
            else if (value as byte[] != null)
            {
                try
                {
                    var stream = new MemoryStream((byte[])value);
                    bitmapResult = new BitmapImage();
                    bitmapResult.BeginInit();
                    bitmapResult.StreamSource = stream;
                    bitmapResult.EndInit();

                }
                catch (Exception)
                {
                   bitmapResult = InvalidImageUri != null ? new BitmapImage(InvalidImageUri) : new BitmapImage();
                }
            }
            else
            {
                bitmapResult = new BitmapImage();
            }
            return bitmapResult;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            byte[] bytesResult = null;
            var bitmapImage = value as BitmapImage;
            if (bitmapImage != null && bitmapImage.UriSource != DefaultImageUri)
            {
                var stream = bitmapImage.StreamSource;
                if (stream != null && stream.Length > 0)
                {
                    using (var binaryReader = new BinaryReader(stream))
                    {
                        bytesResult = binaryReader.ReadBytes((Int32) stream.Length);
                    }
                }
            }
            return bytesResult;
        }
    }
}
