# Running Kafka

## Pre requisistes
> tested on MacOS
* Docker > 1.12

## How to run

* run `start_kafka.sh` - which does this:
```shell
docker run --rm -it -p 2181:2181 -p 3030:3030 -p 8081:8081 -p 8082:8082 -p 8083:8083 -p 9092:9092 -e ADV_HOST=127.0.0.1 --name fast-data-dev landoop/fast-data-dev
```

* After the images has been pulled it should be accessible at `http://127.0.0.1:3030`
* in order to connect to the container and exectue kafka commands etc, use: `docker exec -it fast-data-dev bash`

* some aliases:

````
alias ktopic='kafka-topics --zookeeper 127.0.0.1:2181'
alias kproducer='kafka-console-producer --broker-list 127.0.0.1:9092'
alias kconsumer="kafka-console-consumer --bootstrap-server 127.0.0.1:9092"
````

## Topics
base command: `kafka-topics --zookeeper 127.0.0.1:2181`

````
* list all topics  : --list
* create a topic   : --create --topic <topic name> --partitions <# of partitions> --replication-factor <# of replicas>
* delete a topic   : --delete --topic <topic name>
* describe a topic : --describe --topic <topic name>
````

## Producer
base command: `kafka-console-producer --broker-list 127.0.0.1:9092`


In order to generate data run: `base-command --topic <topic name>` and hit enter, than type in the console whatever, each line is a new message. CTRL+C breaks. if `<topic name>` doesn't exists, one should be created with the default configuration (number of partitions, number of replicas etc..)

## Consumer

base command: `kafka-console-consumer --bootstrap-server 127.0.0.1:9092`

Usage: `base-command --topic <topic name>`
Will open a connection to kafka and will wait on incoming messages for the speicifed topic.

Specify `--from-beginning` to replay all messages in the topic

Specify `--consumer-propery group.id=<group-name>` to specify a consumer group id, which will persist the consumer offset (and will be used for incremental reads when consumer is invoked)
