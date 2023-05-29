using AbstractedRabbitMQ;
using AbstractedRabbitMQ.Subscribers;
using Order.api.DataAcess;
using SagaModel.Orders;

namespace Order.api.ResponseListeners
{
    public class InventoryResponseListener:IHostedService
    {
        private readonly OrderDbContext _context;
        private readonly ISubscriber _subscriber;
        private readonly ILogger<InventoryResponseListener> log;

        public InventoryResponseListener(OrderDbContext context,ISubscriber subscriber, ILogger<InventoryResponseListener> log)
        {
            _context = context;
            _subscriber = subscriber;
            this.log = log;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await _subscriber.SubscribeAsyc<InventoryResponse>(Subscribe);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        private async Task<bool> Subscribe(SubResult<InventoryResponse> response,IDictionary<string,object> header)
        {
            if(!response.IsSuccess)
            {
                log.LogError(response.ReasonPhrase);
                return false;
            }
            else
            {
                if (!response.Value!.IsSuccess)
                {
                    var orderDetail = _context.OderDetail.Find(response.Value.OrderId);
                    _context.OderDetail.Remove(orderDetail);
                    await _context.SaveChangesAsync();
                    return false;
                }
            }
            return true;
        }
    }
}
