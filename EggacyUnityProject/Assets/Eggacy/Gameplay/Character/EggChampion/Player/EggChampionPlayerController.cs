using Eggacy.Network;
using Fusion;
using System;
using UnityEngine;
using UnityEngine.TextCore.Text;

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
                return;
            }
            _cameraController.gameObject.SetActive(true);
            _controls = new EggChampionControls();
            _controls.Enable();

            _controls.Gameplay.Rally.performed += Rally_performed;
            _controls.Gameplay.Charge.performed += Charge_performed;
            _controls.Gameplay.Jump.performed += Jump_performed;

            Runner.GetComponent<NetworkManager>().SetLocalPlayerController(this);
        }

        private void Jump_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
        {
            if (_character)
                _character.Rpc_RequestJump();
        }

        private void Charge_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
        {
            if (_character)
                _character.Rpc_RequestCharge();
        }

        private void Rally_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
        {
            if (_character)
                _character.Rpc_RequestRally();
        }

        public override void FixedUpdateNetwork()
        {
            base.FixedUpdateNetwork();
            HandleMovement();
        }

        private void HandleOrientation()
        {
            if(_character)
                _character.SetOrientation(_cameraController.referencePointForMovement.forward);
        }

        private void HandleMovement()
        {
            if (!GetInput(out NetworkedInput input)) return;
            

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
            _networkedInput = networkedInput;

            HandleOrientation();
            Rpc_SetMovementReferences(_cameraController.referencePointForMovement.forward,
                _cameraController.referencePointForMovement.right);
        }

        [Rpc(RpcSources.InputAuthority, RpcTargets.All, Channel = RpcChannel.Unreliable)]
        private void Rpc_SetMovementReferences(Vector3 forward, Vector3 right)
        {
            _movementReferenceForward = forward;
            _movementReferenceRight = right;
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