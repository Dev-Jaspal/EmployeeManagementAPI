using EmployeeManagement.BusinessModel.Dto;
using EmployeeManagement.Services.IService;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;

namespace EmployeeManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowOrigin")]
    public class AuthController : ControllerBase
    {
        #region Private Member Variables

        /// <summary>
        /// Authenticate service interface variable.
        /// </summary>
        private readonly IAuthenticateService _authenticateService;

        #endregion

        public AuthController(IAuthenticateService authenticateService)
        {
            _authenticateService = authenticateService;
        }
        // GET api/values  
        [HttpPost, Route("login")]
        [EnableCors("AllowOrigin")]
        public IActionResult Login([FromBody] LoginModel user)
        {
            if (user == null)
            {
                return BadRequest(new { message = "User name or password is incorrect." });
            }
            var authUser = _authenticateService.Authenticate(Convert.ToInt32(user.EmployeeId), user.Password);
            if (authUser == null)
            {
                return BadRequest(new { message = "User name or password is incorrect." });
            }
            return Ok(authUser);
            //if (user.EmployeeId == 7925 && user.Password == "123")
            //{
            //  var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("KeyForSignInSecret@1234"));
            //  var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            //  var tokeOptions = new JwtSecurityToken(
            //      issuer: "http://localhost:44323",
            //      audience: "http://localhost:44323",
            //      claims: new List<Claim>(),
            //      expires: DateTime.Now.AddMinutes(30),
            //      signingCredentials: signinCredentials
            //  );

            //  var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
            //  return Ok(new { Token = tokenString });
            //else
            //{
            //  return Unauthorized();
            //}
        }
    }
}