using UnityEngine;

namespace Taydogmus
{
    public static class Registry
    {
        public static int GetUpgradeLevel(UpgradeType upgradeType)
        {
            return PlayerPrefs.GetInt(upgradeType.ToString(), 0);
        }

        public static void SetUpgradeLevel(UpgradeType upgradeType)
        {
            PlayerPrefs.SetInt(upgradeType.ToString(), GetUpgradeLevel(upgradeType) + 1);
            PlayerPrefs.Save();
        }
    }
}
