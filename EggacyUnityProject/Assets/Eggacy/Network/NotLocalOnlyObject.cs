using Fusion;
using System;
using System.Collections;
using UnityEngine;

namespace Eggacy.Network
{
    public class NotLocalOnlyObject : MonoBehaviour
    {
        [SerializeField]
        private NetworkObject _networkObject = null;

        void Start()
        {
            StartCoroutine(WaitForObjectToBeSpawn(DestroyIfObjectIsLocal));
        }

        private IEnumerator WaitForObjectToBeSpawn(Action callback)
        {
            yield return new WaitUntil(() => _networkObject.Flags.HasFlag(NetworkObjectFlags.Spawned));
            callback();
        }

        private void DestroyIfObjectIsLocal()
        {
            if(_networkObject.HasInputAuthority)
            {
                gameObject.SetActive(false);
            }
        }
    }
}