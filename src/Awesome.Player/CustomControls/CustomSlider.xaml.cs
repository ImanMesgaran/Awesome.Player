using System.Collections.Generic;
using Awesome.Player.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Awesome.Player.CustomControls
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CustomSlider : ContentView
	{
		public static readonly BindableProperty NotesListProperty = BindableProperty.Create(
			propertyName:"NotesList",
			returnType:typeof(List<Note>),
			declaringType:typeof(CustomSlider),
			defaultValue:new List<Note>(),
			defaultBindingMode:BindingMode.TwoWay,
			propertyChanged:NotesListPropertyChanged);

		public List<Note> NotesList
		{
			get { return (List<Note>) base.GetValue(NotesListProperty); }
			set { base.SetValue(NotesListProperty, value); }
		}

		private static void NotesListPropertyChanged(BindableObject bindable, object oldvalue, object newvalue)
		{
			var control = (CustomSlider)bindable;
			//NotesStackLayout = new StackLayout();

			//control.title.Text = newValue.ToString();
		}

		public CustomSlider()
		{
			InitializeComponent();
		}
	}
}