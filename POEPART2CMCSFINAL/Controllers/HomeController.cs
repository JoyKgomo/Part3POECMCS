using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using POEPART2CMCSFINAL.Models;
using POEPART2CMCSFINAL.Services;
using System.Diagnostics;
using System.Drawing;
using System.Xml.Linq;





namespace POEPART2CMCSFINAL.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IWebHostEnvironment _environment;
        ClaimContext claimContext;
        ClaimService claimService;
        DocumentService documentService;

        public HomeController(ILogger<HomeController> logger, IWebHostEnvironment _environmentClaimContext, ClaimContext claimContext)
        {
            _logger = logger;
            _environment = _environmentClaimContext;
            this.claimContext = claimContext;
            claimService = new ClaimService(claimContext);
            documentService = new DocumentService(claimContext);
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var person = claimContext.Users.FirstOrDefault(x => x.username == model.username && x.password == model.password);
                if (person != null)
                {
                    HttpContext.Session.SetInt32("UserId", person.ID);
                    HttpContext.Session.SetString("Role", person.role);
                    switch (person.role)
                    {
                        case "Lecturer":
                            return RedirectToAction("Dashboard");
                        case "HR":
                            return RedirectToAction("HRDashboard");
                        default:
                            return RedirectToAction("ManagerDashboard");
                    }

                }
                else
                {
                    ViewBag.Message = "Invalid login details";
                    return View();
                }

            }
            return View();
        }

        [HttpGet]
        public IActionResult HRDashboard()
        {
            var claims = claimContext.Claims.ToList();

            // Pass the claims to the view
            return View(claims);
        }


        [HttpGet]
        public IActionResult Dashboard()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login");
            }
            var user = claimContext.Users.FirstOrDefault(x => x.ID == userId);

            ViewBag.UserProfile = user?.Name;

            //get the user's claims from the database
            var claims = claimService.GetAllClaimsForUser(user?.ID ?? 0);
            return View(claims);
          
        }
       
        public IActionResult SubmitClaim()
        {
            var model = new ClaimViewModel();
            return View(model);
        }

        [HttpPost]
        public IActionResult SubmitClaim(ClaimViewModel model)
        {
            // Validate that HoursWorked and HourlyRate are greater than zero
            if (model.HoursWorked <= 0)
            {
                ModelState.AddModelError(nameof(model.HoursWorked), "Hours worked must be greater than zero.");
            }

            if (model.HourlyRate <= 0)
            {
                ModelState.AddModelError(nameof(model.HourlyRate), "Hourly rate must be greater than zero.");
            }

            if (ModelState.IsValid)
            {
                var userId = HttpContext.Session.GetInt32("UserId");
                var claim = new Claim();
                claim.UserID = userId ?? 0;
                claim.HoursWorked = model.HoursWorked;
                claim.HourlyRate = model.HourlyRate;
                claim.DateClaimed = DateTime.Now;
                claim.status = "Pending";//Pending status
                int claimId = claimService.AddNewClaim(claim);



                if (model.Document != null && model.Document.Length > 0)
                {
                    // Define the upload folder
                    string uploadPath = Path.Combine(_environment.WebRootPath, "uploads");
                    // Create the folder if it doesn't exist
                    if (!Directory.Exists(uploadPath))
                    {
                        Directory.CreateDirectory(uploadPath);
                    }
                    // Generate the file path
                    string filePath = Path.Combine(uploadPath, model.Document.FileName);
                    // Save the file to the specified location
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        model.Document.CopyTo(stream);
                    }


                    var document = new Document();
                    document.FileName = model.Document.FileName;
                    document.DateUploaded = DateTime.Now;
                    document.Id = claimId;
                    documentService.AddClaimDocument(document);
                }
                return RedirectToAction("Dashboard");

            }
            ViewBag.Message = "Error saving claim details";

            return View(model);
        }

        public IActionResult Download(int claimId)
        {
            var documents = documentService.GetClaimDocuments(claimId);
            if (!documents.Any())
            {
                return Content("Filename is not provided.");
            }
            var document = documents.FirstOrDefault();
            string filePath = Path.Combine(_environment.WebRootPath, "uploads", document.FileName);
            if (!System.IO.File.Exists(filePath))
            {
                return Content("File not found.");
            }
            byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
            return File(fileBytes, "application/octet-stream", document.FileName);
        }
    
    public async Task<IActionResult> ManagerDashboard()
        {
            // Fetch users with claims
            var usersWithClaims = await claimContext.Users
                .Include(u => u.Claims) // Include related claims
                .ToListAsync();

            // Pass the list of users to the view
            return View(usersWithClaims);
        }

        // View claims for a specific user by ID
        [HttpGet]
        [Route("Home/ViewClaims/{userId}")]
        public IActionResult ViewClaims(int userId)
        {
            // Retrieve the user along with their claims
            var user = claimContext.Users
                .Include(u => u.Claims) // Include claims for the user
                .FirstOrDefault(x => x.ID == userId);

            // Check if user was found
            if (user == null)
                return View("NotFound");

            // Get the most recent claim for the user
            var mostRecentClaim = user.Claims.OrderByDescending(c => c.DateClaimed).FirstOrDefault();

            // Check if a claim was found
            if (mostRecentClaim == null)
                return View("NotFound"); // or handle no claims appropriately

            // Pass the most recent claim to the view
            return View(mostRecentClaim);
        }



        // Action to approve a claim
        [HttpPost]
        public async Task<IActionResult> ApproveClaim(int claimId)
        {
            var claim = await claimContext.Claims.FindAsync(claimId);

            if (claim != null)
            {
                claim.status = "Approved";
                await claimContext.SaveChangesAsync();
            }

            return RedirectToAction("ManagerDashboard");
        }

        // Action to decline a claim
        [HttpPost]
        public async Task<IActionResult> DeclineClaim(int claimId)
        {
            var claim = await claimContext.Claims.FindAsync(claimId);

            if (claim != null)
            {
                claim.status = "Declined";
                await claimContext.SaveChangesAsync();
            }

            return RedirectToAction("ManagerDashboard");
        }

        public IActionResult ViewAllInvoices()
        {
            // Get all invoices from the service
            var allInvoices = claimService.GetAllInvoices();
            return View(allInvoices);
        }

        public IActionResult GenerateInvoices()
        {
            int testId = 1; // Replace with a valid claim ID from your database
            var invoice = claimService.GenerateInvoice(testId);

            if (invoice == null)
            {
                return NotFound("Invoice not found.");
            }

            return View(invoice);
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
