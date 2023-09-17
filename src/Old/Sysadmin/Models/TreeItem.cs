using System.Collections.Generic;

namespace SysAdmin.Models
{
    public class TreeItem
    {

        public string Name { get; set; }
        public string DistinguishedName { get; set; }
        public List<TreeItem> Items { get; set; }

    }
}