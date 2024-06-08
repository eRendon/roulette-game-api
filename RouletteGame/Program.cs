using RouletteGame;
using Microsoft.EntityFrameworkCore;
using RouletteGame.DB;


var builder =
#if IS_NATIVE_AOT
    WebApplication.CreateSlimBuilder(args);
#else
    WebApplication.CreateBuilder(args);
#endif

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowVueApp",
        builder =>
        {
            builder.WithOrigins("http://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyMethod();
        });
});

builder.Services.AddControllers();

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.TypeInfoResolverChain.Insert(0, RouletteGameJsonContext.Default);
});

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));


var app = builder.Build();

app.MapControllers();
app.UseCors("AllowVueApp");
app.Run();

