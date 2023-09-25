using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReportManager.ReportManagerServiceReference;

namespace ReportManager
{
    public class Program
    {
        public static ReportManagerServiceReference.ReportManagerClient proxy = new ReportManagerClient();
        
        public static void Main(string[] args)
        {
            while (true) {

                printMenu();
                Console.WriteLine("Your option is:");
                String option = Console.ReadLine();

                if (option.Equals("1"))
                {
                    DateTime startDate = InputDate("Input start date: ");
                    DateTime endDate = InputDate("Input end date: ");
                    List<AlarmLog> logs = proxy.Report1(startDate,endDate).ToList();
                    Console.WriteLine(" -------------- Report 1 -------------------");
                    foreach(AlarmLog a in logs) {
                        Console.WriteLine("Activated alarm with priority " + a.Priority + " at " + a.TimeStamp);
                    }
                }
                else if (option.Equals("2"))
                {
                    int priority = -1;
                    while (priority < 1 || priority > 3)
                    {
                        priority = InputNumber("Input alarm priority: ");
                    }
                    List<AlarmLog> logs = proxy.Report2(priority).ToList();
                    Console.WriteLine(" -------------- Report 2 -------------------");
                    foreach (AlarmLog a in logs)
                    {
                        Console.WriteLine("Activated alarm with priority " + a.Priority + " at " + a.TimeStamp);
                    }


                }
                else if (option.Equals("3"))
                {
                    DateTime startDate = InputDate("Input start date: ");
                    DateTime endDate = InputDate("Input end date: ");
                    List<Value> values = proxy.Report3(startDate, endDate).ToList();
                    Console.WriteLine(" -------------- Report 3 -------------------");
                    foreach (Value a in values)
                    {
                        Console.WriteLine("Value of tag "+a.TagId + " is "+ a.InputValue + " at "+a.TimeStamp);
                    }

                }
                else if (option.Equals("4"))
                {
                    List<Value> values = proxy.Report4().ToList();
                    Console.WriteLine(" -------------- Report 4 -------------------");
                    foreach (Value a in values)
                    {
                        Console.WriteLine("Last value of tag " + a.TagId +"("+a.GetType()+")"+ " is " + a.InputValue + " at " + a.TimeStamp);
                    }

                }
                else if (option.Equals("5"))
                {
                    List<Value> values = proxy.Report5().ToList();
                    Console.WriteLine(" -------------- Report 5 -------------------");
                    foreach (Value a in values)
                    {
                        Console.WriteLine("Last value of tag " + a.TagId + "(" + a.GetType() + ")" + " is " + a.InputValue + " at " + a.TimeStamp);
                    }
                }
                else if (option.Equals("6"))
                {
                    int counter = 1;
                    List<String> t = proxy.GetTagNames().ToList();
                    Console.WriteLine(" Id    Tag name");
                    foreach (String tag in t) {
                        Console.WriteLine(counter + " " + tag);
                        counter++;
                    }
                    int option1 = -1;
                    while (option1 < 1 || option1 > t.Count) {
                        option1 = InputNumber("Input tag id: ");
                    }
                    List<Value> values = proxy.Report6(t[option1-1]).ToList();
                    foreach (Value a in values)
                    {
                        Console.WriteLine("Value of tag " + a.TagId + " is " + a.InputValue + " at " + a.TimeStamp);
                    }
                }
                else if (option.Equals("x"))
                {
                    return;
                }
                else {
                    Console.WriteLine("Invalid option");
                }
            }
        }

        private static DateTime InputDate(string v)
        {
            while (true) { 
            Console.WriteLine(v);
            String date = Console.ReadLine();
                try
                {
                    DateTime d = DateTime.Parse(date);
                    return d;
                }
                catch (Exception) {
                    Console.WriteLine("Invalid date.");
                }
            }
        }

        private static void printMenu()
        {
            Console.WriteLine("--------- Report Menu -----------");
            Console.WriteLine("1 - Get activated alarms in certain period sorted by priority and timestamp");
            Console.WriteLine("2 - Get activated alarms of certain priority sorted by timestamp");
            Console.WriteLine("3 - Get all tag values written in certain period sorted by timestamp");
            Console.WriteLine("4 - Get last value of all AI tags sorted by timestamp ");
            Console.WriteLine("5 - Get last value of all DI tags sorted by timestamp ");
            Console.WriteLine("6 - Get all sorted values of certain tag ");
            Console.WriteLine("x - End");

        }

        private static int InputNumber(String message)
        {
            while (true)
            {
                Console.WriteLine(message);
                String input = Console.ReadLine();
                try
                {
                    int num = Int32.Parse(input);
                    return num;
                }
                catch (Exception e)
                {
                    Console.WriteLine("Invalid input!");
                }
            }
        }

    }
}
