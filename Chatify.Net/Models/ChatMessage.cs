using System;

namespace Chatify.Net.Models
{
    public class ChatMessage
    {
        public string Sender { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }

        public bool IsFromUser { get; set; }
    }
}