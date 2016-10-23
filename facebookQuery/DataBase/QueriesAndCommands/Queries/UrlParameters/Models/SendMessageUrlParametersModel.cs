namespace DataBase.QueriesAndCommands.Queries.UrlParameters.Models
{
    public class SendMessageUrlParametersModel : GeneralUrlParametersModel
    {
        private string _client;
        private string _actionType;
        private string _body;
        private string _ephemeralTtlMode;
        private string _hasAttachment;
        private string _messageId;
        private string _offlineThreadingId;
        private string _otherUserFbid;
        private string _source;
        private string _signatureId;
        private string _specificToListOne;
        private string _specificToListTwo;
        private string _timestamp;
        private string _uiPushPhase;

        public string Client
        {
            get { return "client=" + _client; }
            set { _client = value; }
        }

        public string ActionType
        {
            get { return "action_type=" + _actionType; } 
            set { _actionType = value; }
        }

        public string Body
        {
            get { return "body=" + _body; } 
            set { _body = value; }
        }

        public string EphemeralTtlMode
        {
            get { return "ephemeral_ttl_mode=" + _ephemeralTtlMode; } 
            set { _ephemeralTtlMode = value; }
        }

        public string HasAttachment
        {
            get { return "has_attachment=" + _hasAttachment; } 
            set { _hasAttachment = value; }
        }

        public string MessageId
        {
            get { return "message_id=" + _messageId; } 
            set {  _messageId = value; }
        }

        public string OfflineThreadingId
        {
            get { return "offline_threading_id=" + _offlineThreadingId; }
            set { _offlineThreadingId = value; } 
        }

        public string OtherUserFbid
        {
            get { return "other_user_fbid=" + _otherUserFbid; }
            set { _otherUserFbid = value; }
        }

        public string Source
        {
            get { return "source=" + _source; }
            set { _source = value; }
        }

        public string SignatureId
        {
            get { return "signature_id=" + _signatureId; }
            set { _signatureId = value; }
        }

        public string SpecificToListOne
        {
            get { return "specific_to_list[0]=" + _specificToListOne; }
            set { _specificToListOne = value; }
        }

        public string SpecificToListTwo
        {
            get { return "specific_to_list[1]=" + _specificToListTwo; }
            set { _specificToListTwo = value; }
        }

        public string Timestamp
        {
            get { return "timestamp=" + _timestamp; }
            set { _timestamp = value; }
        }

        public string UiPushPhase
        {
            get { return "ui_push_phase=" + _uiPushPhase; }
            set { _uiPushPhase = value; }
        }
    }
}
