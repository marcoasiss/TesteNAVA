using System.Runtime.CompilerServices;

public class Program
{
    static void Main()
    {
        ExibirMenuDeOpcoes();
    }

    public static void ExibirMenuDeOpcoes(){

        string filePath = LerCaminhoArquivo();     

        // Exibe o menu principal
            Console.WriteLine("Digite a opção que deseja:");
            Console.WriteLine("1 - Adicionar Nova Rota");
            Console.WriteLine("2 - Consultar Menor Rota");
            Console.WriteLine("0 - Sair");

        // Lê a entrada do usuário
        string opcao = Console.ReadLine();

            switch (opcao)
            {
                case "1":
                    AdicionarNovaRota();
                    break;

                case "2":
                    ConsultarMenorRota(filePath);
                    break;

                case "0":
                    Console.WriteLine("Encerrando o programa...");
                    return;

                default:
                    Console.WriteLine("Opção inválida. Tente novamente.\n");
                    break;
            }

    }

    public static List<Tuple<string, string, string, int>> CriarListaDeRotas(string filePath)
    {

        var listaDeRotas = new List<Tuple<string, string, string, int>>();

        // Verifica se o arquivo existe.
        if (!File.Exists(filePath))
        {
            Console.WriteLine("Arquivo não encontrado.");
            return listaDeRotas;
        }

         
        // Lê todas as linhas do arquivo.
        var linhas = File.ReadAllLines(filePath);

        foreach (var linha in linhas)
        {
            // Ignora linhas em branco ou sem dados válidos.
            if (string.IsNullOrWhiteSpace(linha))
                continue;

            // Divide a linha em partes separadas por vírgulas.
            var partes = linha.Split(',').Select(p => p.Trim()).ToArray();

            if (partes.Length == 4)
            {
                string rota = partes[0];
                string origem = partes[1];
                string destino = partes[2];

                if (int.TryParse(partes[3], out int valor))
                {
                    // Adiciona a tupla à lista.
                    listaDeRotas.Add(Tuple.Create(rota, origem, destino, valor));
                }
            }
        }

        return listaDeRotas;
    }

    private static string LerCaminhoArquivo(){
        
        // Define o caminho do arquivo
        string filePath = @"C:\\Users\\MICRO\\Documents\\rotas_iniciais.txt";

        return filePath;
    }

    public static void AdicionarNovaRota()
    {
         
        Console.WriteLine("\nDigite as informações da nova rota:");

        Console.Write("Rota: ");
        string rota = Console.ReadLine()?.Trim();

        Console.Write("Origem: ");
        string origem = Console.ReadLine()?.Trim();

        Console.Write("Destino: ");
        string destino = Console.ReadLine()?.Trim();

        Console.Write("Valor: ");
        if (!int.TryParse(Console.ReadLine()?.Trim(), out int valor))
        {
            Console.WriteLine("Valor inválido. A operação foi cancelada.");
            return;
        }

        // Formata a nova linha.
        string novaLinha = $"{rota},{origem},{destino},{valor}";

        string filePath = LerCaminhoArquivo();

        // Adiciona a nova linha ao arquivo.
        try
        {
            File.AppendAllText(filePath, "\n" + novaLinha);
            Console.WriteLine("Nova rota adicionada com sucesso!");

        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao salvar a nova rota: {ex.Message}");
        }
    }

    public static void ConsultarMenorRota(string filePath)
    {
        Console.WriteLine("\nDigite a rota que deseja consultar (Origem-Destino):");
        string rotaConsulta = Console.ReadLine()?.Trim();

        if (string.IsNullOrEmpty(rotaConsulta) || !rotaConsulta.Contains("-"))
        {
            Console.WriteLine("Rota inválida. A consulta foi cancelada.");
            return;
        }

        var partes = rotaConsulta.Split('-');
        if (partes.Length != 2)
        {
            Console.WriteLine("Formato de rota inválido. Use Origem-Destino.");
            return;
        }   

        string origem = partes[0].Trim();
        string destino = partes[1].Trim();

    
        var rotas = CriarListaDeRotas(filePath);

        // Filtra as rotas com a origem e destino especificados e encontra a de menor valor.
        var menorRota = rotas
            .Where(r => r.Item2.Equals(origem, StringComparison.OrdinalIgnoreCase) &&
                        r.Item3.Equals(destino, StringComparison.OrdinalIgnoreCase))
            .OrderBy(r => r.Item4)
            .FirstOrDefault();

        if (menorRota != null)
        {
            Console.WriteLine($"Menor rota encontrada: Rota: {menorRota.Item1}, Origem: {menorRota.Item2}, Destino: {menorRota.Item3}, Valor: {menorRota.Item4}");
        }
        else
        {
            Console.WriteLine("Nenhuma rota encontrada para a consulta.");
        }
    }
}
