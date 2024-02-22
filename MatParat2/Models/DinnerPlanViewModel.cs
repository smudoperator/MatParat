using MatParat2.Models.Domain;

namespace MatParat2.Models
{
    public class DinnerPlanViewModel
    {
        public Dictionary<DayOfWeek, List<Dinner>> Dinners { get; set; }
    }
}
