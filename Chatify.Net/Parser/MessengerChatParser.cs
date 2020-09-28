using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Chatify.Net.Models;

namespace Chatify.Net.Parser
{
    public class MessengerChatParser : IChatParser
    {

        private List<MessengerChat> Chats = new List<MessengerChat>();
        public string UserName { get; set; }

        public MessengerChatParser(string userName)
        {
            this.UserName = userName;
        }


        public List<Chat> Parse()
        {
            return Chats.ConvertAll<Chat>(ParseChat);
        }



        public async Task AddChatFile(Stream fileStream)
        {

            var streamReader = new StreamReader(fileStream);

            MessengerChat chat = await JsonSerializer.DeserializeAsync<MessengerChat>(fileStream);
            Chats.Add(chat);
        }

        private Chat ParseChat(MessengerChat old)
        {
            var chat = new Chat
            {
                Participants = old.Participants.ConvertAll(t => t.Name),
                Messages = old.Messages.Where(t=>t.IsValid).ToList().ConvertAll(t => t.ToChat(UserName))
            };


            return chat;
        }

        private class MessengerChat
        {
            [JsonPropertyName("participants")] public List<MessengerParticipants> Participants{ get; set; }
            [JsonPropertyName("messages")] public List<MessengerMessages> Messages { get; set; }
        }

        private class MessengerParticipants
        {
            [JsonPropertyName("name")] public string Name { get; set; }
        }

        private class MessengerMessages
        {
            [JsonPropertyName("sender_name")] public string Sender { get; set; }
            [JsonPropertyName("timestamp_ms")] public long Timestamp { get; set; }
            [JsonPropertyName("content")] public string Message { get; set; }

            [JsonIgnore] public bool IsValid => !string.IsNullOrEmpty(Message);
            public ChatMessage ToChat(string username)
            {
                return new ChatMessage
                {
                    Date = DateTimeOffset.FromUnixTimeMilliseconds(Timestamp).DateTime,
                    Text = Message,
                    Sender = Sender,
                    IsFromUser = username == Sender
                };
            }
        }
    }
}