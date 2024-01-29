using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Taydogmus
{
    public class UpgradeManager : MonoBehaviour
    {
        public static UpgradeManager Instance;
        
        [SerializeField] private UpgradeData upgradeData;
        
        public UpgradeData UpgradeData => upgradeData;

        private void Awake()
        {
            Instance = this;
            
            EventManager.OnUpdateZone += MakeOffers;
        }
        
        private void OnDestroy()
        {
            EventManager.OnUpdateZone -= MakeOffers;
        }

        [Button]
        public void MakeOffers()
        {
            //Get maximum 3 "not max" level upgrades and send offers to UI
            var upgradeTypes = Extensions.GetEnumValuesList<UpgradeType>();
            upgradeTypes.Shuffle();
            List<UpgradeOffer> availableOffers = new List<UpgradeOffer>();

            foreach (var upgradeType in upgradeTypes)
            {
                if (availableOffers.Count >= 3) break;

                if (TryToOfferUpgrade(upgradeType, out var offer))
                {
                    availableOffers.Add(offer);
                }
            }
            
            EventManager.OffersGenerated(availableOffers);
        }

        private bool TryToOfferUpgrade(UpgradeType uType, out UpgradeOffer offer)
        {
            //Find upgrade type in upgrade data
            var upgradeTypeData = upgradeData.AllUpgrades.Find(x => uType == x.upgradeType);
            if (upgradeTypeData != null)
            {
                //Is it max level?
                var maxLevel = UpgradeData.AllUpgrades.Find(x => uType == x.upgradeType).UpgradeLevels.Count;
                var isMaxLevel = Registry.GetUpgradeLevel(uType) >= maxLevel;
                if (!isMaxLevel)
                {
                    //If not max level, get next level value and send offer
                    var offerValue = upgradeData.GetUpgradeValue(uType, Registry.GetUpgradeLevel(uType) + 1);
                    string offerName = upgradeTypeData.upgradeName;
                    string description = upgradeTypeData.upgradeDescription;
                    Sprite icon = upgradeTypeData.upgradeSprite;

                    offer = new UpgradeOffer(uType, offerValue, offerName, description, icon);
                    return true;
                }
            }

            offer = default(UpgradeOffer);
            return false;
        }
    }
}