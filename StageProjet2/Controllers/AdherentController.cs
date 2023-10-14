using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using StageProjet2.Data;
using StageProjet2.Models;
using StageProjet2.ViewModel;

namespace StageProjet2.Controllers
{
    public class AdherentController : Controller
    {

        private readonly ApplicationDbContext _applicationDbContext;

        public AdherentController(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;

        }

        public IActionResult Index(string SearchString)
        {

            ViewData["CurentFilter"] = SearchString;

            //var adherents = _applicationDbContext.adherents.Include(m => m.Compteur).ToList();

            var adherents = from b in _applicationDbContext.adherents.Include(a => a.Compteur) select b;


            if (!string.IsNullOrEmpty(SearchString))
            {

                adherents = adherents.Where(b => b.Nom.Contains(SearchString));
            }


            return View(adherents);
        }


        public IActionResult Create()
        {

            var viewModel = new AdherentCompteurViewModel { };

            return View(viewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(AdherentCompteurViewModel model)
        {
            var Adherent = new Adherent
            {
                Nom = model.Nom,
                Prenom = model.Prenom,
                Adresse = model.Adresse,
                Num = model.Num,
                Compteur = new Compteur { Libelle = model.Compteur, Marque = model.CompteurMarque }
            };

            _applicationDbContext.adherents.Add(Adherent);
            _applicationDbContext.SaveChanges();

            return RedirectToAction("Index");
        }



        public IActionResult Edit(int id)
        {

            var adherent = _applicationDbContext.adherents.Find(id);

            if (id == null)
            {
                return View(null);
            }
            var viewModel = new AdherentCompteurViewModel
            {
                Nom = adherent.Nom,
                Prenom = adherent.Prenom,
                Adresse = adherent.Adresse,
                Num = adherent.Num,
            };

            return View(viewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(AdherentCompteurViewModel model)
        {
            var adherent = _applicationDbContext.adherents.Find(model.Id);

            if (adherent == null)
            {
                return View(null);
            }

            adherent.Nom = model.Nom;
            adherent.Prenom = model.Prenom;
            adherent.Adresse = model.Adresse;
            adherent.Num = model.Num;


            _applicationDbContext.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            var adherent = _applicationDbContext.adherents.Find(id);
            var compteur = _applicationDbContext.compteurs.Find(adherent.CompteurId);


            if (id == null)
            {
                return View(Index);
            }

            try
            {
                _applicationDbContext.Remove(adherent);
                _applicationDbContext.Remove(compteur);
                _applicationDbContext.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception x)
            {
                TempData["AlerMessage1"] = "Tesssssssssssssssssst";
                return RedirectToAction("Index");
            }
        }

        public IActionResult DeleteALLFactures(int id)
        {
            var adherent = _applicationDbContext.adherents.Find(id);

            _applicationDbContext.factures.FromSql($"DELETE FROM  [factures] where AdherentId = {adherent.Id}");
            _applicationDbContext.SaveChanges();
            return RedirectToAction("Index");


        }
    }
}
