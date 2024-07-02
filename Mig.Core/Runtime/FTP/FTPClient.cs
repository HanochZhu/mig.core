using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using UnityEditor.PackageManager.Requests;
using UnityEngine;

namespace Mig.Core
{
    public class FTPClient
    {
        private static string FTPCONSTR = "ftp://183.131.108.168/";//FTP的服务器地址，格式为ftp://192.168.1.234/。ip地址和端口换成自己的，这些建议写在配置文件中，方便修改
        private static string FTPUSERNAME = "mig";//我的FTP服务器的用户名
        private static string FTPPASSWORD = "migassets";//我的FTP服务器的密码
        public static float uploadPercentage;//上传进度
        public static float downloadPercentage;//下载进度
        private static string ftpFileNamePattern = @"([^\s]+)\s*$";
        private static string ftpDirNamePattern = @"([^\s]+)\s*$";

        /// <summary>
        /// File will store at server according to its upload user id
        /// </summary>
        /// <returns></returns>
        public static string GetCurrentFTPDirRoot()
        {
            return Path.Combine(FTPCONSTR, AccountManager.GetCurrentAccountID());
        }


        #region 本地文件上传到FTP服务器
        /// <summary>
        /// 文件上传到ftp
        /// </summary>
        /// <param name="ftpPath">存储上传文件的ftp路径</param>
        /// <param name="localPath">上传文件的本地目录</param>
        /// <param name="fileName">上传文件名称</param>
        /// <returns></returns>
        public static IEnumerator UploadFiles(string ftpPath, string localPath, string fileName, Action<bool> callback)
        {
            //path = "ftp://" + UserUtil.serverip + path;
            float percent = 0;//进度百分比
            FileInfo f = new FileInfo(localPath);
            localPath = localPath.Replace("\\", "/");
            bool b = MakeDir(ftpPath);
            if (b == false)
            {
                callback.Invoke(false);
                yield break;
            }
            localPath = FTPCONSTR + ftpPath + fileName;
            FtpWebRequest reqFtp = CreateAFtpRequest(ftpPath, f.Length);
            int buffLength = 2048;//缓冲区大小
            byte[] buff = new byte[buffLength];//缓冲区
            int contentLen;//存放读取文件的二进制流
            FileStream fs = f.OpenRead(); //以只读方式打开一个文件并从中读取。
            //用于计算进度
            int allByte = (int)f.Length;
            int startByte = 0;
            //try
            {
                Stream stream = reqFtp.GetRequestStream();//将FtpWebRequest转换成stream类型
                contentLen = fs.Read(buff, 0, buffLength);//存放读取文件的二进制流
                //进度条
                while (contentLen != 0)
                {
                    stream.Write(buff, 0, contentLen);
                    startByte = contentLen + startByte;
                    percent = startByte / allByte * 100;
                    if (percent <= 100)
                    {
                        uploadPercentage = percent;
                    }
                    contentLen = fs.Read(buff, 0, buffLength);
                    yield return null;
                }
                //释放资源
                stream.Close();
                fs.Close();

                callback?.Invoke(true);
                yield break;
            }
        }


        /// <summary>
        /// 文件上传到ftp
        /// </summary>
        /// <param name="ftpFilePath">存储上传文件的ftp路径</param>
        /// <param name="localPath">上传文件的本地目录</param>
        /// <param name="fileName">上传文件名称</param>
        /// <returns></returns>
        public static async Task UploadStream(string ftpFilePath, Stream localFileStream, Action<bool> callback)
        {
            // ftpStream where to create where to destroy
            if (!ftpFilePath.StartsWith(FTPCONSTR))
            {
                Debug.Log("[Mig::FTPClient] Failed to upload to " + ftpFilePath);
                callback.Invoke(false);
                return;
            }
            
            try
            {
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ftpFilePath);
                request.Method = WebRequestMethods.Ftp.UploadFile;
                request.Credentials = new NetworkCredential(FTPUSERNAME, FTPPASSWORD);
                // 获取请求流  
                using (Stream requestStream = request.GetRequestStream())
                {
                    byte[] buffer = new byte[1024 * 1024]; // 例如，1MB的缓冲区  
                    int bytesRead;

                    // 循环读取文件并写入请求流，直到读取完成  
                    while ((bytesRead = localFileStream.Read(buffer, 0, buffer.Length)) != 0)
                    {
                        await requestStream.WriteAsync(buffer, 0, bytesRead);
                    }

                    // 刷新数据以确保所有数据都已发送  
                    requestStream.Flush();
                }

                // 获取FTP响应  
                using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
                {
                    Console.WriteLine($"Upload File Complete, status {response.StatusDescription}");
                }

                callback?.Invoke(true);
            }
            catch (Exception ex)
            {
                var errorInfo = string.Format("[Mig::UploadFiles] Failed to upload to ftp server." + ex.Message);
                Debug.Log("[Mig::UploadFiles] Failed to upload to ftp server. " + errorInfo);
                callback?.Invoke(false);
            }
        }

