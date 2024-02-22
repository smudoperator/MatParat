using MatParat2.Models.Domain;
using MatParat2.Models;
using Microsoft.AspNetCore.Mvc;
using MatParat2.Data;
using Microsoft.EntityFrameworkCore;

namespace MatParat2.Controllers
{
    public class DinnerPlanController : Controller
    {
        private readonly MatParatDbContext _dbContext;
        private static Random _rng = new Random();

        public DinnerPlanController(MatParatDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult View(DinnerPlanViewModel model)
        {
            return View(model);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateDinnerPlanViewModel model)
        {
            var plannedDinners = await PlanDinners(model);

            if(plannedDinners != null)
            {
                var dinnerPlanViewModel = new DinnerPlanViewModel
                {
                    Dinners = plannedDinners
                };
                return View("View", dinnerPlanViewModel);
            }

            return View("Create", model);
        }

        private async Task<Dictionary<DayOfWeek, List<Dinner>>> PlanDinners(CreateDinnerPlanViewModel model)
        {
            var allDinners = await _dbContext.Dinners.ToListAsync();

            if (allDinners == null || allDinners.Count() == 0)
            {
                return null;
            }

            var selectedDinners = new List<Dinner>();

            var shuffledDinners = allDinners.OrderBy(_  => _rng.Next()).ToList();

            var number = model.TacoFriday ? model.NumberOfDays - 1 : model.NumberOfDays;

            var fishyNumber = model.HowManyFish;

            selectedDinners.AddRange(shuffledDinners
                .Take(fishyNumber)
                .Where(x => x.Type == DinnerType.Fish));
            
            selectedDinners.AddRange(shuffledDinners.Take(number).ToList());

            if(model.TacoFriday) 
            {
                var taco = await GetDinnerByName("taco");
                if(taco != null)
                {
                    selectedDinners.Add(taco);
                }
            }


            var selectedDinnersForWeekDay = AssignDinnersToWeekDay(selectedDinners, model.StartDay, model.NumberOfDays);
            return selectedDinnersForWeekDay;
        }


        public async Task<Dinner> GetDinnerByName(string name)
        {
            return await _dbContext.Dinners.FirstOrDefaultAsync(x => x.Name.ToLower().Contains(name));
        }


        public Dictionary<DayOfWeek, List<Dinner>> AssignDinnersToWeekDay(List<Dinner> dinners, DayOfWeek startDay, int numberOfDays) 
        {
            var result = new Dictionary<DayOfWeek, List<Dinner>>();
            var ListOfWeekDays = CreateListOfWeekdays(startDay, numberOfDays);

            foreach (var weekDay in ListOfWeekDays)
            {
                var dayDinners = new List<Dinner>();

                var taco = dinners.FirstOrDefault(x => x.Name.ToLower().Contains("taco"));

                if (weekDay == DayOfWeek.Friday && taco != null)
                {
                    dayDinners.Add(taco);
                    dinners.Remove(taco);
                }
                else
                {
                    var dinner = dinners.FirstOrDefault(x => !x.Name.ToLower().Contains("taco"));
                    if (dinner != null)
                    {
                        dayDinners.Add(dinner);
                        dinners.Remove(dinner);
                    }
                }
                result.Add(weekDay, dayDinners);
            }

            return result;
        }


        public List<DayOfWeek> CreateListOfWeekdays(DayOfWeek startDay, int numberOfDays)
        {
            var count = 0;
            var listOfWeekdays = new List<DayOfWeek>();
            while (count < numberOfDays)
            {
                listOfWeekdays.Add(startDay + count);
                count++;
            }

            return listOfWeekdays;
        }

    }
}
