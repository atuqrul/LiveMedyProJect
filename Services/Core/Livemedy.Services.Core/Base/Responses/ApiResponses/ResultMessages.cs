namespace Livemedy.Core.Responses.ApiResponses;

public abstract class ResultMessages
{
    public static string Success => "messages.success.general.ok";
    public static string InvalidModel => "messages.error.general.invalidModel";
    public static string UnhandledException => "messages.error.general.unhandledException";
    public static string UnAuthorized => "messages.error.general.unAuthorized";

    public static string InternalServerError => "messages.error.general.internalServerError";

}
