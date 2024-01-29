using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

namespace Taydogmus
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private Rigidbody rb;
        [SerializeField] private float rayDistance = 1;

        private ObjectPool<Bullet> _pool;
        private Coroutine _lifeCycle;
        private float _lifeTime;
        private BounceType _bounceType;
        private float _damage;
        private int _bounceCount;

        public void SetCharacteristics(BulletProperties properties)
        {
            // Reset bounce count when bullet is reused
            // This might be moved to OnEnable
            _bounceCount = 0;
            var direction = properties.Direction;
            transform.forward = direction;
            rb.velocity = (transform.forward + direction) * properties.Speed;
            _lifeTime = properties.LifeTime;
            _damage = properties.Damage;
            _bounceType = properties.BounceType;
            _lifeCycle = StartCoroutine(DeActivateAfterLifeTime(_lifeTime));
        }

        private void FixedUpdate()
        {
            CheckFront();
        }

        private void CheckFront()
        {
            if (Physics.Raycast(transform.position, transform.forward, out var hit, rayDistance))
            {
                if (hit.collider.CompareTag("Border"))
                {
                    HandleBounce(hit);
                }
                else if (hit.collider.CompareTag("Obstacle"))
                {
                    var canBounce = _bounceType != BounceType.Default;
                    if (canBounce)
                    {
                        HandleBounce(hit);
                    }
                    else
                    {
                        ReturnToPool();
                    }
                }
                else if (hit.collider.CompareTag("Wall"))
                {
                    if (hit.transform.TryGetComponent<Wall>(out var wall))
                    {
                        wall.TakeHit(_damage);
                        ReturnToPool();   
                    }
                }
            }
        }

        private void HandleBounce(RaycastHit hitInfo)
        {
            if(_bounceType == BounceType.Default) return;
            
            var reflectedVelocity = Vector3.Reflect(rb.velocity, hitInfo.normal);
            rb.velocity = reflectedVelocity;
            transform.forward = reflectedVelocity.normalized;
            _lifeTime *= .8f;
            
            if (_bounceType == BounceType.Twice)
            {
                _bounceCount++;
                if (_bounceCount >= 3)
                {
                    ReturnToPool();
                }
            }
        }

        private void ReturnToPool()
        {
            StopCoroutine(DeActivateAfterLifeTime(_lifeTime));
            _pool.Release(this);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position, transform.forward * rayDistance);
        }

        #region Pool
        
        public void SetPool(ObjectPool<Bullet> pool)
        {
            _pool = pool;
        }

        private IEnumerator DeActivateAfterLifeTime(float lTime)
        {
            var elapsedTime = 0f;
            _lifeTime = lTime;
            while (elapsedTime < _lifeTime)
            {
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            
            _pool.Release(this);
        }
        
        #endregion


    }
}