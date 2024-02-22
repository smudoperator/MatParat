using MatParat2.Data;
using MatParat2.Models;
using MatParat2.Models.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MatParat2.Controllers
{
    public class DinnersController : Controller
    {
        private readonly MatParatDbContext _dbContext;
        public DinnersController(MatParatDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var allDinners = await _dbContext.Dinners.ToListAsync();
            return View(allDinners);
        }


        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddDinnerViewModel addDinnerRequest, IFormFile imageFile)
        {
            if(imageFile != null && imageFile.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await imageFile.CopyToAsync(memoryStream);
                    addDinnerRequest.ImageData = memoryStream.ToArray();
                }
            }
            
            var dinner = new Dinner()
            {
                Id = Guid.NewGuid(),
                Name = addDinnerRequest.Name,
                Type = addDinnerRequest.Type,
                Ingredients = addDinnerRequest.Ingredients,
                Description = addDinnerRequest.Description,
                ImageData = addDinnerRequest.ImageData,
            };

            await _dbContext.Dinners.AddAsync(dinner);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction("Add");
        }


        [HttpGet]
        public async Task<IActionResult> View(Guid id)
        {
            var dinner = await _dbContext.Dinners.FirstOrDefaultAsync(x => x.Id == id);

            if (dinner != null)
            {
                var viewModel = new UpdateDinnerViewModel()
                {
                    Id = dinner.Id,
                    Name = dinner.Name,
                    Type = dinner.Type,
                    Ingredients = dinner.Ingredients,
                    Description = dinner.Description
                };

                return await Task.Run(() => View("View", viewModel)); 
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> View(UpdateDinnerViewModel model)
        {
            var dinner = await _dbContext.Dinners.FindAsync(model.Id);

            if(dinner != null)
            {
                dinner.Name = model.Name;
                dinner.Type = model.Type;
                dinner.Ingredients = model.Ingredients;
                dinner.Description = model.Description;
            }

            await _dbContext.SaveChangesAsync();
            
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(UpdateDinnerViewModel model)
        {
            var dinner = await _dbContext.Dinners.FindAsync(model.Id);

            if (dinner != null)
            {
                _dbContext.Dinners.Remove(dinner);
                await _dbContext.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }



    }
}
