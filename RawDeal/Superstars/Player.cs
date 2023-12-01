using RawDeal.Boundaries;
using RawDeal.Data_Structures;
using RawDeal.Decks;
using RawDeal.OptionsViewDeck;
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
    private readonly BonusSet _bonusSet = new();

    protected Player(SuperstarData superstarData)
    {
        SuperstarData = superstarData;
    }

    public virtual void PlaySpecialAbility() { }
    
    public virtual void ResetAbility() { }
    
    public virtual bool VerifyAbilityUsability()
        => true;
    
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
    
    public void SaveNecessaryAttributes(View view, ImportantPlayerData opponent)
    {
        View = view;
        OpponentData = opponent;
        AddMankindBonusIfNecessary();
    }
    
    private void AddMankindBonusIfNecessary()
    {
        if (OpponentData.SuperstarData.Name == "MANKIND")
            _bonusSet.MankindBonusDamageChange.MankindOpponentDamageChange = 1;
        
        if (SuperstarData.Name == "MANKIND")
            _bonusSet.MankindBonusDamageChange.MankindPlayerDamageChange = 1;
    }
    
    public string CompareLogo()
        => SuperstarData.Logo;
 
    public int CompareSuperstarValue()
        => SuperstarData.SuperstarValue;

    public void BuildDeckInfo(BoundaryList<IViewableCardInfo> arsenalDeck)
        => DecksInfo = new DecksInfo(arsenalDeck);

    public ImportantPlayerData BuildImportantPlayerData()
    {
        BuildPlayerDecksController();
        return new ImportantPlayerData(SuperstarData, PlayerDecksController, _bonusSet);
    }
    
    protected virtual void BuildPlayerDecksController()
        => PlayerDecksController = new PlayerDecksController(DecksInfo, SuperstarData);
    
    public PlayerInfo BuildPlayerInfo()
    {
        var (handCount, arsenalCount) = PlayerDecksController.ShowUpdatedDeckCounts();
        _playerInfo = new PlayerInfo(SuperstarData.Name, SuperstarData.Fortitude, handCount, arsenalCount);
        return _playerInfo;
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
}
    