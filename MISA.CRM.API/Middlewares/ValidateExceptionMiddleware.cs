using MISA.CRM.CORE.Exceptions;
using System.Collections;
using System.Diagnostics;
using System.Text.Json;

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
            await HandleExceptionAsync(context, 500, "Có lỗi xảy ra, vui lòng thử lại sau ít phút", ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, int statusCode, string defaultUserMsg, Exception ex)
    {
        context.Response.Clear();
        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/json";

        var baseEx = ex as BaseException;

        // Lấy frame phù hợp từ stacktrace - cố gắng tìm frame đầu tiên có thông tin file/line
        var st = new StackTrace(ex, true);
        StackFrame? targetFrame = null;
        for (int i = 0; i < st.FrameCount; i++)
        {
            var f = st.GetFrame(i);
            if (f != null && !string.IsNullOrEmpty(f.GetFileName()))
            {
                targetFrame = f;
                break;
            }
        }
        // fallback: frame 0 nếu không có frame có file
        targetFrame ??= st.GetFrame(0);

        string? file = targetFrame?.GetFileName();
        int? line = targetFrame?.GetFileLineNumber();

        // Lấy inner exceptions đệ quy
        List<object> GetInnerExceptions(Exception e)
        {
            var list = new List<object>();
            var inner = e.InnerException;
            while (inner != null)
            {
                list.Add(new
                {
                    message = inner.Message,
                    type = inner.GetType().FullName,
                    stack = inner.StackTrace
                });
                inner = inner.InnerException;
            }
            return list;
        }

        // Lấy ex.Data (nếu có)
        IDictionary? dataDict = null;
        if (ex.Data != null && ex.Data.Count > 0)
        {
            dataDict = new Dictionary<string, object?>();
            foreach (DictionaryEntry entry in ex.Data)
            {
                try
                {
                    ((Dictionary<string, object?>)dataDict)[entry.Key?.ToString() ?? ""] = entry.Value;
                }
                catch
                {
                    // ignore any non-string keys
                }
            }
        }

        // Nếu BaseException có trường chứa chi tiết lỗi (ví dụ Errors hoặc Details), cố gắng lấy ra
        object? validationErrors = null;
        if (baseEx != null)
        {
            // common property names to try
            var candidateNames = new[] { "Errors", "Details", "ValidationErrors", "MoreInfo" };
            var beType = baseEx.GetType();
            foreach (var name in candidateNames)
            {
                var prop = beType.GetProperty(name);
                if (prop != null)
                {
                    var val = prop.GetValue(baseEx);
                    if (val != null)
                    {
                        validationErrors = val;
                        break;
                    }
                }
            }
        }

        // build dev message object
        var devMsg = new
        {
            message = ex.Message,
            type = ex.GetType().FullName,
            file = file ?? "unknown",
            line = line,
            stack = ex.StackTrace?.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries),
            inner = GetInnerExceptions(ex),
            data = dataDict,
            validationErrors = validationErrors,
            moreInfo = (baseEx?.MoreInfo)
        };

        var response = new
        {
            data = (object?)null,
            meta = (object?)null,
            error = new
            {
                userMsg = baseEx?.UserMsg ?? defaultUserMsg,
                devMsg,
                traceId = context.TraceIdentifier
            }
        };

        // Optionally: log full devMsg to server logs here as well
        // e.g. logger?.LogError("Exception: {0}", JsonSerializer.Serialize(devMsg));

        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true,
            DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.Never
        };


        await context.Response.WriteAsJsonAsync(response, options);
    }
}