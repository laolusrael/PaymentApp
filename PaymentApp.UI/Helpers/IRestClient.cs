using System.Threading.Tasks;

namespace PaymentApp.UI.Helpers
{
    public interface IRestClient
    {
        Task<TResponse> Delete<TResponse>(string endPoint, object id);
        Task<T> Get<T>(string endPoint);
        Task<TResponse> Post<TResponse, TRequest>(string endPoint, TRequest data);
        Task<TResponse> Put<TResponse, TRequest>(string endPoint, TRequest data);
        Task<TResponse> Delete<TResponse>(string endPoint);
    }

}
