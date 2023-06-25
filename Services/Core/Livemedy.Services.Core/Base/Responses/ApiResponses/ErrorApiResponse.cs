namespace Livemedy.Core.Base.Responses.ApiResponses;

public class ErrorApiResponse<T> : ApiResponse<T>
{
    public ErrorApiResponse(T data = default) : base(success: false)
    {
        Data = data;
    }

    public ErrorApiResponse(string message) : base(success: false, message: message)
    {
    }
}

/// <summary>
/// You can use it when method worked wrong.
/// </summary>
public class ErrorApiResponse : ApiResponse
{
    public ErrorApiResponse() : base(success: false)
    {
    }

    public ErrorApiResponse(string message) : base(success: false, message: message)
    {
    }
}
