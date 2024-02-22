using MatParat2.Models.Domain;

namespace MatParat2.Models
{
    public class UpdateDinnerViewModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public DinnerType Type { get; set; }

        public string Ingredients { get; set; }

        public string Description { get; set; }

        public byte[] ImageData { get; set; }
    }
}
