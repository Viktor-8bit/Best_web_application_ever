namespace Best_web_application_ever.Services
{
    public class LongTimeService : ITimeService
    {
        public string GetTime() => DateTime.Now.ToShortTimeString();
    }
}
