using Project.Models.Game.Enums;

public static class GameSession
{
    public static Difficulty DifficultyLevel { get; set; } = Difficulty.Custom;

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
    public static int[] ActivityLevels { get; set; } = new int[5];
}
