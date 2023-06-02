using System;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.Events;

namespace Tween
{
    [System.Serializable]
    public abstract class ATween : MonoBehaviour
    {
        #region Callbacks
        [SerializeField]
        private UnityEvent m_onStarted = null;
        [SerializeField]
        private UnityEvent m_onFinish = null;

        public Action<ATween> m_onTweenStarted = null;
        public Action<ATween> m_onTweenFinish = null;
        #endregion

        #region Infos
        private bool m_isPlaying = false;
        public bool IsPlaying => m_isPlaying;
        #endregion

        #region Basic Params
        protected Transform m_target = null;
        [SerializeField]
        private bool m_playOnAwake = false;
        [SerializeField]
        protected float m_tweenDuration = 1f;
        [SerializeField]
        protected float m_tweenDelay = 0f;
        [SerializeField]
        protected ELoopType m_loopType = ELoopType.Once;
        [SerializeField]
        protected AnimationCurve m_tweenCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

        public bool PlayOnAwake => m_playOnAwake;
        public float TweenDuration => m_tweenDuration;
        public float TweenDelay => m_tweenDelay;
        public ELoopType LoopType => m_loopType;
        #endregion

        private ATween[] m_attachedTweens = null;
        private List<Coroutine> m_coroutines = new List<Coroutine>();

        protected virtual void Start()
        {
            Init();
            if (m_playOnAwake)
            {
                StartTween();
            }
        }

        private bool m_isInit = false;
        private void Init()
        {
            m_attachedTweens = gameObject.GetComponents<ATween>();
            m_target = transform;
            m_isInit = true;
        }

        public void StartTween(bool forward = true)
        {
            if (!m_isPlaying)
            {
                if (!m_isInit)
                {
                    Init();
                }
                m_isPlaying = true;
                m_onStarted?.Invoke();
                m_onTweenStarted?.Invoke(this);
                switch (m_loopType)
                {
                    case ELoopType.Once:
                        StartCoroutine(OnceRoutine());
                        break;
                    case ELoopType.Loop:
                        StartCoroutine(LoopRoutine());
                        break;
                    case ELoopType.BackAndForth:
                        StartCoroutine(BackAndForthRoutine());
                        break;
                    case ELoopType.PingPong:
                        StartCoroutine(PingPongRoutine());
                        break;
                }
            }
        }

        public virtual void StartAllAttachedTweens()
        {
            for (int i = 0; i < m_attachedTweens.Length; ++i)
            {
                m_attachedTweens[i].StartTween();
            }
        }

        public virtual void Stop()
        {
            StopAllCoroutines();
            m_isPlaying = false;
            m_onFinish?.Invoke();
            m_onTweenFinish?.Invoke(this);
        }

        public virtual void StopAllAttachedTweens()
        {
            if (m_attachedTweens == null)
            {
                return;
            }
            for (int i = 0; i < m_attachedTweens.Length; ++i)
            {
                m_attachedTweens[i].Stop();
            }
        }

        private IEnumerator OnceRoutine()
        {
            yield return StartCoroutine(ForwardRoutine());
            Stop();
        }

        private IEnumerator LoopRoutine()
        {
            while (m_isPlaying)
            {
                yield return StartCoroutine(ForwardRoutine());
            }
        }

        private IEnumerator BackAndForthRoutine()
        {
            yield return StartCoroutine(ForwardRoutine());
            yield return StartCoroutine(BackwardRoutine());
            Stop();
        }

        private IEnumerator PingPongRoutine()
        {
            while (m_isPlaying)
            {
                yield return StartCoroutine(ForwardRoutine());
                yield return StartCoroutine(BackwardRoutine());
            }
        }

        private IEnumerator ForwardRoutine()
        {
            ResetValues();
            if(m_tweenDelay > 0f)
                yield return new WaitForSeconds(m_tweenDelay);

            float startingTime = Time.time;
            while (Time.time - startingTime < m_tweenDuration)
            {
                ManageTween(Mathf.Clamp01(m_tweenCurve.Evaluate(Mathf.Clamp01((Time.time - startingTime) / m_tweenDuration))));
                yield return null;
            }
        }

        private IEnumerator BackwardRoutine()
        {
            float startingTime = Time.time;
            while (Time.time - startingTime < m_tweenDuration)
            {
                ManageTween(1 - Mathf.Clamp01(m_tweenCurve.Evaluate(Mathf.Clamp01((Time.time - startingTime) / m_tweenDuration))));
                yield return null;
            }
        }


        protected virtual void SetStartingValues()
        {

        }

        protected virtual void SetFinalValues()
        {

        }

        public virtual void ResetValues()
        {
            SetStartingValues();
        }

        protected virtual void ManageTween(float interpolationValue)
        {

        }

        public void RegisterNewRoutine(IEnumerator a_routine)
        {

        }

        public void SetTweenDuration(float a_duration)
        {
            m_tweenDuration = a_duration;
        }

        public void SetTweenDelay(float a_delay)
        {
            m_tweenDelay = a_delay;
        }
    }

    public enum ELoopType
    {
        Once,
        Loop,
        BackAndForth,
        PingPong
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(ATween), true)]
    public class ATweenEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("Start tween !"))
            {
                (target as ATween).StartTween();
            }

            if (GUILayout.Button("Play all attached tweens !"))
            {
                (target as ATween).StartAllAttachedTweens();
            }

            if (GUILayout.Button("Stop all attached tweens !"))
            {
                (target as ATween).StopAllAttachedTweens();
            }

            if (GUILayout.Button("Reset !"))
            {
                (target as ATween).ResetValues();
            }
        }
    }
#endif
}