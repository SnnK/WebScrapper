using System;
using WebScraperApp.Business;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace WebScraperApp.ConsoleApp
{
    public class UdemyConsole
    {
        static void Main()
        {
            Commands();

            while (true)
            {
                string command = Console.ReadLine();

                switch (command)
                {
                    case "/help":
                        Commands();
                        break;
                    case "/quit":
                        Environment.Exit(0);
                        break;
                    case "/info":
                        Console.WriteLine("100% Off Udemy Coupons Finder");
                        break;
                    case "/start":
                        Console.WriteLine("Please Wait!");
                        Start();
                        break;
                    case "/save":
                        Save();
                        break;
                    default:
                        Console.WriteLine("Unknown command | try /help command");
                        break;
                }
            }
        }

        static void Start()
        {
            var tasks = new List<Task>();
            var datas = new Datas();
         
            foreach (var item in BotList.Bots())
            {
                tasks.Add(Task.Factory.StartNew(() =>
                    datas.Collect(item.Url, item.Node)
                ));
            }

            Task.WaitAll(tasks.ToArray());

            Console.WriteLine("done!");
        }

        static void Save()
        {
            var file = new StreamWriter("courses.txt");
            var datas = new Datas();

            foreach (var line in datas.findCourses)
                file.WriteLine($"Title: {line.Title} | Link: {line.Udemy_link} | Coupon Code: {line.Coupon_code}");

            file.Close();

            Console.WriteLine("saved!");
        }

        private static void Commands()
        {
            Console.WriteLine(@"COMMANDS
/quit >> Close App
/start >> Start App
/info >> About App
/save >> Save coupon codes and other course infos
******************");
        }
    }
}