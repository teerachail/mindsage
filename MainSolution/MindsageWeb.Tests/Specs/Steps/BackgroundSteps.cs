using MindsageWeb.Controllers;
using MindsageWeb.Repositories;
using Moq;
using System;
using TechTalk.SpecFlow;

namespace MindsageWeb.Tests.Specs.Steps
{
    [Binding]
    public class BackgroundSteps
    {
        [Given(@"Initialize mocking data")]
        public void GivenInitializeMockingData()
        {
            var mock = ScenarioContext.Current.Get<MockRepository>();

            var classRoomRepo = mock.Create<IClassRoomRepository>();
            var likeLessonRepo = mock.Create<ILikeLessonRepository>();
            var userprofileRepo = mock.Create<IUserProfileRepository>();
            var classCalendarRepo = mock.Create<IClassCalendarRepository>();
            var lessonCatalogRepo = mock.Create<ILessonCatalogRepository>();
            var commentRepo = mock.Create<ICommentRepository>();
            var courseFriendRepo = mock.Create<ICourseFriendRepository>();
            var userActivityRepo = mock.Create<IUserActivityRepository>();

            ScenarioContext.Current.Set(classRoomRepo);
            ScenarioContext.Current.Set(likeLessonRepo);
            ScenarioContext.Current.Set(userprofileRepo);
            ScenarioContext.Current.Set(classCalendarRepo);
            ScenarioContext.Current.Set(lessonCatalogRepo);
            ScenarioContext.Current.Set(commentRepo);
            ScenarioContext.Current.Set(courseFriendRepo);
            ScenarioContext.Current.Set(userActivityRepo);

            var myCourseCtrl = new LessonController(classCalendarRepo.Object,
                userprofileRepo.Object,
                classRoomRepo.Object,
                likeLessonRepo.Object,
                lessonCatalogRepo.Object,
                commentRepo.Object,
                courseFriendRepo.Object,
                userActivityRepo.Object);

            var commentCtrl = new CommentController(classCalendarRepo.Object,
                userprofileRepo.Object,
                commentRepo.Object,
                userActivityRepo.Object);

            ScenarioContext.Current.Set(myCourseCtrl);
            ScenarioContext.Current.Set(commentCtrl);
        }
    }
}
