using UnityEditor;

namespace Eggacy.Gameplay.Combat.LifeManagement.Editor
{
    [CustomEditor(typeof(LifeController), true)]
    public class LifeControllerEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EditorGUILayout.Space(EditorGUIUtility.singleLineHeight);
            EditorGUILayout.LabelField($"Max life: {(target as LifeController).maxLife}");
            EditorGUILayout.LabelField($"Current life: {(target as LifeController).currentLife}");
        }
    }
}