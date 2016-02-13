using MindsageWeb.Controllers;
using MindsageWeb.Repositories;
using MindsageWeb.Repositories.Models;
using Moq;
using Newtonsoft.Json;
using System;
using TechTalk.SpecFlow;

namespace MindsageWeb.Tests.Specs.Steps
{
    [Binding]
    public class Create_A_CommentSteps
    {
        [When(@"UserProfileId '(.*)' create a new comment with a message is '(.*)' in the lesson '(.*)' of ClassRoom: '(.*)'")]
        public void WhenUserProfileIdCreateANewCommentWithAMessageIsInTheLessonOfClassRoom(string userprofile, string message, string lessonId, string classRoomId)
        {
            var mockCommentRepo = ScenarioContext.Current.Get<Mock<ICommentRepository>>();
            mockCommentRepo.Setup(it => it.UpsertComment(It.IsAny<Comment>()));

            var mockUserActivityRepo = ScenarioContext.Current.Get<Mock<IUserActivityRepository>>();
            mockUserActivityRepo.Setup(it => it.UpsertUserActivity(It.IsAny<UserActivity>()));

            var body = new PostNewCommentRequest
            {
                ClassRoomId = classRoomId,
                Description = message,
                LessonId = lessonId,
                UserProfileId = userprofile
            };

            var commentCtrl = ScenarioContext.Current.Get<CommentController>();
            commentCtrl.Post(body);
        }

        [Then(@"System add new Comment by JSON format is")]
        public void ThenSystemAddNewCommentByJSONFormatIs(string multilineText)
        {
            var expected = JsonConvert.DeserializeObject<Comment>(multilineText);
            var mockCommentRepo = ScenarioContext.Current.Get<Mock<ICommentRepository>>();
            mockCommentRepo.Verify(it => it.UpsertComment(It.Is<Comment>(comment =>
                !string.IsNullOrEmpty(comment.id)
                && comment.ClassRoomId == expected.ClassRoomId
                && comment.CreatedByUserProfileId == expected.CreatedByUserProfileId
                && comment.Description == expected.Description
                && comment.TotalLikes == 0
                && comment.LessonId == expected.LessonId
                && !comment.DeletedDate.HasValue
                && comment.CreatorDisplayName == expected.CreatorDisplayName
                && comment.CreatorImageUrl == expected.CreatorImageUrl
            )));
        }

        [Then(@"System update UserActivity collection with JSON format is")]
        public void ThenSystemUpdateUserActivityCollectionWithJSONFormatIs(string multilineText)
        {
            var expected = JsonConvert.DeserializeObject<UserActivity>(multilineText);
            var mockUserActivityRepo = ScenarioContext.Current.Get<Mock<IUserActivityRepository>>();
            mockUserActivityRepo.Verify(it => it.UpsertUserActivity(It.Is<UserActivity>(activity =>
                JsonConvert.SerializeObject(activity) == JsonConvert.SerializeObject(expected)
            )));
        }
    }
}
