using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ZombieSurvivor3D
{
    public class MainMenuHandler : MonoBehaviour
    {

        [Header("Buttons")]
        [SerializeField] private Button m_PlayBtn;
        [SerializeField] private Button m_OptionsBtn;
        [SerializeField] private Button m_CreditsBtn;
        [SerializeField] private Button m_QuitBtn;

        [Header("Buttons")]
        [SerializeField] private Button m_ChildBackButton;

        [Header("Parent Menus")]
        [SerializeField] private GameObject m_MainMenu;
        [SerializeField] private GameObject m_MenuWindow;

        [Header("Child Menus")]
        [SerializeField] private GameObject m_SelectLevel;
        [SerializeField] private GameObject m_Options;
        [SerializeField] private GameObject m_Credits;

        private bool isChildMenuShown;

        void Awake()
        {
            m_PlayBtn.onClick.AddListener(ShowSelectLevelMenu);
            m_OptionsBtn.onClick.AddListener(ShowOptionsMenu);
            m_CreditsBtn.onClick.AddListener(ShowCreditsMenu);
            //
            m_ChildBackButton.onClick.AddListener(ShowMainMenu);
            m_QuitBtn.onClick.AddListener(QuitGame);
        }

        #region Helpers:

        /// <summary>
        /// Show/Hide parent menus.
        /// </summary>
        /// <param name="mainMenu"></param>
        /// <param name="otherMenu"></param>
        private void SetParentMenuAppearance(bool mainMenu, bool otherMenu)
        {
            if (isChildMenuShown)
                return;

            m_MainMenu.SetActive(mainMenu);
            m_MenuWindow.SetActive(otherMenu);
            SetBackBtnAppearance(false);
        }

        /// <summary>
        /// Show/Hide child menus.
        /// </summary>
        /// <param name="selectLevel"></param>
        /// <param name="options"></param>
        /// <param name="credits"></param>
        private void SetChildMenuAppearance(bool selectLevel, bool options, bool credits)
        {
            m_SelectLevel.SetActive(selectLevel);
            m_Credits.SetActive(credits);
            m_Options.SetActive(options);

            if (isChildMenuShown)
                return;

            SetBackBtnAppearance(true);
            isChildMenuShown = true;
        }

        private void SetBackBtnAppearance(bool backbtn)
        {
            m_ChildBackButton.gameObject.SetActive(backbtn);
        }

        #endregion

        private void ShowSelectLevelMenu()
        {
            SetParentMenuAppearance(false, true);
            SetChildMenuAppearance(true, false, false);
        }

        private void ShowOptionsMenu()
        {
            SetParentMenuAppearance(false, true);
            SetChildMenuAppearance(false, true, false);
        }

        private void ShowCreditsMenu()
        {
            SetParentMenuAppearance(false, true);
            SetChildMenuAppearance(false, false, true);
        }

        private void ShowMainMenu()
        {
            isChildMenuShown = false;
            SetParentMenuAppearance(true, false);
        }

        private void QuitGame()
        {
            Application.Quit();
        }

    }
}
