using RawDeal.Data_Structures;
using RawDeal.Effects;

namespace RawDeal.Superstars;

public class TheRock : Player
{
    private bool _abilityResponse;
    private const int CardsToRecover = 1;
    
    public TheRock(SuperstarData superstarData) : base(superstarData) { }
    
    public override void PlaySpecialAbility()
    {
        if (!PlayerDecksController.CheckForEmptyRingside())
        {
            _abilityResponse = View.DoesPlayerWantToUseHisAbility(SuperstarData.Name);
            ExecuteTheRockAbility();
        }
    }

    private void ExecuteTheRockAbility()
    {
        if (_abilityResponse)
        {
            View.SayThatPlayerIsGoingToUseHisAbility(SuperstarData.Name, SuperstarData.SuperstarAbility);
            var importantPlayerData = BuildImportantPlayerData();
            var recoverEffect = new RecoverEffect(importantPlayerData, View, CardsToRecover);
            recoverEffect.Apply();
        }
    }
}