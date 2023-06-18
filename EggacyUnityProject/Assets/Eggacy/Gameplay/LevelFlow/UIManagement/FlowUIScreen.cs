using UnityEngine;

namespace Eggacy.Gameplay.LevelFlow.UIManagement
{
    public class FlowUIScreen : MonoBehaviour
    {
        public void Enable()
        {
            gameObject.SetActive(true);
            SetUp();
        }

        protected virtual void SetUp()
        {

        }

        public void Disable()
        {
            gameObject.SetActive(false);
            CleanUp();
        }

        protected virtual void CleanUp()
        {

        }
    }
}