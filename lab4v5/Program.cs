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
            if (amount > 0)
            {
                Balance += amount;
                Console.WriteLine($"[BankAccount] Deposit: {amount:C}. Balance: {Balance:C}");
            }
        }

        public string GetAccountType()
        {
            return "Standard Bank Account";
        }
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
            if (amount > 0)
            {
                Balance += amount;
                Console.WriteLine($"[SavingsAccount] Deposit: {amount:C}. Balance: {Balance:C}");
            }
        }

        public void AddInterest()
        {
            decimal interest = Balance * (InterestRate / 100);
            Balance += interest;
            Console.WriteLine($"[SavingsAccount] Added interest ({InterestRate}%): {interest:C}. Balance: {Balance:C}");
        }

        public new string GetAccountType()
        {
            return "Savings Account";
        }
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
            if (amount > 0)
            {
                Balance += amount;
                Console.WriteLine($"[CheckingAccount] Deposit: {amount:C}. Balance: {Balance:C}");
            }
        }

        public void ProcessCheck()
        {
            Console.WriteLine($"[CheckingAccount] Check processed. Overdraft Limit: {OverdraftLimit:C}");
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            SavingsAccount savings = new SavingsAccount("SA-101", 1000m, 5.5m);
            CheckingAccount checking = new CheckingAccount("CA-202", 500m, 300m);

            savings.AddInterest();
            checking.ProcessCheck();

            BankAccount[] accounts = new BankAccount[]
            {
                new BankAccount("BA-000", 100m),
                savings,
                checking
            };

            foreach (var acc in accounts)
            {
                acc.Deposit(200m);
            }

            SavingsAccount mySavings = new SavingsAccount("SA-999", 2000m, 4.0m);
            BankAccount baseRef = mySavings;

            Console.WriteLine($"SavingsAccount ref: {mySavings.GetAccountType()}");
            Console.WriteLine($"BankAccount ref:    {baseRef.GetAccountType()}");
        }
    }
}