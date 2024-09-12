using Chat.Api.ConvertExtensions;
using Chat.Api.Dtos;
using Chat.Api.Entities;
using Chat.Api.Helper;
using Chat.Api.Models.MessageModel;
using Chat.Api.UnitOfWork.Interfaces;

namespace Chat.Api.Managers
{
    public class MessageManager
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserHelper _userHelper;
        private readonly MessageHelper _messageHelper;
        public MessageManager(IUnitOfWork unitOfWork, UserHelper userHelper, MessageHelper messageHelper)
        {
            _unitOfWork = unitOfWork;
            _userHelper = userHelper;
            _messageHelper = messageHelper;
        }

        public async Task<List<MessageDto>> GetAllMessages()
        {
            var allMessages = await _unitOfWork.MessageRepository.GetAllMessages();

            return allMessages.ParseToDtos();
        }

        public async Task<List<MessageDto>> GetAllChatMessages(Guid chatId)
        {
            var chat = await _unitOfWork.ChatRepository.GetUserChatById(_userHelper.GetUserId(), chatId);
            var chatMessages = await _unitOfWork.MessageRepository.GetAllChatMessages(chatId);

            return chatMessages.ParseToDtos();
        }


        public async Task<MessageDto> GetMessageById(int messageId)
        {
            var message = await _unitOfWork.MessageRepository.GetMessageById(messageId);

            return message.ParseToDto();
        }

        public async Task<MessageDto> GetChatMessageById(Guid chatId, int messageId)
        {
            var chat = await _unitOfWork.ChatRepository.GetUserChatById(_userHelper.GetUserId(), chatId);
            var chatMessage = await _unitOfWork.MessageRepository.GetChatMessageById(chatId, messageId);
            return chatMessage.ParseToDto();
        }

        public async Task<MessageDto> SendMessage(Guid userId, Guid chatId, SendMessageModel message)
        {
            var user = await _unitOfWork.UserRepository.GetUserById(userId);
            var chat = await _unitOfWork.ChatRepository.GetUserChatById(_userHelper.GetUserId(), chatId);

            var newMessage = new Message
            {
                Text = message.Text,
                FromUsername = user.Username,
                FromUserId = userId,
                ChatId = chatId
            };

            await _unitOfWork.MessageRepository.AddMessage(newMessage);
            return newMessage.ParseToDto();
        }

        public async Task<MessageDto> SendFileMessage(Guid userId, Guid chatId, FileMessageModel fileMessage)
        {
            var user = await _unitOfWork.UserRepository.GetUserById(userId);
            var chat = await _unitOfWork.ChatRepository.GetUserChatById(_userHelper.GetUserId(), chatId);

            string fileUrl = await _messageHelper.WriteToFile(fileMessage.FormFile);

            var newContent = new Content
            {
                FileUrl = fileUrl,
                Type = fileMessage.FormFile.ContentType,
            };

            var message = new Message
            {
                FromUserId = userId,
                ChatId = chatId,
                ContentId = newContent.Id,
                FromUsername = user.Username,
                Content = newContent
            };

            await _unitOfWork.MessageRepository.AddMessage(message);
            return message.ParseToDto();
        }


    }
}