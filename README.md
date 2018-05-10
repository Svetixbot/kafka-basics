# kafka-basics

## Vocabulary:

### Topics
 - Messages are pushed to topic.
 - Messages are immutable.
 - Order is only guaranteed within partition.

### Partitions
 - Each topic gets assigned a number of partitions.
 - All the data will be splitted into these partitions, randomly unless there is a message key.

### Offset
It is an index of a message within partition.

### Replication factor
 - amount of replicas per partition per topic.

### Leader for a partition
 - ZooKeeper perfoms an election of a leader of a partition (picking which partition will be a leader)
 - Only leader partition accepts writes and reads, others are just replicating the data.

### Brokers
nodes of kafka

### ZooKeeper
  - Configuration manager for the kafka cluster. keeps list of active brokers, and notifies cluster memebers of event in the config (broker goes up/down), election of topic leaders, topic creation/deleteion

---
## Producers

### Writing
- need to provide a name of a topic and 1 broker-id, it will connect a producer/consumer to the whole cluster
- partition is randomly assigned, unless key is provided.
- **once message is sent to a topic, it is immutable**

### Acknowledgement
 - `0` - don't wait for a acknowledgement from kafka, just shoot a message. This is the fastest way to push data into kafka.
    > Note: **this option is unsafe** as there is no guarantee the message will be written to the topic, and is suitable to the usecase (f.e. real-time log)

 - `1` - receive acknowledgement once written to a leader of partition
    > Note: this is safer than `acks=0` but still might result in data-loss of the leader crashes before the replicas get the data

 - `all` - receive acknowledgement once written to a leader of partition and all replicast
    > Note: this is the safest option. should use for transactional data

## Guarantees for ordering
 - message will be appended in order for topic -> partition (via offset attribute)
 - reads will be perfomed in order for topic -> partition
 - with topic replicaiton of N, it is safe to fail N-1 partitions.
   > Important! for messages to be ordered:
   When writing a message, there is an option to include a key.
   Kafka will write messages with the same key to the same partition, as long as the number of partitions hasn't changed

   > (Adding partitions after writing to a topic will break the order(probably mapping of key->partition id is a hash function the will recalculate after such configuration changes)

----
## Consumers
  - Data is read from a topic in parallel from the partitions
  - Consumers are organized in groups, to enhance parallelism (each consumer will read from 1 or more paritions). We cannot have more consumers than partition (these consumers will do nothing)
  - consumer last read position is stored in a kafka topic called  `'__consumer_offsets'`
  - after consumer has read the data, it will commit the offset to kafka. if the consumer dies, it can pick up from where it last committed

## Delivery semantics for consumers:
### At most once
commit offset immidiatly after receiving a message:
**potential loss of data** (if processing failes)

### At least once (**common practice**)
commit offset after the message has been processed.

a chance of recieving duplicated messages, consumer has to be idempotent

### Exactly once
Very hard to achieve. required lots of engineering effort, and likely to not be perfect. bottom line: **out of reach. do not attempt**.

---

## Questions to cover

### Kafka Connect
 - Seems like can be useful in the enterprise to quickly reuse code to push data into and read from kafka

### Practices for making idempotent consumers:
 - upsert over insert
 - keeping track of received messages?


### Practices for making sure the order of messages:
 - constant amount of partitions?
 - key for a message

### Understanding of optimal number of partitions
### Understanding of comsumers group
