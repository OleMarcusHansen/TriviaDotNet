using Microsoft.AspNetCore.SignalR;
using System.Linq;

namespace HIOF.Net.V2023.UserIdeProvider
{
    public class UserIdProvider : IUserIdProvider
    {
        public string? GetUserId(HubConnectionContext connection)
        {
            return connection.User.Claims.FirstOrDefault(x => x.Type == "userid").Value;
        }
    }
}