Feature: Get Scores
As an operator,
I want to know which scores beat a total
So that I know who to reward

Scenario Outline: When I give a score only those scores that beat it are returned
	Given I have a score of '<Score>'
	When I call the get scores endpoint
	Then the status code is 'OK'
	And I will get '<Number>' results back
	And all the records are correct
	And all the scores are above '<Score>'
Examples: 
| Score | Number |
| 0     | 4      |
| 1     | 3      |
| 3     | 1      |
| 5     | 0      |

Scenario Outline: When I give bad data I get bad request
	Given I have a score of '<Score>'
		When I call the get scores endpoint
	Then the status code is 'Bad Request'
Examples: 
| Score |
| -1    |
| 6     |
| 1000  | 

Scenario: If there is an issue I get Server Error
	Given I have a score of '1'
	And the client has an issue
	When I call the get scores endpoint
	Then the status code is 'Internal Server Error'


