using RawDeal.Boundaries;
using RawDeal.Data_Structures;
using RawDeal.Superstars;
using RawDealView.Formatters;

namespace RawDeal.Decks;

public class DeckValidator
{
    private BoundaryList<IViewableCardInfo> _equalCardsGroup = new();
    private readonly Deck _deckToReview;
    private readonly CardsSet _cardsSet;
    private string _logoSuperstarToReview;
    private string _logoOtherSuperstar;
    private int _maxRepetitionsAllowed;
    private const int DeckLength = 60;

    public DeckValidator(Deck deck, CardsSet cardsSet)
    {
        _deckToReview = deck;
        _cardsSet = cardsSet;
    }

    public bool ValidateDeckRules()
    {
        bool conditionOne = ValidateDeckLength() && ValidateCardRepetitions();
        bool conditionTwo = ValidateIfCardsAreOnlyHeelOrFace() && ValidateCardLogoEqualToSuperstarLogo();
        return conditionOne && conditionTwo;
    }
    
    private bool ValidateDeckLength()
    {
        return _deckToReview.DeckCards.Count == DeckLength;
    }

    private bool ValidateCardRepetitions()
    {
        var cardGrouping = _deckToReview.DeckCards.GroupBy(card => card.Title);
        foreach (var cardGroupsWithSameName in cardGrouping)
        {
            _equalCardsGroup = cardGroupsWithSameName.ToBoundaryList();
            GetMaximumRepetitionsAllowed();
            if (CheckMaxRepetitionsAllowed())
                return false;
        }
        return true;
    }
    
    private bool ValidateIfCardsAreOnlyHeelOrFace()
    {
        bool heelExists = _deckToReview.DeckCards.Any(c => c.Subtypes.Contains("Heel"));
        bool faceExists = _deckToReview.DeckCards.Any(c => c.Subtypes.Contains("Face"));
        return !(heelExists && faceExists);
    }

    private bool ValidateCardLogoEqualToSuperstarLogo()
    {
        _logoSuperstarToReview = _deckToReview.PlayerDeckOwner.CompareLogo();
        foreach (Player superstar in _cardsSet.PossibleSuperstars)
        {
            _logoOtherSuperstar = superstar.CompareLogo();
            if (!ReviewDeckCardsByLogo())
                return false;
        }
        
        return true;
    }

    private bool ReviewDeckCardsByLogo()
    {
        if (_logoOtherSuperstar != _logoSuperstarToReview)
        {
            if (CheckIfOtherSuperstarLogoInDeck())
                return false;
        }
        return true;
    }
    private void GetMaximumRepetitionsAllowed()
    {
        _maxRepetitionsAllowed = 3;
        ValidateUniqueRepetitions();
        ValidateSetUpRepetitions();
    }
    
    private void ValidateUniqueRepetitions()
    {
        if (_equalCardsGroup.Any(c => c.Subtypes.Contains("Unique"))) 
            _maxRepetitionsAllowed = 1;
    }

    private void ValidateSetUpRepetitions()
    {
        if (_equalCardsGroup.Any(c => c.Subtypes.Contains("SetUp"))) 
            _maxRepetitionsAllowed = 70;
    }
    
    private bool CheckMaxRepetitionsAllowed()
    {
        return _equalCardsGroup.Count > _maxRepetitionsAllowed;
    }

    private bool CheckIfOtherSuperstarLogoInDeck()
    {
       return _deckToReview.DeckCards.Any(c => c.Subtypes.Contains(_logoOtherSuperstar));
    }
}

