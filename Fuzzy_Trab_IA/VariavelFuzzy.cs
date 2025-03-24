namespace Fuzzy_Trab_IA;

public class VariavelFuzzy
{
    public string Nome { get; set; }
    public float B1 { get; set; }
    public float T1 { get; set; }
    public float T2 { get; set; }
    public float B2 { get; set; }
    
    public float Valor;

    public VariavelFuzzy()
    {
    }

    public VariavelFuzzy(string nome, float b1, float t1, float t2, float b2)
    {
        Nome = nome;
        B1 = b1;
        T1 = t1;
        T2 = t2;
        B2 = b2;
    }

    public float Fuzzifica(float v)
    {
        if (v < B1 || v > B2)
        {
            return 0;
        }

        if (v >= T1 && v <= T2)
        {
            return 1;
        }

        if (v > B1 && v < T1)
        {
            float p = (v - B1) / (T1 - B1);
            return p;
        }

        if (v > T2 && v < B2)
        {
            float p = 1.0f - ((v - T2) / (B2 - T2));
            return p;
        }

        return 0;
    }
}