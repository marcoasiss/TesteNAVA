
namespace CalculoRotas.Test;

public class CalculoRotasTest
{
    [Fact]
    public void LerArquivoECriarListaDeRotas_ArquivoValido_DeveRetornarListaCorreta()
    {
        // Arrange
        string filePath = @"C:\\Users\\MICRO\\Documents\\test_rotas.txt";
        var linhas = new[]
        {
            "GRU-BRC, GRU, BRC, 10",
            "BRC-SCL, BRC, SCL, 5",
            "GRU-ORL, GRU, ORL, 56"
        };
        File.WriteAllLines(filePath, linhas);

        // Act
        var resultado = Program.CriarListaDeRotas(filePath);

        // Assert
        Assert.Equal(3, resultado.Count);
        Assert.Contains(resultado, r => r.Item1 == "GRU-BRC" && r.Item2 == "GRU" && r.Item3 == "BRC" && r.Item4 == 10);

        // Cleanup
        File.Delete(filePath);
    }

    [Fact]
    public void AdicionarNovaRota_InformacoesValidas_DeveSalvarNoArquivo()
    {
        // Arrange
        string filePath = "test_nova_rota.txt";
        File.WriteAllText(filePath, "");

        string rota = "GRU-BRC";
        string origem = "GRU";
        string destino = "BRC";
        int valor = 10;

        // Act
        string novaLinha = $"{rota},{origem},{destino},{valor}";
        File.AppendAllText(filePath, novaLinha);


        // Assert
        var linhas = File.ReadAllLines(filePath);
        Assert.Single(linhas);
        Assert.Equal("GRU-BRC,GRU,BRC,10", linhas[0]);

        // Cleanup
        File.Delete(filePath);
    }

    [Fact]
    public void ConsultarMenorRota_RotaValida_DeveRetornarRotaCorreta()
    {
        // Arrange
        var rotas = new List<Tuple<string, string, string, int>>
        {
            Tuple.Create("GRU-BRC", "GRU", "BRC", 10),
            Tuple.Create("GRU-ORL", "GRU", "ORL", 56),
            Tuple.Create("GRU-BRC-ORL", "GRU", "ORL", 40)
        };

        string rotaConsulta = "GRU-ORL";
        Console.SetIn(new StringReader(rotaConsulta));

        using var writer = new StringWriter();
        Console.SetOut(writer);

        // Act

        var partes = rotaConsulta.Split('-');

        string origem = partes[0].Trim();
        string destino = partes[1].Trim();

        // Filtra as rotas com a origem e destino especificados e encontra a de menor valor.
        var menorRota = rotas
            .Where(r => r.Item2.Equals(origem, StringComparison.OrdinalIgnoreCase) &&
                        r.Item3.Equals(destino, StringComparison.OrdinalIgnoreCase))
            .OrderBy(r => r.Item4)
            .FirstOrDefault();


        // Assert
        Assert.Contains("GRU-BRC-ORL, GRU, ORL, 40", menorRota.ToString().Trim());
    }
}
