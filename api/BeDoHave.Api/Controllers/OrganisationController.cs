using BeDoHave.Application.DTOs;
using BeDoHave.Application.Interfaces;
using BeDoHave.Shared.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BeDoHave.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]

    public class OrganisationController : Controller
    {
        private readonly IOrganisationService _organisationService;
        private readonly IAccountService _accountService;

        public OrganisationController(IOrganisationService organisationService, IAccountService accountService)
        {
            _organisationService = organisationService;
            _accountService = accountService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetOrganisations([FromQuery] PaginationParameters paginationParameters)
        {
            HttpContext.Request.Headers.TryGetValue("Authorization", out var token);
            var currentUser = await _accountService.GetAuthenticatedUserAsync(token);

            var organisations = await _organisationService.GetOrganisationsAsync(currentUser.Id, paginationParameters);
            return Ok(organisations);
        }

        [HttpGet("all")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllOrganisation()
        {
            HttpContext.Request.Headers.TryGetValue("Authorization", out var token);
            var currentUser = await _accountService.GetAuthenticatedUserAsync(token);

            var organisations = await _organisationService.GetAllOrganisationsAsync(currentUser.Id);

            return Ok(organisations);
        }

        [HttpGet("{id}/document-tree")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetDocumentTreeOrganisation(int id)
        {
            var organisations = await _organisationService.GetOrganisationTreeAsync(id);

            return Ok(organisations);
        }
        
        [HttpGet("{id}/members")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetMembers(int id, [FromQuery] PaginationParameters paginationParameters)
        {
            var paginationResponse = await _organisationService.GetOrganisationMembersAsync(id, paginationParameters);

            return Ok(paginationResponse);
        }
        
        [HttpGet("{id}/non-members")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetNonMembers(int id, [FromQuery] PaginationParameters paginationParameters)
        {
            var paginationResponse = await _organisationService.GetOrganisationNonMembersAsync(id, paginationParameters);

            return Ok(paginationResponse);
        }
        
        [HttpPost("{id}/add-member")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> AddMember(int id, [FromBody] int userId)
        {
            
            await _organisationService.AddMemberAsync(id, userId);

            return Ok();
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetOrganisation(int id)
        {
            var organisation = await _organisationService.GetOrganisationByIdAsync(id);

            return Ok(organisation);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateOrganisation(CreateOrganisationDTO createOrganisationDTO)
        {
            HttpContext.Request.Headers.TryGetValue("Authorization", out var token);
            var currentUser = await _accountService.GetAuthenticatedUserAsync(token);
            createOrganisationDTO.AuthorId = currentUser.Id;

            var organisation = await _organisationService.CreateOrganisationAsync(createOrganisationDTO);

            return Created("", organisation);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateOrganisation(int id, UpdateOrganisationDTO updateOrganisationDTO)
        {
            updateOrganisationDTO.Id = id;

            var organisation = await _organisationService.UpdateOrganisationAsync(updateOrganisationDTO);

            return Ok(organisation);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteOrganisation(int id)
        {
            await _organisationService.DeleteOrganisationAsync(id);

            return Ok();
        }
    }
}
