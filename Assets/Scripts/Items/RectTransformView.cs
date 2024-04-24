using ItemSystem;
using UnityEngine;
using VContainer;
using static Game.Constants;

public class RectTransformView : MonoBehaviour
{
    [Inject] private ItemController _itemController;
        
    [SerializeField] private RectTransformObject _type;
    [SerializeField] private RectTransform _rect;

    [Inject]
    public void Construct()
    {
        _itemController.AddItemUI(RectTransformViewID + _type, new Item(_rect));
    }
}
