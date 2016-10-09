using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace InventorySystem.Models
{
    public class Inventory
    {
        private bool _IsDeleted = false;

        public int ID { get; set; }
        public string Label { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string Type { get; set; }
        public bool IsDeleted
        {
            get { return _IsDeleted; }
            set { _IsDeleted = value; }
        }

        public static object ValidateItem(string label, string expirationDate, string type)
        {
            List<string> errors = new List<string>();
            if (string.IsNullOrEmpty(label))
                errors.Add("Label cannot be blank.");
            if (string.IsNullOrEmpty(type))
                errors.Add("Type cannot be blank.");

            DateTime date;
            if (!DateTime.TryParse(expirationDate, out date))
                errors.Add("Invalid expiration date.");

            if (errors.Count > 0)
                return String.Join("|", errors);

            Inventory inventoryItem = new Inventory();
            inventoryItem.Label = label;
            inventoryItem.ExpirationDate = date;
            inventoryItem.Type = type;

            return inventoryItem;
        }

        public static List<Inventory> GetExpiredItems(InventoryDBContext dbContext)
        {
            return dbContext.Inventory.Where(item => item.ExpirationDate.CompareTo(DateTime.Now) == -1).ToList();
        }

        public static List<Inventory> GetTakenItems(InventoryDBContext dbContext)
        {
            return dbContext.Inventory.Where(item => item.IsDeleted == true).ToList();
        }
    }

    public class InventoryDBContext : DbContext
    {
        public DbSet<Inventory> Inventory { get; set; }
    }
}