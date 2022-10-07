namespace Project.Models.Game.Enums
{
    public enum Difficulty : byte
    {
        Random,
        Easy,
        Normal,
        Hard,
        Nightmare,
        Custom,
    }

    public enum EntityType
    {
        Player = 0,
        Monster = 1,
        Item = 2,
        Exit = 3,
    }
}