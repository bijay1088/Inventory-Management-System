namespace IMS.Data;

public class Withdraw
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string ItemName { get; set; }
    public int Quantity { get; set; }
    public string StaffName { get; set; }

    public Guid ItemId { get; set; }

    public string ApprovedBy { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;
}


