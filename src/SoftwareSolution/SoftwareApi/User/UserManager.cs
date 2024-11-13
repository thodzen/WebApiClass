using Marten;

namespace Software.Api.User;

public class UserManager(IHttpContextAccessor context, IDocumentSession session) : IProvideUserInformation
{
    public async Task<UserInformation> GetUserInformationAsync()
    {
        var sub = context?.HttpContext?.User?.Identity?.Name ?? throw new Exception("Cannot Be Used in a non-authenticated environment");
        var user = await session.Query<UserInformation>()
            .Where(u => u.Sub == sub)
            .SingleOrDefaultAsync();

        if (user is null)
        {
            var entity = new UserInformation
            {
                Id = Guid.NewGuid(),
                Sub = sub!
            };
            session.Store(entity);
            await session.SaveChangesAsync();
            return entity;

        }
        else
        {
            return user;
        }





    }
}

public class UserInformation
{
    public Guid Id { get; set; }
    public string Sub { get; set; } = string.Empty;
}


// GET /vendors (done)
// GET /vendors/{id}/catalog/{id} - get a single item (done)
// GET /vendors/{id}/catalog - get all the software from a particular vendor 
// GET /catalog - returns ALL software
// GET /catalog?addedby=x89999
// GET /techs - returns a list of all the techs? (weird.)
// GET /techs/{id:guid}/software 