using System;
using ItemSystem;
using VContainer;
using VContainer.Unity;
using static Game.Constants;

namespace GameplaySystem
{
    public class GameplayController : IStartable
    {
        [Inject] private ItemController _itemController;
        [Inject] private PopupController _popupController;

        public Action<bool> OsPlayGame;
        public Action OnGameOver;
        public Action OnGameWin;

        private bool _isPlayGame = false;

        public void Start()
        {
            _itemController.SetAction(ButtonViewID + ButtonObject.StartGame, () => UpdateGame(true));
        }

        private void UpdateGame(bool value)
        {
            _isPlayGame = value;

            OsPlayGame?.Invoke(_isPlayGame);

            if (_isPlayGame)
            {
                _itemController.SetActiveBtn(ButtonViewID + ButtonObject.StartGame, false);
            }
            else
            {
                _itemController.SetActiveBtn(ButtonViewID + ButtonObject.StartGame, true);
            }
        }

        public void GameOver()
        {
            OnGameOver?.Invoke();

            _popupController.ShowPopup(PopupsID.Lose.ToString());
            UpdateGame(false);
        }

        public void GameWin()
        {
            OnGameWin?.Invoke();

            UpdateGame(false);
            _popupController.ShowPopup(PopupsID.Win.ToString());
        }
    }
}