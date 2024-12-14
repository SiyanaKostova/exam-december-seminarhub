using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SeminarHub.Data;
using SeminarHub.Data.Models;
using SeminarHub.Models.Category;
using SeminarHub.Models.Seminar;

namespace SeminarHub.Controllers
{
    [Authorize]
    public class SeminarController : Controller
    {
        private readonly SeminarHubDbContext _data;

        public SeminarController(SeminarHubDbContext data)
        {
            _data = data;
        }

        public async Task<IActionResult> All()
        {
            var seminarsToDisplay = await _data
                .Seminars
                .Select(s => new SeminarViewShortModel()
                {
                    Id = s.Id,
                    Topic = s.Topic,
                    Lecturer = s.Lecturer,
                    DateAndTime = s.DateAndTime.ToString("yyyy-MM-dd H:mm"),
                    Category = s.Category.Name,
                    Organiser = s.Organiser.UserName
                })
                .ToListAsync();

            return View(seminarsToDisplay);
        }

        public async Task<IActionResult> Add()
        {
            SeminarFormModel seminarModel = new SeminarFormModel()
            {
                Categories = GetCategories()
            };

            return View(seminarModel);
        }

        [HttpPost]
        public async Task<IActionResult> Add(SeminarFormModel seminarModel)
        {
            if (!GetCategories().Any(s => s.Id == seminarModel.CategoryId))
            {
                ModelState.AddModelError(nameof(seminarModel.CategoryId), "Category does not exist!");
            }

            if (!ModelState.IsValid)
            {
                return View(seminarModel);
            }

            string currentUserId = GetUserId();

            var seminarToAdd = new Seminar()
            {
                Topic = seminarModel.Topic,
                Lecturer = seminarModel.Lecturer,
                Details = seminarModel.Details,
                DateAndTime = seminarModel.DateAndTime,
                Duration = seminarModel.Duration,
                CategoryId = seminarModel.CategoryId,
                OrganiserId = currentUserId
            };

            await _data.Seminars.AddAsync(seminarToAdd);
            await _data.SaveChangesAsync();

            return RedirectToAction("All", "Seminar");
        }

        public async Task<IActionResult> Join(int id)
        {
            var seminarToAdd = await _data
                .Seminars
                .FindAsync(id);

            if (seminarToAdd ==  null)
            {
                return BadRequest();
            }

            string currentUserId = GetUserId();

            var entry = new SeminarParticipant()
            {
                SeminarId = seminarToAdd.Id,
                ParticipantId = currentUserId,
            };

            if (await _data.SeminarsParticipants.ContainsAsync(entry))
            {
                return RedirectToAction("Joined", "Seminar");
            }

            await _data.SeminarsParticipants.AddAsync(entry);
            await _data.SaveChangesAsync();

            return RedirectToAction("Joined", "Seminar");
        }

        public async Task<IActionResult> Leave(int id)
        {
            var seminarId = id;
            var currentUser = GetUserId();

            var seminarToLeave = _data.Seminars.FindAsync(seminarId);

            if (seminarToLeave == null)
            {
                return BadRequest();
            }

            var entry = await _data.SeminarsParticipants.FirstOrDefaultAsync(sp => sp.ParticipantId == currentUser && sp.SeminarId == seminarId);
            _data.SeminarsParticipants.Remove(entry);
            await _data.SaveChangesAsync();

            return RedirectToAction("All", "Seminar");
        }

        public async Task<IActionResult> Joined()
        {
            string currentUserId = GetUserId();

            var userSeminars = await _data
                .SeminarsParticipants
                .Where(sp => sp.ParticipantId == currentUserId)
                .Select(sp => new SeminarViewShortModel()
                {
                    Id = sp.Seminar.Id,
                    Topic = sp.Seminar.Topic,
                    DateAndTime = sp.Seminar.DateAndTime.ToString("dd/MM/yyyy H:mm"),
                    Category = sp.Seminar.Category.Name
                })
                .ToListAsync();

            return View(userSeminars);
        }

