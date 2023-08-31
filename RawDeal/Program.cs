using RawDeal;
using RawDealView;

View view = View.BuildConsoleView();
string deckFolder = Path.Combine("data", "02-InvalidDecks");
Game game = new Game(view, deckFolder);
game.Play();