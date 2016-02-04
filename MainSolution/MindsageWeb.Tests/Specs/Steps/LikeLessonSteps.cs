using System;
using TechTalk.SpecFlow;

namespace MindsageWeb.Tests.Specs.Steps
{
    [Binding]
    public class LikeLessonSteps
    {
        [When(@"User '(.*)' clicked the like button in the lesson '(.*)'")]
        public void WhenUserClickedTheLikeButtonInTheLesson(string p0, string p1)
        {
            ScenarioContext.Current.Pending();
        }

        [Then(@"System update likes in the lesson '(.*)' to '(.*)' likes")]
        public void ThenSystemUpdateLikesInTheLessonToLikes(string p0, int p1)
        {
            ScenarioContext.Current.Pending();
        }

        [Then(@"LikeLesson collection JSON format in the system are")]
        public void ThenLikeLessonCollectionJSONFormatInTheSystemAre(string multilineText)
        {
            ScenarioContext.Current.Pending();
        }
    }
}
