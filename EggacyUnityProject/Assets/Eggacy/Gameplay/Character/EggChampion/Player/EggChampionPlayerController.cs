using Eggacy.Network;
using UnityEngine;
using Fusion;
using System.Collections.Generic;

namespace Eggacy.Gameplay.Character.EggChampion.Player
{
    public class EggChampionPlayerController : NetworkBehaviour
    {
        [Networked(OnChanged = nameof(HandleCharacterChanged))]
        private NetworkId _possessedCharacterID { get; set; }


        private EggChampionCharacter _character = null;

        private EggChampionControls _controls = null;

        [SerializeField]
        private EggChampionPlayerCameraController _cameraController = null;
        private NetworkedInput _networkedInput;
        public NetworkedInput networkedInput => _networkedInput;

        [Networked]
        private Vector3 _movementReferenceForward { get; set; }
        [Networked]
        private Vector3 _movementReferenceRight { get; set; }

        [SerializeField]
        private List<GameObject> _localGameObjects = new List<GameObject>();

        private void Awake()
        {
            _cameraController.gameObject.SetActive(false);
        }

        public void SetCharacter(EggChampionCharacter character)
        {
            _character = character;
            _possessedCharacterID = _character.Object.Id;

            _cameraController.SetPositionTarget(character.transform);
        }


        public override void Spawned()
        {
            base.Spawned();


            if (!HasInputAuthority)
            {
                for(int i = 0; i <_localGameObjects.Count; ++i)
                {
                    _localGameObjects[i].SetActive(false);
                }
                return;
            }

            Cursor.lockState = CursorLockMode.Locked;

            _cameraController.gameObject.SetActive(true);
            _controls = new EggChampionControls();
            _controls.Enable();

            _controls.Gameplay.Rally.performed += Rally_performed;
            _controls.Gameplay.Charge.performed += Charge_performed;
            _controls.Gameplay.Jump.performed += Jump_performed;
            _controls.Gameplay.Attack.started += Attack_started;
            _controls.Gameplay.Attack.canceled += Attack_canceled;
            _controls.Gameplay.SecondaryAttack.started += SecondaryAttack_started;
            _controls.Gameplay.SecondaryAttack.canceled += SecondaryAttack_canceled;

            Runner.GetComponent<NetworkManager>().SetLocalPlayerController(this);
        }

        #region Attack Input
        private void Attack_started(UnityEngine.InputSystem.InputAction.CallbackContext obj)
        {
            Ray aimRay = _cameraController.camera.ViewportPointToRay(new Vector2(0.5f, 0.5f));
            if (_character)
                _character.Rpc_StartPrimaryAttack(aimRay.origin, aimRay.direction);
        }

        private void Attack_canceled(UnityEngine.InputSystem.InputAction.CallbackContext obj)
        {
            if (_character)
                _character.Rpc_StopPrimaryAttack();
        }

        private void SecondaryAttack_started(UnityEngine.InputSystem.InputAction.CallbackContext obj)
        {
            Ray aimRay = _cameraController.camera.ViewportPointToRay(new Vector2(0.5f, 0.5f));
            if (_character)
                _character.Rpc_StartSecondaryAttack(aimRay.origin, aimRay.direction);
        }

        private void SecondaryAttack_canceled(UnityEngine.InputSystem.InputAction.CallbackContext obj)
        {
            if (_character)
                _character.Rpc_StopSecondaryAttack();
        }
        #endregion

        #region Jump Input
        private void Jump_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
        {
            if (_character)
                _character.Rpc_RequestJump();
        }
        #endregion

        #region Charge Input
        private void Charge_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
        {
            if (_character)
                _character.Rpc_RequestCharge();
        }
        #endregion

        #region Rally Input
        private void Rally_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
        {
            if (_character)
                _character.Rpc_RequestRally();
        }
        #endregion

        public override void FixedUpdateNetwork()
        {
            base.FixedUpdateNetwork();

            if (!GetInput(out NetworkedInput input)) return;

            HandleMovement(input);
            HandleOrientation(input);

        }

        private void HandleOrientation(NetworkedInput input)
        {
            if (_character)
                _character.SetOrientation(input.orientation);
        }

        private void HandleMovement(NetworkedInput input)
        {
            if (input.movement.sqrMagnitude > 1)
            { 
                input.movement.Normalize();
            }

            Vector3 directionToMoveCharacter = _movementReferenceForward * input.movement.y
                + _movementReferenceRight * input.movement.x;

            // Debug.Log($"Movement input: {input.movement} | camera forward: {_cameraController.referencePointForMovement.forward}");


            if (_character)
                _character.SetDirectionToMove(directionToMoveCharacter);
        }

        private void Update()
        {
            if (!HasInputAuthority) return;
            if (_controls == null) return;

            _cameraController.SetRotationInput(_controls.Gameplay.LookAround.ReadValue<Vector2>());
            NetworkedInput networkedInput = default;
            networkedInput.movement = _controls.Gameplay.Move.ReadValue<Vector2>();
            networkedInput.orientation = _cameraController.referencePointForMovement.forward;
            _networkedInput = networkedInput;

            Rpc_SetMovementReferences(_cameraController.referencePointForMovement.forward,
                _cameraController.referencePointForMovement.right);

            Ray aimRay = _cameraController.camera.ViewportPointToRay(new Vector2(0.5f, 0.5f));
            Rpc_SetAim(aimRay.origin, aimRay.direction);
        }

        [Rpc(RpcSources.InputAuthority, RpcTargets.All, Channel = RpcChannel.Unreliable)]
        private void Rpc_SetMovementReferences(Vector3 forward, Vector3 right)
        {
            _movementReferenceForward = forward;
            _movementReferenceRight = right;
        }

        [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority, Channel = RpcChannel.Unreliable)]
        private void Rpc_SetAim(Vector3 aimSource, Vector3 aimDirection)
        {
            if (!Runner.IsServer) return;

            Server_UpdateAim(aimSource, aimDirection);
        }

        private void Server_UpdateAim(Vector3 aimSource, Vector3 aimDirection)
        {
            if (_character)
                _character.SetAim(aimSource, aimDirection);
        }

        public override void Despawned(NetworkRunner runner, bool hasState)
        {
            base.Despawned(runner, hasState);


            if (!HasInputAuthority)
            {
                return;
            }
            _controls.Gameplay.Rally.performed -= Rally_performed;
            _controls.Gameplay.Charge.performed -= Charge_performed;
            _controls.Gameplay.Jump.performed -= Jump_performed;
            _controls.Gameplay.Attack.started -= Attack_started;
            _controls.Gameplay.Attack.canceled -= Attack_canceled;
            _controls.Gameplay.SecondaryAttack.started -= SecondaryAttack_started;
            _controls.Gameplay.SecondaryAttack.canceled -= SecondaryAttack_canceled;
            _controls.Disable();
            _controls.Dispose();
        }

        public static void HandleCharacterChanged(Changed<EggChampionPlayerController> changesHandler)
        {
            changesHandler.Behaviour._character = 
                changesHandler.Behaviour.Runner.FindObject(changesHandler.Behaviour._possessedCharacterID).GetComponent<EggChampionCharacter>();
            changesHandler.Behaviour._cameraController.SetPositionTarget(changesHandler.Behaviour._character.transform);
        }
    }
}