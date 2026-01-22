using TaskManager.Infrastructure;
using Microsoft.EntityFrameworkCore;
using DotNetEnv;


Env.Load(); 


var builder = WebApplication.CreateBuilder(args);


// var connectionString = builder.Configuration.GetConnectionString("Supabase");

var connectionString  = Environment.GetEnvironmentVariable("SUPABASE_CONNECTION_STRING")  ; 

// Fix PostgreSQL UTC vs timestamp without time zone
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);


builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));



builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy =>
        {
            policy
                .WithOrigins("http://localhost:3000")
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
        });
});

    
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services
    .AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler =
            System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// jwt
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
var app = builder.Build();
app.UseCors("AllowFrontend");

// add swagger ui
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "TaskManager API V1");
        c.RoutePrefix = string.Empty; // Swagger at root URL
    });
}

// 6️Middleware
app.UseHttpsRedirection();
// app.UseAuthentication(); // Uncomment when JWT is enabled
// app.UseAuthorization();


using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
       Console.WriteLine("connectionString")   ;

   Console.WriteLine(connectionString)   ;
    try
    {
        if (db.Database.CanConnect())

{            Console.WriteLine("✅ Successfully connected to database!");
}        else

{            Console.WriteLine("❌ Cannot connect to database!");
       Console.WriteLine("connectionString")   ;

   Console.WriteLine(connectionString)   ;
}    }
    catch (Exception ex)
    {
        Console.WriteLine($"❌ Database connection failed: {ex.Message}");
    }
}


app.MapControllers();


app.Run();
