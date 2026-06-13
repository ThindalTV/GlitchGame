using Microsoft.Extensions.AI;
using System.Text.Json;

namespace GlitchGame.Web.Services;

public record MessageInfo
{
    public int SentimentScore { get; set; }
    public string Reason { get; set; }

    public MessageAnalysisResult Result { get; set; }
}

public enum MessageAnalysisResult
{
    Positive,
    Negative,
    Neutral,
    Offensive,
    Toxic
}

public class MessageAnalysisService
{
    public async Task<MessageInfo> AnalyseMessage(ChatMessage msg)
    {
        string apiKey = String.Empty;
        string modelName = String.Empty;

        OpenAI.OpenAIClient client = new OpenAI.OpenAIClient(apiKey);

        var chat = client.GetChatClient(modelName).AsIChatClient();

        var messageInfoSchema = """
{
    "type": "object",
    "properties": {
        "SentimentScore": { "type": "integer" },
        "Reason": { "type": "string" },
        "Result": {
            "type": "string",
            "enum": ["Positive", "Negative", "Neutral", "Offensive", "Toxic"]
        }
    },
    "required": ["SentimentScore", "Reason", "Result"]
}
""";

        var promptMessage = new Microsoft.Extensions.AI.ChatMessage(
            ChatRole.System,
            $"""
You are a message analysis service.
Analyze the following message and provide a sentiment score, reason, and result.
""");



        var userMessage = new Microsoft.Extensions.AI.ChatMessage(
            ChatRole.User,
            msg.Message);

        var resp = await chat.GetResponseAsync([promptMessage, userMessage]
        , new ChatOptions()
        {
            ResponseFormat = ChatResponseFormat.ForJsonSchema<MessageInfo>()
        });

        var messageInfo = JsonSerializer.Deserialize<MessageInfo>(resp.Text);

        return messageInfo;
    }
}