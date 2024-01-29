using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Taydogmus
{
    [RequireComponent(typeof(WeaponHandler))]
    public class UpgradeHandler : MonoBehaviour
    {
        [SerializeField] private WeaponHandler weaponHandler;
        
        private void Awake()
        {
            EventManager.OnUpgraded += OnUpgraded;
        }
        
        private void OnDestroy()
        {
            EventManager.OnUpgraded -= OnUpgraded;
        }
        
        [Button]
        private void OnUpgraded(UpgradeType type)
        {
            /*print("Upgrading " + type + " to level " + (Registry.GetUpgradeLevel(type) + 1) + 
                  "Old value : " + UpgradeManager.Instance.UpgradeData.GetUpgradeValue(type, Registry.GetUpgradeLevel(type))
                  + "New value : " + UpgradeManager.Instance.UpgradeData.GetUpgradeValue(type, Registry.GetUpgradeLevel(type) + 1));*/
            var upgradeLevel = Registry.GetUpgradeLevel(type);
            Registry.SetUpgradeLevel(type);
            var upgradeValue = UpgradeManager.Instance.UpgradeData.GetUpgradeValue(type, upgradeLevel);
            weaponHandler.GetUpgraded(type, upgradeValue);
        }
        
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Upgrade"))
            {
                other.enabled = false;
                other.transform.DOMoveY(-10, .5f).SetUpdate(true).SetRelative().OnComplete(()=>
                {
                    other.gameObject.SetActive(false);
                });
                EventManager.UpgradeZone();
            }
        }
    }
}