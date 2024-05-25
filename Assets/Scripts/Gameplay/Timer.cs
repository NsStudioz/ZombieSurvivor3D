using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZombieSurvivor3D
{
    public class Timer : GameListener
    {

        public static Timer Instance;

        public float timeElapsed { get; private set; }
        [SerializeField] private float timerSpecialEvent;

        [SerializeField] private int SpecialEventMin;
        [SerializeField] private int SpecialEventMax;

        private const float TIMER_RESET = 0f;
        public bool hasStarted { get; private set; }

        protected override void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
                DontDestroyOnLoad(Instance);

            base.Awake();
            EventManager<int>.Register(Events.EventKey.OnPlayerDead.ToString(), StopTimer);
            EventManager<int>.Register(Events.EventKey.OnGameOverButtonsCallback.ToString(), ResetTimer);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            EventManager<int>.Unregister(Events.EventKey.OnPlayerDead.ToString(), StopTimer);
            EventManager<int>.Unregister(Events.EventKey.OnGameOverButtonsCallback.ToString(), ResetTimer);
        }

        // TEST, fix in start game logic/UI:
        private void Start()
        {
            hasStarted = true;
            SetSpecialEventThreshold();
        }

        void Update()
        {
            if (!hasStarted)
                return;

            timeElapsed += Time.deltaTime;

            ApplySpecialEvent();
        }

        private void SetState(bool state)
        {
            hasStarted = state;
        }

        private void StopTimer(int value)
        {
            if (!hasStarted) 
                return;

            SetState(false);
        }

        private void ResetTimer(int value)
        {
            timeElapsed = TIMER_RESET;
            timerSpecialEvent = TIMER_RESET;
        }

        private void SetSpecialEventThreshold()
        {
            timerSpecialEvent = (int)timeElapsed + Random.Range(SpecialEventMin, SpecialEventMax);
        }

        private void ApplySpecialEvent()
        {
            if (timeElapsed >= timerSpecialEvent)
            {
                SetSpecialEventThreshold();
                EventManager<bool>.Raise(Events.EventKey.OnSpecialEvent.ToString(), true);
            }
        }

    }
}
