using Conde.Selos;

var comando = args.Length > 0 ? args[0] : "tudo";

if (comando is "sylvester" or "tudo") Sylvester();
if (comando is "tres" or "tudo") Tres();
if (comando is "nugget" or "tudo") Nugget();
if (comando is "postal" or "tudo") Postal();
if (comando is "binario" or "tudo") Binario();
if (comando is "juiz" or "tudo") Juiz();

static void Sylvester()
{
    Console.WriteLine();
    Console.WriteLine("DOIS SELOS: a formula de Sylvester, de 1884, contra a varredura.");
    Console.WriteLine();
    Console.WriteLine($"  {"a",4} {"b",4} {"maior que nao da",17} {"pela formula",13} {"buracos",9} {"pela formula",13} {"bate",6}");

    foreach (var l in Sylvester())
        Console.WriteLine($"  {l.A,4} {l.B,4} {l.PelaVarredura,17} {l.PelaFormula,13} {l.BuracosVarridos,9} {l.BuracosPelaFormula,13} {(l.Bate ? "sim" : "NAO"),6}");

    Console.WriteLine();

    List<Medidas.LinhaSylvester> Sylvester() => Medidas.Sylvester();
}

static void Tres()
{
    Console.WriteLine();
    Console.WriteLine("TRES SELOS, onde nao existe formula nenhuma.");
    Console.WriteLine();
    Console.WriteLine($"  {"conjunto",-16} {"maior que nao da",17} {"buracos",9} {"o melhor par dentro",20} {"o terceiro ajudou",18}");

    foreach (var l in Medidas.ComTres())
        Console.WriteLine($"  {l.Conjunto,-16} {l.Frobenius,17} {l.Buracos,9} {l.MaiorPar,20} {(l.OTerceiroAjudou ? "sim" : "nao"),18}");

    Console.WriteLine();
}

static void Nugget()
{
    Console.WriteLine();
    var (frobenius, buracos, comDois) = Medidas.McNugget();
    Console.WriteLine("AS CAIXAS DE NUGGET: 6, 9 e 20.");
    Console.WriteLine();
    Console.WriteLine($"  maior numero que nao da para comprar   {frobenius,6}");
    Console.WriteLine($"  quantos nao dao, ao todo               {buracos,6}");
    Console.WriteLine($"  so com as de 9 e 20                    {comDois,6}");
    Console.WriteLine();
}

static void Postal()
{
    Console.WriteLine();
    Console.WriteLine("O PROBLEMA DOS SELOS POSTAIS: no envelope cabem h selos, e nao se pode pular valor.");
    Console.WriteLine();
    Console.WriteLine($"  {"denominacoes",13} {"selos",6} {"o melhor conjunto",-22} {"vai ate",8} {"conjuntos varridos",19}");

    foreach (var l in Medidas.Postais())
        Console.WriteLine($"  {l.Denominacoes,13} {l.Selos,6} {l.Melhor,-22} {l.Alcance,8} {l.Conjuntos,19:n0}");

    Console.WriteLine();
}

static void Binario()
{
    Console.WriteLine();
    Console.WriteLine("DOBRAR A CADA SELO parece a escolha certa. Contra o melhor conjunto:");
    Console.WriteLine();
    Console.WriteLine($"  {"denominacoes",13} {"selos",6} {"potencias de 2",-16} {"vai ate",8} {"o melhor",-16} {"vai ate",8}");

    foreach (var l in Medidas.Binario())
        Console.WriteLine($"  {l.Denominacoes,13} {l.Selos,6} {l.Binario,-16} {l.AlcanceBinario,8} {l.Melhor,-16} {l.AlcanceMelhor,8}");

    Console.WriteLine();
}

static void Juiz()
{
    Console.WriteLine();
    Console.WriteLine("A PROGRAMACAO DINAMICA contra a forca bruta que tenta todas as combinacoes.");
    Console.WriteLine();
    Console.WriteLine($"  {"conjunto",-16} {"selos",6} {"vai ate",8} {"pela forca bruta",17} {"bate",6}");

    foreach (var l in Medidas.ContraOJuiz())
        Console.WriteLine($"  {l.Conjunto,-16} {l.Selos,6} {l.Alcance,8} {l.PorForcaBruta,17} {(l.Bate ? "sim" : "NAO"),6}");

    Console.WriteLine();
}
