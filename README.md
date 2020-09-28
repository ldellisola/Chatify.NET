# Chatify.NET

Chatify.Net is a .net standard library to import and parse all your exported chats from different social media platforms such as WhatsApp, Facebook Messenger and more!

## Todo

- [x] Support WhatsApp
- [x] Support Facebook Messenger
- [ ] Support Discord
- [ ] Support Telegram

## Usage

```c#
// Intantiate the parser.
// Your name must match your user name on the platform
IChatParser parser = new WhatsAppChatParser("Your Name");

// Add any amount of chats to the parser
await parser.AddChatFile(@"path/to/file");
await parser.AddChatFile(@"path/to/file");
await parser.AddChatFile(@"path/to/file");
await parser.AddChatFile(@"path/to/file");

// Obtain your chats parsed
List<Chat> chats = parser.Parse();

// You can also export your chat to Json

string json = chats.First().ExportToJson();
```

