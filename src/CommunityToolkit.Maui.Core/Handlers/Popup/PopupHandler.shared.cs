using System.ComponentModel;

namespace CommunityToolkit.Maui.Core.Handlers;

/// <summary>
/// Handler Popup control
/// </summary>
#if NET10_0_OR_GREATER
#error Remove PopupHandler
#endif
[EditorBrowsable(EditorBrowsableState.Never), Obsolete($"{nameof(PopupHandler)} is no longer used by {nameof(CommunityToolkit)}.{nameof(Maui)} and will be removed in .NET 10")]
public partial class PopupHandler
{
	/// <summary>
	/// PropertyMapper for Popup Control
	/// </summary>
	public static IPropertyMapper<IPopup, PopupHandler> PopUpMapper = new PropertyMapper<IPopup, PopupHandler>(ElementMapper)
	{
		[nameof(IPopup.Anchor)] = MapAnchor,
		[nameof(IPopup.Color)] = MapColor,
		[nameof(IPopup.Size)] = MapSize,
		[nameof(IPopup.VerticalOptions)] = MapSize,
		[nameof(IPopup.HorizontalOptions)] = MapSize,
		[nameof(IPopup.CanBeDismissedByTappingOutsideOfPopup)] = MapCanBeDismissedByTappingOutsideOfPopup
	};

	/// <summary>
	/// <see cref ="CommandMapper"/> for Popup Control.
	/// </summary>
	public static CommandMapper<IPopup, PopupHandler> PopUpCommandMapper = new(ElementCommandMapper)
	{
#if !(IOS || MACCATALYST)
		[nameof(IPopup.OnOpened)] = MapOnOpened,
		[nameof(IPopup.OnClosed)] = MapOnClosed,
#endif
		[nameof(IPopup.OnDismissedByTappingOutsideOfPopup)] = MapOnDismissedByTappingOutsideOfPopup
	};

	/// <summary>
	/// Constructor for <see cref="PopupHandler"/>.
	/// </summary>
	/// <param name="mapper">Custom instance of <see cref="PropertyMapper"/>, if it's null the <see cref="PopUpMapper"/> will be used</param>
	/// <param name="commandMapper">Custom instance of <see cref="CommandMapper"/>, if it's null the <see cref="PopUpCommandMapper"/> will be used</param>
	public PopupHandler(IPropertyMapper? mapper, CommandMapper? commandMapper)
		: base(mapper ?? PopUpMapper, commandMapper ?? PopUpCommandMapper)
	{
	}

	/// <summary>
	/// Default Constructor for <see cref="PopupHandler"/>.
	/// </summary>
	public PopupHandler()
		: base(PopUpMapper, PopUpCommandMapper)
	{
	}
}