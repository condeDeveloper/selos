# Selos

Com selos de 6, 9 e 20 centavos, qual o maior valor que **não** dá para postar? A
resposta é 43, e depois dele todo valor dá, para sempre. É o número de Frobenius,
e é a mesma conta das caixas de nuggets do McDonald's, que vinham em 6, 9 e 20.

C# e .NET 8, com três caminhos independentes para a mesma pergunta: um crivo, uma
força bruta recursiva, e a fórmula de Sylvester de 1884, que não é usada em
cálculo nenhum e existe para discordar.

```bash
dotnet test testes/Selos.Testes/Selos.Testes.csproj -c Release    # 68 testes
dotnet run --project ferramentas/Medidor/Medidor.csproj -c Release
```

## Dois selos: a fórmula existe

```
     a    b  maior que nao da  pela formula   buracos  pela formula   bate
     3    5                 7             7         4             4    sim
     4    7                17            17         9             9    sim
     5    8                27            27        14            14    sim
     6   11                49            49        25            25    sim
     7   10                53            53        27            27    sim
     9   20               151           151        76            76    sim
    11   13               119           119        60            60    sim
```

Com dois selos primos entre si, Sylvester resolveu tudo em 1884: o maior valor
inalcançável é `ab - a - b`, e a quantidade de inalcançáveis é `(a-1)(b-1)/2`.

Repare que a segunda fórmula é **exatamente metade da primeira mais um**. Dos
valores de 0 a `ab-a-b`, metade dá e metade não dá, sempre, em qualquer par. Não é
aproximadamente metade: é metade exata, e é por isso que existe uma fórmula tão
curta para uma pergunta que parece precisar de busca.

## Três selos: a fórmula acaba

```
  conjunto          maior que nao da   buracos  o melhor par dentro  o terceiro ajudou
  {6, 9, 20}                      43        22                  151                sim
  {3, 5, 7}                        4         3                    7                sim
  {4, 9, 11}                      14         8                   23                sim
  {5, 8, 12}                      19        10                   27                sim
  {7, 11, 13}                     30        16                   59                sim
  {11, 13, 15}                    64        35                  119                sim
```

Com três valores **não existe fórmula fechada**, e não é por falta de tentativa:
está provado que não existe uma expressão polinomial geral. Sobra varrer.

A última coluna compara com o melhor que daria para fazer usando só dois dos três
selos. O terceiro sempre ajuda, e ajuda muito: no McNugget, 9 e 20 sozinhos deixam
**151** valores impossíveis acima do último buraco; acrescentar a caixa de 6
derruba para 43. Um terço.

## O que o envelope muda

O problema dos selos postais é o mesmo, com uma regra a mais: **no envelope cabem
no máximo h selos**. Agora não adianta um valor ser alcançável se precisar de
quarenta selos, e a pergunta vira "até onde dá para ir sem pular nenhum valor".

```
   denominacoes  selos o melhor conjunto       vai ate  conjuntos varridos
              2      2 {1, 2}                        4                  11
              2      3 {1, 3}                        7                  15
              2      4 {1, 3}                       10                  19
              2      5 {1, 4}                       14                  23
              3      2 {1, 3, 4}                     8                  78
              3      3 {1, 4, 5}                    15                 171
              3      4 {1, 5, 8}                    26                 351
              4      2 {1, 3, 5, 6}                 12                 455
              4      3 {1, 4, 7, 8}                 24               2,300
```

Os melhores conjuntos são achados varrendo todos: 2.300 conjuntos de quatro
valores para a última linha, cada um com o alcance calculado do zero.

E o melhor conjunto **muda com o número de selos do envelope**. Com dois
selos o melhor par é `{1, 2}`; com três, `{1, 3}`; com cinco, `{1, 4}`. Não há um
conjunto bom em geral: há um conjunto bom para cada envelope.

## Dobrar não é a resposta

Qualquer um escolheria potências de 2. Com `{1, 2, 4, 8}` dá para montar qualquer
valor até 15, um selo de cada, e é a base do sistema binário.

