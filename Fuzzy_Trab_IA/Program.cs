using Fuzzy_Trab_IA;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

#region Regras Fuzzy Helpers
static void RodaRegraE(Dictionary<string, float> vars, string var1, string var2, string resultado)
{
    float v1 = vars.ContainsKey(var1) ? vars[var1] : 0f;
    float v2 = vars.ContainsKey(var2) ? vars[var2] : 0f;
    float valorMin = Math.Min(v1, v2);

    if (vars.ContainsKey(resultado))
        vars[resultado] = Math.Max(vars[resultado], valorMin);
    else
        vars[resultado] = valorMin;
}
#endregion

#region Gênero Validação
static float ValidarGenero(string[] generos)
{
    float soma = 0;
    foreach (var g in generos)
    {
        soma += g switch
        {
            "Action" or "Drama" or "Family" or "Romance" or "War" or "Mystery" or "Animation" or "Foreign" => 5,
            "Adventure" or "Science_Fiction" or "Comedy" => 3,
            "Fantasy" or "Crime" or "Western" => 2,
            "History" or "TV" or "Music" => 1,
            "Thriller" or "Horror" or "Documentary" => 0,
            _ => 0
        };
    }
    return soma;
}
#endregion

#region Fuzzy Variáveis Setup
var grupoGeneros = new GrupoVariaveis();
grupoGeneros.Add(new VariavelFuzzy("Muito Indicado", 0, 0, 10, 20));
grupoGeneros.Add(new VariavelFuzzy("Indicado", 10, 20, 30, 60));
grupoGeneros.Add(new VariavelFuzzy("Indicado Medio", 20, 40, 50, 70));
grupoGeneros.Add(new VariavelFuzzy("Pouco Indicado", 40, 60, 70, 120));
grupoGeneros.Add(new VariavelFuzzy("Muito Pouco Indicado", 70, 110, 500, 500));


var grupoRating = new GrupoVariaveis();
grupoRating.Add(new VariavelFuzzy("MR", 0, 0, 30, 40));
grupoRating.Add(new VariavelFuzzy("R", 30, 40, 50, 60));
grupoRating.Add(new VariavelFuzzy("B", 50, 60, 70, 80));
grupoRating.Add(new VariavelFuzzy("MB", 70, 80, 100, 100));


var grupoVotos = new GrupoVariaveis();
grupoVotos.Add(new VariavelFuzzy("V_MPV", 0, 0, 10, 20));
grupoVotos.Add(new VariavelFuzzy("V_PV", 10, 20, 50, 60));
grupoVotos.Add(new VariavelFuzzy("V_MEV", 40, 80, 200, 300));
grupoVotos.Add(new VariavelFuzzy("V_BAV", 200, 300, 500, 1000));
grupoVotos.Add(new VariavelFuzzy("V_MUV", 400, 500, 3200, 3200));


var grupoAtratividade = new GrupoVariaveis();
grupoAtratividade.Add(new VariavelFuzzy("NA", 0, 0, 3, 6));
grupoAtratividade.Add(new VariavelFuzzy("A", 5, 7, 8, 10));
grupoAtratividade.Add(new VariavelFuzzy("MA", 7, 9, 10, 10));

#endregion

#region Processamento CSV e Cálculo de Score
var filmes = new List<Filme>();

try
{
    string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "movie_dataset.csv");
    if (!File.Exists(path))
        throw new FileNotFoundException("Arquivo não encontrado.");

    using var reader = new StreamReader(path);
    var header = reader.ReadLine();
    if (string.IsNullOrWhiteSpace(header))
        throw new IOException("Arquivo vazio ou corrompido.");

    var splitHeader = header.Split(';');

    string line;
    while ((line = reader.ReadLine()) != null)
    {
        var spl = line.Split(';');
        var varsFuzzy = new Dictionary<string, float>();

        var generos = spl[1].Split(' ');
        float valorGenero = ValidarGenero(generos);
        Console.WriteLine($"Genero {valorGenero}");

        if (!float.TryParse(spl[5], out float rating)) continue;
        if (!float.TryParse(spl[6], out float votos)) continue;

        grupoRating.Fuzzifica(rating, varsFuzzy);
        grupoVotos.Fuzzifica(votos, varsFuzzy);

        Console.WriteLine($"{spl[3]} - rating {rating} votos {votos}");
        Console.WriteLine("Variáveis fuzzificadas:");
        foreach (var kvp in varsFuzzy)
            Console.WriteLine($"{kvp.Key} = {kvp.Value}");

        #region Regras Fuzzy de Atratividade
        RodaRegraE(varsFuzzy, "MA", "V_MPV", "NA");
        RodaRegraE(varsFuzzy, "MA", "V_PV", "A");
        RodaRegraE(varsFuzzy, "MA", "V_MEV", "A");

        RodaRegraE(varsFuzzy, "A", "V_MPV", "NA");
        RodaRegraE(varsFuzzy, "A", "V_PV", "NA");
        RodaRegraE(varsFuzzy, "A", "V_MEV", "NA");

        RodaRegraE(varsFuzzy, "B", "V_MUV", "MA");
        RodaRegraE(varsFuzzy, "B", "V_MEV", "A");
        RodaRegraE(varsFuzzy, "B", "V_PV", "NA");
        #endregion

        float na = varsFuzzy.GetValueOrDefault("NA", 0f);
        float a = varsFuzzy.GetValueOrDefault("A", 0f);
        float ma = varsFuzzy.GetValueOrDefault("MA", 0f);

        float pesoTotal = na + a + ma;
        float score = pesoTotal > 0 ? (na * 3f + a * 6f + ma * 9f) / pesoTotal : 1f; // Score mínimo = 1f

        Console.WriteLine($"NA {na} A {a} MA {ma}");
        Console.WriteLine($" {rating} -> {score}");

        filmes.Add(new Filme
        {
            Nome = spl[3],
            Rating = rating,
            Votos = votos,
            Generos = new List<string>(generos),
            ScoreFinal = score
        });
    }

    var top10Filmes = filmes.OrderByDescending(f => f.ScoreFinal).Take(10).ToList();

    Console.WriteLine("\nTop 10 Filmes Recomendados:");
    foreach (var filme in top10Filmes)
    {
        Console.WriteLine($"Nome: {filme.Nome}, Rating: {filme.Rating}, Votos: {filme.Votos}, Score Final: {filme.ScoreFinal}");
    }
}
catch (Exception ex)
{
    Console.WriteLine("Erro: " + ex.Message);
}
#endregion
