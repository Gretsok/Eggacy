using Eggacy.Gameplay.Combat.TeamManagement;
using System.Collections.Generic;
using UnityEngine;

namespace Eggacy.Gameplay.Character.ChickenTank.TeamColor
{
    public class ChickenTankTeamColorer : MonoBehaviour
    {
        [SerializeField]
        private TeamController _teamController = null;

        [SerializeField]
        private List<Renderer> _renderersToColor = null;


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
            s = 0.7f;
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