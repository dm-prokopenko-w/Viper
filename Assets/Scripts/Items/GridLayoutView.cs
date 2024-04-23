using ItemSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;
using static Game.Constants;
namespace UISystem
{
    public class GridLayoutView : MonoBehaviour
    {
        [Inject] private ItemController _itemController;

        [SerializeField] private GridLayoutGroup _grid;

        [Inject]
        public void Construct()
        {
            _itemController.AddItemUI(GridLayoutViewID, new Item(_grid));
        }
    }
}