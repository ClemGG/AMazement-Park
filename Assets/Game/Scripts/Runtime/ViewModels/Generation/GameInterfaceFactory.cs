using System;
using Project.Procedural.MazeGeneration;

namespace Project.ViewModels.Generation
{
    //Will create all the interfaces we need (IGeneration, IDraw & ISolwing) for the maze.
    public static class GameInterfaceFactory
    {
        //Same as the previous method but uses a different namespace to take care
        //of the monster and item prefabs
        public static IDrawMethod GetGameDrawMode(GenerationSettingsSO settings)
        {

            Type algType = Type.GetType($"Project.ViewModels.Draw.{settings.DrawMode}Draw");

            //If the constructor is the default one (with no parameters),
            //we don't pass the settings to avoid missing the default constructor
            bool constructorHasParameters = algType.GetConstructors()[0].GetParameters().Length > 0;
            object[] parameters = constructorHasParameters ? new[] { settings } : null;
            IDrawMethod genAlg = (IDrawMethod)Activator.CreateInstance(algType, parameters);

            return genAlg;
        }

    }
}