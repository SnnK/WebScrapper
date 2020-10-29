using System;
using Business;
using System.Reflection;
using System.IO;
using Models;
using System.Threading.Tasks;

namespace Examples
{
    public class UdemyConsole
    {
        private static Task _task;

        static void Main()
        {
            string command;
            bool quitNow = default(bool);

            Commands();

            while (!quitNow)
            {
                command = Console.ReadLine();

                switch (command)
                {
                    case "/help":
                        Commands();
                        break;
                    case "/quit":
                        Environment.Exit(0);
                        break;
                    case "/info":
                        Console.WriteLine("100% Off Udemy Coupons");
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
            Datas d = new Datas();

            Type mType = typeof(BotList);
            MethodInfo[] methodInfos = mType.GetMethods(BindingFlags.Public |
                           BindingFlags.Static);

            //Stopwatch sw = new Stopwatch();
            //sw.Start();

            foreach (var item in methodInfos)
            {
                var returnValue = ((BotList)item.Invoke(null, null)).Value;
                 _task = Task.Run(() => { d.Collect(returnValue[0], returnValue[1]); });
            }

            _task.Wait();

            //sw.Stop();
            //Console.WriteLine(sw.Elapsed);

            Console.WriteLine("done!");
        }

        static void Commands()
        {
            Console.WriteLine("COMMANDS\n" +
        "/quit >> Close App \n" +
        "/start >> Start App \n" +
        "/info >> About App \n" +
        "/save >> Save coupon codes and other course infos \n" +
        "******************");
        }

        static void Save()
        {
            StreamWriter file = new StreamWriter("courses.txt");

            foreach (var line in Udemy.findCourses)
            {
                string l = String.Format("Title: {0} | Link: {1} | Coupon Code: {2}", line.title, line.udemy_link, line.coupon_code);
                file.WriteLine(l);
            }

            file.Close();

            Console.WriteLine("saved!");
        }
    }
}
