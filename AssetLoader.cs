using System;
using System.Drawing;
using System.IO;

namespace GameEntity
{
    public static class AssetLoader
    {
        public static Image LoadImage(string fileName)
        {
            string[] possiblePaths =
            {
                Path.Combine(AppContext.BaseDirectory, "Assets", fileName),
                Path.Combine(AppContext.BaseDirectory, "assets", fileName),
                Path.Combine(Directory.GetCurrentDirectory(), "Assets", fileName),
                Path.Combine(Directory.GetCurrentDirectory(), "assets", fileName)
            };

            foreach (string path in possiblePaths)
            {
                if (File.Exists(path))
                {
                    using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read))
                    using (var tempImage = Image.FromStream(stream))
                    {
                        return new Bitmap(tempImage);
                    }
                }
            }

            return null;
        }
    }
}