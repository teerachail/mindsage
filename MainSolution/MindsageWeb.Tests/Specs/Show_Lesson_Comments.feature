Feature: Show_Lesson_Comments
	In order to avoid silly mistakes
	As a math idiot
	I want to be told the sum of two numbers

Background: Initialize mocking data  
	Given Initialize mocking data  
    And System have ClassCalendar collection with JSON format are
    """
    [
        {
            "id": "CCalendar01",
            "BeginDate": "2/1/2016",
            "ClassRoomId": "CR01",
            "LessonCalendars":
            [
                {
                    "Id": "LC01",
                    "LessonId": "L01",
                    "BeginDate": "2/1/2016",
                    "LessonCatalogId": "LCatalog01"
                },
                {
                    "Id": "LC02",
                    "LessonId": "L02",
                    "BeginDate": "2/8/2016",
                    "LessonCatalogId": "LCatalog02"
                },
                {
                    "Id": "LC03",
                    "LessonId": "L03",
                    "BeginDate": "2/15/2016",
                    "LessonCatalogId": "LCatalog03"
                },
            ]
        },
    ]
    """  
    And System have Subscription collection with JSON format are  
    """
    [
        {
            "id": "S01",
            "UserProfileId": "sakul@mindsage.com",
            "ClassRoomId": "CR01",
            "Role": "Teacher"
        },
    ]
    """  
    And System have CourseFriend collection with JSON format are
    """
    [
        {
            "id": "CourseFriend01",
            "UserProfileId": "sakul@mindsage.com",
            "ClassRoomId": "CR01",
            "FriendWith":
            [
                "earn@mindsage.com"
            ]
        }
    ]
    """  
    And System have Comment collection with JSON format are  
    """
    [
        {
            "id": "Comment01",
            "ClassRoomId": "CR01",
            "CreatedByUserProfileId": "sakul@mindsage.com",
            "Description": "Msg01",
            "TotalLikes": 0,
            "LessonId": "L01",
        },
        {
            "id": "Comment02",
            "ClassRoomId": "CR01",
            "CreatedByUserProfileId": "sakul@mindsage.com",
            "Description": "Msg02",
            "TotalLikes": 5,
            "LessonId": "L02",
            "Discussions":
            [
                {
                    "Id": "DiscussionId01",
                    "Description": "Discussion01",
                    "TotalLikes": 100,
                    "CreatedByUserProfileId": "sakul@mindsage.com",
                }
            ]
        },
        {
            "id": "Comment03",
            "ClassRoomId": "CR01",
            "CreatedByUserProfileId": "earn@mindsage.com",
            "Description": "Msg03",
            "TotalLikes": 10,
            "LessonId": "L02",
            "Discussions":
            [
                {
                    "Id": "DiscussionId02",
                    "Description": "Discussion02",
                    "TotalLikes": 200,
                    "CreatedByUserProfileId": "someone@mindsage.com",
                },
                {
                    "Id": "DiscussionId03",
                    "Description": "Discussion03",
                    "TotalLikes": 300,
                    "CreatedByUserProfileId": "sakul@mindsage.com",
                }
            ]
        },
        {
            "id": "Comment04",
            "ClassRoomId": "CR01",
            "CreatedByUserProfileId": "someone@mindsage.com",
            "Description": "Msg04",
            "TotalLikes": 15,
            "LessonId": "L02",
            "Discussions":
            [
                {
                    "Id": "DiscussionId04",
                    "Description": "Discussion04",
                    "TotalLikes": 400
                }
            ]
        },
    ]
    """ 

@mock  
Scenario: User request lesson's comments and their discussions Then system send the lesson's comments and their discussions back  
    Given Today is '2/8/2016 00:00 am'  
    When UserProfile 'sakul@mindsage.com' request comment & discussion from the lesson 'L02' of ClassRoom: 'CR01'
    Then System send lesson's comment and their discussions with JSON format are  
    """
    [
        {
            "id": "Comment02",
            "ClassRoomId": "CR01",
            "UserProfileId": "sakul@mindsage.com",
            "Description": "Msg02",
            "TotalLikes": 5,
            "LessonId": "L02",
            "Discussions":
            [
                {
                    "Id": "DiscussionId01",
                    "Description": "Discussion01",
                    "TotalLikes": 100,
                    "CreatedByUserProfileId": "sakul@mindsage.com",
                }
            ]
        },
        {
            "id": "Comment03",
            "ClassRoomId": "CR01",
            "UserProfileId": "earn@mindsage.com",
            "Description": "Msg03",
            "TotalLikes": 10,
            "LessonId": "L02",
            "Discussions":
            [
                {
                    "Id": "DiscussionId02",
                    "Description": "Discussion02",
                    "TotalLikes": 200,
                    "CreatedByUserProfileId": "someone@mindsage.com",
                },
                {
                    "Id": "DiscussionId03",
                    "Description": "Discussion03",
                    "TotalLikes": 300,
                    "CreatedByUserProfileId": "sakul@mindsage.com",
                }
            ]
        }
    ]
    """ 