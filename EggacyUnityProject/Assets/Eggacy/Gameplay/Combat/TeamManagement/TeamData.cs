using UnityEngine;

namespace Eggacy.Gameplay.Combat.TeamManagement
{
    [CreateAssetMenu(fileName = "{name}Team", menuName = "Eggacy/Gameplay/Combat/TeamManagement/Team")]
    public class TeamData : ScriptableObject
    {
        [SerializeField, HideInInspector]
        private int _instanceIndex = -1;
        public int instanceIndex => _instanceIndex;

        [SerializeField]
        private Team _team = null;
        public Team team => _team;

#if UNITY_EDITOR
        public void GenerateInstanceIndex()
        {
            _instanceIndex = Random.Range(int.MinValue, int.MaxValue);
            UnityEditor.EditorUtility.SetDirty(this);
        }
#endif
    }
}