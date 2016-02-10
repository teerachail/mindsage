using Microsoft.VisualStudio.TestTools.UnitTesting;
using MindsageWeb.Controllers;
using MindsageWeb.Repositories.Models;
using Newtonsoft.Json;
using System;
using TechTalk.SpecFlow;

namespace MindsageWeb.Tests.Specs.Steps
{
    [Binding]
    public class Show_Lesson_ContentSteps
    {
        [When(@"UserProfile '(.*)' open the lesson '(.*)' of ClassRoom: '(.*)'")]
        public void WhenUserProfileOpenTheLessonOfClassRoom(string userprofileId, string lessonId, string classRoomId)
        {
            var lessonCtrl = ScenarioContext.Current.Get<LessonController>();
            var result = lessonCtrl.Get(lessonId, classRoomId, userprofileId);
            ScenarioContext.Current.Set(result);
        }

        [Then(@"System send lesson's content with JSON format is")]
        public void ThenSystemSendLessonSContentWithJSONFormatIs(string multilineText)
        {
            var expected = JsonConvert.DeserializeObject<LessonContentRespond>(multilineText);
            var actual = ScenarioContext.Current.Get<LessonContentRespond>();

            Assert.AreEqual(expected.id, actual.id, "Id");
            Assert.AreEqual(expected.Title, actual.Title, "Title");
            Assert.AreEqual(expected.ShortTeacherLessonPlan, actual.ShortTeacherLessonPlan, "ShortTeacherLessonPlan");
            Assert.AreEqual(expected.FullTeacherLessonPlan, actual.FullTeacherLessonPlan, "FullTeacherLessonPlan");
            Assert.AreEqual(expected.CourseMessage, actual.CourseMessage, "CourseMessage");
            Assert.AreEqual(expected.TotalLikes, actual.TotalLikes, "TotalLikes");
            Assert.AreEqual(expected.IsTeacher, actual.IsTeacher, "IsTeacher");
        }
    }
}
