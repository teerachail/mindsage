using System;
using TechTalk.SpecFlow;

namespace MindsageWeb.Tests.Specs.Steps
{
    [Binding]
    public class BackgroundSteps
    {
        [Given(@"Initialize mocking data")]
        public void GivenInitializeMockingData()
        {
            ScenarioContext.Current.Pending();
        }
    }
}
