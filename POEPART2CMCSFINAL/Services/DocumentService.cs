using POEPART2CMCSFINAL.Services;
using POEPART2CMCSFINAL.Models;
using System.Security.Claims;

namespace POEPART2CMCSFINAL.Services

{
    public class DocumentService
    {
        private readonly ClaimContext claimContext;
        public DocumentService(ClaimContext claimContext)
        {
            claimContext = claimContext ?? throw new ArgumentNullException(nameof(claimContext));
            claimContext.Database.EnsureCreated();
        }

        //add new document
        public int AddClaimDocument(Document claimDocument)
        {

            claimContext.Documents.Add(claimDocument);
            return claimDocument.Id;
        }

        public void DeleteClaimDocument(int claimDocumentId)
        {
            var claimDocument = claimContext.Documents.FirstOrDefault(x => x.Id == claimDocumentId);
            if (claimDocument != null)
            {
                claimContext.Documents.Remove(claimDocument);
                // claimsContext.SaveChanges();
            }
        }



        //get documents for a claim
        public List<Document> GetClaimDocuments(int claimId)
        {
            // Ensure ClaimContext is initialized
            if (claimContext == null)
            {
                throw new InvalidOperationException("ClaimContext is not initialized.");
            }

            return claimContext.Documents
                .Where(x => x.ClaimId == claimId) // Ensure you use the correct property (ClaimId)
                .ToList();
        }

    }

}

