using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace Awesome.Player.Core.Converters
{
	public class StringToUriImageSourceConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var uriString = (string)value;

			if (!string.IsNullOrEmpty(uriString))
			{
				return ImageSource.FromUri(new Uri(uriString));
			}

			return null;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
	}
}
