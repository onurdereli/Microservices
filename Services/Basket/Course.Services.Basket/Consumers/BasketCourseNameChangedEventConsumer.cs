using System.Linq;
using System.Threading.Tasks;
using Course.Services.Basket.Services.Abstract;
using Course.Shared.Messages.Events.Abstract;
using Course.Shared.Services.Abstract;
using MassTransit;

namespace Course.Services.Basket.Consumers
{
    public class BasketCourseNameChangedEventConsumer : IConsumer<IBasketCourseNameChangedEvent>
    {
        private readonly IBasketService _basketService;

        public BasketCourseNameChangedEventConsumer(IBasketService basketService)
        {
            _basketService = basketService;
        }


        public async Task Consume(ConsumeContext<IBasketCourseNameChangedEvent> context)
        {
            
            var basket = await _basketService.GetBasketAysnc(context.Message.UserId);
            var basketItem = basket.Data.BasketItems.Where(x => x.CourseId == context.Message.CourseId).ToList();
            basketItem.ForEach(item=> item.CourseName = context.Message.UpdatedName);
            await _basketService.SaveOrUpdateAysnc(basket.Data);
        }
    }
}
