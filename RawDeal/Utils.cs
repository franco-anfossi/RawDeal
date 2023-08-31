using System.Text.Json;
namespace RawDeal;

public static class Utils
{
    public static List<T> AbrirArchivo<T>(string archivo)
    {
        string myJson = File.ReadAllText(archivo);
        var cartas = JsonSerializer.Deserialize<List<T>>(myJson);
        return cartas;
    }

    public static string[] AbrirMazo(string archivo)
    {
        string[] lines = File.ReadAllLines(archivo);
        return lines;
    }
    
    // -------------- Codigo obtenido de la clase 7 --------------
    public static void Shuffle<T>(this IList<T>list) 
    {
        var rnd = new Random ();
        for (var i = list.Count ; i > 0; i--) 
            list.Swap(0, rnd.Next(0, i)); 
    } 
    
    private static void Swap<T>(this IList<T>list, int i, int j)
    {
        (list[i], list[j]) = (list[j], list[i]);
    }
    // ------------------------------------------------------------

}