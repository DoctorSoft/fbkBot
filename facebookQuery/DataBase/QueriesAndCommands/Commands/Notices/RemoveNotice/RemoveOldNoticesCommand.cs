namespace DataBase.QueriesAndCommands.Commands.Notices.RemoveNotice
{
    public class RemoveOldNoticesCommand : ICommand<VoidCommandResponse>
    {
        public int Skip { get; set; }
    }
}
