using System;
using System.Windows;
using F0.ComponentModel;

namespace F0.Wpf.Example.Windows
{
	internal class MainViewModel : ViewModel
	{
		private bool? isVisible;
		public bool? IsVisible
		{
			get => isVisible;
			set => SetField(ref isVisible, value);
		}

		private Visibility visibility;
		public Visibility Visibility
		{
			get => visibility;
			set => SetField(ref visibility, value);
		}

		public Uri IconUrl { get; }

		public MainViewModel()
		{
			IsVisible = true;
			IconUrl = new Uri("https://raw.githubusercontent.com/Flash0ver/F0/master/Branding/NuGet/F0.Wpf.png");
		}
	}
}
