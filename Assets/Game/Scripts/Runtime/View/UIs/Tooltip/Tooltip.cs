using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Project.View.UIs
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

        #endregion

        #region Mono

        private void Start()
        {
            _rt = GetComponent<RectTransform>();
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

        private void SetPosition()
        {
            if (!Application.isPlaying) return;

            Vector2 position = Input.mousePosition;

            float pivotX = position.x / Screen.width;
            float pivotY = position.y / Screen.height;
            float finalPivotX = 0f;
            float finalPivotY = 0f;

            if (pivotX < 0.5) //If mouse on left of screen move tooltip to right of cursor and vice vera
            {
                finalPivotX = -0.1f;
            }
            else
            {
                finalPivotX = 1.01f;
            }

            if (pivotY < 0.5) //If mouse on lower half of screen move tooltip above cursor and vice versa
            {
                finalPivotY = 0;
            }
            else
            {
                finalPivotY = 1;
            }

            _rt.pivot = new Vector2(finalPivotX, finalPivotY);
            _rt.position = position;
        }

        #endregion
    }
}