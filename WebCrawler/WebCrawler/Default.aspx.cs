using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Facebook;
using WebCrawler.Core;

namespace WebCrawler
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        private void CheckAuthorization()
        {
            string app_id = "374961455917802";
            string app_secret = "9153b340ee604f7917fd57c7ab08b3fa";
            string scope = "publish_stream,manage_pages";

            if (Request["code"] == null)
            {
                Response.Redirect(string.Format(
                    "https://graph.facebook.com/oauth/authorize?client_id={0}&redirect_uri={1}&scope={2}",
                    app_id, Request.Url.AbsoluteUri, scope));
            }
            else
            {
                Dictionary<string, string> tokens = new Dictionary<string, string>();

                string url = string.Format("https://graph.facebook.com/oauth/access_token?client_id={0}&redirect_uri={1}&scope={2}&code={3}&client_secret={4}",
                    app_id, Request.Url.AbsoluteUri, scope, Request["code"].ToString(), app_secret);

                HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;

                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    StreamReader reader = new StreamReader(response.GetResponseStream());

                    string vals = reader.ReadToEnd();

                    foreach (string token in vals.Split('&'))
                    {
                        //meh.aspx?token1=steve&token2=jake&...
                        tokens.Add(token.Substring(0, token.IndexOf("=")),
                            token.Substring(token.IndexOf("=") + 1, token.Length - token.IndexOf("=") - 1));
                    }
                }

                string access_token = tokens["access_token"];

                var client = new FacebookClient(access_token);

                client.Post("/me/feed", new { message = "markhagan.me video tutorial" });
            }
        }

        protected void Button1_OnClick(object sender, EventArgs e)
        {
            HtmlTool ahHtmlTool = new HtmlTool();
            string url = TextBox1.Text;
            string a = "";
            var list = ahHtmlTool.FetchLinksFromSource(url);

            foreach (var astring in list)
            {
                a += "<img src='" + astring +"' />";
            }

            Literal1.Text = a;
        }
    }

    public class PostToWall
    {
        public string Message = "";
        public string AccessToken = "";
        public string ArticleTitle = "";
        public string FacebookProfileID = "";
        public string ErrorMessage { get; private set; }
        public string PostID { get; private set; }
        /// <summary>
        /// Perform the post
        /// </summary>
        public void Post()
        {
            if (string.IsNullOrEmpty(this.Message)) return;
            // Append the user's access token to the URL
            var url = "https://graph.facebook.com/me/feed"
                .AppendQueryString("access_token", this.AccessToken);
            // The POST body is just a collection of key=value pairs, the same way a URL GET string might be formatted
            var parameters = "";
            parameters.AppendQueryString("name", "name")
                .AppendQueryString("link", "http://link.com")
                .AppendQueryString("caption", "a test caption")
                .AppendQueryString("description", "a test description")
                .AppendQueryString("source", "http://blackballsoftware.com/images/whitetheme/headerwhite.png")
                .AppendQueryString("actions", "{\"name\": \"View on Rate-It\", \"link\": \"http://www.rate-it.co.nz\"}")
                .AppendQueryString("privacy", "{\"value\": \"EVERYONE\"}")
                .AppendQueryString("message", this.Message);
            // Mark this request as a POST, and write the parameters to the method body (as opposed to the query string for a GET)
            var webRequest = WebRequest.Create(url);
            webRequest.ContentType = "application/x-www-form-urlencoded";
            webRequest.Method = "POST";
            byte[] bytes = System.Text.Encoding.ASCII.GetBytes(parameters);
            webRequest.ContentLength = bytes.Length;
            System.IO.Stream os = webRequest.GetRequestStream();
            os.Write(bytes, 0, bytes.Length);
            os.Close();
            // Send the request to Facebook, and query the result to get the confirmation code
            try
            {
                var webResponse = webRequest.GetResponse();
                StreamReader sr = null;
                try
                {
                    sr = new StreamReader(webResponse.GetResponseStream());
                    this.PostID = sr.ReadToEnd();
                }
                finally
                {
                    if (sr != null) sr.Close();
                }
            }
            catch (WebException ex)
            {
                // To help with debugging, we grab the exception stream to get full error details
                StreamReader errorStream = null;
                try
                {
                    errorStream = new StreamReader(ex.Response.GetResponseStream());
                    this.ErrorMessage = errorStream.ReadToEnd();
                }
                finally
                {
                    if (errorStream != null) errorStream.Close();
                }
            }
        }
    }
}