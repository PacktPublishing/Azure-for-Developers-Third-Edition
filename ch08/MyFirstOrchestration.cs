using DurableTask.Core;
using DurableTask.Core.Entities;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.DurableTask;
using Microsoft.DurableTask.Client;
using Microsoft.DurableTask.Entities;
using Microsoft.Extensions.Logging;

namespace Afd
{
    public static class MyFirstOrchestration
    {
        [Function(nameof(MyFirstOrchestration))]
        public static async Task<List<string>> RunOrchestrator(
            [OrchestrationTrigger] TaskOrchestrationContext context)
        {
            ILogger logger = context.CreateReplaySafeLogger(nameof(MyFirstOrchestration));
            logger.LogInformation("Saying hello.");
            var outputs = new List<string>();

            // Replace name and input with values relevant for your Durable Functions Activity
            outputs.Add(await context.CallActivityAsync<string>(nameof(SayHello), "Tokyo"));
            outputs.Add(await context.CallActivityAsync<string>(nameof(SayHello), "Seattle"));
            outputs.Add(await context.CallActivityAsync<string>(nameof(SayHello), "London"));

            // returns ["Hello Tokyo!", "Hello Seattle!", "Hello London!"]
            return outputs;
        }

        [Function(nameof(SayHello))]
        public static string SayHello([ActivityTrigger] string name, FunctionContext executionContext)
        {
            ILogger logger = executionContext.GetLogger("SayHello");
            logger.LogInformation("Saying hello to {name}.", name);
            return $"Hello {name}!";
        }

        [Function("MyFirstOrchestration_HttpStart")]
        public static async Task<HttpResponseData> HttpStart(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "/orch/start/{instanceId}")] HttpRequestData req,
            [DurableClient] DurableTaskClient client,
            FunctionContext executionContext,
            string instanceId
            )
        {
            var existingInstance = await client.GetInstanceAsync(instanceId);
            await foreach (var orchestration in client.GetAllInstancesAsync(new OrchestrationQuery(
                DateTimeOffset.Now.AddMinutes(-5), 
                DateTimeOffset.Now, 
                new List<OrchestrationRuntimeStatus> { OrchestrationRuntimeStatus.Running })))
            {
                // Do something
            }

            await client.ScheduleNewOrchestrationInstanceAsync(nameof(MyFirstOrchestration), null, new StartOrchestrationOptions 
            {
                InstanceId = instanceId,
            });

            return await client.CreateCheckStatusResponseAsync(req, instanceId);
        }

        [Function("Store")]
        public static Task DispatchAsync(
            [EntityTrigger] TaskEntityDispatcher dispatcher,
            StoreData data)
        {
            return dispatcher.DispatchAsync(operation =>
            {
                switch (operation.Name.ToLowerInvariant())
                {
                    case "add":
                        operation.State.SetState(data);
                        return new(data);
                    case "reset":
                        operation.State.SetState(new StoreData());
                        break;
                    case "get":
                        return new(operation.State.GetState<StoreData>());
                    case "delete":
                        operation.State.SetState(null);
                        break;
                }

                return default;
            });
        }

        [Function("AddFromQueue")]
        public static Task Run(
            [QueueTrigger("durable-function-trigger")] string input, [DurableClient] DurableTaskClient client)
        {
            var entityId = new EntityInstanceId("Store", "myStore1");
            int amount = int.Parse(input);
            return client.Entities.SignalEntityAsync(entityId, "Add", amount);
        }

        [Function("OrchestrationExample")]
        public static async Task ProvisionNewDevices(
            [OrchestrationTrigger] TaskOrchestrationContext context)
        {
            await context.CallSubOrchestratorAsync("Suborchestration", new {
                userId = 1
            });
        }

        [Function("Suborchestration")]
        public static async Task Suborchestration(
            [OrchestrationTrigger] TaskOrchestrationContext context)
        {
            // Do something
        }
    }

    public class StoreData
    {
    }
}
