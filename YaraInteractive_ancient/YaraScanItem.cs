using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class YaraScanItem
    {
        public string Md5 { get;set; }
        public string Path { get;set; }
        public string OriPath { get;set; }

        public string Ext { get;set; }

        public string mac_name { get;set; }
        public string taskId { get;set; }
    }
}
