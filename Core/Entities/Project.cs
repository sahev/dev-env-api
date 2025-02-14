using Core.Models;

namespace Core.Entities
{
    public class Project : BaseModel
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public string StorageSize { get; set; }
        public virtual ICollection<Service>? Services { get; set; }
    }
}
