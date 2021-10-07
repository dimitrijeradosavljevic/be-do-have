using System.ComponentModel.DataAnnotations.Schema;

namespace BeDoHave.Data.AccessLayer.UserDefinedTables
{
    [NotMapped]
    public class PageTree
    {
        public int RootId { get; set; }
        public string Title { get; set; }
        public string IconName { get; set; }
        public string IconColor { get; set; }
        public string? Children { get; set; }
        public bool QueryRoot { get; set; }
    }
}
