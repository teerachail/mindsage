Feature: LikeLesson
	In order to avoid silly mistakes
	As a math idiot
	I want to be told the sum of two numbers

Background: Initialize mocking data  
	Given Initialize mocking data  
    And System have ClassRoom collection with JSON format are  
    """
    [
        {
            "id": "CR01",
            "Name": "Emotional literacy",
            "CourseCatalogId": "CC01",
            "CreatedDate": "2/1/2016",
            "Lessons":
            [
                {
                    "id": "L01",
                    "TotalLikes": 1,
                    "LessonCatalogId": "LC01"
                },
                {
                    "id": "L02",
                    "TotalLikes": 0,
                    "LessonCatalogId": "LC02"
                },
            ]
        }
    ]
    """  
    And System have LikeLesson collection with JSON format are  
    """
    [
        {
            "id": "LL01",
            "ClassRoomId": "CR01",
            "LessonId": "L01",
            "LikedByUserProfileId": "miolynet@perfenterprise.com",
            "CreatedDate": "1/1/2016",
        },
    ]
    """

@mock
Scenario: User click like lesson Then system update lesson's total like  
    When UserProfileId 'sakul@mindsage.com' click the like button in the lesson 'L01' of ClassRoom: 'CR01'  
    Then System update total likes in the lesson 'L01' of ClassRoom 'CR01' to '2' likes  
    And System add new LikeLesson by JSON format is  
    """
    {
        "ClassRoomId": "CR01",
        "LessonId": "L01",
        "LikedByUserProfileId": "sakul@mindsage.com",
    }
    """