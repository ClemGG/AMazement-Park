using Project.Procedural.MazeGeneration;
using UnityEngine;

namespace Project.Models.Maze
{
    [CreateAssetMenu(fileName = "New Custom Maze Settings", menuName = "ScriptableObjects/Custom Maze Settings (Use this)", order = 1)]
    public class CustomMazeSettingsSO : GenerationSettingsSO
    {
        [field: SerializeField] public string MaskName { get; set; }

        //In order : Hunter, Omniscient, Embuscade, Useless Moron, Cheater
        //The Hostility levels for each monster.
        /*
         * - 1 : Disabled
           - 2 : The monstes are always in their Wait mode and only activate when the player is in their FOV 
                 (for the Changeling, it always stays disguised and justs flies away, and the Cheater is always at 1 maximum).
           - 3 : The default activity level for all hostile monsters (used in Easy and Normal modes).
           - 4 : Same as Level 3, but the countdowns between each of the monsters' actions decreases over time, 
                 making them more aggressive over time (used in Hard mode).
           - 5 : The monsters are always in Attack mode and their countdonws are reduced to the minimum (used in Nightmare mode).
         */
        [field: SerializeField] public int[] ActivityLevels { get; set; } = new int[5];


        //In order : Key, Map and Tracker
        // 0 : L'objet est d�sactiv� et ne peut pas �tre ramass� (Pour la cl�, 0 et 2 donnent le m�me r�sultat)
        // 1 : L'objet doit �tre ramass� pour �tre activ�
        // 2 : L'objet est actif par d�faut
        [field: SerializeField] public int[] ActiveItems { get; set; } = new int[3];


        [field: SerializeField, Range(3f, 9f)] public float PlayerFOV { get; set; } = 7f;


        //The number of runs to complete a full session.
        //Set to 10 by default for a normal session in custom Mode.
        [field: SerializeField] public int MaxNumberOfRuns { get; set; } = 10;


        /// <summary>
        /// Le temps en secondes avant que les monstres ne restent actifs jusqu'� la fin de la partie
        /// (15 minutes par d�faut)
        /// </summary>
        [field: SerializeField] public float ActiveTimeLimit { get; set; } = 900f;

        protected override void OnValidate()
        {
            base.OnValidate();
            Inset = Mathf.Min(Inset, .25f);
        }
    }
}