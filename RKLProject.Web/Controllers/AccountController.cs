using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using RKLProject.Core.DTOs;
using RKLProject.Core.IServices;
using RKLProject.Core.Utilities;
using RKLProject.Utilities.Identity;

namespace RKLProject.Web.Controllers
{
    public class AccountController : BaseController
    {
        private readonly IUserService _userService;
        public AccountController(IUserService userService)
        {
            _userService = userService;
        }

        #region Login, Register and LogOut

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserDTO loginUser)
        {
            if (!ModelState.IsValid)
                return JsonResponseStatus.Error();

            var Login = await _userService.LoginUser(loginUser);

            switch (Login)
            {
                case LoginResponse.Exist:
                    return JsonResponseStatus.NotFound(new { status = "El Email Introducido no existe" });
                case LoginResponse.NotActive:
                    return JsonResponseStatus.Error(new { status = "La cuenta no esta activo" });
                case LoginResponse.Success:
                    var user = await _userService.getUserByEmailAddress(loginUser.Email);
                    var secretKey =
                        new SymmetricSecurityKey(Encoding.UTF8.GetBytes("RKL158e82a5-4899-4a57-93e2-a63599c5da59"));
                    var signninCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
                    var tokenOptions = new JwtSecurityToken(
                        issuer: "https://localhost:44399",
                        claims: new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, user.Email),
                            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
                        },
                        expires: DateTime.Now.AddDays(30),
                        signingCredentials: signninCredentials
                    );

                    var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
                    user.Token = tokenString;
                    user.ExpireTime = 30;
                    return JsonResponseStatus.Success(user);
            }

            return JsonResponseStatus.Error();
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserDTO user)
        {
            var register = await _userService.RegisterUser(user);

            switch (register)
            {
                case RegisterResponse.Exist:
                    return JsonResponseStatus.Error("El usuario existe");
                case RegisterResponse.Success:
                    return JsonResponseStatus.Success();
            }
            return JsonResponseStatus.Success();
        }


        [HttpGet("sign-out")]
        public async Task<IActionResult> LogOut()
        {
            if (User.Identity.IsAuthenticated)
            {
                await HttpContext.SignOutAsync();
                return JsonResponseStatus.Success();
            }

            return JsonResponseStatus.Error();
        }

        #endregion

        #region Check if User is Authenticated

        [HttpPost("check-auth")]
        public async Task<IActionResult> CheckAuth()
        {
            if (User.Identity.IsAuthenticated)
            {
                UserInfoDTO user = await _userService.getUserByUId(User.GetUserId());
                return JsonResponseStatus.Success(user);
            }

            return JsonResponseStatus.Error();
        }

        #endregion

        #region UserCount


        [HttpGet("get-users-count")]
        public async Task<long> UsersCount()
        {
            return await _userService.TotalUsersCount();
        }

        #endregion

    }
}
