using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class Program
{
    static void Main()
    {
        // Define o caminho do arquivo de entrada (ajuste conforme necessário).
        string filePath = @"C:\\Users\\MICRO\\Documents\\rotas_iniciais.txt";

        // Lê o arquivo e cria a lista de tuplas.
        var rotas = LerArquivoEFormarTuplas(filePath);

        // Exibe as informações no console.
        foreach (var rota in rotas)
        {
            Console.WriteLine($"Rota: {rota.Item1}, Origem: {rota.Item2}, Destino: {rota.Item3}, Valor: {rota.Item4}");
        }

        // Permitir que o usuário insira uma nova rota.
        AdicionarNovaRota(filePath);

        // Permitir que o usuário consulte a menor rota.
        ConsultarMenorRota(rotas);
    }

    public static List<Tuple<string, string, string, int>> LerArquivoEFormarTuplas(string filePath)
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

    public static void AdicionarNovaRota(string filePath)
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

        // Adiciona a nova linha ao arquivo.
        try
        {
            File.AppendAllText(filePath,"\n" + novaLinha);
            Console.WriteLine("Nova rota adicionada com sucesso!");

        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao salvar a nova rota: {ex.Message}");
        }
    }

    public static void ConsultarMenorRota(List<Tuple<string, string, string, int>> rotas)
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
