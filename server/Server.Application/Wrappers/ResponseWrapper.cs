namespace Server.Application.Wrappers;

public class ResponseWrapper : IResponseWrapper
{
    public List<string> Messages { get; set; } = new();
    public bool IsSuccessfull { get; set; }

    public static IResponseWrapper Fail()
        => new ResponseWrapper { IsSuccessfull = false };

    public static IResponseWrapper Fail(string message)
        => new ResponseWrapper { IsSuccessfull = false, Messages = new List<string> { message } };

    public static IResponseWrapper Fail(List<string> messages)
        => new ResponseWrapper { IsSuccessfull = false, Messages = messages };

    public static Task<IResponseWrapper> FailAsync()
        => Task.FromResult(Fail());

    public static Task<IResponseWrapper> FailAsync(string message)
        => Task.FromResult(Fail(message));

    public static Task<IResponseWrapper> FailAsync(List<string> messages)
      => Task.FromResult(Fail(messages));

    public static IResponseWrapper Success()
      => new ResponseWrapper { IsSuccessfull = true };

    public static IResponseWrapper Success(string message)
      => new ResponseWrapper { IsSuccessfull = true, Messages = new List<string> { message } };

    public static Task<IResponseWrapper> SuccessAsync()
      => Task.FromResult(Success());

    public static Task<IResponseWrapper> SuccessAsync(string message)
      => Task.FromResult(Success(message));
}

public class ResponseWrapper<T> : ResponseWrapper, IResponseWrapper<T>
{
    public List<string> Messages { get; set; } = new();
    public bool IsSuccessfull { get; set; }
    public T ResponseData { get; set; }

    public static IResponseWrapper<T> Fail()
        => new ResponseWrapper<T> { IsSuccessfull = false };

    public static IResponseWrapper<T> Fail(string message)
        => new ResponseWrapper<T> { IsSuccessfull = false, Messages = new List<string> { message } };

    public static IResponseWrapper<T> Fail(List<string> messages)
        => new ResponseWrapper<T> { IsSuccessfull = false, Messages = messages };

    public static Task<IResponseWrapper<T>> FailAsync()
        => Task.FromResult(Fail());

    public static Task<IResponseWrapper<T>> FailAsync(string message)
        => Task.FromResult(Fail(message));

    public static Task<IResponseWrapper<T>> FailAsync(List<string> messages)
      => Task.FromResult(Fail(messages));

    public static IResponseWrapper<T> Success()
      => new ResponseWrapper<T> { IsSuccessfull = true };

    public static IResponseWrapper<T> Success(T data)
      => new ResponseWrapper<T> { IsSuccessfull = true, ResponseData = data };
    
    public static IResponseWrapper<T> Success(string message)
      => new ResponseWrapper<T> { IsSuccessfull = true, Messages = new List<string> { message } };

    public static IResponseWrapper<T> Success(T data, string message)
      => new ResponseWrapper<T> { IsSuccessfull = true, Messages = new List<string> { message }, ResponseData = data };

    public static IResponseWrapper<T> Success(T data, List<string> messages)
      => new ResponseWrapper<T> { IsSuccessfull = true, Messages = messages, ResponseData = data };

    public static Task<IResponseWrapper<T>> SuccessAsync()
      => Task.FromResult(Success());

    public static Task<IResponseWrapper<T>> SuccessAsync(T data)
      => Task.FromResult(Success(data));

    public static Task<IResponseWrapper<T>> SuccessAsync(string message)
      => Task.FromResult(Success(message));

    public static Task<IResponseWrapper<T>> SuccessAsync(T data, string message)
      => Task.FromResult(Success(data, message));
}