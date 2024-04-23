using UnityEngine;
using VContainer;
using static Game.Constants;

namespace ItemSystem
{
    public class AnimatorView : MonoBehaviour
    {
        [Inject] private ItemController _uiController;

        [SerializeField] private Animator _anim;
        [SerializeField] private AnimatorObject _type;
        
        [Inject]
        public void Construct()
        {
            _uiController.AddItemUI(AnimatorViewID + _type, new Item(_anim));
        }
    }
}
