using UnityEngine;
using static Game.Constants;

namespace Core
{
    public abstract class Character: MonoBehaviour
    {
        [SerializeField] protected RectTransform _rect;
        [SerializeField] protected CellType _type;

        public Vector2Int Pos { get; set; }
        public CellType Type => _type;

        public void SetSize(float size) 
        {
            _rect.sizeDelta = new Vector2(size, size);
            _rect.localPosition = Vector3.zero;
            _rect.localScale = Vector3.one;
        }
    }
}