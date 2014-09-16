using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using Image = System.Web.UI.WebControls.Image;

namespace WebApplication1
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void btnCrop_Click(object sender, EventArgs e)
        {
            int X1 = Convert.ToInt32(Request.Form["x1"]);
            int Y1 = Convert.ToInt32(Request["y1"]);
            int X2 = Convert.ToInt32(Request.Form["x2"]);
            int Y2 = Convert.ToInt32(Request.Form["y2"]);
            int X = System.Math.Min(X1, X2);
            int Y = System.Math.Min(Y1, Y2);
            int w = Convert.ToInt32(Request.Form["w"]);
            int h = Convert.ToInt32(Request.Form["h"]);

            // That can be any image type (jpg,jpeg,png,gif) from any where in the local server
            string originalFile = Server.MapPath("~/images/02.jpg");


            using (System.Drawing.Image img = System.Drawing.Image.FromFile(originalFile))
            {
                using (System.Drawing.Bitmap _bitmap = new System.Drawing.Bitmap(w, h))
                {
                    _bitmap.SetResolution(img.HorizontalResolution, img.VerticalResolution);
                    using (Graphics _graphic = Graphics.FromImage(_bitmap))
                    {
                        _graphic.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                        _graphic.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                        _graphic.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                        _graphic.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                        _graphic.DrawImage(img, 0, 0, w, h);
                        _graphic.DrawImage(img, new Rectangle(0, 0, w, h), X, Y, w, h, GraphicsUnit.Pixel);

                        string extension = Path.GetExtension(originalFile);
                        string croppedFileName = Guid.NewGuid().ToString();
                        string path = Server.MapPath("~/cropped/");


                        // If the image is a gif file, change it into png
                        if (extension.EndsWith("gif", StringComparison.OrdinalIgnoreCase))
                        {
                            extension = ".png";
                        }

                        string newFullPathName = string.Concat(path, croppedFileName, extension);

                        using (EncoderParameters encoderParameters = new EncoderParameters(1))
                        {
                            encoderParameters.Param[0] = new EncoderParameter(Encoder.Quality, 100L);
                            _bitmap.Save(newFullPathName, GetImageCodec(extension), encoderParameters);
                        }

                        lblCroppedImage.Text = string.Format("<img src='cropped/{0}' alt='Cropped image'>", croppedFileName + extension);
                    }
                }
            }
        }


        /// <summary>
        /// Find the right codec
        /// </summary>
        /// <param name="extension"></param>
        /// <returns></returns>
        public static ImageCodecInfo GetImageCodec(string extension)
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

    }
}