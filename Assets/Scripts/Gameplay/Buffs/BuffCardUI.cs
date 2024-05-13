using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

        [Header("Other Elements")]
        [SerializeField] private float timerToHide;

        #region EventListeners:

        protected override void Awake()
        {
            base.Awake();
            BuffRandomizer.OnBuffRolled += Activate;

            buffCardGO.SetActive(false);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            BuffRandomizer.OnBuffRolled -= Activate;
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

            StartCoroutine(SetVisibilityTimer(timerToHide));

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
            yield return new WaitForSeconds(timeDelay);
            buffCardGO.SetActive(false);
            buffItem = null;
        }
    }
}
