using ElmahCore;
using ElmahCore.DemoCore6;
using ElmahCore.Mvc;
using ElmahCore.Sql;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
//builder.Services.AddElmah<SQ>(options =>
//{
//    options.LogPath = "~/log";
//    options.Notifiers.Add(new MyNotifier());
//    options.Filters.Add(new CmsErrorLogFilter());
//});
builder.Services.AddElmah<SqlErrorLog>(options =>
{
    options.ConnectionString = builder.Configuration.GetConnectionString("ElmahConnection");
    options.SqlServerDatabaseSchemaName = "dbo"; //Defaults to dbo if not set
    options.SqlServerDatabaseTableName = "ELMAH_Error"; //Defaults to ELMAH_Error if not set
    //options.OknPermissionCheck = context => context.User.Identity.IsAuthenticated;
    options.Path = "/elmah";
    options.Filters.Add(new CmsErrorLogFilter());
});

builder.Services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
{
    builder.AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader();
}));

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseElmahExceptionPage();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseCors("MyPolicy");

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseElmah();

app.MapRazorPages();

app.Run();
