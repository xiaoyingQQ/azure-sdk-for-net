﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Text;
using System.Threading.Tasks;
using Azure.Messaging.EventHubs.Samples.Infrastructure;

namespace Azure.Messaging.EventHubs.Samples
{
    /// <summary>
    ///   An introduction to publishing events, using a simple <see cref="EventHubProducer" />.
    /// </summary>
    ///
    public class Sample3_PublishAnEvent : IEventHubsSample
    {
        /// <summary>
        ///   The name of the sample.
        /// </summary>
        ///
        public string Name { get; } = nameof(Sample3_PublishAnEvent);

        /// <summary>
        ///   A short description of the sample.
        /// </summary>
        ///
        public string Description { get; } = "An introduction to publishing events, using a simple Event Hub producer.";

        /// <summary>
        ///   Runs the sample using the specified Event Hubs connection information.
        /// </summary>
        ///
        /// <param name="connectionString">The connection string for the Event Hubs namespace that the sample should target.</param>
        /// <param name="eventHubName">The name of the Event Hub, sometimes known as its path, that she sample should run against.</param>
        ///
        public async Task RunAsync(string connectionString,
                                   string eventHubName)
        {
            // To create an Event Hub producer, a client is needed.  We will start by creating a client with
            // the default set of options.
            //
            // Using our client, we will then create a producer.  Like the client, our Event Hub producer also manages resources
            // and should be explicitly closed or disposed, but it is not necessary to do both.  In our case, we will take
            // advantage of the new asynchonous dispose to ensure that we clean up our client and producer when we are
            // done or when an exception is encountered.

            await using (var client = new EventHubClient(connectionString, eventHubName))
            await using (EventHubProducer producer = client.CreateProducer())
            {
                // By default, an Event Hub producer is not associated with any specific partition.  When publishing events,
                // it will allow the Event Hubs service to route the event to an available partition.
                //
                // Allowing automatic routing of partitions is recommended when:
                //  - The publishing of events needs to be highly available.
                //  - The event data should be evenly distributed among all available partitions.


                // An event is represented by an arbitrary collection of bytes and metadata.  Event Hubs does not make any
                // assumptions about the data nor attempt to perform any operations on it; you are free to create the data
                // in whatever form makes sense for your scenario.
                //
                // In our case, we will translate a simple sentence into bytes and send it to our Event Hub.

                var eventData = new EventData(Encoding.UTF8.GetBytes("Hello, Event Hubs!"));

                // When the producer sends the event, it will receive an acknowedgement from the Event Hubs service; so
                // long as there is no exception thrown by this call, the service is now responsible for delivery.  Your
                // event data will be published to one of the Event Hub partitions, though there may be a (very) slight
                // delay until it is available to be consumed.

                await producer.SendAsync(eventData);

                Console.WriteLine("The event has been published.");
            }

            // At this point, our client and producer have passed their "using" scope and have safely been disposed of.  We
            // have no further obligations.

            Console.WriteLine();
        }
    }
}
