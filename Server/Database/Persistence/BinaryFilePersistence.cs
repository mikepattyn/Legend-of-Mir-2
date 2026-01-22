using Server.MirEnvir;

namespace Server.Database.Persistence;

public sealed class BinaryFilePersistence : IMirPersistence
{
    public void Initialize()
    {
        // Binary-file persistence requires no initialization step.
    }

    public void LoadServerData(Envir envir)
    {
        envir.LoadDB();
    }

    public void LoadUserData(Envir envir)
    {
        envir.LoadAccounts();
        envir.LoadGuilds();
        envir.LoadConquests();
        // NPC used-goods are loaded by NPC scripts (.msd) as part of script loading.
    }

    public void SaveAll(Envir envir, bool forced)
    {
        envir.SaveDB();

        if (forced)
            envir.SaveAccounts();
        else
            envir.BeginSaveAccounts();

        envir.SaveGuilds(forced);
        envir.SaveConquests(forced);
        envir.SaveGoods(forced);
    }
}

