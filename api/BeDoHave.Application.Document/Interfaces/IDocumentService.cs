using BeDoHave.Application.Document.DTOs;
using BeDoHave.Shared.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BeDoHave.Application.Document.Interfaces
{
    public interface IDocumentService
    {
        Task<IList<DocumentDTO>> GetDocumentsAsync(PaginationParameters paginationParameters);
        Task<IList<DocumentDTO>> GetAllDocumentsAsync();
        Task<DocumentDTO> GetDocumentByIdAsync(int documentId);
        Task CreateDocumentAsync(CreateDocumentDTO createDocumentDTO);
        Task UpdateDocumentAsync(UpdateDocumentDTO updateDocumentDTO);
        Task DeleteDocumentAsync(int documentId);
    }
}
