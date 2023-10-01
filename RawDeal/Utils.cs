using System.Runtime.InteropServices;
using System.Text.Json;
using RawDeal.Habilidades_Superstars;
using RawDealView.Formatters;

namespace RawDeal;

public static class Utils
{
    private static string? _valorParaLogo;
    private static string _archivoDeCartasJson;
    private static string _archivoDeSuperstarJson;
    private static List<Carta>? _cartasDeserializadas;
    private static List<Dictionary<string, JsonElement>>? _superstarsDeserializadas;
    
    public static string[] AbrirMazo(string archivo)
    {
        string[] lineasDelArchivo = File.ReadAllLines(archivo);
        return lineasDelArchivo;
    }
    public static void CambiarPosicionesDeLaLista<T>(List<T> lista)
    {
        (lista[0], lista[1]) = (lista[1], lista[0]);
    }

    public static List<string> FormatearMazoDeCartas(List<IViewableCardInfo> mazoDeCartas)
    {
        List<string> datosFormateadosDeLasCartas = new List<string>();
        foreach (var carta in mazoDeCartas)
        {
            string cartaFormateada = Formatter.CardToString(carta);
            datosFormateadosDeLasCartas.Add(cartaFormateada);
        }

        return datosFormateadosDeLasCartas;
    }
    
    public static (List<Jugador>, List<Carta>) DeserializarDeCartasYSuperstarsDesdeLosJson()
    {
        string archivoJsonCartas = Path.Combine("data", "cards.json");
        string archivoJsonSuperstars = Path.Combine("data", "superstar.json");
        
        AbrirArchivo(archivoJsonSuperstars);
        var superstarsDeserializadas = DeserializarSuperstars();
        
        AbrirArchivo(archivoJsonCartas);
        var cartasDeserializadas = DeserializarCartas();

        return (superstarsDeserializadas, cartasDeserializadas)!;
    }
    private static void AbrirArchivo(string nombreDelArchivo)
    {
        _archivoDeCartasJson = File.ReadAllText(nombreDelArchivo);
    }
    private static List<Carta> DeserializarCartas()
    {
        _cartasDeserializadas = JsonSerializer.Deserialize<List<Carta>>(_archivoDeCartasJson);
        return _cartasDeserializadas ?? throw new InvalidOperationException();
    }
    
    private static List<Jugador?> DeserializarSuperstars()
    {
        _superstarsDeserializadas = JsonSerializer.Deserialize<List<Dictionary<string, JsonElement>>>(_archivoDeCartasJson);
        List<Jugador?> superstars = AnadirCadaSuperstarDespuesDeDeserializar();
        
        return superstars;
    }
    private static List<Jugador?> AnadirCadaSuperstarDespuesDeDeserializar()
    {
        List<Jugador?> superstars = new List<Jugador?>();
        foreach (var item in _superstarsDeserializadas!)
        {
            _valorParaLogo = item["Logo"].GetString();
            _archivoDeSuperstarJson = JsonSerializer.Serialize(item);
            Jugador? superstar = CrearClaseSuperstarDesdeJson();
            superstars.Add(superstar);
        }
        return superstars;
    }
    private static Jugador? CrearClaseSuperstarDesdeJson()
    {
        Superstar? superstarData = JsonSerializer.Deserialize<Superstar>(_archivoDeSuperstarJson);
        Console.WriteLine(superstarData);
        Jugador? superstar = _valorParaLogo switch
        {
            "StoneCold" => new StoneCold(superstarData),
            "Undertaker" => new Undertaker(superstarData),
            "HHH" => new HHH(superstarData),
            "Jericho" => new Jericho(superstarData),
            "Mankind" => new Mankind(superstarData),
            "TheRock" => new TheRock(superstarData),
            "Kane" => new Kane(superstarData),
            _ => throw new ArgumentOutOfRangeException()
        };
        return superstar;
    }
}