using System.Collections.Generic;
using Game;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using Core;
using ItemSystem;
using static Game.Constants;

namespace VFXSystem
{
    public class VFXController : IStartable
    {
        [Inject] private AssetLoader _assetLoader;
        [Inject] private ItemController _itemController;

        private Dictionary<VFXObjectType, VFXObject> _vfx = new();
        private ObjectPool<VFXObject> _pool;
        private Transform _parentActive;

        public void Start()
        {
            //var data = _assetLoader.LoadConfig(VFXConfigPath) as VFXConfig;
            
            //if(data == null)
            {
                //Debug.LogError("Add vfx objects");
                return;
            }
            
            _pool = new ObjectPool<VFXObject>();
           // _parentActive = _itemController.GetTransform(TransformViewID + TransformObject.ActiveVFXParent);
           // var parentInactive = _itemController.GetTransform(TransformViewID + TransformObject.InactiveVFXParent);

            //foreach (var vfx in data.VFX)
            {
              //  _vfx.Add(vfx.ID, vfx.Prefab);
               // _pool.InitPool(vfx.Prefab, parentInactive);
            }
        }

        public void SpawnEffect(VFXObjectType id, Vector3 pos)
        {
            if(!_vfx.TryGetValue(id, out VFXObject prefab)) return;
            
            var vfx = _pool.Spawn(prefab, pos, Quaternion.identity, _parentActive);
            vfx.Init(() => DespawnEffect(vfx));
        }

        private void DespawnEffect(VFXObject vfx)
        {
            _pool.Despawn(vfx);
        }
    }
}
