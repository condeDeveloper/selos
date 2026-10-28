namespace Conde.Selos;

/// <summary>
/// O problema dos selos postais de verdade: no envelope cabem no máximo
/// <c>h</c> selos. A pergunta deixa de ser "quais valores dão" e passa a ser "até
/// onde dá para ir sem pular nenhum", porque um valor que precisa de vinte selos
/// não serve para postar carta nenhuma.
/// </summary>
public static class Postal
{
    /// <summary>
    /// O menor número de selos para montar cada valor de 0 a <paramref name="teto"/>,
    /// ou -1 quando não dá. É um caminho mínimo em grafo, feito com uma
    /// programação dinâmica de uma linha.
    /// </summary>
    public static int[] SelosNecessarios(Conjunto conjunto, int teto)
    {
        ArgumentNullException.ThrowIfNull(conjunto);
        if (teto < 0) throw new ArgumentOutOfRangeException(nameof(teto));

        var custo = new int[teto + 1];
        Array.Fill(custo, -1);
        custo[0] = 0;

        for (var v = 1; v <= teto; v++)
            foreach (var selo in conjunto.Valores)
            {
                if (selo > v || custo[v - selo] < 0) continue;
                var candidato = custo[v - selo] + 1;
                if (custo[v] < 0 || candidato < custo[v]) custo[v] = candidato;
            }

        return custo;
    }

    /// <summary>
    /// O alcance: o maior N tal que todo valor de 1 a N dá para montar com no
    /// máximo <paramref name="selos"/> selos. É o número que o problema dos selos
    /// postais quer maximizar.
    /// </summary>
    public static int Ate(Conjunto conjunto, int selos)
    {
        ArgumentNullException.ThrowIfNull(conjunto);
        if (selos < 1) throw new ArgumentOutOfRangeException(nameof(selos));

        var teto = conjunto.Maior * selos;
        var custo = SelosNecessarios(conjunto, teto);

        var n = 0;
        while (n + 1 <= teto && custo[n + 1] >= 0 && custo[n + 1] <= selos) n++;
        return n;
    }

    /// <summary>
    /// O mesmo alcance contado na marra: para cada valor, tenta todas as maneiras
    /// de escolher até h selos. Lento, e por isso só para valores pequenos — é o
    /// juiz da programação dinâmica.
    /// </summary>
    public static int AtePorForcaBruta(Conjunto conjunto, int selos)
    {
        ArgumentNullException.ThrowIfNull(conjunto);
        var n = 0;
        while (DaComAteTantosSelos(conjunto, n + 1, selos)) n++;
        return n;
    }

    private static bool DaComAteTantosSelos(Conjunto conjunto, int valor, int selos)
    {
        if (valor == 0) return true;
        if (selos == 0 || valor < 0) return false;

        foreach (var selo in conjunto.Valores)
            if (DaComAteTantosSelos(conjunto, valor - selo, selos - 1))
                return true;

        return false;
    }

    /// <summary>
    /// Todos os conjuntos de <paramref name="quantos"/> valores começando em 1 e
    /// até <paramref name="maiorValor"/>, com o alcance de cada um. O 1 está
    /// sempre dentro porque sem ele o valor 1 já não dá e o alcance é zero.
    /// </summary>
    public static (Conjunto Melhor, int Alcance, int Conjuntos) OMelhor(int quantos, int selos, int maiorValor)
    {
        if (quantos < 1) throw new ArgumentOutOfRangeException(nameof(quantos));

        Conjunto? melhor = null;
        var melhorAlcance = -1;
        var conjuntos = 0;
        var escolha = new int[quantos];
        escolha[0] = 1;

        void Passo(int indice, int minimo)
        {
            if (indice == quantos)
            {
                conjuntos++;
                var conjunto = new Conjunto((int[])escolha.Clone());
                var alcance = Ate(conjunto, selos);
                if (alcance <= melhorAlcance) return;
                melhorAlcance = alcance;
                melhor = conjunto;
                return;
            }

            for (var v = minimo; v <= maiorValor; v++)
            {
                escolha[indice] = v;
                Passo(indice + 1, v + 1);
            }
        }

        Passo(1, 2);
        return (melhor!, melhorAlcance, conjuntos);
    }
}
