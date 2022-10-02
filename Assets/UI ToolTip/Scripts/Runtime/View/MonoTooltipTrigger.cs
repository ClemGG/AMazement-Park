using UnityEngine;

namespace Tooltip.View
{
    [RequireComponent(typeof(Collider))]
    public class MonoTooltipTrigger : MonoBehaviour, ITooltipTrigger
    {
        #region Public Fields

        [field: SerializeField]
        public string Header { get; set; }

        [field: SerializeField, TextArea(3, 10), Space(10)]
        public string Content { get; set; }

        #endregion


        #region Mono

        private void OnMouseEnter()
        {
            TooltipView.Show(Header, Content);
        }

        private void OnMouseExit()
        {
            TooltipView.Hide();
        }

        #endregion
    }
}