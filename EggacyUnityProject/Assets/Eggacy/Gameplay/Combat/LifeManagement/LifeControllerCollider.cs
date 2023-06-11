using UnityEngine;

namespace Eggacy.Gameplay.Combat.LifeManagement
{
    public class LifeControllerCollider : MonoBehaviour
    {
        [SerializeField]
        private LifeController _lifeController = null;
        public LifeController lifeController => _lifeController;
    }
}