using System.Collections.Generic;
using VContainer;
using VContainer.Unity;
using static Game.Constants;

namespace ItemSystem
{
	public class PopupController : IStartable
    {
		[Inject] private ItemController _itemController;

		private Dictionary<string, PopupView> _popups = new ();
		private string _idCurrentPopup;
		
		public void Start()
		{
			_itemController.SetAction(ActivePopupID + true, ShowPopup);
			_itemController.SetAction(ActivePopupID + false, (id) => HideCurrentPopup());
		}

		public void AddPopupView(string id, PopupView popupView) => _popups.Add(id, popupView);

		public void ShowPopup(string id)
		{
			_idCurrentPopup = id;
			if (_popups.TryGetValue(_idCurrentPopup, out PopupView popup))
			{
				popup.GetAnimator().Play(ShowKey);
			}
		}

		private void HideCurrentPopup()
		{
			if (_popups.TryGetValue(_idCurrentPopup, out PopupView popup))
			{
				popup.GetAnimator().Play(HideKey);
			}
		}
	}
}