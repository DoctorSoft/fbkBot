namespace DataBase.QueriesAndCommands.Queries.UrlParameters.Models
{
    public class SendMessageUrlParametersModel : GeneralUrlParametersModel
    {
        public string Client { get; set; }

        public string ActionType { get; set; }

        public string Body { get; set; }

        public string EphemeralTtlMode { get; set; }

        public string HasAttachment { get; set; }

        public string MessageId { get; set; }

        public string OfflineThreadingId { get; set; }

        public string OtherUserFbid { get; set; }

        public string Source { get; set; }

        public string SignatureId { get; set; }

        public string SpecificToListOne { get; set; }

        public string SpecificToListTwo{ get; set; }

        public string Timestamp{ get; set; }

        public string UiPushPhase{ get; set; }
    }
}
