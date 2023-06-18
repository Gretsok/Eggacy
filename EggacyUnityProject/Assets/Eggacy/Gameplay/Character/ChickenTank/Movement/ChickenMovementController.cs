using Fusion;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

namespace Eggacy.Gameplay.Character.ChickenTank.Movement
{
    public class ChickenMovementController : NetworkBehaviour
    {
        [SerializeField]
        private float _movementSpeed = 3f;
        [SerializeField]
        private LayerMask _groundDetectionLayerMask = default;
        private RaycastHit _hitInfo = default;

        private ChickenWaypointsManager _waypointsManager = null;

        public void SetWaypointsManager(ChickenWaypointsManager waypointsManager)
        { 
            _waypointsManager = waypointsManager;
        }

        private ChickenWaypoint _waypointTarget = default;
        private ChickenWaypoint _previousWaypointTarget = default;
        private bool _isInitialized = false;
        private NavMeshPath _path = null;

        [Networked]
        private Vector3 _direction { get; set; }
        [SerializeField]
        private float _pathUpdateCooldown = 0.5f;
        private float _lastTimeOfPathUpdated = float.MinValue;
        [SerializeField]
        private float _distanceToChangeWaypointTarget = 0.5f;

        public void SetStartingWaypoint(ChickenWaypoint waypoint)
        {
            _waypointTarget = waypoint;
            transform.position = _waypointTarget.transform.position;
            transform.rotation = _waypointTarget.transform.rotation;
        }

        public void SetReady()
        {
            if (!Runner.IsServer) return;

            _path = new NavMeshPath();
            UpdateLinkAndTarget();
            _isInitialized = true;
        }

        private void UpdateLinkAndTarget()
        {
            if (!_waypointsManager)
            {
                Debug.LogError($"ChickenMovementController on {gameObject.name} needs a reference to a ChickenWaypointsManager to work.");
                return;
            }

            var newLink = _waypointsManager.GetWaypointLinkFrom(_waypointTarget, _previousWaypointTarget);

            _previousWaypointTarget = _waypointTarget;
            if(_previousWaypointTarget == newLink.waypoint1)
            {
                _waypointTarget = newLink.waypoint2;
            }
            else
            {
                _waypointTarget = newLink.waypoint1;
            }
            UpdateDirection();
        }

        public override void FixedUpdateNetwork()
        {
            base.FixedUpdateNetwork();
            transform.position += _direction * _movementSpeed * Runner.DeltaTime;

            transform.forward = _direction;
            if(Physics.Raycast(transform.position + transform.up *2f, -transform.up, out _hitInfo, 10f, _groundDetectionLayerMask))
            {
                Debug.Log("HIT");
                transform.position = _hitInfo.point;
                transform.forward = Vector3.Cross(_hitInfo.normal, -transform.right);
                Debug.DrawLine(transform.position + transform.up * 2f, _hitInfo.point, Color.green);
            }
            else
            {
                Debug.DrawLine(transform.position + transform.up * 2f, transform.position + transform.up * 5f - transform.up * 10f, Color.red);
            }

        }

        private void Update()
        {
            if (!Runner) return;
            if (!Runner.IsServer) return;
            if (!_isInitialized) return;
            if (!_waypointTarget)
            {
                Debug.LogError($"{gameObject.name} is not set up correctly and is not working.");
                return;
            }

            if (Time.time - _lastTimeOfPathUpdated > _pathUpdateCooldown)
            {
                _lastTimeOfPathUpdated = Time.time;
                UpdateDirection();
            }

            if (_path.corners.Length < 2) return;

            if ((_waypointTarget.transform.position - transform.position).sqrMagnitude < _distanceToChangeWaypointTarget * _distanceToChangeWaypointTarget) 
            {
                UpdateLinkAndTarget(); 
            }
        }

        private void UpdateDirection()
        {
            bool calculated = NavMesh.CalculatePath(transform.position, _waypointTarget.transform.position, NavMesh.AllAreas, _path);
            if(!calculated)
            {
                Debug.LogError("Error when calculating path");
            }

            if (_path.corners.Length < 2) return;

            _direction = (_path.corners[1] - transform.position).normalized; 

            for(int i = 0; i < _path.corners.Length - 1; ++i)
            {
                Debug.DrawLine(_path.corners[i], _path.corners[i + 1], Color.red, _pathUpdateCooldown);
            }

        }
    }
}