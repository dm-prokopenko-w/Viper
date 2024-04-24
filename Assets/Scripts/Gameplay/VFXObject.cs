using System;
using System.Collections;
using UnityEngine;

namespace VFXSystem
{
    public class VFXObject : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _ps;

        public void Init(Action onDespawn)
        {
            StartCoroutine(DestroyTimer(onDespawn));
        }

        private IEnumerator DestroyTimer(Action onDespawn)
        {
            while(true && _ps != null)
            {
                yield return new WaitForSeconds(0.5f);
                if (_ps.IsAlive(true)) continue;
                onDespawn?.Invoke();
                break;
            }
        }
    }
}