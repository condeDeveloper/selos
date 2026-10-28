namespace Conde.Selos;

/// <summary>
/// Um conjunto de valores de selo: inteiros positivos, sem repetição, em ordem.
/// Se eles tiverem um divisor comum maior que 1, nenhum valor fora dos múltiplos
/// desse divisor é alcançável, e aí a pergunta "qual o maior que não dá" não tem
/// resposta.
/// </summary>
public sealed class Conjunto
{
    private readonly int[] _valores;

    public Conjunto(params int[] valores)
    {
        ArgumentNullException.ThrowIfNull(valores);
        if (valores.Length == 0) throw new ArgumentException("conjunto vazio", nameof(valores));
        foreach (var valor in valores)
            if (valor < 1)
                throw new ArgumentException("valor de selo precisa ser positivo: " + valor, nameof(valores));

        _valores = valores.Distinct().OrderBy(v => v).ToArray();
        Divisor = _valores.Aggregate(Mdc);
    }

    public IReadOnlyList<int> Valores => _valores;

    public int Quantidade => _valores.Length;

    public int Menor => _valores[0];

    public int Maior => _valores[^1];

    /// <summary>O máximo divisor comum de todos os valores.</summary>
    public int Divisor { get; }

    /// <summary>
    /// Só faz sentido perguntar pelo maior inalcançável quando o divisor comum é
    /// 1 — fora isso, há infinitos inalcançáveis.
    /// </summary>
    public bool TemFrobenius => Divisor == 1 && !_valores.Contains(1);

    public override string ToString() => "{" + string.Join(", ", _valores) + "}";

    public static int Mdc(int a, int b)
    {
        while (b != 0) (a, b) = (b, a % b);
        return Math.Abs(a);
    }
}
