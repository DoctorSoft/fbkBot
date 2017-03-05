namespace Services.Interfaces.Notices
{
    public interface INoticesProxy
    {
        void AddNotice(dynamic accountId, dynamic message);
    }
}
