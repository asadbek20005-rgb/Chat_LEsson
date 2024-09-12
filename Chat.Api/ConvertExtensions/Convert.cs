using Chat.Api.Dtos;
using Chat.Api.Entities;
using Mapster;

namespace Chat.Api.ConvertExtensions
{
    public static class Convert
    {
        public static UserDto ParseToDto(this User user)
        {
            UserDto userDto = user.Adapt<UserDto>();
            return userDto;
        }
        public static List<UserDto> ParseToDtos(this List<User> users)
        {
            if (users.Count == 0 || users.Count == null) return new List<UserDto>();
            var userDtos = new List<UserDto>();
            users.ForEach(user => userDtos.Add(ParseToDto(user)));
            return userDtos;
        }


        public static ChatDto ParseToDto(this Entities.Chat chat)
        {
            ChatDto chatDto = chat.Adapt<ChatDto>();
            return chatDto;
        }

        public static List<ChatDto> ParseToDtos(this List<Entities.Chat> chats)
        {
            if(chats.Count == 0 || chats.Count == null) return new List<ChatDto>();
            var chatDtos = new List<ChatDto>();
            chats.ForEach(chat => chatDtos.Add(chat.ParseToDto()));
            return chatDtos;
        }


        public static MessageDto ParseToDto(this Message message)
        {
            MessageDto messageDto = message.Adapt<MessageDto>();
            return messageDto;
        }

        public static List<MessageDto> ParseToDtos(this List<Message> messages)
        {
            if(messages.Count <= 0 || messages.Count == null) return new List<MessageDto>();
            var messagesDtos = new List<MessageDto>();
            messages.ForEach(m => messagesDtos.Add(m.ParseToDto()));
            return messagesDtos;
        }

    }
}
