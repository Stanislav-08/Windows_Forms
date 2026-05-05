using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Services
{
    public static class Icons
    {
        //Cache
        private static Dictionary<string, Image> cache = new Dictionary<string, Image>();

        private static string folderPath = Path.Combine(Application.StartupPath, "assets", "icons");

        public static Image Get(string name)
        {
            if (cache.ContainsKey(name))
            {
                return cache[name];
            }

            var path = Path.Combine(folderPath, name + "_icon.png");

            if (!File.Exists(path))
            {
                return null;
            }

            var img = Image.FromStream(new MemoryStream(File.ReadAllBytes(path)));
            cache[name] = img;

            return img;
        }
        //Icon function
        public static Image Get(string name, int size)
        {
            var original = Get(name);
            if (original == null)
            {
                return null;
            }

            var bmp = new Bitmap(size, size);

            using (var g = Graphics.FromImage(bmp))
            {
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

                g.DrawImage(original, 0, 0, size, size);
            }

            return bmp;
        }
    }
}

