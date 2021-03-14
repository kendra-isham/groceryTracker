# Grocery Calculator

### Code Louisville Needs Fulfulled 
	- api call 
	- master loop 
	- additional classes 
	- create a list 
	- read data from external file 
	- visualize data (if time, visualize trends in purchased items)

### Premise
	- User designates file path to Costco receipt image
	- Loop through assigning each item\price to user
	- Log each user's purchased items to database 
	- Ability to view each user's purchase history 

### Setup to run locally 
	- Set up Window's environment variables 
		- Open a Window's Run prompt (win key + R)
		- Enter 'sysdm.cpl'
		- Advanced Tab 
			- Environment Variables 
			- System Variables 
			- Set up COMPUTER_VISION_ENDPOINT, COMPUTER_VISION_SUBSCRIPTION_KEY, DATASOURCE, SQLUsername, SQLPassword
				- Information will be provided in "additional information" when turning in the project 

### TODO
  - SQL connection
	- Connect to azure SQL 
	- Potentially refactor code to better suit SQL 
	- https://docs.microsoft.com/en-us/azure/azure-sql/database/connect-query-dotnet-visual-studio
  - Assign items
  - GUI 
	- Utilize windows file explorer 
  - Regex to clean Kroger and Meijer receipts 
  