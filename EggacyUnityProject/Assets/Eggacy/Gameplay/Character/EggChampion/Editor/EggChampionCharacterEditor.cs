using UnityEditor;

namespace Eggacy.Gameplay.Character.EggChampion.Editor
{
    [CustomEditor(typeof(EggChampionCharacter), true)]
    public class EggChampionCharacterEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (!(target as EggChampionCharacter).Object) return;

            EditorGUILayout.Space(EditorGUIUtility.singleLineHeight);
            EditorGUILayout.LabelField($"Egg champion character info:");
            EditorGUILayout.LabelField($"IsAlive: {(target as EggChampionCharacter).isAlive}");
            EditorGUILayout.LabelField($"IsGrounded: {(target as EggChampionCharacter).isGrounded}");
            
        }
    }
}