using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatabaseManager.DatabaseManagerServiceReference;

namespace DatabaseManager
{
    class Program
    {
        public static DatabaseManagerServiceReference.DatabaseManagerClient proxy = new DatabaseManager.DatabaseManagerServiceReference.DatabaseManagerClient();

        static String token = "";

        static void Main(string[] args)
        {
               
            proxy.Init();
            while (true) {

                while (true) { 
                     printLogInMenu();
                     Console.WriteLine("Your option is:");
                     String o = Console.ReadLine();
                     if (o.Equals("1"))
                     {
                         Console.WriteLine("Input username: ");
                         String username = Console.ReadLine();

                         Console.WriteLine("Input password: ");
                         String password = Console.ReadLine();
                         token = proxy.LogIn(username, password);
                         Console.WriteLine(token);
                         if (token != null)
                         {
                             Console.WriteLine("Successful login!");
                             break;
                         }
                         else {
                             Console.WriteLine("Username or password is invalid");
                         }

                     }
                     else if (o.Equals("x")) {
                         return;
                     }
                     else {
                         Console.WriteLine("Incorrect option.");
                     }
                 }



                while (true) {
                    printMenu();
                    Console.WriteLine("Your option is:");
                    String option = Console.ReadLine();
                    if (option.Equals("1"))
                    {
                        int counter = 1;
                        List<String> info = proxy.GetOutputValue(token).ToList();
                        Console.WriteLine("Id TagName   InitialValue");
                        foreach (String i in info)
                        {
                            String[] tokens = i.Split(':');
                            Console.WriteLine($"{counter}     {tokens[0]}         {tokens[1]}");
                            counter++;
                        }
                    }
                    else if (option.Equals("2"))
                    {
                        int counter = 1;
                        List<String> info = proxy.GetOutputValue(token).ToList();
                        Console.WriteLine("Id TagName   InitialValue");
                        foreach (String i in info)
                        {
                            String[] tokens = i.Split(':');
                            Console.WriteLine($"{counter}     {tokens[0]}         {tokens[1]}");
                            counter++;
                        }
                        int l = -1;
                        while (l < 1 || l > info.Count())
                        {
                            l = InputNumber("Input tagName for changing initialValue: ");
                        }
                        double newValue = InputDoubleNumber("Input new value: ");
                        String tagname = info[l - 1].Split(':')[0];
                        proxy.ChangeOutputValue(tagname, newValue,token);
                    }
                    else if (option.Equals("3"))
                    {
                        while (true)
                        {
                            PrintAddRemoveMenu();
                            String o = Console.ReadLine();
                            if (o.Equals("1"))
                            {
                                List<Object> info = InputTagInfo();
                                Console.WriteLine("sada upisuje");

                                proxy.AddTags(info.ToArray(),token);
                            }
                            else if (o.Equals("2"))
                            {
                                List<String> tags = proxy.GetTagNames(token).ToList();
                                int index = ChooseTagForRemoving(tags);
                                proxy.RemoveTags(tags[index - 1],token);

                            }
                            else if (o.Equals("x"))
                            {
                                break;
                            }
                            else { Console.WriteLine("Incorrect option."); }
                        }


                    }
                    else if (option.Equals("4"))
                    {
                        Console.WriteLine("Input username: ");
                        String username = Console.ReadLine();
                        Console.WriteLine("Input password: ");
                        String password = Console.ReadLine();
                        bool succeed = proxy.RegisterUser(username, password,token);
                        if (succeed)
                        {
                            Console.WriteLine("User is registered!");
                        }
                        else
                        {
                            Console.WriteLine("Username is already in use. Registration failed!");
                        }


                    }
                    else if (option.Equals("5"))
                    {
                        String tagName = getAITagName();

                        Console.WriteLine("------- Alarm input -------------");
                        int id = InputNumber("Input id: ");
                        String typeId = Choose1OR2(GetAlarmTypeMenu());
                        int priority = -1;
                        while (priority < 1 || priority > 3)
                        {
                            priority = InputNumber("Input alarm priority: ");
                        }
                        int keyValue = InputNumber("Input border value: ");
                        List<Object> info = new List<object>
                        {
                            id,
                            typeId,
                            priority,
                            keyValue,
                            tagName
                        };
                        proxy.AddAlarm(info.ToArray(),token);


                    }
                    else if (option.Equals("6")) {
                        String tagName = getInputTagNameForScan();
                        if (tagName != null)
                        {
                            proxy.ChangeScanMode(tagName,token);
                            Console.WriteLine("Successfully completed");
                        }
                        else {
                            Console.WriteLine("There aren't input tags!");
                        }

                    }
                    else if (option.Equals("7"))
                    {
                        proxy.LogOut(token);
                        Console.WriteLine("Successful log out");
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Incorrect input.Try again.");
                    }
                }
            }
        }

            private static string getAITagName()
            {
                while (true) { 
                int counter = 1;
                List<String> info = proxy.GetAITagNames(token).ToList();
                Console.WriteLine("Id TagName  ");
                foreach (String i in info)
                {
                    Console.WriteLine($"{counter}     {i}  ");
                    counter++;
                }
                int option = InputNumber("Input your option: ");
                    if (option > 0 && option < info.Count() + 1) {
                        return info[option - 1];
                    }
                }
            }



