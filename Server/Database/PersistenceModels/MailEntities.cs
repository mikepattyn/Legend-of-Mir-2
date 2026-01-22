namespace Server.Database.PersistenceModels;

public sealed class MailEntity
{
    public ulong MailId { get; set; }

    public string Sender { get; set; } = string.Empty;
    public int RecipientIndex { get; set; }

    public string Message { get; set; } = string.Empty;
    public long Gold { get; set; } // uint32 in legacy

    public DateTime DateSent { get; set; }
    public DateTime DateOpened { get; set; }

    public bool Locked { get; set; }
    public bool Collected { get; set; }
    public bool CanReply { get; set; }
}

public sealed class MailItemEntity
{
    public ulong MailId { get; set; }
    public int SlotIndex { get; set; }
    public ulong UserItemId { get; set; }
}

