using Game;
using UnityEngine;
using UnityEngine.Serialization;

namespace PlayerSystem
{
    [CreateAssetMenu(fileName = "PlayerConfig", menuName = "Configs/PlayerConfig", order = 0)]
    public class PlayerConfig : Config
    {
        [Range(0.01f, 0.1f)] public float Step = 0.05f;
        [Range(0.01f, 0.1f)] public float TimeStep = 1f;
        [FormerlySerializedAs("CountOnWin")] [Range(1, 20)] public int CountWin = 10;
        public PlayerHead Head;
        public PlayerBody Body;
    }
}