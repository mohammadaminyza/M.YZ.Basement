namespace M.YZ.Basement.EndPoints.Web.Middlewares.ApiExceptionHandler;

public static class ExceptionHelper
{
    public static string GetInnermostExceptionMessage(this Exception exception)
    {
        if (exception.InnerException != null)
            return GetInnermostExceptionMessage(exception.InnerException);

        return exception.Message;
    }
}