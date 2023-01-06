
using System.Text.Json;
    
namespace IMS.Data;

public static class ItemService
{
    private static void SaveAll(List<Item> items) // save method to save all items in json file
    {
        string appDataDirectoryPath = Utils.GetAppDirectoryPath();
        string itemFilePath = Utils.GetItemFilePath();

        if (!Directory.Exists(appDataDirectoryPath)) //if directory doesn't exist
        {
            Directory.CreateDirectory(appDataDirectoryPath); //create directory
        }

        var json = JsonSerializer.Serialize(items); // Serialize the object to a JSON string and store in json
        File.WriteAllText(itemFilePath, json);


    }

    public static List<Item> GetAll() // get all items from json file
    {
        string itemFilePath = Utils.GetItemFilePath();
        if (!File.Exists(itemFilePath))
        {
            return new List<Item>();
        }

        var json = File.ReadAllText(itemFilePath);

        return JsonSerializer.Deserialize<List<Item>>(json);
    }

    public static List<Item> Delete(Guid id) // delete item 
    {
        List<Item> items = GetAll();
        Item item = items.FirstOrDefault(x => x.Id == id);

        if (item == null)
        {
            throw new Exception("Item not found.");
        }
        
        items.Remove(item);
        SaveAll(items);
        return items;
    }

    public static List<Item> WithdrawItem(Guid id, string itemname, int quantity) //withdrawing item
    {
        if (quantity <= 0)
        {
            throw new Exception("Quantity must be greater than 0.");
        }
        
        DateTime currentTime = DateTime.Now;
        if (currentTime.Hour >= 9 && currentTime.Hour <= 16 && currentTime.DayOfWeek >= DayOfWeek.Monday && currentTime.DayOfWeek <= DayOfWeek.Friday)
        {
            List<Item> items = GetAll();
            Item itemUpdate = items.FirstOrDefault(x => x.Id == id);

            if (itemUpdate == null)
            {
                throw new Exception("Item not found.");
            }
            itemUpdate.ItemName = itemname;
            itemUpdate.Quantity = itemUpdate.Quantity - quantity;
            itemUpdate.LastTakenOut = currentTime.ToString();
            SaveAll(items);
            return items;
        }
        else
        {
            throw new Exception("You cannot withdraw during this time.");
        }

    }

    
    public static List<Item> SaveItems(string itemname, int quantity)  // saving item in list
    {
        List<Item> item = GetAll();

        if (quantity <= 0)
        {
            throw new Exception("Quantity must be greater than 0.");
        }

        if (itemname == null || itemname == "")
        {
            throw new Exception("Please enter item name.");
        }

        if (item.Any(x => x.ItemName == itemname))
        {
            item = Update(itemname, quantity);
        }
        else{
            item.Add(
            new Item
            {
                ItemName = itemname,
                Quantity = quantity,
                LastTakenOut = null
            }
        );
            SaveAll(item);
            
        }
        return item;


    }

    public static List<Item> Update(string itemname, int quantity)  //updating item in list
    {
        List<Item> items = GetAll();
        Item itemUpdate = items.FirstOrDefault(x => x.ItemName == itemname);
        itemUpdate.Quantity = itemUpdate.Quantity + quantity;
        SaveAll(items);
        return items;


    }
    

}



