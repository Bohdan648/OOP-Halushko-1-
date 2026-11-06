using System;
using System.Collections.Generic;

namespace lab9v5
{
    internal class Program
    {
        static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

           
            Console.WriteLine("=== 1. HashSet: Унікальні одержувачі ===");
            HashSet<string> recipients = new HashSet<string>();

            recipients.Add("user1@gmail.com");
            recipients.Add("user2@gmail.com");
            recipients.Add("user1@gmail.com"); // Дублікат, не додасться

            Console.WriteLine($"Кількість унікальних одержувачів: {recipients.Count}");
            foreach (var email in recipients)
            {
                Console.WriteLine($" - {email}");
            }
            Console.WriteLine($"Чи є user1@gmail.com у списку? {recipients.Contains("user1@gmail.com")}");


           
            Console.WriteLine("\n=== 2. Queue: Черга повідомлень (FIFO) ===");
            Queue<string> messageQueue = new Queue<string>();

            messageQueue.Enqueue("Повідомлення 1: Вітаємо у системі!");
            messageQueue.Enqueue("Повідомлення 2: Ваше замовлення оформлено.");
            messageQueue.Enqueue("Повідомлення 3: Знижка 10% для вас!");

            Console.WriteLine($"Перше в черзі (Peek): {messageQueue.Peek()}");

            Console.WriteLine("Обробка черги:");
            while (messageQueue.Count > 0)
            {
                string msg = messageQueue.Dequeue();
                Console.WriteLine($" [Надіслано] {msg}");
            }


            
            Console.WriteLine("\n=== 3. Stack: Історія повідомлень (LIFO) ===");
            Stack<string> historyStack = new Stack<string>();

            historyStack.Push("Повідомлення 1");
            historyStack.Push("Повідомлення 2");
            historyStack.Push("Повідомлення 3");

            Console.WriteLine($"Останнє надіслане (Peek): {historyStack.Peek()}");

            Console.WriteLine("Перегляд історії (від останнього до першого):");
            while (historyStack.Count > 0)
            {
                string lastMsg = historyStack.Pop();
                Console.WriteLine($" [Історія] {lastMsg}");
            }
        }
    }
}