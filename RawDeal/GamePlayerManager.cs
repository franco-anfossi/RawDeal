using RawDeal.Boundaries;
using RawDeal.Data_Structures;
using RawDeal.Exceptions;
using RawDeal.OptionsPlayCard;
using RawDeal.Superstars;
using RawDealView;
using RawDealView.Options;

namespace RawDeal;

public class GamePlayerManager
{
    private readonly View _view;
    private int _inTurnPlayerIndex;
    private int _opponentPlayerIndex;
    private readonly BoundaryList<Player> _players;
    private ImportantPlayerData _inTurnPlayerData;
    private ImportantPlayerData _opponentPlayerData;
    
    public GamePlayerManager(BoundaryList<Player> players, View view)
    {
        _view = view;
        _inTurnPlayerIndex = 0;
        _opponentPlayerIndex = 1;
        _players = players;
    }
    
    public void InitializeNecessaryPlayerVariables()
    {
        SelectFirstPlayer();
        InitializePlayerVariables();
        AddNecessarySuperstarAttributes();
    }
    
    private void SelectFirstPlayer()
    {
        if (!ChooseFirstPlayerBySuperstarValue())
            Utils.ChangePositionsOfTheList(_players);
    }
    
    private bool ChooseFirstPlayerBySuperstarValue()
    {
        return _players[0].CompareSuperstarValue() >= _players[1].CompareSuperstarValue();
    }
    
    private void InitializePlayerVariables()
    {
        _inTurnPlayerData = _players[_inTurnPlayerIndex].BuildImportantPlayerData();
        _opponentPlayerData = _players[_opponentPlayerIndex].BuildImportantPlayerData();
    }
    
    private void AddNecessarySuperstarAttributes()
    {
        _players[_inTurnPlayerIndex].ApplyNecessaryAttributes(_view, _opponentPlayerData);
        _players[_opponentPlayerIndex].ApplyNecessaryAttributes(_view, _inTurnPlayerData);
    }
    
    public void DrawInitialCards()
    {
        _inTurnPlayerData.DecksController.DrawCardsInTheBeginning();
        _opponentPlayerData.DecksController.DrawCardsInTheBeginning();
    }
    
    public void SayThatSuperstarStartsTurn()
    {
        _view.SayThatATurnBegins(_inTurnPlayerData.Name);
    }
    
    public void PlaySpecialAbilityBeforeDrawingACard()
    {
        if (_players[_inTurnPlayerIndex].VerifyAbilityUsability())
            _players[_inTurnPlayerIndex].PlaySpecialAbility();
    }

    public void DrawCard()
    {
        _inTurnPlayerData.DecksController.DrawTurnCard();
    }

    public void ResetAbility()
    {
        _players[_inTurnPlayerIndex].ResetAbility();
    }
    
    public PlayerInfoManager UpdatePlayersInfo()
    {
        PlayerInfo inTurnPlayerInfo = _players[_inTurnPlayerIndex].BuildPlayerInfo();
        PlayerInfo opponentPlayerInfo = _players[_opponentPlayerIndex].BuildPlayerInfo();
        return new PlayerInfoManager(inTurnPlayerInfo, opponentPlayerInfo, _view);
    }
    
    public NextPlay ShowAppropriateOptionsSelector()
    {
        NextPlay firstOptionChoice = _players[_inTurnPlayerIndex].VerifyAbilityUsability() ? 
            _view.AskUserWhatToDoWhenHeCannotUseHisAbility() : _view.AskUserWhatToDoWhenUsingHisAbilityIsPossible(); 
        return firstOptionChoice;
    }
    
    public void SelectShowCardsOption()
    {
        _players[_inTurnPlayerIndex].ShowOptionsToViewDecks();
    }
    
    public void SelectPlayCardOption()
    {
        var inTurnPlayerInfo = _players[_inTurnPlayerIndex].BuildImportantPlayerData();
        var opponentPlayerInfo = _players[_opponentPlayerIndex].BuildImportantPlayerData();
        
        var optionsToPlayCard = new OptionsToPlayCard(inTurnPlayerInfo, opponentPlayerInfo, _view);
        optionsToPlayCard.StartElectionProcess();
    }
    
    public void SelectPlayAbilityOption()
    {
        _players[_inTurnPlayerIndex].PlaySpecialAbility();
    }
    
    public void ResetPlayersChangesByJockeyingForPosition()
    {
        _inTurnPlayerData.ChangesByJockeyingForPosition.Reset();
        _opponentPlayerData.ChangesByJockeyingForPosition.Reset();
    }

    public void CheckOpponentLoss()
    {
        if (_opponentPlayerData.DecksController.CheckForEmptyArsenal())
            throw new EndOfPrincipalLoop(_inTurnPlayerData.Name);
    }

    public void CheckPlayerInTurnLoss()
    {
        if (_inTurnPlayerData.DecksController.CheckForEmptyArsenal())
            GiveUp();
    }
    
    public void GiveUp()
    {
        throw new EndOfPrincipalLoop(_opponentPlayerData.Name);
    }
    
    public void ChangePlayersPositions()
    {
        ChangePlayersIndex();
        ChangePlayersDataVariable();
    }
    
    private void ChangePlayersIndex()
    {
        var newOpponentPlayerIndex = _inTurnPlayerIndex;
        var newInTurnPlayerIndex = _opponentPlayerIndex;
        
        _inTurnPlayerIndex = newInTurnPlayerIndex;
        _opponentPlayerIndex = newOpponentPlayerIndex;
    }
    
    private void ChangePlayersDataVariable()
    {
        var newInTurnPlayerDecksController = _inTurnPlayerData;
        var newOpponentPlayerDecksController = _opponentPlayerData;
        
        _inTurnPlayerData = newOpponentPlayerDecksController;
        _opponentPlayerData = newInTurnPlayerDecksController;
    }
}