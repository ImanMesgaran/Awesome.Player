using System;
using System.Globalization;
using Awesome.Player.Models;
using MediaManager.Library;
using Xamarin.Forms;

namespace Awesome.Player.Converters
{
	public class MediaFormatConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var ext = ((MediaModel)value).FileExtension;
			
			switch (ext)
            {
                // video 
                case "webm":
                case "mkv":
                case "mp4":
                case "m4p":
                case "m4v":
                case "mov":
                case "mpg":
                case "mpeg":
                case "mp2":
                case "mpe":
                case "mpv":
                case "m2v":
                case "wmv":
                case "avi":
                case "3gp":
                case "flv":

                    return true;

                // audio 
                case "mp3":
                case "wav":
                case "wma":
                case "m4a":
                case "m4b":
                case "ogg":
                case "oga":
                case "mogg":
                case "ra":
                case "rm":
                case "flac":

                    return false;
                    
                // file
                default:

                    return false;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
