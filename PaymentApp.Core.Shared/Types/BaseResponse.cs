namespace PaymentApp.Core.Shared.Types
{
    public abstract class BaseResponse<T>
    {
        public string Version = "v1";
        public int Code;
        public string Message;
        public T Result;

    }
}
