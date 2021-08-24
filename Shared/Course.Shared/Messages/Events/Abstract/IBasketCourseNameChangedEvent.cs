using System;
using System.Collections.Generic;
using System.Text;

namespace Course.Shared.Messages.Events.Abstract
{
    public interface IBasketCourseNameChangedEvent
    {
        public string CourseId { get; set; }

        public string UpdatedName { get; set; }

        public string UserId { get; set; }
    }
}
