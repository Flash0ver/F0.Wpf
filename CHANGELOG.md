# F0.Wpf
CHANGELOG

## vNext
- Added target framework: `.NET 5` (Windows).
- Added annotations for _nullable reference types_.
- Package: Embed icon (fixed _NuGet Warning NU5048_), keep fallback icon URL.

## v0.7.0 (2020-04-03)
- Added `Windows.Data.LogicalNegationConverter`, a value converter that applies NOT operator to a boolean value.
- Added `Windows.Data.NumericNegationConverter`, converting integral and floating-point numeric values to its negation.

## v0.6.0 (2019-12-31)
- Added target framework: `.NET Core 3.1`.

## v0.5.0 (2019-10-31)
- Added `Windows.Data.InverseValueConverter` inverting calls to a `System.Windows.Data.IValueConverter`.

## v0.4.0 (2019-07-31)
- Added `IsVisible` attached property, targeting `System.Windows.UIElement`, complying with behavior of `System.Windows.Controls.BooleanToVisibilityConverter`.

## v0.3.0 (2019-04-30)
- Changed target framework from `.NET Framework 4.5.2` to `.NET Framework 4.7.2`.
- Package: Use license expression instead of deprecated license URL (fixed _NuGet Warning NU5125_).

## v0.2.0 (2018-12-21)
- Added `Windows.Data.CompositeValueConverter` chaining multiple `System.Windows.Data.IValueConverter`.

## v0.1.0 (2018-09-18)
- Added `Diagnostics.DataBindingTraceListener` detecting data-binding errors.
