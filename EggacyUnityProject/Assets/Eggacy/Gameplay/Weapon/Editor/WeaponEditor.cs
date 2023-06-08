using UnityEditor;

namespace Eggacy.Gameplay.Weapon.Editor
{
    [CustomEditor(typeof(AWeapon), true)]
    public class WeaponEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

        }
    }
}