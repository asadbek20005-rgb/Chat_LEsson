using Chat.Api.Helper;
using Chat.Api.Managers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Chat.Api.Controllers
{
    [Route("api/users/userId/[controller]")]
    [ApiController]
    public class ChatsController : ControllerBase
    {
        private readonly ChatManager _chatManager;
        private readonly UserHelper _userHelper;
        public ChatsController(ChatManager chatManager, UserHelper userHelper)
        {
            _chatManager = chatManager;
            _userHelper = userHelper;
        }
        [Authorize(Roles = "admin")]
        [HttpGet("/api/Chats")]
        public async Task<IActionResult> GetAllChats()
        {
            var allUsers = await _chatManager.GetAllChats();

            return Ok(allUsers);
        }

        [Authorize(Roles = "admin, user")]
        [HttpGet]
        public async Task<IActionResult> GetAllUserChats()
        {
            var userChats = await _chatManager.GetAllUserChats(_userHelper.GetUserId());
            return Ok(userChats);
        }
        [Authorize(Roles = "admin, user")]
        [HttpPost]
        public async Task<IActionResult> AddOrEnterChat([FromBody] Guid userId2)
        {
            var chat = await _chatManager.AddOrEnterChat(_userHelper.GetUserId(), userId2);
            return Ok(chat);
        }

        [Authorize(Roles = "admin, user")]
        [HttpDelete("{chatId:guid}")]
        public async Task<IActionResult> DeleteUserChat(Guid chatId)
        {
            try
            {
            string result = await _chatManager.DeleteChat(chatId);
            return Ok(result);

            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
