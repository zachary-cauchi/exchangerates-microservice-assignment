## Objectives:

### Roadmap 0:

* [ ] Get a barebones 'Hello World' API running.
* [ ] Get a working database set up.
* [ ] Create a table in that database with dummy data.
* [ ] Return data from that database.
* [ ] Set up a logging scheme for these APIs.

### Roadmap 1:

* [ ] Obtain an API key to either fixer.io or exchangeratesapi.io.
* [ ] Implement a service that fetches a currency rate from one of the chosen APIs.
* [ ] Save that information to a database.
* [ ] Create an endpoint to get currency rates from the database and fetch that record if it's less than 30 minutes old.
* [ ] If the currency record 30 minutes old or older, update it with a new rate record instead.
* [ ] Develop unit tests for these services.

### Roadmap 2:

* [ ] Implement an identity setup to allow existing users to sign in.
* [ ] Allow the user to perform currency exchanges using the currency API and record them to the database.
* [ ] Develop unit tests for these services.
