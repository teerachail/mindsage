Feature: Remove_A_Comment
	In order to avoid silly mistakes
	As a math idiot
	I want to be told the sum of two numbers

Background: Initialize mocking data  
	Given Initialize mocking data  
    And System have Subscription collection with JSON format are  
    """
    [
        {
            "id": "Subscription01",
            "UserProfileId": "sakul@mindsage.com",
            "ClassRoomId": "ClassRoom01",
            "Role": "Teacher"
        },
    ]
    """  
    And System have ClassCalendar collection with JSON format are
    """
    [
        {
            "id": "ClassCalendar01",
            "BeginDate": "2/1/2016",
            "ClassRoomId": "ClassRoom01",
            "LessonCalendars":
            [
                {
                    "Id": "LessonCalendar01",
                    "LessonId": "Lesson01",
                    "LessonCatalogId": "LessonCatalog01",
                    "Order": 1,
                    "SemesterGroupName": "A",
                    "BeginDate": "2/1/2016",
                }
            ]
        },
    ]
    """  
    And System have Comment collection with JSON format are  
    """
    [
        {
            "Id": "Comment01",
            "ClassRoomId": "ClassRoom01",
            "CreatedByUserProfileId": "sakul@mindsage.com",
            "Description": "Hello lesson 1",
            "TotalLikes": 0,
            "LessonId": "Lesson01",
            "Discussions": []
        }
    ]
    """ 
    And System have UserActivity collection with JSON format are
    """
    [
        {
            "id": "UserActivity01",
            "ClassRoomId": "ClassRoom01",
            "UserProfileName": "sakul@mindsage.com",
            "LessonActivities":
            [
				{
                    "Id": "LessonActivity01",
                    "LessonId": "Lesson01",
                    "ViewContentsCounts": 1,
					"TotalContents": 1,
                    "CreatedComments": 1,
                    "SendLikes": 1
                }
            ]
        },
    ]
    """  
    
@mock  
Scenario: User remove his comment Then system remove his comment  
    Given Today is '2/8/2016 00:00 am'  
    When UserProfileId 'sakul@mindsage.com' remove the comment 'Comment01' in the lesson 'Lesson01' of ClassRoom: 'ClassRoom01'  
    Then System Update Comment by JSON format is  
    """
    {
        "Id": "Comment01",
        "ClassRoomId": "ClassRoom01",
        "CreatedByUserProfileId": "sakul@mindsage.com",
        "Description": "Hello lesson 1",
        "TotalLikes": 0,
        "LessonId": "Lesson01",
        "Discussions": [],
        "DeletedDate": "2/8/2016 00:00 am"
    }
    """
    And System doesn't update UserActivity collection with JSON format is  