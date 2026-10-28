using Conde.Selos;
using Xunit;

namespace Conde.Selos.Testes;

public class PostalTestes
{
    [Fact]
    public void OCustoDoZeroEhZeroEOQueNaoDaEhMenosUm()
    {
        var custo = Postal.SelosNecessarios(new Conjunto(3, 5), 12);
        Assert.Equal(0, custo[0]);
        Assert.Equal(-1, custo[1]);
        Assert.Equal(1, custo[3]);
        Assert.Equal(2, custo[6]);
        Assert.Equal(2, custo[8]);
    }

    [Fact]
    public void OCustoMinimoEhMesmoOMinimo()
    {
        var conjunto = new Conjunto(1, 5, 8);
        var custo = Postal.SelosNecessarios(conjunto, 40);
        for (var v = 1; v <= 40; v++)
        {
            Assert.True(custo[v] > 0);
            // Com um selo a menos não dá.
            Assert.False(DaCom(conjunto, v, custo[v] - 1));
        }
    }

    private static bool DaCom(Conjunto conjunto, int valor, int selos)
    {
        if (valor == 0) return true;
        if (selos <= 0 || valor < 0) return false;
        foreach (var selo in conjunto.Valores)
            if (DaCom(conjunto, valor - selo, selos - 1))
                return true;
        return false;
    }

    [Theory]
    [InlineData(new[] { 1, 2 }, 2, 4)]
    [InlineData(new[] { 1, 3 }, 3, 7)]
    [InlineData(new[] { 1, 3, 4 }, 2, 8)]
    [InlineData(new[] { 1, 4, 5 }, 3, 15)]
    [InlineData(new[] { 1, 5, 8 }, 4, 26)]
    public void AteOndeOConjuntoVai(int[] valores, int selos, int alcance)
    {
        Assert.Equal(alcance, Postal.Ate(new Conjunto(valores), selos));
    }

    [Fact]
    public void ADinamicaEAForcaBrutaConcordam()
    {
        foreach (var valores in new[] { new[] { 1, 3 }, new[] { 1, 2, 5 }, new[] { 1, 4, 9 }, new[] { 1, 3, 5, 7 } })
            foreach (var selos in new[] { 1, 2, 3, 4 })
            {
                var conjunto = new Conjunto(valores);
                Assert.Equal(Postal.AtePorForcaBruta(conjunto, selos), Postal.Ate(conjunto, selos));
            }
    }

    [Fact]
    public void SemOSeloDeUmOAlcanceEhZero()
    {
        Assert.Equal(0, Postal.Ate(new Conjunto(2, 3), 5));
    }

    [Fact]
    public void OMelhorConjuntoAchadoEhMesmoOMelhor()
    {
        var (melhor, alcance, conjuntos) = Postal.OMelhor(3, 3, 20);
        Assert.Equal(alcance, Postal.Ate(melhor, 3));
        Assert.Equal(171, conjuntos);

        // Nenhum outro conjunto de três valores até 20 vai mais longe.
        for (var b = 2; b <= 20; b++)
            for (var c = b + 1; c <= 20; c++)
                Assert.True(Postal.Ate(new Conjunto(1, b, c), 3) <= alcance);
    }

    [Fact]
    public void MaisSelosNoEnvelopeNuncaDiminuiOAlcance()
    {
        var conjunto = new Conjunto(1, 4, 7, 8);
        for (var selos = 1; selos < 6; selos++)
            Assert.True(Postal.Ate(conjunto, selos + 1) >= Postal.Ate(conjunto, selos));
    }

    [Fact]
    public void ZeroSelosNaoEhPergunta()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => Postal.Ate(new Conjunto(1, 3), 0));
        Assert.Throws<ArgumentOutOfRangeException>(() => Postal.OMelhor(0, 3, 10));
    }
}
