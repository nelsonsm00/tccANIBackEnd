using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using ANI.Segurança;
using ANI.Arquitetura;

var builder = WebApplication.CreateBuilder(args);
var MyAllowSpecificOrigins = "CORS";
var key = Encoding.ASCII.GetBytes(TokenSecret.Secret);

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.WithOrigins("*")
                          .AllowAnyOrigin()
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                      });
});
// Add services to the container.

builder.Services.AddHttpContextAccessor();  
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(x =>
    {
        x.RequireHttpsMetadata = false;
        x.SaveToken = true;
        x.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(MyAllowSpecificOrigins);

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

AppContext.Configure(app.Services.GetRequiredService<IHttpContextAccessor>());
ContextRequest.Init(new FacadeRequest());

app.Run();

public static class AppContext
{
    public static IHttpContextAccessor HttpContextAccessor { get; set; }
    public static void Configure(IHttpContextAccessor accessor)
    {
        HttpContextAccessor = accessor;
    }
}

public class FacadeRequest : IFacadeRequest
{
    public IHttpContextAccessor ObterRequest() => AppContext.HttpContextAccessor;
}