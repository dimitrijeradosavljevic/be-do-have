using BeDoHave.Data.Core.Entities;


namespace BeDoHave.Application.Specifications
{
    public class UserByIdentityIdSpecification : BaseSpecification<User>
    {
        public UserByIdentityIdSpecification(string identityId)
             : base(user => user.IdentityId == identityId)
        {
        }
    }
}
