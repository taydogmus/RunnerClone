using UnityEngine;
using Sirenix.OdinInspector;

namespace Taydogmus
{
    public class PanelBase : MonoBehaviour
    {
        [SerializeField] private GameObject panel;
        
        [Button]
        public virtual void Hide()
        {
            if(panel != null)
                panel.gameObject.SetActive(false);
        }
        
        [Button]
        public virtual void Show()
        {
            if(panel != null)
                panel.gameObject.SetActive(true);
        }
    }
}
