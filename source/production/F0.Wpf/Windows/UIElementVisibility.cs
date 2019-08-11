using System;
using System.Windows;

namespace F0.Windows
{
	public static class UIElementVisibility
	{
		public static bool? GetIsVisible(UIElement target)
		{
			return (bool?)target.GetValue(IsVisibleProperty);
		}

		public static void SetIsVisible(UIElement target, bool? value)
		{
			target.SetValue(IsVisibleProperty, value);
		}

		public static readonly DependencyProperty IsVisibleProperty = DependencyProperty.RegisterAttached(
			"IsVisible",
			typeof(bool?),
			typeof(UIElementVisibility),
			new PropertyMetadata(default(bool?), IsVisibleChangedCallback));

		private static void IsVisibleChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (d is UIElement uiElement)
			{
				Visibility visibility = ((bool?)e.NewValue) == true
					? Visibility.Visible
					: Visibility.Collapsed;
				uiElement.Visibility = visibility;
			}
			else
			{
				string message = $"'{nameof(UIElement)}' has '{nameof(UIElement.Visibility)}', but the '{nameof(DependencyObject)}' was '{d.GetType()}'.";
				throw new ArgumentException(message, nameof(d));
			}
		}
	}
}
