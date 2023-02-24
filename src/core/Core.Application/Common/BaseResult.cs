namespace Core.Application.Common
{
    public class BaseResult
    {
        public bool Succeeded { get; private set; }
        public string ErrorCode { get; private set; }
        public string ErrorMessage { get; private set; }

        protected BaseResult(bool succeeded = true, string errorCode = "0000", string errorMessage = "")
        {
            Succeeded= succeeded;
            ErrorCode = errorCode;
            ErrorMessage = errorMessage;
        }

        public static BaseResult Success()
        {
            return new BaseResult();
        }

        public static BaseResult Failure(string errorCode, string errorMessage)
        {
            return new BaseResult(false, errorCode, errorMessage);
        }
    }

    public class BaseResult<T> : BaseResult
    {
        public T Data { get; private set; }

        protected BaseResult(T data, bool succeeded = true, string errorCode = "0000", string errorMessage = "") : base(succeeded, errorCode, errorMessage)
        {
            Data = data;
        }

        public static BaseResult<T> Success(T data)
        {
            return new BaseResult<T>(data);
        }

        public static BaseResult<T> Failure(string errorCode, string errorMessage, T data = default(T))
        {
            return new BaseResult<T>(data, false, errorCode, errorMessage);
        }
    }
}
