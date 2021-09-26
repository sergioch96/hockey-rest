using hockey_rest.Models.Request;
using hockey_rest.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hockey_rest.Services
{
    public interface IUserService
    {
        UserResponse Auth(AuthRequest model);
    }
}
