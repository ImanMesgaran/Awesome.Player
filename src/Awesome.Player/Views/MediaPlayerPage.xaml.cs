using System;
using System.Collections.Generic;
using System.Diagnostics;
using Awesome.Player.Core.CustomRenderer;
using Awesome.Player.Events;
using Awesome.Player.Models;
using Awesome.Player.ViewModels;
using Prism.Events;
using Prism.Navigation;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Awesome.Player.Views
{
	public partial class MediaPlayerPage : ContentPage, IDestructible
	{
		private IEventAggregator _eventAggregator;
		public MediaPlayerPage(IEventAggregator eventAggregator)
		{
			InitializeComponent();

			_eventAggregator = eventAggregator;

			_eventAggregator.GetEvent<NotesChangedEvent>().Subscribe(NotesListChanged);
			
			//TimeLabel.Text = "";
			//PositionSlider.Value = 0;

			//var viewmodel = this.BindingContext as MediaPlayerPageViewModel;
			//var notes = viewmodel?.Media.Notes;
			//foreach (var note in notes)
			//{
			//	//var newStep = Math.Round(e.NewValue / 100); 
			//	//mySlider.Value = newStep * 100;
			//	//lblText.Text = mySlider.Value.ToString(); 
			//	//lblText.TranslateTo(mySlider.Value * ((mySlider.Width - 40) / mySlider.Maximum), 0, 100);


			//	var lbl = new Label() { Text = note.Text };
			//	lbl.TranslateTo(note.Position,0);

			//	//NotesStackLayout.Children.Add(lbl);

			//	Debug.WriteLine($"************************************************** MIDIA NOTE {note} ***********************************");
			//}
		}

		/*
		private void NotesListChanged(List<Note> notes)
		{
			// clear old notes
			//NotesStackLayout.Children.Clear();

			foreach (var note in notes)
			{
				//var newStep = Math.Round(e.NewValue / 100); 
				//mySlider.Value = newStep * 100;
				//lblText.Text = mySlider.Value.ToString(); 
				//lblText.TranslateTo(mySlider.Value * ((mySlider.Width - 40) / mySlider.Maximum), 0, 100);


				var lbl = new Label() { Text = note.Text };
				//lbl.TranslateTo(note.Position, 0);
				var step = Math.Round(note.Position / 100);
				//lbl.TranslateTo(note.Position * ((PositionSlider.Width - 40) / PositionSlider.Maximum), 0, 100);
				var sposition = note.Position * ((PositionSlider.Width - 40) / PositionSlider.Maximum);
				//lbl.TranslateTo(sposition, 0, 100);
				lbl.TranslationX = sposition;

				var bounds = PositionSlider.Bounds;

				//NotesStackLayout.Children.Add(lbl);

				Debug.WriteLine($"************************************************** MIDIA NOTE: Note Text: {note.Text} *** Note Position: {note.Position} ***********************************");
				Debug.WriteLine($"************************************************** MIDIA NOTE: S POSITION: {sposition} ***********************************");
				Debug.WriteLine($"************************************************** MIDIA NOTE: Slider Bound: ** Right : {bounds.Right} ** Top: {bounds.Top} ** Left: {bounds.Left} ** Bottom: {bounds.Bottom} ***********************************");
				Debug.WriteLine($"************************************************** MIDIA NOTE: Slider ** Width : {PositionSlider.Width} ** Height: {PositionSlider.Height} ***********************************");
				
			}
		}
		*/

		public void Destroy()
		{
			_eventAggregator.GetEvent<NotesChangedEvent>().Unsubscribe(NotesListChanged);
		}


		protected override bool OnBackButtonPressed()
		{
			var viewmodel = this.BindingContext as MediaPlayerPageViewModel;
			viewmodel?.OnBackToMainPageCommand.Execute();

			return true;
		}

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

		private void NotesListChanged(List<Note> notes)
		{
			Notes = notes;

			/*
			//TimeTextPosition = ((SliderWidth - 25) * Position) / Duration;

			foreach (var note in notes)
			{
				//var step = Math.Round(note.Position / 100);
				//var sposition = note.Position * ((PositionSlider.Width - 40) / PositionSlider.Maximum);
				var position = note.Position * ((PositionSlider.Width - 40) / PositionSlider.Maximum);
				//lbl.TranslationX = position;

				var bounds = PositionSlider.Bounds;

				//NotesStackLayout.Children.Add(lbl);

				var smallposition = ((PositionSlider.Width - 25) * note.Position) / PositionSlider.Maximum;

				Debug.WriteLine($"************************************************** MIDIA NOTE: Note Text: {note.Text} *** Note Position: {note.Position} ***********************************");
				Debug.WriteLine($"************************************************** MIDIA NOTE: S POSITION: {position} ***********************************");
				Debug.WriteLine($"************************************************** MIDIA NOTE: Slider Bound: ** Right : {bounds.Right} ** Top: {bounds.Top} ** Left: {bounds.Left} ** Bottom: {bounds.Bottom} ***********************************");
				Debug.WriteLine($"************************************************** MIDIA NOTE: Slider ** Width : {PositionSlider.Width} ** Height: {PositionSlider.Height} ***********************************");

			}
			*/
		}


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

			SKImageInfo info = e.Info;
			SKSurface surface = e.Surface;
			SKCanvas canvas = surface.Canvas;

			canvas.Clear();

			foreach (var note in Notes)
			{
				//var vposition = note.Position * ((PositionSlider.Width - 40) / PositionSlider.Maximum);

				var position = ((PositionSlider.Width - 25) * note.Position) / PositionSlider.Maximum;
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
			
		}

		//void Draw(SKCanvas canvas, int width, int height)
		//{
		//	var nodes = new List<int>() { 5, 25, 40, 70, 90, 100, 120, 170 };

		//	foreach (var n in nodes)
		//	{
		//		var p = new SKPaint
		//		{
		//			Color = SKColors.Red,
		//			IsAntialias = true,
		//			Style = SKPaintStyle.Fill,
		//			StrokeWidth = 2
		//		};

		//		canvas.DrawLine(n, 0, n, 20, p);
		//	}

		//}
	}
}
