using Fusion;
using System;
using System.Collections;
using UnityEngine;

namespace Eggacy.Network
{
    public class LocalOnlyObject : MonoBehaviour
    {
        [SerializeField]
        private NetworkObject _networkObject = null;

        void Start()
        {
            StartCoroutine(WaitForObjectToBeSpawn(DestroyIfObjectIsNotLocal));
        }

        private IEnumerator WaitForObjectToBeSpawn(Action callback)
        {
            yield return new WaitUntil(() => _networkObject.Flags.HasFlag(NetworkObjectFlags.Spawned));
            callback();
        }

        private void DestroyIfObjectIsNotLocal()
        {
            if (!_networkObject.HasInputAuthority)
            {
                gameObject.SetActive(false);
            }
        }
    }
}