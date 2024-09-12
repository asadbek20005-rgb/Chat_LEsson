using Chat.Api.ConvertExtensions;
using Chat.Api.Dtos;
using Chat.Api.Entities;
using Chat.Api.Exceptions;
using Chat.Api.Helper;
using Chat.Api.UnitOfWork.Interfaces;

namespace Chat.Api.Managers
{
    public class ChatManager
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserHelper _userHelper;
        public ChatManager(IUnitOfWork unitOfWork, UserHelper userHelper)
        {
            _unitOfWork = unitOfWork;
            _userHelper = userHelper;
        }

        // For Admin
        
        public async Task<List<ChatDto>> GetAllChats()
        {
            var chats= await _unitOfWork.ChatRepository.GetAllChats();
            return chats.ParseToDtos();
        }

        public async Task<List<ChatDto>> GetAllUserChats(Guid userId)
        {
            var userChats = await _unitOfWork.ChatRepository.GetAllUserChats(userId);

            return userChats.ParseToDtos();
        }


        public async Task<ChatDto> AddOrEnterChat(Guid fromUserId, Guid toUserId)
        {
            var (isExist, chat) = await _unitOfWork.ChatRepository.CheckChatExist(fromUserId, toUserId);
            if (isExist)
                return chat.ParseToDto();
            var fromUser = await _unitOfWork.UserRepository.GetUserById(fromUserId);
            var toUser = await _unitOfWork.UserRepository.GetUserById(toUserId);
            List<string> names = new()
            {
                StaticHelper.GetName(fromUser.FirstName, fromUser.LastName),
                StaticHelper.GetName(toUser.FirstName, toUser.LastName)
            };
            chat = new Entities.Chat
            { ChatNames = names };
            await _unitOfWork.ChatRepository.AddChat(chat);
            var fromUserChat = new User_Chat()
            {
                UserId = fromUserId,
                ToUserId = toUserId,
                ChatId = chat.Id,
            };
            await _unitOfWork.UserChatRepository.AddUserChat(fromUserChat);
            var toUserChat = new User_Chat()
            {
                UserId = toUserId,
                ToUserId = fromUserId,
                ChatId = chat.Id,
            };
            await _unitOfWork.UserChatRepository.AddUserChat(toUserChat);
            return chat.ParseToDto();
        }

        public async Task<string> DeleteChat(Guid chatId)
        {
            //var user = await _unitOfWork.UserRepository.GetUserById(userId);

            var userId = _userHelper.GetUserId();
            var chat = await _unitOfWork.ChatRepository.GetUserChatById(userId, chatId);
           await _unitOfWork.ChatRepository.DeleteChat(chat);
            return "Delete was successfull done!";
        }
    }
}
