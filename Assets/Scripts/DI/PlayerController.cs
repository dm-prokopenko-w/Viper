using System;
using System.Collections;
using System.Collections.Generic;
using CellsSystem;
using Core;
using CoroutineSystem;
using Game;
using GameplaySystem;
using ItemSystem;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using static Game.Constants;
using Object = UnityEngine.Object;

namespace PlayerSystem
{
    public class PlayerController: IStartable
    {
        [Inject] private GameplayController _gameplay;
        [Inject] private AssetLoader _assetLoader;
        [Inject] private ItemController _itemController;
        [Inject] private CellsController _cellsController;
        [Inject] private CoroutineHandler _coroutineHandler;

        public float Time { get; set; }
        
        private ObjectPool<PlayerBody> _pool;

        private PlayerBody _bodyPrefab;
        private PlayerHead _head;
        
        private Vector2Int _startPosHead;
        private Vector2Int _startPosBody;
        private Vector2Int _lastPosBody;
        
        private Coroutine _moveCoroutine;
        
        private Dir _dir = Dir.Up;
        
        private LinkedList<PlayerBody> _path = new();
        
        private bool _isPlay = false;
        
        public void Start()
        {
            var dataPlayer = _assetLoader.LoadConfig(PlayerConfigPath) as PlayerConfig;

            InitHead(dataPlayer.Head);
            InitBody(dataPlayer.Body);
            InitButtons();
        }

        public void UpdateGame(bool value, Action<CellItem> onHeadConnect)
        {
            _isPlay = value;
            _head.gameObject.SetActive(value);

            if (value)
            {
                _dir = Dir.Up;
                _head.Pos = _startPosHead;
                _head.ResetRot();
                var cell = _cellsController.FindCellItem(_startPosHead);
                if(cell == null) return;
                onHeadConnect?.Invoke(cell);
                _cellsController.SetToCell(cell, _head);
                _lastPosBody = _cellsController.GetNextPos(_startPosHead, Dir.Down, () =>  _gameplay.GameOver());
                SpawnNewBody();
                
                _moveCoroutine = _coroutineHandler.StartActiveCoroutine(Move(onHeadConnect));
            }
            else
            {
                _coroutineHandler.StopActiveCoroutine(_moveCoroutine);
                foreach (var body in _path)
                {
                    _cellsController.ClearCell(body.Pos);
                    _pool.Despawn(body);
                }
                _path.Clear();
                _cellsController.ClearCell(_head.Pos);
            }
        }

        public void SpawnNewBody()
        {
            var body = SpanBody(_lastPosBody);
            if(body == null) return;
            _path.AddLast(body);
        }
        
        private PlayerBody SpanBody(Vector2Int pos)
        {
            var cell = _cellsController.FindCellItem(pos);
            if (cell == null) return null;

            var body = _pool.Spawn(_bodyPrefab, cell.Item.transform);
            body.Pos = pos;

            _cellsController.SetToCell(cell, body);
            cell.Type = CellType.Body;
            return body;
        }

        private IEnumerator Move(Action<CellItem> onHeadConnect)
        {
            while (true)
            {
                yield return new WaitForSeconds(Time);

                var newHeadPos = _cellsController.GetNextPos(_head.Pos, _dir, () =>  _gameplay.GameOver());
                if (_isPlay)
                {
                    var body = SpanBody(_head.Pos);
                    if(body != null) _path.AddFirst(body);
                }
                
                var cell = _cellsController.FindCellItem(newHeadPos);
                if(cell == null) continue;
                
                onHeadConnect?.Invoke(cell);
                
                _cellsController.SetToCell(cell, _head);
                _head.Pos = newHeadPos;
                _head.ResetRotCount();
                
                if (_path.Count <= 0) break;
                ClearCell();
            }
        }

        private void ClearCell()
        {
            _cellsController.ClearCell(_path.Last.Value.Pos);
            _lastPosBody = _path.Last.Value.Pos;
            _pool.Despawn(_path.Last.Value);
            _path.RemoveLast();
        }
        
        private void InitHead(PlayerHead prefab)
        {
            var dataCells = _assetLoader.LoadConfig(CellsConfigPath) as CellsConfig;

            var parentHead = _itemController.GetRectTransform(RectTransformViewID + RectTransformObject.HeadParent);
            _head = Object.Instantiate(prefab, parentHead);
            _head.gameObject.SetActive(false);
            _startPosHead = new Vector2Int(
                GetStartPos(dataCells.HorizontalCountCells),
                GetStartPos(dataCells.VerticalCountCells));
            _head.Pos = _startPosHead;
        }

        private void InitBody(PlayerBody prefab)
        {
            var parentBodyInactive =
                _itemController.GetRectTransform(RectTransformViewID + RectTransformObject.InactivePlayerBodyParent);

            _bodyPrefab = prefab;
            _pool = new ObjectPool<PlayerBody>();
            _pool.InitPool(prefab, parentBodyInactive);
        }
        
        private void InitButtons()
        {
            _itemController.SetAction(ButtonViewID + ButtonObject.RotateLeft, () =>
            {
                if(!_head.RotateLeft()) return;
                _dir = _dir switch
                {
                    Dir.Up => Dir.Left,
                    Dir.Left => Dir.Down,
                    Dir.Down => Dir.Right,
                    _ => Dir.Up
                };
            });

            _itemController.SetAction(ButtonViewID + ButtonObject.RotateRight, () =>
            {
                if(!_head.RotateRight()) return;
                _dir = _dir switch
                {
                    Dir.Up => Dir.Right,
                    Dir.Right => Dir.Down,
                    Dir.Down => Dir.Left,
                    _ => Dir.Up
                };
            });
        }

        private int GetStartPos(int count) => count % 2 > 0 ? 0 : 1;
    }
}