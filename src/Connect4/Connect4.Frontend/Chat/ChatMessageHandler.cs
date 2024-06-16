using System.Collections.Concurrent;
using Game.Contract;
using MediatR;

namespace Connect4.Frontend.Chat
{
    public record ChatMemberId(Guid Id)
    {
        public ChatMemberId() : this(Guid.NewGuid())
        {
        }
    }

    internal record ChatMember(GameId GameId, Func<ChatMessage, Task> MessageReciviedCallbackAsync);

    internal interface IChat
    {
        ChatMemberId Join(GameId gameId, Func<ChatMessage, Task> messageReciviedCallbackAsync);
        void Leave(ChatMemberId chatMemberId);
        Task PublishMessageAsync(ChatMessage chatMessage);
    }

    internal class Chat : IChat
    {
        private ConcurrentDictionary<ChatMemberId, ChatMember> ChatMembers { get; } = new();

        public ChatMemberId Join(GameId gameId, Func<ChatMessage, Task> messageReciviedCallbackAsync)
        {
            var chatMemberId = new ChatMemberId();
            this.ChatMembers.TryAdd(chatMemberId, new ChatMember(gameId, messageReciviedCallbackAsync));

            return chatMemberId;
        }

        public void Leave(ChatMemberId chatMemberId)
        {
            this.ChatMembers.Remove(chatMemberId, out _);
        }

        public async Task PublishMessageAsync(ChatMessage chatMessage)
        {
            foreach (var chatMember in this.ChatMembers.Values.Where(m => m.GameId == chatMessage.GameId))
            {
                await chatMember.MessageReciviedCallbackAsync(chatMessage);
            }
        }
    }

    internal record ChatMessage(GameId GameId, string Author, string Message);
}
