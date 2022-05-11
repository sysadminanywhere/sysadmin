using System;
using System.Collections.Generic;
using System.Text;

namespace Sysadmin.WMI
{
    public class WMIAttribute : Attribute
    {

        public string Name { get; private set; }
        public bool IsReadOnly { get; private set; } = false;

        public WMIAttribute(string name, bool isReadOnly = false)
        {
            Name = name;
            IsReadOnly = isReadOnly;
        }

    }
}