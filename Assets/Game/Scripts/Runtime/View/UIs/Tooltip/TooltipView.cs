using UnityEngine;

/// <summary>
/// Contrôle l'apparition du Tooltip à l'écran
/// quand on survole un élément de la scène
/// </summary>
public class TooltipView : MonoBehaviour
{
    #region Static Fields

    public static TooltipView s_Instance { get; private set; }

    #endregion

    #region Public Fields

    [field: SerializeField]
    private Tooltip Tooltip { get; set; }

    #endregion

    #region Mono

    private void Awake()
    {
        if(s_Instance is not null)
        {
            Destroy(gameObject);
        }

        s_Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    #endregion


    #region Static Methods

    public static void Show(string header = "", string content = "")
    {
        s_Instance.Tooltip.SetText(header, content);
        s_Instance.Tooltip.gameObject.SetActive(true);
    }

    public static void Hide()
    {
        s_Instance.Tooltip.gameObject.SetActive(false);
    }

    #endregion
}
