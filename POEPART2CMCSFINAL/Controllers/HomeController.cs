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
        Claim Claim;

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
            // Filter claims with an "Approved" status
            var approvedClaims = claimContext.Claims
                                              .Where(c => c.status == "Approved")
                                              .ToList();

            // Debug: Print the IDs of the approved claims
            foreach (var claim in approvedClaims)
            {
                Console.WriteLine(claim.Id);
            }

            // Pass the approved claims to the view
            return View(approvedClaims);
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
                    document.ClaimId = claimId;
                    claimContext.Documents.Add(document);
                    claimContext.SaveChanges();
                }
                return RedirectToAction("Dashboard");

            }
            ViewBag.Message = "Error saving claim details";

            return View(model);
        }

        public IActionResult Download(int claimId)
        {
            var documents = claimContext.Documents
                .Where(x => x.ClaimId == claimId) // Ensure you use the correct property (ClaimId)
                .ToList();//documentService.GetClaimDocuments(claimId);
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
            // Fetch users with their claims, including related claim data
            var usersWithClaims = await claimContext.Users
                .Include(u => u.Claims) 
                .Where(u => u.role == "Lecturer")// This includes all claims for each user
                .ToListAsync();

            // Pass the list of users along with their claims to the view
            return View(usersWithClaims);
        }

        [HttpPost]
        public IActionResult ManagerDashboard(int claimID, string actionType)
        {
            // Check for a valid claim ID and action
            if (claimID <= 0 || string.IsNullOrEmpty(actionType))
            {
                return BadRequest("Invalid claim ID or action.");
            }

            // Update the claim status based on the action
            if (actionType == "app")
            {
                var claim = claimContext.Claims.FirstOrDefault(u => u.Id == claimID);
                if (claim != null)
                {
                    claim.status = "Approved";
                    claimContext.SaveChanges();
                }
                else
                {
                    return NotFound($"Claim with ID {claimID} not found.");
                }
            }
            else if (actionType == "rej")
            {
                var claim = claimContext.Claims.FirstOrDefault(u => u.Id == claimID);
                if (claim != null)
                {
                    claim.status = "Rejected";
                    claimContext.SaveChanges();
                }
                else
                {
                    return NotFound($"Claim with ID {claimID} not found.");
                }
            }
            else
            {
                return BadRequest("Invalid action specified.");
            }

            // Redirect or return a view after updating the status
            return Redirect("ManagerDashboard");
        }


        // View claims for a specific user by ID
        [HttpGet]
        [Route("Home/ViewClaims/{claimId}")]
        public IActionResult ViewClaims(int claimId)
        {
            // Retrieve the user along with their claims
            var user = claimContext.Users
                .Include(u => u.Claims) // Include claims for the user
                .FirstOrDefault(x => x.ID == claimId);

            var claim = claimContext.Claims.FirstOrDefault(x => x.Id == claimId);

            // Check if user was found
            if (claim == null)
                return View("NotFound");

   

            // Pass the most recent claim to the view
            return View(claim);
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

        [HttpPost]
        public IActionResult GenerateInvoices(int claimId)
        {
            // Fetch the invoice using the claim ID
            var invoice = claimService.GenerateInvoice(claimId);

            // Check if the invoice is null, which means the claim does not exist or is not approved
            if (invoice == null)
            {
                return NotFound($"Invoice could not be generated for Claim ID: {claimId}. It may not exist or is not approved.");
            }

            // Pass the invoice to the view
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
