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
    }
}
