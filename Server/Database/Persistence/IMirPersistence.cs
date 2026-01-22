using Server.MirEnvir;

namespace Server.Database.Persistence;

public interface IMirPersistence
{
    void Initialize();

    void LoadServerData(Envir envir);
    void LoadUserData(Envir envir);

    void SaveAll(Envir envir, bool forced);
}

