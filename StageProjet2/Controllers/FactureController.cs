using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StageProjet2.Data;
using StageProjet2.Models;
using StageProjet2.ViewModel;
using iTextSharp.text;
using iTextSharp.text.pdf;
using OfficeOpenXml;




namespace StageProjet2.Controllers
{
    public class FactureController : Controller
    {

        private readonly ApplicationDbContext _applicationDbContext;
        public FactureController(ApplicationDbContext applicationDbContext)
        {

            _applicationDbContext = applicationDbContext;

        }

        public IActionResult Index(string SearchString )
        {

            ViewData["CurentFilter"] = SearchString;

            var factures = from F in _applicationDbContext.factures.Include(c => c.Compteur).Include(a => a.Adherent).OrderByDescending(a => a.FactureDate) select F;
            
            if (!string.IsNullOrEmpty(SearchString))
            {

                factures = factures.Where(b => b.Adherent.Nom.Contains(SearchString));
            }


            return View(factures);
        }


        public IActionResult FactureByAdherent(int id)
        {

            var facturesByAdherent = _applicationDbContext.factures.FromSql($"select * from factures where AdherentId = {id} ORDER BY FactureDate DESC ");


            return View(facturesByAdherent);
        }

        public IActionResult Create()
        {

            var viewModel = new FactureView { };

            return View(viewModel);
        }


        //------------------------------------------------------------------------------

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(FactureView model)
        {
            var compteur = _applicationDbContext.compteurs.FromSql($"select * from compteurs where Libelle = {model.Libelle}").FirstOrDefault();
            if (compteur == null)
            {
                TempData["AlerMessage"] = "pas de compteur dans les systeme avec ce libelle";
                return View(model);

            }
            var adherent = _applicationDbContext.adherents.FromSql($"select * from adherents where CompteurId = {compteur.Id}").FirstOrDefault();
            if (adherent == null)
            {
                TempData["AlerMessage"] = "pas de Adherent associer a ce compteur";
                return View(model);
            }

            var AncieneFacture = _applicationDbContext.factures.FromSql($"SELECT * from factures where FactureDate = ( select MAX(FactureDate) from factures where CompteurId ={compteur.Id}) And CompteurId ={compteur.Id}").FirstOrDefault();




            if (AncieneFacture == null)
            {
                var facture = new Facture
                {
                    Etat = "No Payee",
                    //AncienValeur = 0,
                    NouvelleValeur = model.NouvelleValeur,
                    FactureDate = model.FactureDate,
                    prix = CalculePrix(0, model.NouvelleValeur),
                    AdherentId = adherent.Id,
                    Adherent = adherent,
                    CompteurId = compteur.Id,
                    Compteur = compteur
                };
                _applicationDbContext.factures.Add(facture);
                _applicationDbContext.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                if (AncieneFacture.NouvelleValeur>model.NouvelleValeur)
                {
                    TempData["AlerMessage"] = "L'operation est impossible, la nouvelle valeur est inferieur a l'ancienne valeur ";
                    return View(model);

                }
                else
                {
                    if (AncieneFacture.FactureDate>model.FactureDate)
                    {
                        TempData["AlerMessage"] = "la facture de ce mois est deja existe";
                        return View(model);
                    }
                    else
                    {
                        var facture = new Facture
                        {
                            Etat = "No Payee",
                            AncienValeur = AncieneFacture.NouvelleValeur,
                            NouvelleValeur = model.NouvelleValeur,
                            FactureDate = model.FactureDate,
                            prix = CalculePrix(AncieneFacture.NouvelleValeur, model.NouvelleValeur),
                            AdherentId = adherent.Id,
                            Adherent = adherent,
                            CompteurId = compteur.Id,
                            Compteur = compteur
                        };
                        _applicationDbContext.factures.Add(facture);
                        _applicationDbContext.SaveChanges();
                        return RedirectToAction("Create");
                    }
                }          
            }
        }

        //------------------------------------------------------------------------------------------------------------------

        public async Task<IActionResult> ImportExcelFile(IFormFile file)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            string ExContourLibelle;
            float ExConsomation;
            DateTime ExFactureDate;
            int ExFactureId;
            
