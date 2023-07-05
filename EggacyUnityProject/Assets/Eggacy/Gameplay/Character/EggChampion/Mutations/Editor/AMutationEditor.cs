using UnityEditor;
using UnityEngine;

namespace Eggacy.Gameplay.Character.EggChampion.Mutations.Editor
{
    [CustomEditor(typeof(AMutation), true)]
    public class AMutationEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (!(target as AMutation).Object) return;

            GUILayout.Space(EditorGUIUtility.singleLineHeight);
            GUILayout.Label($"Level: {(target as AMutation).level}");
            GUILayout.Label($"Experience: {(target as AMutation).currentExperience}");
        }
    }
}