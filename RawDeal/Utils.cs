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
}