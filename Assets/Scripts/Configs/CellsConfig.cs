using Game;
using UnityEngine;

namespace CellsSystem
{
    [CreateAssetMenu(fileName = "CellsConfig", menuName = "Configs/CellsConfig", order = 0)]
    public class CellsConfig : Config
    {
        [Range(1, 30)]public int HorizontalCountCells = 20;
        [Range(1, 30)]public int VerticalCountCells = 20;

        public Cell Prefab;
    }
}