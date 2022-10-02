using UnityEngine;
using UnityEngine.EventSystems;

namespace Tooltip.View
{

    /// <summary>
    /// A placer sur les élement d'UI ayant besoin d'afficher le tooltip
    /// </summary>
    public class UITooltipTrigger : MonoBehaviour, ITooltipTrigger, IPointerEnterHandler, IPointerExitHandler
    {
        #region Public Fields

        [field: SerializeField]
        public string Header { get; set; }

        [field: SerializeField, TextArea(3, 10), Space(10)]
        public string Content { get; set; }

        #endregion

        #region Overrides

        public void OnPointerEnter(PointerEventData eventData)
        {
            TooltipView.Show(Header, Content);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            TooltipView.Hide();
        }

        #endregion
    }
}