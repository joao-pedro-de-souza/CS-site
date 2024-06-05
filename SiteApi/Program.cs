using Microsoft.AspNetCore.Http.Features;
using post;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.MapGet("/SessionInit", (HttpContext context) =>
{
    var Auth=new[] {"0000"};
    string headerKey = "Auth";
    if (context.Request.Headers.TryGetValue(headerKey, out var headerContent))
    {

        if (Auth.Contains(headerContent.ToString()))
        {
            DateTime today= DateTime.Now;
            var sessionToken =today.ToString();
            return sessionToken+headerContent;
        }
        else
        {
            return $"{headerContent} Not a valid Token.";
        }
    }
    else
    {
        return "Header not found.";
    }
});
app.MapGet("/posts", (HttpContext context) =>
{
    if (context.Request.Headers.TryGetValue("id", out var headerContent))
    {
        headerContent= headerContent.ToString();
        if (int.TryParse(headerContent, out int n))
        {
            int.TryParse(headerContent, out int id);
            return $"id: {id} recieved";

        }
        return $"id: NaN";
    }
    else
    {
        return "ID not found.";
    }
});

app.MapPost("/posts", static async (HttpContext context) =>
{
    List<(string, int)> posts = new List<(string, int)>();
    using (StreamReader reader = new StreamReader(context.Request.Body))
    {
        try
        {
            string requestBody = await reader.ReadToEndAsync();
            Post item=new Post(requestBody);
            int id=item.ID;
            string Message=item.Message;
            var myTuple=(id,Message);
            Console.WriteLine($"{myTuple.id} : {myTuple.Message}");
            return myTuple.ToString();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            return "An error occurred while processing the request.";
        }
    }
});

app.Run();
