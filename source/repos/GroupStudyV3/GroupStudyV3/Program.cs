using Microsoft.EntityFrameworkCore;
using GroupStudyV3.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<GroupStudyV2Context>(opts =>
    opts.UseSqlServer(builder.Configuration.GetConnectionString("GroupStudyV2Context")));
builder.Services.AddRazorPages();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options => {
    options.Cookie.HttpOnly = true;
    options.IdleTimeout = TimeSpan.FromHours(1);
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
    app.UseDeveloperExceptionPage();
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseSession();

app.UseAuthorization();
app.MapRazorPages();
app.Run();
