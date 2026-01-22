namespace Server.Database.PersistenceModels;

public sealed class AuctionEntity
{
    public ulong AuctionId { get; set; }

    public ulong UserItemId { get; set; }

    public DateTime ConsignmentDate { get; set; }
    public long Price { get; set; }       // uint32 in legacy
    public long CurrentBid { get; set; }  // uint32 in legacy

    public int SellerIndex { get; set; }
    public int? CurrentBuyerIndex { get; set; }

    public bool Expired { get; set; }
    public bool Sold { get; set; }

    public byte ItemType { get; set; }
}

