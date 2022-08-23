using Project.Models.Maze;
using Project.Models.Game.Enums;
using static Enums;
using UnityEngine;
using Project.Procedural.MazeGeneration;

namespace Project.Services
{
    //Changes the maze's settings accordingly with the difficulty.
    //This allows us to add masks, enemies or items without breaking the whole thing
    public static class MazeSettingsService
    {
        private static readonly GenerationType[] _maskAlgs = new GenerationType[]
        {
            GenerationType.AldousBroder,
            GenerationType.GrowingTree,
            GenerationType.Houston,
            GenerationType.HuntAndKill,
            GenerationType.RandomizedKruskal,
            GenerationType.RecursiveBacktracker,
            GenerationType.SimplifiedPrim,
            GenerationType.TruePrim,
            GenerationType.Wilson,
        };


        public static CustomMazeSettingsSO SetupSettings(Difficulty difficulty)
        {
            var settings = Resources.Load<CustomMazeSettingsSO>($"Settings/{difficulty}");
            settings.MaxNumberOfRuns = 10;

            switch (difficulty)
            {
                case Difficulty.Random:
                    SetRandomSettings(settings);
                    break;
                case Difficulty.Easy:
                    SetEasySettings(settings);
                    break;
                case Difficulty.Normal:
                    SetNormalSettings(settings);
                    break;
                case Difficulty.Hard:
                    SetHardSettings(settings);
                    break;
                case Difficulty.Nightmare:
                    SetNightmareSettings(settings);
                    break;
            }

            return settings;
        }


        //The random mode is only used in the Main Menu,
        //but this method is set up to be used for a regular run as well.
        private static void SetRandomSettings(CustomMazeSettingsSO settings)
        {
            //If we have 50% chance, we add a mask and we change the gen type
            //to an algorithm that can handle dead cells in the maze.
            int randMaskChance = 4.Sample();
            if (randMaskChance < 2)
            {
                (string maskName, string extension) = FileService.GetRandomMask();
                settings.MaskName = maskName;
                settings.Extension = extension;
                settings.GenerationType = _maskAlgs.Sample();
            }
            else
            {
                settings.MaskName = "";
                settings.Extension = "";
                settings.GenerationType = GenerationType.Random;
            }

            settings.GridSize = new(30.Sample(5), 30.Sample(5));
            settings.RoomSize = new(settings.GridSize.x.Sample(), settings.GridSize.y.Sample());
            settings.BiasTowardsRooms = RandomSample.RandomBool();
            settings.LambdaSelection = ValuesOf<GrowingTreeLambda>().Sample();
            settings.Inset = .25f.Sample();
            settings.BraidRate = 1f.Sample(.75f);
            settings.HoustonSwapPercent = 1f.Sample();

            settings.PlayerFOV = 9f.Sample(3f) * settings.MeshCellSize.x;
            settings.ActiveItems = new[] { true, RandomSample.RandomBool(), RandomSample.RandomBool() };
            settings.ActivityLevels = new[] { 5.Sample(2), 5.Sample(1), 5.Sample(1), 5.Sample(1), 2.Sample(1) };
        }

        //Easy mode doesn't use masks, all mazes have the default shape.
        //Only one enemy (the Hunter) and the Cheater, all items are enabled by default.
        private static void SetEasySettings(CustomMazeSettingsSO settings)
        {
            settings.GenerationType = GenerationType.Random;
            settings.GridSize = new(10, 10);
            settings.RoomSize = new(settings.GridSize.x.Sample(), settings.GridSize.y.Sample());
            settings.BiasTowardsRooms = RandomSample.RandomBool();
            settings.LambdaSelection = ValuesOf<GrowingTreeLambda>().Sample();
            settings.Inset = 0f;
            settings.BraidRate = 1f;
            settings.HoustonSwapPercent = 1f.Sample();
            settings.ImageMask = null;
            settings.AsciiMask = null;

            settings.MaskName = "";
            settings.PlayerFOV = 9f * settings.MeshCellSize.x;
            settings.ActiveItems = new[] { true, true, true };
            settings.ActivityLevels = new[] { 3, 0, 0, 0, 2 };
        }

