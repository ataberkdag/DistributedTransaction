namespace Core.Application.Exceptions
{
    public class ExceptionError
    {
        public string MethodName { get; private set; }
        public string Message { get; private set; }
        public string InnerException { get; private set; }
        public string StackTrace { get; private set; }

        private ExceptionError(string methodName, string message, string innerException, string stackTrace)
        {
            MethodName= methodName;
            Message= message;
            InnerException= innerException;
            StackTrace=stackTrace;
        }

        public static ExceptionError Create(string methodName, string message, string innerException, string stackTrace)
        {
            return new ExceptionError(methodName, message, innerException, stackTrace);
        }
    }
}
