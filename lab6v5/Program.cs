using System;

namespace lab6v5
{
    public class BankAccount
    {
        public string AccountNumber { get; set; }
        public decimal Balance { get; protected set; }

        public BankAccount(string accountNumber, decimal initialBalance)
        {
            AccountNumber = accountNumber;
            Balance = initialBalance;
        }

        public virtual void Deposit(decimal amount)
        {
            Balance += amount;
            Console.WriteLine($"[BankAccount] Поповнено на {amount} UAH. Баланс: {Balance} UAH");
        }

        public string GetAccountType() => "Standard Bank Account";
    }

    public class SavingsAccount : BankAccount
    {
        public decimal InterestRate { get; set; }

        public SavingsAccount(string accountNumber, decimal initialBalance, decimal interestRate)
            : base(accountNumber, initialBalance)
        {
            InterestRate = interestRate;
        }

        public override void Deposit(decimal amount)
        {
            Balance += amount;
            Console.WriteLine($"[SavingsAccount] Поповнено на {amount} UAH (Ощадний рахунок). Баланс: {Balance} UAH");
        }

        public void AddInterest()
        {
            decimal interest = Balance * (InterestRate / 100);
            Balance += interest;
            Console.WriteLine($"[SavingsAccount] Нараховано відсотки ({InterestRate}%): +{interest} UAH. Новий баланс: {Balance} UAH");
        }

        public new string GetAccountType() => "Savings Account";
    }

    public class CheckingAccount : BankAccount
    {
        public decimal OverdraftLimit { get; set; }

        public CheckingAccount(string accountNumber, decimal initialBalance, decimal overdraftLimit)
            : base(accountNumber, initialBalance)
        {
            OverdraftLimit = overdraftLimit;
        }

        public override void Deposit(decimal amount)
        {
            Balance += amount;
            Console.WriteLine($"[CheckingAccount] Поповнено на {amount} UAH (Розрахунковий рахунок). Баланс: {Balance} UAH");
        }

        public void ProcessCheck()
        {
            Console.WriteLine($"[CheckingAccount] Чек оброблено. Ліміт овердрафту: {OverdraftLimit} UAH");
        }
    }

    internal class Program
    {
        static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            SavingsAccount sa = new SavingsAccount("SA-101", 1000m, 5.5m);
            CheckingAccount ca = new CheckingAccount("CA-202", 500m, 300m);

            sa.AddInterest();
            ca.ProcessCheck();

            BankAccount[] accounts = { new BankAccount("BA-000", 100m), sa, ca };
            foreach (var acc in accounts)
            {
                acc.Deposit(200m);
            }

            Console.WriteLine($"SavingsAccount ref: {sa.GetAccountType()}");
            
            BankAccount baseRef = sa;
            Console.WriteLine($"BankAccount ref:    {baseRef.GetAccountType()}");
        }
    }
}