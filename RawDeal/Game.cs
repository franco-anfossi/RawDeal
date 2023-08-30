using RawDealView;

namespace RawDeal;

public class Game
{
    private View _view;
    private string _deckFolder;
    
    public Game(View view, string deckFolder)
    {
        _view = view;
        _deckFolder = deckFolder;
    }

    public void Play()
    {
        throw new NotImplementedException();
    }
}