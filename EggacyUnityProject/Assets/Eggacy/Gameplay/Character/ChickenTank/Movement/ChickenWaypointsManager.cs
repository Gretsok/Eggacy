using Eggacy.Gameplay.Character.ChickenTank.Movement;
using System.Collections.Generic;
using UnityEngine;

namespace Eggacy.Gameplay.Character.ChickenTank
{
    public class ChickenWaypointsManager : MonoBehaviour
    {
        [SerializeField]
        private List<SWaypointLink> _waypointLinks = null;

        public SWaypointLink GetWaypointLinkFrom(ChickenWaypoint sourceWaypoint, params ChickenWaypoint[] waypointsToIgnore)
        {
            var waypointsToIgnoreList = new List<ChickenWaypoint>(waypointsToIgnore);


            var linksWithSourceWaypoint = _waypointLinks.FindAll(link => link.waypoint1 == sourceWaypoint || link.waypoint2 == sourceWaypoint);
            if(linksWithSourceWaypoint.Count == 0)
            {
                Debug.LogError($"source waypoint : {sourceWaypoint.name} is not linked to any other waypoint on {gameObject.name}");
                return default;
            }
            
            // We filter the links to keep only those with no waypointsToIgnore
            var validLinks = new List<SWaypointLink>();
            linksWithSourceWaypoint.ForEach(link =>
            {
                if(!waypointsToIgnoreList.Contains(link.waypoint1) && !waypointsToIgnoreList.Contains(link.waypoint2))
                {
                    validLinks.Add(link);
                }
            });

            if(validLinks.Count > 0)
            {
                return validLinks[Random.Range(0, validLinks.Count)];
            }

            Debug.LogWarning($"Could not find waypoint links using waypoint to ignore : Not ignoring waypoints to ignore.");

            return linksWithSourceWaypoint[Random.Range(0, linksWithSourceWaypoint.Count)];
        }
    }
}