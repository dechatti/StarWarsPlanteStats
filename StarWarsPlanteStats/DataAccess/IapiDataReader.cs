public interface IapiDataReader
{
    Task<string> Read(string baseAddress, string requestUri);
}
