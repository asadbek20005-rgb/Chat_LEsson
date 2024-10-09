using Chat.Api.Chat_Hub;
using Chat.Api.Helper;
using Chat.Api.Managers;
using Chat.Api.Models.MessageModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Chat.Api.Controllers
{
    [Route("api/users/userId/chats/{chatId:guid}/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly MessageManager _messageManagaer;
        private readonly UserHelper _userHelper;
        private readonly IHubContext<ChatHub> _hubContext;

        public MessagesController(MessageManager messageManager, UserHelper user, IHubContext<ChatHub> hubContext)
        {
            _messageManagaer = messageManager;
            _userHelper = user;
            _hubContext = hubContext;
        }
        [Authorize(Roles = "admin")]
        [HttpGet("/api/Messages")]
        public async Task<IActionResult> GetAllMessages()
        {
            var allMessages = await _messageManagaer.GetAllMessages();
            return Ok(allMessages);
        }

        [Authorize(Roles = "admin, user")]    
        [HttpGet]
        public async Task<IActionResult> GetAllChatMessages(Guid chatId)
        {
            var chatMessages = await _messageManagaer.GetAllChatMessages(chatId);
            return Ok(chatMessages);
        }

        [Authorize(Roles ="admin, user")]
        [HttpGet("{messageId:int}")]
        public async Task<IActionResult> GetChatMessages(Guid chatId, int messageId)
        {
            var result = await _messageManagaer.GetChatMessageById(chatId, messageId);
            return Ok(result);
        }
            
       
    
        [Authorize(Roles = "admin, user")]
        [HttpPost("send-text-message")]
        public async Task<IActionResult> SendTextMessage(Guid chatId,[FromBody] SendMessageModel model)
        {
            try
            {
                var userId = _userHelper.GetUserId();
                var result = await _messageManagaer.SendMessage(userId, chatId, model);
                await _hubContext.Clients.All.SendAsync("ReceiveMessage", result);

                return Ok(result);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [Authorize(Roles = "admin, user")]
        [HttpPost("send-file-message")]
        public async Task<IActionResult> SendFileMessage(Guid chatId, [FromForm] FileMessageModel model)
        {
            try
            {
                var userId = _userHelper.GetUserId();
                var result = await _messageManagaer.SendFileMessage(userId, chatId, model);
                await _hubContext.Clients.All.SendAsync("ReceiveMessage", result);

                return Ok(result);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}