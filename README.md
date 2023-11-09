## Objectives:

### Roadmap 0:

* [x] Get a barebones 'Hello World' API running.
* [x] Get a working database set up.
* [x] Create a table in that database with dummy data.
* [x] Return data from that database.
* [x] Set up a logging scheme for these APIs.

### Roadmap 1:

* [x] Create tables and data structures for users.
* [ ] Create tables and data structures for user account balances.
* [ ] Create tables and data structures for exchange rates.
* [ ] Create tables and data structures for past exchanges.
* [x] Create a controller that gets users.
* [ ] Create a controller that gets exchange rates (seeded with dummy data for now).
* [ ] Create a controller that gets and creates exchanges.
* [ ] Set up a testing strategy.
* [ ] Test the business logic behind the currency exchanges.

### Roadmap 2:

* [ ] Obtain an API key to either fixer.io or exchangeratesapi.io.
* [ ] Implement a service that fetches a currency rate from one of the chosen APIs.
* [ ] Save that information to a database.
* [ ] Create an endpoint to get currency rates from the database and fetch that record if it's less than 30 minutes old.
* [ ] If the currency record 30 minutes old or older, update it with a new rate record instead.
* [ ] Develop unit tests for these services.

### Roadmap 3:

* [ ] Implement an identity setup to allow existing users to sign in.
* [ ] Allow the user to perform currency exchanges using the currency API and record them to the database.
* [ ] Develop unit tests for these services.


### Roadmap 4:

* [ ] Polish up deployment data support.
* [ ] Fix Seq integration.