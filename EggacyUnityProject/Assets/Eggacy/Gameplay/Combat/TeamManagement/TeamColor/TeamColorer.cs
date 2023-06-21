using System.Collections.Generic;
using UnityEngine;

namespace Eggacy.Gameplay.Combat.TeamManagement.TeamColor
{
    public class TeamColorer : MonoBehaviour
    {
        [SerializeField]
        private TeamController _teamController = null;

        [SerializeField]
        private List<Renderer> _renderersToColor = null;

        [SerializeField]
        private float _saturationValue = 0.7f;
        [SerializeField]
        private float _valueValue = 1.0f;

        private void Awake()
        {
            ApplyColor();
            _teamController.onTeamChanged += HandleTeamChanged;
        }

        private void ApplyColor()
        {
            if (!_teamController.teamData) return;

            Color teamColor = _teamController.teamData.team.teamColor;
            Color.RGBToHSV(teamColor, out float h, out float s, out float v);
            s = _saturationValue;
            v = _valueValue;
            teamColor = Color.HSVToRGB(h, s, v);

            _renderersToColor?.ForEach(r =>
            {
                r.material.color = teamColor;
            });
        }

        private void HandleTeamChanged(TeamController obj)
        {
            ApplyColor();
        }
    }
}