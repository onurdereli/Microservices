using Course.Shared.Messages.Events.Abstract;

namespace Course.Shared.Messages.Events.Concreate
{
    public class BasketCourseNameChangedEvent: IBasketCourseNameChangedEvent
    {
        public string CourseId { get; set; }

        public string UpdatedName { get; set; }

        public string UserId { get; set; }
    }
}
