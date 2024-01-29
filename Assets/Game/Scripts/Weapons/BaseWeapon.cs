using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Taydogmus
{
    public abstract class BaseWeapon : MonoBehaviour
    {
        public GameObject model;
        
        [SerializeField] protected Transform firePoint;
        [SerializeField] protected WeaponData weaponData;
        [SerializeField] protected List<Transform> handPositions;

        public List<Transform> HandPositions => handPositions;
        public WeaponData WeaponData => weaponData;
        public Transform FirePoint => firePoint;
        
        [Button]
        public abstract void SetStatus(bool status);
    }
}