using RawDeal.Data_Structures;
using RawDeal.Effects;

namespace RawDeal.Superstars;

public class StoneCold : Player
{
    private int _timesTheAbilityWasUsed;
    private const int CardsToDraw = 1;
    private const int MaxTimesTheAbilityCanBeUsed = 1;
    
    public StoneCold(SuperstarData superstarData) : base(superstarData) { }
    
    public override void PlaySpecialAbility()
    {
        if (CanUseAbility())
            ExecuteAbilitySteps();
    }
    
    private void ExecuteAbilitySteps()
    {
        View.SayThatPlayerIsGoingToUseHisAbility(SuperstarData.Name, SuperstarData.SuperstarAbility);
        View.SayThatPlayerDrawCards(SuperstarData.Name, CardsToDraw);
        ApplyAbilityEffects();
        _timesTheAbilityWasUsed++;
    }
    
    private void ApplyAbilityEffects()
    {
        PlayerDecksController.DrawTurnCard();
        var playerData = BuildImportantPlayerData();
        var returnCardEffect = new ReturnCardToArsenalEffect(playerData, View);
        returnCardEffect.Apply();
    }
    
    public override bool VerifyAbilityUsability()
        => !CanUseAbility();
    
    private bool CanUseAbility()
        => !CheckForEmptyArsenal() && DontUseMoreThanMaxTimes();
    
    private bool DontUseMoreThanMaxTimes()
        => _timesTheAbilityWasUsed < MaxTimesTheAbilityCanBeUsed;
    
    public override void ResetAbility()
    {
        if (!CheckForEmptyArsenal())  
            _timesTheAbilityWasUsed = 0;
    }
    
    private bool CheckForEmptyArsenal()
        => PlayerDecksController.CheckForEmptyArsenal();
}