        public async Task<IActionResult> Details(int id)
        {
            var seminarToDisplay = await _data
               .Seminars
               .Where(s => s.Id == id)
               .Select(s => new SeminarViewDetailedModel()
               {
                   Id = s.Id,
                   Topic = s.Topic,
                   DateAndTime = s.DateAndTime.ToString("yyyy-MM-dd H:mm"),
                   Lecturer = s.Lecturer,
                   Organiser = s.Organiser.UserName,
                   Category = s.Category.Name,
                   Details = s.Details,
                   Duration = s.Duration,
               })
               .FirstOrDefaultAsync();

            if (seminarToDisplay == null)
            {
                return BadRequest();
            }

            return View(seminarToDisplay);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var seminarToEdit = await _data.Seminars.FindAsync(id);

            if (seminarToEdit == null)
            {
                return BadRequest();
            }

            string currentUserId = GetUserId();
            if (currentUserId != seminarToEdit.OrganiserId)
            {
                return Unauthorized();
            }

            SeminarFormModel seminarModel = new SeminarFormModel()
            {
                Topic = seminarToEdit.Topic,
                Lecturer = seminarToEdit.Lecturer,
                Details = seminarToEdit.Details,
                DateAndTime = seminarToEdit.DateAndTime,
                Duration = seminarToEdit.Duration,
                CategoryId = seminarToEdit.CategoryId,
                Categories = GetCategories()
            };

            return View(seminarModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, SeminarFormModel model)
        {
            var seminarToEdit = await _data.Seminars.FindAsync(id);

            if (seminarToEdit == null)
            {
                return BadRequest();
            }

            string currentUser = GetUserId();
            if (currentUser != seminarToEdit.OrganiserId)
            {
                return Unauthorized();
            }

            if (!GetCategories().Any(s => s.Id == model.CategoryId))
            {
                ModelState.AddModelError(nameof(model.CategoryId), "Category does not exist!");
            }

            seminarToEdit.Topic = model.Topic;
            seminarToEdit.Lecturer = model.Lecturer;
            seminarToEdit.Details = model.Details;
            seminarToEdit.DateAndTime = model.DateAndTime;
            seminarToEdit.Duration = model.Duration;
            seminarToEdit.CategoryId = model.CategoryId;
            seminarToEdit.OrganiserId = currentUser;

            await _data.SaveChangesAsync();
            return RedirectToAction("All", "Seminar");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var currentUserId = GetUserId();

            var seminarToDelete = await _data
                .Seminars
                .Where(s => s.Id == id)
                .FirstOrDefaultAsync();

            if (seminarToDelete == null)
            {
                return BadRequest();
            }

            if (seminarToDelete.OrganiserId != currentUserId)
            {
                return Unauthorized();
            }

            var seminar = new SeminarViewShortModel()
            {
                Id = id,
                Topic = seminarToDelete.Topic,
                DateAndTime = seminarToDelete.DateAndTime.ToString()
            };

            return View(seminar);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var seminarToDelete = await _data
                .Seminars
                .Where(s => s.Id == id)
                .FirstOrDefaultAsync();

            var seminarParticipants = await _data
                .SeminarsParticipants
                .Where(sp => sp.SeminarId == id)
                .ToListAsync();

            var currentUser = GetUserId();

            if (seminarToDelete == null)
            {
                return BadRequest();
            }

            if (seminarToDelete.OrganiserId != currentUser)
            {
                return Unauthorized();
            }

            if (seminarParticipants != null && seminarParticipants.Any())
            {
                _data.SeminarsParticipants.RemoveRange(seminarParticipants);
            }

            _data.Seminars.Remove(seminarToDelete);
            await _data.SaveChangesAsync();

            return RedirectToAction("All", "Seminar");
        }

        private IEnumerable<CategoryViewModel> GetCategories()
            => _data
                .Categories
                .Select(c => new CategoryViewModel()
                {
                    Id = c.Id,
                    Name = c.Name
                });

        private string GetUserId()
           => User.FindFirstValue(ClaimTypes.NameIdentifier);

    }
}
