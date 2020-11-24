using System.Net;

namespace WebApplication.Exception
{
    public class BaseException : System.Exception
    {
        public BaseException(HttpStatusCode httpStatusCode)
        {
            StatusCode = httpStatusCode;
        }

        public HttpStatusCode StatusCode;
    }
}