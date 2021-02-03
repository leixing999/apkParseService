using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApkDownloadSerice.domain
{
    public class UrlThreadParameter
    {
        public UrlContentDomain urlDomian {
            get; set;
        }
        public int index
        {
            get; set;
        }
        
        public UrlThreadParameter(UrlContentDomain urlDomian, int index)
        {
            this.index = index;
            this.urlDomian = urlDomian;

        }

    }
}
