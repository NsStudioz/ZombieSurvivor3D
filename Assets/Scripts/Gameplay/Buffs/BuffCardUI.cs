using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using ZombieSurvivor3D.Gameplay.GameState;

namespace ZombieSurvivor3D.Gameplay.Buffs
{
    /// <summary>
    /// UI Card, will appear with animation when triggered by the BuffRandomizer.
    /// </summary>
    public class BuffCardUI : GameListener
    {

        [Header("Buff Item")]
        [SerializeField] private GameObject buffCardGO = null;
        [SerializeField] private BuffsTemplateSO buffItem = null;

        [Header("Buff Card Elements")]
        [SerializeField] private Image cardImg = null; // need to access the sprite.
        //[SerializeField] private Image buffIcon = null; // need to access the sprite.
        [SerializeField] private TextMeshProUGUI nameTxt;
        [SerializeField] private TextMeshProUGUI descriptionTxt;
        [SerializeField] private TextMeshProUGUI durationTxt;

        private IEnumerator iEnumeratorRef;

        [Header("Other Elements")]
        [SerializeField] private float timerToHide;

        #region EventListeners:

        protected override void Awake()
        {
            base.Awake();
            //BuffRandomizer.OnBuffRolled += Activate;
            EventManager<BuffsTemplateSO>.Register(Events.EventKey.OnBuffRoll.ToString(), Activate);

            buffCardGO.SetActive(false);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            //BuffRandomizer.OnBuffRolled -= Activate;
            EventManager<BuffsTemplateSO>.Unregister(Events.EventKey.OnBuffRoll.ToString(), Activate);

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

        #endregion

        /// <summary>
        /// Activate the card, apply the desired stats to the card.
        /// </summary>
        private void Activate(BuffsTemplateSO buffInstance)
        {
            // Activating the buff:
            buffCardGO.SetActive(true);
            buffItem = buffInstance;

            iEnumeratorRef = SetVisibilityTimer(timerToHide);
            //StartCoroutine(SetVisibilityTimer(timerToHide));
            StartCoroutine(iEnumeratorRef);

            // Sync card color with card rarity:
            // might replace this with switch expression:
            // (MUST BE CHANGED TO BETTER COLORS LATER)
            if (buffItem.rarity == BuffsTemplateSO.Rarity.Common)
                cardImg.color = Color.white;
            else if (buffItem.rarity == BuffsTemplateSO.Rarity.Uncommon)
                cardImg.color = Color.cyan;
            else if (buffItem.rarity == BuffsTemplateSO.Rarity.Rare)
                cardImg.color = Color.yellow;

            // Filling with new data:
            //buffIcon.sprite = buffItem.buffSprite;
            nameTxt.text = buffItem.buffName;
            descriptionTxt.text = buffItem.buffDescription;
            durationTxt.text = buffItem.buffDurationUI + " Seconds";
        }

        private IEnumerator SetVisibilityTimer(float timeDelay)
        {
            for (int i = 0; i < 100; i++)
                yield return new WaitForSeconds(timeDelay / 100);
            //yield return new WaitForSeconds(timeDelay);
            buffCardGO.SetActive(false);
            buffItem = null;
        }
    }
}
