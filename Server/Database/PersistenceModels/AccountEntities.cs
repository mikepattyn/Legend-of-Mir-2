namespace Server.Database.PersistenceModels;

public sealed class AccountEntity
{
    public int Index { get; set; }

    public string AccountId { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public byte[] Salt { get; set; } = Array.Empty<byte>();
    public bool RequirePasswordChange { get; set; }

    public string UserName { get; set; } = string.Empty;
    public DateTime BirthDate { get; set; }
    public string SecretQuestion { get; set; } = string.Empty;
    public string SecretAnswer { get; set; } = string.Empty;
    public string EmailAddress { get; set; } = string.Empty;

    public string CreationIp { get; set; } = string.Empty;
    public DateTime CreationDate { get; set; }

    public bool Banned { get; set; }
    public string BanReason { get; set; } = string.Empty;
    public DateTime ExpiryDate { get; set; }

    public string LastIp { get; set; } = string.Empty;
    public DateTime LastDate { get; set; }

    public bool HasExpandedStorage { get; set; }
    public DateTime ExpandedStorageExpiryDate { get; set; }

    public long Gold { get; set; }   // uint32 in legacy
    public long Credit { get; set; } // uint32 in legacy

    public int StorageSize { get; set; }

    public bool AdminAccount { get; set; }
}

public sealed class AccountStorageItemEntity
{
    public int AccountIndex { get; set; }
    public int SlotIndex { get; set; }
    public ulong UserItemId { get; set; }
}

