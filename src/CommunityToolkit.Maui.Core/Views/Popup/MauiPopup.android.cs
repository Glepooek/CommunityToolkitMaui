﻿using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using Android.Content;
using Android.Views;
using Microsoft.Maui.Platform;
using AView = Android.Views.View;

namespace CommunityToolkit.Maui.Core.Views;

/// <summary>
/// The native implementation of Popup control.
/// </summary>
#if NET10_0_OR_GREATER
#error Remove MauiPopup
#endif
[EditorBrowsable(EditorBrowsableState.Never), Obsolete($"{nameof(MauiPopup)} is no longer used by {nameof(CommunityToolkit)}.{nameof(Maui)} and will be removed in .NET 10")]
public class MauiPopup : Dialog, IDialogInterfaceOnCancelListener
{
	readonly IMauiContext mauiContext;

	/// <summary>
	/// Constructor of <see cref="MauiPopup"/>.
	/// </summary>
	/// <param name="context">An instance of <see cref="Context"/>.</param>
	/// <param name="mauiContext">An instance of <see cref="IMauiContext"/>.</param>
	/// <exception cref="ArgumentNullException">If <paramref name="mauiContext"/> is null an exception will be thrown. </exception>
	public MauiPopup(Context context, IMauiContext mauiContext)
		: base(context)
	{
		this.mauiContext = mauiContext ?? throw new ArgumentNullException(nameof(mauiContext));
	}

	/// <summary>
	/// An instance of the <see cref="IPopup"/>.
	/// </summary>
	public IPopup? VirtualView { get; private set; }

	/// <summary>
	/// Method to initialize the native implementation.
	/// </summary>
	/// <param name="element">An instance of <see cref="IPopup"/>.</param>
	public AView? SetElement(IPopup? element)
	{
		ArgumentNullException.ThrowIfNull(element);

		VirtualView = element;

		if (TryCreateContainer(VirtualView, out var container))
		{
			SubscribeEvents();
		}

		return container;
	}

	/// <summary>
	/// Method to show the Popup.
	/// </summary>
	public override void Show()
	{
		base.Show();

		_ = VirtualView ?? throw new InvalidOperationException($"{nameof(VirtualView)} cannot be null");

		VirtualView.OnOpened();
	}

	/// <summary>
	/// Method triggered when the Popup is dismissed by tapping outside of the Popup.
	/// </summary>
	/// <param name="dialog">An instance of the <see cref="IDialogInterface"/>.</param>
	public void OnDismissedByTappingOutsideOfPopup(IDialogInterface dialog)
	{
		_ = VirtualView ?? throw new InvalidOperationException($"{nameof(VirtualView)} cannot be null");
		_ = VirtualView.Handler ?? throw new InvalidOperationException($"{nameof(VirtualView.Handler)} cannot be null");

		VirtualView.Handler?.Invoke(nameof(IPopup.OnDismissedByTappingOutsideOfPopup));
	}

	/// <summary>
	/// Method to clean up the resources of the <see cref="MauiPopup"/>.
	/// </summary>
	public void CleanUp()
	{
		VirtualView = null;
	}

	/// <inheritdoc/>
	public override bool OnTouchEvent(MotionEvent e)
	{
		if (VirtualView is not null)
		{
			if (VirtualView.CanBeDismissedByTappingOutsideOfPopup &&
				e.Action == MotionEventActions.Up)
			{
				if (Window?.DecorView is AView decorView)
				{
					float x = e.GetX();
					float y = e.GetY();

					if (!(x >= 0 && x <= decorView.Width && y >= 0 && y <= decorView.Height))
					{
						if (IsShowing)
						{
							OnDismissedByTappingOutsideOfPopup(this);
						}
					}
				}
			}
		}

		return !this.IsDisposed() && base.OnTouchEvent(e);
	}

	bool TryCreateContainer(in IPopup popup, [NotNullWhen(true)] out AView? container)
	{
		container = null;

		if (popup.Content is null)
		{
			return false;
		}

		container = popup.Content.ToPlatform(mauiContext);
		SetContentView(container);

		return true;
	}

	void SubscribeEvents()
	{
		SetOnCancelListener(this);
	}

	void IDialogInterfaceOnCancelListener.OnCancel(IDialogInterface? dialog) => OnDismissedByTappingOutsideOfPopup(this);
}