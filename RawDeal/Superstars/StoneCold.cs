using RawDeal.Data_Structures;
using RawDeal.Effects;

namespace RawDeal.Superstars;

public class StoneCold : Player
{
    private bool _abilityPermission;
    private int _timesTheAbilityWasUsed;
    public StoneCold(SuperstarData superstarData) : base(superstarData)
    {
        SuperstarData = superstarData;
    }
    
    public override bool PlaySpecialAbility()
    {
        if (_timesTheAbilityWasUsed < 1 && DecksInfo.Arsenal.Count >= 0)
        {
            ExecuteAbilitySteps();
            _timesTheAbilityWasUsed++;
            _abilityPermission = true;
        }
        return true;
    }
    public override bool VerifyAbilityUsability()
    {
        return _abilityPermission;
    }
    public override void ChangeAbilitySelectionVisibility()
    {
        if (DecksInfo.Arsenal.Count > 0)  
        {
            _timesTheAbilityWasUsed = 0;
            _abilityPermission = false;
        }
        
    }

    private void ExecuteAbilitySteps()
    {
        View.SayThatPlayerIsGoingToUseHisAbility(SuperstarData.Name, SuperstarData.SuperstarAbility);
        View.SayThatPlayerDrawCards(SuperstarData.Name, 1);
        PlayerDecksController.DrawTurnCard();
        var importantPlayerData = BuildImportantPlayerData();
        var returnCardEffect = new ReturnCardToArsenalEffect(importantPlayerData, View);
        returnCardEffect.Apply();
    }
}