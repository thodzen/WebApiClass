
namespace Software.Api.User;

public interface IProvideUserInformation
{
    Task<UserInformation> GetUserInformationAsync();
}