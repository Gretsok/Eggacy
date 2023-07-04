using Eggacy.Gameplay.Combat.Weapon;
using UnityEngine;
using Fusion;
using Eggacy.Gameplay.Combat.LifeManagement;
using System.Collections;
using Eggacy.Gameplay.Character.EggChampion.Mutations;

namespace Eggacy.Gameplay.Character.EggChampion
{
    public class EggChampionCharacter : NetworkBehaviour
    {
        [SerializeField]
        private NetworkRigidbody _rigidbody = null;
        public NetworkRigidbody networkRigidbody => _rigidbody;
        [SerializeField]
        private HoldedWeaponController _weaponController = null;
        public HoldedWeaponController weaponController => _weaponController;
        [SerializeField]
        private LifeController _lifeController = null;
        public LifeController lifeController => _lifeController;

        [SerializeField]
        private TankAllyPositionFeedback m_tankAllyPositionFeedback;
        public TankAllyPositionFeedback TankAllyPositionFeedback => m_tankAllyPositionFeedback;

        [SerializeField]
        private Mutations.MutationsController m_mutationsController;
        public MutationsController MutationsController => m_mutationsController;

        [SerializeField]
        private Transform _modelRoot = null;
        public Transform modelRoot => _modelRoot;
        [SerializeField]
        private GameObject _collidersGO = null;

        [SerializeField]
        private float _jumpVelocity = 8f;
        [SerializeField]
        private float _movementSpeed = 5f;
        [Networked]
        private float _movementSpeedMultiplier { get; set; }
        public float movementSpeed => _movementSpeed * _movementSpeedMultiplier;
        [SerializeField]
        private float _movementSmoothness = 8f;
        [SerializeField]
        private float _movementInAirSmoothness = 4f;
        [SerializeField]
        private float _gravity = 9.81f;
        [SerializeField]
        private LayerMask _groundLayerMask = default;
        private Vector3 _directionToMove { get; set; }
        [Networked]
        private Vector3 _orientation { get; set; }
        [Networked]
        private Vector3 _currentPlannedVelocity { get; set; }

        [Networked(OnChanged = nameof(HandleIsAliveChanged))]
        private bool _isAlive { get; set; }
        public bool isAlive => _isAlive;

        [Networked]
        private bool _isGrounded { get; set; } 
        public bool isGrounded => _isGrounded;

        private Transform _respawnPoint = null;
        [SerializeField]
        private float _respawnDuration = 7f;
        public float respawnDuration => _respawnDuration;


        private void Start()
        {
            _lifeController.onDied_ServerOnly += HandleDied_ServerOnly;

        }
        private void OnDestroy()
        {
            _lifeController.onDied_ServerOnly -= HandleDied_ServerOnly;
        }

        public override void Spawned()
        {
            base.Spawned();
            _movementSpeedMultiplier = 1;
        }

        public override void FixedUpdateNetwork()
        {
            base.FixedUpdateNetwork();
             
            _rigidbody.Rigidbody.rotation = Quaternion.LookRotation(_orientation, Vector3.up);

            if (!_isAlive) return;

            _isGrounded = IsGrounded();

            _rigidbody.Rigidbody.AddForce(Vector3.down * _gravity, ForceMode.Acceleration);

            if(_isGrounded)
            {
                float gravity = _rigidbody.Rigidbody.velocity.y;
                _currentPlannedVelocity = Vector3.Lerp(_currentPlannedVelocity, _directionToMove * movementSpeed, Runner.DeltaTime * _movementSmoothness);
                _rigidbody.Rigidbody.velocity = _currentPlannedVelocity + gravity * Vector3.up;
            }
            else
            {
                float gravity = _rigidbody.Rigidbody.velocity.y;
                _currentPlannedVelocity = Vector3.Lerp(_currentPlannedVelocity, _directionToMove * movementSpeed, Runner.DeltaTime * _movementInAirSmoothness);
                _rigidbody.Rigidbody.velocity = _currentPlannedVelocity + gravity * Vector3.up;
            }
        }

        #region Movement Behaviour
        public void SetDirectionToMove(Vector3 directionToMove)
        {
            _directionToMove = directionToMove;
        }

        public bool IsGrounded()
        {
            var isGrounded = Physics.OverlapBox(transform.position, new Vector3(0.5f, 0.05f, 0.5f), Quaternion.identity, _groundLayerMask).Length > 0;
            Debug.DrawLine(transform.position + Vector3.up * 0.1f, transform.position + Vector3.down * 0.15f, isGrounded ? Color.red : Color.yellow);
            return isGrounded;
        }

