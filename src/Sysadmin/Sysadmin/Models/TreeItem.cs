using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysAdmin.Models
{
    public class TreeItem
    {

        public string Name { get; set; }
        public string DistinguishedName { get; set; }
        public List<TreeItem> Items { get; set; }

    }
}