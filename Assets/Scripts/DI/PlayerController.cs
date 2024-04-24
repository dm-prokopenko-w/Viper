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

        private ObjectPool<PlayerBody> _pool;

        private PlayerBody _bodyPrefab;
        private Transform _parentBodyActive;
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

        public void UpdateGame(bool value, Action<CellItem> onHeadConnect, float time)
        {
            _isPlay = value;
            _head.gameObject.SetActive(value);

            if (value)
            {
                _dir = Dir.Up;
                _head.Pos = _startPosHead;
                var cell = _cellsController.FindCellItem(_startPosHead);
                if(cell == null) return;
                onHeadConnect?.Invoke(cell);
                _cellsController.SetToCell(cell, _head);
                _lastPosBody = _cellsController.GetNextPos(_startPosHead, Dir.Down, () =>  _gameplay.GameOver());
                SpawnNewBody();
                
                _moveCoroutine = _coroutineHandler.StartActiveCoroutine(Move(onHeadConnect, time));
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
            _path.AddLast(SpanBody(_lastPosBody));
        }
        
        private PlayerBody SpanBody(Vector2Int pos)
        {
            var body = _pool.Spawn(
                _bodyPrefab,
                _parentBodyActive);

            body.Pos = pos;
            var cell = _cellsController.FindCellItem(pos);
            if (cell == null) return body;
            _cellsController.SetToCell(cell, body);
            cell.Type = CellType.Body;
            return body;
        }

        private IEnumerator Move(Action<CellItem> onHeadConnect, float time)
        {
            while (true)
            {
                yield return new WaitForSeconds(time);

                var newHeadPos = _cellsController.GetNextPos(_head.Pos, _dir, () =>  _gameplay.GameOver());
                if (_isPlay)
                {
                    _path.AddFirst(SpanBody(_head.Pos));
                }
                
                var cell = _cellsController.FindCellItem(newHeadPos);
                if(cell == null) continue;
                
                onHeadConnect?.Invoke(cell);
                _cellsController.SetToCell(cell, _head);
                _head.Pos = newHeadPos;

                if (_path.Count <= 0) break;
                _cellsController.ClearCell(_path.Last.Value.Pos);
                _lastPosBody = _path.Last.Value.Pos;
                _pool.Despawn(_path.Last.Value);
                _path.RemoveLast();
            }
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
            _parentBodyActive =
                _itemController.GetRectTransform(RectTransformViewID + RectTransformObject.ActivePlayerBodyParent);
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