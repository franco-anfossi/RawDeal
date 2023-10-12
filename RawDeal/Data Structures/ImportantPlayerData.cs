using RawDeal.Decks;

namespace RawDeal.Data_Structures;

public class ImportantPlayerData
{
    public string Name;
    public SuperstarData SuperstarData;
    public PlayerDecksController DecksController;
    
    public ImportantPlayerData(SuperstarData superstarData, PlayerDecksController decksController)
    {
        Name = superstarData.Name;
        SuperstarData = superstarData;
        DecksController = decksController;
    }
}