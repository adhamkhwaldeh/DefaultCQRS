using AlJawad.DefaultCQRS.CQRS;
using DefaultCQRS.Data;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using Microsoft.OpenApi.Models;
using AlJawad.DefaultCQRS.Extensions;
using DefaultCQRS.Entities;
using DefaultCQRS.DTOs;
using DefaultCQRS.Authorization;
using DefaultCQRS.UnitOfWork;
using System.Security.Claims;
using AlJawad.DefaultCQRS.UnitOfWork;
using System.Security.Principal;
using AlJawad.SqlDynamicLinker.ModelBinder;
using AutoMapper;
using DefaultCQRS.Validators;
using FluentValidation;
using DefaultCQRS.Handlers;
//using AlJawad.DefaultCQRS.ModelBinder;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
var assembly = typeof(Program).Assembly;
builder.Services.AddMediatR(new MediatRServiceConfiguration().RegisterServicesFromAssembly(assembly));


builder.Services.AddScoped<IUnitOfWork,UnitOfWork<AppDbContext>>();
//builder.Services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));

//#region required for Default CQRS
// Register controllers
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());


//builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GeneralHandlerInitializer).Assembly));

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ClaimsPrincipal>(sp =>
    sp.GetRequiredService<IHttpContextAccessor>().HttpContext.User);

builder.Services.AddScoped<IPrincipal>(sp =>
    sp.GetRequiredService<IHttpContextAccessor>().HttpContext.User);

builder.Services.AddControllers().AddJsonOptions(
                x =>
                {
                    x.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                    //x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                })
     .AddNewtonsoftJson(options =>
     {
         // Handle cycles safely (optional)
         options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
         options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
     }).ConfigureApiBehaviorOptions(options => { options.SuppressModelStateInvalidFilter = true; });

builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);
builder.Services.InitializeDefaultCQRS();

// Register the dynamic CQRS handlers for the Product entity
builder.Services.AddEntityDynamicConfiguration<UnitOfWork<AppDbContext>,Product, long, CreateProductDto, UpdateProductDto, ProductDto, ProductAuthorizationHandler>(
    builder.Configuration, options =>
{
    // Example of overriding a handler:
    // options.CreateCommandHandler = typeof(MyCustomCreateProductCommandHandler);
    //options.WithCreateValidator<ProductCreateValidator>()
    //.WithUpdateValidator<ProductUpdateValidator>();
});

builder.Services.AddEntityDynamicConfiguration<UnitOfWork<AppDbContext>, Category, long, CreateCategoryDto, UpdateCategoryDto, CategoryDto, CategoryAuthorizationHandler>(
    builder.Configuration, options =>
{
    options.WithCreateHandler<CreateCategoryHandler>();
});
//#endregion

builder.Services.AddControllers();
builder.Services.AddRazorPages();

builder.Services.AddApiVersioning(options =>
{
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.ReportApiVersions = true;
});



builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header,

            },
            new List<string>()
        }
    });
});
//builder.Services.AddVersionedApiExplorer(options =>
//{
//    options.GroupNameFormat = "'v'VVV";
//    options.SubstituteApiVersionInUrl = true;
//});

var app = builder.Build();

var mapper = app.Services.GetRequiredService<IMapper>();
ResultExtensions.Configur(mapper);

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


app.UseHttpsRedirection();

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    //var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
    //foreach (var description in provider.ApiVersionDescriptions)
    //{
    //    options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
    //}
});

app.UseRouting();

app.UseAuthorization();

app.MapControllers();
app.MapRazorPages();

app.Run();
