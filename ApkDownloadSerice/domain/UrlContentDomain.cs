using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApkDownloadSerice.domain
{
   
    public class UrlContentDomain
    {
        public string originUrl {
            get; 
            set;
        }

        public string apkUrl
        {
            get;
            set;
        }

        public string apkName
        {
            get;
            set;
        }

        public string isValid
        {
            get;
            set;
        }
    }
}
