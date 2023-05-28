using AbstractedRabbitMQ.Constants;
using AbstractedRabbitMQ.Setup;
using Inventory.mvc.DataAccess;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSqlServer<InventoryDbContext>(builder.Configuration.GetConnectionString("connString"));
builder.Services.AddRabbitMQConnection(x =>
{
    x.Url = "amqp://guest:guest@localhost:5672";
    x.ClientProvideName = "Inventory app";
});

builder.Services.AddRabbitMQPublisher(x =>
{
    x.exchange = "invetory-exchange";
    x.exchangeType = ExchangeTypeRMQ.direct;
});

builder.Services.AddRabbitMQSubscriber(x =>
{
    x.queue = "order-response";
    x.exchange = "order-exchange";
    x.routingKey = "order.created";
    x.exchangeType= ExchangeTypeRMQ.direct;     
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
