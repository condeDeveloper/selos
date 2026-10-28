namespace Conde.Selos;

/// <summary>
/// Quem é alcançável. Esta é a parte burra do repositório, e é de propósito: um
/// crivo que marca todo valor que dá para montar com os selos, sem limite de
/// quantos selos, e depois responde pergunta nenhuma além de "esse dá?".
/// </summary>
public static class Alcance
{
    /// <summary>
    /// Marca de 0 a <paramref name="teto"/> quem dá para montar. Começa do zero e
    /// vai somando: se v dá, v + selo também dá.
    /// </summary>
    public static bool[] Ate(Conjunto conjunto, int teto)
    {
        ArgumentNullException.ThrowIfNull(conjunto);
        if (teto < 0) throw new ArgumentOutOfRangeException(nameof(teto));

        var da = new bool[teto + 1];
        da[0] = true;
        for (var v = 1; v <= teto; v++)
            foreach (var selo in conjunto.Valores)
            {
                if (selo > v || !da[v - selo]) continue;
                da[v] = true;
                break;
            }

        return da;
    }

    /// <summary>
    /// Dá para montar este valor exato? Força bruta recursiva, sem crivo e sem
    /// memória. Lenta de propósito: é ela que confere o crivo.
    /// </summary>
    public static bool Da(Conjunto conjunto, int valor)
    {
        ArgumentNullException.ThrowIfNull(conjunto);
        if (valor < 0) return false;
        if (valor == 0) return true;

        foreach (var selo in conjunto.Valores)
            if (selo <= valor && Da(conjunto, valor - selo))
                return true;

        return false;
    }

    /// <summary>
    /// O maior valor que NÃO dá para montar: o número de Frobenius. Depois do
    /// menor valor ao quadrado não há mais buraco nenhum, e é por isso que varrer
    /// até ali basta.
    /// </summary>
    public static int Frobenius(Conjunto conjunto)
    {
        ArgumentNullException.ThrowIfNull(conjunto);
        if (!conjunto.TemFrobenius)
            throw new ArgumentException("com divisor comum maior que 1, ou com o selo de 1, não existe maior inalcançável");

        var teto = conjunto.Menor * conjunto.Maior;
        var da = Ate(conjunto, teto);
        for (var v = teto; v >= 0; v--)
            if (!da[v])
                return v;

        return 0;
    }

    /// <summary>Quantos valores NÃO dão para montar, ao todo.</summary>
    public static int Buracos(Conjunto conjunto)
    {
        ArgumentNullException.ThrowIfNull(conjunto);
        if (!conjunto.TemFrobenius) throw new ArgumentException("há infinitos buracos");

        var teto = conjunto.Menor * conjunto.Maior;
        var da = Ate(conjunto, teto);
        var buracos = 0;
        for (var v = 1; v <= teto; v++)
            if (!da[v])
                buracos++;
        return buracos;
    }

    /// <summary>
    /// A fórmula de Sylvester, de 1884, para dois selos: o maior inalcançável é
    /// ab - a - b, e os buracos são (a-1)(b-1)/2. Ela não é usada em lugar
    /// nenhum do cálculo: existe para ser confrontada com a varredura.
    /// </summary>
    public static (int Frobenius, int Buracos) PorSylvester(int a, int b)
    {
        if (a < 1 || b < 1) throw new ArgumentOutOfRangeException(nameof(a));
        if (Conjunto.Mdc(a, b) != 1) throw new ArgumentException("Sylvester pede dois primos entre si");
        return (a * b - a - b, (a - 1) * (b - 1) / 2);
    }
}
