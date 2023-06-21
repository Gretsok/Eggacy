using Eggacy.Gameplay.Combat.Damage;
using Eggacy.Gameplay.Combat.LifeManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Eggacy.Gameplay.Level
{
    public class KillingZone : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if(other.TryGetComponent(out LifeControllerCollider lifeControllerCollider))
            {
                Damage damageToDeal = new Damage();
                damageToDeal.teamSource = null;
                damageToDeal.source = gameObject;
                damageToDeal.amountToRetreat = lifeControllerCollider.lifeController.maxLife;

                lifeControllerCollider.lifeController.TakeDamage(damageToDeal);
            }
        }
    }
}