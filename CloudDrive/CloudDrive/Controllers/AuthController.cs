using Application.Auth;
using Application.Foundations;
using CloudDrive.Dto;
using CloudDrive.Utilities;
using Domain.Auth;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CloudDrive.Controllers
{
    [ApiController]
    [Route("api")]
    public class AuthController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthService _authService;
        private readonly IValidator<RegisterDto> _registerDtoValidator;
        private readonly IValidator<LoginDto> _loginDtoValidator;

        public AuthController(IUnitOfWork unitOfWork,
            IAuthService authService,
            IValidator<RegisterDto> registerDtoValidator,
            IValidator<LoginDto> loginDtoValidator)
        {
            _unitOfWork = unitOfWork;
            _authService = authService;
            _registerDtoValidator = registerDtoValidator;
            _loginDtoValidator = loginDtoValidator;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto body)
        {
            ValidationResult validationResult = await _registerDtoValidator.ValidateAsync(body);

            if (!validationResult.IsValid)
            {
                return BadRequest(new ErrorResponse(validationResult.ToDictionary()));
            }

            try
            {
                await _authService.RegisterUser(body.ToDomain());
            }
            catch (Exception exception)
            {
                return BadRequest(new ErrorResponse(exception.Message));
            }

            _unitOfWork.Commit();

            return Ok();
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto body)
        {
            ValidationResult validationResult = await _loginDtoValidator.ValidateAsync(body);

            if (!validationResult.IsValid)
            {
                return BadRequest(new ErrorResponse(validationResult.ToDictionary()));
            }

            User user = await _authService.GetUser(body.Username, body.Password);

            if (user is null)
            {
                return BadRequest(new ErrorResponse("Username or password not exist"));
            }

            List<Claim> claims = new ()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.Username),
            };

            ClaimsIdentity identity = new(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            ClaimsPrincipal principial = new(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principial, new AuthenticationProperties()
            {
                IsPersistent = true
            }); ;

            return Ok();
        }
    }
}
