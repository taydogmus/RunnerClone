using UnityEngine;
using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;

namespace Taydogmus
{
    [CreateAssetMenu(fileName = "UpgradesData", menuName = "Scriptable Objects/Upgrades/UpgradesData", order = 0)]
    [InlineEditor()]
    public class UpgradeData : ScriptableObject
    {
        public List<UpgradeTypeData> AllUpgrades;
        
        public float GetUpgradeValue(UpgradeType type, int level)
        {
            var upgradeData = AllUpgrades.Find(u => u.upgradeType == type);
            if(upgradeData != null && upgradeData.UpgradeLevels.Count > level)
            {
                return upgradeData.UpgradeLevels[level].Value;
            }
            return 0;
        }
    }

    [Serializable]
    public class UpgradeTypeData
    {
        public UpgradeType upgradeType;
        public List<UpgradeItem> UpgradeLevels;
        public Sprite upgradeSprite;
        public string upgradeName;
        public string upgradeDescription;
    }
    
    [Serializable]
    public struct UpgradeItem
    {
        public float Value;
    }
}