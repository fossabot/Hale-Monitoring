namespace Hale.Core.Models
{
    /// <summary>
    /// A YAML representation of a node config source paired with an ID
    /// </summary>
    public class ConfigSourceDTO
    {
        public int Id { get; set; }

        public string Body { get; set; }
    }
}
