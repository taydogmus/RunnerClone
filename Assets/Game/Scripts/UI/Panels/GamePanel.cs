using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Taydogmus
{
    public class GamePanel : PanelBase
    {
        [SerializeField] private Joystick joystick;
        [SerializeField] private VerticalLayoutGroup layoutGroup;
        [SerializeField] private UpgradeElement upgradeElementPrefab;
        
        public Joystick Joystick => joystick;

        private void Awake()
        {
            EventManager.OnOffersGenerated += ShowOffers;
            EventManager.OnUpgraded += OnUpgraded;
            EventManager.OnLoadNewLevel += AdjustToNewLevel;
        }

        private void AdjustToNewLevel(bool obj)
        {
            base.Show();
        }


        private void OnDestroy()
        {
            EventManager.OnOffersGenerated -= ShowOffers;
            EventManager.OnUpgraded -= OnUpgraded;
            EventManager.OnLoadNewLevel -= AdjustToNewLevel;
        }

        private void OnUpgraded(UpgradeType obj)
        {
            //Hide offers panel
            layoutGroup.gameObject.SetActive(false);
            //Reset joystick values to zero
            //So when offer picked, player can move straight
            SetJoystickStatus(true);
        }

        private void ShowOffers(List<UpgradeOffer> availableOffers)
        {
            //Receive offers and show them
            for (int i = 0; i < layoutGroup.transform.childCount; i++)
            {
                Destroy(layoutGroup.transform.GetChild(i).gameObject);
            }
            SetJoystickStatus(false);
            for (int i = 0; i < availableOffers.Count; i++)
            {
                var upgradeElement = Instantiate(upgradeElementPrefab, layoutGroup.transform);
                var offer = availableOffers[i];
                upgradeElement.AdjustOption(offer.UpgradeType, offer.Icon, offer.OfferName, offer.Description);
                upgradeElement.transform.DOScale(Vector3.one, .2f).SetDelay(i * .1f)
                    //Since we stop time scale while offers are available
                    //we set update method true so its independent from timescale.
                    .SetUpdate(true);
            }
            layoutGroup.gameObject.SetActive(true);
        }
        
        
        private void SetJoystickStatus(bool newStatus)
        {
            joystick.gameObject.SetActive(newStatus);
        }
    }
}
