using Game;
using UnityEngine;

namespace EnemySystem
{
    [CreateAssetMenu(fileName = "EnemyConfig", menuName = "Configs/EnemyConfig", order = 0)]
    public class EnemyConfig : Config
    {
        [Range(1, 10)] public int MinCountOnStart = 3;
        [Range(1, 10)] public int MaxCountOnStart = 5;
        public Enemy Prefab;
        
        private void OnValidate()
        {
            if (MinCountOnStart >= MaxCountOnStart)
            {
                MaxCountOnStart = MinCountOnStart + 1;
            }
        }
    }
}
