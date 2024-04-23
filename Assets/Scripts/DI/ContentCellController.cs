using GameplaySystem;
using CellsSystem;
using UnityEngine;
using VContainer;
using static Game.Constants;

namespace ContentCellSystem
{
    public class ContentCellController
    {
        [Inject] private GameplayController _gameplay;
        
        public void Connect(CellItem cell)
        {
            switch (cell.Type)
            {
                case CellType.None:
                    cell.Type = CellType.Head;
                    break;
                case CellType.Body:
                case CellType.Block:
                    Debug.LogError(111);
                    _gameplay.GameOver();
                    break;
                case CellType.Fruit:
                    cell.Type = CellType.Head;
                    break;
            }
        }
    }
}
