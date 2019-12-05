using System;
using System.Collections.Generic;
using System.Diagnostics;
using Awesome.Player.Core.CustomRenderer;
using Awesome.Player.Events;
using Awesome.Player.Models;
using Awesome.Player.ViewModels;
using Prism.Events;
using Prism.Navigation;
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

		public void Destroy()
		{
			_eventAggregator.GetEvent<NotesChangedEvent>().Unsubscribe(NotesListChanged);
		}

		TimeSpan pos, dur;

		private void PositionSlider_OnValueChanged(object sender, ValueChangedEventArgs e)
		{
			pos = videoView.Position;
			dur = videoView.Duration;

			if (pos.Ticks == 0 || dur.Ticks == 0)
			{
				pos = new TimeSpan(1);
				dur = new TimeSpan(100);
				//return;
			}

			var formula = (pos.Ticks * 100) / dur.Ticks;
			
			Debug.WriteLine($"************************************************** Current position in percent: {formula} ***********************************");
			
			TimeLabel.Text = $"{pos:mm}:{pos:ss}|{dur:mm}:{dur:ss}";

			var x = ((PositionSlider.Width - 25) * pos.Ticks) / dur.Ticks;
			TimeLabel.TranslateTo(x, 0, 100);
			Debug.WriteLine($"************************************************** current thumb coordinate: {x} ***********************************");

		}
	}
}
