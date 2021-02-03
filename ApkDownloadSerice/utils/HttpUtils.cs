using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ApkDownloadSerice.utils
{
   public class HttpUtils
    {
        const long MAX_DOWNSIZE = 100 * 1024*1024;
        ///<summary>
        /// 下载文件
        /// </summary>
        /// <param name="URL">下载文件地址</param>
        /// <param name="Filename">下载后另存为（全路径）</param>
        public static bool DownloadFile(string URL, string filename)
        {
            try
            {
                HttpWebRequest Myrq = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(URL);
                HttpWebResponse myrp = (System.Net.HttpWebResponse)Myrq.GetResponse();
              //  Myrq.Proxy = null;
                /// Myrq.KeepAlive = false;
                //// Myrq.Timeout = 5000;
                /// Myrq.ReadWriteTimeout = 5000;
                Stream st = myrp.GetResponseStream();
                long length =   myrp.ContentLength;
                byte[] by = new byte[1024];
                int osize = st.Read(by, 0, (int)by.Length);
                if(length > MAX_DOWNSIZE)
                {
                    return false;
                }
                Stream so = new System.IO.FileStream(filename, System.IO.FileMode.Create);

                while (osize > 0)
                {
                    so.Write(by, 0, osize);
                    osize = st.Read(by, 0, (int)by.Length);
                }
                so.Flush();
                so.Close();
                st.Close();
                myrp.Close();
                Myrq.Abort();
                return true;
            }
            catch (System.Exception e)
            {
                return false;
            }
        }


        public static bool HttpDownload(string url,string filename)
        {
            System.GC.Collect();
            try
            {
              
                string fileName = Path.GetFileName(url);
                string filePath = filename;
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
                // 设置参数
                HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
               
                //发送请求并获取相应回应数据
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                //直到request.GetResponse()程序才开始向目标网页发送Post请求
                Stream responseStream = response.GetResponseStream();
                request.KeepAlive = false;
                long length = response.ContentLength;
                if (length > MAX_DOWNSIZE)
                {
                    return false;
                }



                FileStream fs = new FileStream(filePath, FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
                //创建本地文件写入流
                byte[] bArr = new byte[1024*10];
                int iTotalSize = 0;
                int size = responseStream.Read(bArr, 0, (int)bArr.Length);
                while (size > 0)
                {
                    iTotalSize += size;
                    fs.Write(bArr, 0, size);
                    size = responseStream.Read(bArr, 0, (int)bArr.Length);
                }
                fs.Close();
                responseStream.Close();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
