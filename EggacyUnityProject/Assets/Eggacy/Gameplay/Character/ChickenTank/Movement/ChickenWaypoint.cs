using UnityEngine;

namespace Eggacy.Gameplay.Character.ChickenTank.Movement
{
    public class ChickenWaypoint : MonoBehaviour
    {
#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position + Vector3.up * 1.5f, 0.5f);
        }
#endif
    }
}