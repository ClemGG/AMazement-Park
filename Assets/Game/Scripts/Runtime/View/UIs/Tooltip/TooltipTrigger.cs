using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// A placer sur les élement d'UI ayant besoin d'afficher le tooltip
/// </summary>
public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    #region Public Fields

    [field: SerializeField] 
    private string Header { get; set; }

    [field: SerializeField, TextArea(3, 10), Space(10)]
    private string Content { get; set; }

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
