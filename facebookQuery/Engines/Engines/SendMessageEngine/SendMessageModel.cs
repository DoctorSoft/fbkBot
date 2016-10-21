namespace Engines.Engines.SendMessageEngine
{
    public class SendMessageModel
    {
        public long AccountId { get; set; }

        public long InterlocutorId { get; set; }

        public string Message { get; set; }
    }
}
