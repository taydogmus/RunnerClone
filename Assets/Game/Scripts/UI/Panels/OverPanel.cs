using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Taydogmus
{
    public class OverPanel : PanelBase
    {
        [SerializeField] private Image bgImage;
        [SerializeField] protected AnimationCurve curve;
        [SerializeField] protected Transform continueButton;
        [SerializeField] protected Transform restartButton;
        
        public override void Show()
        {
            base.Show();
            var isWin = GameManager.Instance.CurrentWinState == WinState.Win;
            if (isWin)
            {
                continueButton.gameObject.SetActive(true);
                continueButton.DOScale(Vector3.one, .2f).SetEase(Ease.InQuint);
            }
            else
            {
                restartButton.DOScale(Vector3.one, .2f).SetEase(Ease.InQuint);
                restartButton.gameObject.SetActive(true);
            }
        }
        
        public override void Hide()
        {
            base.Hide();
            continueButton.localScale = Vector3.zero;
            restartButton.localScale = Vector3.zero;
            continueButton.gameObject.SetActive(false);
            restartButton.gameObject.SetActive(false);
        }
        
        public void OnContinueClicked()
        {
            //Continue
            base.Hide();
            EventManager.LoadNewLevel(true);
        }
        
        public void OnRestartClicked()
        {
            //Restart
            base.Hide();
            EventManager.LoadNewLevel(false);
        }
    }
}
