using System.Data;
using System.Text.Json;
    
namespace IMS.Data;

public static class WithdrawService
{
    private static void SaveAll(List<Withdraw> withdraw) //saving withdraw log in json file
    {
        string appDataDirectoryPath = Utils.GetAppDirectoryPath();
        string itemFilePath = Utils.GetItemWithdrawFilePath();

        if (!Directory.Exists(appDataDirectoryPath))
        {
            Directory.CreateDirectory(appDataDirectoryPath);
        }

        var json = JsonSerializer.Serialize(withdraw); // Serialize the object to a JSON string and store in json
        File.WriteAllText(itemFilePath, json);


    }

    public static List<Withdraw> GetAll() //getting all withdraw log from json 
    {
        string itemFilePath = Utils.GetItemWithdrawFilePath();
        if (!File.Exists(itemFilePath))
        {
            return new List<Withdraw>();
        }

        var json = File.ReadAllText(itemFilePath);

        return JsonSerializer.Deserialize<List<Withdraw>>(json);
    }


    public static List<Withdraw> SaveLog(string username, Guid itemID, string itemname, int quantity, string staff) //saving withdraw log to list
    {
        
        List<Withdraw> withdraw = GetAll();

        withdraw.Add(
            new Withdraw
            {
                ApprovedBy = username,
                ItemId = itemID,
                StaffName = staff,
                ItemName = itemname,
                Quantity = quantity
            }
        );
        SaveAll(withdraw);
        return withdraw;
    }
    

}



