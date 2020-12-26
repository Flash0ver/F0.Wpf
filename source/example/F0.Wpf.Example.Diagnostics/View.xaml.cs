using System.Windows.Controls;

namespace F0.Wpf.Example.Diagnostics
{
	internal partial class View : UserControl
	{
		public View()
		{
			InitializeComponent();
		}

		public View(object bindingContext)
			: this()
		{
			DataContext = bindingContext;
		}
	}
}
