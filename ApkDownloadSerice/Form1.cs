using ApkDownloadSerice.domain;
using ApkDownloadSerice.services;
using ApkDownloadSerice.utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace ApkDownloadSerice
{
    public partial class apk下载工具 : DevExpress.XtraEditors.XtraForm
    {
        private FileOperationServices fileService = null;

        int cnt = 10;

        public apk下载工具()
        {

            InitializeComponent();
            // System.Windows.Forms.Control.CheckForIllegalCrossThreadCalls = false;
            System.Net.ServicePointManager.DefaultConnectionLimit = 20;
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            string filePath = System.Windows.Forms.Application.StartupPath+ @"\download.txt";

             fileService = new FileOperationServices(filePath);
            /// int rowLines =  FileHelper.GetLineCount(@"C:\file\apk_url_count_0201.txt");
            /// 
            if ("".Equals(this.parseFileText.Text) || "".Equals(this.parseSavePathText.Text))
            {
                MessageBox.Show("请选择解析文件和保存下载目录");
                return;
            }

            string[] rows = FileHelper.GetContentByRows(this.parseFileText.Text);

            StringBuilder build = new StringBuilder();
            List<UrlContentDomain> list = new List<UrlContentDomain>();

            foreach (string row in rows)
            {
                string desUrl = UrlUtils.UrlDecode(row);
                if (desUrl.IndexOf(".apk") > 0 && desUrl.LastIndexOf("http") > 10)
                {
                    desUrl = desUrl.Substring(desUrl.LastIndexOf("http"));
                    desUrl = desUrl.Substring(0, desUrl.IndexOf(".apk") + 4);

                }
                else
                {
                    desUrl = desUrl.Substring(0, desUrl.IndexOf(".apk") + 4);
                }

                UrlContentDomain urlContent = new UrlContentDomain();
                try
                {

                    urlContent.originUrl = row;
                    urlContent.apkUrl = desUrl;
                    urlContent.apkName = desUrl.Substring(desUrl.LastIndexOf(@"/") + 1);
                    if (urlContent.apkName.IndexOf("=") > 0)
                    {
                        urlContent.apkName = urlContent.apkName.Substring(urlContent.apkName.LastIndexOf(@"=") + 1);
                    }
                    urlContent.isValid = "待下载";
                    list.Add(urlContent);
                }
                catch (Exception ex)
                {
                    urlContent.isValid = "链接无效";
                }

            }
         
            for (int i = 0; i < list.Count; i++)
            {
                UrlContentDomain urlContent = list[i];
                if ("待下载".Equals(urlContent.isValid))
                {
                    if (fileService.isFileItem(urlContent.apkName) == false) {
                        Boolean isSuccess = HttpUtils.HttpDownload(urlContent.apkUrl, this.parseSavePathText.Text +@"\"+ urlContent.apkName);
                        if (isSuccess)
                        {
                            list[i].isValid = "下载成功";
                        }
                        else
                        {
                            list[i].isValid = "下载失败";
                        }
                        fileService.append(urlContent.apkName);
                        fileService.addFileItem(urlContent.apkName);
                    }
                    else
                    {
                        list[i].isValid = "下载成功";
                    }
                }

            }


          

            /**
           ThreadPool.SetMinThreads(1, 1);
           ThreadPool.SetMaxThreads(10, 10);
           List<UrlContentDomain> tempList = new List<UrlContentDomain>();
           cnt = list.Count;
           int index = 0;
           for(int j = 0; j < list.Count; j++)
           {

              
               tempList.Add(list[j]);
               if (tempList.Count == 10)
               {
                   for (int i = 0; i < 10; i++)
                   {
                       ThreadPool.QueueUserWorkItem(new WaitCallback(download), new UrlThreadParameter(tempList[i], i));
                   }
                   myEvent.WaitOne();
                   index = 0;
                   tempList.Clear();

               }
               ThreadPool.QueueUserWorkItem(new WaitCallback(download), new UrlThreadParameter(list[j], j));

           }***/


            this.gridControl1.DataSource = list;
        }
        /**
        public void download(Object urlContentParameter)
        {
            cnt -= 1;
            UrlThreadParameter urlThreadObj = (UrlThreadParameter)urlContentParameter;
            UrlContentDomain urlContentDomain = urlThreadObj.urlDomian;
            
            if ("待下载".Equals(urlContentDomain.isValid))
            {
                if (fileService.isFileItem(urlContentDomain.apkName) == false)
                {
                    Boolean isSuccess = HttpUtils.DownloadFile(urlContentDomain.apkUrl, this.parseSavePathText.Text + @"\" + urlContentDomain.apkName);
                    if (isSuccess)
                    {
                        urlContentDomain.isValid = "下载成功";
                    }
                    else
                    {
                        urlContentDomain.isValid = "下载失败";
                    }
                    fileService.append(urlContentDomain.apkName);
                    fileService.addFileItem(urlContentDomain.apkName);
                }
                else
                {
                    urlContentDomain.isValid = "下载成功";
                }
            }
            this.labelControl1.Text = "还剩下：" + this.cnt;
            if (cnt == 0)
            {
                myEvent.Set();
            }

        }***/

        /// <summary>
        /// 解析报文文件按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void parseFileBtn_Click(object sender, EventArgs e)
        {
            if(this.openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                this.parseFileText.Text = this.openFileDialog1.FileName;
            }

        }

        /// <summary>
        /// 保存下载文件结果路径
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveApkBtn_Click(object sender, EventArgs e)
        {
            if (this.xtraFolderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                this.parseSavePathText.Text = this.xtraFolderBrowserDialog1.SelectedPath;
            }

        }

        private void simpleButton1_Click_1(object sender, EventArgs e)
        {
            SaveFileDialog fileDialog = new SaveFileDialog();
            fileDialog.Title = "导出Excel";
            fileDialog.Filter = "Excel文件(*.xls)|*.xls";
            DialogResult dialogResult = fileDialog.ShowDialog(this);
            if (dialogResult == DialogResult.OK)
            {
                DevExpress.XtraPrinting.XlsExportOptions options = new DevExpress.XtraPrinting.XlsExportOptions();
                this.gridControl1.ExportToXls(fileDialog.FileName);
                DevExpress.XtraEditors.XtraMessageBox.Show("导出成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
       
}
