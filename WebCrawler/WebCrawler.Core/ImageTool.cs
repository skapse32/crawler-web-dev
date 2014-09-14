using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace WebCrawler.Core
{
   public class ImageTool
    {
        public string CropImage(string fileName, string dirsaveImage, int x, int y, int w, int h)
        {
            string filePath = Path.Combine(dirsaveImage, fileName);
            string cropFileName = "";
            string cropFilePath = "";
            if (File.Exists(filePath))
            {
                System.Drawing.Image orgImg = System.Drawing.Image.FromFile(filePath);
                Rectangle aRectangle = new Rectangle(x, y, w, h);
                try
                {
                    Bitmap bitMap = new Bitmap(aRectangle.Width, aRectangle.Height);
                    using (Graphics g = Graphics.FromImage(bitMap))
                    {
                        g.DrawImage(orgImg, new Rectangle(0, 0, bitMap.Width, bitMap.Height), aRectangle,
                            GraphicsUnit.Pixel);
                    }
                    cropFileName = "crop_" + fileName;
                    cropFilePath = Path.Combine(dirsaveImage, cropFileName);
                    bitMap.Save(cropFilePath);
                    return cropFilePath;
                }
                catch
                {
                }
            }
            return cropFilePath;
        }

        public void addTitle(string pfilename, string psavePath, Stream file, string ptitle = "", Stream pLogo = null)
        {
            try
            {
                string fileName = Guid.NewGuid() + Path.GetExtension(pfilename);
                Image upImage = Image.FromStream(file);
                Image logoImage = Image.FromStream(pLogo);

                using (Graphics g = Graphics.FromImage(upImage))
                {

                    // For Transparent Watermark Text 
                    int opacity = 128; // range from 0 to 255

                    //SolidBrush brush = new SolidBrush(Color.Red);
                    SolidBrush brush = new SolidBrush(Color.FromArgb(opacity, Color.Red));
                    Font font = new Font("Arial", 16);
                    if (ptitle.Trim() != "")
                    {
                        g.DrawString(ptitle, font, brush, new PointF(0, 0));
                    }
                    if (pLogo != null)
                    {
                        g.DrawImage(logoImage, new Point(upImage.Width - logoImage.Width - 10, 10));
                    }
                    upImage.Save(psavePath);
                }
            }
            catch (Exception ex)
            {
                throw  new Exception(ex.Message);
            }
            
        }

       public string CropAndAddTitle(Image orgImg, string filename, string dirsaveImage, Rectangle pRectangle,
           string ptitle = "", Stream pLogo = null)
       {
           string cropFilePath = "";
           try
           {
               try
               {
                   Bitmap bitMap = new Bitmap(pRectangle.Width, pRectangle.Height);
                   using (Graphics g = Graphics.FromImage(bitMap))
                   {
                       g.DrawImage(orgImg, new Rectangle(0, 0, bitMap.Width, bitMap.Height), pRectangle,
                           GraphicsUnit.Pixel);

                       if (ptitle.Trim() != "")
                       {
                           // For Transparent Watermark Text 
                           int opacity = 255; // range from 0 to 255

                           //SolidBrush brush = new SolidBrush(Color.Red);
                           SolidBrush brush = new SolidBrush(Color.FromArgb(opacity, Color.White));
                           Font font = new Font("Arial", 16);
                           g.DrawString(ptitle, font, brush, new PointF(0, 10));
                       }

                       if (pLogo != null)
                       {
                           Image logoImage = Image.FromStream(pLogo);
                           g.DrawImage(logoImage,
                               new Point(pRectangle.Width - logoImage.Width - 10, pRectangle.Height - logoImage.Height));
                       }
                   }
                   cropFilePath = Path.Combine(dirsaveImage, filename + ".jpg");
                   bitMap.Save(cropFilePath, ImageFormat.Jpeg);
                   return cropFilePath;
               }
               catch
               {
               }
           }
           catch (Exception ex)
           {

               throw new Exception(ex.Message);
           }

           return cropFilePath;
       }

       public System.Drawing.Image DownloadImageFromUrl(string imageUrl)
        {
            System.Drawing.Image image = null;

            try
            {
                System.Net.HttpWebRequest webRequest = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(imageUrl);
                webRequest.AllowWriteStreamBuffering = true;
                webRequest.Timeout = 30000;

                System.Net.WebResponse webResponse = webRequest.GetResponse();

                System.IO.Stream stream = webResponse.GetResponseStream();

                image = System.Drawing.Image.FromStream(stream);

                webResponse.Close();
            }
            catch (Exception ex)
            {
                return null;
            }

            return image;
        }

        private Image ByteArrayToImage(byte[] fileBytes)
        {
            using (var stream = new MemoryStream(fileBytes))
            {
                return Image.FromStream(stream);
            }
        }
    }
}
