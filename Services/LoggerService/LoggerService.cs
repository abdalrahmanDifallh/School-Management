using Core.Enums;


namespace Services.LoggerService
{
    public class LoggerService<T> : ILoggerService<T> where T : class
    {
        public async Task LogErrorAsync(string source, Exception ex, LogImportance logImportance = LogImportance.Low)
        {
            Console.WriteLine(String.Format("Error In [{0}.{1}] \r\n {2}", typeof(T).Name, source, ex.Message));
            await Task.Delay(5);
        }
    }
}
