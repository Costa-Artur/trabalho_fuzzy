  using Fuzzy_Trab_IA;
  
  static void RodaRegraE(Dictionary<string, float> asVariaveis, string var1, string var2, string varr)
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
  
  static void RodaRegraOU(Dictionary<string, float> asVariaveis, string var1, string var2, string varr)
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
  
  static float ValidarGenero(string[] genero)
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

  var muitoIndicado = new VariavelFuzzy("Muito Indicado", 0, 0, 10, 20);
        var indicado = new VariavelFuzzy("Indicado", 10, 20, 30, 60);
        var indicadoMedio = new VariavelFuzzy("Indicado Medio", 20, 40, 50, 70);
        var poucoIndicado = new VariavelFuzzy("Pouco Indicado", 40, 60, 70, 120);
        var mPoucoIndicado = new VariavelFuzzy("Muito Pouco Indicado", 70, 110, 500, 500);

        var grupoGeneros = new GrupoVariaveis();
        grupoGeneros.Add(muitoIndicado);
        grupoGeneros.Add(indicado);
        grupoGeneros.Add(indicadoMedio);
        grupoGeneros.Add(poucoIndicado);
        grupoGeneros.Add(mPoucoIndicado);

        var grupoRating = new GrupoVariaveis();
        grupoRating.Add(new VariavelFuzzy("MR", 0, 0, 10, 20));
        grupoRating.Add(new VariavelFuzzy("R", 10, 20, 30, 40));
        grupoRating.Add(new VariavelFuzzy("B", 20, 40, 45, 50));
        grupoRating.Add(new VariavelFuzzy("MB", 40, 48, 50, 50));

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

        try
        {
            using (var reader = new StreamReader(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "movie_dataset.csv")))
            {
                var header = reader.ReadLine();
                var splitHeader = header.Split(';');

                for (int i = 0; i < splitHeader.Length; i++)
                {
                    Console.WriteLine($"{i} {splitHeader[i]}");
                }

                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    var spl = line.Split(';');
                    var asVariaveis = new Dictionary<string, float>();

                    var genero = spl[1].Split(' ');
                    float genres = ValidarGenero(genero);
                    Console.WriteLine($"Genero {genres}");

                    float rating = float.Parse(spl[5]);
                    grupoRating.Fuzzifica(rating, asVariaveis);

                    float votos = float.Parse(spl[6]);
                    grupoVotos.Fuzzifica(votos, asVariaveis);

                    Console.WriteLine($"{spl[3]} - rating {rating} votos {votos}");

                    RodaRegraE(asVariaveis, "MA", "V_MPV", "NA");

                    RodaRegraE(asVariaveis, "MA", "V_PV", "A");
                    RodaRegraE(asVariaveis, "MA", "V_MEV", "A");

                    RodaRegraE(asVariaveis, "A", "V_MPV", "NA");
                    RodaRegraE(asVariaveis, "A", "V_PV", "NA");
                    RodaRegraE(asVariaveis, "A", "V_MEV", "NA");

                    float na = asVariaveis.ContainsKey("NA") ? asVariaveis["NA"] : 0f;
                    float a = asVariaveis.ContainsKey("A") ? asVariaveis["A"] : 0f;
                    float ma = asVariaveis.ContainsKey("MA") ? asVariaveis["MA"] : 0f;

                    float score = (na * 1.5f + a * 7.0f + ma * 9.5f) / (na + a + ma);

                    Console.WriteLine($"NA {na} A {a} MA {ma}");
                    Console.WriteLine($" {rating} -> {score}");
                }
            }
        }
        catch (FileNotFoundException e)
        {
            Console.WriteLine("Arquivo não encontrado: " + e.Message);
        }
        catch (IOException e)
        {
            Console.WriteLine("Erro de leitura: " + e.Message);
        }