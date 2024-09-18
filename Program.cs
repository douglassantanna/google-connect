using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
    x => x.EnableRetryOnFailure());
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/login", async (HttpContext context) =>
{
    string html = @"
    <!DOCTYPE html>
    <html lang='en'>
    <head>
        <meta charset='UTF-8'>
        <meta name='viewport' content='width=device-width, initial-scale=1.0'>
        <title>Login</title>
        <link rel='stylesheet' href='/css/styles.css'>
    </head>
    <body>
        <h2>Login</h2>
        <form method='post' action='/login'>
            <label for='email'>Email:</label>
            <input type='email' id='email' name='email' required>
            <button type='submit'>Login</button>
        </form>
    </body>
    </html>";

    context.Response.ContentType = "text/html";
    await context.Response.WriteAsync(html);
});

app.MapPost("/login", async (HttpContext context) =>
{
    var form = await context.Request.ReadFormAsync();
    string email = form["email"];

    if (!string.IsNullOrEmpty(email))
    {
        await context.Response.WriteAsync($"Welcome, {email}!");
    }
    else
    {
        await context.Response.WriteAsync("Invalid email.");
    }
});

app.Run();
public class User
{
    public int Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
}

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options)
    : base(options)
    {
    }

    public DbSet<User> Users { get; set; } = null!;

}

