using System;
using ItemSystem;
using UnityEngine;
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
        
        public void Start()
        {
            _itemController.SetAction(ButtonViewID + ButtonObject.StartGame, StartGame);
        }

        private void StartGame()
        {
            _popupController.ActivePopup(PopupsID.Start.ToString(), false);
            UpdateGame(true);
        }
        
        private void UpdateGame(bool value) => OsPlayGame?.Invoke(value);

        public void GameOver()
        {
            OnGameOver?.Invoke();

            _popupController.ActivePopup(PopupsID.Lose.ToString(), true);
            UpdateGame(false);
        }

        public void GameWin()
        {
            OnGameWin?.Invoke();

            UpdateGame(false);
            _popupController.ActivePopup(PopupsID.Win.ToString(), true);
        }
    }
}