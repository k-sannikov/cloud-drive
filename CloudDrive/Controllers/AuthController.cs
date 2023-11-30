using Application.Auth;
using Application.Foundations;
using CloudDrive.Auth;
using CloudDrive.Dto.AuthDto;
using CloudDrive.Dto.Extensions;
using CloudDrive.Utilities;
using Domain.Auth;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CloudDrive.Controllers
{
    [ApiController]
    [Route("api")]
    public class AuthController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthService _authService;
        private readonly ITokenService _tokenService;
        private readonly IValidator<RegisterDto> _registerDtoValidator;
        private readonly IValidator<LoginDto> _loginDtoValidator;
        private readonly IValidator<RefreshTokenDto> _refreshTokenDtoValidator;

        const int _refreshTokenExpiredTime = 20;

        public AuthController(IUnitOfWork unitOfWork,
            IAuthService authService,
            ITokenService tokenService,
            IValidator<RegisterDto> registerDtoValidator,
            IValidator<LoginDto> loginDtoValidator,
            IValidator<RefreshTokenDto> refreshTokenDtoValidator)
        {
            _unitOfWork = unitOfWork;
            _authService = authService;
            _tokenService = tokenService;
            _registerDtoValidator = registerDtoValidator;
            _loginDtoValidator = loginDtoValidator;
            _refreshTokenDtoValidator = refreshTokenDtoValidator;
        }

        /// <summary>
        /// Регистрация пользователя
        /// </summary>
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

        /// <summary>
        /// Получить токен
        /// </summary>
        [HttpPost]
        [Route("token")]
        public async Task<IActionResult> Token([FromBody] LoginDto body)
        {
            ValidationResult validationResult = await _loginDtoValidator.ValidateAsync(body);

            if (!validationResult.IsValid)
            {
                return BadRequest(new ErrorResponse(validationResult.ToDictionary()));
            }

            User user = await _authService.GetUserByCredentionals(body.Username, body.Password);

            if (user is null)
            {
                return BadRequest(new ErrorResponse("Username or password not exist"));
            }

            string jwtToken = _tokenService.GenerateJwtToken(user);
            string refreshToken = _tokenService.GenerateRefreshToken();

            user.SetRefreshToken(refreshToken, _refreshTokenExpiredTime);
            _unitOfWork.Commit();

            TokenDto response = new()
            {
                AccessToken = jwtToken,
                RefreshToken = refreshToken
            };

            return Ok(response);
        }

        /// <summary>
        /// Обновить токен
        /// </summary>
        [HttpPost]
        [Route("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenDto body)
        {
            ValidationResult validationResult = await _refreshTokenDtoValidator.ValidateAsync(body);

            if (!validationResult.IsValid)
            {
                return BadRequest(new ErrorResponse(validationResult.ToDictionary()));
            }

            User user = await _authService.GetUserByToken(body.RefreshToken);

            if (user is null)
            {
                return BadRequest(new ErrorResponse("Refresh token not exist"));
            }

            if (user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                return BadRequest(new ErrorResponse("Refresh token expired"));
            }

            string jwtToken = _tokenService.GenerateJwtToken(user);
            string refreshToken = _tokenService.GenerateRefreshToken();

            user.SetRefreshToken(refreshToken, _refreshTokenExpiredTime);
            _unitOfWork.Commit();

            TokenDto response = new()
            {
                AccessToken = jwtToken,
                RefreshToken = refreshToken
            };

            return Ok(response);
        }

        /// <summary>
        /// Проверить авторизацию пользователя
        /// </summary>
        [Authorize]
        [HttpGet]
        [Route("check")]
        public IActionResult Check()
        {
            return Ok();
        }
    }
}
