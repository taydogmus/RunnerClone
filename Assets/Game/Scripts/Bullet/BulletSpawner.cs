using UnityEngine;
using UnityEngine.Pool;

namespace Taydogmus
{
    public class BulletSpawner : MonoBehaviour
    {
        private ObjectPool<Bullet> _pool;

        public ObjectPool<Bullet> Pool => _pool;

        [SerializeField] private Bullet bulletPrefab;

        private void Start()
        {
            _pool = new ObjectPool<Bullet>(CreateBullet, OnTakeBulletFromPool, OnReturnBulletToPool, OnDestroyBullet, true,100, 1000);
        }

        private Bullet CreateBullet()
        {
            Bullet bullet = Instantiate(bulletPrefab, GameManager.Instance.transform);
            bullet.SetPool(_pool);
            return bullet;
        }

        private void OnTakeBulletFromPool(Bullet bullet)
        {
            bullet.transform.position = Vector3.down * 10f;
            bullet.gameObject.SetActive(true);
        }
        
        private void OnReturnBulletToPool(Bullet bullet)
        {
            bullet.gameObject.SetActive(false);
        }
        
        private void OnDestroyBullet(Bullet bullet)
        {
            Destroy(gameObject);
        }
    }
}
