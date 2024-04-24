using System.Collections.Generic;
using CellsSystem;
using Game;
using GameplaySystem;
using VContainer;
using Core;
using ItemSystem;
using UnityEngine;
using VContainer.Unity;
using static Game.Constants;

namespace EnemySystem
{
    public class EnemyController: IStartable
    {
        [Inject] private AssetLoader _assetLoader;
        [Inject] private GameplayController _gameplay;
        [Inject] private ItemController _itemController;
        [Inject] private CellsController _cellsController;

        private List<Enemy> _enemies = new();
        private ObjectPool<Enemy> _pool;

        private int _minCount;
        private int _maxCount;

        private Enemy _prefab;
        
        public void Start()
        {
            var data = _assetLoader.LoadConfig(EnemyConfigPath) as EnemyConfig;
            _minCount = data.MinCountOnStart;
            _maxCount = data.MaxCountOnStart;

            _prefab = data.Prefab;
            
            var inactiveParent = _itemController.GetRectTransform(RectTransformViewID + RectTransformObject.InactiveEnemyParent);
            _pool = new ObjectPool<Enemy>();
            _pool.InitPool(_prefab, inactiveParent);
        }

        public void UpdateGame(bool value)
        {
            if (value)
            {
                SpawnEnemyPack();
            }
            else
            {
                foreach (var enemy in _enemies)
                {
                    _cellsController.ClearCell(enemy.Pos);
                    _pool.Despawn(enemy);
                }
                _enemies.Clear();
            }
        }

        public void SpawnEnemyPack()
        {
            var count = Random.Range(_minCount, _maxCount + 1);
            for (int i = 0; i < count; i++)
            {
                var cell = _cellsController.FindFreeCellItem();
                var enemy = _pool.Spawn(_prefab, cell.Item.transform);
                enemy.Pos = cell.Pos;
                _cellsController.SetToCell(cell, enemy);
                _enemies.Add(enemy);
            }
        }
    }
}