using UnityEngine;

namespace Taydogmus
{
    public struct UpgradeOffer
    {
        public UpgradeType UpgradeType { get; private set; }
        public float UpgradeValue { get; private set; }
        public string OfferName { get; private set; }
        public string Description { get; private set; }
        public Sprite Icon { get; private set; }
        
        public UpgradeOffer(UpgradeType upgradeType, float upgradeValue, string offerName, string description, Sprite icon)
        {
            UpgradeType = upgradeType;
            UpgradeValue = upgradeValue;
            OfferName = offerName;
            Description = description;
            Icon = icon;
        }
    }
}
