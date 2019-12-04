using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Awesome.Player.Core.CustomRenderer
{
	public class CustomSlider : Slider
	{
		public static readonly BindableProperty ThumbLeftProperty = BindableProperty.Create(
			nameof(ThumbLeft),
			typeof(double),
			typeof(CustomSlider),
			0.0d);

		public double ThumbLeft
		{
			get { return (double)GetValue(ThumbLeftProperty); }
			set { SetValue(ThumbLeftProperty, value); }
		}
	}
}
