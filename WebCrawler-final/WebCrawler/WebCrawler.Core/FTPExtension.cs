using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.AccessControl;
using System.Text;

namespace WebCrawler.Core
{
    public class FTPExtension
    {

        private string _ftpURL = "testftp.com"; //Host URL or address of the FTP server
        private string _UserName = "admin"; //User Name of the FTP server
        private string _Password = "admin123"; //Password of the FTP server
        private string _ftpDirectory = "Receipts"; //The directory in FTP server where the files are present
        private string _FileName = "test1.csv"; //File name, which one will be downloaded
        private string _LocalDirectory = "D:\\FilePuller"; //Local directory where the files will be downloaded
        string _ftpDirectoryProcessed = "Done"; //The directory in FTP server where the file will be moved

        /// <summary>
        /// Show file list in FTP host
        /// </summary>
        /// <param name="ftpURL">Host URL or address of the FTP server</param>
        /// <param name="UserName">User Name of the FTP server</param>
        /// <param name="Password">Password of the FTP server</param>
        /// <param name="ftpDirectory">Local directory where the files will be show</param>
        /// <returns></returns>
        public List<string> ShowFileList(string ftpURL, string UserName, string Password, string ftpDirectory)
        {
            FtpWebRequest request = (FtpWebRequest) WebRequest.Create(ftpURL + "/" + ftpDirectory);
            request.Credentials = new NetworkCredential(UserName, Password);
            request.Method = WebRequestMethods.Ftp.ListDirectory;
            StreamReader streamReader = new StreamReader(request.GetResponse().GetResponseStream());
            List<string> lines = new List<string>();
            string line;

            while ((line = streamReader.ReadLine()) != null)
            {
                lines.Add(line);
            }
            streamReader.Close();
            return lines;
        }