        /// <summary>
        /// 文件上传到ftp
        /// </summary>
        /// <param name="ftpFilePath">存储上传文件的ftp路径</param>
        /// <param name="localPath">上传文件的本地目录</param>
        /// <param name="fileName">上传文件名称</param>
        /// <returns></returns>
        public static async Task UploadBytes(string ftpFilePath, Byte[] bytes, Action<bool> callback)
        {
            // ftpStream where to create where to destroy
            if (!ftpFilePath.StartsWith(FTPCONSTR))
            {
                Debug.Log("[Mig::FTPClient] Failed to upload to " + ftpFilePath);
                callback.Invoke(false);
                return;
            }

            var ftpRequest = CreateAFtpRequest(ftpFilePath, bytes.Length);
            try
            {
                using (Stream stream = ftpRequest.GetRequestStream())
                {
                    await stream.WriteAsync(bytes, 0, bytes.Length);
                }
                callback?.Invoke(true);
            }
            catch (Exception ex)
            {
                Debug.Log($"[Mig::UploadFiles] Failed to upload to ftp server {ftpFilePath}. Error: {ex.Message}");
                ftpRequest.Abort();
                callback?.Invoke(false);
                return ;
            }
            ftpRequest.Abort();
        }

        private static FtpWebRequest CreateAFtpRequest(string address, long length)
        {
            FtpWebRequest reqFtp = (FtpWebRequest)FtpWebRequest.Create(new Uri(address));
            reqFtp.UseBinary = true;
            reqFtp.Credentials = new NetworkCredential(FTPUSERNAME, FTPPASSWORD);
            reqFtp.KeepAlive = false;
            reqFtp.Method = WebRequestMethods.Ftp.UploadFile;
            reqFtp.ContentLength = length;

            return reqFtp;
        }
        #endregion

        #region 从ftp服务器下载文件
        /// <summary>
        /// 从ftp服务器下载文件的功能----带进度条
        /// </summary>
        /// <param name="address">ftp下载的地址</param>
        /// <param name="savePath">保存本地的地址</param>
        /// <param name="fileName">保存的名字</param>
        /// <returns></returns>
        public static async void DownloadToFile(string address, string savePath, Action<bool> callback)
        {
            FtpWebRequest reqFtp = null;
            FtpWebResponse response = null;
            Stream ftpStream = null;
            FileStream outputStream = null;

            try
            {
                address = address.Replace("\\", "/");//字符转换
                if (File.Exists(savePath))//文件是否存在
                {
                    try
                    {
                        File.Delete(savePath);//删除相同文件
                    }
                    catch { }
                }

                string url = Path.Combine(FTPCONSTR, address);//整合访问地址
                reqFtp = (FtpWebRequest)WebRequest.Create(new Uri(url));//为指定的 URI 方案初始化新的 WebRequest 实例。

                reqFtp.UseBinary = true;//指定文件传输的数据类型 true为二进制 false为文本
                reqFtp.Credentials = new NetworkCredential(FTPUSERNAME, FTPPASSWORD);//设置用户名，密码
                response = (FtpWebResponse)await reqFtp.GetResponseAsync();//返回ftp服务器反应 用于转换成FtpWebResponse
                ftpStream = response.GetResponseStream();//返回ftp服务器反应 用于转换成Stream
                long cl = GetFileSize(url);//文件大小
                int bufferSize = 2048;//缓冲区大小
                int readCount;//读取长度
                byte[] buffer = new byte[bufferSize];//新建缓冲区
                readCount = await ftpStream.ReadAsync(buffer, 0, bufferSize);//读取二进制数据（缓冲区，从第几个开始读取，读取总长度）
                outputStream = new FileStream(savePath, FileMode.Create);//创建文件
                float percent = 0;//百分比
                while (readCount > 0)//存在数据时
                {
                    await outputStream.WriteAsync(buffer, 0, readCount);//写入数据
                    readCount = await ftpStream.ReadAsync(buffer, 0, bufferSize);//设置读取长度
                    percent = outputStream.Length / cl * 100;//百分比转换
                    if (percent <= 100)
                    {
                        // Debug.Log(percent);
                        downloadPercentage = percent;
                    }
                }
                callback?.Invoke(true);
            }
            catch (Exception ex)
            {
                Debug.Log("无法下载" + ex.Message);
                //Debug.LogError("因{0},无法下载" + ex.Message);
                callback?.Invoke(false);
            }
            finally
            {
                if (reqFtp != null)
                    reqFtp.Abort();
                if (response != null)
                    response.Close();
                if (ftpStream != null)
                    ftpStream.Close();
                if (outputStream != null)
                    outputStream.Close();
            }
        }


