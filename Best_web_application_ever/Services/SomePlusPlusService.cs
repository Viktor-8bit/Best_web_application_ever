namespace Best_web_application_ever.Services
{
    public class SomePlusPlusService : ICountService
    {
        public int Count { get; set; } = 0;
        public Task Dowork()
        {
            this.Count++;
            return Task.CompletedTask;
        }
    }
}
