# Approach

Based on the 'eShopOnContainers' microservices example provided by Microsoft.

## Roadmap 0

Implemented a basic MSSQL database setup with migrations and data-seeding.
The server is minimally set up for now but will incorporate better logging functionality soon.
The server and database are both hosted in docker containers and created using a docker-compose file.
Added Serilog logging to the program but could not get Seq working for log persistence and metrics support.
Will resolve it as it is an optional objective at this stage.

## Roadmap 1

Currency exchange trades:
* Will require two currencies.
* Will require one user.
* Will require at least one exchange rate between the two currencies.

Flow:
* Call the exchange rate endpoint with two currencies to get the current rate between user's chosen currencies.
  * Check that the exchange rate is less than 30 minutes old.
	* If it is, send back the current rate.
	* If it isn't, call the external api to get the latest rate (this will be mocked for now).
* Call the post exchange rate endpoint passing in an amount to be exchanged.
  * Check that the amount being exchanged is valid (greater than zero, etc.).
  * If valid, record the transaction in a table as the currencies used, the amount exchanged, the amount received, the user performing it, and the date-time of the transaction.
  * Return a success message and the transaction details saved.
