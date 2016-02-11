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
    public partial class Create_A_CommentFeature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "Create_A_Comment.feature"
#line hidden
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.ClassInitializeAttribute()]
        public static void FeatureSetup(Microsoft.VisualStudio.TestTools.UnitTesting.TestContext testContext)
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner(null, 0);
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "Create_A_Comment", "\tIn order to avoid silly mistakes\r\n\tAs a math idiot\r\n\tI want to be told the sum o" +
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
                        && (testRunner.FeatureContext.FeatureInfo.Title != "Create_A_Comment")))
            {
                MindsageWeb.Tests.Specs.Create_A_CommentFeature.FeatureSetup(null);
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
    testRunner.And("System have Subscription collection with JSON format are", "[\r\n    {\r\n        \"id\": \"Subscription01\",\r\n        \"UserProfileId\": \"sakul@mindsa" +
                    "ge.com\",\r\n        \"ClassRoomId\": \"ClassRoom01\",\r\n        \"Role\": \"Teacher\"\r\n    " +
                    "},\r\n]", ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 19
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
                ""Order"": 1,
                ""SemesterGroupName"": ""A"",
                ""BeginDate"": ""2/1/2016"",
            },
            {
                ""Id"": ""LessonCalendar02"",
                ""LessonId"": ""Lesson02"",
                ""Order"": 2,
                ""SemesterGroupName"": ""A"",
                ""BeginDate"": ""2/8/2016"",
            },
            {
                ""Id"": ""LessonCalendar03"",
                ""LessonId"": ""Lesson03"",
                ""Order"": 3,
                ""SemesterGroupName"": ""B"",
                ""BeginDate"": ""2/15/2016"",
            },
        ]
    },
]", ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 53
    testRunner.And("System have UserActivity collection with JSON format are", @"[
    {
        ""id"": ""UserActivity01"",
        ""ClassRoomId"": ""ClassRoom01"",
        ""UserProfileName"": ""sakul@mindsage.com"",
        ""LessonActivities"":
        [
            {
                ""Id"": ""LessonActivity01"",
                ""LessonId"": ""Lesson01"",
                ""ViewContentsCounts"": 1,
	""TotalContents"": 1,
                ""CreatedComments"": 0,
                ""SendLikes"": 1
            }
        ]
    },
]", ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("User create a new comment Then system create a new comment")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "Create_A_Comment")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("mock")]
        public virtual void UserCreateANewCommentThenSystemCreateANewComment()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("User create a new comment Then system create a new comment", new string[] {
                        "mock"});
#line 76
this.ScenarioSetup(scenarioInfo);
#line 6
this.FeatureBackground();
#line 77
    testRunner.Given("Today is \'2/8/2016 00:00 am\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 78
    testRunner.When("UserProfileId \'sakul@mindsage.com\' create a new comment with a message is \'Hello " +
                    "lesson 1\' in the lesson \'Lesson01\' of ClassRoom: \'ClassRoom01\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 79
    testRunner.Then("System add new Comment by JSON format is", "{\r\n    \"ClassRoomId\": \"ClassRoom01\",\r\n    \"CreatedByUserProfileId\": \"sakul@mindsa" +
                    "ge.com\",\r\n    \"Description\": \"Hello lesson 1\",\r\n    \"TotalLikes\": 0,\r\n    \"Lesso" +
                    "nId\": \"Lesson01\",\r\n    \"Discussions\": []\r\n}", ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
#line 90
    testRunner.And("System update UserActivity collection with JSON format is", @"{
    ""id"": ""UserActivity01"",
    ""ClassRoomId"": ""ClassRoom01"",
    ""UserProfileName"": ""sakul@mindsage.com"",
    ""LessonActivities"":
    [
        {
            ""Id"": ""LessonActivity01"",
            ""LessonId"": ""Lesson01"",
            ""ViewContentsCounts"": 1,
""TotalContents"": 1,
            ""CreatedComments"": 1,
            ""SendLikes"": 1
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