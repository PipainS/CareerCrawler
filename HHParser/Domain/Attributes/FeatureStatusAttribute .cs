namespace HHParser.Domain.Attributes
{
    [AttributeUsage(AttributeTargets.Field)]
    public class FeatureStatusAttribute : Attribute
    {
        public bool IsImplemented { get; }
        public FeatureStatusAttribute(bool isImplemented)
        {
            IsImplemented = isImplemented;
        }
    }
}
