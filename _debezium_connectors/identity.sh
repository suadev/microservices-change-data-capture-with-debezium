#!/bin/bash

## configure debezium connector for identity database

create_connector () {
    curl -X POST http://localhost:8083/connectors -d @configs/identity_config.json \
        --header "Content-Type: application/json"
}

# update_connector () {  
#     curl -X PUT http://localhost:8083/connectors/identity_outbox_connector/config --data "$(jq '.config' configs/identity_config.json)" \
#         --header "Content-Type: application/json"        
# }

delete_connector () {
    curl -X DELETE http://localhost:8083/connectors/identity_outbox_connector  \
        --header "Content-Type: application/json"
}

stop_connector () {
    curl -X PUT http://localhost:8083/connectors/identity_outbox_connector/pause  \
        --header "Content-Type: application/json"
}

start_connector () {
    curl -X PUT http://localhost:8083/connectors/identity_outbox_connector/resume  \
        --header "Content-Type: application/json"
}

"$@"
read