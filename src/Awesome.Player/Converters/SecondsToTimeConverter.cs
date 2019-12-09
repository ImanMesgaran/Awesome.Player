using System;
using System.Globalization;
using Xamarin.Forms;

namespace Awesome.Player.Converters
{
	public class SecondsToTimeConverter : IValueConverter
	{
		private const string DEFAULT_FORMAT = @"mm\:ss";

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (!(parameter is string format))
			{
				format = DEFAULT_FORMAT;
			}
			return ((TimeSpan)value).ToString(format);
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
