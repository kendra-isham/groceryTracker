# Grocery Calculator

### Code Louisville Needs Fulfulled 
	- api call 
	- master loop 
	- additional classes 
	- create a list 
	- read data from external file 
	- LINQ

### Premise
	- User designates file path to Costco receipt image
	- API call to return text data from image 
	- Loop through assigning each item\price to user
	- Log each user's purchased items to database 

### Setup to run locally 
	- Set up Window's environment variables
		- Open a Window's Run prompt (win key + R)
		- Enter 'sysdm.cpl'
		- Advanced Tab 
			- Environment Variables 
			- System Variables 
			- Set up COMPUTER_VISION_ENDPOINT, COMPUTER_VISION_SUBSCRIPTION_KEY, DATASOURCE, SQLUsername, SQLPassword
				- Information will be provided in "additional information" when turning in the project 
	- Costco Receipt examples will be in a Google Drive link provided within additional info during project submission 

### TODO
  - SQL 
	- Menu option to view previous purchases 
  - Add "still processing message" if api takes over 5 seconds to respond
  - Regex to clean Kroger and Meijer receipts 
  