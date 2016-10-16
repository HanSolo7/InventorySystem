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

        //Gets all unexpired items remaining in the inventory.
        [AcceptVerbs("GET")]
        public string Get()
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            result.Add("Inventory", _dbContext.Inventory.Where(item => item.IsDeleted == false).ToList());
            return JsonConvert.SerializeObject(result);
        }

        //Gets an inventory item by id.
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
                return JsonConvert.SerializeObject("Bad Request - Item Not Found.");
            }
        }

        //Adds an item to the inventory.
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
                    Inventory item = (Inventory)modelResult;
                    return JsonConvert.SerializeObject(result);
                }
            }
            catch
            {
                result[0] = "Bad Request";
                result[1] = "Data type conversion error.";
                return JsonConvert.SerializeObject(result);
            }
            
        }

        //Takes an item from the inventory and performs a soft delete on the database record.
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
                    NotificationsController.MessageCallback(string.Format("The item '{0}' (ID:{1}) has been taken from the inventory.", item.Label, item.ID.ToString()));
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
                result[0] = "Bad Request";
                result[1] = "Item not found.";
                return JsonConvert.SerializeObject(result);
            }
        }

        
    }
}
