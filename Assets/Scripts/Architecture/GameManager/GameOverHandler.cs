using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using ZombieSurvivor3D.Gameplay.GameState;

namespace ZombieSurvivor3D
{
    public class GameOverHandler : MonoBehaviour
    {

        [Header("Elements")]
        [SerializeField] private GameObject m_GameOverUI;

        [Header("Survived Text")]
        [SerializeField] private TextMeshProUGUI m_TimerText;

        [Header("Buttons")]
        [SerializeField] private Button m_RestartBtn;
        [SerializeField] private Button m_MainMenuBtn;

        void Awake()
        {
            m_RestartBtn.onClick.AddListener(RestartGame);
            m_MainMenuBtn.onClick.AddListener(MainMenu);
            EventManager<int>.Register(Events.Gameplay.OnPlayerDead.ToString(), StopGameplayLoop);
            EventManager<string>.Register(Events.Gameplay.OnGameOverResult.ToString(), GetTimeResults);
        }

        private void OnDestroy()
        {
            m_RestartBtn.onClick.RemoveAllListeners();
            m_MainMenuBtn.onClick.RemoveAllListeners();
            EventManager<int>.Unregister(Events.Gameplay.OnPlayerDead.ToString(), StopGameplayLoop);
            EventManager<string>.Unregister(Events.Gameplay.OnGameOverResult.ToString(), GetTimeResults);
        }

        private void StopGameplayLoop(int value)
        {
            m_GameOverUI.SetActive(true);
            GameStateManager.Instance.SetState(GameStateManager.GameState.Gameover);
            Debug.Log("GameState Changed: " + GameStateManager.Instance.CurrentGameState);
        }

        private void GetTimeResults(string text)
        {
            // Invoker = TimerUI.
            m_TimerText.text = "YOU SURVIVED FOR\n\n" + text + "\n\nMINUTES";
        }

        private void RestartGame()
        {
            SetAppearance(false);
            EventResetTimer();
        }

        private void MainMenu()
        {
            SetAppearance(false);
            EventResetTimer();
        }

        private void EventResetTimer()
        {
            EventManager<int>.Raise(Events.Gameplay.OnGameOverButtonsCallback.ToString(), 0);
        }

        private void SetAppearance(bool isVisible)
        {
            m_GameOverUI.SetActive(isVisible);
        }


    }
}
