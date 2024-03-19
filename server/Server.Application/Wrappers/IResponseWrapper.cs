namespace Server.Application.Wrappers;

public interface IResponseWrapper
{
    List<string> Messages { get; set; }
    bool IsSuccessfull { get; set; }
}

public interface IResponseWrapper<T> : IResponseWrapper
{
    T ResponseData { get; set; }
}