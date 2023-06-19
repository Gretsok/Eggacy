using Fusion;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Eggacy.Gameplay.LevelFlow
{
    public class LevelFlowManager : NetworkBehaviour
    {
        [SerializeField]
        private List<LevelFlowState> _flowStates = new List<LevelFlowState>();
        private LevelFlowState _currentState = null;
        [Networked(OnChanged = nameof(HandleCurrentStateIndexChanged))]
        private int _currentStateIndex { get; set; }
        public int currentStateIndex => _currentStateIndex;

        public Action<LevelFlowManager> onStateUpdated = null;
        public Action onFlowEnded = null;

        public override void Spawned()
        {
            base.Spawned();

            SwitchToState(0);
        }

        private void Update()
        {
            if (Runner && !Runner.IsServer) return;

            HandleUpdate();
        }

        protected virtual void HandleUpdate()
        {
            if(_currentState)
                _currentState.Server_Update();
        }

        private void SwitchToState(int index)
        {
            if(!Runner.IsServer) return;

            _currentStateIndex = index;

            if (index < _flowStates.Count)
            {
                if (_currentState)
                {
                    _currentState.Server_Leave();
                }
                _currentState = _flowStates[index];
            }
            else
            {
                _currentState = null;
                onFlowEnded?.Invoke();
            }

            if(_currentState)
            {
                _currentState.Server_Enter();
                _currentState.onStateEnded += HandleCurrentStateEnded;
            }
        }

        private void HandleCurrentStateEnded(LevelFlowState state)
        {
            state.onStateEnded -= HandleCurrentStateEnded;

            if (state != _currentState) return;

            SwitchToState(_flowStates.FindIndex(s => s == state) + 1);
        }

        public static void HandleCurrentStateIndexChanged(Changed<LevelFlowManager> changesHandler)
        {
            var index = changesHandler.Behaviour._currentStateIndex;
            var states = changesHandler.Behaviour._flowStates;

            if (index < states.Count)
                changesHandler.Behaviour._currentState = states[index];
            else
                changesHandler.Behaviour._currentState = null;

            changesHandler.Behaviour.onStateUpdated?.Invoke(changesHandler.Behaviour);
        }
    }
}