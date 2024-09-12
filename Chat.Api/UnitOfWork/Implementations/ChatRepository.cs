using Chat.Api.Context;
using Chat.Api.Exceptions;
using Chat.Api.UnitOfWork.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Chat.Api.UnitOfWork.Implementations
{
    public class ChatRepository : IChatRepository, IDisposable
    {
        private readonly AppDbContext _context;
        private bool _disposed = false;
        public ChatRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddChat(Entities.Chat chat)
        {
            await _context.Chats.AddAsync(chat);
            await _context.SaveChangesAsync();
        }

        public async Task<Tuple<bool, Entities.Chat>> CheckChatExist(Guid fromUserid, Guid toUserId)
        {
            var userChat = await _context.User_Chats
                .FirstOrDefaultAsync(uc => uc.UserId == fromUserid
                && uc.ToUserId == toUserId);
            if (userChat != null)
            {
                var chat = await GetUserChatById(userChat.UserId, userChat.ChatId);
                return new(true, chat);
            }

            return new(false, null);
        }

        public async Task DeleteChat(Entities.Chat chat)
        {
            _context.Chats.Remove(chat);
            await _context.SaveChangesAsync();
        }
        private void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {

                    _context.Dispose();
                }
            }
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public async Task<List<Entities.Chat>> GetAllChats()
        {
            var chats = await _context.Chats.AsNoTracking().ToListAsync();
            return chats;
        }

        public async Task<List<Entities.Chat>> GetAllUserChats(Guid userId)
        {
            var userChats = await _context.User_Chats
                .Where(x => x.UserId == userId)
                .ToListAsync();

            var sortedChats = new List<Entities.Chat>();
            var check = userChats.Count == 0 || userChats is null;
            if (check)
                return sortedChats;

            foreach (var userChat in userChats)
            {   
                var sortedChat = await _context.Chats
                    .SingleAsync(x => x.Id == userChat.ChatId);
                sortedChats.Add(sortedChat);
            }
            return sortedChats;
        }

        public async Task<Entities.Chat> GetUserChatById(Guid userId, Guid chatId)
        {
            var userChat = await _context.User_Chats
                 .FirstOrDefaultAsync(uc => uc.UserId == userId && uc.ChatId == chatId);

            if (userChat == null)   
                throw new ChatNotFound();

            var chat = await _context.Chats
                .SingleAsync(ch => ch.Id == userChat.ChatId);
            return chat;
        }

        public async Task UpdateChat(Entities.Chat chat)
        {
           _context.Chats.Remove(chat);
            await _context.SaveChangesAsync();
        }
    }
}