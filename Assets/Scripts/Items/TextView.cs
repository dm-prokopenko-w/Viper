using ItemSystem;
using TMPro;
using UnityEngine;
using VContainer;
using static Game.Constants;

namespace UISystem
{
    public class TextView : MonoBehaviour
    {
        [Inject] private ItemController _itemController;

        [SerializeField] private TextObject _type;
        [SerializeField] private TMP_Text _text;

        [Inject]
        public void Construct()
        {
            _itemController.AddItemUI(TextViewID + _type, new Item(_text));
        }
    }
}
