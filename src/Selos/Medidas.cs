namespace Conde.Selos;

/// <summary>
/// As medidas do README. Os testes conferem o que estes métodos devolvem; o
/// medidor só imprime.
/// </summary>
public static class Medidas
{
    public readonly record struct LinhaSylvester(
        int A, int B, int PelaVarredura, int PelaFormula, int BuracosVarridos, int BuracosPelaFormula, bool Bate);

    public readonly record struct LinhaTres(
        string Conjunto, int Frobenius, int Buracos, int MaiorPar, bool OTerceiroAjudou);

    public readonly record struct LinhaPostal(
        int Denominacoes, int Selos, string Melhor, int Alcance, int Conjuntos);

    public readonly record struct LinhaCusto(
        string Conjunto, int Selos, int Alcance, int PorForcaBruta, bool Bate);

    /// <summary>A fórmula de 1884 contra a varredura, em pares de selos.</summary>
    public static List<LinhaSylvester> Sylvester()
    {
        var linhas = new List<LinhaSylvester>();
        foreach (var (a, b) in new[] { (3, 5), (4, 7), (5, 8), (6, 11), (7, 10), (9, 20), (11, 13) })
        {
            var conjunto = new Conjunto(a, b);
            var varrido = Alcance.Frobenius(conjunto);
            var buracos = Alcance.Buracos(conjunto);
            var (formula, buracosFormula) = Alcance.PorSylvester(a, b);
            linhas.Add(new LinhaSylvester(
                a, b, varrido, formula, buracos, buracosFormula,
                varrido == formula && buracos == buracosFormula));
        }
        return linhas;
    }

    /// <summary>
    /// Três selos, onde não existe fórmula. A última coluna pergunta se o terceiro
    /// valor mudou alguma coisa em relação ao melhor par de dentro do conjunto.
    /// </summary>
    public static List<LinhaTres> ComTres()
    {
        var linhas = new List<LinhaTres>();
        foreach (var valores in new[]
                 {
                     new[] { 6, 9, 20 },
                     new[] { 3, 5, 7 },
                     new[] { 4, 9, 11 },
                     new[] { 5, 8, 12 },
                     new[] { 7, 11, 13 },
                     new[] { 11, 13, 15 },
                 })
        {
            var conjunto = new Conjunto(valores);
            var frobenius = Alcance.Frobenius(conjunto);

            // O melhor que daria para fazer com dois dos três.
            var maiorPar = int.MaxValue;
            for (var i = 0; i < valores.Length; i++)
                for (var j = i + 1; j < valores.Length; j++)
                {
                    if (Conjunto.Mdc(valores[i], valores[j]) != 1) continue;
                    maiorPar = Math.Min(maiorPar, Alcance.PorSylvester(valores[i], valores[j]).Frobenius);
                }

            if (maiorPar == int.MaxValue) maiorPar = -1;
            linhas.Add(new LinhaTres(
                conjunto.ToString(), frobenius, Alcance.Buracos(conjunto), maiorPar, maiorPar < 0 || frobenius < maiorPar));
        }
        return linhas;
    }

    /// <summary>
    /// O problema dos selos postais: o melhor conjunto de k valores para um
    /// envelope que aceita h selos, achado varrendo todos os conjuntos.
    /// </summary>
    public static List<LinhaPostal> Postais()
    {
        var linhas = new List<LinhaPostal>();
        foreach (var (denominacoes, selos, teto) in new[]
                 {
                     (2, 2, 12), (2, 3, 16), (2, 4, 20), (2, 5, 24),
                     (3, 2, 14), (3, 3, 20), (3, 4, 28),
                     (4, 2, 16), (4, 3, 26),
                 })
        {
            var (melhor, alcance, conjuntos) = Postal.OMelhor(denominacoes, selos, teto);
            linhas.Add(new LinhaPostal(denominacoes, selos, melhor.ToString(), alcance, conjuntos));
        }
        return linhas;
    }

    /// <summary>A programação dinâmica contra a força bruta, enquanto a força bruta aguenta.</summary>
    public static List<LinhaCusto> ContraOJuiz()
    {
        var linhas = new List<LinhaCusto>();
        foreach (var (valores, selos) in new (int[], int)[]
                 {
                     ([1, 3], 3), ([1, 4, 9], 3), ([1, 2, 5], 4), ([1, 3, 5, 7], 3), ([1, 5, 8], 4),
                 })
        {
            var conjunto = new Conjunto(valores);
            var rapido = Postal.Ate(conjunto, selos);
            var burro = Postal.AtePorForcaBruta(conjunto, selos);
            linhas.Add(new LinhaCusto(conjunto.ToString(), selos, rapido, burro, rapido == burro));
        }
        return linhas;
    }

    public readonly record struct LinhaBinario(
        int Denominacoes, int Selos, string Binario, int AlcanceBinario, string Melhor, int AlcanceMelhor);

    /// <summary>
    /// O conjunto de potências de 2 contra o melhor que existe. Dobrar o valor a
    /// cada selo é o que qualquer um escolheria, e dá para medir quanto isso
    /// custa quando o envelope limita o número de selos.
    /// </summary>
    public static List<LinhaBinario> Binario()
    {
        var linhas = new List<LinhaBinario>();
        foreach (var (denominacoes, selos, teto) in new[]
                 {
                     (3, 2, 14), (3, 3, 20), (3, 4, 28), (4, 2, 16), (4, 3, 26),
                 })
        {
            var potencias = new int[denominacoes];
            for (var i = 0; i < denominacoes; i++) potencias[i] = 1 << i;
            var binario = new Conjunto(potencias);

            var (melhor, alcance, _) = Postal.OMelhor(denominacoes, selos, teto);
            linhas.Add(new LinhaBinario(
                denominacoes, selos, binario.ToString(), Postal.Ate(binario, selos), melhor.ToString(), alcance));
        }
        return linhas;
    }

    /// <summary>
    /// O conjunto do McNugget, que é o exemplo que todo mundo conhece: caixas de
    /// 6, 9 e 20 nuggets.
    /// </summary>
    public static (int Frobenius, int Buracos, int MaiorComDois) McNugget()
    {
        var conjunto = new Conjunto(6, 9, 20);
        return (Alcance.Frobenius(conjunto), Alcance.Buracos(conjunto), Alcance.PorSylvester(9, 20).Frobenius);
    }
}
