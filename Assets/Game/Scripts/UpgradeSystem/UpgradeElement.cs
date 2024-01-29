using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Taydogmus
{
    public class UpgradeElement : MonoBehaviour
    {
        [SerializeField] private Image upgradeImage;
        [SerializeField] private TextMeshProUGUI upgradeName;
        [SerializeField] private TextMeshProUGUI upgradeDescription;

        private UpgradeType upgradeType;
        
        public void AdjustOption(UpgradeType uType, Sprite sprite, string name, string description)
        {
            upgradeImage.sprite = sprite;
            upgradeName.text = name;
            upgradeDescription.text = description;
            upgradeType = uType;
        }

        public void OnClick()
        {
            EventManager.Upgraded(upgradeType);
        }
    }
}
