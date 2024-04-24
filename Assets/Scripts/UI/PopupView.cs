using UnityEngine;
using VContainer;
using static Game.Constants;

namespace ItemSystem
{
    public class PopupView : MonoBehaviour
    {
        [Inject] private PopupController _controller;

        [SerializeField] private PopupsID _id;
        [SerializeField] private Animator _anim;
        [SerializeField] private bool _isHideOnStart = true;

        [Inject]
        public void Construct()
        {
            _controller.AddPopupView(_id.ToString(), this);
        }

        private void Start()
        {
            if(_isHideOnStart) _anim.Play(StartKey);
        }

        public Animator GetAnimator() => _anim;
    }
}