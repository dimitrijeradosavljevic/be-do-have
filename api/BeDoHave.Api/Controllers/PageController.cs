using System.Text.Json;
using BeDoHave.Application.DTOs;
using BeDoHave.Application.Interfaces;
using BeDoHave.Shared.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using BeDoHave.ElasticSearch.Entities;
using Microsoft.AspNetCore.Authorization;

namespace BeDoHave.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PageController : Controller
    {
        private readonly IPageService _pageService;
        private readonly IAccountService _accountService;

        public PageController(IPageService pageService, IAccountService accountService)
        {
            _pageService = pageService;
            _accountService = accountService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        //[AllowAnonymous]
        public async Task<IActionResult> GetPages([FromQuery] PaginationParameters paginationParameters)
        {
            var pages = await _pageService.GetPagesAsync(paginationParameters);

            return Ok(pages);
        }

        [HttpGet("all")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        // [AllowAnonymous]
        public async Task<IActionResult> GetAllPages()
        {
            var pages = await _pageService.GetAllPagesAsync();

            return Ok(pages);
        }
        
        [HttpGet("trashed")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetTrashedPages([FromQuery] PaginationParameters paginationParameters)
        {
            var pages = await _pageService.GetTrashedPagesAsync(paginationParameters);

            return Ok(pages);
        }
        
        [HttpPost("search")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> SearchPages([FromBody] PageSearchParameters parameters)
        { var pages = await _pageService.SearchPagesAsync(parameters);

            return Ok(pages);
        }
        
        [HttpGet("for-picker")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> ForPicker([FromQuery] PaginationParameters paginationParameters)
        {
            HttpContext.Request.Headers.TryGetValue("Authorization", out var token);
            var currentUser = await _accountService.GetAuthenticatedUserAsync(token);

            var pages = await _pageService.GetPagesForPickerAsync(paginationParameters, currentUser.Id);

            return Ok(pages);
        }
        
        [HttpGet("auto-complete")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAutoComplete([FromQuery] string term)
        {
            var suggestions = await _pageService.GetAutoCompleteAsync(term);

            return Ok(suggestions);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetPage(int id)
        {
            var page = await _pageService.GetPageByIdAsync(id);

            return Ok(page);
        }
        
        [HttpPost("{id}/search")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> SearchPage([FromBody] PageSearchParameters parameters)
        {
            var page = await _pageService.SearchPageAsync(parameters);
            
            return Ok(JsonSerializer.Serialize(page));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreatePage(CreatePageDTO createPageDTO)
        {
            var page = await _pageService.CreatePageAsync(createPageDTO);

            return Created("", page);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdatePage(int id, UpdatePageDTO updatePageDTO)
        {
            updatePageDTO.Id = id;

            var page = await _pageService.UpdatePageAsync(updatePageDTO);

            return Ok(page);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeletePage(int id)
        {
            await _pageService.DeletePageAsync(id);

            return Ok();
        }
        
        
        [HttpPut("{id}/trash")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> TrashPage(int id, UpdatePageDTO page)
        {
            await _pageService.TrashPageAsync(id, page.Archived);

            return Ok();
        }
        
        [HttpPut("{id}/move")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> MovePage(int id, [FromBody] int directParent)
        {
            await _pageService.MovePageAsync(id, directParent);

            return Ok();
        }
        
        [HttpPut("{id}/move-under-organisation")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> MovePageUnderOrganisation(int id, [FromBody] int organisationId)
        {
            await _pageService.MovePageUnderOrganisationAsync(id, organisationId);

            return Ok();
        }

    }
}
