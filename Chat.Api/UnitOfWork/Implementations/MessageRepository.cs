using Chat.Api.Context;
using Chat.Api.Entities;
using Chat.Api.UnitOfWork.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Chat.Api.UnitOfWork.Implementations
{
    public class MessageRepository : IMessageRepository, IDisposable
    {
        private readonly AppDbContext _context;
        private bool _disposed = false;
        public MessageRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddMessage(Message message)
        {
            await _context.Messages.AddAsync(message);
            await _context.SaveChangesAsync();
        }
        public void Dispose(bool disposing)
        {
            if(!disposing)
            {
                if(disposing)
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

        public async Task<List<Message>> GetAllChatMessages(Guid chatId)
        {
            var chatMessages = await _context.Messages
                .Where(x =>x.ChatId == chatId)
                .ToListAsync();
            return chatMessages;
        }

        public async Task<List<Message>> GetAllMessages()
        {
            return await _context.Messages.ToListAsync();
        }

        public async Task<Message> GetChatMessageById(Guid chatId, int messageId)
        {
            var chatMessage = await _context.Messages
                .SingleOrDefaultAsync(x => x.ChatId == chatId && x.Id == messageId);
            return chatMessage;
        }

        public async Task<Message> GetMessageById(int messageId)
        {
            var message = await _context.Messages.FindAsync(messageId);    
            return message;
        }
    }
}
