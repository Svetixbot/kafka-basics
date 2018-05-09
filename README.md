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

===

## Producers

### Writing
- need to provide a name of a topic and 1 broker-id, it will connect a producer/consumer to the whole cluster
- partition is randomly assigned, unless key is provided.

### Acknowlidgment
 - `0` - don't wait for a acknowlidgemnt from kafka, just shoot a message.
 - `1` - receive acknowlidgemnt once written to a leader of partition
 - `all` - receive acknowlidgemnt once written to a leader of partition and replicated across all partitions.

## Guarantees for ordering
 - messaged will be appended in order for topic-partition
 - reads will be perfomed in order for topic-partition
 - with topic replicaiton of N, it is safe to fail N-1 partitions.
 
```
Important! guarantee for messages to be ordered: 
Kafka will write messages with the same key to the same partition, as long as the number of partitions is constant. 
Adding partitions after writing to a topic will break the order.
```
 
## Delivery semantics for consumers:
### At most once
 
commit offset immidiatly after receiving a message
potential loss of data  

### At least once (`common practice`)
 
commit offiset after the message has been processed
a chance of recieving duplicated messages, consumer has to be idempotent

### Exactly once
 
out of reach. do not attempt.
 
 ===
 
## Questions to cover
 
### Practices for making idempotent consumers:
 - upsert over insert
 - keeping track of received messages?


### Practices for making sure the order of messages:
 - constant amount of partitions
 - key for a message
  
### Understanding of optimal number of partitions
### Understanding of comsumers group
