#!/bin/bash

## configure debezium connector for customer database

create_connector () {
    curl -X POST http://localhost:8083/connectors -d @configs/customer_config.json \
        --header "Content-Type: application/json"
}

# update_connector () {    
#     curl -X PUT http://localhost:8083/connectors/customer_outbox_connector/config --data "$(jq '.config' configs/customer_config.json)" \
#         --header "Content-Type: application/json"     
# }

delete_connector () {
    curl -X DELETE http://localhost:8083/connectors/customer_outbox_connector  \
        --header "Content-Type: application/json"
}

stop_connector () {
    curl -X PUT http://localhost:8083/connectors/customer_outbox_connector/pause  \
        --header "Content-Type: application/json"
}

start_connector () {
    curl -X PUT http://localhost:8083/connectors/customer_outbox_connector/resume  \
        --header "Content-Type: application/json"
}

"$@"
read