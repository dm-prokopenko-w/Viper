using Game;
using UnityEngine;

namespace BonusSystem
{
    [CreateAssetMenu(fileName = "BonusConfig", menuName = "Configs/BonusConfig", order = 0)]
    public class BonusConfig : Config
    {
        public Bonus Prefab;
    }
}
