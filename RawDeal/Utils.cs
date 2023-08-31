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
}