        /// <summary>
        /// 从ftp服务器下载文件的功能----带进度条
        /// </summary>
        /// <param name="address">ftp下载的地址</param>
        /// <param name="savePath">保存本地的地址</param>
        /// <param name="fileName">保存的名字</param>
        /// <returns></returns>
        public static async Task<byte[]> DownloadToBytesAsync(string fromAddress, CancellationToken token)
        {
            FtpWebRequest reqFtp = null;

            try
            {
                reqFtp = (FtpWebRequest)WebRequest.Create(new Uri(fromAddress));//为指定的 URI 方案初始化新的 WebRequest 实例。
                reqFtp.UseBinary = true;//指定文件传输的数据类型 true为二进制 false为文本
                reqFtp.Credentials = new NetworkCredential(FTPUSERNAME, FTPPASSWORD);//设置用户名，密码

                using (FtpWebResponse response = (FtpWebResponse)await reqFtp.GetResponseAsync())
                using (Stream responseStream = response.GetResponseStream())
                using (MemoryStream outputStream = new MemoryStream())
                {
                    long memoryLength = (long)GetFileSize(fromAddress);//文件大小
                    if (memoryLength == -1)
                    {
                        throw new Exception($"File {fromAddress} is not exit"); 
                    }

                    await responseStream.CopyToAsync(outputStream, token);

                    return outputStream.ToArray();
                }
            }
            
            catch (Exception ex)
            {
                Debug.Log($"无法下载 {fromAddress} {ex.Message}");
            }
            finally
            {
                if (reqFtp != null)
                    reqFtp.Abort();
            }
            return new byte[0];
        }

        public static async Task<bool> DownloadToFileAsync(string fromAddress, string toPath, bool isCover = false)
        {
            FtpWebRequest reqFtp = null;

            if (isCover && File.Exists(toPath))
            {
                File.Delete(toPath);
            }

            try
            {
                reqFtp = (FtpWebRequest)WebRequest.Create(new Uri(fromAddress));//为指定的 URI 方案初始化新的 WebRequest 实例。
                reqFtp.UseBinary = true;//指定文件传输的数据类型 true为二进制 false为文本
                reqFtp.Credentials = new NetworkCredential(FTPUSERNAME, FTPPASSWORD);//设置用户名，密码

                using (FtpWebResponse response = (FtpWebResponse)await reqFtp.GetResponseAsync())
                using (Stream responseStream = response.GetResponseStream())
                using (FileStream outputStream = new FileStream(toPath, FileMode.CreateNew))
                {
                    long memoryLength = GetFileSize(fromAddress);//文件大小

                    await responseStream.CopyToAsync(outputStream);
                }

                return true;
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to download {fromAddress} {ex.Message}");
            }
            finally
            {
                if (reqFtp != null)
                    reqFtp.Abort();
            }
            return false;
        }
        #endregion

        #region 获得文件的大小
        /// <summary>
        /// 获得文件大小
        /// </summary>
        /// <param name="url">FTP文件的完全路径</param>
        /// <returns></returns>
        public static long GetFileSize(string url)
        {
            long fileSize = 0;
            try
            {
                FtpWebRequest reqFtp = (FtpWebRequest)WebRequest.Create(new Uri(url));//创建ftp连接
                reqFtp.UseBinary = true;//二进制流
                reqFtp.Credentials = new NetworkCredential(FTPUSERNAME, FTPPASSWORD);//设置访问用户名，密码
                reqFtp.Method = WebRequestMethods.Ftp.GetFileSize;//1.Method设置或获取对文集进行的操作（上传，下载，删除等）  2.GetFileSize检索 FTP 服务器上的文件大小的 FTP SIZE 协议方法
                using (FtpWebResponse response = (FtpWebResponse)reqFtp.GetResponse())
                {
                    fileSize = response.ContentLength;//文件长度
                }//返回ftp服务器反应 用于转换成FtpWebResponse

            }
            catch (WebException ex)
            {
                // 检查异常是否是因为文件不存在  
                if (ex.Status == WebExceptionStatus.ProtocolError && ex.Response is FtpWebResponse)
                {
                    FtpWebResponse errorResponse = (FtpWebResponse)ex.Response;
                    if (errorResponse.StatusCode == FtpStatusCode.ActionNotTakenFileUnavailable)
                    {
                        // 文件不存在  
                        return -1;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.LogError(ex.Message);
            }
            return fileSize;
        }
        #endregion

        #region 在ftp服务器上创建文件目录
        /// <summary>
        ///在ftp服务器上创建文件目录
        /// </summary>
        /// <param name="ftpDirPath">文件目录</param>
        /// <returns></returns>
        public static bool MakeDir(string ftpDirPath)
        {
            try
            {
                if (DirectoryIsExist(ftpDirPath))
                {
                    Debug.Log($"{ftpDirPath} has already exit");
                    return true;
                }

                FtpWebRequest reqFtp = (FtpWebRequest)WebRequest.Create(new Uri(ftpDirPath));
                reqFtp.UseBinary = true;
                reqFtp.Method = WebRequestMethods.Ftp.MakeDirectory;
                reqFtp.Credentials = new NetworkCredential(FTPUSERNAME, FTPPASSWORD);

                using (FtpWebResponse response = (FtpWebResponse)reqFtp.GetResponse()) ;

                return true;
            }
            catch (Exception ex)
            {
                Debug.LogError("因{0},无法下载" + ex.Message);
                return false;
            }
        }
        /// <summary>
        /// 判断ftp上的文件目录是否存在
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>        
        public static bool DirectoryIsExist(string ftpDirPath)
        {
            return GetFileList(ftpDirPath).Length > 0;
        }

        private static string[] GetFileList(string ftpDirPath)
        {
            List<string> result = new();
            try
            {
                FtpWebRequest reqFTP = (FtpWebRequest)WebRequest.Create(ftpDirPath);
                reqFTP.UseBinary = true;
                reqFTP.Credentials = new NetworkCredential(FTPUSERNAME, FTPPASSWORD);
                reqFTP.Method = WebRequestMethods.Ftp.ListDirectoryDetails;

                using (WebResponse response = reqFTP.GetResponse())
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        Debug.Log(line);
                        result.Add(line);
                    }
                }
                return result.ToArray();
            }
            catch
            {
                return null;
            }
        }
        #endregion

