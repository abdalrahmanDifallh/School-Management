

namespace Services.Extensions
{
    public static class MyExtensions
    {
        public static string GetError(this Exception ex)
        {
            if (ex.InnerException != null)
                return ex.InnerException.Message;

            return ex.Message;
        }


    }
}
