# kafka-basics

## Delivery semantics for consumers:

 - At most once
commit offset immidiatly after receiving a message
potential loss of data  
 - At least once
commit offiset after the message has been processed
a chance of recieving duplicated messages, consumer has to be idempotent

#### Making idempotent consumers:
  - upsert over insert
  - keeping track of received messages

 - Exactly once
out of reach. do not attempt.
 
