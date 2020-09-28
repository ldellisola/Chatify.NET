using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Chatify.Net.Models;

namespace Chatify.Net.Parser
{
    public interface IChatParser
    {
        List<Chat> Parse();

        Task AddChatFile(Stream fileStream);

    }
}