using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Tooltip.View
{
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

        #region Private Fields

        private RectTransform _rt { get; set; }

        private RectTransform _canvasRT { get; set; }

        #endregion

        #region Mono

        private void Start()
        {
            _rt = GetComponent<RectTransform>();
            _canvasRT = transform.parent.GetComponent<RectTransform>();
        }

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

            SetPosition();
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

        #region Private Methods

        /// <summary>
        /// Place le Tooltip à la position du curseur
        /// en veillant à le garder dans les limites de l'écran
        /// </summary>
        private void SetPosition()
        {
            if (!Application.isPlaying) return;

            Vector2 anchoredPos = Input.mousePosition / _canvasRT.localScale.x;

            //On le descend car son pivot inverse sa position en Y
            anchoredPos.y -= _canvasRT.rect.height;

            //Confine le tooltip à l'écran
            anchoredPos.x = Mathf.Clamp(anchoredPos.x, 0f, _canvasRT.rect.width - _rt.rect.width);
            anchoredPos.y = Mathf.Clamp(anchoredPos.y, _rt.rect.height - _canvasRT.rect.height, 0f);

            _rt.anchoredPosition = anchoredPos;
        }

        #endregion
    }
}