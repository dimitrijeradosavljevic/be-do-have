using System;
using System.Collections.Generic;
using System.Globalization;

namespace BeDoHave.Shared.Entities
{
    public class PaginationParameters
    {
        private string _orderBy = "Id";
        private string _keyword = string.Empty;

        public int PageIndex { get; set; } = 0;
        public int PageSize { get; set; } = 10;
        public string Keyword { get => _keyword; set => _keyword = value ?? string.Empty; }
        public string OrderBy { get => _orderBy; set => _orderBy = ToPascalCase(value); }
        public string Direction { get; set; }

        public int? UserId { get; set; }

        public int? OrganisationId { get; set; }

        public static string ToPascalCase(string value)
        {
            return CultureInfo.InvariantCulture.TextInfo
                .ToTitleCase(value);
        }
    }
}