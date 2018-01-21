namespace Web.Models.Input
{
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;

    /// <summary>
    /// Request for configuration values modification
    /// </summary>
    [DataContract]
    public class ValuesModificationRequest
    {
        /// <summary>
        /// Collection of new configuration values
        /// </summary>
        [Required]
        [DataMember(EmitDefaultValue = false, IsRequired = true, Order = 1)]
        public ConfigurationValue[] Values { get; set; }
    }

    /// <summary>
    /// Configuration value
    /// </summary>
    [DataContract]
    public class ConfigurationValue
    {
        /// <summary>
        /// Configuration value Id
        /// </summary>
        [DataMember(EmitDefaultValue = false, IsRequired = true, Order = 1)]
        public int Id { get; set; }

        /// <summary>
        /// Configuration value
        /// </summary>
        [DataMember(EmitDefaultValue = false, IsRequired = true, Order = 2)]
        public string Value { get; set; }
    }
}