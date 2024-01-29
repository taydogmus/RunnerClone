using UnityEngine;

namespace Taydogmus
{
    [CreateAssetMenu(fileName = "WeaponData", menuName = "WeaponData", order = 0)]
    public class WeaponData : ScriptableObject
    {
        public float baseFireRate = 1f;
        public float baseBulletDamage = 1f;
        public float travelSpeed = 5f;
    }
}