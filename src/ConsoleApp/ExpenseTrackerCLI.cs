using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.CommandLine;
using System.CommandLine.NamingConventionBinder;
using Application.ApplicationCommands;
namespace ConsoleApp
{
    class ExpenseTrackerCLI
    {
        static int Main(string[] args)
        {
            _ = ConfigFileHandler.GetDBName();

            RootCommand rootCommand = new RootCommand();
            Command edit = Edit();
            Command create = Create();
            Command expense = Expense();
            Command income = Income();

            rootCommand.Add(edit);
            rootCommand.Add(create);
            rootCommand.Add(expense);
            rootCommand.Add(income);

            return rootCommand.Invoke(args);
        }

        private static Command Edit()
        {
            Command editTransfer = new Command("transfer", "Edição de um meio de transferência de dinheiro.")
            {
                new Argument<string>("transfer-name", "Nome atual do meio de transferência."),
                new Argument<string>("newName", "Novo Nome."),
                new Argument<string>("newIdentifier", "Novo código de identificação.")
            };
            editTransfer.Handler = CommandHandler.Create<string, string, string>(EditHandler.EditTransfer);


            Command edit = new Command("edit", "Edição de campos de entidade.")
            {
                editTransfer
            };
            return edit;
        }
        private static Command Create()
        {
            Command account = new Command("account", "Criar uma conta")
            {
                new Argument<string>("name", "Identificação da conta (nome)"),
                new Argument<string>("bank", "Nome do banco referente à conta.")
            };
            account.Handler = CommandHandler.Create<string, string>(CreateHandler.CreateAccount);

            Command transfer = new Command("transfer", "Criar meio de transferência de dinheiro.")
            {
                new Argument<string>("account-name", "Nome da conta a qual pertence o meio."),
                new Argument<string>("type", "Pix ou Transference."),
                new Argument<string>("name", "Nome do Transfer."),
                new Argument<string>("identifier", "Número ou código de identificação da conta.")
            };
            transfer.Handler = CommandHandler.Create<string, string, string, string>(CreateHandler.CreateTransfer);


            Command create = new Command("create", "Criação de uma das entidade.")
            {
                account,
                transfer
            };
            return create;
        }
        private static Command Expense()
        {
            Command expense = new Command("expense", "Adicionar um gasto")
            {
                new Argument<string>("account-name", "Nome da conta."),
                new Argument<string>("transfer-name", "Nome do meio que foi utilizado para o gasto."),
                new Argument<double>("value", "Valor gasto."),
                new Option<string>("--tags", description: "Identificadores do gasto.", getDefaultValue: () =>""),
                new Option<string>("--note", description: "Informações adicionais sobre o gasto.", getDefaultValue: () =>""),
                new Option<string>("--date", description: "Data do gasto.", getDefaultValue: () =>"")
            };
            expense.Handler = CommandHandler.Create<string, string, double, string, string, string>(CreateHandler.Expense);
            return expense;
        }
        private static Command Income()
        {
            Command income = new Command("income", "Adicionar uma entrada de recursos")
            {
                new Argument<string>("account-name", "Nome da conta."),
                new Argument<string>("transfer-name", "Nome do meio que foi utilizado para o gasto."),
                new Argument<double>("value", "Valor gasto."),
                new Option<string>("--tags", description: "Identificadores da entrada.", getDefaultValue: () =>""),
                new Option<string>("--note", description: "Informações adicionais sobre a entrada.", getDefaultValue: () =>""),
                new Option<string>("--date", description: "Data da entrada.", getDefaultValue: () =>"")
            };
            income.Handler = CommandHandler.Create<string, string, double, string, string, string>(CreateHandler.Income);
            return income;
        }

    }
}
