﻿using Chat.Api.Exceptions;
using Chat.Api.Helper;
using Chat.Api.Managers;
using Chat.Api.Models;
using Chat.Api.Models.UserModel;
using Chat.Api.Models.UserModels;
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
        public async Task<IActionResult> Register(CreateUserModel model)
        {
            try
            {
                await _userManager.RegisterUser(model);
                return Ok(model);
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
        [Authorize(Roles = "admin")]
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
        [Authorize(Roles = "user")]
        [HttpPost("userId/add-or-update-photo")]
        public async Task<IActionResult> AddOrUpdateUserPhoto([FromForm] FileClass fileClass)
        {
            var result = await _userManager.AddOrUpdatePhoto(_userHelper.GetUserId(), fileClass.formFile);
            return Ok(result);
        }

        [Authorize(Roles = "admin,user")]
        [HttpDelete("userId")]
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
        [HttpPut("userId")]
        public async Task<IActionResult> UpdateUserGeneralData(UpdateUserGeneralDataModel updateUserGeneralDataModel)
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
                var result = await _userManager.UpdateUserUsername(usernameModel);
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