namespace Services.Services
{
    public class NoticeService
    {
        public string ConvertNoticeText(string functionName, string noticeText)
        {
            return string.Format("[{0}] {1}", functionName, noticeText);
        }
    }
}
