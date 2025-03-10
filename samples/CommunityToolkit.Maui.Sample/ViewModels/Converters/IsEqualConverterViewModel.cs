﻿using CommunityToolkit.Mvvm.ComponentModel;

namespace CommunityToolkit.Maui.Sample.ViewModels.Converters;

public partial class IsEqualConverterViewModel : BaseViewModel
{
	[ObservableProperty]
	public partial string InputValue { get; set; } = string.Empty;
}