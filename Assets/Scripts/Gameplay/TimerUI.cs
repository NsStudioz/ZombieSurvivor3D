using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using ZombieSurvivor3D.Gameplay.GameState;

namespace ZombieSurvivor3D
{
    public class TimerUI : GameListener
    {
        [Header("Text Elements")]
        [SerializeField] private TextMeshProUGUI timerText;

        [Header("Configurations")]
        [SerializeField] private float effectDelayTime = 1f;
        [SerializeField] private int effectLoopTimes = 5;

        private string stringFormat = "{0:00}:{1:00}:{2:00}";
        private IEnumerator iEnumeratorRef;

        protected override void Awake()
        {
            base.Awake();
            EventManager<bool>.Register(Events.EventKey.OnSpecialEvent.ToString(), SetTextEffect);
            EventManager<int>.Register(Events.EventKey.OnPlayerDead.ToString(), GetTimeResults);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            EventManager<bool>.Unregister(Events.EventKey.OnSpecialEvent.ToString(), SetTextEffect);
            EventManager<int>.Unregister(Events.EventKey.OnPlayerDead.ToString(), GetTimeResults);

            if (iEnumeratorRef != null)
                StopCoroutine(iEnumeratorRef);
        }

        protected override void OnGameStateChanged(GameStateManager.GameState newGameState)
        {
            base.OnGameStateChanged(newGameState);
            
            if (iEnumeratorRef == null)
                return;

            if (newGameState == GameStateManager.GameState.Paused)
                StopCoroutine(iEnumeratorRef);

            else if (newGameState == GameStateManager.GameState.Gameplay)
                StartCoroutine(iEnumeratorRef);
        }

        #region TimeSync:

        void Update()
        {
            SyncTextToTimer();
        }

        private void SyncTextToTimer()
        {
            timerText.text = string.Format(stringFormat, GetHours(), GetMinutes(), GetSeconds());
        }

        // Get will give you hours
        private int GetHours()
        {
            return TimeSpan.FromSeconds(Timer.Instance.timeElapsed).Hours;
        }

        // Get minutes that aren't enough to make a full hour
        private int GetMinutes()
        {
            return TimeSpan.FromSeconds(Timer.Instance.timeElapsed).Minutes;
        }

        // Get seconds that aren't enough for a full min
        private int GetSeconds()
        {
            return TimeSpan.FromSeconds(Timer.Instance.timeElapsed).Seconds;
        }

        private void GetTimeResults(int value)
        {
            EventManager<string>.Raise(Events.EventKey.OnGameOverResult.ToString(), timerText.text);
            // Listener = GameOverHandler.
        }

        #endregion

        /// <summary>
        /// Occurs during special events.
        /// </summary>
        private void SetTextEffect(bool state)
        {
            if (state)
            {
                iEnumeratorRef = ApplyEffect();
                StartCoroutine(iEnumeratorRef);
            }
        }

        // Missing! Scale animations
        private IEnumerator ApplyEffect()
        {
            for (int i = 0; i < effectLoopTimes; i++)
            {
                timerText.color = Color.red;
                yield return new WaitForSeconds(effectDelayTime);
                timerText.color = Color.white;

                if (i > effectLoopTimes)
                {
                    timerText.color = Color.white;
                    break;
                }

                yield return new WaitForSeconds(effectDelayTime);
            }
        }
    }
}

//         private string oldStringFormat = "{00:00:00}";
//timerText.text = string.Format(stringFormat, Timer.Instance.timeElapsed);

//Debug.Log(hours.ToString("00") + ":" + minutes.ToString("00") + ":" + secs.ToString("00"));
//Debug.Log(string.Format("{0:00}:{1:00}:{2:00}", hours, minutes, secs));

