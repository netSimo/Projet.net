using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StageProjet2.Data;
using StageProjet2.Models;
using StageProjet2.ViewModel;

namespace StageProjet2.Controllers
{
    public class PrixController : Controller
    {

        private readonly ApplicationDbContext _applicationDbContext;


        public PrixController(ApplicationDbContext applicationDbContext )
        {
            _applicationDbContext = applicationDbContext;
        }


        // GET: HomeController1
        public ActionResult Index()
        {
            var prixs = from p in _applicationDbContext.prixs.OrderByDescending(d => d.DateTimePrix) select p;
            return View(prixs);
        }

       

        
       

        // GET: HomeController1/Edit/5
        public ActionResult Edit()
        {

            
            var prixModel = new PrixView
            { };
            return View(prixModel);
        }

        // POST: HomeController1/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PrixView model)
        {

            //var prix = _applicationDbContext.prixs.Find(model.Id);

             var prixModif =_applicationDbContext.prixs.FromSql($"Select * From [SProjetDB2].[dbo].[prixs] where etat = 'Actuelle'").FirstOrDefault(); 


            prixModif.etat = "Ancienne";
            _applicationDbContext.SaveChanges();


            if (model == null)
            {
                return View(null);
            }

            var prix = new Prix
            {
                Prixtranche1 = model.Prixtranche1,
                Prixtranche2 = model.Prixtranche2,
                Prixtranche3 = model.Prixtranche3,
                DateTimePrix = DateTime.Now,
                etat = "Actuelle"
            };

            _applicationDbContext.prixs.Add(prix);
            _applicationDbContext.SaveChanges();

            return RedirectToAction("Index");
            
        }

        // GET: HomeController1/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: HomeController1/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
