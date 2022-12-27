using System;
using System.IO;
using System.ServiceModel;
using System.Timers;


/*
    class: AuthServer.cs
    author: Kade McGarraghy
    purpose: contains implementation of user services for logging in, registering, and validation
    date last modified: 27/4/21
*/


namespace Authenticator
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, UseSynchronizationContext = false)]
    internal class AuthServer : AuthServerInterface
    {
        
        private Random rand = new Random();
       // private Timer timer;
       // private int xMinutes;

        public string Register(string name, string password)
        {
            string outputStr = name + "," + password;
            string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            bool success = true;

            if (name.Equals("") | password.Equals("")) // username/pass cannot be empty
            {
                success = false;
            }
            else
            {
                try
                {
                    StreamWriter writer = new StreamWriter(Path.Combine(path, "userList.txt"), true);
                    using (writer)
                    {
                        writer.WriteLine(outputStr);
                    }

                }
                catch (IOException e1)
                {
                    success = false;
                    Console.WriteLine("Exception: " + e1.Message);
                    Console.ReadLine();
                }
                catch (ObjectDisposedException e2)
                {
                    success = false;
                    Console.WriteLine("Exception: " + e2.Message);
                }
            }

          

            if (success)
            {
                return "Successfully registered";
            }
            else
            {
                return "Unsuccessfully registered";
            }
        }

        public int Login(string name, string password)
        {
            int token = -1;
            String line;
            bool success = false;

            try
            {
                string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

                StreamReader reader = new StreamReader(Path.Combine(path, "userList.txt"));
                line = reader.ReadLine();
                while (line != null)
                {
                    string[] values = line.Split(',');

                    if (name.Equals(values[0]) && password.Equals(values[1]))
                    {
                        success = true;
                        token = rand.Next(); // token is a random integer
                    }
                    line = reader.ReadLine();
                }
                
                reader.Close();

                if (success)
                {
                    StreamWriter writer = new StreamWriter(Path.Combine(path, "tokenList.txt"), true);
                    {
                        using (writer)
                        {
                            writer.WriteLine(token);
                        }
                    }
                }

            }
            catch (IOException e1)
            {
                Console.WriteLine("Exception: " + e1.Message);
            }
            catch (ObjectDisposedException e2)
            {
                Console.WriteLine("Exception: " + e2.Message);
            }

            return token;

        }


        public string Validate(int token)
        {
            bool validated = false;
            String line;
            try
            {
                string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                StreamReader reader = new StreamReader(Path.Combine(path, "tokenList.txt"));
                line = reader.ReadLine();
                while (line != null)
                {
                    if (token.ToString().Equals(line))
                    {
                        validated = true;
                        break; // want to stop if it already finds a token
                    }
                    line = reader.ReadLine();
                }

                reader.Close();
            }
            catch (IOException e1)
            {
                Console.WriteLine("Exception: " + e1.Message);
            }
            catch (ObjectDisposedException e2)
            {
                Console.WriteLine("Exception: " + e2.Message);
            }

            if (validated)
            {
                return "Successfully validated"; 
            }
            else
            {
                return "Not validated";
            }
        }

        /*public void SetXMinutes(int inXMinutes)
        {
            xMinutes = inXMinutes;
            timer = new Timer(xMinutes * 60 * 1000); // converts time from minutes into ms
            timer.Elapsed += OnTimedEvent;
            timer.AutoReset = true;
            timer.Enabled = true;
        }

        private static void OnTimedEvent(Object src, ElapsedEventArgs e)
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            StreamWriter writer = new StreamWriter(Path.Combine(path, "tokenList.txt"));

            try
            {
                using (writer)
                {
                    writer.WriteLine("");
                }
            }
            catch (IOException e1)
            {
                Console.WriteLine("Exception clearing tokens: " + e1.Message);
            }
            catch (ObjectDisposedException e2)
            {
                Console.WriteLine("Exception clearing tokens: " + e2.Message);
            }

            Console.WriteLine("Token list cleared");
        }*/
    }
}
