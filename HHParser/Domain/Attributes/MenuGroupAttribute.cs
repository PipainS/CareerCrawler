namespace HHParser.Domain.Attributes
{
    [AttributeUsage(AttributeTargets.Field)]
    public class MenuGroupAttribute : Attribute
    {
        public string GroupName { get; }
        public MenuGroupAttribute(string groupName)
        {
            GroupName = groupName;
        }
    }
}
