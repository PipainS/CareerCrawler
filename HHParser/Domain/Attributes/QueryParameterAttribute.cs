namespace HHParser.Domain.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class QueryParameterAttribute : Attribute
    {
        public string Key { get; }
        public QueryParameterAttribute(string key)
        {
            Key = key;
        }
    }
}
