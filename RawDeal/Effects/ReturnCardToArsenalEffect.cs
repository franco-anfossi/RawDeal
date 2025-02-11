using RawDeal.Data_Structures;
using RawDealView;

namespace RawDeal.Effects;

public class ReturnCardToArsenalEffect : Effect
{
    public ReturnCardToArsenalEffect(ImportantPlayerData superstarData, View view) : base(superstarData, view) { }
    
    public override void Apply()
    {
        var formattedCardData = PlayerData.DecksController.BuildFormattedDecks();
        var formattedHand = formattedCardData.Hand;
        int selectedCardIndex = 
            View.AskPlayerToReturnOneCardFromHisHandToHisArsenal(PlayerData.Name, formattedHand.ToList());
        PlayerData.DecksController.PassCardFromHandToTheBackOfTheArsenal(selectedCardIndex);
    }
}