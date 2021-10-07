namespace BeDoHave.Data.Core.Entities
{
    public class TagPage: BaseEntity
    {
        public int TagId { get; set; }
        public virtual Tag Tag { get; set; }

        public int PageId { get; set; }
        public virtual Page Page { get; set; }
    }
}