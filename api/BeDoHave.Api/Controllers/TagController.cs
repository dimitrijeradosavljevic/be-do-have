using System.Collections.Generic;
using System.Threading.Tasks;
using BeDoHave.Application.DTOs;
using BeDoHave.Application.Interfaces;
using BeDoHave.Shared.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BeDoHave.Api.Controllers
{
    [Authorize]
    [Route("api/tags")]
    [ApiController]
    public class TagController : Controller
    {
        private readonly ITagService _tagService;

        public TagController(ITagService tagService)
        {
            _tagService = tagService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        //[AllowAnonymous]
        public async Task<IActionResult> GetTags([FromQuery] PaginationParameters paginationParameters)
        {
            var pages = await _tagService.GetTagsAsync(paginationParameters);

            return Ok(pages);
        }
        
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateTag(CreateTagDto createTag)
        {
            var tag = await _tagService.CreateTagAsync(createTag);

            return Created("", tag);
        }

    }
}