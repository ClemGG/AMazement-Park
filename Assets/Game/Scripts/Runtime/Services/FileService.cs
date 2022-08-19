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

        public static (string, string) GetRandomMask(string nameContent = "")
        {
            _names.Clear();
            string path = $"{Application.streamingAssetsPath}/Masks/";
            DirectoryInfo dir = new(path);

            foreach (string extension in _allowedExtensions)
            {
                FileInfo[] infos = dir.GetFiles($"*{extension}");
                foreach (FileInfo f in infos)
                {
                    /*The nameContent contains the dimension of the maze.
                     * If it is empty or it has a dimension, we retrieve that name.
                     * This ensure the size of the mask fits the dimension specified
                     * in the difficulty mode.
                     */
                    if (nameContent == "" ^ f.Name.Contains(nameContent))
                    {
                        _names.Add(f.Name);
                    }
                }
            }
            if (_names.Count == 0)
            {
                //Debug.LogError("Error : No valid mask in the StreamingAssets folder. Defaulting to asciimask.txt");
                return ("asciimask", ".txt");
            }
            string randomName = _names.Sample();
            string ext = $".{randomName.Split('.')[1]}";
            return (randomName.Replace(ext, ""), ext);
        }
    }
}