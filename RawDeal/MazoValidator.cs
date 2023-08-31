namespace RawDeal;

using System.Linq;

public static class MazoValidator
{
    public static bool Validate(Mazo mazo, ConjuntoCartas todasCartas)
    {
        bool condicionUno = ValidateCantidad(mazo) && ValidateRepeticion(mazo);
        bool condicionDos = ValidateHeelFace(mazo) && ValidateLogo(mazo, todasCartas);
        return condicionUno && condicionDos;
    }
    
    private static bool ValidateCantidad(Mazo mazo)
    {
        // El mazo debe tener 60 cartas y una de superestrellas.
        return mazo.SuperstarsInicial.Count == 1 && mazo.Cartas.Count == 60;
    }

    private static bool ValidateRepeticion(Mazo mazo)
    {
        // En general no puedes tener mas de 3 cartas repetidas.
        // Si una es setup puedes tener infinitas de estas.
        // Si una carta es unique puedes tener 1 de estas cartas máximo.
        var gruposCartas = mazo.Cartas.GroupBy(carta => carta.Title);
        foreach (var grupo in gruposCartas)
        {
            var cartasIguales = grupo.ToList();
            int maxPermitido = cartasIguales.Any(c => c.Subtypes.Contains("Unique")) ? 1 : 3;
            if (cartasIguales.Any(c => c.Subtypes.Contains("SetUp"))) 
            {
                maxPermitido = 70;
            }

            if (cartasIguales.Count > maxPermitido)
            {
                return false;
            }
        }
        return true;
    }

    private static bool ValidateHeelFace(Mazo mazo)
    {
        // No puedes tener cartas heel y face al mismo tiempo.
        bool existeHeel = mazo.Cartas.Any(c => c.Subtypes.Contains("Heel"));
        bool existeFace = mazo.Cartas.Any(c => c.Subtypes.Contains("Face"));
        return !(existeHeel && existeFace);
    }

    private static bool ValidateLogo(Mazo mazo, ConjuntoCartas todasCartas)
    {
        // Si una carta en subtype contiene el logo de la superestrella solo la puede usar él.
        if (mazo.SuperstarsInicial.Count != 1)
        {
            return false;
        }

        var superstarLogo = mazo.SuperstarsInicial[0].Logo;
        foreach (var superstar in todasCartas.Superstars)
        {
            if (superstar.Logo != superstarLogo)
            {
                if (mazo.Cartas.Any(c => c.Subtypes.Contains(superstar.Logo))) { return false; }
            }
        }
        
        return true;
    }
}

