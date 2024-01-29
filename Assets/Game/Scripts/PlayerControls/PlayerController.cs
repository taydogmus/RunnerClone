using DG.Tweening;
using UnityEngine;

namespace Taydogmus
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private InputHandler inputHandler;
        [SerializeField] private Collider playerCollider;
        [SerializeField] private float walkSpeed;
        [SerializeField] private Vector2 bounds;
        [SerializeField] private Animator animator;
        
        private bool _isPlaying;
        private Vector3 _movementVector;
        
        private void OnEnable()
        {
            EventManager.OnGamePhaseChanged += OnGamePhaseChanged;
            EventManager.OnFirstInput += OnFirstInput;
            EventManager.OnEndReached += EndReached;
        }

        private void OnDisable()
        {
            EventManager.OnGamePhaseChanged -= OnGamePhaseChanged;
            EventManager.OnFirstInput -= OnFirstInput;
            EventManager.OnEndReached -= EndReached;
        }

        private void OnGamePhaseChanged(GamePhase newPhase)
        {
            //Listen to game phase changes to start/stop playing
            switch (newPhase)
            {
                case GamePhase.Menu:
                    _isPlaying = false;
                    break;
                case GamePhase.Playing:
                    //isPlaying = true;
                    break;
                case GamePhase.Over:
                    _isPlaying = false;
                    break;
            }
        }
        
        private void OnFirstInput()
        {
            _isPlaying = true;
        }
        
        private void Update()
        {
            if (_isPlaying)
            {
                //Take input in update
                TakeInput();
            }
            Animate();
        }

        private void Animate()
        {
            if (_isPlaying)
            {
                var movementDirection = inputHandler.GetMovementVector();
                animator.SetFloat("Side", movementDirection.x);
                animator.SetFloat("Forward", movementDirection.z + .1f);
            }
            else
            {
                animator.SetLayerWeight(1, 0);
                animator.SetFloat("Side", 0);
                animator.SetFloat("Forward", 0);
            }
        }

        private void FixedUpdate()
        {
            if (_isPlaying)
            {
                //Move in fixed update to avoid collision misses
                Move();
            }
        }
        
        private void TakeInput()
        {
            _movementVector = inputHandler != null ? inputHandler.GetMovementVectorClamped() : Vector3.zero;
        }

        private void Move()
        {
            if (playerCollider == null) return;
    
            // Calculate the displacement vector
            var displacement = _movementVector * (walkSpeed * Time.fixedDeltaTime);
            // Predict the next position
            var nextPosition = playerCollider.transform.localPosition + displacement;
            // Clamp the next position within the bounds
            nextPosition.x = Mathf.Clamp(nextPosition.x, -bounds.x, bounds.x);
            nextPosition.z = Mathf.Clamp(nextPosition.z, -bounds.y, bounds.y);
            
            playerCollider.transform.localPosition = nextPosition;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Wall") || other.CompareTag("Obstacle"))
            {
                FailedRun();
                EventManager.FailedRun();
            }
        }

        private void FailedRun()
        {
            playerCollider.enabled = false;
            //Do a minimal reverse bunny hop
            playerCollider.transform.DOLocalJump( playerCollider.transform.localPosition + Vector3.back * 2f
                ,.2f,1,.2f);
            //animator.SetTrigger("Lose");
            animator.CrossFade("Fall",.01f);
        }

        private void EndReached()
        {
            //animator.SetTrigger("Win");
            animator.CrossFade("Cheer",.01f);
        }
    }
}
