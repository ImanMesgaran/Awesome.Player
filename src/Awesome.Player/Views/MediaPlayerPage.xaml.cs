using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Awesome.Player.Core.CustomRenderer;
using Awesome.Player.Events;
using Awesome.Player.Models;
using Awesome.Player.ViewModels;
using Prism.Events;
using Prism.Navigation;
using Realms;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Awesome.Player.Views
{
	public partial class MediaPlayerPage : ContentPage, IDestructible
	{
		private IEventAggregator _eventAggregator;

		//private IEnumerable<Note> Notes { get; }
		//private IDisposable token;
		//public List<Note> Notes { get ; set; }

		private List<Note> _notes = new List<Note>();
		public List<Note> Notes
		{
			get { return _notes; }
			set
			{
				_notes = value;
				canvasView.InvalidateSurface();
			}
		}

		public double SliderWidth { get; set; }
		public double SliderMaximum { get; set; }

		public MediaPlayerPage(IEventAggregator eventAggregator)
		{
			InitializeComponent();

			_eventAggregator = eventAggregator;

			_eventAggregator.GetEvent<NotesChangedEvent>().Subscribe(NotesListChanged);
			_eventAggregator.GetEvent<MixedEvent>().Subscribe(MixedNotesListChanged);

			var media = ((MediaPlayerPageViewModel) BindingContext).Media;

			var realm = Realm.GetInstance();
			var notes = realm
				.All<Note>()
				.Where(x => x.MediaUri == media.MediaUri);
				
		}

		private void MixedNotesListChanged((List<Note> notes, double PositionSliderWidth, double PositionSliderMaximum) obj)
		{
			Notes = obj.notes;
			SliderMaximum = obj.PositionSliderMaximum;
			SliderWidth = obj.PositionSliderWidth;
		}


		protected override bool OnBackButtonPressed()
		{
			var viewmodel = this.BindingContext as MediaPlayerPageViewModel;
			viewmodel?.OnBackToMainPageCommand.Execute();

			return true;
		}
		
		private void NotesListChanged(List<Note> notes)
		{
			Notes = notes;
		}

		//public SKSurface surface { get; set; }

		private void CanvasView_OnPaintSurface(object sender, SKPaintSurfaceEventArgs e)
		{
			// Get Metrics
			var mainDisplayInfo = DeviceDisplay.MainDisplayInfo;

			// Orientation (Landscape, Portrait, Square, Unknown)
			var orientation = mainDisplayInfo.Orientation;

			// Rotation (0, 90, 180, 270)
			var rotation = mainDisplayInfo.Rotation;

			// Width (in pixels)
			var width = mainDisplayInfo.Width;

			// Height (in pixels)
			var height = mainDisplayInfo.Height;

			// Screen density
			var density = mainDisplayInfo.Density;

			var media = ((MediaPlayerPageViewModel)BindingContext).Media;


			SKImageInfo info = e.Info;
			SKSurface surface = e.Surface;
			SKCanvas canvas = surface.Canvas;

			//using (SKSurface surface = e.Surface)
			//using (SKCanvas canvas = surface.Canvas)
			//{
				canvas.Clear();

				foreach (var note in Notes)
				{
					//var vposition = note.Position * ((PositionSlider.Width - 40) / PositionSlider.Maximum);

					var position = ((SliderWidth - 25) * note.Position) / media.Duration.TotalSeconds;
					//var position = ((SliderWidth - 25) * note.Position) / SliderMaximum;
					//var position = ((PositionSlider.Width - 25) * note.Position) / PositionSlider.Maximum;
					//var position = ((PositionSlider.Width) * note.Position) / PositionSlider.Maximum;

					//var sliderBounds = PositionSlider.Bounds;
					position *= density;

					// position slider bounds
					var x = PositionSlider.X;
					var y = PositionSlider.Y;
					var parent = PositionSlider.ParentView;
					while (parent != null)
					{
						x += parent.X;
						y += parent.Y;
						parent = parent.ParentView;
					}

					// skiasharp surface bounds
					var sx = canvasView.X;
					var sy = canvasView.Y;
					var sparent = canvasView.ParentView;
					while (sparent != null)
					{
						sx += sparent.X;
						sy += sparent.Y;
						sparent = sparent.ParentView;
					}



					using (var p = new SKPaint
					{
						Color = SKColors.White,
						IsAntialias = true,
						Style = SKPaintStyle.Fill,
						StrokeWidth = 8
					})
					{
						canvas.DrawLine((float)position, 0, (float)position, 20, p);
					}
				}
			//}
		}

		protected override void OnAppearing()
		{
			//canvasView.InvalidateSurface();
		}

		public void Destroy()
		{
			_eventAggregator.GetEvent<NotesChangedEvent>().Unsubscribe(NotesListChanged);
			_eventAggregator.GetEvent<MixedEvent>().Unsubscribe(MixedNotesListChanged);
			videoView.Dispose();
			//canvasView.PaintSurface -= CanvasView_OnPaintSurface;
			//surface.Dispose();
			
			GC.Collect();
		}

	}
}
