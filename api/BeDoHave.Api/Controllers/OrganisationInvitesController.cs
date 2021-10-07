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
    [Route("api/organisation-invites")]
    [ApiController]

    public class OrganisationInvitesController : Controller
    {
        private readonly IOrganisationInviteService _organisationInviteService;

        public OrganisationInvitesController(IOrganisationInviteService organisationInviteService)
        {
            _organisationInviteService = organisationInviteService;
        }

        [HttpGet("")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetInvites(int id, [FromQuery] PaginationParameters parameters)
        {
            var response = await _organisationInviteService.GetInvitesAsync(parameters);

            return Ok(response);
        }
        
        [HttpPost("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> InviteRespond(int id, [FromBody] bool response)
        {
            var result = await _organisationInviteService.RespondOnInviteAsync(id, response);

            return Ok(result);
        }
        
        [HttpPost("{organisationId}/invite")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Invite(int organisationId, [FromBody] CreateOrganisationInviteDto inviteDto)
        {
            await _organisationInviteService.InviteAsync(organisationId, inviteDto);

            return Ok();
        }
        
    }
}
