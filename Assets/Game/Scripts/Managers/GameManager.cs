using System.Collections.Generic;
using UnityEngine;

namespace Taydogmus
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }
        
        [SerializeField] private List<Level> levels;
        [SerializeField] private GamePhase currentGamePhase;
        
        private Level _currentLevel;
        private WinState _currentWinState;
        private int _currentLevelIndex;
        
        public Level CurrentLevel => _currentLevel;
        public WinState CurrentWinState => _currentWinState;
        public int CurrentLevelIndex => _currentLevelIndex;

        #region Unity Func
        
        private void Awake()
        {
            Application.targetFrameRate = 120;
            
            if(Instance == null)
            {
                Instance = this;
            }
            else if(Instance != null)
            {
                Destroy(gameObject);
            }
            ClearUpgrades();
            
            EventManager.OnPlayPressed += OnPlayPressed;
            EventManager.OnUpgraded += ResumeGame;
            EventManager.OnUpdateZone += PauseGame;
            EventManager.OnEndReached += OnWin;
            EventManager.OnFall += OnLose;
            EventManager.OnLoadNewLevel += OnNewLevelRequested;
        }

        private void OnDestroy()
        {
            EventManager.OnPlayPressed -= OnPlayPressed;
            EventManager.OnUpgraded -= ResumeGame;
            EventManager.OnUpdateZone -= PauseGame;
            EventManager.OnEndReached -= OnWin;
            EventManager.OnFall -= OnLose;
            EventManager.OnLoadNewLevel -= OnNewLevelRequested;
        }

        private void Start()
        {
            _currentLevelIndex = 0;
            SetGamePhase(GamePhase.Menu);
        }
        
        #endregion
        
        private void OnLose()
        {
            ClearUpgrades();
            ProgressGame(false);
        }
        
        private void OnWin()
        { 
            ClearUpgrades();
            ProgressGame(true);
        }
        
        private void OnNewLevelRequested(bool newLevel)
        {
            // From game over screen
            if (newLevel)
            {
                LoadNextLevel();
            }
            else
            {
                ReloadLevel();
            }
        }
        
        private void OnPlayPressed()
        {
            ProgressGame();
        }

        private void ProgressGame(bool isWin = true)
        {
            switch (currentGamePhase)
            {
                case GamePhase.Menu:
                    _currentWinState = WinState.OnGoing;
                    _currentLevelIndex--;
                    LoadNextLevel();
                    break;
                case GamePhase.Playing:
                    _currentWinState = isWin ? WinState.Win : WinState.Lose;
                    SetGamePhase(GamePhase.Over);
                    break;
                case GamePhase.Over:
                    _currentWinState = WinState.OnGoing;
                    SetGamePhase(GamePhase.Menu);
                    break;
            }
        }

        private void ReloadLevel()
        {
            Destroy(_currentLevel.gameObject);
            _currentLevel = Instantiate(levels[_currentLevelIndex], transform);
            SetGamePhase(GamePhase.Playing);
        }

        private void LoadNextLevel()
        {
            if (_currentLevel != null)
            {
                Destroy(_currentLevel.gameObject);   
            }
            _currentLevelIndex++;
            _currentLevelIndex %= levels.Count;
            _currentLevel = Instantiate(levels[_currentLevelIndex], transform);
            SetGamePhase(GamePhase.Playing);
        }
        
        private void SetGamePhase(GamePhase newPhase)
        {
            currentGamePhase = newPhase;
            print("New Game Phase :" + newPhase);
            EventManager.ChangeGamePhase(currentGamePhase);
        }
        
        private void ClearUpgrades()
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
        }

        #region Pause/Unpause

        private void ResumeGame(UpgradeType obj)
        {
            Time.timeScale = 1;
        }

        private void PauseGame()
        {
            Time.timeScale = 0;
        }

        #endregion
    }
}