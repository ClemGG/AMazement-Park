using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Project.Procedural.MazeGeneration;

namespace Project.Services
{

    //Use to browse folders and retrieve the required files
    public static class FileService
    {
        private static readonly List<string> _allowedExtensions = new() { ".txt", ".png", ".jpg", ".jpeg", ".jpe", ".jfif" };
        private static readonly List<string> _names = new();

        public static string GetRandomMask(string nameContent = "")
        {
            _names.Clear();
            string path = $"{Application.streamingAssetsPath}/Masks/";
            DirectoryInfo dir = new(path);

            foreach (string extension in _allowedExtensions)
            {
                FileInfo[] infos = dir.GetFiles($"*.{extension}");
                foreach (FileInfo f in infos)
                {
                    /*The nameContent contains the dimension of the maze.
                     * If it is empty or it has a dimension, we retrieve that name.
                     * This ensure the size of the mask fits the dimension specified
                     * in the difficulty mode.
                     */
                    string fileName = f.Name.Replace(extension, "");
                    if (nameContent == "" ^ fileName.Contains(nameContent))
                    {
                        _names.Add(fileName);
                    }
                }
            }

            return _names.Sample();
        }
    }
}