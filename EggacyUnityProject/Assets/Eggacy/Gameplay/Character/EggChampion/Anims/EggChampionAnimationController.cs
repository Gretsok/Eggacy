using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Eggacy.Gameplay.Character.EggChampion
{
    public class EggChampionAnimationController : MonoBehaviour
    {
        public static readonly int IS_MOVING = Animator.StringToHash("IsMoving");
        public static readonly int IS_IN_AIR = Animator.StringToHash("IsInAir");

        public static readonly int SPEED_X = Animator.StringToHash("SpeedX");
        public static readonly int SPEED_Z = Animator.StringToHash("SpeedZ");

        [SerializeField]
        private Rigidbody m_rigidbody = null;

        [SerializeField]
        private EggChampionCharacter m_eggChampionCharacter = null;

        [SerializeField]
        private Animator m_animator = null;

        [SerializeField]
        private float m_speedMaxForAnim = 5f;

        private bool m_isMoving = false;
        private bool m_isGrounded = true;

        private void LateUpdate()
        {
            if(m_rigidbody != null && m_animator != null && m_eggChampionCharacter != null)
            {
                if (m_eggChampionCharacter.isGrounded)
                {
                    Vector3 localVelocity = transform.InverseTransformDirection(m_rigidbody.velocity);
                    if (localVelocity.magnitude > 0.01f)
                    {
                        if (m_isMoving == false)
                        {
                            m_isMoving = true;
                            m_animator.SetBool(IS_MOVING, true);
                        }

                        m_animator.SetFloat(SPEED_X, localVelocity.x / m_speedMaxForAnim);
                        m_animator.SetFloat(SPEED_Z, localVelocity.z / m_speedMaxForAnim);
                    }
                    else
                    {
                        if (m_isMoving == true)
                        {
                            m_isMoving = false;
                            m_animator.SetBool(IS_MOVING, false);
                        }
                    }

                    if (m_isGrounded == false)
                    {
                        m_isGrounded = true;
                        m_animator.SetBool(IS_IN_AIR, false);
                    }
                }
                else
                {
                    if (m_isGrounded == true)
                    {
                        m_isGrounded = false;
                        m_animator.SetBool(IS_IN_AIR, true);
                    }
                }
            }
        }
    }
}
