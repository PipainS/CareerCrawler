namespace HHParser.Domain.Attributes
{
    /// <summary>
    /// Indicates that the decorated property represents a query parameter in API requests.
    /// The associated key specifies the name of the parameter in the query string.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class QueryParameterAttribute(string key) : Attribute
    {
        /// <summary>
        /// Gets the key name of the query parameter.
        /// </summary>
        public string Key { get; } = key;
    }
}
