using RawDeal.Boundaries;
using RawDealView.Formatters;

namespace RawDeal;

public static class Utils
{
    public static string[] OpenDeckArchive(string archive)
    {
        string[] archiveLines = File.ReadAllLines(archive);
        return archiveLines;
    }
    public static void ChangePositionsOfTheList<T>(BoundaryList<T> list)
    {
        (list[0], list[1]) = (list[1], list[0]);
    }

    public static BoundaryList<string> FormatDecksOfCards(BoundaryList<IViewableCardInfo> deckOfCards)
    {
        BoundaryList<string> formattedCardData = new BoundaryList<string>();
        foreach (IViewableCardInfo card in deckOfCards)
        {
            string formattedCard = Formatter.CardToString(card);
            formattedCardData.Add(formattedCard);
        }

        return formattedCardData;
    }
    
    public static BoundaryList<string> FormatPlays(BoundaryList<IViewablePlayInfo> plays)
    {
        var formattedPlays = new BoundaryList<string>();
        foreach (var play in plays)
        {
            var formattedPlay = FormatSpecificPlay(play);
            formattedPlays.Add(formattedPlay);
        }

        return formattedPlays;
    }
    
    public static string FormatSpecificPlay(IViewablePlayInfo reversalPlay)
        => Formatter.PlayToString(reversalPlay);
}