using Dreamteck.Splines;
using UnityEngine;

namespace Taydogmus
{
    public class SplineChaser : MonoBehaviour
    {
        [SerializeField] private SplineFollower follower;
        [SerializeField] private float speed;
        
        private SplineComputer _computer;

        private void Awake()
        {
            EventManager.OnFirstInput += OnFirstInput;
            EventManager.OnFall += Failed;
        }
        
        private void Start()
        {
            SetUp();
        }
        
        private void OnDestroy()
        {
            EventManager.OnFirstInput -= OnFirstInput;
            EventManager.OnFall -= Failed;
        }

        private void Failed()
        {
            follower.follow = false;
        }

        private void OnFirstInput()
        {
            follower.follow = true;
        }

        private void SetUp()
        {
            _computer = GameManager.Instance.CurrentLevel.Computer;
            
            if(_computer == null)
            {
                Debug.LogError("SplineChaser: Computer is null!");
                return;
            }
            follower.spline = _computer;
            follower.followSpeed = speed;
        }

        public void EndReached()
        {
            print("End Reached");
            EventManager.ReachedEnd();
        }
    }
}
