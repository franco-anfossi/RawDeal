using RawDeal.Decks;

namespace RawDeal.Data_Structures;

public class ImportantPlayerData
{
    public readonly string Name;
    public readonly SuperstarData SuperstarData;
    public readonly PlayerDecksController DecksController;
    
    public ImportantPlayerData(SuperstarData superstarData, PlayerDecksController decksController)
    {
        Name = superstarData.Name;
        SuperstarData = superstarData;
        DecksController = decksController;
    }
}