        #region 从ftp服务器删除文件的功能
        /// <summary>
        /// 从ftp服务器删除文件的功能
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static bool DeleteFtpFile(string fileName)
        {
            try
            {
                string url = FTPCONSTR + fileName;
                FtpWebRequest reqFtp = (FtpWebRequest)WebRequest.Create(new Uri(url));
                reqFtp.UseBinary = true;
                reqFtp.KeepAlive = false;
                reqFtp.Method = WebRequestMethods.Ftp.DeleteFile;
                reqFtp.Credentials = new NetworkCredential(FTPUSERNAME, FTPPASSWORD);
                FtpWebResponse response = (FtpWebResponse)reqFtp.GetResponse();
                response.Close();
                return true;
            }
            catch (Exception ex)
            {
                var errorinfo = string.Format("因{0},无法下载", ex.Message);
                Debug.Log(errorinfo);
                return false;
            }
        }
        #endregion

        #region  从ftp服务器上获得文件夹列表
        /// <summary>
        /// 从ftp服务器上获得文件夹列表
        /// </summary>
        /// <param name="requestAddress">服务器下的相对路径</param>
        /// <returns></returns>
        public static List<string> GetFTPDirList(string requestAddress)
        {
            List<string> result = new List<string>();
            try
            {
                FtpWebRequest reqFTP = (FtpWebRequest)WebRequest.Create(new Uri(requestAddress));
                // ftp用户名和密码
                reqFTP.Credentials = new NetworkCredential(FTPUSERNAME, FTPPASSWORD);
                reqFTP.Method = WebRequestMethods.Ftp.ListDirectoryDetails;

                using (WebResponse response = reqFTP.GetResponse())
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))//中文文件名
                {
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        Match match = Regex.Match(line, ftpDirNamePattern);
                        if (!match.Success)
                        {
                            Debug.LogError($"Failed to match regex. {line}");
                            continue;
                        }
                        result.Add(match.Value);
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                Debug.LogError("[Mig] Failed to get dir list from website. Detail," + ex.Message);
            }
            return result;
        }
        #endregion

        #region 从ftp服务器上获得文件列表
        /// <summary>
        /// 从ftp服务器上获得文件列表
        /// </summary>
        /// <param name="address">服务器下的相对路径</param>
        /// <returns></returns>
        public static List<string> GetFtpFiles(string address)
        {
            List<string> result = new List<string>();
            FtpWebRequest reqFTP = (FtpWebRequest)WebRequest.Create(new Uri(address));
            // ftp用户名和密码
            reqFTP.Credentials = new NetworkCredential(FTPUSERNAME, FTPPASSWORD);
            reqFTP.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
            try
            {
                using (FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse())
                using (Stream responseStream = response.GetResponseStream())
                using (StreamReader reader = new StreamReader(responseStream))
                {
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        Match match = Regex.Match(line, ftpFileNamePattern);
                        if (match.Success)
                        {
                            result.Add(match.Value);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            return result;
        }

        public static float GetUpLoadPercentage()
        {
            return uploadPercentage;
        }
        public static float GetDownLoadPercentage()
        {
            return downloadPercentage;
        }
        #endregion
    }
}
