using AnimTools.Curves;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AnimTools.Animate
{
    [ExecuteAlways]
    public abstract class Animate : MonoBehaviour
    {
        [Header("Animate")]
        [SerializeField, Range(0, 1)] private float animationProgress;
        [SerializeField] private float playbackSpeed = 1;

        public float TotalDuration { get; protected set; }
        public bool Scrubbable { get; protected set; }

        private List<float> _inputTimes = new List<float>();

        private bool _playing;

        private int _currentInputIndex;
        private bool _previousInputHandled;

        private float _currentTime = 0f;

        private float _nextStartTime;
        private float _lastDuration;


        protected virtual void OnEnable()
        {
            AnimationStart();
        }

        private void Update()
        {
            ResetVariables();
            if (_playing) { RegisterInput(); }
            AnimationUpdate();
            _currentTime += Time.deltaTime;
        }

        private void ResetVariables()
        {
            _nextStartTime = 0f;
            _lastDuration = 0f;
            _previousInputHandled = true;
            _currentInputIndex = 0;
        }

        private void RegisterInput()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _inputTimes.Add(_currentTime);
            }
        }

        protected virtual void AnimationUpdate()
        {

        }

        protected virtual void AnimationStart()
        {

        }

        protected void WaitForFinish()
        {
            _nextStartTime += _lastDuration;
        }

        protected void Wait(float duration)
        {
            _nextStartTime += duration;
        }

        protected void WaitForFraction(float fraction)
        {
            _nextStartTime += _lastDuration * fraction;
        }

        protected void WaitForInput()
        {
            // if we have have a time for the current input event
            if (_inputTimes.Count - 1 >= _currentInputIndex)
            {
                _nextStartTime = _inputTimes[_currentInputIndex];
            }
            // if not the next animations shouldn't start
            else
            {
                _nextStartTime = Mathf.Infinity;

            }
            _currentInputIndex++;
        }

        protected float CalculateAnimationProgress(float duration, Func<float,float> easeFunction)
        {
            float startTime = _nextStartTime;
            _lastDuration = duration;

            float timeSinceStart = _currentTime - startTime;
            float linearProgress = Mathf.Clamp01(timeSinceStart / duration);
            float progress = easeFunction(linearProgress);

            TotalDuration = Mathf.Max(TotalDuration, startTime + duration);

            return progress;
        }

    }
}