```
   denominacoes  selos potencias de 2    vai ate o melhor          vai ate
              3      2 {1, 2, 4}               6 {1, 3, 4}               8
              3      3 {1, 2, 4}              10 {1, 4, 5}              15
              3      4 {1, 2, 4}              14 {1, 5, 8}              26
              4      2 {1, 2, 4, 8}            6 {1, 3, 5, 6}           12
              4      3 {1, 2, 4, 8}           14 {1, 4, 7, 8}           24
```

Com quatro denominações e dois selos no envelope, as potências de 2 vão até 6 e o
melhor conjunto vai até **12: o dobro**. Com três selos, 14 contra 24.

O motivo é que o binário é feito para **usar cada valor no máximo uma vez**, e o
envelope não proíbe repetir selo: proíbe usar muitos. São restrições diferentes, e
a solução ótima de uma não tem nada a ver com a da outra. `{1, 3, 5, 6}` repete
descaradamente — 1+1, 3+3, 5+5, 6+6 — e é justamente isso que a faz ganhar.

## O juiz

```
  conjunto          selos  vai ate  pela forca bruta   bate
  {1, 3}                3        7                 7    sim
  {1, 4, 9}             3        6                 6    sim
  {1, 2, 5}             4       17                17    sim
  {1, 3, 5, 7}          3       15                15    sim
  {1, 5, 8}             4       26                26    sim
```

A programação dinâmica que calcula o custo mínimo de cada valor contra uma
recursão que tenta todas as combinações de até h selos, sem memória nenhuma. A
segunda é inútil e lenta, e é exatamente por isso que ela serve: não compartilha
uma linha de código com a primeira.

## O que as medidas me corrigiram

**Achei que o binário fosse ótimo e ele faz metade.** Potências de 2 é a resposta
que todo mundo dá, inclusive eu, e ela resolve um problema parecido mas diferente:
representar todo número usando cada peça uma vez. Quando o limite é a *quantidade*
de selos e não a repetição, o conjunto certo é outro, e a diferença é um fator de
dois. Eu ia escrever "potências de 2 são a escolha natural" como se fosse elogio.

**Varri até o teto errado antes de pensar.** A primeira versão do crivo ia até
`maior × maior`, para garantir. O teto certo é `menor × maior`: depois do produto
do menor pelo maior não há mais buraco possível. Varrer mais não deixa o número
errado, mas deixa a frase errada no README, e a frase era "varre até onde pode
haver buraco".

**Escrevi que o terceiro selo "pode ajudar".** Em todos os seis conjuntos que
medi, ele ajudou, e muito: o McNugget cai de 151 para 43. A frase cautelosa não
era humildade, era não ter medido.

**Metade exata não era para ser exata.** Ao ver que os buracos eram sempre
`(frobenius + 1) / 2`, achei que fosse coincidência das linhas que eu tinha
escolhido. É teorema, e virou um teste: em todo par de selos, exatamente metade
dos valores antes do último buraco é inalcançável.

## Os arquivos

| arquivo | o que faz |
|---|---|
| `src/Selos/Conjunto.cs` | os valores de selo, o divisor comum, e quando a pergunta faz sentido |
| `src/Selos/Alcance.cs` | o crivo, a força bruta, o número de Frobenius e a fórmula de Sylvester |
| `src/Selos/Postal.cs` | o envelope com h selos: custo mínimo, alcance, e a varredura dos conjuntos |
| `src/Selos/Medidas.cs` | as seis tabelas deste README |
| `ferramentas/Medidor` | imprime as tabelas, uma por comando |

## Rodando

```bash
dotnet test testes/Selos.Testes/Selos.Testes.csproj -c Release

dotnet run --project ferramentas/Medidor/Medidor.csproj -c Release -- sylvester
dotnet run --project ferramentas/Medidor/Medidor.csproj -c Release -- tres
dotnet run --project ferramentas/Medidor/Medidor.csproj -c Release -- nugget
dotnet run --project ferramentas/Medidor/Medidor.csproj -c Release -- postal
dotnet run --project ferramentas/Medidor/Medidor.csproj -c Release -- binario
dotnet run --project ferramentas/Medidor/Medidor.csproj -c Release -- juiz
```

C# 12 e .NET 8. Zero dependências fora do xUnit, e só nos testes.

68 testes. Os números deste README estão presos em `MedidasTestes`.
