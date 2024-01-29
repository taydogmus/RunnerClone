using UnityEngine;

namespace Taydogmus
{
    public class CanvasManager : MonoBehaviour
    {
        public static CanvasManager Instance;

        [SerializeField] private MenuPanel menuPanel;
        [SerializeField] private GamePanel gamePanel;
        [SerializeField] private OverPanel overPanel;
        
        public MenuPanel MenuPanel => menuPanel;
        public GamePanel GamePanel => gamePanel;
        public OverPanel OverPanel => overPanel;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else if (Instance != null)
            {
                Destroy(gameObject);
            }
            
            EventManager.OnGamePhaseChanged += OnGamePhaseChanged;
        }
        
        private void OnDestroy()
        {
            EventManager.OnGamePhaseChanged -= OnGamePhaseChanged;
        }

        private void OnGamePhaseChanged(GamePhase newPhase)
        {
            ClosePanels();
            switch (newPhase)
            {
                case GamePhase.Menu:
                    menuPanel.Show();
                    break;
                case GamePhase.Playing:
                    gamePanel.Show();
                    break;
                case GamePhase.Over:
                    overPanel.Show();
                    break;
            }
        }

        private void ClosePanels()
        {
            menuPanel.Hide();
            gamePanel.Hide();
            overPanel.Hide();
        }
    }
}
