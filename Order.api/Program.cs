using AbstractedRabbitMQ.Constants;
using AbstractedRabbitMQ.Setup;
using Order.api.DataAcess;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddSqlServer<OrderDbContext>(builder.Configuration.GetConnectionString("connString"));

builder.Services.AddRabbitMQConnection(x =>
{
    x.Url = "amqp://guest:guest@localhost:5672";
    x.ClientProvideName = "Order web Api";
});

builder.Services.AddRabbitMQPublisher(x =>
{
    x.exchangeType = ExchangeTypeRMQ.direct;
    x.exchange = "order-exchange";
});
builder.Services.AddRabbitMQSubscriber(x =>
{
    x.exchange = "invetory-exchange";
    x.exchangeType= ExchangeTypeRMQ.direct;
    x.queue = "invetory-response";
    x.routingKey = "inventory.updated";
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
