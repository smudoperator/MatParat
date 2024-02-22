namespace MatParat2.Models.Domain
{
    public class Dinner
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public DinnerType Type { get; set; }

        public string Ingredients { get; set; }

        public string Description { get; set; }

        public byte[] ImageData { get; set; }

    }

    public enum DinnerType
    {
        Fish,
        Meat,
        Chicken,
        Vegan,
        Other
    }
}
