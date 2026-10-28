using Conde.Selos;
using Xunit;

namespace Conde.Selos.Testes;

/// <summary>Todo número do README está preso aqui.</summary>
public class MedidasTestes
{
    [Theory]
    [InlineData(3, 5, 7, 4)]
    [InlineData(4, 7, 17, 9)]
    [InlineData(5, 8, 27, 14)]
    [InlineData(6, 11, 49, 25)]
    [InlineData(7, 10, 53, 27)]
    [InlineData(9, 20, 151, 76)]
    [InlineData(11, 13, 119, 60)]
    public void CadaLinhaDeSylvester(int a, int b, int frobenius, int buracos)
    {
        var linha = Medidas.Sylvester().Single(l => l.A == a && l.B == b);
        Assert.Equal(frobenius, linha.PelaVarredura);
        Assert.Equal(frobenius, linha.PelaFormula);
        Assert.Equal(buracos, linha.BuracosVarridos);
        Assert.Equal(buracos, linha.BuracosPelaFormula);
        Assert.True(linha.Bate);
    }

    [Fact]
    public void AFormulaNuncaDiscordaDaVarredura()
    {
        foreach (var linha in Medidas.Sylvester())
            Assert.True(linha.Bate, $"{linha.A} e {linha.B}");
    }

    [Theory]
    [InlineData("{6, 9, 20}", 43, 22, 151)]
    [InlineData("{3, 5, 7}", 4, 3, 7)]
    [InlineData("{4, 9, 11}", 14, 8, 23)]
    [InlineData("{5, 8, 12}", 19, 10, 27)]
    [InlineData("{7, 11, 13}", 30, 16, 59)]
    [InlineData("{11, 13, 15}", 64, 35, 119)]
    public void CadaLinhaDeTresSelos(string conjunto, int frobenius, int buracos, int melhorPar)
    {
        var linha = Medidas.ComTres().Single(l => l.Conjunto == conjunto);
        Assert.Equal(frobenius, linha.Frobenius);
        Assert.Equal(buracos, linha.Buracos);
        Assert.Equal(melhorPar, linha.MaiorPar);
        Assert.True(linha.OTerceiroAjudou);
    }

    [Fact]
    public void OTerceiroSeloSempreAjuda()
    {
        foreach (var linha in Medidas.ComTres())
            Assert.True(linha.Frobenius < linha.MaiorPar, linha.Conjunto);
    }

    [Fact]
    public void OMcNuggetDaQuarentaETres()
    {
        var (frobenius, buracos, comDois) = Medidas.McNugget();
        Assert.Equal(43, frobenius);
        Assert.Equal(22, buracos);
        Assert.Equal(151, comDois);
    }

    [Theory]
    [InlineData(2, 2, "{1, 2}", 4, 11)]
    [InlineData(2, 3, "{1, 3}", 7, 15)]
    [InlineData(2, 4, "{1, 3}", 10, 19)]
    [InlineData(2, 5, "{1, 4}", 14, 23)]
    [InlineData(3, 2, "{1, 3, 4}", 8, 78)]
    [InlineData(3, 3, "{1, 4, 5}", 15, 171)]
    [InlineData(3, 4, "{1, 5, 8}", 26, 351)]
    [InlineData(4, 2, "{1, 3, 5, 6}", 12, 455)]
    [InlineData(4, 3, "{1, 4, 7, 8}", 24, 2_300)]
    public void CadaLinhaDoProblemaPostal(int denominacoes, int selos, string melhor, int alcance, int conjuntos)
    {
        var linha = Medidas.Postais().Single(l => l.Denominacoes == denominacoes && l.Selos == selos);
        Assert.Equal(melhor, linha.Melhor);
        Assert.Equal(alcance, linha.Alcance);
        Assert.Equal(conjuntos, linha.Conjuntos);
    }

    [Theory]
    [InlineData(3, 2, 6, 8)]
    [InlineData(3, 3, 10, 15)]
    [InlineData(3, 4, 14, 26)]
    [InlineData(4, 2, 6, 12)]
    [InlineData(4, 3, 14, 24)]
    public void AsPotenciasDeDoisPerdemSempre(int denominacoes, int selos, int binario, int melhor)
    {
        var linha = Medidas.Binario().Single(l => l.Denominacoes == denominacoes && l.Selos == selos);
        Assert.Equal(binario, linha.AlcanceBinario);
        Assert.Equal(melhor, linha.AlcanceMelhor);
        Assert.True(linha.AlcanceBinario < linha.AlcanceMelhor);
    }

    [Fact]
    public void ComQuatroDenominacoesEDoisSelosOBinarioFazMetade()
    {
        var linha = Medidas.Binario().Single(l => l is { Denominacoes: 4, Selos: 2 });
        Assert.Equal("{1, 2, 4, 8}", linha.Binario);
        Assert.Equal("{1, 3, 5, 6}", linha.Melhor);
        Assert.Equal(2.0, (double)linha.AlcanceMelhor / linha.AlcanceBinario, 2);
    }

    [Fact]
    public void ADinamicaEOJuizNuncaDiscordam()
    {
        foreach (var linha in Medidas.ContraOJuiz())
        {
            Assert.True(linha.Bate, linha.Conjunto);
            Assert.Equal(linha.Alcance, linha.PorForcaBruta);
        }
    }
}
