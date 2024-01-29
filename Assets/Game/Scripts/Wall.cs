using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;

namespace Taydogmus
{
    public class Wall : MonoBehaviour , ILivingEntity
    {
        [SerializeField] private float health;
        [SerializeField] private Collider collider;
        [SerializeField] private MeshRenderer meshRenderer;
        [SerializeField] private AnimationCurve curve;
        [SerializeField] private AnimationCurve colorCurve;
        [SerializeField] private float tweenDuration;
        [SerializeField] private float deltaY;
        [SerializeField] private Gradient color;

        private void Start()
        {
            AdjustColor();
        }

        public void TakeHit(float damage)
        {
            Debug.Log("Wall: TakeHit");
            health -= damage;
            if (health <= 0)
            {
                collider.enabled = false;
                meshRenderer.material.DOKill(true);
                meshRenderer.material.DOColor(Color.clear, tweenDuration);
                transform.DORotate(new Vector3(90f, 0f, 0f), tweenDuration * 1.1f).SetEase(curve).SetRelative().OnComplete(() =>
                {
                    Destroy(gameObject);
                });
                //transform.DOMoveY(deltaY, tweenDuration).SetRelative().SetEase(curve)
            }
            else
            {
                meshRenderer.material.DOKill(true);
                meshRenderer.material.DOColor(Color.red, tweenDuration)
                    .SetLoops(5, LoopType.Restart)
                    .SetEase(colorCurve)
                    .OnComplete(AdjustColor);
            }
        }

        [Button]
        private void AdjustColor()
        {
            var newColor = color.Evaluate(health / 10f);
            meshRenderer.material.color = newColor;
        }
    }
}