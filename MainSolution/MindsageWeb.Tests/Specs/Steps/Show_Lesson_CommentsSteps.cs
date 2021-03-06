﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using MindsageWeb.Controllers;
using MindsageWeb.Repositories.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
            var expectedObj = JsonConvert.DeserializeObject<IEnumerable<Comment>>(multilineText);
            var actualObj = ScenarioContext.Current.Get<IEnumerable<Comment>>();

            var expectedString = JsonConvert.SerializeObject(expectedObj);
            var actualString = JsonConvert.SerializeObject(actualObj);

            Assert.AreEqual(expectedString, actualString);
        }
    }
}
