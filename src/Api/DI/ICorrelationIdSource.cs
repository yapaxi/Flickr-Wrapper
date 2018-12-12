namespace FlickrWrapper.Api.DI
{
    public interface ICorrelationIdSource
    {
        string GetCurrentCorrelationId();
    }
}