        public void SetSpeedMultiplier(float speedMultiplier)
        {
            _movementSpeedMultiplier = speedMultiplier;
        }
        #endregion

        #region Attack Behaviour
        [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
        public void Rpc_StartPrimaryAttack(Vector3 aimSource, Vector3 aimDirection)
        {
            if (!Runner.IsServer) return;
            if (!_isAlive) return;

            if(_weaponController)
                _weaponController.StartPrimaryAttack(aimSource, aimDirection);
        }

        [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
        public void Rpc_StartSecondaryAttack(Vector3 aimSource, Vector3 aimDirection)
        {
            if (!Runner.IsServer) return;
            if (!_isAlive) return;

            if (_weaponController)
                _weaponController.StartSecondaryAttack(aimSource, aimDirection);
        }

        [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
        public void Rpc_StopPrimaryAttack()
        {
            if (!Runner.IsServer) return;
            if (!_isAlive) return;

            if (_weaponController)
                _weaponController.StopPrimaryAttack();
        }

        [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
        public void Rpc_StopSecondaryAttack()
        {
            if (!Runner.IsServer) return;
            if (!_isAlive) return;

            if (_weaponController)
                _weaponController.StopSecondaryAttack();
        }
        #endregion

        #region Rally Behaviour
        [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
        public void Rpc_RequestRally()
        {
            if (!Runner.IsServer) return;
            if (!_isAlive) return;

            Rpc_HandleRallyFeedback();
        }

        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        public void Rpc_HandleRallyFeedback()
        {
            if (!Runner.IsServer) return;
            if (!_isAlive) return;

        }
        #endregion

        #region Charge Behaviour
        [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
        public void Rpc_RequestCharge()
        {
            if (!Runner.IsServer) return;
            if (!_isAlive) return;

            Rpc_HandleChargeFeedback();
        }

        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        public void Rpc_HandleChargeFeedback()
        {
            if (!Runner.IsServer) return;
            if (!_isAlive) return;

        }
        #endregion

        #region Jump Behaviour
        [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
        public void Rpc_RequestJump()
        {
            if (!Runner.IsServer) return;
            if (!_isAlive) return;
            if(!_isGrounded) return;

            _rigidbody.Rigidbody.AddForce(Vector3.up * _jumpVelocity, ForceMode.VelocityChange);

            Rpc_HandleJumpFeedback();
        }

        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        public void Rpc_HandleJumpFeedback()
        {
            if (!Runner.IsServer) return;
            if (!_isAlive) return;

        }
        #endregion

        #region Aim Behaviour
        public void SetAim(Vector3 aimSource, Vector3 aimDirection)
        {
            if (!Runner.IsServer) return;
            if (!_isAlive) return;

            _orientation = new Vector3(aimDirection.x, 0f, aimDirection.z).normalized;
            _weaponController.SetAim(aimSource, aimDirection);
        }
        #endregion

        #region Death Behaviour
        public void SetRespawnPoint(Transform respawnPoint)
        {
            if (!Runner.IsServer) return;

            _respawnPoint = respawnPoint;
        }

        public void SetToAlive()
        {
            if (!Runner.IsServer) return;

            GetComponent<NetworkTransform>().TeleportToPositionRotation(_respawnPoint.position, _respawnPoint.rotation);
            _isAlive = true;
            _lifeController.ResetLife();
        }

        private void HandleDied_ServerOnly(LifeController controller)
        {
            if (!Runner.IsServer) return;

            Die();
        }

        public void Die()
        {
            StartCoroutine(DeathRoutine());
        }

        private IEnumerator DeathRoutine()
        {
            _isAlive = false;
            _rigidbody.Rigidbody.velocity = Vector3.zero;
            _rigidbody.Rigidbody.angularVelocity = Vector3.zero;
            _rigidbody.Rigidbody.isKinematic = true;
            yield return new WaitForSeconds(respawnDuration);
            _rigidbody.Rigidbody.isKinematic = false;
            SetToAlive();
        }

        public static void HandleIsAliveChanged(Changed<EggChampionCharacter> changesHandler)
        {
            changesHandler.Behaviour._modelRoot.gameObject.SetActive(changesHandler.Behaviour._isAlive);
            changesHandler.Behaviour._collidersGO.SetActive(changesHandler.Behaviour._isAlive);
            changesHandler.Behaviour._rigidbody.Rigidbody.useGravity = changesHandler.Behaviour._isAlive;
        }
        #endregion
    }
}