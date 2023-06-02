using Eggacy.Network;
using Fusion;
using System;
using UnityEngine;

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

        public void SetCharacter(EggChampionCharacter character)
        {
            _character = character;
            _possessedCharacterID = _character.Object.Id;
        }

        public void Initialize()
        {
            if (!HasInputAuthority)
            {
                _cameraController.gameObject.SetActive(false);
                return;
            }

            _controls = new EggChampionControls();
            _controls.Enable();

            _controls.Gameplay.Rally.performed += Rally_performed;
            _controls.Gameplay.Charge.performed += Charge_performed;
            _controls.Gameplay.Jump.performed += Jump_performed;

            _cameraController.SetPositionTarget(_character.transform);
        }

        private void Jump_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
        {
            _character.Rpc_RequestJump();
        }

        private void Charge_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
        {
            _character.Rpc_RequestCharge();
        }

        private void Rally_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
        {
            _character.Rpc_RequestRally();
        }

        public override void FixedUpdateNetwork()
        {
            base.FixedUpdateNetwork();
            HandleMovement();
            HandleOrientation();
        }

        private void HandleOrientation()
        {
            if(_character)
                _character.SetOrientation(_cameraController.referencePointForMovement.forward);
        }

        private void HandleMovement()
        {
            if (!GetInput(out NetworkedInput input)) return;

            if(input.movement.sqrMagnitude > 1)
            { 
                input.movement.Normalize();
            }

            Vector3 directionToMoveCharacter = _cameraController.referencePointForMovement.forward * input.movement.y
                + _cameraController.referencePointForMovement.right * input.movement.x;
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
        }


        public override void Despawned(NetworkRunner runner, bool hasState)
        {
            base.Despawned(runner, hasState);
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
        }
    }
}