using RawDeal.Decks;

namespace RawDeal.Data_Structures;

public class ImportantPlayerData
{
    public readonly string Name;
    public readonly SuperstarData SuperstarData;
    public readonly PlayerDecksController DecksController;
    public readonly ChangesByJockeyingForPosition ChangesByJockeyingForPosition;
    
    public ImportantPlayerData(SuperstarData superstarData, PlayerDecksController decksController, 
        ChangesByJockeyingForPosition changesByJockeyingForPosition)
    {
        Name = superstarData.Name;
        SuperstarData = superstarData;
        DecksController = decksController;
        ChangesByJockeyingForPosition = changesByJockeyingForPosition;
    }
}