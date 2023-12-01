using RawDeal.Data_Structures;
using RawDeal.Reversals;
using RawDealView;
using RawDealView.Formatters;

namespace RawDeal.Effects;

public class ReverseFromHandEffect : Effect
{
    private readonly IViewablePlayInfo _selectedPlay;
    private readonly ImportantPlayerData _opponentData;
    
    public ReverseFromHandEffect(ImportantPlayerData superstarData, ImportantPlayerData opponentData, 
        IViewablePlayInfo selectedPlay, View view) : base(superstarData, view)
    {
        _selectedPlay = selectedPlay;
        _opponentData = opponentData;
    }
    
    public override void Apply()
    {
        var reversalFromHand = new ReversalFromHandController(PlayerData, _opponentData, _selectedPlay, View);
        if (reversalFromHand.CheckPreconditions())
            reversalFromHand.SelectReversalFromHand();
    }
}