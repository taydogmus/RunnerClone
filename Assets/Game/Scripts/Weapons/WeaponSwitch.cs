using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Taydogmus
{
    public class WeaponSwitch : MonoBehaviour
    {
        [SerializeField] private List<BaseWeapon> weapons;
        [SerializeField, Range(0,2)] private int currentWeaponIndex;
        [SerializeField] private Transform weaponHolder;
        [SerializeField] private AnimationCurve weaponSwitchCurve;

        public int CurrentWeaponIndex => currentWeaponIndex;

        private void OnValidate()
        {
            UpdateWeaponIndex();
        }

        private void Start()
        {
            weaponHolder.DOLocalMoveY(.15f, 2f).SetLoops(-1, LoopType.Restart).SetEase(weaponSwitchCurve);
        }

        private void OnDestroy()
        {
            // Although DoTween is in safe mode and handles this error, its good to kill tween before destroying the object.
            weaponHolder.DOKill();
        }

        private void UpdateWeaponIndex()
        {
            DisableCurrentWeapon();
            EnableCurrentWeapon();
        }

        private void EnableCurrentWeapon()
        {
            // To stay in the range of weapons list, we use modulo operator.
            weapons[currentWeaponIndex % weapons.Count].SetStatus(true);
        }

        private void DisableCurrentWeapon()
        {
            foreach (var weapon in weapons)
            {
                weapon.SetStatus(false);
            }
        }
    }
}