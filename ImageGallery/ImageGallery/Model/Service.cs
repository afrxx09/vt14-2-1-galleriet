using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace ImageGallery.Model
{
    public class Service
    {
        private static readonly Regex ApprovedExtentions;
        private static readonly Regex SantizePath;
        private static readonly string WorkingDir;

        static Service()
        {
            var invalidChars = new string(Path.GetInvalidFileNameChars());
            SantizePath = new Regex(string.Format("[{0}]", Regex.Escape(invalidChars)));
            ApprovedExtentions = new Regex(@"^.*\.(gif|jpg|png)$", RegexOptions.IgnoreCase);
            WorkingDir = Path.Combine(
                AppDomain.CurrentDomain.GetData("APPBASE").ToString(),
                @"Content\Images"
            );
        }

        public IEnumerable<string> GetImageData()
        {
            var di = new DirectoryInfo(WorkingDir);
            return (
                from fi in di.GetFiles()
                where ApprovedExtentions.IsMatch(fi.Extension)
                select fi.Name
                ).AsEnumerable();
        }

        public IEnumerable<string> GetCachedImages()
        {
            var images = HttpContext.Current.Cache["images"] as IEnumerable<string>;
            if (images == null)
            {
                images = GetImageData();
                HttpContext.Current.Cache.Insert("images", images, null, DateTime.Now.AddSeconds(10), TimeSpan.Zero);
            }
            return images;
        }

        public string UploadImage(HttpPostedFile PostedFile)
        {
            if (PostedFile.ContentLength != 0)
            {
                string fileName = PostedFile.FileName.ToLower();
                if (!ApprovedExtentions.IsMatch(fileName))
                {
                    throw new ArgumentOutOfRangeException("Otillåten Filändelse.");
                }
                SantizePath.Replace(fileName, "");

                if (ImageExists(fileName))
                {
                    fileName = RenameImage(fileName);
                }
                string FullPath = WorkingDir + "\\" + fileName;
                PostedFile.SaveAs(FullPath);
                CreateThumbnail(fileName);
                return fileName;
            }
            else
            {
                throw new ArgumentOutOfRangeException("Ingen fil att ladda upp.");
            }
        }

        private string RenameImage(string fileName, int i = 1)
        {
            string ext =  fileName.Substring(fileName.LastIndexOf('.'));
            string name = fileName.Replace(ext, "");
            name = fileName.Replace("("+i+")", "");
            string newFileName = String.Format("{0}({1}){2}", name, i, ext);
            return (!ImageExists(newFileName)) ? newFileName : RenameImage(newFileName, i+1);
        }

        public void CreateThumbnail(string filename)
        {
            Image image = Image.FromFile(WorkingDir + "\\" + filename);
            Rectangle thumbCoords = GetThumbnailCoordinates(image);
            
            /* http://stackoverflow.com/questions/6381310/c-sharp-crop-then-scale-the-cropped-image */
            try
            {
                Bitmap bitmap = new Bitmap(200, 200, PixelFormat.Format24bppRgb);
                Graphics graphics = Graphics.FromImage(bitmap);

                graphics.SmoothingMode = SmoothingMode.AntiAlias;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                graphics.DrawImage(
                    image,
                    new Rectangle(0, 0, 200, 200),
                    thumbCoords,
                    GraphicsUnit.Pixel
                );

                bitmap.Save(WorkingDir + "\\thumbs\\" + filename);
                graphics.Dispose();
            }
            catch
            {
                throw new Exception("Det gick inte skapa en tumnagel.");
            }
        }

        public bool ImageExists(string imageName)
        {
            return File.Exists(WorkingDir + "\\" + imageName);
        }

        private Rectangle GetThumbnailCoordinates(Image i)
        {
            int t = 0;
            int l = 0;
            int w = 0;
            int h = 0;
            if (i.Height > i.Width)
            {
                t = (int)(i.Height - i.Width) / 2;
                h = i.Width;
                w = i.Width;
            }
            else
            {
                l = (int)(i.Width - i.Height) / 2;
                h = i.Height;
                w = i.Height;
            }
            return new Rectangle(l, t, w, h);
        }
    }
}