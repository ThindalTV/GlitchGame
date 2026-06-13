using GlitchGame.Shared;
using GlitchGame.Web.Services;

namespace GlitchGame.Web.Repositories
{
    public class MessageRepository
    {
        private const string messageFile = "messages.json";
        public List<ChatMessage> Messages { get; private set; } = new List<ChatMessage>();

        public void LoadMessages()
        {
            if (File.Exists(messageFile))
            {
                var fileContent = File.ReadAllText(messageFile);

                Messages = System.Text.Json.JsonSerializer.Deserialize<List<ChatMessage>>(fileContent) ?? new List<ChatMessage>();
            }
        }

        public void SaveMessages()
        {
            var content = System.Text.Json.JsonSerializer.Serialize(Messages);

            File.WriteAllText(messageFile, content);
        }


    }
}
