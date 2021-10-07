using AutoMapper;
using BeDoHave.Application.Document.DTOs;
using BeDoHave.Application.Document.Interfaces;
using BeDoHave.Application.Document.Specifications;
using BeDoHave.Data.AccessLayer.Interfaces;
using BeDoHave.Shared.Entities;
using BeDoHave.Shared.Exceptions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BeDoHave.Application.Document.Services
{
    public class DocumentService : IDocumentService
    {
        private readonly IAsyncRepository<Data.Core.Entities.Document> _documentRepository;
        private readonly IMapper _mapper;

        public DocumentService(
            IAsyncRepository<Data.Core.Entities.Document> documentRepository,
            IMapper mapper)
        {
            _documentRepository = documentRepository;
            _mapper = mapper;
        }

        public async Task<IList<DocumentDTO>> GetAllDocumentsAsync()
        {
            var documents = await _documentRepository.GetAsync();

            return _mapper.Map<IList<DocumentDTO>>(documents);
        }

        public async Task<IList<DocumentDTO>> GetDocumentsAsync(PaginationParameters paginationParameters)
        {
            var documents = await _documentRepository.GetBySpecAsync(
                new DocumentSpecification(
                    document => document.Title.Contains(paginationParameters.Keyword),
                    start: paginationParameters.PageIndex * paginationParameters.PageSize,
                    take: paginationParameters.PageSize,
                    orderBy: paginationParameters.OrderBy,
                    direction: paginationParameters.Direction));

            return _mapper.Map<IList<DocumentDTO>>(documents);
        }

        public async Task<DocumentDTO> GetDocumentByIdAsync(int documentId)
        {
            var document = await _documentRepository.GetByIdAsync(documentId);

            if (document is null)
            {
                throw new ApiException($"Document: {documentId} not found", 404);
            }

            return _mapper.Map<DocumentDTO>(document);
        }

        public async Task CreateDocumentAsync(CreateDocumentDTO createDocumentDTO)
        {
            var document = _mapper.Map<Data.Core.Entities.Document>(createDocumentDTO);

            await _documentRepository.AddAsync(document);
        }

        public async Task UpdateDocumentAsync(UpdateDocumentDTO updateDocumentDTO)
        {
            var document = await _documentRepository.GetByIdAsync(updateDocumentDTO.Id);

            if (document is null)
            {
                throw new ApiException($"Document {updateDocumentDTO.Id} not found", 404);
            }

            _mapper.Map(updateDocumentDTO, document);

            await _documentRepository.UpdateAsync(document);
        }

        public async Task DeleteDocumentAsync(int documentId)
        {
            var document = await _documentRepository.GetByIdAsync(documentId);

            if (document is null)
            {
                throw new ApiException($"Document {documentId} not found", 404);
            }

            await _documentRepository.DeleteAsync(document);
        }
    }
}
