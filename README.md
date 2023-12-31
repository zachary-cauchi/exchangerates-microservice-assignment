## Objectives:

### Roadmap 0:

* [x] Get a barebones 'Hello World' API running.
* [x] Get a working database set up.
* [x] Create a table in that database with dummy data.
* [x] Return data from that database.
* [x] Set up a logging scheme for these APIs.

### Roadmap 1:

* [x] Create tables and data structures for users.
* [x] Create tables and data structures for user account balances.
* [x] Create tables and data structures for exchange rates.
* [x] Create tables and data structures for past exchanges.
* [x] Create a controller that gets users.
* [x] Create a controller that gets exchange rates (seeded with dummy data for now).
* [x] Create a controller that gets and creates exchanges.
* [x] Set up a testing strategy.
* [x] Test the business logic behind the currency exchanges.

### Roadmap 2:

* [x] Obtain an API key to either fixer.io or exchangeratesapi.io.
* [x] Implement a service that fetches a currency rate from one of the chosen APIs.
* [x] Save that information to a database.
* [x] Create an endpoint to get currency rates from the database and fetch that record if it's less than 30 minutes old.
* [x] If the currency record is 30 minutes old or older, update it with a new rate record instead.
* [x] Develop unit tests for these services.

### Roadmap 3:

* [ ] Implement an identity setup to allow existing users to sign in.
* [x] Allow the user to perform currency exchanges using the currency API and record them to the database.
* [x] Develop unit tests for these services.


### Roadmap 4:

* [ ] Polish up deployment data support.
* [ ] Fix Seq integration.
