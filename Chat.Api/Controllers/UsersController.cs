using Chat.Api.Exceptions;
using Chat.Api.Helper;
using Chat.Api.Managers;
using Chat.Api.Models;
using Chat.Api.Models.UserModel;
using Chat.Api.Models.UserModels;
using Chat.Api.ModelValidators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Chat.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserManager _userManager;
        private readonly UserHelper _userHelper;
        public UsersController(UserManager userManager, UserHelper userHelper) { _userManager = userManager; _userHelper = userHelper; }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody]CreateUserModel model)
        {
            try
            {
                var valdator = new CreateUserValidator();
              var modelState =  valdator.Validate(model);
                if (modelState.IsValid)
                {
                    await _userManager.RegisterUser(model);
                    return Ok(model);
                }
                return BadRequest(modelState.Errors);
            }
            catch (UserNotFound e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginUserModel model)
        {
            try
            {
                var valdator = new LoginUserValidator();
                valdator.Validate(model);
                var token = await _userManager.LoginUser(model);
                return Ok(token);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Authorize(Roles = "admin, user")]
        [HttpGet("Profile")]
        public async Task<IActionResult> GetUserById()
        {
            try
            {
                var user = await _userManager.GetUserById(_userHelper.GetUserId());
                return Ok(user);
            }
            catch (UserExist e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [Authorize(Roles = "admin, user")]
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            try
            {
                var users = await _userManager.GetAllUsers();
                return Ok(users);
            }
            catch (NullReferenceException e)
            {
                return BadRequest(e.Message);
            }
        }
        [Authorize(Roles = "admin,user")]
        [HttpPost("userId/add-or-update-photo")]
        public async Task<IActionResult> AddOrUpdateUserPhoto([FromForm] FileClass fileClass)
        {
            var result = await _userManager.AddOrUpdatePhoto(_userHelper.GetUserId(), fileClass.formFile);
            return Ok(result);
        }

        [Authorize(Roles = "admin,user")]
        [HttpDelete("Profile")]
        public async Task<IActionResult> DeleteUser()
        {
            try
            {
                var userId = _userHelper.GetUserId();
                var result = await _userManager.DeleteUser(userId);
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Authorize(Roles = "admin, user")]
        [HttpPut("Profile")]
        public async Task<IActionResult> UpdateUserGeneralData([FromBody] UpdateUserGeneralDataModel updateUserGeneralDataModel)
        {
            try
            {
                var userId = _userHelper.GetUserId();
                var result = await _userManager.UpdateGeneralUserData(userId, updateUserGeneralDataModel);
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Authorize(Roles = "admin, user")]
        [HttpPut("userId/Username")]
        public async Task<IActionResult> UpdateUserUsername(UpdateUsernameModel usernameModel)
        {
            try
            {
                var userId = _userHelper.GetUserId();
                var result = await _userManager.UpdateUserUsername(userId, usernameModel);
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Authorize(Roles = "admin, user")]
        [HttpPut("userId/Bio")]
        public async Task<IActionResult> UpdateUserBio(UpdateUserBioModel updateUserBio)
        {
            try
            {
                var userId = _userHelper.GetUserId();
                var result = await _userManager.UpdateUserBio(userId, updateUserBio);
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        public class FileClass
        {
            public IFormFile formFile { get; set; }
        }
    }
}
