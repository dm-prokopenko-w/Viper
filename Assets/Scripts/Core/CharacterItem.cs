using UnityEngine;

namespace Core
{
    public abstract class Character: MonoBehaviour
    {
        [SerializeField] protected RectTransform _rect;

        public Vector2Int Pos { get; set; }

        public void SetSize(float size) 
        {
            _rect.sizeDelta = new Vector2(size, size);
            _rect.localPosition = Vector3.zero;
            _rect.localScale = Vector3.one;
        }
    }
}