            using(var stream  = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                using (var package = new ExcelPackage(stream))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                    var rowcount = worksheet.Dimension.Rows;
                    for (int row = 2; row <= rowcount; row++)
                    {
                        ExFactureId = int.Parse(worksheet.Cells[row,1].Value.ToString());
                        ExContourLibelle = worksheet.Cells[row,2].Value.ToString().Trim();
                        ExConsomation = float.Parse(worksheet.Cells[row, 3].Value.ToString());
                        ExFactureDate = DateTime.Parse(worksheet.Cells[row,4].Value.ToString());

                        var compteur = _applicationDbContext.compteurs.FromSql($"select * from compteurs where Libelle = {ExContourLibelle}").FirstOrDefault();
                        if (compteur == null)
                        {
                            TempData["AlerMessage"] = "L'operation est impossible! pas de compteur dans les systeme avec ce libelle, La ligne "+ExFactureId;
                            
                            return RedirectToAction("Create");

                        }
                        var adherent = _applicationDbContext.adherents.FromSql($"select * from adherents where CompteurId = {compteur.Id}").FirstOrDefault();
                        if (adherent == null)
                        {
                            TempData["AlerMessage"] = "pas de Adherent associer a ce compteur";
                            return RedirectToAction("Create");
                        }

                        var AncieneFacture = _applicationDbContext.factures.FromSql($"SELECT * from factures where FactureDate = ( select MAX(FactureDate) from factures where CompteurId ={compteur.Id}) And CompteurId ={compteur.Id}").FirstOrDefault();




                        if (AncieneFacture == null)
                        {
                            var facture = new Facture
                            {
                                Etat = "No Payee",
                                //AncienValeur = 0,
                                NouvelleValeur = ExConsomation,
                                FactureDate = ExFactureDate,
                                prix = CalculePrix(0, ExConsomation),
                                AdherentId = adherent.Id,
                                Adherent = adherent,
                                CompteurId = compteur.Id,
                                Compteur = compteur
                            };
                            _applicationDbContext.factures.Add(facture);
                            _applicationDbContext.SaveChanges();
                            return RedirectToAction("Create");
                        }
                        else
                        {
                            if (AncieneFacture.NouvelleValeur > ExConsomation)
                            {
                                TempData["AlerMessage"] = "L'operation est impossible, la nouvelle valeur est inferieur a l'ancienne valeur, La ligne "+ExFactureId;
                                return RedirectToAction("Create");

                            }
                            else
                            {
                                if (AncieneFacture.FactureDate > ExFactureDate)
                                {
                                    TempData["AlerMessage"] = "la facture de ce mois est deja existe, La ligne " + ExFactureId;
                                    return RedirectToAction("Create");
                                }
                                else
                                {
                                    var facture = new Facture
                                    {
                                        Etat = "No Payee",
                                        AncienValeur = AncieneFacture.NouvelleValeur,
                                        NouvelleValeur = ExConsomation,
                                        FactureDate = ExFactureDate,
                                        prix = CalculePrix(AncieneFacture.NouvelleValeur, ExConsomation),
                                        AdherentId = adherent.Id,
                                        Adherent = adherent,
                                        CompteurId = compteur.Id,
                                        Compteur = compteur
                                    };
                                    _applicationDbContext.factures.Add(facture);
                                    _applicationDbContext.SaveChanges();
                                    
                                }}}


                    }
                }}
            return RedirectToAction("Create");

        }


        public IActionResult Edit(int id)
        {

            if (id == null)
            {
                return View(null);
            }

            var facture = _applicationDbContext.factures.Find(id);

            if (facture == null)
            {
                return View(null);
            }

            facture.Etat = "Payee";

            var AdId = facture.AdherentId;

            _applicationDbContext.SaveChanges();


            return RedirectToAction("FactureByAdherent", new { id= AdId });
        }
        public IActionResult Edit1(int id)
        {

            if (id == null)
            {
                return View(null);
            }

            var facture = _applicationDbContext.factures.Find(id);

            if (facture == null)
            {
                return View(null);
            }

            facture.Etat = "Payee";

            var AdId = facture.AdherentId;

            _applicationDbContext.SaveChanges();


            return RedirectToAction("Index");
        }


      




            public double CalculePrix(double EncienV, double NouvelleV)
        {

            double PrixT = 0;

            double consomation = NouvelleV - EncienV;

            var Prix = _applicationDbContext.prixs.FromSql($"Select * From [SProjetDB2].[dbo].[prixs] where etat = 'Actuelle'").FirstOrDefault();

            if (consomation < 0)
            {
                Console.WriteLine("Error de consomation");
            }

            if (consomation >= 1 && consomation <= 20)
            {

                PrixT = consomation * Prix.Prixtranche1;

            }

            if (consomation >= 21 && consomation <= 30)
            {

                PrixT = consomation * Prix.Prixtranche2;

            }

            if (consomation >= 31 && consomation <= 40)
            {

                PrixT = consomation * Prix.Prixtranche3;

            }


            return PrixT;

        }

        //-----------------------------------------------------------------------------------------------------

      





        //-----------------------------------------------------------------------------------------------------


        public IActionResult GeneratePDf(int id)
        {

            if (id == null)
            {
                return View(null);
            }

            var facture = _applicationDbContext.factures.Find(id);

            var adherent = _applicationDbContext.adherents.Find(facture.AdherentId);
            var compteur = _applicationDbContext.compteurs.Find(facture.CompteurId);

            ///////////////////////////////////////////////////////////////////////////////////////////////////////////


            using (MemoryStream ms = new MemoryStream()) 
            {
                Document document = new Document(PageSize.A4, 25, 25, 30, 30);
                PdfWriter writer = PdfWriter.GetInstance(document, ms);
                document.Open();              
                Paragraph para1 = new Paragraph("Association  espoir Kzoula al Rokba", new Font(Font.FontFamily.HELVETICA, 20));
                para1.Alignment = Element.ALIGN_CENTER;               
                document.Add(para1);
                Paragraph para2 = new Paragraph("L'eau potable", new Font(Font.FontFamily.HELVETICA, 15));
                para2.Alignment = Element.ALIGN_CENTER;           
                document.Add(para2);
                Paragraph para3 = new Paragraph("Communauté Ouidane - Préfecture de Marrakech", new Font(Font.FontFamily.HELVETICA, 13));
                para3.Alignment = Element.ALIGN_CENTER;          
                document.Add(para3);
                PdfContentByte contentByte = writer.DirectContent;
                // Définition des coordonnées de départ et d'arrivée de la ligne
                float xStart = document.Left;
                float xEnd = document.Right;
                float y = 710;
                // Dessin de la ligne
                contentByte.SetLineWidth(1f);
                contentByte.MoveTo(xStart, y);
                contentByte.LineTo(xEnd, y);
                contentByte.Stroke();
                Paragraph para4 = new Paragraph(" Annonce                                               L'eau potable", new Font(Font.FontFamily.HELVETICA,20));
                para4.Alignment=Element.ALIGN_CENTER;
                para4.SpacingBefore = 30;
                para4.SpacingAfter = 45;
                document.Add(para4);
                PdfContentByte contentByte2 = writer.DirectContent;
                // Définition des coordonnées de départ et d'arrivée de la ligne
                float xStart2 = document.Left;
                float xEnd2 = document.Right;
                float y2 = 660;
                // Dessin de la ligne
                contentByte2.SetLineWidth(1f);
                contentByte2.MoveTo(xStart2, y2);
                contentByte2.LineTo(xEnd2, y2);
                contentByte2.Stroke();
                PdfContentByte contentByte3 = writer.DirectContent;
                // Définition des coordonnées de départ et d'arrivée de la ligne
                float xStart3 = document.Left;
                float xEnd3 = document.Right;
                float y3 = 480;
                // Dessin de la ligne
                contentByte3.SetLineWidth(1f);
                contentByte3.MoveTo(xStart3, y3);
                contentByte3.LineTo(xEnd3, y3);
                contentByte3.Stroke();
                Paragraph paraNOM = new Paragraph("Nom                                                                            "+adherent.Nom, new Font(Font.FontFamily.HELVETICA, 13));
                paraNOM.Alignment = Element.ALIGN_LEFT;
                paraNOM.SpacingAfter = 15;
                document.Add(paraNOM);
                Paragraph paraPrenom = new Paragraph("Prenom                                                                       "+ adherent.Prenom, new Font(Font.FontFamily.HELVETICA, 13));
                paraPrenom.Alignment = Element.ALIGN_LEFT;
                paraPrenom.SpacingAfter = 15;
                document.Add(paraPrenom);                
                Paragraph paraC = new Paragraph("Compteur                                                                    "+compteur.Libelle, new Font(Font.FontFamily.HELVETICA, 13));
                paraC.Alignment = Element.ALIGN_LEFT;
                paraC.SpacingAfter = 15;
                document.Add(paraC);
                Paragraph paraDate = new Paragraph("Date du facture                                                            "+facture.FactureDate, new Font(Font.FontFamily.HELVETICA, 13));
                paraDate.Alignment = Element.ALIGN_LEFT;
                paraDate.SpacingAfter = 70;
                document.Add(paraDate);
                Paragraph paraE = new Paragraph("Anciene consomation                                                            " + facture.AncienValeur, new Font(Font.FontFamily.HELVETICA, 13));
                paraE.Alignment = Element.ALIGN_LEFT;
                paraE.SpacingAfter = 15;
                document.Add(paraE);
                Paragraph paraN = new Paragraph("Anciene consomation                                                            " + facture.NouvelleValeur, new Font(Font.FontFamily.HELVETICA, 13));
                paraN.Alignment = Element.ALIGN_LEFT;
                paraN.SpacingAfter = 15;
                document.Add(paraN);
                var d = facture.NouvelleValeur - facture.AncienValeur;
                Paragraph paraD = new Paragraph("La difference                                                                         "+d+"  T " , new Font(Font.FontFamily.HELVETICA, 13));
                paraD.Alignment = Element.ALIGN_LEFT;
                paraD.SpacingAfter = 15;
                document.Add(paraD);
                Paragraph paraI = new Paragraph("Prix d'hygiene                                                                        10 Dirhams" , new Font(Font.FontFamily.HELVETICA, 13));
                paraI.Alignment = Element.ALIGN_LEFT;
                paraI.SpacingAfter = 15;
                document.Add(paraI);
                var prixT = facture.prix + 10;
                Paragraph paraP = new Paragraph("Prix Totale                                                                              "+prixT+"  Dirhams", new Font(Font.FontFamily.HELVETICA, 13));
                paraP.Alignment = Element.ALIGN_LEFT;
                paraP.SpacingAfter = 15;
                document.Add(paraP);
                Paragraph paraT = new Paragraph("Attention : Le montant visé doit être payé avant la deuxième semaine du mois suivant  ", new Font(Font.FontFamily.HELVETICA, 13));
                paraT.Alignment = Element.ALIGN_CENTER;
                paraT.SpacingAfter = 100;
                paraT.SpacingBefore = 50;
                document.Add(paraT);

                Barcode39 barcode = new Barcode39();
                barcode.Code = "123456";  
                // Create a new PdfContentByte object which will write to the PDF document
                PdfContentByte cb = writer.DirectContent;
                // Set the barcode dimensions and position on the page
                barcode.X = 2; // Barcode width (default is 0.8)
                barcode.N = 3; // Bar height (default is 9)
                barcode.Size = 12; // Barcode text size (default is 8)
                barcode.BarHeight = 30; // Height of the bars (default is 0.6)
                // Create an Image object from the Barcode39 object
                Image image = barcode.CreateImageWithBarcode(cb, null, null);
                // Add the image to the document
                document.Add(image);


                BarcodeQRCode qrCode = new BarcodeQRCode("Nom : "+adherent.Nom +" ,Prenom : " +adherent.Prenom+" ,La date : "+facture.FactureDate+" , le Prix : "+prixT, 100, 100, null);

                // Get an image representation of the QR code
                Image qrCodeImage = qrCode.GetImage();
                qrCodeImage.SetAbsolutePosition(370, 30); // Set the position of the QR code image

                // Add the QR code image to the document
                document.Add(qrCodeImage);






                document.Close();
                writer.Close();
                var constant = ms.ToArray();
                return File(constant, "applicatoon/vnd", "Facture.pdf");
                                        }                        
        }

    }
}
