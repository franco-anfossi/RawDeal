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
    
    public static (List<Superstar>, List<Carta>) DeserializarDeCartasYSuperstarsDesdeLosJson()
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
    
    private static List<Superstar?> DeserializarSuperstars()
    {
        _superstarsDeserializadas = JsonSerializer.Deserialize<List<Dictionary<string, JsonElement>>>(_archivoDeCartasJson);
        List<Superstar?> superstars = AnadirCadaSuperstarDespuesDeDeserializar();
        
        return superstars;
    }
    private static List<Superstar?> AnadirCadaSuperstarDespuesDeDeserializar()
    {
        List<Superstar?> superstars = new List<Superstar?>();
        foreach (var item in _superstarsDeserializadas!)
        {
            _valorParaLogo = item["Logo"].GetString();
            _archivoDeSuperstarJson = JsonSerializer.Serialize(item);
            Superstar? superstar = CrearClaseSuperstarDesdeJson();
            superstars.Add(superstar);
        }
        return superstars;
    }
    private static Superstar? CrearClaseSuperstarDesdeJson()
    {
        Superstar? superstar = _valorParaLogo switch
        {
            "StoneCold" => JsonSerializer.Deserialize<StoneCold>(_archivoDeSuperstarJson),
            "Undertaker" => JsonSerializer.Deserialize<Undertaker>(_archivoDeSuperstarJson),
            "HHH" => JsonSerializer.Deserialize<HHH>(_archivoDeSuperstarJson),
            "Jericho" => JsonSerializer.Deserialize<Jericho>(_archivoDeSuperstarJson),
            "Mankind" => JsonSerializer.Deserialize<Mankind>(_archivoDeSuperstarJson),
            "TheRock" => JsonSerializer.Deserialize<TheRock>(_archivoDeSuperstarJson),
            "Kane" => JsonSerializer.Deserialize<Kane>(_archivoDeSuperstarJson),
            _ => throw new ArgumentOutOfRangeException()
        };
        return superstar;
    }
}