using Conde.Selos;
using Xunit;

namespace Conde.Selos.Testes;

public class ConjuntoTestes
{
    [Fact]
    public void OsValoresSaemEmOrdemESemRepetir()
    {
        var c = new Conjunto(9, 6, 20, 6);
        Assert.Equal(new[] { 6, 9, 20 }, c.Valores);
        Assert.Equal(3, c.Quantidade);
    }

    [Fact]
    public void OMenorEOMaiorSaoOsDasPontas()
    {
        var c = new Conjunto(6, 9, 20);
        Assert.Equal(6, c.Menor);
        Assert.Equal(20, c.Maior);
    }

    [Fact]
    public void SeloDeValorZeroOuNegativoNaoExiste()
    {
        Assert.Throws<ArgumentException>(() => new Conjunto(0, 5));
        Assert.Throws<ArgumentException>(() => new Conjunto(-3));
        Assert.Throws<ArgumentException>(() => new Conjunto());
    }

    [Theory]
    [InlineData(new[] { 6, 9, 20 }, 1)]
    [InlineData(new[] { 4, 6, 10 }, 2)]
    [InlineData(new[] { 9, 12 }, 3)]
    public void ODivisorComumEhOMdcDeTodos(int[] valores, int divisor)
    {
        Assert.Equal(divisor, new Conjunto(valores).Divisor);
    }

    [Fact]
    public void ComDivisorComumNaoExisteMaiorInalcancavel()
    {
        Assert.False(new Conjunto(4, 6).TemFrobenius);
        Assert.Throws<ArgumentException>(() => Alcance.Frobenius(new Conjunto(4, 6)));
    }

    [Fact]
    public void ComOSeloDeUmTambemNao()
    {
        // Com o selo de 1 todo valor dá, e não sobra nenhum inalcançável.
        Assert.False(new Conjunto(1, 5).TemFrobenius);
    }

    [Fact]
    public void OMdcFazOQuePrometeu()
    {
        Assert.Equal(6, Conjunto.Mdc(12, 18));
        Assert.Equal(1, Conjunto.Mdc(9, 20));
        Assert.Equal(7, Conjunto.Mdc(7, 0));
    }

    [Fact]
    public void OTextoMostraOConjunto()
    {
        Assert.Equal("{6, 9, 20}", new Conjunto(20, 6, 9).ToString());
    }
}
