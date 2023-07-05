using UnityEngine;

namespace Eggacy.Gameplay.Character.EggChampion.InvincibilityAfterRespawn
{
    public class InvincibilityFeedbackHandler : MonoBehaviour
    {
        [SerializeField]
        private InvincibilityAfterRespawnHandler _invincibilityAfterRespawnHandler = null;

        [SerializeField]
        private ParticleSystem _invincibilityFX = null;

        private void Start()
        {
            HandleInvincibilityStopped();
            _invincibilityAfterRespawnHandler.onInvincibilityStarted += HandleInvincibilityStarted;
            _invincibilityAfterRespawnHandler.onInvincibilityStopped += HandleInvincibilityStopped;
        }

        private void OnDestroy()
        {
            _invincibilityAfterRespawnHandler.onInvincibilityStarted -= HandleInvincibilityStarted;
            _invincibilityAfterRespawnHandler.onInvincibilityStopped -= HandleInvincibilityStopped;
        }

        private void HandleInvincibilityStopped()
        {
            _invincibilityFX.Stop();
        }

        private void HandleInvincibilityStarted()
        {
            _invincibilityFX.Play();
        }
    }
}