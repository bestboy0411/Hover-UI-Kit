﻿namespace Hover.Common.Items {

	/*================================================================================================*/
	public interface IBaseItem {

		event ItemEvents.IsEnabledChangedHandler OnIsEnabledChanged;
		event ItemEvents.IsVisibleChangedHandler OnIsVisibleChanged;

		int AutoId { get; }
		string Id { get; set; }
		string Label { get; set; }
		float Width { get; set; }
		float Height { get; set; }

		bool IsEnabled { get; set; }
		bool IsVisible { get; set; }

	}

}