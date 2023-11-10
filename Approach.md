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
* Will require two account balances..
* Will require at least one exchange rate between the two currencies.

Flow:
* Call the exchange rate endpoint with two currencies to get the current rate between user's chosen currencies.
  * Check that the exchange rate is less than 30 minutes old.
	* If it is, send back the current rate.
	* If it isn't, call the external api to get the latest rate (this will be mocked for now).
* Call the post exchange rate endpoint passing in an amount to be exchanged.
  * Check that the amount being exchanged is valid (greater than zero, less than the account balance, etc.).
  * If valid, check that the target account balance exists.
	* If not, create it and assign it to the user.
  * Record the transaction in a table as the currencies used, the amount exchanged, the amount received, the user performing it, and the date-time of the transaction.
  * Return a success message and the transaction details saved.

Currency exchange logic was migrated to use MediatR in a Command-Query pattern. This should allow for better scalability when implementing new functionality.

## Roadmap 2

Currency exchanges are now fully implemented. The flow is as follows:
* Send POST request is sent to the account balance controller.
* Ensure the source and destination accounts exist.
  * One could create the account if it doesn't exist, but that falls out-of-scope of the microservice.
* Get the exchange rate between the source and destination currencies by sending a `getCurrencyExchangeRateCommand` command.
  * Check the Redis cache if the record already exists.
	* If it does, use that.
	* If it does not, fetch it using the `ExchangeRateFixerService` and record the retrieved rate in the database.
	* If a new record is saved in the database, set the expiration time to 30 minutes so the record will automatically expire and require renewal.
* Send a `createCurrencyExchangeCommand` command to execute the exchange.
  * This performs final set validation by checking the accounts and currencies are distinct.
  * If all the data is correct, perform the transaction and persist the updated accountsinto the dataset.
  * Create a receipt of this transaction and record it in the database.
* Send the created receipt back as the response.
