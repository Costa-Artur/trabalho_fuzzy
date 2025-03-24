namespace Fuzzy_Trab_IA;

using System.Collections.Generic;

public class GrupoVariaveis
{
    private List<VariavelFuzzy> listaDeVariaveis;

    public GrupoVariaveis()
    {
        listaDeVariaveis = new List<VariavelFuzzy>();
    }

    public void Add(VariavelFuzzy var)
    {
        listaDeVariaveis.Add(var);
    }

    public void Fuzzifica(float v, Dictionary<string, float> variaveisFuzzy)
    {
        foreach (var variavel in listaDeVariaveis)
        {
            float val = variavel.Fuzzifica(v);
            variaveisFuzzy[variavel.Nome] = val;
        }
    }
}