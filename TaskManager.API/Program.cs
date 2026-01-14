using TaskManager.Infrastructure;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


var connectionString = builder.Configuration.GetConnectionString("Supabase");


builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

// add swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ---------------------------
// 3️⃣ JWT Authentication (Optional, ready for later)
// ---------------------------
// builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//     .AddJwtBearer(options =>
//     {
//         options.TokenValidationParameters = new TokenValidationParameters
//         {
//             ValidateIssuer = true,
//             ValidateAudience = true,
//             ValidateLifetime = true,
//             ValidateIssuerSigningKey = true,
//             ValidIssuer = builder.Configuration["Jwt:Issuer"],
//             ValidAudience = builder.Configuration["Jwt:Audience"],
//             IssuerSigningKey = new SymmetricSecurityKey(
//                 Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
//         };
//     });

// builder.Services.AddAuthorization();

// ---------------------------
// 4️⃣ Build the App
// ---------------------------
var app = builder.Build();

// ---------------------------
// 5️⃣ Swagger Setup
// ---------------------------
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "TaskManager API V1");
        c.RoutePrefix = string.Empty; // Swagger at root URL
    });
}

// ---------------------------
// 6️⃣ Middleware
// ---------------------------
app.UseHttpsRedirection();
// app.UseAuthentication(); // Uncomment when JWT is enabled
// app.UseAuthorization();

// ---------------------------
// 7️⃣ Test Routes
// ---------------------------
app.MapGet("/", () => "TaskManager API is running!");
app.MapGet("/health", () => Results.Ok("Healthy"));

// ---------------------------
// 8️⃣ Database Connection Test (console)
// ---------------------------
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    try
    {
        if (db.Database.CanConnect())
            Console.WriteLine("✅ Successfully connected to database!");
        else
            Console.WriteLine("❌ Cannot connect to database!");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"❌ Database connection failed: {ex.Message}");
    }
}


app.MapControllers();


app.Run();
