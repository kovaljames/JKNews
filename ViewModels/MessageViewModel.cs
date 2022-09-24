using Newtonsoft.Json;

namespace JKNews.ViewModels
{
    public enum MessageType
    {
        Info,
        Error
    }

    public class MessageViewModel
    {
        public MessageType Type { get; set; }
        public string Text { get; set; }
        public MessageViewModel(string message, MessageType type = MessageType.Info)
        {
            this.Type = type;
            this.Text = message;
        }

        public static string Serialize(string message, MessageType tipo = MessageType.Info)
        {
            var messageModel = new MessageViewModel(message, tipo);
            return JsonConvert.SerializeObject(messageModel);
        }

        public static MessageViewModel Deserialize(string messageString)
        {
            return JsonConvert.DeserializeObject<MessageViewModel>(messageString);
        }
    }
}