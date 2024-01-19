using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace trabalho_completo
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Xml.Linq;

    class Program
    {
        static void Main()
        {
            Console.WriteLine("Bem-vindo ao Registro de Despesas!");

            // Solicita ao usuário o nome do arquivo para a planilha
            Console.Write("Digite o nome do arquivo para a planilha: ");
            string fileName = Console.ReadLine() + ".csv"; // Adiciona a extensão .csv

            // Cria ou carrega a planilha
            List<string[]> expenses = LoadOrCreateSpreadsheet(fileName);

            bool isRunning = true;
            while (isRunning)
            {
                // Exibe o menu de opções
                Console.WriteLine("\nOpções:");
                Console.WriteLine("1. Adicionar despesa");
                Console.WriteLine("2. Visualizar despesas");
                Console.WriteLine("3. Sair");
                Console.WriteLine("4. Converter para CSV");
                Console.WriteLine("5. Converter para XML");

                // Solicita a escolha do usuário
                Console.Write("Escolha uma opção (1-5): ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        // Adiciona uma nova despesa à planilha
                        AddExpense(expenses);
                        break;
                    case "2":
                        // Exibe todas as despesas na planilha
                        DisplayExpenses(expenses);
                        break;
                    case "3":
                        // Salva a planilha e encerra o programa
                        SaveAndExit(fileName, expenses);
                        isRunning = false;
                        break;
                    case "4":
                        // Converte a planilha em um arquivo CSV
                        ConvertToCSV(fileName, expenses);
                        break;
                    case "5":
                        // Converte a planilha em um arquivo XML
                        ConvertToXML(fileName, expenses);
                        break;
                    default:
                        Console.WriteLine("Opção inválida. Tente novamente.");
                        break;
                }
            }
        }

        // Cria uma nova planilha ou carrega uma existente
        static List<string[]> LoadOrCreateSpreadsheet(string fileName)
        {
            List<string[]> expenses = new List<string[]>();

            if (File.Exists(fileName))
            {
                // Carrega a planilha existente
                Console.WriteLine($"Carregando {fileName}...");
                string[] lines = File.ReadAllLines(fileName);
                foreach (string line in lines)
                {
                    expenses.Add(line.Split(','));
                }
                Console.WriteLine("Planilha carregada com sucesso!");
            }
            else
            {
                // Cria uma nova planilha
                Console.WriteLine($"Criando nova planilha: {fileName}");
                File.WriteAllText(fileName, "Data,Descrição,Valor");
                Console.WriteLine("Nova planilha criada com sucesso!");
            }

            return expenses;
        }

        // Adiciona uma nova despesa à planilha
        static void AddExpense(List<string[]> expenses)
        {
            Console.WriteLine("\nAdicionar Nova Despesa:");

            // Solicita detalhes da despesa
            Console.Write("Data (DD/MM/AAAA): ");
            string date = Console.ReadLine();
            Console.Write("Descrição: ");
            string description = Console.ReadLine();
            Console.Write("Valor: ");
            string amount = Console.ReadLine();

            // Adiciona a despesa à planilha
            expenses.Add(new string[] { date, description, amount });
            Console.WriteLine("Despesa adicionada com sucesso!");
        }

        // Exibe todas as despesas na planilha
        static void DisplayExpenses(List<string[]> expenses)
        {
            Console.WriteLine("\nDespesas Registradas:");

            // Exibe cabeçalho
            Console.WriteLine("Data\t\tDescrição\t\tValor");

            // Exibe cada despesa na planilha
            foreach (string[] expense in expenses)
            {
                Console.WriteLine($"{expense[0]}\t\t{expense[1]}\t\t\t{expense[2]}");
            }
        }

        // Salva a planilha e encerra o programa
        static void SaveAndExit(string fileName, List<string[]> expenses)
        {
            Console.WriteLine($"Salvando {fileName}...");

            // Cria linhas formatadas para cada despesa
            List<string> lines = new List<string>();
            foreach (string[] expense in expenses)
            {
                lines.Add(string.Join(",", expense));
            }

            // Salva as linhas no arquivo
            File.WriteAllLines(fileName, lines);

            Console.WriteLine("Planilha salva com sucesso! O programa será encerrado.");
        }

        // Converte a planilha em um arquivo CSV
        static void ConvertToCSV(string fileName, List<string[]> expenses)
        {
            string csvFileName = Path.ChangeExtension(fileName, "csv");
            Console.WriteLine($"Convertendo para CSV e salvando {csvFileName}...");

            // Cria linhas formatadas para cada despesa
            List<string> lines = new List<string> { "Data,Descrição,Valor" };
            lines.AddRange(expenses.Select(expense => string.Join(",", expense)));

            // Salva as linhas no arquivo CSV
            File.WriteAllLines(csvFileName, lines);

            Console.WriteLine($"Planilha convertida para CSV com sucesso: {csvFileName}");
        }

        // Converte a planilha em um arquivo XML
        static void ConvertToXML(string fileName, List<string[]> expenses)
        {
            string xmlFileName = Path.ChangeExtension(fileName, "xml");
            Console.WriteLine($"Convertendo para XML e salvando {xmlFileName}...");

            // Cria um documento XML
            var xmlDocument = new XDocument(
                new XElement("Expenses",
                    expenses.Select(expense =>
                        new XElement("Expense",
                            new XElement("Date", expense[0]),
                            new XElement("Description", expense[1]),
                            new XElement("Amount", expense[2])
                        )
                    )
                )
            );

            // Salva o documento XML no arquivo
            xmlDocument.Save(xmlFileName);

            Console.WriteLine($"Planilha convertida para XML com sucesso: {xmlFileName}");
        }
    }
}
