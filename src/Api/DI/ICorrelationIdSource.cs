namespace FlickerWrapper.Api.DI
{
    public interface ICorrelationIdSource
    {
        string GetCurrentCorrelationId();
    }
}