using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;


namespace Chatify.Net.Models
{
    public class Chat
    {
        public List<string> Participants { get; set; } = new List<string>();
        public List<ChatMessage> Messages  { get; set; } = new List<ChatMessage>();


        public  string ExportToJson()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}