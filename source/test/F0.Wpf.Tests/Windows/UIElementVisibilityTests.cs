using System;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using F0.Windows;
using Xunit;

namespace F0.Tests.Windows
{
	public class UIElementVisibilityTests
	{
		[Fact]
		public void CheckAttachedProperty_IsVisible()
		{
			DependencyProperty attachedProperty = UIElementVisibility.IsVisibleProperty;

			Assert.Equal(typeof(UIElementVisibility), attachedProperty.OwnerType);
			Assert.Equal(typeof(bool?), attachedProperty.PropertyType);
			Assert.Equal("IsVisible", attachedProperty.Name);
			Assert.False(attachedProperty.ReadOnly);
		}

		[Fact]
		public void CheckAttachedProperty_IsVisible_GetAccessor()
		{
			BindingFlags bindingAttr = BindingFlags.Static | BindingFlags.Public;
			MethodInfo get = typeof(UIElementVisibility).GetMethod(nameof(UIElementVisibility.GetIsVisible), bindingAttr);
			ParameterInfo[] parameters = get.GetParameters();
			ParameterInfo target = parameters[0];

			string propertyName = "IsVisible";

			Assert.Equal($"Get{propertyName}", get.Name);
			Assert.Equal(typeof(bool?), get.ReturnType);
			Assert.Single(parameters);
			Assert.Equal(typeof(UIElement), target.ParameterType);
		}

		[Fact]
		public void CheckAttachedProperty_IsVisible_SetAccessor()
		{
			BindingFlags bindingAttr = BindingFlags.Static | BindingFlags.Public;
			MethodInfo set = typeof(UIElementVisibility).GetMethod(nameof(UIElementVisibility.SetIsVisible), bindingAttr);
			ParameterInfo[] parameters = set.GetParameters();
			ParameterInfo target = parameters[0];
			ParameterInfo value = parameters[1];

			string propertyName = "IsVisible";

			Assert.Equal($"Set{propertyName}", set.Name);
			Assert.Equal(typeof(void), set.ReturnType);
			Assert.Equal(2, parameters.Length);
			Assert.Equal(typeof(UIElement), target.ParameterType);
			Assert.Equal(typeof(bool?), value.ParameterType);
		}

		[Fact]
		public void CheckAttachedProperty_IsVisible_PropertyChangedCallback()
		{
			var uiElement = new UIElement();
			IValueConverter converter = new BooleanToVisibilityConverter();

			Assert.Null(UIElementVisibility.GetIsVisible(uiElement));

			UIElementVisibility.SetIsVisible(uiElement, false);
			Assert.False(UIElementVisibility.GetIsVisible(uiElement));
			Assert.Equal(Visibility.Collapsed, uiElement.Visibility);
			Assert.Equal(Visibility.Collapsed, converter.Convert(false, default, default, default));

			UIElementVisibility.SetIsVisible(uiElement, null);
			Assert.Null(UIElementVisibility.GetIsVisible(uiElement));
			Assert.Equal(Visibility.Collapsed, uiElement.Visibility);
			Assert.Equal(Visibility.Collapsed, converter.Convert(null, default, default, default));

			UIElementVisibility.SetIsVisible(uiElement, true);
			Assert.True(UIElementVisibility.GetIsVisible(uiElement));
			Assert.Equal(Visibility.Visible, uiElement.Visibility);
			Assert.Equal(Visibility.Visible, converter.Convert(true, default, default, default));
		}

		[Fact]
		public void CheckAttachedProperty_IsVisible_UIElement_HasVisibility()
		{
			DependencyProperty attachedProperty = UIElementVisibility.IsVisibleProperty;
			PropertyChangedCallback propertyChangedCallback = attachedProperty.DefaultMetadata.PropertyChangedCallback;

			var obj = new DependencyObject();
			var eventArgs = new DependencyPropertyChangedEventArgs();

			Assert.Throws<ArgumentException>("d", () => propertyChangedCallback(obj, eventArgs));
		}
	}
}
