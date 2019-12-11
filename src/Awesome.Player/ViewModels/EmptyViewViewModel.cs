using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using Awesome.Player.Core.Infrastructure;
using Awesome.Player.Models;
using Prism.Navigation;
using Prism.Services;
using Prism.Services.Dialogs;
using Realms;

namespace Awesome.Player.ViewModels
{
	public class EmptyViewViewModel : BaseProvider
	{
		public string MediaUri { get; set; }

		private string _noteText = "";
		public string NoteText
		{
			get { return _noteText; }
			set { SetProperty(ref _noteText, value); }
		}

		private double _notePosition;
		public double NotePosition
		{
			get { return _notePosition; }
			set { SetProperty(ref _notePosition, value); }
		}

		private double _noteDuration = 3;
		public double NoteDuration
		{
			get { return _noteDuration; }
			set { SetProperty(ref _noteDuration, value); }
		}

		private DelegateCommand _addNoteToMediaCommand;
		public DelegateCommand AddNoteToMediaCommand =>
			_addNoteToMediaCommand ?? (_addNoteToMediaCommand = new DelegateCommand(ExecuteAddNoteToMediaCommand));

		void ExecuteAddNoteToMediaCommand()
		{
			if (string.IsNullOrEmpty(NoteText) || NoteDuration <= 0) return;

			var realm = Realm.GetInstance();

			//var notes = realm.All<Note>()
			//	.FirstOrDefault(x => x.MediaUri == MediaUri);

			realm.Write(() =>
			{
				realm.Add(new Note()
				{
					MediaUri = MediaUri,
					Position = Math.Floor(NotePosition),
					Duration = Math.Floor(NoteDuration),
					Text = NoteText
				});
			});

			var songNotes = realm.All<Note>()
				.Where(x=>x.MediaUri == MediaUri);

			NavigationService.GoBackAsync();
		}

		public EmptyViewViewModel(
			INavigationService navigationService,
			IPageDialogService pageDialogService,
			IDeviceService deviceService) : base(navigationService, pageDialogService, deviceService)
		{

		}

		public override void OnNavigatedTo(INavigationParameters parameters)
		{
			if (parameters.ContainsKey("position"))
			{
				NotePosition = (double)parameters["position"];
			}

			if (parameters.ContainsKey("mediaUri"))
			{
				MediaUri = (string) parameters["mediaUri"];
			}
		}
	}
}
