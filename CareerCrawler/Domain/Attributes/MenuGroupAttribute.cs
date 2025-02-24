namespace HHParser.Domain.Attributes
{
    /// <summary>
    /// Specifies the group to which a menu item belongs.
    /// This attribute is used for displaying the group name of menu options in the console.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class MenuGroupAttribute(string groupName) : Attribute
    {
        /// <summary>
        /// Gets the name of the menu group.
        /// </summary>
        public string GroupName { get; } = groupName;
    }
}