        /// <summary>
        /// Download file from FTP
        /// </summary>
        /// <param name="ftpURL">Host URL or address of the FTP server</param>
        /// <param name="UserName">User Name of the FTP server</param>
        /// <param name="Password">Password of the FTP server</param>
        /// <param name="ftpDirectory">Local directory where the files on FTP host</param>
        /// <param name="FileName">File name</param>
        /// <param name="LocalDirectory">Local directory where the files will be download</param>
        public void DownloadFile(string ftpURL, string UserName, string Password, string ftpDirectory, string FileName, string LocalDirectory)
        {
            if (!File.Exists(LocalDirectory + "/" + FileName))
            {
                try
                {
                    FtpWebRequest requestFileDownload = (FtpWebRequest)WebRequest.Create(ftpURL + "/" + ftpDirectory + "/" + FileName);
                    requestFileDownload.Credentials = new NetworkCredential(UserName, Password);
                    requestFileDownload.Method = WebRequestMethods.Ftp.DownloadFile;
                    FtpWebResponse responseFileDownload = (FtpWebResponse)requestFileDownload.GetResponse();
                    Stream responseStream = responseFileDownload.GetResponseStream();
                    FileStream writeStream = new FileStream(LocalDirectory + "/" + FileName, FileMode.Create);
                    int Length = 2048;
                    Byte[] buffer = new Byte[Length];
                    int bytesRead = responseStream.Read(buffer, 0, Length);
                    while (bytesRead > 0)
                    {
                        writeStream.Write(buffer, 0, bytesRead);
                        bytesRead = responseStream.Read(buffer, 0, Length);
                    }
                    responseStream.Close();
                    writeStream.Close();
                    requestFileDownload = null;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        /// <summary>
        /// Upload file to FTP
        /// </summary>
        /// <param name="ftpURL">Host URL or address of the FTP server</param>
        /// <param name="UserName">User Name of the FTP server</param>
        /// <param name="Password">Password of the FTP server</param>
        /// <param name="ftpDirectory">Local directory where the files on FTP host</param>
        /// <param name="FileName">File name</param>
        /// <param name="LocalDirectory">Local directory where the files will be upload</param>
        public void UploadFile(string ftpURL, string UserName, string Password, string ftpDirectory, string FileName, string LocalDirectory)
        {
            FtpWebRequest ftpRequest = null;
            Stream ftpStream = null;
            int bufferSize = 2048;
            string path = ftpURL + @"/" + ftpDirectory + @"/" + FileName;
            ftpRequest = (FtpWebRequest)WebRequest.Create(path);
            ftpRequest.Credentials = new NetworkCredential(UserName, Password);
            ftpRequest.UseBinary = true;
            ftpRequest.UsePassive = false;
            ftpRequest.KeepAlive = true;
            ftpRequest.Method = WebRequestMethods.Ftp.UploadFile;
            ftpStream = ftpRequest.GetRequestStream();

            FileStream localUserFileStrem = new FileStream(LocalDirectory, FileMode.Open, FileAccess.ReadWrite);
            byte[] byteBuffer = new byte[bufferSize];
            int bytesSent = localUserFileStrem.Read(byteBuffer, 0, bufferSize);

            try
            {
                while (bytesSent != 0)
                {
                    ftpStream.Write(byteBuffer, 0, bytesSent);
                    bytesSent = localUserFileStrem.Read(byteBuffer, 0, bufferSize);
                }
            }
            catch (Exception ex)
            {
                    
                throw new Exception(ex.Message);
            }

            localUserFileStrem.Close();
            ftpStream.Close();
            ftpRequest = null;
        }

        /// <summary>
        /// Delete file on FTP
        /// </summary>
        /// <param name="ftpURL">Host URL or address of the FTP server</param>
        /// <param name="UserName">User Name of the FTP server</param>
        /// <param name="Password">Password of the FTP server</param>
        /// <param name="ftpDirectory">Local directory where the files on FTP host</param>
        /// <param name="FileName">File name will be delete</param>
        public void DeleteFile(string ftpURL, string UserName, string Password, string ftpDirectory, string FileName)
        {
            FtpWebRequest ftpRequest = (FtpWebRequest)WebRequest.Create(ftpURL + "/" + ftpDirectory + "/" + FileName);
            ftpRequest.Credentials = new NetworkCredential(UserName, Password);
            ftpRequest.Method = WebRequestMethods.Ftp.DeleteFile;
            FtpWebResponse responseFileDelete = (FtpWebResponse)ftpRequest.GetResponse();
        }

        /// <summary>
        /// Moving file to FTP
        /// </summary>
        /// <param name="ftpURL">Host URL or address of the FTP server</param>
        /// <param name="UserName">User Name of the FTP server</param>
        /// <param name="Password">Password of the FTP server</param>
        /// <param name="ftpDirectory">Local directory where the files on FTP host source</param>
        /// <param name="FileName">File name</param>
        /// <param name="ftpDirectoryProcessed">Local directory where the files will be move</param>
        public void MoveFile(string ftpURL, string UserName, string Password, string ftpDirectory, string ftpDirectoryProcessed, string FileName)
        {
            FtpWebRequest ftpRequest = null;
            FtpWebResponse ftpResponse = null;
            try
            {
                ftpRequest = (FtpWebRequest)WebRequest.Create(ftpURL + "/" + ftpDirectory + "/" + FileName);
                ftpRequest.Credentials = new NetworkCredential(UserName, Password);
                ftpRequest.UseBinary = true;
                ftpRequest.UsePassive = true;
                ftpRequest.KeepAlive = true;
                ftpRequest.Method = WebRequestMethods.Ftp.Rename;
                ftpRequest.RenameTo = ftpDirectoryProcessed + "/" + FileName;
                ftpResponse = (FtpWebResponse)ftpRequest.GetResponse();
                ftpResponse.Close();
                ftpRequest = null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Check file exist in FTP
        /// </summary>
        /// <param name="ftpURL">Host URL or address of the FTP server</param>
        /// <param name="UserName">User Name of the FTP server</param>
        /// <param name="Password">Password of the FTP server</param>
        /// <param name="ftpDirectory">Local directory where the files on FTP host source</param>
        /// <param name="FileName">File name</param>
        public bool CheckFileExistOrNot(string ftpURL, string UserName, string Password, string ftpDirectory, string FileName)
        {
            FtpWebRequest ftpRequest = null;
            FtpWebResponse ftpResponse = null;
            bool IsExists = true;
            try
            {
                ftpRequest = (FtpWebRequest)WebRequest.Create(ftpURL + "/" + ftpDirectory + "/" + FileName);
                ftpRequest.Credentials = new NetworkCredential(UserName, Password);
                ftpRequest.Method = WebRequestMethods.Ftp.GetFileSize;
                ftpResponse = (FtpWebResponse)ftpRequest.GetResponse();
                ftpResponse.Close();
                ftpRequest = null;
            }
            catch (Exception ex)
            {
                IsExists = false;
            }
            return IsExists;
        }
    }
}
