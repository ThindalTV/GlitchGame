using GlitchGame.Shared;
using GlitchGame.Web.Repositories;

namespace GlitchGame.Web.Services
{
    public record ChatMessage
    {
        public string Id { get; init; } = Guid.NewGuid().ToString();
        public Districts District { get; set; }
        public string User { get; init; } = string.Empty;
        public string Message { get; init; } = string.Empty;
        public DateTime Timestamp { get; init; } = DateTime.UtcNow;

        public bool Redacted { get; set; }
    }

    public class ChatService
    {

        public event Action<ChatMessage>? OnMessageReceived;
        public event Action<ChatMessage>? OnMessageChanged;

        private readonly PointsTracker _pointsTracker;
        private readonly MessageRepository _messageRepository;

        public ChatService(PointsTracker pointsTracker, MessageRepository messageRepository)
        {
            _pointsTracker = pointsTracker;
            _messageRepository = messageRepository;

            _messageRepository.LoadMessages();

        }

        public void SendMessage(ChatMessage message)
        {

            // Add points, just everything is a good chat for now
            _pointsTracker.RegisterUserAction(message.User, message.District, EventType.GoodMessage);

            _messageRepository.Messages.Add(message);
            _messageRepository.SaveMessages();

            // For now, just echo the message back to the client
            OnMessageReceived?.Invoke(message);
        }

        public void RedactMessage(string id)
        {
            var msg = _messageRepository.Messages.FirstOrDefault(m => m.Id == id);
            if(msg == null)
            {
                return;
            }
            msg.Redacted = true;

            // TODO: Remove points from the user for sending a bad message

            _messageRepository.SaveMessages();

            OnMessageChanged?.Invoke(msg);
        }
    }
}
