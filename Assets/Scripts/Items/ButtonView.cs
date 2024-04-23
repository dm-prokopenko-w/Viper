using UnityEngine;
using UnityEngine.UI;
using VContainer;
using static Game.Constants;

namespace ItemSystem
{
    public class ButtonView : MonoBehaviour
    {
        [Inject] private ItemController _itemController;

        [SerializeField] private Button _btn;
        [SerializeField] private ButtonObject _type;
        
        [Inject]
        public void Construct()
        {
            _itemController.AddItemUI(ButtonViewID + _type, new Item(_btn));
        }
    }
}