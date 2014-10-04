using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace WebCrawler.Core
{
    public class HtmlTool
    {
        public  string FileName { get; set; }
        public void StartBrowser(string source)
        {
            var th = new Thread(() =>
            {
                var webBrowser = new WebBrowser();
                webBrowser.ScrollBarsEnabled = false;
                webBrowser.DocumentCompleted +=
                    webBrowser_DocumentCompleted;
                webBrowser.DocumentText = source;
                Application.Run();
            });
            th.SetApartmentState(ApartmentState.STA);
            th.Start();
        }

        void webBrowser_DocumentCompleted(
            object sender,
            WebBrowserDocumentCompletedEventArgs e)
        {
            var webBrowser = (WebBrowser)sender;
            using (Bitmap bitmap =
                new Bitmap(
                    webBrowser.Width,
                    webBrowser.Height))
            {
                webBrowser
                    .DrawToBitmap(
                    bitmap,
                    new System.Drawing
                        .Rectangle(0, 0, bitmap.Width, bitmap.Height));
                bitmap.Save(FileName,
                    System.Drawing.Imaging.ImageFormat.Jpeg);
            }
        }

        public void DownloadImage(string pUrl)
        {
            WebClient wchtml = new WebClient();
            string htmlString = wchtml.DownloadString("http://imgur.com/a/GPlx4");
            int mastercount = 0;
            Regex regPattern = new Regex(@"http://i.imgur.com/(.*?)alt=""", RegexOptions.Singleline);
            MatchCollection matchImageLinks = regPattern.Matches(htmlString);

            foreach (Match img_match in matchImageLinks)
            {
                string imgurl = img_match.Groups[1].Value.ToString();
                Regex regx =
                    new Regex(
                        "http://([\\w+?\\.\\w+])+([a-zA-Z0-9\\~\\!\\@\\#\\$\\%\\^\\&amp;\\*\\(\\)_\\-\\=\\+\\\\\\/\\?\\.\\:\\;\\'\\,]*)?",
                        RegexOptions.IgnoreCase);
                MatchCollection ms = regx.Matches(imgurl);
                foreach (Match m in ms)
                {
                    Console.WriteLine("Downloading..  " + m.Value);
                    mastercount++;
                    try
                    {
                        WebClient wc = new WebClient();
                        wc.DownloadFile(m.Value, @"C:\_Images\bg_" + mastercount + ".gif");
                        Thread.Sleep(1000);
                    }
                    catch (Exception x)
                    {
                        Console.WriteLine("Failed to download image.");
                    }
                    break;
                }
            }
        }

        private bool ExistImage(string link, List<string> list)
        {
            if (list.Contains(link))
            {
                return true;
            }
            return false;
        }

        public List<string> FetchLinksFromSource(string pUrl)
        {
            List<string> links = new List<string>();
            try
            {
                string regexImgSrc = "";
                if (pUrl.Contains("www.cars.com"))
                {
                    regexImgSrc = @"<img[^>]*?data-def-src\s*=\s*[""']?([^'"" >]+?)[ '""][^>]*?>";
                }
                else
                {
                    regexImgSrc = @"<img[^>]*?src\s*=\s*[""']?([^'"" >]+?)[ '""][^>]*?>";
                }
                using (var aWebClient = new WebClient())
                {
                    aWebClient.Headers.Add("user-agent",
                        "Mozilla/5.0 (iPhone; U; CPU iPhone OS 4_3_2 like Mac OS X; en-us) AppleWebKit/533.17.9 (KHTML, like Gecko) Version/5.0.2 Mobile/8H7 Safari/6533.18.5");
                    string htmlSource = aWebClient.DownloadString(pUrl);

                    if (pUrl.Contains("www.cars.com"))
                    {
                        MatchCollection matchesImgSrc = Regex.Matches(htmlSource, regexImgSrc,
                            RegexOptions.IgnoreCase | RegexOptions.Singleline);
                        foreach (Match m in matchesImgSrc)
                        {
                            string href = m.Groups[1].Value;

                            if (href.Contains("phototab"))
                            {
                                href = href.Replace("phototab", "supersized");
                                if (!ExistImage(href, links))
                                    links.Add(href);
                            }
                            if (href.Contains("www.cstatic-images.com/stock/900x600") ||
                                href.Contains("www.cstatic-images.com/images"))
                            {
                                if (!ExistImage(href, links))
                                    links.Add(href);
                            }
                        }
                    }
                    else if (pUrl.Contains("www.autotrader.com"))
                    {
                        regexImgSrc = @"<a[^>]*?href\s*=\s*[""']?([^'"" >]+?)[ '""][^>]*?>";
                        MatchCollection matchesImgSrc = Regex.Matches(htmlSource, regexImgSrc,
                            RegexOptions.IgnoreCase | RegexOptions.Singleline);
                        //case 1:
                        foreach (Match m in matchesImgSrc)
                        {
                            string href = m.Groups[1].Value;
                            if (href.Contains("images.autotrader.com"))
                            {
                                string tempUrlImage = "";
                                if (CheckImageSize(pUrl, href, ref tempUrlImage))
                                {
                                    links.Add(tempUrlImage);
                                }
                            }
                        }
                        //case 2:
                        regexImgSrc = @"<img[^>]*?src\s*=\s*[""']?([^'"" >]+?)[ '""][^>]*?>";
                        matchesImgSrc = Regex.Matches(htmlSource, regexImgSrc,
                            RegexOptions.IgnoreCase | RegexOptions.Singleline);
                        foreach (Match m in matchesImgSrc)
                        {
                            string href = m.Groups[1].Value;

                            string tempUrlImage = "";
                            if (CheckImageSize(pUrl, href, ref tempUrlImage))
                            {
                                links.Add(tempUrlImage);
                            }
                        }

                    }
                    else
                    {
                        MatchCollection matchesImgSrc = Regex.Matches(htmlSource, regexImgSrc,
                            RegexOptions.IgnoreCase | RegexOptions.Singleline);
                        foreach (Match m in matchesImgSrc)
                        {
                            string href = m.Groups[1].Value;

                            string tempUrlImage = "";
                            if (CheckImageSize(pUrl, href, ref tempUrlImage))
                            {
                                links.Add(tempUrlImage);
                            }
                        }
                    }
                }
            }
            catch (Exception exception)
            {
              
            }
            
            return links;
        }

        private bool CheckImageSize(string pUrl, string pUrlImage, ref string tempUrlImage)
        {
            bool flag = false;
            string tempUrl = pUrl.Contains("http://")
                ? pUrl.Replace("http://", "")
                : pUrl.Replace("https://", "");
            string[] tempPath = tempUrl.Split('/');
            tempUrlImage = pUrlImage;
            int i = 0;
            do
            {
                try
                {
                    WebRequest request = WebRequest.Create(tempUrlImage);
                    WebResponse response = request.GetResponse();
                    Image image = Image.FromStream(response.GetResponseStream());
                    if (image.Height > 150)
                    {
                        return true;
                    }
                    return false;
                }
                catch (Exception ex)
                {
                    if (i > tempPath.Length - 1)
                        return false;
                    string tmp = "";
                    for (int j = 0; j <= i; j++)
                    {
                        tmp += tempPath[j] + "/";
                    }
                    if (pUrlImage.StartsWith("/"))
                    {
                        tmp = tmp.Substring(0, tmp.Length - 1);

                        tempUrlImage = pUrl.Contains("http://")
                            ? "http://" + tmp + tempUrlImage
                            : "https://" + tmp + tempUrlImage;
                    }
                    else
                    {
                        tempUrlImage = tmp + tempUrlImage;
                    }
                    i++;
                    flag = false;
                }
            } while (!flag);

            return false;
        }
       
    }
}
