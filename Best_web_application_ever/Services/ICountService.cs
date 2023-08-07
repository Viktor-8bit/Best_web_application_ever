namespace Best_web_application_ever.Services
{
    public interface ICountService
    {
        int Count { get; set; }
        Task Dowork();
    }
}
