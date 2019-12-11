using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using Awesome.Player.Core.Infrastructure;
using Prism.Navigation;
using Prism.Services;
using Prism.Services.Dialogs;

namespace Awesome.Player.ViewModels
{
	public class NoteAddViewViewModel : BaseProvider, IDialogAware
	{
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

		private double _noteDuration;
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

		}

		public NoteAddViewViewModel(
			INavigationService navigationService, 
			IPageDialogService pageDialogService,
			IDeviceService deviceService) : base(navigationService, pageDialogService, deviceService)
		{

		}

		public bool CanCloseDialog() => true;

		public void OnDialogClosed()
		{
			
		}

		public void OnDialogOpened(IDialogParameters parameters)
		{
			if (parameters.ContainsKey("position"))
			{
				NotePosition = parameters.GetValue<double>("position");
			}
		}

		public event Action<IDialogParameters> RequestClose;
	}
}
