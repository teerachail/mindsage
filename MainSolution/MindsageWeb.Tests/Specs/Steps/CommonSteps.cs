using MindsageWeb.Repositories;
using MindsageWeb.Repositories.Models;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechTalk.SpecFlow;

namespace MindsageWeb.Tests.Specs.Steps
{
    [Binding]
    public sealed class CommonSteps
    {
        [Given(@"System have Subscription collection with JSON format are")]
        public void GivenSystemHaveSubscriptionCollectionWithJSONFormatAre(string multilineText)
        {
            var subscriptions = JsonConvert.DeserializeObject<IEnumerable<Subscription>>(multilineText);
            var mockSubscriptionRepo = ScenarioContext.Current.Get<Mock<ISubscriptionRepository>>();
            mockSubscriptionRepo.Setup(it => it.GetSubscriptionsByUserProfileId(It.IsAny<string>()))
                .Returns<string>(userprofileId => subscriptions.Where(it => it.UserProfileId == userprofileId));
        }

        [Given(@"System have ClassCalendar collection with JSON format are")]
        public void GivenSystemHaveClassCalendarCollectionWithJSONFormatAre(string multilineText)
        {
            var classCalendars = JsonConvert.DeserializeObject<IEnumerable<ClassCalendar>>(multilineText);
            var mockClassCalendarRepo = ScenarioContext.Current.Get<Mock<IClassCalendarRepository>>();
            mockClassCalendarRepo.Setup(it => it.GetClassCalendarByClassRoomId(It.IsAny<string>()))
                .Returns<string>(classRoomId => classCalendars.Where(it => it.ClassRoomId == classRoomId).FirstOrDefault());
        }

        [Given(@"Today is '(.*)'")]
        public void GivenTodayIs(DateTime currentTime)
        {
            ScenarioContext.Current.Set(currentTime);
        }

        [Given(@"System have ClassRoom collection with JSON format are")]
        public void GivenSystemHaveClassRoomCollectionWithJSONFormatAre(string multilineText)
        {
            var classRooms = JsonConvert.DeserializeObject<IEnumerable<ClassRoom>>(multilineText);
            var mockClassRoomRepo = ScenarioContext.Current.Get<Moq.Mock<IClassRoomRepository>>();
            mockClassRoomRepo.Setup(it => it.GetClassRoomById(It.IsAny<string>()))
                .Returns<string>(id => classRooms.FirstOrDefault(it => it.id == id));
        }

        [Given(@"System have LikeLesson collection with JSON format are")]
        public void GivenSystemHaveLikeLessonCollectionWithJSONFormatAre(string multilineText)
        {
            var likeLessons = JsonConvert.DeserializeObject<IEnumerable<LikeLesson>>(multilineText);
            var mockLikeLessonRepo = ScenarioContext.Current.Get<Moq.Mock<ILikeLessonRepository>>();
            mockLikeLessonRepo.Setup(it => it.GetLikeLessonsByLessonId(It.IsAny<string>()))
                .Returns<string>(id => likeLessons.Where(it => it.LessonId == id));
        }

        [Given(@"System have LessonCatalog collection with JSON format are")]
        public void GivenSystemHaveLessonCatalogCollectionWithJSONFormatAre(string multilineText)
        {
            var lessonCatalogs = JsonConvert.DeserializeObject<IEnumerable<LessonCatalog>>(multilineText);
            var mockLessonCatalogRepo = ScenarioContext.Current.Get<Moq.Mock<ILessonCatalogRepository>>();
            mockLessonCatalogRepo.Setup(it => it.GetLessonCatalogById(It.IsAny<string>()))
                .Returns<string>(id => lessonCatalogs.Where(it => it.id == id).FirstOrDefault());
        }

        [Given(@"System have CourseFriend collection with JSON format are")]
        public void GivenSystemHaveCourseFriendCollectionWithJSONFormatAre(string multilineText)
        {
            var courseFriends = JsonConvert.DeserializeObject<IEnumerable<CourseFriend>>(multilineText);
            var mockCourseFriendRepo = ScenarioContext.Current.Get<Moq.Mock<ICourseFriendRepository>>();
            mockCourseFriendRepo.Setup(it => it.GetCourseFriendByUserProfile(It.IsAny<string>()))
                .Returns<string>(id => courseFriends.Where(it => it.UserProfileId == id).FirstOrDefault());
        }

        [Given(@"System have Comment collection with JSON format are")]
        public void GivenSystemHaveCommentCollectionWithJSONFormatAre(string multilineText)
        {
            var comments = JsonConvert.DeserializeObject<IEnumerable<Comment>>(multilineText);
            var mockCommentRepo = ScenarioContext.Current.Get<Moq.Mock<ICommentRepository>>();
            mockCommentRepo.Setup(it => it.GetCommentsByLessonId(It.IsAny<string>(), It.IsAny<IEnumerable<string>>()))
                .Returns<string, IEnumerable<string>>((id, creators) => comments.Where(it => it.LessonId == id && creators.Contains(it.CreatedByUserProfileId)));
        }

        [Given(@"System have UserActivity collection with JSON format are")]
        public void GivenSystemHaveUserActivityCollectionWithJSONFormatAre(string multilineText)
        {
            var userActivities = JsonConvert.DeserializeObject<IEnumerable<UserActivity>>(multilineText);
            var mockUserActivityRepo = ScenarioContext.Current.Get<Moq.Mock<IUserActivityRepository>>();
            mockUserActivityRepo.Setup(it => it.GetUserActivityByUserProfile(It.IsAny<string>()))
                .Returns<string>(userprofile => userActivities.FirstOrDefault(it => it.UserProfileName == userprofile));
        }
    }
}
