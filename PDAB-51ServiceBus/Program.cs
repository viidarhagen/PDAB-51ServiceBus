using Microsoft.Azure.ServiceBus;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PDAB_51ServiceBus
{
    class Program
    {
        //Must be updated to reflect your service
        const string ServiceBusConnectionString = "<<Your Service Bus Connection String>>";
        const string QueueName = "salesmessages";

        const int numberOfMessages = 10;
        static IQueueClient queueClient;

        static void Main(string[] args)
        {
            MainAsync(args).GetAwaiter().GetResult();
        }

       
        static Task ExceptionReceivedHandler(ExceptionReceivedEventArgs exceptionReceivedEventArgs)
        {
            Console.WriteLine($"Message handler encountered an exception {exceptionReceivedEventArgs.Exception}.");
            return Task.CompletedTask;
        }




        // Sends messages to the queue.
        static async Task SendMessagesAsync(int numberOfMessagesToSend, string messageText)
        {
            for (var i = 0; i < numberOfMessagesToSend; i++)
            {
                try
                {
                    // Create a new message to send to the queue
                    string messageBody = $" {i}";
                    var message = new Message(Encoding.UTF8.GetBytes(messageBody));

                    // Write the body of the message to the console
                    Console.WriteLine($"Sending " + messageText + ": {messageBody}");

                    // Send the message to the queue
                    await queueClient.SendAsync(message);
                }
                catch (Exception exception)
                {
                    Console.WriteLine($"{DateTime.Now} :: Exception: {exception.Message}");
                }
            }
        }


        static async Task MainAsync(string[] args)
        {
            queueClient = new QueueClient(ServiceBusConnectionString, QueueName);

            // Register QueueClient's MessageHandler and receive messages in a loop
            //RegisterOnMessageHandlerAndReceiveMessages();

            // Send Messages
            await SendMessagesAsync(numberOfMessages,"Sales message");

            Console.WriteLine("Press any key to exit after receiving all the messages.");
            Console.ReadKey();

            await queueClient.CloseAsync();
        }



        //static async Task ProcessMessagesAsync(Message message, CancellationToken token)
        //{
        //    // Process the message
        //    Console.WriteLine($"Received message: SequenceNumber:{message.SystemProperties.SequenceNumber} Body:{Encoding.UTF8.GetString(message.Body)}");

        //    // Complete the message so that it is not received again.
        //    // This can be done only if the queueClient is opened in ReceiveMode.PeekLock mode (which is default).
        //    await queueClient.CompleteAsync(message.SystemProperties.LockToken);
        //}

        //static void RegisterOnMessageHandlerAndReceiveMessages()
        //{
        //    // Configure the MessageHandler Options in terms of exception handling, number of concurrent messages to deliver etc.
        //    var messageHandlerOptions = new MessageHandlerOptions(ExceptionReceivedHandler)
        //    {
        //        // Maximum number of Concurrent calls to the callback `ProcessMessagesAsync`, set to 1 for simplicity.
        //        // Set it according to how many messages the application wants to process in parallel.
        //        MaxConcurrentCalls = 1,

        //        // Indicates whether MessagePump should automatically complete the messages after returning from User Callback.
        //        // False value below indicates the Complete will be handled by the User Callback as seen in `ProcessMessagesAsync`.
        //        AutoComplete = false
        //    };

        //    // Register the function that will process messages
        //    queueClient.RegisterMessageHandler(ProcessMessagesAsync, messageHandlerOptions);
        //}

    }
}
