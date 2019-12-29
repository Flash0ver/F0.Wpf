using F0.ComponentModel;

namespace F0.Wpf.Example.Windows.Data
{
	internal class MainViewModel : ViewModel
	{
		private string convertText;
		public string ConvertText
		{
			get => convertText;
			set => SetProperty(ref convertText, value);
		}

		private string convertBackText;
		public string ConvertBackText
		{
			get => convertBackText;
			set => SetProperty(ref convertBackText, value);
		}

		private string name;
		public string Name
		{
			get => name;
			set => SetProperty(ref name, value);
		}

		private string initials;
		public string Initials
		{
			get => initials;
			set => SetProperty(ref initials, value);
		}


		public MainViewModel()
		{
			convertText = "240";
			convertBackText = "240";
			name = "Stefan Pölz";
			initials = "SP";
		}
	}
}
