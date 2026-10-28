using Conde.Selos;
using Xunit;

namespace Conde.Selos.Testes;

public class AlcanceTestes
{
    [Fact]
    public void OZeroSempreDa()
    {
        Assert.True(Alcance.Ate(new Conjunto(6, 9, 20), 10)[0]);
        Assert.True(Alcance.Da(new Conjunto(6, 9, 20), 0));
    }

    [Fact]
    public void OCrivoEAForcaBrutaConcordamEmTodoValor()
    {
        var conjunto = new Conjunto(4, 9, 11);
        var crivo = Alcance.Ate(conjunto, 60);
        for (var v = 0; v <= 60; v++)
            Assert.Equal(crivo[v], Alcance.Da(conjunto, v));
    }

    [Theory]
    [InlineData(new[] { 3, 5 }, 7)]
    [InlineData(new[] { 4, 7 }, 17)]
    [InlineData(new[] { 6, 9, 20 }, 43)]
    [InlineData(new[] { 3, 5, 7 }, 4)]
    public void OMaiorQueNaoDa(int[] valores, int frobenius)
    {
        var conjunto = new Conjunto(valores);
        Assert.Equal(frobenius, Alcance.Frobenius(conjunto));
        Assert.False(Alcance.Da(conjunto, frobenius));
    }

    [Fact]
    public void DepoisDoMaiorInalcancavelTudoDa()
    {
        foreach (var valores in new[] { new[] { 3, 5 }, new[] { 6, 9, 20 }, new[] { 7, 11, 13 } })
        {
            var conjunto = new Conjunto(valores);
            var frobenius = Alcance.Frobenius(conjunto);
            var crivo = Alcance.Ate(conjunto, frobenius + 50);
            for (var v = frobenius + 1; v <= frobenius + 50; v++)
                Assert.True(crivo[v], $"{conjunto} não monta {v}");
        }
    }

    [Theory]
    [InlineData(3, 5, 7, 4)]
    [InlineData(4, 7, 17, 9)]
    [InlineData(9, 20, 151, 76)]
    public void ASylvesterBateComAVarredura(int a, int b, int frobenius, int buracos)
    {
        var conjunto = new Conjunto(a, b);
        Assert.Equal(frobenius, Alcance.Frobenius(conjunto));
        Assert.Equal(buracos, Alcance.Buracos(conjunto));
        Assert.Equal((frobenius, buracos), Alcance.PorSylvester(a, b));
    }

    [Fact]
    public void SylvesterSoAceitaDoisPrimosEntreSi()
    {
        Assert.Throws<ArgumentException>(() => Alcance.PorSylvester(4, 6));
    }

    [Fact]
    public void OsBuracosDeUmParSaoMetadeDoMaiorMaisUm()
    {
        // (a-1)(b-1)/2 é exatamente metade de ab-a-b+1.
        foreach (var (a, b) in new[] { (3, 5), (5, 8), (7, 10), (11, 13) })
        {
            var (frobenius, buracos) = Alcance.PorSylvester(a, b);
            Assert.Equal((frobenius + 1) / 2, buracos);
        }
    }

    [Fact]
    public void TetoNegativoNaoEhPergunta()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => Alcance.Ate(new Conjunto(3, 5), -1));
    }

    [Fact]
    public void ValorNegativoNuncaDa()
    {
        Assert.False(Alcance.Da(new Conjunto(3, 5), -4));
    }
}
