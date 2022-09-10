using TMPro;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class Tooltip : MonoBehaviour
{
    #region Public Fields

    [field: SerializeField]
    private TextMeshProUGUI HeaderField { get; set; }

    [field: SerializeField]
    private TextMeshProUGUI ContentField { get; set; }

    [field: SerializeField]
    private LayoutElement LayoutElement { get; set; }

    [field: SerializeField]
    [field: Tooltip("A combien de caractères doit-on limiter la largeur du Tooltip ?")]
    private int CharWrapLimit { get; set; }

    #endregion

    #region Mono

    private void Update()
    {
        if (Application.isEditor)
        {
            int headerLength = HeaderField.text.Length;
            int contentLength = ContentField.text.Length;

            //Si notre texte dépasse la limite de caractères,
            //on active le LayoutElement pour limiter la largeur du tooltip
            LayoutElement.enabled = headerLength > CharWrapLimit || contentLength > CharWrapLimit;
        }
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Remplit le tooltip et ajuste sa taille en fonction de celle de son texte
    /// </summary>
    public void SetText(string header = "", string content = "")
    {
        HeaderField.gameObject.SetActive(!string.IsNullOrEmpty(header));
        HeaderField.SetText(header);
        
        ContentField.gameObject.SetActive(!string.IsNullOrEmpty(content));
        ContentField.SetText(content);

        int headerLength = HeaderField.text.Length;
        int contentLength = ContentField.text.Length;

        //Si notre texte dépasse la limite de caractères,
        //on active le LayoutElement pour limiter la largeur du tooltip
        LayoutElement.enabled = headerLength > CharWrapLimit || contentLength > CharWrapLimit;
    }

    #endregion
}
