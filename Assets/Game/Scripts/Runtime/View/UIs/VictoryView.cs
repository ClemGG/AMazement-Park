using System.Collections;
using Project.Models.Scenes;
using Project.ViewModels;
using Project.ViewModels.Scenes;
using TMPro;
using UnityEngine;

/// <summary>
/// Gère l'écran de la scène de victoire
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
    /// Le message de victoire quand le Chasseur est en colère
    /// (une fois tous les 2 niveaux ou quand le Tricheur est utilisé)
    /// </summary>
    [field: SerializeField, TextArea(3, 10)]
    private string AngryVictoryMsg { get; set; }

    /// <summary>
    /// Le message qui s'affiche quand un nouveau personnage doit être ajouté
    /// </summary>
    [field: SerializeField, TextArea(3, 10)]
    private string NewMonsterVictoryMsg { get; set; }

    #endregion

    #region Mono

    private IEnumerator Start()
    {
        /* On passe 5 secondes sur l'écran de victoire.
         * Puis, si le nombre de victoire est pair,
         * ou si le Tricheur a été utilisé,
         * on ajoute un nouveau monstre en jeu
         */

        WaitForSeconds delay = new(5f);
        bool levelNbIsEven = GameSession.NbLevelsWon % (GameSession.NbItemsUsed > 1 ? 2 : 4) == 0;

        if (GameSession.WasCheaterUsed || GameSession.HasReachedTimeLimit || GameSession.WasLastLevel)
        {
            VictoryField.SetText(string.Format(AngryVictoryMsg, GameSession.TimeInMinutesSeconds));
        }
        else
        {
            VictoryField.SetText(string.Format(NormalVictoryMsg, GameSession.TimeInMinutesSeconds));
        }

        yield return delay;

        //Un fois tous les deux niveaux, ou si le Tricheur a été utilisé,
        //on rajoute un monstre au hasard
        if (levelNbIsEven && !GameSession.WasLastLevel || GameSession.ShouldSpawnNewMonster)
        {
            VictoryField.SetText(NewMonsterVictoryMsg);
            GameSession.AddNewRandomMonster();
            yield return delay;
        }

        //Permet d'ajouter un nouveau monstre à la prochaine victoire
        GameSession.WasMonsterSpawnedLastLevel = false;

        //Une fois le message affiché, on peut retourner au jeu
        if (GameSession.WasLastLevel)
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
