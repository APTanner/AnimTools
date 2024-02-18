using AnimTools.Curves;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AnimTools
{
    [ExecuteAlways]
    public abstract class Animate : MonoBehaviour
    {
        [Header("Animate")]
        [SerializeField, Range(0, 1)] private float animationProgress;
        [SerializeField] private float playbackSpeed = 1;

        [SerializeField] private float totalDuration;
        private bool _scrubbable;
        private bool _animationInProgress;

        private List<float> _inputTimes = new List<float>();

        [SerializeField] private bool playing;

        private int _currentInputIndex;
        //private bool _previousInputHandled;

        public float currentTime = 0f;

        private float _nextStartTime;
        private float _lastDuration;

        public void BeginAnimation()
        {
            _inputTimes.Clear();
            _currentInputIndex = 0;
            currentTime = 0f;
            _scrubbable = false;
            totalDuration = 0f;
            playing = false;
        }

        public void Play()
        {
            playing = true;
            _scrubbable = false;
        }

        protected virtual void OnEnable()
        {
            BeginAnimation();
            AnimationStart();
        }

        public void Update()
        {
            ResetVariables();
            AnimationUpdate();
            if (playing) 
            {
                RegisterInput();
                currentTime += Time.deltaTime * playbackSpeed;
                animationProgress = currentTime / totalDuration;
                if (!_animationInProgress)
                {
                    playing = false;
                    _scrubbable = true;
                    animationProgress = 1f;
                }
            }
            if (_scrubbable)
            {
                currentTime = totalDuration * animationProgress;
                ResetVariables();
            }
        }

        private void ResetVariables()
        {
            _nextStartTime = 0f;
            _lastDuration = 0f;
            _currentInputIndex = 0;
            _animationInProgress = false;
        }

        private void RegisterInput()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _inputTimes.Add(currentTime);
                Debug.Log("Input");
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
                _nextStartTime = currentTime;

            }
            _currentInputIndex++;
        }

        protected float CalculateAnimationProgress(float duration, Func<float,float> easeFunction)
        {
            float startTime = _nextStartTime;
            float linearProgress = getLinearProgress(duration, startTime);
            float progress = easeFunction(linearProgress);

            totalDuration = Mathf.Max(totalDuration, startTime + duration);

            return progress;
        }

        protected float CalculateAnimationProgress(float duration, Func<BezierCurves.BezierPoints, float, float> easeFunction, in BezierCurves.BezierPoints points)
        {
            float startTime = _nextStartTime;
            float linearProgress = getLinearProgress(duration, startTime);
            float progress = easeFunction(points, linearProgress);

            totalDuration = Mathf.Max(totalDuration, startTime + duration);

            return progress;
        }

        private float getLinearProgress(float duration, float startTime)
        {
            _lastDuration = duration;

            float timeSinceStart = currentTime - startTime;
            float linearProgress = Mathf.Clamp01(timeSinceStart / duration);
            if (linearProgress < 1)
            {
                _animationInProgress = true;
            }
            return linearProgress;
        }
    }
}
