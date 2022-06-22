global using iRead.DBModels.Models;
using iRead.API.Repositories;
using iRead.API.Repositories.Interfaces;
using iRead.API.Utilities;
using iRead.API.Utilities.Interfaces;
using iRead.RecommendationSystem;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.ML;
using iRead.RecommendationSystem.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options => options.AddPolicy("CORSPolicy", builderCORS => builderCORS.AllowAnyMethod().AllowAnyHeader().WithOrigins("http://localhost:3000", "http://192.168.2.42:3000")));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<iReadDBContext>();
builder.Services.AddScoped<IGenderRepository, GenderRepository>();
builder.Services.AddScoped<IIdentificationRepository, IdentificationRepository>();
builder.Services.AddScoped<IAuthorRepository, AuthorRepository>();
builder.Services.AddScoped<IMemberRepository, MemberRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserCategoryRepository, UserCategoryRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IPublisherRepository, PublisherRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IRatingRepository, RatingRepository>();
builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<IFavoriteRepository, FavoriteRepository>();
builder.Services.AddScoped<INotificationRepository, NotificationRepository>();

builder.Services.AddScoped<IValidationUtilities, ValidationUtilities>();
builder.Services.AddScoped<IAuthenticationUtilities, AuthenticationUtilities>();
builder.Services.AddScoped<IRecommendationUtilities, RecommendationUtilities>();
builder.Services.AddScoped<IEmailUtilities, EmailUtilities>();

builder.Services.AddScoped<Recommender>();
builder.Services.AddPredictionEnginePool<RecommendationInput, RecommendationOutput>().FromFile(builder.Configuration.GetSection("RecommendationModelPath").Value, true);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options => {
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(builder.Configuration.GetSection("AppSettings:Token").Value)),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("CORSPolicy");
app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
