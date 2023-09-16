using System.Text.Json;
using RawDeal.Habilidades_Superstars;

namespace RawDeal;

public static class Utils
{
    private static string _archivoJson;
    
    public static void AbrirArchivo(string archivo)
    {
        _archivoJson = File.ReadAllText(archivo);
    }

    public static List<Carta> DeserializacionCartas()
    {
        var cartas = JsonSerializer.Deserialize<List<Carta>>(_archivoJson);
        return cartas ?? throw new InvalidOperationException();
    }
    
    public static List<Superstar> DeserializacionSuperstar()
    {
        var cartas = JsonSerializer.Deserialize<List<Dictionary<string, JsonElement>>>(_archivoJson);
        List<Superstar> superstars = new List<Superstar>();
        foreach (var item in cartas)
        {
            string? valorParaLogo = item["Logo"].GetString();
            string superstarJson = JsonSerializer.Serialize(item);

            Superstar? superstar = valorParaLogo switch
            {
                "StoneCold" => JsonSerializer.Deserialize<StoneCold>(superstarJson),
                "Undertaker" => JsonSerializer.Deserialize<Undertaker>(superstarJson),
                "HHH" => JsonSerializer.Deserialize<HHH>(superstarJson),
                "Jericho" => JsonSerializer.Deserialize<Jericho>(superstarJson),
                "Mankind" => JsonSerializer.Deserialize<Mankind>(superstarJson),
                "TheRock" => JsonSerializer.Deserialize<TheRock>(superstarJson),
                "Kane" => JsonSerializer.Deserialize<Kane>(superstarJson),
                _ => throw new InvalidOperationException("Tipo de Superstar no reconocido")
            };
            if (superstar != null) superstars.Add(superstar);
        }

        return superstars;
    }

    public static string[] AbrirMazo(string archivo)
    {
        string[] lines = File.ReadAllLines(archivo);
        return lines;
    }
    
    public static void Shuffle<T>(this IList<T>list) 
    {
        var rnd = new Random ();
        for (var i = list.Count ; i > 0; i--) 
            list.Swap(0, rnd.Next(0, i)); 
    } 
    
    public static void CambiarPosicionesDeLaLista<T>(List<T> lista)
    {
        (lista[0], lista[1]) = (lista[1], lista[0]);
    }
    
    private static void Swap<T>(this IList<T>list, int i, int j)
    {
        (list[i], list[j]) = (list[j], list[i]);
    }
}