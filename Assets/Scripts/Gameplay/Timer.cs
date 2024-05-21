using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZombieSurvivor3D
{
    public class Timer : GameListener
    {

        private float timeElapsed;
        private float timeElapsedSpecialThreshold;

        private int SpecialThresholdMin;
        private int SpecialThresholdMax;

        private const float TIMER_RESET = 0f;
        public bool hasStarted { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            EventManager<bool>.Register(Events.EventKey.OnTimerStateChange.ToString(), SetState);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            EventManager<bool>.Unregister(Events.EventKey.OnTimerStateChange.ToString(), SetState);
        }

        void Update()
        {
            if (!hasStarted)
                return;

            timeElapsed += Time.deltaTime;
        }

        private void SetState(bool state)
        {
            hasStarted = state;
        }

        private void ResetTimer()
        {
            timeElapsed = TIMER_RESET;
            timeElapsedSpecialThreshold = TIMER_RESET;
        }

        private void SetSpecialEventThreshold()
        {
            timeElapsedSpecialThreshold = timeElapsed + Random.Range(SpecialThresholdMin, SpecialThresholdMax);
        }

        private void ApplySpecialEvent()
        {
            if (timeElapsed >= timeElapsedSpecialThreshold)
                EventManager<bool>.Raise(Events.EventKey.OnSpecialEvent.ToString(), true);
        }

    }
}
