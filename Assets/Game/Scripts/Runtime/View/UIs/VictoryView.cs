using System.Collections;
using Project.Models.Scenes;
using Project.ViewModels;
using Project.ViewModels.Scenes;
using TMPro;
using UnityEngine;

/// <summary>
/// G�re l'�cran de la sc�ne de victoire
/// </summary>
public class VictoryView : MonoBehaviour
{
    #region Public Fields

    [field: SerializeField]
    private SceneToLoadParams GameScene { get; set; }

    [field: SerializeField]
    private SceneToLoadParams MenuScene { get; set; }

    [field: SerializeField]
    private TextMeshProUGUI VictoryField { get; set; }

    /// <summary>
    /// Le message normal de victoire
    /// </summary>
    [field: SerializeField, TextArea(3, 10)]
    private string NormalVictoryMsg { get; set; }

    /// <summary>
    /// Le message de victoire quand le Chasseur est en col�re
    /// (une fois tous les 2 niveaux ou quand le Tricheur est utilis�)
    /// </summary>
    [field: SerializeField, TextArea(3, 10)]
    private string AngryVictoryMsg { get; set; }

    /// <summary>
    /// Le message qui s'affiche quand un nouveau personnage doit �tre ajout�
    /// </summary>
    [field: SerializeField, TextArea(3, 10)]
    private string NewMonsterVictoryMsg { get; set; }

    #endregion

    #region Mono

    private IEnumerator Start()
    {
        /* On passe 5 secondes sur l'�cran de victoire.
         * Puis, si le nombre de victoire est pair,
         * ou si le Tricheur a �t� utilis�,
         * on ajoute un nouveau monstre en jeu
         */

        WaitForSeconds delay = new(5f);
        bool levelNbIsEven = GameSession.NbLevelsWon % 2 == 0;
        bool wasLastLevel = GameSession.NbLevelsWon == GameSession.Settings.MaxNumberOfRuns;

        if (GameSession.WasCheaterUsed || wasLastLevel)
        {
            VictoryField.SetText(string.Format(AngryVictoryMsg, GameSession.TimeInMinutesSeconds));
        }
        else
        {
            VictoryField.SetText(string.Format(NormalVictoryMsg, GameSession.TimeInMinutesSeconds));
        }

        yield return delay;

        if (levelNbIsEven && !wasLastLevel)
        {
            VictoryField.SetText(NewMonsterVictoryMsg);
            yield return delay;
        }

        //Une fois le message affich�, on peut retourner au jeu
        if (wasLastLevel)
        {
            SceneMaster.Instance.LoadSingleSceneAsync(MenuScene);
        }
        else
        {
            SceneMaster.Instance.LoadSingleSceneAsync(GameScene);
        }
    }

    #endregion
}
