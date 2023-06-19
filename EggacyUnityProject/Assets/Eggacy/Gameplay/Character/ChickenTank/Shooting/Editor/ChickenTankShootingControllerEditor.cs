using UnityEditor;

namespace Eggacy.Gameplay.Character.ChickenTank.Shooting.Editor
{
    [CustomEditor(typeof(ChickenTankShootingController), true)]
    public class ChickenTankShootingControllerEditor : UnityEditor.Editor
    {

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (!(target as ChickenTankShootingController).Object) return;

            EditorGUILayout.Space(EditorGUIUtility.singleLineHeight);
            EditorGUILayout.LabelField($"Rotation target: {(target as ChickenTankShootingController).turretRotationTarget}");
        }
    }
}