using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Chatify.Net.Models;
using Chatify.Net.Models.Exceptions;

namespace Chatify.Net.Parser
{
    public class WhatsAppChatParser : IChatParser
    {
        private const string pattern = @"^\[(\d{1,2}\/\d{1,2}\/\d{1,2}, \d{1,2}:\d{1,2}:\d{1,2})\] (.+):(.+)$";
        public WhatsAppChatParser(string userName, bool excludeMedia = false)
        {
            this.UserName = userName;
            this.ExcludeMedia = excludeMedia;
        }

        private bool ExcludeMedia { get; set; }

        public string UserName { get; }
        private List<string> unformattedChats = new List<string>();



        public List<Chat> Parse()
        {
            return unformattedChats.ConvertAll<Chat>(ParseChat);
        }

        public async Task AddChatFile(Stream fileStream)
        {

            fileStream.Seek(0, SeekOrigin.Begin);

            var streamReader = new StreamReader(fileStream);

            string chat = await streamReader.ReadToEndAsync();

            unformattedChats.Add(chat);

            streamReader.Close();

        }


        private Chat ParseChat(string unformattedChat)
        {
            var chat = new Chat();

            using (var reader = new StringReader(unformattedChat))
            {
                string line;

                while ((line = reader.ReadLine()) != null)
                {
                    var match = Regex.Match(line, pattern);

                    if (!match.Success)
                    {
                        chat.Messages.Last().Text += @"\n " + line;
                        continue;
                    }

                    var message = new ChatMessage
                    {
                        Date = DateTime.ParseExact(match.Groups[1].Value, "M/d/y, HH:mm:ss",
                            CultureInfo.InvariantCulture),
                        Sender = match.Groups[2].Value,
                        Text = match.Groups[3].Value,
                        IsFromUser = match.Groups[2].Value == UserName
                    };

                    if(!ExcludeMedia || !Regex.IsMatch(message.Text,@"^(audio|image|video|GIF|sticker) omitted$"))
                        chat.Messages.Add(message);

                }
            }

            chat.Participants = chat.Messages.ConvertAll(t => t.Sender).Distinct().ToList();


            return chat;
        }

        
    }
}