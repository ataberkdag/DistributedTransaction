namespace Core.Application.Common
{
    public class PaginatedResult<T> : BaseResult
    {
        public int PageIndex { get; private set; }
        public int PageSize { get; private set; }
        public int TotalCount { get; private set; }
        public List<T> Result { get; private set; }

        protected PaginatedResult(int pageIndex, int pageSize, int totalCount, List<T> result, bool succeeded = true, string errorCode = "0000", string errorMessage = "") 
            : base(succeeded, errorCode, errorMessage)
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
            TotalCount = totalCount;
            Result = result;
        }

        public static PaginatedResult<T> Success(int pageIndex, int pageSize, int totalCount, List<T> result)
        {
            return new PaginatedResult<T>(pageIndex, pageSize, totalCount, result);
        }

        public static PaginatedResult<T> Failure(string errorCode, string errorMessage)
        {
            return new PaginatedResult<T>(0, 0, 0, null, false, errorCode, errorMessage);
        }
    }
}
