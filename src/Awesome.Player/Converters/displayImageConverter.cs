using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Awesome.Player.Resources;
using MediaManager.Library;
using Xamarin.Forms;

namespace Awesome.Player.Converters
{
	public class DisplayImageConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var item = value;

			if (item == null)
			{
				//< FontImageSource
				//	FontFamily = "{DynamicResource MaterialFontFamily}"
				//Glyph = "{StaticResource Play}"
				//Size = "44"
				//Color = "{StaticResource PrimaryColor}" />
				var imageSource =  new FontImageSource()
				{
					FontFamily = Device.RuntimePlatform == Device.Android ? "FontAwesome5Brands-Regular-400.otf#Font Awesome 5 Brands Regular" : null,
					Glyph = AppIcons.NoteFont
				};

				return imageSource;
			}
			else
			{
				return item;
			}
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
