using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Awesome.Player.Droid.CustomRenderer;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(HorizontalScrollView), typeof(HorizontalScrollViewRenderer))]
namespace Awesome.Player.Droid.CustomRenderer
{
	public class HorizontalScrollViewRenderer : ScrollViewRenderer
	{
		protected override void OnElementChanged(VisualElementChangedEventArgs e)
		{
			base.OnElementChanged(e);

			var element = e.NewElement as Core.CustomRenderer.HorizontalScrollView;
			element?.Render();

			if (e.OldElement != null || this.Element == null)
				return;

			if (e.OldElement != null)
				e.OldElement.PropertyChanged -= OnElementPropertyChanged;

			e.NewElement.PropertyChanged += OnElementPropertyChanged;
		}

		protected void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (ChildCount > 0)
			{
				GetChildAt(0).HorizontalScrollBarEnabled = false;
				GetChildAt(0).VerticalScrollBarEnabled = false;
			}
		}
	}
}