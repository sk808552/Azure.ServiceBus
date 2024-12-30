using Azure.Messaging.ServiceBus;
using System.Diagnostics;

var connectionString = "";
var queueName = "az-queue-pras";

ServiceBusClient serviceBusClient = new ServiceBusClient(connectionString);
ServiceBusProcessor serviceBusProcessor = serviceBusClient.CreateProcessor(queueName);

async Task MessageHandler(ProcessMessageEventArgs processMessageEventArgs)
{
    await processMessageEventArgs.CompleteMessageAsync(processMessageEventArgs.Message);
}
Task ErrorHandler(ProcessErrorEventArgs processErrorEventArgs)
{
    Console.WriteLine(processErrorEventArgs.Exception.ToString());
    return Task.CompletedTask;
}


try
{
    serviceBusProcessor.ProcessMessageAsync += MessageHandler;
    serviceBusProcessor.ProcessErrorAsync += ErrorHandler;

    await serviceBusProcessor.StartProcessingAsync();
    Console.WriteLine("Press any key to end the processing");
    Console.ReadKey();

    Console.WriteLine("\nStopping the receiver...");
    await serviceBusProcessor.StopProcessingAsync();
    Console.WriteLine("Stopped receiving messages");
}
catch (Exception ex)
{
    Console.WriteLine(ex.ToString());
}
finally
{
    await serviceBusClient.DisposeAsync();
    await serviceBusProcessor.DisposeAsync();
}

