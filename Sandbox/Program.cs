using System;
using System.IO;
using System.Threading.Tasks;
using Chatify.Net.Parser;

namespace Sandbox
{
    class Program
    {
        static async Task Main(string[] args)
        {
            
            IChatParser chatParser = new MessengerChatParser("Lucas Dell'Isola");

            await chatParser.AddChatFile(File.OpenRead(@"C:\Users\luckd\Desktop\Lucia.txt"));

            var chats = chatParser.Parse();

        }
    }
}
