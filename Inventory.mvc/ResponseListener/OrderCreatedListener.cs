using AbstractedRabbitMQ;
using AbstractedRabbitMQ.Publishers;
using AbstractedRabbitMQ.Subscribers;
using Inventory.mvc.DataAccess;
using SagaModel.Orders;

namespace Inventory.mvc.ResponseListener
{
    public class OrderCreatedListener : IHostedService
    {
        private readonly ISubscriber subscriber;
        private readonly IPublisher publisher;
        private readonly ILogger<OrderCreatedListener> log;
        private readonly InventoryDbContext context;

        public OrderCreatedListener(ISubscriber subscriber,IPublisher publisher,ILogger<OrderCreatedListener> log, InventoryDbContext context)
        {
            this.subscriber = subscriber;
            this.publisher = publisher;
            this.log = log;
            this.context = context;
        }
        public async Task StartAsync(CancellationToken cancellationToken)
        {
           await subscriber.SubscribeAsyc<OrderRequest>(Subscribe);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        private async Task<bool> Subscribe(SubResult<OrderRequest> response, IDictionary<string,object> header)
        {
            var inventory = new InventoryResponse();
            if (!response.IsSuccess) 
            {
                log.LogError(response.ReasonPhrase);
            }
            var product = context.Inventory.Where(x => x.ProductId == response.Value.ProductId).FirstOrDefault();
            product.Quantity = +1;
            try
            {
                
                context.Inventory.Update(product);
                await context.SaveChangesAsync();
                inventory.IsSuccess = true;
                inventory.OrderId = product.ProductId;
                publisher.Publish<InventoryResponse>(inventory, "inventory.updated", null, null);
            }
            catch (Exception)
            {
                inventory.IsSuccess = false;
                inventory.OrderId = product.ProductId;
                publisher.Publish<InventoryResponse>(inventory, "inventory.updated", null, null);
            }
            
            return true;
        }

    }
}
