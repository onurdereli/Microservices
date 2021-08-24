using Course.Shared.Messages.Events.Abstract;

namespace Course.Shared.Messages.Events.Concreate
{
    public class CourseNameChangedEvent: ICourseNameChangedEvent
    {
        public string CourseId { get; set; }

        public string UpdatedName { get; set; }
    }
}
