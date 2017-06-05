namespace Services.Interfaces.Notices
{
    public interface INotices
    {
        string ConvertNoticeText(string functionName, string noticeText);

        void AddNotice(long accountId, string noticeText);
    }
}
