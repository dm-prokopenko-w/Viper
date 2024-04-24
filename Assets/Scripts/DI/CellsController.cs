using System;
using System.Collections.Generic;
using Core;
using Game;
using ItemSystem;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using static Core.Сalculations;
using static Game.Constants;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace CellsSystem
{
    public class CellsController : IStartable
    {
        [Inject] private AssetLoader _assetLoader;
        [Inject] private ItemController _itemController;

        private List<CellItem> _cellItems = new();

        private List<int> _horizontalList = new();
        private List<int> _verticalList = new();
        private Vector2Int _lastIndex;
        private float _cellSize;
        private int _horCount;
        private int _verCount;

        public void Start()
        {
            var data = _assetLoader.LoadConfig(CellsConfigPath) as CellsConfig;
            _horCount = data.HorizontalCountCells;
            _verCount = data.VerticalCountCells;

            InitList();
            SpawnCells(data.Prefab);
            InitGrid(data.HorizontalCountCells);
        }

        public void SetToCell(CellItem cell, Character character)
        {
            cell.Type = character.Type;
            character.transform.SetParent(cell.Item.transform);
            character.SetSize(_cellSize);
        }

        public CellItem FindCellItem(Vector2Int pos) => _cellItems.Find(x => x.Pos == pos);

        public CellItem FindFreeCellItem()
        {
            int num = 0;

            for (int i = 0; i < _cellItems.Count; i++)
            {
                num = Random.Range(0, _cellItems.Count);

                if (_cellItems[num].Type == CellType.None)
                {
                    return _cellItems[num];
                }
            }
            return _cellItems[num];
        }

        public void ClearCell(Vector2Int pos)
        {
            var cell = FindCellItem(pos);
            if (cell == null) return;
            cell.Type = CellType.None;
        }

        public Vector2Int GetNextPos(Vector2Int oldPos, Dir dir, Action onDespawn)
        {
            Vector2Int newPos = Vector2Int.zero;
            switch (dir)
            {
                case Dir.Up:
                    newPos = new Vector2Int(oldPos.x, oldPos.y - 1);
                    if (!IsUseZero(_verCount) && newPos.y == 0)
                    {
                        newPos = new Vector2Int(oldPos.x, newPos.y - 1);
                    }

                    if (newPos.y < -GetSize(_verCount))
                    {
                        onDespawn?.Invoke();
                    }

                    break;
                case Dir.Down:
                    newPos = new Vector2Int(oldPos.x, oldPos.y + 1);
                    if (!IsUseZero(_verCount) && newPos.y == 0)
                    {
                        newPos = new Vector2Int(oldPos.x, newPos.y + 1);
                    }

                    if (newPos.y > GetSize(_verCount))
                    {
                        onDespawn?.Invoke();
                    }

                    break;
                case Dir.Left:
                    newPos = new Vector2Int(oldPos.x - 1, oldPos.y);
                    if (!IsUseZero(_horCount) && newPos.x == 0)
                    {
                        newPos = new Vector2Int(newPos.x - 1, oldPos.y);
                    }

                    if (newPos.x < -GetSize(_horCount))
                    {
                        onDespawn?.Invoke();
                    }

                    break;
                case Dir.Right:
                    newPos = new Vector2Int(oldPos.x + 1, oldPos.y);
                    if (!IsUseZero(_horCount) && newPos.x == 0)
                    {
                        newPos = new Vector2Int(newPos.x + 1, oldPos.y);
                    }

                    if (newPos.x > GetSize(_horCount))
                    {
                        onDespawn?.Invoke();
                    }

                    break;
            }

            return newPos;
        }

        private void InitGrid(int count)
        {
            var grid = _itemController.GetGridLayoutGroup(GridLayoutViewID);
            _cellSize = (AspectRatio - grid.padding.left - grid.padding.right) / count;
            grid.cellSize = new Vector2(_cellSize, _cellSize);
        }

        private void InitList()
        {
            _horizontalList = GetLineList(_horCount);
            _verticalList = GetLineList(_verCount);
        }

        private void SpawnCells(Cell prefab)
        {
            var cellsParent = _itemController.GetRectTransform(RectTransformViewID + RectTransformObject.CellsParent);
            cellsParent.sizeDelta = new Vector2(AspectRatio, AspectRatio);

            foreach (var h in _horizontalList)
            {
                foreach (var v in _verticalList)
                {
                    var obj = Object.Instantiate(prefab, cellsParent);
                    CellItem item = new()
                    {
                        Type = CellType.None,
                        Item = obj,
                        Pos = new Vector2Int(v, h)
                    };
                    obj.name = "v: " + v + "; h: " + h + ";";
                    _cellItems.Add(item);
                }
            }
        }
    }

    public class CellItem
    {
        public CellType Type;
        public Vector2Int Pos;
        public Cell Item;
    }
}