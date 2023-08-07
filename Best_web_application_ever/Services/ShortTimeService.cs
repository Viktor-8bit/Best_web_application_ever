namespace Best_web_application_ever.Services
{
    public class ShortTimeService : ITimeService
    {
        public string GetTime() => DateTime.Now.ToLongTimeString();
    }
}
