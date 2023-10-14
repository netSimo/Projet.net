using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using StageProjet2.Data;

using StageProjet2.Models;
using StageProjet2.ViewModel;
using System.Linq;
using System.Threading.Tasks;
using System.Linq;
using System.Threading.Tasks;
using StageProjet2.Models;



namespace StageProjet2.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _applicationDbContext;

        public UserController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext applicationDbContext, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _applicationDbContext = applicationDbContext;
            _signInManager = signInManager;
        }

       

        public async Task<IActionResult> Index(IList<string> task)
        {

            var users = await _userManager.Users.Select(user => new UserViewModel
            {
                Id = user.Id,
                Nom = user.Nom,
                Prenom = user.Prenom,
                PhoneNumber = user.PhoneNumber,
                Email = user.Email,
                Role = _userManager.GetRolesAsync(user).Result
            }).ToListAsync();
            return View(users);
        }

        public async Task<IActionResult> Edit(string userId )
        {
            var users = await _userManager.FindByIdAsync(userId);

            if (users == null)
                return NotFound();

            var viewModel = new UserViewModel { 
                Id = userId,
                Nom = users.Nom,
                Prenom= users.Prenom,
                PhoneNumber = users.PhoneNumber,
                Email = users.Email,
            };
            
            return View(viewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UserViewModel model)
        {
           
            var user = await _userManager.FindByIdAsync(model.Id);

            if (user == null)
                return NotFound();

           user.Nom = model.Nom;
            user.Prenom = model.Prenom;
            user.PhoneNumber = model.PhoneNumber;
            user.Email = model.Email;
            await _userManager.UpdateAsync(user);

            return RedirectToAction(nameof(Index));
        }

        public async Task <IActionResult> Delete(string userId)
        {


            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return View(Index);
            }

            _applicationDbContext.Remove(user);

            _applicationDbContext.SaveChanges();

            return RedirectToAction("Index");
        }














    }
}
