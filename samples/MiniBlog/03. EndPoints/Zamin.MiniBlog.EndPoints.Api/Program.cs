using M.YZ.Basement.EndPoints.Web;
using M.YZ.Basement.EndPoints.Web.StartupExtensions;
using M.YZ.Basement.MiniBlog.Infra.Data.Sql.Commands.Common;
using M.YZ.Basement.MiniBlog.Infra.Data.Sql.Queries.Common;
using M.YZ.Basement.Utilities.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

var builder = new BasementProgram().Main(args, "appsettings.json", "appsettings.zamin.json", "appsettings.serilog.json");

ConfigurationManager Configuration = builder.Configuration;

builder.Services.AddBasementApiServices(Configuration);
builder.Services.AddDbContext<MiniblogDbContext>(c => c.UseSqlServer(Configuration.GetConnectionString("MiniBlogCommand_ConnectionString")));
builder.Services.AddDbContext<MiniblogQueryDbContext>(c => c.UseSqlServer(Configuration.GetConnectionString("MiniBlogCommand_ConnectionString")));


//Middlewares
var app = builder.Build();
var basementOptions = app.Services.GetService<BasementConfigurationOptions>();

app.UseBasementApiConfigure(basementOptions, app.Environment);

app.Run();