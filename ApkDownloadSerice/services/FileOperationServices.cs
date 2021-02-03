using ApkDownloadSerice.utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApkDownloadSerice.services
{
    public class FileOperationServices
    {
        private List<string> list = new List<string>();
        private string filePath ="";

        /// <summary>
        /// 构造函数生成文件总行数
        /// </summary>
        /// <param name="filePath"></param>
        public FileOperationServices(string filePath)
        {
            this.filePath = filePath;
            string[] fileContents = this.getFileContents(filePath);
            list.AddRange(fileContents);
        }

        /// <summary>
        /// 获取文件内容信息
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public string [] getFileContents(string filePath)
        {
           return  FileHelper.GetContentByRows(filePath);
        }

        /// <summary>
        /// 判断是否存在文件项目在目录里
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public Boolean isFileItem(string item)
        {
            var result = list.Exists(t => t == item);
            return result;
        }

        /// <summary>
        /// 添加文件内容项目
        /// </summary>
        /// <param name="item"></param>
        public void addFileItem(string item)
        {
            list.Add(item);
        }

        /// <summary>
        /// 追加文字内容
        /// </summary>
        /// <param name="fileContent"></param>
        public void append(string fileContent)
        {
            FileHelper.AppendText(this.filePath, fileContent);
        }
    }
}
