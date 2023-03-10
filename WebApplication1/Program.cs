using System.Text;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.Use(async (context, next) =>
{
    Console.WriteLine(await context.Request.GetDetails());
    await next.Invoke();
});
app.Run();

public static class HttpRequestExtensions
{
    public static async Task<string> GetDetails(this HttpRequest request)
    {
        var baseUrl = $"{request.Scheme}://{request.Host}{request.Path}{request.QueryString.Value}";
        var sbHeaders = new StringBuilder();
        foreach (var header in request.Headers)
            sbHeaders.Append($"{header.Key}: {header.Value}\n");

        using var sr = new StreamReader(request.Body);
        var body =  await sr.ReadToEndAsync();
        
        body = body is null or "" ? "no-body" : body;

        return $"{request.Protocol} {request.Method} {baseUrl}\n\n{sbHeaders}\n{body}";
    }
}