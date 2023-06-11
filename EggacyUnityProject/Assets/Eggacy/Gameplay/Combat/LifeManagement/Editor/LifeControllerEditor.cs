using UnityEditor;

namespace Eggacy.Gameplay.Combat.LifeManagement.Editor
{
    [CustomEditor(typeof(LifeController), true)]
    public class LifeControllerEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (!(target as LifeController).Object) return;

            EditorGUILayout.Space(EditorGUIUtility.singleLineHeight);
            EditorGUILayout.LabelField($"Max life: {(target as LifeController).maxLife}");
            EditorGUILayout.LabelField($"Current life: {(target as LifeController).currentLife}");
        }
    }
}