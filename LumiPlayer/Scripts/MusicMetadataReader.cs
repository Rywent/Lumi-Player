using LumiPlayer.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using TagLib;

namespace LumiPlayer.Scripts
{
    public class MusicMetadataReader
    {

        public static TrackInfoModel ReadMetadata(string filePath)
        {
            var tFile = TagLib.File.Create(filePath);
            var fileName = Path.GetFileNameWithoutExtension(filePath);
            return new TrackInfoModel
            {
                PathToImage = GetAlbumArt(tFile),
                TrackName = tFile.Tag.Title ?? fileName,
                ExecuteName = string.Join(", ", tFile.Tag.Performers) ?? "No data",
                Duration = tFile.Properties.Duration.ToString("mm\\:ss")
            };
        }

        private static BitmapImage GetAlbumArt(TagLib.File file)
        {
            if(file.Tag.Pictures.Length > 0)
            {
                var picData = file.Tag.Pictures[0].Data.Data;
                using(var ms = new MemoryStream(picData))
                {
                    var image = new BitmapImage();
                    image.BeginInit();
                    image.CacheOption = BitmapCacheOption.OnLoad;
                    image.StreamSource = ms;
                    image.EndInit();
                    return image;
                }
            }
            else
            {
                return SetImageFromPath("/Resources/img/SystemPhoto/photo-not-found.png");
            }
        }

        private static BitmapImage SetImageFromPath(string imagePath)
        {
            var defaultImage = new BitmapImage();
            defaultImage.BeginInit();
            defaultImage.UriSource = new Uri($"pack://application:,,,{imagePath}", UriKind.Absolute);
            defaultImage.CacheOption = BitmapCacheOption.OnLoad;
            defaultImage.EndInit();
            return defaultImage;
        }
    }
}
