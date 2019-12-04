using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Awesome.Player.Core.CustomRenderer;
using Awesome.Player.Droid.CustomRenderer;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

//[assembly: ExportRenderer(typeof(CustomSlider), typeof(CustomSliderRenderer))]
namespace Awesome.Player.Droid.CustomRenderer
{
	public class CustomSliderRenderer : SliderRenderer
	{
		private CustomSlider view;
		protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Slider> e)
		{
			base.OnElementChanged(e);
			if (e.OldElement != null || e.NewElement == null)
				return;
			view = (CustomSlider)Element;
			//if (!string.IsNullOrEmpty(view.ThumbImage))
			//{    // Set Thumb Icon  
			//	Control.SetThumb(Resources.GetDrawable(view.ThumbImage));
			//}
			//else if (view.ThumbColor != Xamarin.Forms.Color.Default ||
			//         view.MaxColor != Xamarin.Forms.Color.Default ||
			//         view.MinColor != Xamarin.Forms.Color.Default)
			//	Control.Thumb.SetColorFilter(view.ThumbColor.ToAndroid(), PorterDuff.Mode.SrcIn);
			//Control.ProgressTintList = Android.Content.Res.ColorStateList.ValueOf(view.MinColor.ToAndroid());
			//Control.ProgressTintMode = PorterDuff.Mode.SrcIn;
			////this is for Maximum Slider line Color  
			//Control.ProgressBackgroundTintList = Android.Content.Res.ColorStateList.ValueOf(view.MaxColor.ToAndroid());
			//Control.ProgressBackgroundTintMode = PorterDuff.Mode.SrcIn;
			
			view.ThumbLeft = Control.Thumb.Bounds.Left;

		}
	}

}