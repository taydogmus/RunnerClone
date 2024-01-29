using System;
using System.Collections.Generic;

namespace Taydogmus
{
    public static class EventManager
    {
        public static Action<GamePhase> OnGamePhaseChanged;
        public static Action<LevelState> OnLevelStateChanged;
        public static Action<bool> OnLoadNewLevel;
        public static Action OnPlayPressed;
        public static Action OnEndReached;
        public static Action OnFall;
        public static Action OnFirstInput;
        public static Action OnUpdateZone;
        public static Action<UpgradeType> OnUpgraded;
        public static Action<List<UpgradeOffer>> OnOffersGenerated;
        public static void ChangeGamePhase(GamePhase newState)
        {
            OnGamePhaseChanged?.Invoke(newState);
        }
        
        public static void PlayPressed()
        {
            OnPlayPressed?.Invoke();
        }
        
        public static void ReachedEnd()
        {
            OnEndReached?.Invoke();
        }
        
        public static void FailedRun()
        {
            OnFall?.Invoke();
        }
        
        public static void FirstInput()
        {
            OnFirstInput?.Invoke();
        }
        
        public static void UpgradeZone()
        {
            OnUpdateZone?.Invoke();
        }
        
        public static void Upgraded(UpgradeType upgradeType)
        {
            OnUpgraded?.Invoke(upgradeType);
        }
        
        public static void OffersGenerated(List<UpgradeOffer> offers)
        {
            OnOffersGenerated?.Invoke(offers);
        }
        
        public static void LevelStateChanged(LevelState newState)
        {
            OnLevelStateChanged?.Invoke(newState);
        }
        
        public static void LoadNewLevel(bool isWin)
        {
            OnLoadNewLevel?.Invoke(isWin);
        }
    }
}
