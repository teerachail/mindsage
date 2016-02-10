using MindsageWeb.Controllers;
using System;
using TechTalk.SpecFlow;

namespace MindsageWeb.Tests.Specs.Steps
{
    [Binding]
    public class Show_Lesson_CommentsSteps
    {
        [When(@"UserProfile '(.*)' request comment & discussion from the lesson '(.*)' of ClassRoom: '(.*)'")]
        public void WhenUserProfileRequestCommentDiscussionFromTheLessonOfClassRoom(string userprofile, string lessonId, string classRoomId)
        {
            var lessonCtrl = ScenarioContext.Current.Get<LessonController>();
            var result = lessonCtrl.Comments(lessonId, classRoomId, userprofile);
            ScenarioContext.Current.Set(result);
        }

        [Then(@"System send lesson's comment and their discussions with JSON format are")]
        public void ThenSystemSendLessonSCommentAndTheirDiscussionsWithJSONFormatAre(string multilineText)
        {
            ScenarioContext.Current.Pending();
        }
    }
}
