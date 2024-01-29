using UnityEngine;

namespace Taydogmus
{
    public class CameraManager : MonoBehaviour
    {
        // In case of need
        // Is this the pre-optimization that everyone is talking about?
        // Nah, it's just a singleton that everyone is hatin' on.
        
        public static CameraManager Instance;
        
        [SerializeField] private Camera mainCamera;
        
        private void Awake()
        {
            if(Instance == null)
            {
                Instance = this;
            }
            else if(Instance != null)
            {
                Destroy(gameObject);
            }
            
            EventManager.OnGamePhaseChanged += OnGamePhaseChanged;
        }
        
        private void OnDestroy()
        {
            EventManager.OnGamePhaseChanged -= OnGamePhaseChanged;
        }

        private void OnGamePhaseChanged(GamePhase obj)
        {
            switch (obj)
            {
                case GamePhase.Menu:
                    SwitchMainCameraStatus(true);
                    break;
                case GamePhase.Playing:
                    SwitchMainCameraStatus(false);
                    break;
                case GamePhase.Over:
                    SwitchMainCameraStatus(false);
                    break;
            }
        }

        private void SwitchMainCameraStatus(bool b)
        {
            mainCamera.enabled = b;
        }
    }
}
