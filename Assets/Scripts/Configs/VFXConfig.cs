using System;
using System.Collections.Generic;
using Game;
using UnityEngine;

namespace VFXSystem
{
    [CreateAssetMenu(fileName = "VFXConfig", menuName = "Configs/VFXConfig", order = 0)]
    public class VFXConfig : Config
    {
        public List<VFXItem> VFX;
    }

    [Serializable]
    public class VFXItem
    {
        public Constants.VFXObjectType ID;
        public VFXObject Prefab;
    }
}