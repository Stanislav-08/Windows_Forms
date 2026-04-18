using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Services
{
    public static class Icons
    {
        private static readonly Dictionary<string, Image> _cache = new();

        private static readonly string folderPath =
            Path.Combine(Application.StartupPath, "assets", "icons");

        public static Image Get(string name)
        {
            if (_cache.ContainsKey(name))
                return _cache[name];

            var path = Path.Combine(folderPath, name + "_icon.png");

            if (!File.Exists(path))
                return null;

            var img = Image.FromStream(new MemoryStream(File.ReadAllBytes(path)));
            _cache[name] = img;

            return img;
        }
        public static Image Get(string name, int size)
        {
            var original = Get(name);
            if (original == null) return null;

            var bmp = new Bitmap(size, size);

            using (var g = Graphics.FromImage(bmp))
            {
                g.InterpolationMode =
                    System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

                g.DrawImage(original, 0, 0, size, size);
            }

            return bmp;
        }
    }
}

