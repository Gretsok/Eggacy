using UnityEngine;

namespace Eggacy.Gameplay.Combat.Weapon
{
    /// <summary>
    /// Don't inherit from that
    /// </summary>
    public abstract class AWeaponModelController : MonoBehaviour
    {
        public virtual void SetModelParent(Transform modelParent)
        {

        }

        public virtual void Update()
        {

        }
    }

    public abstract class AWeaponModelController<W, M> : AWeaponModelController where W : AWeapon where M : AWeaponModel
    {
        [SerializeField]
        private W _weapon = null;
        [SerializeField]
        private M _weaponModel = null;
        protected W weapon => _weapon;
        protected M weaponModel => _weaponModel;

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
            _weaponModel.transform.SetParent(modelParent);
            _weaponModel.transform.localPosition = Vector3.zero;
            _weaponModel.transform.localRotation = Quaternion.identity;
        }
    }
}