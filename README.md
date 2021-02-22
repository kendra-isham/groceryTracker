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
			- Set up COMPUTER_VISION_ENDPOINT and COMPUTER_VISION_SUBSCRIPTION_KEY
				- Keys will be provided in additional information when turning in the project 

### TODO
  - Clean API response data 
	- isolate item number, item name, item price 
  - Refactor CleanData() call to be from main 
  - Create users 
  - Assign items 
  - SQL connection
  - GUI 
	- Utilize windows file explorer 
  - Regex to clean Kroger and Meijer receipts 
  