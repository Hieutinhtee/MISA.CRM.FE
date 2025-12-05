using MISA.CRM.CORE.Exceptions;

public class ValidateExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public ValidateExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ValidateException ex)
        {
            await HandleExceptionAsync(context, 400, "Dữ liệu không hợp lệ", ex);
        }
        catch (NotFoundException ex)
        {
            await HandleExceptionAsync(context, 404, "Không tìm thấy dữ liệu", ex);
        }
        catch (DuplicateException ex)
        {
            await HandleExceptionAsync(context, 409, "Dữ liệu bị trùng", ex);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, 500, "Lỗi hệ thống", ex);
        }
    }

    private async Task HandleExceptionAsync(
        HttpContext context,
        int statusCode,
        string defaultUserMsg,
        Exception ex)
    {
        context.Response.Clear();
        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/json";

        var baseEx = ex as BaseException;

        var response = new
        {
            data = (object?)null,
            meta = (object?)null,
            error = new
            {
                userMsg = baseEx?.UserMsg ?? defaultUserMsg,
                devMsg = ex.Message,
                moreInfo = baseEx?.MoreInfo,
                traceId = context.TraceIdentifier
            }
        };

        await context.Response.WriteAsJsonAsync(response);
    }
}