using RawDeal.Decks;

namespace RawDeal.Data_Structures;

public class ImportantPlayerData
{
    public readonly string Name;
    public readonly SuperstarData SuperstarData;
    public readonly PlayerDecksController DecksController;
    public readonly BonusSet BonusSet;
    public LastCardUsed LastCardUsed;
    
    public ImportantPlayerData(SuperstarData superstarData, PlayerDecksController decksController, 
        BonusSet bonusSet)
    {
        Name = superstarData.Name;
        SuperstarData = superstarData;
        DecksController = decksController;
        BonusSet = bonusSet;
    }
}