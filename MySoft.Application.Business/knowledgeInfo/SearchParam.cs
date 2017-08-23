using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MySoft.Application.Business
{
    public class SearchParam
    {
        public string KeyValue { set; get; }
        public int PageIndex { set; get; }
        public int PageSize { set; get; }
        public int TotalCount { set; get; }
        public string DataType;
        public int IsLog { set; get; }
    }
}
