using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json;
using InventorySystem.Models;
using System.Net.Http.Formatting;
using System.Collections.Specialized;

namespace InventorySystem.Controllers
{
    public class InventoryController : ApiController
    {
        private InventoryDBContext _dbContext = new InventoryDBContext();

        [AcceptVerbs("GET")]
        public string Get()
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            result.Add("ExpiredItems", Inventory.GetExpiredItems(_dbContext));
            result.Add("TakenItems", Inventory.GetTakenItems(_dbContext));
            result.Add("Inventory", _dbContext.Inventory.Where(item => item.IsDeleted == false).ToList());
            return JsonConvert.SerializeObject(result);
        }

        [AcceptVerbs("GET")]
        [ActionName("Get")]
        public string Get(int id)
        {
            try
            {
                Inventory item = _dbContext.Inventory.First(l => l.ID == id && l.IsDeleted == false);
                if (item != null)
                {
                    return JsonConvert.SerializeObject(item);    
                }
                else
                {
                    return JsonConvert.SerializeObject("Item not found.");
                }
            }
            catch
            {
                Request.CreateErrorResponse(HttpStatusCode.BadRequest, new Exception("Item not found."));
                return JsonConvert.SerializeObject("Bad Request - Item Not Found.");
            }
        }

        // POST api/values
        [AcceptVerbs("POST")]
        public string Post(FormDataCollection postData)
        {
            object[] result = new object[2];    
            try
            {
                object modelResult = Inventory.ValidateItem(postData.Get("label"), postData.Get("expiration_date"), postData.Get("type"));
                if (modelResult is string)
                {
                    result[0] = "Invalid Request";
                    result[1] = modelResult;
                    return JsonConvert.SerializeObject(result);
                }
                else
                {
                    _dbContext.Inventory.Add((Inventory)modelResult);
                    _dbContext.SaveChanges();
                    Request.CreateResponse(HttpStatusCode.Created, modelResult);
                    result[0] = "Item Created";
                    result[1] = modelResult;
                    return JsonConvert.SerializeObject(result);
                }
            }
            catch
            {
                Request.CreateErrorResponse(HttpStatusCode.BadRequest, new Exception("Data type conversion error occurred."));
                result[0] = "Bad Request";
                result[1] = "Data type conversion error.";
                return JsonConvert.SerializeObject(result);
            }
            
        }

        [AcceptVerbs("GET")]
        [ActionName("take")]
        public string Take(string label)
        {
            object[] result = new object[2];
                
            try
            {
                Inventory item = _dbContext.Inventory.Where(l => l.Label == label && l.IsDeleted == false).First();
                if (item != null)
                {
                    item.IsDeleted = true;
                    _dbContext.Inventory.Attach(item);
                    _dbContext.Entry(item).State = System.Data.EntityState.Modified;
                    _dbContext.SaveChanges();
                    result[0] = "Item taken from inventory.";
                    result[1] = item;
                }
                else
                {
                    result[0] = "Item not found.";
                    result[1] = "Unable to find item '" + label + "'";
                }

                return JsonConvert.SerializeObject(result);
            }
            catch
            {
                Request.CreateErrorResponse(HttpStatusCode.BadRequest, new Exception("Data type conversion error occurred."));
                result[0] = "Bad Request";
                result[1] = "Item not found.";
                return JsonConvert.SerializeObject(result);
            }
        }
    }
}
