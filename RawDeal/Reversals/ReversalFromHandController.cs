using RawDeal.Boundaries;
using RawDeal.Cards;
using RawDeal.Cards.Builders;
using RawDeal.Data_Structures;
using RawDealView;
using RawDealView.Formatters;

namespace RawDeal.Reversals;

public class ReversalFromHandController
{
    private readonly View _view;
    private readonly IViewablePlayInfo _selectedPlay;
    private readonly ImportantPlayerData _playerData;
    private readonly ImportantPlayerData _opponentData;
    private CardController _cardController;

    public ReversalFromHandController(ImportantPlayerData playerData, 
        ImportantPlayerData opponentData, IViewablePlayInfo selectedPlay, View view)
    {
        _view = view;
        _playerData = playerData;
        _opponentData = opponentData;
        _selectedPlay = selectedPlay;
    }
    
    public bool CheckPreconditions()
    {
        var reversalPreconditionBuilder = new ReversalPreconditionBuilder(_selectedPlay, _opponentData);
        var reversalPreconditions = reversalPreconditionBuilder.BuildConditions();

        return reversalPreconditions.All(condition => condition.Check());
    }

    public void SelectReversalFromHand()
    {
        var correctReversalCards = SearchForCorrectReversals();
        var reversalPlaysInfo = CreateReversalPlays(correctReversalCards);
        var applicableReversals = Utils.FormatPlays(reversalPlaysInfo);
        var numReversalToPlay = _view.AskUserToSelectAReversal(_opponentData.Name, applicableReversals.ToList());
        
        if (numReversalToPlay != -1)
            ExecuteSelectedReversalEffect(reversalPlaysInfo, numReversalToPlay);
    }
    
    private BoundaryList<IViewableCardInfo> SearchForCorrectReversals()
    {
        var reversalCards = _opponentData.DecksController.SearchForReversalInHand();
        
        return _selectedPlay.PlayedAs == "MANEUVER" ? 
            SearchForManeuverReversals(reversalCards) : SearchForActionReversals(reversalCards);
    }
    
    private BoundaryList<IViewablePlayInfo> CreateReversalPlays(BoundaryList<IViewableCardInfo> cardsToPlays)
    {
        var plays = new BoundaryList<IViewablePlayInfo>();
        
        foreach (var card in cardsToPlays)
            plays.Add(new PlayInfo(card, "REVERSAL"));

        return plays;
    }
    
    private void ExecuteSelectedReversalEffect(BoundaryList<IViewablePlayInfo> reversalPlaysInfo, int numReversalToPlay)
    {
        var selectedReversalPlay = reversalPlaysInfo[numReversalToPlay];
        ProcessSelectedReversal(selectedReversalPlay);
    }
    
    private BoundaryList<IViewableCardInfo> SearchForManeuverReversals(BoundaryList<IViewableCardInfo> reversalCards)
    {
        var generalReversalCards = FindGeneralReversals(reversalCards);
        return CheckIfReversalsAreUsable(generalReversalCards.ToBoundaryList());
    }
    
    private BoundaryList<IViewableCardInfo> SearchForActionReversals(BoundaryList<IViewableCardInfo> reversalCards)
    {
        var actionReversalCards = FindActionReversals(reversalCards);
        return CheckIfReversalsAreUsable(actionReversalCards.ToBoundaryList());
    }
    
    private IEnumerable<IViewableCardInfo> FindGeneralReversals(BoundaryList<IViewableCardInfo> reversalCards)
    {
        var subtype = _selectedPlay.CardInfo.Subtypes[0];
        IEnumerable<IViewableCardInfo> generalReversalCards = reversalCards.Where(card =>
            card.Subtypes[0].Contains($"{subtype}") || card.Subtypes[0].Contains($"ReversalSpecial"));

        return generalReversalCards;
    }
    
    private IEnumerable<IViewableCardInfo> FindActionReversals(BoundaryList<IViewableCardInfo> reversalCards)
    {
        IEnumerable<IViewableCardInfo> actionReversalCards = reversalCards.Where(card => 
            card.Subtypes[0].Contains($"Action") || card.Title == "Jockeying for Position" || 
            card.Title == "Clean Break" || card.Title == "Irish Whip");

        return actionReversalCards;
    }
    
    private void ProcessSelectedReversal(IViewablePlayInfo selectedReversalPlay)
    {
        var formattedReversal = Utils.FormatSpecificPlay(selectedReversalPlay);
        selectedReversalPlay = HandleSpecialReversalDamage(selectedReversalPlay);
        
        BuildCardController(selectedReversalPlay);
        ResetBonuses();
        if (_cardController.CheckConditions())
            ApplyReversalEffect(selectedReversalPlay, formattedReversal);
    }
    
    private BoundaryList<IViewableCardInfo> CheckIfReversalsAreUsable(BoundaryList<IViewableCardInfo> generalReversals)
    {
        var correctFortitudeReversalCards = 
            CheckCardsFortitude(generalReversals.ToBoundaryList());
        
        var correctReversalCards = 
            FilterLessThanEightDamageEffectCards(correctFortitudeReversalCards);

        return FilterReversalCards(correctReversalCards);
    }

    private IViewablePlayInfo HandleSpecialReversalDamage(IViewablePlayInfo selectedReversalPlay)
    {
        if (selectedReversalPlay.CardInfo.Damage == "#")
            selectedReversalPlay = SetTotalDamage(selectedReversalPlay);

        return selectedReversalPlay;
    }
    
