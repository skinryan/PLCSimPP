Startup (Client):
  1) Client thread calls AddCommMessageConverterWithCallback() for each message 
     that needs to be converted and processed.
  2) Client thread calls ConnectToServer(), which...
     a) Connects to the server.
     b) Creates the network stream used for receiving/transmitting bytes.
     c) Calls StartReceivingThread() to start a receiving thread to continuous 
        receive bytes.
Startup (Server):
  1) Server thread calls ListenForClient(), which...
     a) Creates a new temporary thread to listen for a client connection.
     b) Returns back to the caller.
     c) Note that a callback delegate is given to ListenForClient() for use 
        when the client connection completes.
  2) When a client connects, the temporary thread...
     a) Creates the network stream used for receiving/transmitting bytes.
     b) Calls StartReceivingThread() to start a receiving thread to continuous 
        receive bytes.
     c) Invokes the callback to perform the desired processing.

Send:
  1) Caller thread calls Send(), which will enqueue the bytes to the 
     mSendQueueCommandDispatcher.
  2) The mSendQueueCommandDispatcher thread dequeues and transmits the bytes.

Receive:
  1) The receiving thread receives bytes, then calls ConvertToMessageAndNotify(), 
     which calls each message converter/callback object to convert the bytes to 
     a message.  If a message can be created, the message is enqueued to 
     mReceiveQueueCommandDispatcher.
  2) The mReceiveQueueCommandDispatcher thread dequeues a message and calls back 
     to the client to process the message.

Shutdown:
  1) Client thread calls Disconnect(), which...
     a) Stops the receiving thread.
     b) Closes the network stream used for receiving/transmitting bytes.
     c) Shuts down mSendQueueCommandDispatcher.
     d) Shuts down mReceiveQueueCommandDispatcher.
