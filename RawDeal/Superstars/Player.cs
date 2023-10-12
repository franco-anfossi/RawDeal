using RawDeal.Data_Structures;
using RawDeal.Decks;
using RawDealView;
using RawDealView.Formatters;

namespace RawDeal.Superstars;

public abstract class Player
{
    protected View View;
    protected DecksInfo DecksInfo;
    private PlayerInfo _playerInfo;
    protected SuperstarData SuperstarData;
    protected ImportantPlayerData OpponentData;
    protected PlayerDecksController PlayerDecksController;

    protected Player(SuperstarData superstarData)
    {
        SuperstarData = superstarData;
    }

    public abstract bool PlaySpecialAbility();
    
    public virtual bool VerifyAbilityUsability()
    {
        return true;
    }

    public virtual void ChangeAbilitySelectionVisibility()
    {
    }

    public void ShowOptionsToViewDecks()
    {
        var playerDecks = BuildFormattedDecks();
        var opponentDecks = OpponentData.DecksController.BuildFormattedDecks();
        var optionsToViewDeck = new OptionsToViewDeck(playerDecks, opponentDecks, View);
        optionsToViewDeck.SelectWhatDeckToView();
    }
    
    private FormattedDecksInfo BuildFormattedDecks()
    {
        return PlayerDecksController.BuildFormattedDecks();
    }
    public void ApplyNecessaryAttributes(View view, ImportantPlayerData opponent)
    {
        View = view;
        OpponentData = opponent;
    }
    
    public string GetLogo()
    {
        return SuperstarData.Logo;
    }
 
    public int GetSuperstarValue()
    {
        return SuperstarData.SuperstarValue;
    }

    public void BuildDeckInfo(List<IViewableCardInfo> arsenalDeck)
    {
        DecksInfo = new DecksInfo(arsenalDeck);
    }
    
    public virtual PlayerDecksController BuildPlayerDecksController()
    {
        PlayerDecksController = new PlayerDecksController(DecksInfo, SuperstarData);
        return PlayerDecksController;
    }
    
    public PlayerInfo BuildPlayerInfo()
    {
        var (handCount, arsenalCount) = PlayerDecksController.ShowUpdatedDeckCounts();
        _playerInfo = new PlayerInfo(SuperstarData.Name, SuperstarData.Fortitude, handCount, arsenalCount);
        return _playerInfo;
    }
    
    public ImportantPlayerData BuildImportantPlayerData()
    {
        return new ImportantPlayerData(SuperstarData, PlayerDecksController);
    } 

    public object Clone()
    {
        var clonedPlayer = (Player)MemberwiseClone();
        clonedPlayer.SuperstarData = (SuperstarData)SuperstarData.Clone();
        return clonedPlayer;
    }
    
    public bool CompareNames(string opponentName)
    {
        return SuperstarData.Name == opponentName;
    }

    public void NotifyThatPlayerWon()
    {
        View.CongratulateWinner(SuperstarData.Name);
    }
    
    public void SayPlayerTurnBegin()
    {
        View.SayThatATurnBegins(SuperstarData.Name);
    }
}
    