This simple project demonstrates how to manage eventual consistency between microservices with Change Data Capture and Outbox Pattern using Debezium, Kafka, and Kafka Connect.

## Prerequisites

* .NET 5.0 SDK
* Docker Desktop

## Run in Debug Mode

* Run 'docker-compose up' and wait for all infra to up and running.
* Select 'All' debug option and start debugging. (for vs code)
* Wait until all microservices are up and running.

**Initiating Databases:** Each service will be created its own database while it's starting for the first time.

## Register Debezium Postgres Connectors to Kafka Connect

You need to register two Postgres Connectors. One for Customer Database and the other for Identity Database. 

<a href="https://github.com/suadev/microservices-change-data-capture-with-debezium/blob/main/_debezium_connectors/configs/customer_config.json">customer_config.json</a> -> Customer Connector Config.

<a href="https://github.com/suadev/microservices-change-data-capture-with-debezium/blob/main/_debezium_connectors/configs/identity_config.json">identity_config.json</a> -> Identity Connector Config.


Use <a href="https://github.com/suadev/microservices-change-data-capture-with-debezium/blob/main/_debezium_connectors/customer.sh">customer.sh</a> and <a href="https://github.com/suadev/microservices-change-data-capture-with-debezium/blob/main/_debezium_connectors/identity.sh">identity.sh</a> to create/update/delete connectors. For instance, to create customer connector;

```bash 
.\customer.sh create_connector
```   

**update_connector** function is commented out. If you want to update the connector config, uncomment the function and download jq from <a href="https://stedolan.github.io/jq/download/">here</a>.

After registeration of two Debezium Connectors, two workers will be created on Kafka Connect which are listening to the outbox tables to push events to Kafka topics.

Check the connector list via the following endpoint and see the following json result to be sure everything is okay.

```bash
curl -X GET http://localhost:8083/connectors 
```

```json
["identity_outbox_connector", "customer_outbox_connector"]
```

Now you are ready to test. See sample postman requests <a href="https://github.com/suadev/microservices-change-data-capture-with-debezium/blob/main/_postman/dev_summit_cdc_debezium.postman_collection.json">here.</a>

## Overall Architecture

When a new user created on Identity Service, eventual consistency will be obtained for Customer and Notification Services as shown following flow.

<img src="https://raw.githubusercontent.com/suadev/microservices-change-data-capture-with-debezium/main/_img/user_and_customer_creation_flow.png" />

## Tool Set

* Asp.Net 5.0
* Entity Framework Core 5.0
* PostgreSQL - Npgsql
* MediatR
* Kafka - Zookeeper
* Confluent.Kafka
* Kafka Connect
* Debezium
* Kafdrop
* Docker - Docker Compose
* Azure Data Studio
* VS Code
