using CellsSystem;
using Game;
using UnityEngine;
using static Game.Constants;
using VContainer;
using VContainer.Unity;

namespace BonusSystem
{
    public class BonusController : IStartable
    {
        [Inject] private AssetLoader _assetLoader;
        [Inject] private CellsController _cellsController;

        private Bonus _bonus;

        public void Start()
        {
            var data = _assetLoader.LoadConfig(BonusConfigPath) as BonusConfig;
            _bonus = Object.Instantiate(data.Prefab);
            _bonus.gameObject.SetActive(false);
        }

        public void UpdateGame(bool value)
        {
            _bonus.gameObject.SetActive(value);

            if (value)
            {
                Move();
            }
            else
            {
                _cellsController.ClearCell(_bonus.Pos);
            }
        }

        public void Move()
        {
            var cell = _cellsController.FindFreeCellItem();
            _bonus.Pos = cell.Pos;
            _cellsController.SetToCell(cell, _bonus);
        }
    }
}
