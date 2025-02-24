namespace HHParser.Domain.Attributes
{
    /// <summary>
    /// Indicates the implementation status of a feature.
    /// This attribute is used to mark whether the associated menu option is currently implemented.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class FeatureStatusAttribute(bool isImplemented) : Attribute
    {
        /// <summary>
        /// Gets a value indicating whether the feature is implemented.
        /// </summary>
        public bool IsImplemented { get; } = isImplemented;
    }
}
