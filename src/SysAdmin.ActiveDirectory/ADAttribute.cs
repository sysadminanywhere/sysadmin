namespace SysAdmin.ActiveDirectory
{
    [AttributeUsage(AttributeTargets.All, Inherited = true)]
    public class ADAttribute : Attribute
    {

        public enum DateTypes
        {
            None,
            Date,
            TimeStamp
        }

        public string Name { get; private set; }
        public DateTypes DateType { get; private set; }
        public bool IsReadOnly { get; private set; } = false;

        public ADAttribute(string name, bool isReadOnly = false)
        {
            Name = name.ToLower();
            IsReadOnly = isReadOnly;
        }

        public ADAttribute(string name, DateTypes dateType, bool isReadOnly = false)
        {
            Name = name.ToLower();
            DateType = dateType;
            IsReadOnly = isReadOnly;
        }
    }
}