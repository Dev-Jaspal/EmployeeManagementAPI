using EmployeeManagement.BusinessModel.AuthModel;
using EmployeeManagement.BusinessModel.Dto;
using EmployeeManagement.Services.IService;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EmployeeManagement.Services.Service
{
    public class AuthenticateService : IAuthenticateService
    {
        //private List<LoginModel> loginModels = new List<LoginModel>()
        //{
        //  new LoginModel()
        //  {
        //    EmployeeId = 7925,
        //    FirstName ="Jaspal",
        //    LastName="Singh",
        //    UserName ="Jaspalsingh",
        //    Password="123"
        //  }
        //};
        private readonly AppSettings _appSettings;
        private readonly IUserService _userService;
        public AuthenticateService(IOptions<AppSettings> appSettings, IUserService userService)
        {
            _appSettings = appSettings.Value;
            _userService = userService;
        }
        public LoginModel Authenticate(int employeeId, string password)
        {
            // var user = loginModels.SingleOrDefault(x => x.EmployeeId == employeeId && x.UserName == userName && x.Password == password);
            var user = _userService.AuthenticateUserFromDb(employeeId, password);
            if (user == null)
                return null;

            // user found

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.SecretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new System.Security.Claims.ClaimsIdentity(new Claim[] {
          new Claim(ClaimTypes.Name, user.EmployeeId.ToString()),
		  // need to get role from database.
		  new Claim(ClaimTypes.Role, user.Role),
		//  new Claim(ClaimTypes.Role, "Employee"),
		  new Claim(ClaimTypes.Version, "V3.1")
        }),
                Expires = DateTime.UtcNow.AddDays(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            user.Token = tokenHandler.WriteToken(token);
            user.Password = null;

            return user;
        }
    }
}