        private static void SetNormalSettings(CustomMazeSettingsSO settings)
        {

            //If we have 25% chance, we add a mask and we change the gen type
            //to an algorithm that can handle dead cells in the maze.
            int randMaskChance = 4.Sample();
            if (randMaskChance == 0)
            {
                (string maskName, string extension) = FileService.GetRandomMask("15x15");
                settings.MaskName = maskName;
                settings.Extension = extension;
                settings.GenerationType = _maskAlgs.Sample();
            }
            else
            {
                settings.MaskName = "";
                settings.Extension = "";
                settings.GenerationType = GenerationType.Random;
            }

            settings.GridSize = new(15, 15);
            settings.RoomSize = new(settings.GridSize.x.Sample(), settings.GridSize.y.Sample());
            settings.BiasTowardsRooms = RandomSample.RandomBool();
            settings.LambdaSelection = ValuesOf<GrowingTreeLambda>().Sample();
            settings.Inset = .25f.Sample();
            settings.BraidRate = 1f.Sample(.85f);
            settings.HoustonSwapPercent = 1f.Sample();

            settings.PlayerFOV = 7f * settings.MeshCellSize.x;
            settings.ActiveItems = new[] { true, true, true };
            settings.ActivityLevels = new[] { 3, 3, 3, 3, 2 };
        }

        private static void SetHardSettings(CustomMazeSettingsSO settings)
        {
            //If we have 50% chance, we add a mask and we change the gen type
            //to an algorithm that can handle dead cells in the maze.
            int randMaskChance = 4.Sample();
            if (randMaskChance < 2)
            {
                (string maskName, string extension) = FileService.GetRandomMask("20x20");
                settings.MaskName = maskName;
                settings.Extension = extension;
                settings.GenerationType = _maskAlgs.Sample();
            }
            else
            {
                settings.MaskName = "";
                settings.Extension = "";
                settings.GenerationType = GenerationType.Random;
            }

            settings.GridSize = new(20, 20);
            settings.RoomSize = new(settings.GridSize.x.Sample(), settings.GridSize.y.Sample());
            settings.BiasTowardsRooms = RandomSample.RandomBool();
            settings.LambdaSelection = ValuesOf<GrowingTreeLambda>().Sample();
            settings.Inset = .25f.Sample();
            settings.BraidRate = 1f.Sample(.85f);
            settings.HoustonSwapPercent = 1f.Sample();

            settings.PlayerFOV = 5f * settings.MeshCellSize.x;
            settings.ActiveItems = new[] { true, false, false };
            settings.ActivityLevels = new[] { 4, 4, 4, 4, 2 };
        }

        private static void SetNightmareSettings(CustomMazeSettingsSO settings)
        {
            //If we have 75% chance, we add a mask and we change the gen type
            //to an algorithm that can handle dead cells in the maze.
            int randMaskChance = 4.Sample();
            if (randMaskChance < 3)
            {
                (string maskName, string extension) = FileService.GetRandomMask("25x25");
                settings.MaskName = maskName;
                settings.Extension = extension;
                settings.GenerationType = _maskAlgs.Sample();
            }
            else
            {
                settings.MaskName = "";
                settings.Extension = "";
                settings.GenerationType = GenerationType.Random;
            }

            settings.GridSize = new(25, 25);
            settings.RoomSize = new(settings.GridSize.x.Sample(), settings.GridSize.y.Sample());
            settings.BiasTowardsRooms = RandomSample.RandomBool();
            settings.LambdaSelection = ValuesOf<GrowingTreeLambda>().Sample();
            settings.Inset = .25f.Sample();
            settings.BraidRate = .9f.Sample(.75f);
            settings.HoustonSwapPercent = 1f.Sample();

            settings.PlayerFOV = 3f * settings.MeshCellSize.x;
            settings.ActiveItems = new[] { true, false, false };
            settings.ActivityLevels = new[] { 5, 5, 5, 5, 2 };
        }

    }
}