namespace BeDoHave.Data.AccessLayer.Entities
{
    public class OrganisationInviteDb
    {
        public int Id { get; set; }
        public int OrganisationId { get; set; }
        public string OrganisationName { get; set; }
        public int InviterId { get; set; }
        public int InvitedId { get; set; }
        public string InviterName { get; set; }
    }
}