using BeDoHave.Data.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BeDoHave.Application.DTOs
{
    public class PageDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        
        public string Content { get; set; }
        
        public int UserId { get; set; }
        
        public User User { get; set; }
        
        public bool Archived { get; set; }
        
        public string IconName { get; set; }
        
        public string IconColor { get; set; }

        public DateTime CreatedAt { get; set; }
        public virtual ICollection<TagDto> Tags { get; set; }
        public string? Path { get; set; }
        public IList<PageLink> AncestorsLinks { get; set; }
        
        public IList<DescendantDto> Descendants { get; set; }
    }

    public class CreatePageDTO
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }
        [Required]
        public int UserId { get; set; }
        
        public int OrganisationId { get; set; }
        
        public ICollection<Tag> Tags { get; set; }

        public int? DirectPageId { get; set; }
        public string IconName { get; set; }
        public string IconColor { get; set; }
    }

    public class UpdatePageDTO
    {
        [Required]
        public int Id { get; set; }

        public string Title { get; set; }
        
        public string Content { get; set; }

        public bool Archived { get; set; }
        
        public string IconName { get; set; }

        public string IconColor { get; set; }

        public IList<Tag> Tags { get; set; }
        // Organisation logic
    }

    public class PageSearchDto
    {
        [Required]
        public int Id { get; set; }
        
        [Required] 
        public string Title { get; set; }
        
        [Required] 
        public IReadOnlyDictionary<string, IReadOnlyCollection<string>> HighlightedContent { get; set; }
    }

    public class DescendantDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string IconName { get; set; }
        public string IconColor { get; set; }
    }
}