        private static string getInputTagNameForScan()
        {
            while (true)
            {
                int counter = 1;
                List<String> info = proxy.GetScanValue(token).ToList();
                if (info.Count != 0) { 
                Console.WriteLine("Id TagName Scan mode ");
                foreach (String i in info)
                {
                    String[] tokens = i.Split(':');
                    Console.WriteLine($"{counter}     {tokens[0]}     {tokens[1]}  ");
                    counter++;
                }
                int option = InputNumber("Input tag for changing scanning mode: ");
                if (option > 0 && option < info.Count() + 1)
                {
                    return info[option - 1].Split(':')[0];
                }
                }
                else { return null; }
            }
        }

        private static int ChooseTagForRemoving(List<String> tags)
        {
            while (true) { 
            Console.WriteLine("Choose tag name for removing : ");
            int counter = 1;
            foreach (String t in tags) {
                Console.WriteLine($"{counter} - {t}");
                counter++;
            }
            int ind = InputNumber("Your option is: ");
            if (ind > 0 && ind < tags.Count() + 1)
            {
                return ind;
            }
            else {
                    Console.WriteLine("Invalid option. Try again.");
            }
            }
        }

        private static void PrintAddRemoveMenu()
        {
            Console.WriteLine("----  Menu  ----");
            Console.WriteLine("1 - Add tag");
            Console.WriteLine("2 - Remove Tag");
            Console.WriteLine("x - End");

        }

        private static List<object> InputTagInfo() {
            String option = InputTagType();
            List<object> info = new List<object>();
            Console.WriteLine("Input tag id:");
            String tagId = Console.ReadLine();
            Console.WriteLine("Input I/O address:");
            String address = Console.ReadLine();
            Console.WriteLine("Input description:");
            String description = Console.ReadLine();
            info.Add(option);
            info.Add(tagId);
            info.Add(address);
            info.Add(description);
            if (option.Equals("3"))
            {
                info.Add( Choose1OR2(GetDriverMenu()));
                info.Add(InputNumber("Input scan time: "));
                return info;
            }
            else if (option.Equals("4"))
            {
                info.Add(InputNumber("Input initial value: "));
                return info;
            }
            else if (option.Equals("1"))
            {
                info.Add(Choose1OR2(GetDriverMenu()));
                info.Add(InputNumber("Input scan time: "));
                info.Add(InputNumber("Input low limit: "));
                info.Add(InputNumber("Input high limit: "));
                return info;


            }
            else {
                info.Add(InputNumber("Input initial value: "));
                info.Add(InputNumber("Input low limit: "));
                info.Add(InputNumber("Input high limit: "));
                return info;

            }
        }

        private static String InputTagType()
        {
            while (true) {
                PrintTagInputMenu();
                Console.WriteLine("Your option is: ");
                String option = Console.ReadLine();
                if ("1234".Contains(option)) { return option; }
                Console.WriteLine("Invalid option.Try again.");
            }

        }
        private static void PrintTagInputMenu()
        {
            Console.WriteLine("----  Add tag  ----");
            Console.WriteLine("1 - Analog Input");
            Console.WriteLine("2 - Analog Output");
            Console.WriteLine("3 - Digital Input");
            Console.WriteLine("4 - Digital Output");
        }

        private static void printMenu()
        {
            Console.WriteLine("Menu for DatabaseManager");
            Console.WriteLine("1 - Get output value");
            Console.WriteLine("2 - Change output value");
            Console.WriteLine("3 - Add/remove tags");
            Console.WriteLine("4 - Register user");
            Console.WriteLine("5 - Add new alarm");
            Console.WriteLine("6 - Change scanning mode");
            Console.WriteLine("7 - Log out");
        }



        private static void printLogInMenu() {
            Console.WriteLine("Menu: ");
            Console.WriteLine("1 - Login");
            Console.WriteLine("x - end");

        }

        private static int InputNumber(String message) {
            while (true) {
                Console.WriteLine(message);
                String input = Console.ReadLine();
                try
                {
                    int num = Int32.Parse(input);
                    return num;
                }
                catch (Exception e) {
                    Console.WriteLine("Invalid input!");
                }
            }
        }

        private static double InputDoubleNumber(String message)
        {
            while (true)
            {
                Console.WriteLine(message);
                String input = Console.ReadLine();
                try
                {
                    double num = Double.Parse(input);
                    return num;
                }
                catch (Exception e)
                {
                    Console.WriteLine("Invalid input!");
                }
            }
        }

        private static String GetDriverMenu()
        {
            return "------ Menu ------------\n" +
                           "1 - Real driver \n" +
                           "2 - Stimulation driver";

        }
       private static String GetAlarmTypeMenu()
            {
                return "------ Menu ------------\n" +
                               "1 - low \n" +
                               "2 - high";
            }
        private static String Choose1OR2(String menu) {
            while (true) { 
                Console.WriteLine(menu);
                String option = Console.ReadLine();
                if ("12".Contains(option) && option.Length==1)
                {
                    return option;
                }
                else {
                    Console.WriteLine("Invalid option!");
                }
            }

        }

    }
}
