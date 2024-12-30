using Azure.Messaging.ServiceBus;

var connectionString = "";
var queueName = "az-queue-pras";
var maxNoMsg = 1;

ServiceBusClient serviceBusClient = new ServiceBusClient(connectionString);
ServiceBusSender serviceBusSender = serviceBusClient.CreateSender(queueName);
ServiceBusMessageBatch batch = await serviceBusSender.CreateMessageBatchAsync();

for (int i = 0; i < maxNoMsg; i++)
{
    if (!batch.TryAddMessage(new ServiceBusMessage("This is message no " + i)))
    {
        Console.WriteLine("Message " + i + " didn't added to the batch");
    }
}

try
{
    await serviceBusSender.SendMessageAsync(new ServiceBusMessage(batch.ToString()));
    Console.WriteLine("Messages Sent Successfully");
}
catch (Exception ex)
{
    Console.WriteLine(ex.ToString());
}
finally
{
    await serviceBusClient.DisposeAsync();
    await serviceBusSender.DisposeAsync();
}
