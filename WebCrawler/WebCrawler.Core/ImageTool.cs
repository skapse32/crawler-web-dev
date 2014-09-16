using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace WebCrawler.Core
{
   public class ImageTool
    {
       public string CropImage(Image img, string croppedFileName, string originalFile, 
           string dirsaveImage, Rectangle pRectangle, string pTitle = "", Stream pLogo = null)
       {
           string extension = "";
            using (Bitmap _bitmap = new Bitmap(pRectangle.Width, pRectangle.Height))
            {
                _bitmap.SetResolution(img.HorizontalResolution, img.VerticalResolution);
                using (Graphics _graphic = Graphics.FromImage(_bitmap))
                {
                    _graphic.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    _graphic.SmoothingMode = SmoothingMode.HighQuality;
                    _graphic.PixelOffsetMode = PixelOffsetMode.HighQuality;
                    _graphic.CompositingQuality = CompositingQuality.HighQuality;
                    _graphic.DrawImage(img, 0, 0, pRectangle.Width, pRectangle.Height);
                    _graphic.DrawImage(img, new Rectangle(0, 0, pRectangle.Width, pRectangle.Height), 
                        pRectangle, GraphicsUnit.Pixel);

                    if (pTitle.Trim() != "")
                    {
                        // For Transparent Watermark Text 
                        int opacity = 255; // range from 0 to 255

                        //SolidBrush brush = new SolidBrush(Color.Red);
                        SolidBrush brush = new SolidBrush(Color.FromArgb(opacity, Color.White));
                        Font font = new Font("Arial", 16);
                        _graphic.DrawString(pTitle, font, brush, new PointF(0, 10));
                    }

                    if (pLogo != null)
                    {
                        Image logoImage = Image.FromStream(pLogo);
                        logoImage = resizeImage(logoImage, new Size(50, 20));
                        _graphic.DrawImage(logoImage,
                            new Point(pRectangle.Width - logoImage.Width - 10, pRectangle.Height - logoImage.Height));
                    }
                    
                    extension = Path.GetExtension(originalFile);


                    // If the image is a gif file, change it into png
                    if (extension.EndsWith("gif", StringComparison.OrdinalIgnoreCase))
                    {
                        extension = ".png";
                    }

                    string newFullPathName = string.Concat(dirsaveImage, croppedFileName, extension);

                    using (EncoderParameters encoderParameters = new EncoderParameters(1))
                    {
                        encoderParameters.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 100L);
                        _bitmap.Save(newFullPathName, GetImageCodec(extension), encoderParameters);
                    }
                }
            }
           return string.Concat(croppedFileName, extension);
       }

       /// <summary>
       /// Find the right codec
       /// </summary>
       /// <param name="extension"></param>
       /// <returns></returns>
       private ImageCodecInfo GetImageCodec(string extension)
       {
           extension = extension.ToUpperInvariant();
           ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
           foreach (ImageCodecInfo codec in codecs)
           {
               if (codec.FilenameExtension.Contains(extension))
               {
                   return codec;
               }
           }
           return codecs[1];
       }

       private Image resizeImage(Image imgToResize, Size size)
       {
           int sourceWidth = imgToResize.Width;
           int sourceHeight = imgToResize.Height;

           float nPercent = 0;
           float nPercentW = 0;
           float nPercentH = 0;

           nPercentW = ((float)size.Width / (float)sourceWidth);
           nPercentH = ((float)size.Height / (float)sourceHeight);

           if (nPercentH < nPercentW)
               nPercent = nPercentH;
           else
               nPercent = nPercentW;

           int destWidth = (int)(sourceWidth * nPercent);
           int destHeight = (int)(sourceHeight * nPercent);

           Bitmap b = new Bitmap(destWidth, destHeight);
           Graphics g = Graphics.FromImage((Image)b);
           g.InterpolationMode = InterpolationMode.HighQualityBicubic;

           g.DrawImage(imgToResize, 0, 0, destWidth, destHeight);
           g.Dispose();

           return (Image)b;
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
