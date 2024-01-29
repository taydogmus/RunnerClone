using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;

namespace Taydogmus
{
    public class MenuPanel : PanelBase
    {
        [SerializeField] private Button _playButton;

        public override void Show()
        {
            base.Show();
            _playButton.transform.DOScale(Vector3.one, .5f).SetEase(Ease.InOutSine).OnComplete(() =>
            {
                _playButton.interactable = true;
            });
        }
        
        public override void Hide()
        {
            base.Hide();
            _playButton.interactable = false;
        }


        public void OnPlayButtonClicked()
        {
            EventManager.PlayPressed();
        }
    }
}
