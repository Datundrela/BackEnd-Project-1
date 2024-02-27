using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BackEnd_Project_1
{
    internal class Bank
    {
        public static string basePath { get; } = @"C:\Users\sadag\OneDrive\Desktop\Accounts\";


        public static void SerializeAccount(Account account, string filePath)
        {
            try
            {
                JsonSerializerSettings settings = new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                };
                string json = JsonConvert.SerializeObject(account, settings);
                File.WriteAllText(filePath, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error serializing account: {ex.Message}");
            }
        }

        public static Account DeserializeAccount(string filePath)
        {
            Account account = null;
            try
            {
                string json = File.ReadAllText(filePath);
                account = JsonConvert.DeserializeObject<Account>(json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deserializing account: {ex.Message}");
            }
            return account;
        }
        

        public static string FindFileByIBAN(string iban)
        {
            string filePath = Path.Combine(basePath, $"{iban}.txt");
            if (File.Exists(filePath))
            {
                return filePath;
            }
            else
            {
                return null;
            }
        }

        public static string FindFileByID(string id)
        {
            string[] files = Directory.GetFiles(basePath, $"*_{id}.txt");
            if (files.Length > 0)
            {
                return files[0];
            }
            else
            {
                Console.WriteLine($"File with ID '{id}' not found.");
                return null;
            }
        }

        public static int AskForOptions()
        {
            string input = null;
            Console.WriteLine("Please Choose:\n\n1) Send Money\n2) Deposit\n3) Withdraw\n4) See Balance\n5) See Transaction History\n6) Log out");
            string pattern = @"^[1-6]$";
            do
            {
                input = Console.ReadLine();
                if(Regex.IsMatch(input, pattern) == false)
                {
                    Console.WriteLine("Please enter valid option!");
                }
            }while (Regex.IsMatch(input, pattern) == false);
            return int.Parse(input);
        }

        public static int AskForIbanOrID()
        {
            string input = null;
            Console.WriteLine("1) Using Iban\n2) Using ID");
            string pattern = @"^[1-2]$";
            do
            {
                input = Console.ReadLine();
                if (Regex.IsMatch(input, pattern) == false)
                {
                    Console.WriteLine("Please enter valid option!");
                }
            } while (Regex.IsMatch(input, pattern) == false);
            return int.Parse(input);
        }

        public static Decimal AskForAmount()
        {
            string input = null;
            string pattern = @"^[0-9]+(\.[0-9]+)?$";
            Console.WriteLine("Please enter amount:");
            do
            {
                input = Console.ReadLine();
                if (Regex.IsMatch(input, pattern) == false)
                {
                    Console.WriteLine("Please enter valid option!");
                }
            } while (Regex.IsMatch(input, pattern) == false);
            return Decimal.Parse(input);
        }

        public static Account CreateAccount()
        {
            string input = null;
            string pattern = @"^[a-zA-Z\s]+$";
            Console.WriteLine("Please enter your Name and Surname");
            do
            {
                input = Console.ReadLine();
                if(Regex.IsMatch(input, pattern) == false)
                {
                    Console.WriteLine("Invalid input!");
                }
            } while(Regex.IsMatch(input, pattern) == false);
            string name = input;
            Console.WriteLine("Please enter your ID");
            pattern = @"^\d{11}$";
            do
            {
                input = Console.ReadLine();
                if (Regex.IsMatch(input, pattern) == false)
                {
                    Console.WriteLine("Invalid input!");
                }
            } while (Regex.IsMatch(input, pattern) == false);
            string id = input;
            Console.WriteLine("Please create Pin Code");
            pattern = @"^\d{4}$";
            do
            {
                input = Console.ReadLine();
                if (Regex.IsMatch(input, pattern) == false)
                {
                    Console.WriteLine("Invalid input!");
                }
            } while (Regex.IsMatch(input, pattern) == false);
            string pinCode = input;
            return new Account(name, id, pinCode);
        }

        public bool AskForPinCode(string pinCode)
        {
            string input = null;
            do
            {
                Console.WriteLine("Please enter Pin Code");
                string pattern = @"^\d{4}$";
                do
                {
                    input = Console.ReadLine();
                    if (Regex.IsMatch(input, pattern) == false)
                    {
                        Console.WriteLine("Invalid input!");
                    }
                } while (Regex.IsMatch(input, pattern) == false);
                if (input == pinCode)
                {
                    return true;
                }
                else
                {
                    Console.WriteLine("Incorrect Pin Code!");
                }
            } while (input != pinCode);
            return false;
        }

        public void Start()
        {
            bool logIn = true;
            string input;
            Console.WriteLine("Welcome!\n\n1) Log in\n2)Create Account");
            do
            {
                input = Console.ReadLine();
                if(input != "1" && input != "2")
                {
                    Console.WriteLine("Invalid Input!");
                }
            } while(input != "1" && input != "2");
            if(input == "1")
            {
                string id = null;
                Console.WriteLine("Please enter you ID:");
                do
                {
                    id = Console.ReadLine();
                    if (FindFileByID(id) == null)
                    {
                        Console.WriteLine("Such account does not exist.");
                    }
                } while (FindFileByID == null);
                Account account = DeserializeAccount(FindFileByID(id));
                AskForPinCode(account.GetPinCode());
                while (logIn) 
                {
                    switch (AskForOptions())
                    {
                        case 1:
                            Account receiver = null;
                            switch (AskForIbanOrID())
                            {
                                case 1:
                                    Console.WriteLine("Please enter Iban:");
                                    string iban  = Console.ReadLine();
                                    if(FindFileByIBAN(iban) == null)
                                    {
                                        Console.WriteLine("Could not find account.");
                                        break;
                                    }
                                    account.MakeTransactionUsingIban(iban, AskForAmount());
                                    break;
                                case 2:
                                    Console.WriteLine("Please enter ID:");
                                    string receiverId = Console.ReadLine();
                                    if (FindFileByID(receiverId) == null)
                                    {
                                        Console.WriteLine("Could not find account.");
                                        break;
                                    }
                                    account.MakeTransactionUsingID(receiverId, AskForAmount());
                                    break;
                            }
                            break;
                        case 2:
                            account.Deposit(AskForAmount());
                            break;
                        case 3:
                            account.Withdraw(AskForAmount());
                            break;
                        case 4:
                            account.CheckBalance();
                            break;
                        case 5:
                            account.ShowTransactionHistory();
                            break;
                        case 6:
                            logIn = false;
                            break;
                    }
                    SerializeAccount(account, account.GetPath());
                }
            } else if(input == "2")
            {
                Account newAccount = CreateAccount();
                newAccount.GivePathAndIban();
                SerializeAccount(newAccount,newAccount.GetPath());
                this.Start();
            }
        }
    }



    [Serializable]
    internal class Account
    {
        public string Name { get; private set; }
        public string ID { get; private set; }
        public string Iban { get; private set; }
        public string PinCode { get; set; }
        public Decimal Balance { get; set; } = Decimal.Zero;
        public LinkedList<Transaction> TransactionHistory { get; set; }
        public string Path { get; set; }

        public Account(string name, string id, string pinCode)
        {
            Name = name;
            ID = id;
            TransactionHistory = new LinkedList<Transaction>();
            PinCode = pinCode;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"{Name}  {Iban}");
            return sb.ToString();
        }

        public void GivePathAndIban()
        {
            Iban = CreateIban();
            using (FileStream fs = new FileStream(Bank.basePath + @$"{Iban}_{ID}.txt", FileMode.Create)) { }
            Path = Bank.basePath + @$"{Iban}_{ID}.txt";
        }

        public string GetPath()
        {
            return Path;
        }

        public string GetPinCode()
        {
            return PinCode;
        }

        public string CreateIban()
        {
            Random random = new Random();
            string randomString = "";
            for (int i = 0; i < 10; i++)
            {
                randomString += random.Next(0, 10);
            }
            return randomString;
        }

        public void MakeTransactionUsingIban(string iban, Decimal amount)
        {
            Account receiver = null;
            if (Bank.FindFileByIBAN != null && this.Balance >= amount)
            {
                receiver = Bank.DeserializeAccount(Bank.FindFileByIBAN(iban));
                receiver.Balance += amount;
                TransactionHistory.AddLast(new Transaction($"{this.Name}  {this.ID}", $"{receiver.Name}  {receiver.ID}", amount, DateTime.Now));
                receiver.TransactionHistory.AddLast(new Transaction($"{this.Name}  {this.ID}", $"{receiver.Name}  {receiver.ID}", amount, DateTime.Now));
                this.Balance -= amount;
                Bank.SerializeAccount(receiver, receiver.GetPath());
                Console.WriteLine("Transaction executed successfully!");
            }
            else
            {
                 Console.WriteLine($"File with IBAN '{iban}' not found.");
            }
        }

        public void MakeTransactionUsingID(string id, Decimal amount)
        {
            Account receiver = null;
            if (Bank.FindFileByID != null && this.Balance >= amount)
            {
                receiver = Bank.DeserializeAccount(Bank.FindFileByID(id));
                receiver.Balance += amount;
                TransactionHistory.AddLast(new Transaction($"{this.Name}  {this.ID}", $"{receiver.Name}  {receiver.ID}", amount));
                receiver.TransactionHistory.AddLast(new Transaction($"{this.Name}  {this.ID}", $"{receiver.Name}  {receiver.ID}", amount));
                this.Balance -= amount;
                Bank.SerializeAccount(receiver, receiver.GetPath());
                Console.WriteLine("Transaction executed successfully!");
            }
            else
            {
                Console.WriteLine($"File with ID '{id}' not found.");
            }
        }

        public void CheckBalance()
        {
            Console.WriteLine($"Your Balance: ${Balance}");
        }

        public void Deposit(Decimal amount)
        {
            Balance += amount;
            Console.WriteLine($"Deposited ${amount}");
        }

        public void Withdraw(Decimal amount)
        {
            if(Balance >= amount)
            {
                Balance -= amount;
                Console.WriteLine($"Withdrawed ${amount}");
            } else
            {
                Console.WriteLine("Insufficient balance.");
            }
        }

        public void ShowTransactionHistory()
        {
            if (TransactionHistory.Count > 0)
            {
                if(TransactionHistory.Count > 10)
                {
                    for(int i = 1; i <= 10; i++)
                    {
                        Console.WriteLine(TransactionHistory.ElementAt(TransactionHistory.Count - i).ToString());
                    }
                } else
                {
                    for (int i = 1; i <= TransactionHistory.Count; i++)
                    {
                        Console.WriteLine(TransactionHistory.ElementAt(TransactionHistory.Count - i).ToString());
                    }
                }
            }
        }

    }

    [Serializable]
    internal class Transaction
    {
        public string Sender { get; }
        public string Receiver { get; }
        public decimal Amount { get; }


        public Transaction(string sender, string receiver, Decimal amount)
        {
            Sender = sender;
            Receiver = receiver;
            Amount = amount;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"Sender: {Sender};  Receiver: {Receiver};  Amount: {Amount};");
            return sb.ToString();
        }
    }
    
}
