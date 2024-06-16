using System.Runtime.CompilerServices;
using Game.Contract;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Identity.Web;
using Microsoft.JSInterop;

namespace Connect4.Frontend.Chat
{
    public partial class ChatComponent : IDisposable
    {
        [Inject]
        private IChat Chat { get; set; } = null!;

        [Inject]
        public AuthenticationStateProvider AuthenticationStateProvider { get; set; } = null!;

        [Inject]
        private IJSRuntime JsRuntime { get; set; } = null!;

        [Parameter]
        public required GameId GameId { get; init; }

        private List<MessageBlock> Messages { get; } = new();

        private int NewMessagesCount { get; set; }

        private bool IsOpen { get; set; }

        private ChatMemberId? ChatMemberId { get; set; }

        public string? CurrentMessage { get; set; }

        public string Author { get; set; } = "Anonym";

        protected override async Task OnInitializedAsync()
        {
            var authenticationState = await this.AuthenticationStateProvider.GetAuthenticationStateAsync();
            this.Author = authenticationState.User.GetDisplayName() ?? "Anonym";
            this.ChatMemberId = this.Chat.Join(this.GameId, this.MessageReciviedCallbackAsync);
            await base.OnInitializedAsync();
        }

        private async Task MessageReciviedCallbackAsync(ChatMessage newMessage)
        {
            if (this.Messages.LastOrDefault()?.Author == newMessage.Author)
            {
                this.Messages.Last().Messages.Add(newMessage);
            }
            else
            {
                this.Messages.Add(new MessageBlock
                {
                    Author = newMessage.Author,
                    Messages = [newMessage]
                });
            }

            if (!this.IsOpen)
                this.NewMessagesCount++;

            await this.ScrollToElementId("end");
            await this.InvokeAsync(this.StateHasChanged);
        }

        private void ToogleChat()
        {
            if (!this.IsOpen)
                this.NewMessagesCount = 0;

            this.IsOpen = !this.IsOpen;
        }

        public void Dispose()
        {
            if (this.ChatMemberId != null)
                this.Chat.Leave(this.ChatMemberId);
        }


        private async Task SendMessage()
        {
            if (this.CurrentMessage == null)
                return;

            await this.Chat.PublishMessageAsync(new ChatMessage(this.GameId, this.Author, this.CurrentMessage));
            this.CurrentMessage = null;
        }

        private async Task OnMessageKeyDown(KeyboardEventArgs e)
        {
            if (e.Code is "Enter" or "NumpadEnter" && e is { CtrlKey: false, ShiftKey: false })
            {
                await Task.Delay(100);
                await this.SendMessage();
                await this.InvokeAsync(this.StateHasChanged);
            }
        }

        private async Task<bool> ScrollToElementId(string elementId)
        {
            return await this.JsRuntime.InvokeAsync<bool>("scrollToElementId", elementId);
        }
    }

    internal record MessageBlock
    {
        public required string Author { get; set; }

        public required List<ChatMessage> Messages { get; init; }
    }
}