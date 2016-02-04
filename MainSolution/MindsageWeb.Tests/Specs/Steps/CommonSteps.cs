using MindsageWeb.Repositories.Models;
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
        [Given(@"System have ClassRoom collection with JSON format are")]
        public void GivenSystemHaveClassRoomCollectionWithJSONFormatAre(string multilineText)
        {
            var classRooms = JsonConvert.DeserializeObject<IEnumerable<ClassRoom>>(multilineText);
            ScenarioContext.Current.Pending();
        }

        [Given(@"System have LikeLesson collection with JSON format are")]
        public void GivenSystemHaveLikeLessonCollectionWithJSONFormatAre(string multilineText)
        {
            var likeLessons = JsonConvert.DeserializeObject<IEnumerable<LikeLesson>>(multilineText);
            ScenarioContext.Current.Pending();
        }
    }
}
