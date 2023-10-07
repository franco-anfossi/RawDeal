using RawDealView;

namespace RawDeal;

public class PlayerInfoManager
{
    private readonly PlayerInfo _inTurnPlayerInfo;
    private readonly PlayerInfo _opponentPlayerInfo;
    private readonly View _view;
    
    public PlayerInfoManager(PlayerInfo inTurnPlayerInfo, PlayerInfo opponentPlayerInfo, View view)
    {
        _view = view;
        _inTurnPlayerInfo = inTurnPlayerInfo;
        _opponentPlayerInfo = opponentPlayerInfo;
    }
    
    public void ShowPlayerInfo()
    {
        _view.ShowGameInfo(_inTurnPlayerInfo, _opponentPlayerInfo);
    }
}