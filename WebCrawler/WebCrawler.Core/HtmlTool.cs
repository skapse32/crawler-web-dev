using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace WebCrawler.Core
{
    public class HtmlTool
    {
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

        public List<string> FetchLinksFromSource(string pUrl)
        {
            List<string> links = new List<string>();
            string regexImgSrc = @"<img[^>]*?data-def-src\s*=\s*[""']?([^'"" >]+?)[ '""][^>]*?>";

            using (var aWebClient = new WebClient())
            {
                aWebClient.Headers.Add("user-agent", "Mozilla/5.0 (iPhone; U; CPU iPhone OS 4_3_2 like Mac OS X; en-us) AppleWebKit/533.17.9 (KHTML, like Gecko) Version/5.0.2 Mobile/8H7 Safari/6533.18.5");
                string htmlSource = aWebClient.DownloadString(pUrl);
                MatchCollection matchesImgSrc = Regex.Matches(htmlSource, regexImgSrc,
                                                                RegexOptions.IgnoreCase | RegexOptions.Singleline);
                foreach (Match m in matchesImgSrc)
                {
                    string href = m.Groups[1].Value;
                    links.Add(href);
                }
            }
            return links;
        }
    }
}
