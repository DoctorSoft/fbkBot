namespace DataBase.QueriesAndCommands.Commands.Notices.AddNotice
{
    public class AddNoticeCommand : ICommand<VoidCommandResponse>
    {
        public long AccountId { get; set; }

        public string NoticeText { get; set; }
    }
}