    private IViewablePlayInfo SetTotalDamage(IViewablePlayInfo selectedReversalPlay)
    {
        int damageToAdd = HandleDamageAddedByEffects();
        int damageValue = Convert.ToInt32(_selectedPlay.CardInfo.Damage) + damageToAdd;
        damageValue -= _playerData.BonusSet.MankindBonusDamageChange.MankindOpponentDamageChange;
        selectedReversalPlay.CardInfo.Damage = "#" + damageValue;
        return selectedReversalPlay;
    } 
    
    private void BuildCardController(IViewablePlayInfo reversalPlay)
    {
        var conditionBuilder = new ConditionBuilder(_playerData, _selectedPlay, reversalPlay);
        var conditions = conditionBuilder.BuildConditions();
        
        var effectBuilder = new ReversalEffectBuilder(_opponentData, 
            _playerData, reversalPlay, ReversalPlayedFrom.PlayedFromHand, _view);
        var effects = effectBuilder.BuildEffects();
        
        _cardController = new CardController(effects, conditions);
    }
    
    private void ResetBonuses()
    {
        _opponentData.BonusSet.ChangesByJockeyingForPosition.Reset();
        _playerData.BonusSet.ChangesByJockeyingForPosition.Reset();
        
        _opponentData.BonusSet.ChangesByIrishWhip.Reset();
        _playerData.BonusSet.ChangesByIrishWhip.Reset();
    }
    
    private void ApplyReversalEffect(IViewablePlayInfo selectedReversalPlay, string formattedReversal)
    {
        _view.SayThatPlayerReversedTheCard(_opponentData.Name, formattedReversal);
        _playerData.DecksController.PassCardFromHandToRingside(_selectedPlay.CardInfo);
        _opponentData.DecksController.PassCardFromHandToRingArea(selectedReversalPlay.CardInfo);
        _cardController.PlayCard();
    }
    
    private BoundaryList<IViewableCardInfo> CheckCardsFortitude(BoundaryList<IViewableCardInfo> filteredCards)
    {
        var cardsWithCorrectFortitude = new BoundaryList<IViewableCardInfo>();
        foreach (IViewableCardInfo filteredCard in filteredCards)
        {
            cardsWithCorrectFortitude = FilterCardWithCorrectFortitude(filteredCard, cardsWithCorrectFortitude);
        }
        
        return cardsWithCorrectFortitude;
    }

    private BoundaryList<IViewableCardInfo> FilterCardWithCorrectFortitude(IViewableCardInfo filteredCard, 
        BoundaryList<IViewableCardInfo> cardsWithCorrectFortitude)
    {
        int fortitudeToAdd = HandleFortitudeAddedByEffects();
        var cardFortitude = Convert.ToInt32(filteredCard.Fortitude);
        if (cardFortitude + fortitudeToAdd <= _opponentData.SuperstarData.Fortitude) 
            cardsWithCorrectFortitude.Add(filteredCard);
        
        return cardsWithCorrectFortitude;
    }

    private int HandleFortitudeAddedByEffects()
    {
        int fortitudeAdded = 0;
        if (CheckIfSelectedCardIsGrapple())
            fortitudeAdded = _opponentData.BonusSet.ChangesByJockeyingForPosition.FortitudeNeeded;
        
        return fortitudeAdded;
    }
    
    private BoundaryList<IViewableCardInfo> FilterLessThanEightDamageEffectCards(
        BoundaryList<IViewableCardInfo> filteredCards)
    {
        int damageToAdd = HandleDamageAddedByEffects();
        int damageReduction = _playerData.BonusSet.MankindBonusDamageChange.MankindOpponentDamageChange;
        var cardDamage = Convert.ToInt32(_selectedPlay.CardInfo.Damage) + damageToAdd - damageReduction;
        
        if (cardDamage > 7)
            filteredCards.RemoveAll(card => card.CardEffect.Contains("that does 7D or less."));
        
        return filteredCards;
    }
    
    private int HandleDamageAddedByEffects()
    {
        int damageAdded = 0;
        if (CheckIfSelectedCardIsGrapple())
            damageAdded += _playerData.BonusSet.ChangesByJockeyingForPosition.DamageAdded;
        
        else if (CheckIfSelectedCardIsStrike())
            damageAdded += _playerData.BonusSet.ChangesByIrishWhip.DamageAdded;
        
        return damageAdded;
    }
    
    private BoundaryList<IViewableCardInfo> FilterReversalCards(BoundaryList<IViewableCardInfo> filteredCards)
    {
        var cardName = _selectedPlay.CardInfo.Title;
        return cardName switch
        {
            "Jockeying for Position" => FilterForJockeyingForPosition(filteredCards),
            "Irish Whip" => FilterForIrishWhip(filteredCards),
            _ => DefaultFilter(filteredCards)
        };
    }

    private BoundaryList<IViewableCardInfo> FilterForJockeyingForPosition(BoundaryList<IViewableCardInfo> cards)
    {
        cards.RemoveAll(card => card.Title != "Jockeying for Position" && 
                                card.Title != "Clean Break" && card.Title != "No Chance in Hell");
        return cards;
    }

    private BoundaryList<IViewableCardInfo> FilterForIrishWhip(BoundaryList<IViewableCardInfo> cards)
    {
        cards.RemoveAll(card => card.Title != "Irish Whip" && card.Title != "No Chance in Hell");
        return cards;
    }

    private BoundaryList<IViewableCardInfo> DefaultFilter(BoundaryList<IViewableCardInfo> cards)
    {
        cards.RemoveAll(card => card.Title == "Jockeying for Position" || 
                                card.Title == "Clean Break" || card.Title == "Irish Whip");
        return cards;
    }
    
    private bool CheckIfSelectedCardIsGrapple()
        => _selectedPlay.CardInfo.Subtypes.Contains("Grapple");
    
    private bool CheckIfSelectedCardIsStrike()
        => _selectedPlay.CardInfo.Subtypes.Contains("Strike");
}