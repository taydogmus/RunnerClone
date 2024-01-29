using System;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Animations.Rigging;


namespace Taydogmus
{
    public class WeaponHandler : MonoBehaviour
    {
        [SerializeField] private List<BaseWeapon> weapons;
        [SerializeField] private List<Rig> handRigs;
        [SerializeField] private List<Transform> handControlPoints;
        [SerializeField] private BulletSpawner bulletSpawner;
        [SerializeField] private AnimationCurve ikCurve;
        [SerializeField] private float localZOffset = 0.1f;
        [SerializeField] private float angleStep = 30f;
        
        
        private bool _isActive;
        private Transform _firePoint;
        private float _lastShootTime;

        private WeaponData _currentWeaponData;
        [ShowInInspector] private BounceType _bounceType;
        [ShowInInspector] private FormationType _formationType;
        private float _fireRate = 2f;
        private float _bulletDamage;
        private float _bulletSpeed;
        private float _lifeTime = 4f;
        [ShowInInspector] private float _fireRateMultiplier = 1;
        [ShowInInspector] private float _bulletDamageMultiplier = 1;
        private int _currentWeaponIndex;
        
        
        [ShowInInspector]
        private int CurrentWeaponIndex
        {
            get => _currentWeaponIndex;
            set
            {
                DisableCurrentWeapon();
                _currentWeaponIndex = value;
                EnableCurrentWeapon();
            }
        }

        private void EnableCurrentWeapon()
        {
            weapons[CurrentWeaponIndex].SetStatus(true);
            GetWeaponData();
            //UpdateHandPositions(weapons[CurrentWeaponIndex].HandPositions);
        }

        private void UpdateHandPositions(List<Transform> newWeaponHandPositions)
        {
            handControlPoints[1].position = newWeaponHandPositions[1].position;
            handControlPoints[1].rotation = newWeaponHandPositions[1].rotation;
        }

        private void DisableCurrentWeapon()
        {
            weapons[CurrentWeaponIndex].SetStatus(false);
        }

        private void Awake()
        {
            EventManager.OnFirstInput += OnFirstInput;
            EventManager.OnEndReached += OnEndReached;
            EventManager.OnFall += OnFail;
            
            foreach (var weapon in weapons)
            {
                weapon.SetStatus(false);
            }
        }

        private void Start()
        {
            CurrentWeaponIndex = 0;
        }

        private void OnDestroy()
        {
            EventManager.OnFirstInput -= OnFirstInput;
            EventManager.OnEndReached -= OnEndReached;
            EventManager.OnFall -= OnFail;
        }

        private void Update()
        {
            if(!_isActive) return;

            if (Time.time > _lastShootTime + (_fireRate * _fireRateMultiplier))
            {
                ShootBullet();
            }
        }

        private void OnFirstInput()
        {
            _isActive = true;
            _lastShootTime = Time.time - _fireRate;
        }
        
        private void OnEndReached()
        {
            _isActive = false;
            AdjustHandRigs(0);
        }
        
        
        private void OnFail()
        {
            _isActive = false;
            AdjustHandRigs(0);
        }

        private void AdjustHandRigs(float endPoint)
        {
            foreach (var rig in handRigs)
            {
                DOTween.To( () => rig.weight, x => rig.weight = x, endPoint, .1f);
            }
        }

        private void ShootBullet()
        {
            _lastShootTime = Time.time;
            BulletProperties bulletProperties;
            Bullet newBullet;

            foreach (var controlPoint in handControlPoints)
            {
                controlPoint.DOLocalMoveZ( localZOffset, (_fireRate * _fireRateMultiplier) * .7f).SetEase(ikCurve);
            }
            
            switch (_formationType)
            {
                case FormationType.Single:
                    newBullet = bulletSpawner.Pool.Get();
                    newBullet.transform.position = _firePoint.position;
                    bulletProperties = new BulletProperties(transform.forward, _lifeTime, _bulletSpeed, _bulletDamage * _bulletDamageMultiplier, _bounceType);
                    newBullet.SetCharacteristics(bulletProperties);
                    break;
                case FormationType.Triple: // Angle between bullets
                    for (int i = 0; i < 3; i++)
                    {
                        newBullet = bulletSpawner.Pool.Get();
                        newBullet.transform.position = _firePoint.position;

                        // Calculate angle for each bullet
                        float angle = (i - 1) * angleStep; // This will give -45, 0, 45 degrees for i = 0, 1, 2 respectively

                        // Calculate direction using trigonometric functions
                        Vector3 direction = new Vector3(Mathf.Sin(Mathf.Deg2Rad * angle), 0, Mathf.Cos(Mathf.Deg2Rad * angle));
                        direction = transform.rotation * direction; // Rotate direction based on weapon's orientation

                        bulletProperties = new BulletProperties(direction, _lifeTime, _currentWeaponData.travelSpeed, _currentWeaponData.baseBulletDamage * _bulletDamageMultiplier, _bounceType);
                        newBullet.SetCharacteristics(bulletProperties);
                    }
                    break;
                case FormationType.DoubleTriple:
                    for (int j = 0; j < 2; j++)
                    {
                        for (int i = 0; i < 3; i++)
                        {
                            newBullet = bulletSpawner.Pool.Get();
                            newBullet.transform.position = _firePoint.position + Vector3.right * (j * .2f);

                            // Calculate angle for each bullet
                            float angle = (i - 1) * angleStep;

                            // Calculate direction using trigonometric functions
                            Vector3 direction = new Vector3(Mathf.Sin(Mathf.Deg2Rad * angle), 0, Mathf.Cos(Mathf.Deg2Rad * angle));
                            direction = transform.rotation * direction;

                            bulletProperties = new BulletProperties(direction, _lifeTime, _currentWeaponData.travelSpeed, _currentWeaponData.baseBulletDamage * _bulletDamageMultiplier, _bounceType);
                            newBullet.SetCharacteristics(bulletProperties);
                        }
                    }
                    break;
            }
        }
        
        private void GetWeaponData()
        {
            _firePoint = weapons[CurrentWeaponIndex].FirePoint;
            _currentWeaponData = weapons[CurrentWeaponIndex].WeaponData;
            _fireRate = _currentWeaponData.baseFireRate;
            _bulletSpeed = _currentWeaponData.travelSpeed;
            _bulletDamage = _currentWeaponData.baseBulletDamage;
        }
        
        public FormationType GetFormationType()
        {
            return _formationType;
        }
        
        public void GetUpgraded(UpgradeType type, float value)
        {
            switch (type)
            {
                case UpgradeType.FireRate:
                    _fireRateMultiplier = value;
                    break;
                case UpgradeType.BulletDamage:
                    _bulletDamageMultiplier = value;
                    break;
                case UpgradeType.AttackFormation:
                    _formationType = (FormationType) value;
                    break;
                case UpgradeType.Bounce:
                    _bounceType = (BounceType) value;
                    break;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Switch"))
            {
                if (other.transform.TryGetComponent<WeaponSwitch>(out var _switch))
                {
                    CurrentWeaponIndex = _switch.CurrentWeaponIndex;
                    Destroy(other.gameObject);
                }
            }
        }
    }
    
    #region Characteristics

    public struct BulletProperties
    {
        public Vector3 Direction;
        public float LifeTime;
        public float Speed;
        public float Damage;
        public BounceType BounceType;

        public BulletProperties(Vector3 direction, float lifeTime, float speed, float damage, BounceType bounceType)
        {
            Direction = direction;
            LifeTime = lifeTime;
            Speed = speed;
            Damage = damage;
            BounceType = bounceType;
        }
    }

    #endregion
}