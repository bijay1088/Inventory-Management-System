
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace IMS.Data;

public class Item
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string ItemName { get; set; }
    public int Quantity { get; set; }

    public string LastTakenOut { get; set; }
}


