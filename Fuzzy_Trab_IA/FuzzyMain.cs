namespace Fuzzy_Trab_IA;

public class FuzzyMain
{

    public static void RodaRegraE(Dictionary<string, float> asVariaveis, string var1, string var2, string varr)
    {
        float v = Math.Min(asVariaveis.ContainsKey(var1) ? asVariaveis[var1] : 0f,
                           asVariaveis.ContainsKey(var2) ? asVariaveis[var2] : 0f);

        if (asVariaveis.ContainsKey(varr))
        {
            asVariaveis[varr] = Math.Max(asVariaveis[varr], v);
        }
        else
        {
            asVariaveis[varr] = v;
        }
    }

    public static void RodaRegraOU(Dictionary<string, float> asVariaveis, string var1, string var2, string varr)
    {
        float v = Math.Max(asVariaveis.ContainsKey(var1) ? asVariaveis[var1] : 0f,
                           asVariaveis.ContainsKey(var2) ? asVariaveis[var2] : 0f);

        if (asVariaveis.ContainsKey(varr))
        {
            asVariaveis[varr] = Math.Max(asVariaveis[varr], v);
        }
        else
        {
            asVariaveis[varr] = v;
        }
    }

    public static float ValidarGenero(string[] genero)
    {
        float soma = 0;
        foreach (var g in genero)
        {
            switch (g)
            {
                case "Action":
                case "Drama":
                case "Family":
                case "Romance":
                case "War":
                case "Mystery":
                case "Animation":
                case "Foreign":
                    soma += 5;
                    break;
                case "Adventure":
                case "Science_Fiction":
                case "Comedy":
                    soma += 3;
                    break;
                case "Fantasy":
                case "Crime":
                case "Western":
                    soma += 2;
                    break;
                case "History":
                case "TV":
                case "Music":
                    soma += 1;
                    break;
                case "Thriller":
                case "Horror":
                case "Documentary":
                    soma += 0;
                    break;
            }
        }
        return soma;
    }
}