using UnityEditor;
using UnityEngine;

namespace Eggacy.Gameplay.Combat.TeamManagement.Editor
{
    [CustomEditor(typeof(TeamData), true)]
    public class TeamDataEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            GUILayout.Space(EditorGUIUtility.singleLineHeight);
            GUILayout.Label($"Instance index: {(target as TeamData).instanceIndex}");
            if(GUILayout.Button("Generate Random Instance Index"))
            {
                (target as TeamData).GenerateInstanceIndex();
            }
        }
    }
}