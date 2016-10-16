using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Net.Http.Headers;
using System.Collections.Concurrent;
using System.IO;
using Newtonsoft.Json;
using System.Threading;
using InventorySystem.Models;

namespace InventorySystem.Controllers
{
    public class NotificationsController : ApiController
    {
        //Queue for the push subscriber push streams. 
        private static readonly List<StreamWriter> _streamSubscriberQueue = new List<StreamWriter>();
        private InventoryDBContext _dbContext = new InventoryDBContext();
        
        //Subscribes to the push stream. 
        [AcceptVerbs("GET")]
        public HttpResponseMessage GetNotifications(HttpRequestMessage request)
        {
            HttpResponseMessage response = request.CreateResponse();
            response.Content = new PushStreamContent(OnStreamAvailable, "text/event-stream");
            Thread messageThread = new Thread(new ThreadStart(GetExpiredItems));
            messageThread.Start();
            return response;
        }

        private void GetExpiredItems()
        {
            Thread.Sleep(10000);
            MessageCallback(Inventory.GetExpiredItems(_dbContext, DateTime.Now));
        }

        //Adds a new stream writer instance to the queue.
        public static void OnStreamAvailable(Stream stream, HttpContent content, TransportContext context)
        {
            StreamWriter streamwriter = new StreamWriter(stream);
            _streamSubscriberQueue.Add(streamwriter);
        }

        //Pushes data to the client via push stream. 
        public static void MessageCallback(object message)
        {
            StreamWriter subscriberStream;
            for (int i = _streamSubscriberQueue.Count - 1; i > -1; i-- )
            {
                try
                {
                    subscriberStream = _streamSubscriberQueue[i];
                    subscriberStream.WriteLine("data:" + JsonConvert.SerializeObject(message) + "\n");
                    subscriberStream.Flush();
                }
                catch (Exception exc)
                {
                    _streamSubscriberQueue.RemoveAt(i);
                    Console.Write(exc.Message);
                }
            }
        }
    }
}
