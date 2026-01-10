namespace Mercora.Application.Common.Exceptions
{
    public class BusinessRuleException : Exception
    {
        public int ErrorCode { get; }

        public BusinessRuleException(int errorCode, string message) : base(message)
        {
            ErrorCode = errorCode;
        }
    }
}
