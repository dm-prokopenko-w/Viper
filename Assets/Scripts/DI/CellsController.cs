using System.Collections.Generic;
using Core;
using Game;
using ItemSystem;
using PlayerSystem;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using static Game.Constants;
using Object = UnityEngine.Object;

namespace CellsSystem
{
    public class CellsController : IStartable
    {
        [Inject] private AssetLoader _assetLoader;
        [Inject] private ItemController _itemController;
        
        private List<CellItem> _cellItems = new();
        private int _size;

        private List<int> _horizontalList = new();
        private List<int> _verticalList = new();
        private Vector2Int _lastIndex;
        private bool _isUseZero;
        private float _cellSize;
        
        public void Start()
        {
            var data = _assetLoader.LoadConfig(CellsConfigPath) as CellsConfig;
            SpawnCells(data.HorizontalCountCells, data.VerticalCountCells, data.Prefab);
            InitGrid(data.HorizontalCountCells);
            _isUseZero = data.HorizontalCountCells % 2 > 0;
        }

        public void SetToCell(CellItem cell, Character character)
        {
            character.transform.SetParent(cell.Item.transform);
            character.SetSize(_cellSize);
        }

        public CellItem FindCellItem(Vector2Int pos) => _cellItems.Find(x => x.Pos == pos);

        public void ClearCell(Vector2Int pos)
        {
            var cell = FindCellItem(pos);
            if(cell == null) return;
            cell.Type = CellType.None;
        }

        public Vector2Int GetNextPos(Vector2Int oldPos, Dir dir)
        {
            Vector2Int newPos = Vector2Int.zero;
            switch (dir)
            {
                case Dir.Up:
                    newPos = new Vector2Int(oldPos.x, oldPos.y - 1);
                    if (!_isUseZero && newPos.y == 0)
                    {
                        newPos = new Vector2Int(oldPos.x, newPos.y - 1);
                    }
                    if (newPos.y < -_size)
                    {
                        newPos = new Vector2Int(oldPos.x, _size);
                    }
                    break;
                case Dir.Down:
                    newPos = new Vector2Int(oldPos.x, oldPos.y + 1);
                    if (!_isUseZero && newPos.y == 0)
                    {
                        newPos = new Vector2Int(oldPos.x, newPos.y + 1);
                    }
                    if (newPos.y > _size)
                    {
                        newPos = new Vector2Int(oldPos.x, -_size);
                    }
                    break;
                case Dir.Left:
                    newPos = new Vector2Int(oldPos.x - 1, oldPos.y);
                    if (!_isUseZero && newPos.x == 0)
                    {
                        newPos = new Vector2Int(newPos.x - 1, oldPos.y);
                    }

                    if (newPos.x < -_size)
                    {
                        newPos = new Vector2Int(_size, oldPos.y);
                    }
                    break;
                case Dir.Right:
                    newPos = new Vector2Int(oldPos.x + 1, oldPos.y);
                    if (!_isUseZero && newPos.x == 0)
                    {
                        newPos = new Vector2Int(newPos.x + 1, oldPos.y);
                    }

                    if (newPos.x > _size)
                    {
                        newPos = new Vector2Int(-_size, oldPos.y);
                    }
                    break;
            }

            return newPos;
        }
        
        private void InitGrid(int count)
        {
            var grid = _itemController.GetGridLayoutGroup(GridLayoutViewID);
            _cellSize = (Screen.width - grid.padding.left - grid.padding.right) / count;
            grid.cellSize = new Vector2(_cellSize, _cellSize);
        }
        
        private void SpawnCells(int hor, int ver, Cell prefab)
        {
            _horizontalList = GetLineList(hor);
            _verticalList = GetLineList(ver);
            var cellsParent = _itemController.GetRectTransform(RectTransformViewID + RectTransformObject.CellsParent);

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
        
        private List<int> GetLineList(int rowCount)
        {
            if (rowCount % 2 > 0)
            {
                _size = (rowCount - 1) / 2;
            }
            else
            {
                _size = rowCount / 2;
            }

            List<int> nums = new();
            for (int i = -_size; i < 0; i++)
            {
                nums.Add(i);
            }

            if (rowCount % 2 > 0)
            {
                nums.Add(0);
            }

            for (int i = 1; i < _size + 1; i++)
            {
                nums.Add(i);
            }

            return nums;
        }
    }

    public enum Dir
    {
        Up,
        Right,
        Down,
        Left
    }
    
    public class CellItem
    {
        public CellType Type;
        public Vector2Int Pos;
        public Cell Item;
    }
}