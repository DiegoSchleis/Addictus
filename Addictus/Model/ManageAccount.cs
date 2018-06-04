using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows;

namespace Addictus
{
    class ManageAccount
    {

        public ManageAccount()
        {

        }

        public bool AddAccount(string username, string password)
        {
            if (username != null && password != null)
            {
                if (!AccountExists(username, password))
                {
                    string usersPath = Path.Combine(Environment.CurrentDirectory, "users.txt");
                    string directoryPath = username;
                    StreamWriter writer = new StreamWriter(usersPath, true);
                    writer.Write("{0};{1}{2}", username, password, Environment.NewLine);
                    writer.Close();
                    Directory.CreateDirectory(directoryPath);
                    return true;
                }
                else
                {
                    MessageBox.Show("Account already exists");
                }
            }
            else
            {
                MessageBox.Show("Username and password must not be empty");
            }
            return false;
        }

        public bool CheckUserValidation(string usernameToCheck, string passwordToCheck)
        {
            if (CreateFileIfNotExists("users.txt"))
            {
                return false;
            }
            else
            {
                string path = Path.Combine(Environment.CurrentDirectory, "users.txt");
                StreamReader reader = new StreamReader(path);
                while (!reader.EndOfStream)
                {
                    string local = reader.ReadLine();
                    List<string> checkList = local.Split(';').ToList();
                    if (checkList.ElementAt(0).Equals(usernameToCheck))
                    {
                        if (checkList.ElementAt(1).Equals(passwordToCheck))
                        {
                            reader.Close();
                            return true;
                        }
                    }
                }
                reader.Close();
                return false;
            }
        }

        public bool AccountExists(string usernameToCheck, string passwordToCheck)
        {
            if (CreateFileIfNotExists("users.txt"))
            {
                return false;
            }
            else
            {
                string path = Path.Combine(Environment.CurrentDirectory, "users.txt");
                StreamReader reader = new StreamReader(path);
                while (!reader.EndOfStream)
                {
                    string local = reader.ReadLine();
                    List<string> checkList = local.Split(';').ToList();
                    if (checkList.ElementAt(0).Equals(usernameToCheck))
                    {
                        reader.Close();
                        return true;
                    }
                }
                reader.Close();
                return false;
            }
        }

        public bool CreateFileIfNotExists(string filename)
        {
            string path = Path.Combine(Environment.CurrentDirectory, filename);
            if (!File.Exists(path))
            {
                var myFile = File.Create(path);
                myFile.Close();

                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
