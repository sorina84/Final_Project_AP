using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;

namespace GameEntity
{
    public static class AssetLoader
    {
        public static Image LoadImage(string fileName)
        {
            string path = FindAssetPath(fileName);

            if (path == null)
            {
                Debug.WriteLine("ASSET NOT FOUND: " + fileName);
                return null;
            }

            try
            {
                using (FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read))
                using (Image tempImage = Image.FromStream(stream))
                {
                    return new Bitmap(tempImage);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("ASSET FOUND BUT CANNOT LOAD: " + path);
                Debug.WriteLine(ex.Message);
                return null;
            }
        }

        public static string FindAssetPath(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                return null;

            foreach (string root in GetSearchRoots())
            {
                string directPath = Path.Combine(root, fileName);

                if (File.Exists(directPath))
                    return directPath;

                string assetsPath = Path.Combine(root, "Assets", fileName);

                if (File.Exists(assetsPath))
                    return assetsPath;

                string assetsPathLower = Path.Combine(root, "assets", fileName);

                if (File.Exists(assetsPathLower))
                    return assetsPathLower;

                string foundInAssets = FindFileInsideDirectory(Path.Combine(root, "Assets"), fileName);

                if (foundInAssets != null)
                    return foundInAssets;

                string foundInAssetsLower = FindFileInsideDirectory(Path.Combine(root, "assets"), fileName);

                if (foundInAssetsLower != null)
                    return foundInAssetsLower;
            }

            return null;
        }

        private static List<string> GetSearchRoots()
        {
            List<string> roots = new List<string>();

            AddRootAndParents(roots, AppContext.BaseDirectory);
            AddRootAndParents(roots, Directory.GetCurrentDirectory());

            return roots;
        }

        private static void AddRootAndParents(List<string> roots, string startPath)
        {
            if (string.IsNullOrWhiteSpace(startPath))
                return;

            DirectoryInfo directory = new DirectoryInfo(startPath);

            int safetyCounter = 0;

            while (directory != null && safetyCounter < 15)
            {
                if (!roots.Contains(directory.FullName))
                    roots.Add(directory.FullName);

                directory = directory.Parent;
                safetyCounter++;
            }
        }

        private static string FindFileInsideDirectory(string directoryPath, string fileName)
        {
            if (!Directory.Exists(directoryPath))
                return null;

            try
            {
                string[] files = Directory.GetFiles(directoryPath, "*.*", SearchOption.AllDirectories);

                foreach (string file in files)
                {
                    string currentFileName = Path.GetFileName(file);

                    if (string.Equals(currentFileName, fileName, StringComparison.OrdinalIgnoreCase))
                        return file;
                }
            }
            catch
            {
                return null;
            }

            return null;
        }
    }
}