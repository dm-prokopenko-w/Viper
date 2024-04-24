using System;
using BonusSystem;
using GameplaySystem;
using CellsSystem;
using EnemySystem;
using Game;
using ItemSystem;
using PlayerSystem;
using VContainer;
using VContainer.Unity;
using static Game.Constants;

namespace ContentCellSystem
{
    public class ContentCellController: IStartable, IDisposable
    {
        [Inject] private GameplayController _gameplay;
        [Inject] private PlayerController _player;
        [Inject] private EnemyController _enemy;
        [Inject] private AssetLoader _assetLoader;
        [Inject] private ItemController _itemController;
        [Inject] private BonusController _bonus;

        private float _time;
        private float _startTime;
        private float _step;
        
        private int _bodyCount;
        private int _countToUpdateEnemy;
        private int _countWin;
        
        public void Start()
        {
            _gameplay.OsPlayGame += UpdateGame;

            var dataPlayer = _assetLoader.LoadConfig(PlayerConfigPath) as PlayerConfig;
            _startTime = dataPlayer.TimeStep;
            _step = dataPlayer.Step;
            _countWin = dataPlayer.CountWin;
        }

        public void Dispose()
        {
            _gameplay.OsPlayGame -= UpdateGame;
        }

        private void UpdateGame(bool value)
        {
            _time = _startTime;
            _bodyCount = 1;
            _countToUpdateEnemy = 1;
            
            SetTimeStep();
            SetBodyCount();
            _player.Time = _time;
            _player.UpdateGame(value, HeadConnect);
            _enemy.UpdateGame(value);
            _bonus.UpdateGame(value);
        }

        private void HeadConnect(CellItem cell)
        {
            switch (cell.Type)
            {
                case CellType.None:
                    cell.Type = CellType.Head;
                    break;
                case CellType.Body:
                case CellType.Enemy:
                    _gameplay.GameOver();
                    break;
                case CellType.Bonus:
                    cell.Type = CellType.Head;
                    BonusConnect();
                    break;
            }
        }
        
        private void SetTimeStep() => 
            _itemController.SetText(TextViewID + TextObject.TimeStep, TimeStepCountText + _time);

        private void SetBodyCount() => 
            _itemController.SetText(TextViewID + TextObject.BodyCount, BonusCountText + _bodyCount);

        private void BonusConnect()
        {
            _time -= _step;
            _time = (float)Math.Round(_time, 2);
            _player.Time = _time;

            _bodyCount++;
            _countToUpdateEnemy++;
            
            if (_countToUpdateEnemy > 4)
            {
                _countToUpdateEnemy = 0;
                _enemy.SpawnEnemyPack();
            }

            if (_bodyCount >= _countWin)
            {
                _gameplay.GameWin();
            }
            
            SetTimeStep();
            SetBodyCount();
            _player.SpawnNewBody();
            _bonus.Move();
        }
    }
}
