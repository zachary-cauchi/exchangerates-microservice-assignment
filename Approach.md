# Approach

Based on the 'eShopOnContainers' microservices example provided by Microsoft.

## Roadmap 0

Implemented a basic MSSQL database setup with migrations and data-seeding.
The server is minimally set up for now but will incorporate better logging functionality soon.
The server and database are both hosted in docker containers and created using a docker-compose file.
Added Serilog logging to the program but could not get Seq working for log persistence and metrics support.
Will resolve it as it is an optional objective at this stage.
