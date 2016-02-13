﻿// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by SpecFlow (http://www.specflow.org/).
//      SpecFlow Version:2.0.0.0
//      SpecFlow Generator Version:2.0.0.0
// 
//      Changes to this file may cause incorrect behavior and will be lost if
//      the code is regenerated.
//  </auto-generated>
// ------------------------------------------------------------------------------
#region Designer generated code
#pragma warning disable
namespace MindsageWeb.Tests.Specs
{
    using TechTalk.SpecFlow;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "2.0.0.0")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [Microsoft.VisualStudio.TestTools.UnitTesting.TestClassAttribute()]
    public partial class Click_Like_A_CommentFeature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "Click_Like_A_Comment.feature"
#line hidden
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.ClassInitializeAttribute()]
        public static void FeatureSetup(Microsoft.VisualStudio.TestTools.UnitTesting.TestContext testContext)
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner(null, 0);
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "Click_Like_A_Comment", "\tIn order to avoid silly mistakes\r\n\tAs a math idiot\r\n\tI want to be told the sum o" +
                    "f two numbers", ProgrammingLanguage.CSharp, ((string[])(null)));
            testRunner.OnFeatureStart(featureInfo);
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.ClassCleanupAttribute()]
        public static void FeatureTearDown()
        {
            testRunner.OnFeatureEnd();
            testRunner = null;
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestInitializeAttribute()]
        public virtual void TestInitialize()
        {
            if (((testRunner.FeatureContext != null) 
                        && (testRunner.FeatureContext.FeatureInfo.Title != "Click_Like_A_Comment")))
            {
                MindsageWeb.Tests.Specs.Click_Like_A_CommentFeature.FeatureSetup(null);
            }
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCleanupAttribute()]
        public virtual void ScenarioTearDown()
        {
            testRunner.OnScenarioEnd();
        }
        
        public virtual void ScenarioSetup(TechTalk.SpecFlow.ScenarioInfo scenarioInfo)
        {
            testRunner.OnScenarioStart(scenarioInfo);
        }
        
        public virtual void ScenarioCleanup()
        {
            testRunner.CollectScenarioErrors();
        }
        
        public virtual void FeatureBackground()
        {
#line 6
#line 7
 testRunner.Given("Initialize mocking data", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 8
    testRunner.And("System have UserProfile collection with JSON format are", "[\r\n{\r\n\"id\": \"sakul@mindsage.com\",\r\n\"Subscriptions\":\r\n[\r\n{\r\n\t\"id\": \"Subscription01" +
                    "\",\r\n\t\"Role\": \"Teacher\",\r\n\t\"ClassRoomId\": \"ClassRoom01\",\r\n\t\"ClassCalendarId\": \"Cl" +
                    "assCalendar01\",\r\n},\r\n]\r\n},\r\n]", ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 25
    testRunner.And("System have ClassCalendar collection with JSON format are", @"[
    {
        ""id"": ""ClassCalendar01"",
        ""BeginDate"": ""2/1/2016"",
        ""ClassRoomId"": ""ClassRoom01"",
        ""LessonCalendars"":
        [
            {
                ""Id"": ""LessonCalendar01"",
                ""LessonId"": ""Lesson01"",
                ""LessonCatalogId"": ""LessonCatalog01"",
                ""Order"": 1,
                ""SemesterGroupName"": ""A"",
                ""BeginDate"": ""2/1/2016"",
            }
        ]
    },
]", ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 46
    testRunner.And("System have Comment collection with JSON format are", @"[
    {
        ""Id"": ""Comment01"",
        ""ClassRoomId"": ""ClassRoom01"",
        ""CreatedByUserProfileId"": ""sakul@mindsage.com"",
        ""Description"": ""Hello lesson 1"",
        ""TotalLikes"": 0,
        ""LessonId"": ""Lesson01"",
        ""Discussions"": []
    }
]", ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 60
 testRunner.And("System have LikeComment collection with JSON format are", "[]", ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 64
    testRunner.And("System have UserActivity collection with JSON format are", @"[
{
""id"": ""UserActivity01"",
""UserProfileId"": ""sakul@mindsage.com"",
""ClassRoomId"": ""ClassRoom01"",
""LessonActivities"":
[
{
	""id"": ""LessonActivity01"",
	""LessonId"": ""Lesson01"",

	""TotalContentsAmount"": 1,
	""SawContentIds"": 
	[
		""Content01""
	],
	""CreatedCommentAmount"": 1,
	""ParticipationAmount"": 1
}
]
}
]", ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("User click like a comment Then system update comment\'s total like")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "Click_Like_A_Comment")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("mock")]
        public virtual void UserClickLikeACommentThenSystemUpdateCommentSTotalLike()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("User click like a comment Then system update comment\'s total like", new string[] {
                        "mock"});
#line 91
this.ScenarioSetup(scenarioInfo);
#line 6
this.FeatureBackground();
#line 92
    testRunner.Given("Today is \'2/8/2016 00:00 am\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 93
    testRunner.When("UserProfileId \'sakul@mindsage.com\' click the like button for comment \'Comment01\' " +
                    "in the lesson \'Lesson01\' of ClassRoom: \'ClassRoom01\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 94
    testRunner.Then("System update total likes for comment \'Comment01\' in the lesson \'Lesson01\' of Cla" +
                    "ssRoom \'ClassRoom01\' to \'1\' likes", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
#line 95
    testRunner.And("System add new LikeComment by JSON format is", "{\r\n    \"LessonId\": \"Lesson01\",\r\n\"CommentId\": \"Comment01\",\r\n    \"LikedByUserProfil" +
                    "eId\": \"sakul@mindsage.com\",\r\n\"CreatedDate\": \"2/8/2016 00:00 am\"\r\n}", ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 104
 testRunner.And("System update UserActivity collection with JSON format is", @"{
""id"": ""UserActivity01"",
""UserProfileId"": ""sakul@mindsage.com"",
""ClassRoomId"": ""ClassRoom01"",
""LessonActivities"":
[
{
""id"": ""LessonActivity01"",
""LessonId"": ""Lesson01"",

""TotalContentsAmount"": 1,
""SawContentIds"": 
[
	""Content01""
],
""CreatedCommentAmount"": 1,
""ParticipationAmount"": 2
}
]
}", ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion