using UnityEngine;

namespace Eggacy.Gameplay.Combat.Weapon
{
    /// <summary>
    /// Don't inherit from that
    /// </summary>
    public abstract class AWeaponModelController : MonoBehaviour
    {
        [SerializeField]
        private AWeapon _weapon = null;
        [SerializeField]
        private AWeaponModel _weaponModel = null;
        public AWeapon notCastedWeapon => _weapon;
        public AWeaponModel notCastedWeaponModel => _weaponModel;

        public virtual void SetModelParent(Transform modelParent)
        {

        }

        public virtual void Update()
        {

        }
    }

    public abstract class AWeaponModelController<W, M> : AWeaponModelController where W : AWeapon where M : AWeaponModel
    {
        protected W weapon => notCastedWeapon as W;
        protected M weaponModel => notCastedWeaponModel as M;

        private void Start()
        {
            SetUp();
        }

        private void OnDestroy()
        {
            CleanUp();
            Destroy(weaponModel.gameObject);
        }

        protected virtual void SetUp()
        {

        }

        protected virtual void CleanUp()
        {

        }

        public override void SetModelParent(Transform modelParent)
        {
            weaponModel.transform.SetParent(modelParent);
            weaponModel.transform.localPosition = Vector3.zero;
            weaponModel.transform.localRotation = Quaternion.identity;
        }
    }
}