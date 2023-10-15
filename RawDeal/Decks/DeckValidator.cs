using RawDealView.Formatters;

namespace RawDeal.Decks;

public class DeckValidator
{
    private List<IViewableCardInfo> _equalCardsGroup = new();
    private readonly Deck _deckToReview;
    private readonly CardsSet _cardsSet;
    private string _logoSuperstarToReview;
    private string _logoOtherSuperstar;
    private int _maxRepetitionsAllowed;

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
        return _deckToReview.DeckCards.Count == 60;
    }

    private bool ValidateCardRepetitions()
    {
        var cardGrouping = _deckToReview.DeckCards.GroupBy(card => card.Title);
        foreach (var cardGroupsWithSameName in cardGrouping)
        {
            _equalCardsGroup = cardGroupsWithSameName.ToList();
            GetMaximumRepetitionsAllowed();
            if (_equalCardsGroup.Count > _maxRepetitionsAllowed)
            {
                return false;
            }
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
        _logoSuperstarToReview = _deckToReview.PlayerDeckOwner.GetLogo();
        foreach (var superstar in _cardsSet.PossibleSuperstars)
        {
            _logoOtherSuperstar = superstar.GetLogo();
            if (!ReviewDeckCardsByLogo())
            {
                return false;
            }
        }
        
        return true;
    }

    private bool ReviewDeckCardsByLogo()
    {
        if (_logoOtherSuperstar != _logoSuperstarToReview)
        {
            if (_deckToReview.DeckCards.Any(c => c.Subtypes.Contains(_logoOtherSuperstar)))
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
}

