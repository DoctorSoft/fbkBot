namespace Services.Interfaces.Notices
{
    public interface INotices
    {
        string ConvertNoticeText(string functionName, string noticeText);


        void AddNotice(string functionName, long accountId, string noticeText);
    }
}
