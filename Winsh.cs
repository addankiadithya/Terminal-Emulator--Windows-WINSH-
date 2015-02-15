using System;
using System.Collections;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Net.NetworkInformation;

namespace WinShell
{
    class Winsh
    {
        static String promptDis = "WINSH>";
        static Hashtable hlp = new Hashtable();
        static string user = Environment.UserName.ToString();
        static DirectoryInfo cdDefault;
        Winsh()
        {
            Console.Clear();
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Title = "WINSH";
            cdDefault= new DirectoryInfo(Directory.GetCurrentDirectory());
        }
        static void Main(string[] args)
        {
            Winsh w = new Winsh();
            w.initHashMap();
            w.promptatTerm(user);
            Console.Write(user + "@" + promptDis);
            String com = Console.ReadLine().Trim();
            Regex rSpcSpec = new Regex("[ ]{1,}");
            Regex rSpc = new Regex(" ");
            com = rSpcSpec.Replace(com," ");
            while (true)
            {
                 if (com.Trim().Equals("") || com.Equals(null))
                {
                    Console.Write(user + "@" + promptDis);
                    com = Console.ReadLine().Trim();
                    com = rSpcSpec.Replace(com, " ");
                    continue;
                }
                 String origComnd;
                 origComnd=com.Substring(0);
                String[] comSplt = rSpc.Split(origComnd);
                com = comSplt[0].Substring(0).Trim();
                if (com.Contains("cat>>"))
                {
                    com = "cat>>";
                }
                try
                {
                    switch (com.Trim())
                    {
                        case "man":
                            w.manHelp(comSplt);
                            break;
                        case "ls":
                            w.lsFunc(comSplt);
                            break;
                        case "?":
                            int i = 1;
                            foreach (DictionaryEntry ent in hlp)
                            {
                                Console.WriteLine("{0}.\t{1}", i++, ent.Key.ToString());
                            }
                            break;
                        case "cd":
                            w.cdFunc(comSplt);
                            break;
                        case "pwd":
                            w.pwdFunc();
                            break;
                        case "more":
                            w.moreFunc(comSplt);
                            break;
                        case "cp":
                            w.cpFunc(comSplt);
                            break;
                        case "date":
                            w.dateFunc(comSplt);
                            break;
                        case "grep":
                            w.grepFunc(comSplt);
                            break;
                        case "wc":
                            w.wcFunc(comSplt);
                            break;
                        case "mkdir":
                            w.mkdirFunc(comSplt);
                            break;
                        case "ping":
                            w.pingFunc(comSplt);
                            break;
                        case "rmdir":
                            w.rmdirFunc(comSplt);
                            break;
                        case "ps":
                            w.psFunc(comSplt);
                            break;
                        case "clear":
                            Console.Clear();
                            w.promptatTerm(user);
                            break;
                        case "cat":
                            w.catDispFUnc(comSplt);
                            break;
                        case "cat>>":
                            w.catAppendFUnc(origComnd);
                            break;
                        case "rm":
                            w.rmFunc(comSplt);
                            break;
                        case "exit":
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.BackgroundColor = ConsoleColor.Black;
                            return;
                        default:
                            Console.WriteLine("Illegal Command, Please try again.");
                            break;
                    }
                }
                catch (Exception)
                {
                    Console.Write("Please use man for usage");
                }
                Console.Write(user + "@" + promptDis);
                com = Console.ReadLine().Trim();
                com = rSpcSpec.Replace(com, " ");
            }
        }
        static void grepDirectory(String gPattern, DirectoryInfo dir)
        {
            try
            {
                Regex rgx = new Regex(gPattern);
                DirectoryInfo di = new DirectoryInfo(dir.FullName);
                FileInfo[] fi = di.GetFiles("*.*");
                for (int i = 0; i < fi.Length && fi[i].Length>0; i++)
                {
                    FileStream fs = new FileStream(fi[i].FullName, FileMode.Open);
                    StreamReader sr = new StreamReader(fs);
                    string line = "";
                    int lineNum = 1;
                    line = sr.ReadLine();
                    int j = 0;
                    bool flag = false;
                    while (line != null)
                    {
                        String[] splt = rgx.Split(line);
                        if (splt.Length > 1)
                        {
                            flag = true;
                            if (j == 0)
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("FileName : {0} Start", fi[i].FullName);
                                Console.ForegroundColor = ConsoleColor.Green;
                            }
                            Console.Write("Line " + lineNum + ": ");
                            for (int s = 0; s < splt.Length; s++)
                            {
                                Console.Write(splt[s]);
                                if (s != splt.Length - 1)
                                {
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.Write(rgx.ToString());
                                    Console.ForegroundColor = ConsoleColor.Green;
                                }
                            }
                            Console.WriteLine();
                            j++;
                        }
                        line = sr.ReadLine();
                        ++lineNum;
                        if (line == null && flag)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("{0} End", fi[i].FullName);
                            Console.ForegroundColor = ConsoleColor.Green;
                        }
                    }
                    sr.Dispose();
                    sr.Close();
                    fs.Close();
                    Console.WriteLine();
                }
                DirectoryInfo[] diSub = di.GetDirectories();
                for (int dirNum = 0; dirNum < diSub.Length; dirNum++)
                {
                    grepDirectory(gPattern, diSub[dirNum]);
                }
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("Could not open file");
            }
            catch (Exception)
            {
                //Console.WriteLine("Please use man for usage");
            }
        }
        void rmdirFunc(String[] hlpcom)
        {
            DirectoryInfo curD = new DirectoryInfo(Directory.GetCurrentDirectory());
            if (hlpcom.Length > 1)
            {
                try
                {
                    for (int i = 1; i < hlpcom.Length; i++)
                    {
                        if (hlpcom[i].Trim().Length > 0)
                        {
                            if (Directory.Exists(Path.Combine(curD.FullName, hlpcom[i])))
                            {
                                if (Directory.GetFiles(Path.Combine(curD.FullName, hlpcom[i])).Length == 0)
                                {
                                    if (Directory.GetDirectories(Path.Combine(curD.FullName, hlpcom[i])).Length == 0)
                                        Directory.Delete(Path.Combine(curD.FullName, hlpcom[i]));
                                    else
                                    {
                                        Console.Write("Directory :{0} not empty, Continue delete? Yes/No:  ", hlpcom[i]);
                                        String conf = Console.ReadLine().Trim();
                                        if (conf.ToLower().Equals("yes"))
                                        {
                                            Directory.Delete(Path.Combine(curD.FullName, hlpcom[i]), true);
                                        }
                                    }
                                }
                                else
                                {
                                    Console.Write("Directory :{0} not empty, Continue delete? Yes/No:  ", hlpcom[i]);
                                    String conf = Console.ReadLine().Trim();
                                    if (conf.ToLower().Equals("yes"))
                                    {
                                        Directory.Delete(Path.Combine(curD.FullName, hlpcom[i]), true);
                                    }
                                }
                            }
                            else
                                Console.WriteLine("Directory Not found");
                        }
                    }
                }
                catch (ArgumentException)
                {
                    Console.WriteLine("Argument Error");
                }
                catch (Exception)
                {
                    Console.WriteLine("Please check if the directory already exists");
                }
            }
            else
            {
                Console.WriteLine("Improper Usage of rmdir;\"man rmdir\" for usage");
            }
        }
        void mkdirFunc(String[] hlpcom)
        {
            DirectoryInfo curD = new DirectoryInfo(Directory.GetCurrentDirectory());
            if (hlpcom.Length > 1)
            {
                try
                {
                    for (int i = 1; i < hlpcom.Length; i++)
                    {
                        if (!hlpcom[i].Contains("."))
                        {
                            curD.CreateSubdirectory(hlpcom[i]);
                        }
                    }
                }
                catch (ArgumentException)
                {
                    Console.WriteLine("Argument Error");
                }
                catch (Exception)
                {
                    Console.WriteLine("Please check if the directory already exists");
                }
            }
        }
        void initHashMap()
        {
            hlp.Add("ls", "Lists the files in the directory specified\nUsage: ls \"[path]\"");
            hlp.Add("more", "Displays the content that fits on the screen; use \"Enter\" to display more \nUsage: more \"filename\"");
            hlp.Add("cp", "Copies the source file to destination\nUsage: cp \"srcfile[s]\" \"destfile|destFolder\\\";\n Please note the \\ symbol for telling destination as a directory");
            hlp.Add("grep", "Searches for the given pattern within the files in the directory\nUsage: grep \"pattern\"");
            hlp.Add("man", "Display help for command\nUsage: man \"command\"");
            hlp.Add("cd", "Changes the directory to the path specified\nUsage: cd [\"path\"]; with no arguments defaults to initial directory");
            hlp.Add("cat", "Displays the content of the file\nUsage: cat \"file\"\nUse cat>>\"file\" for appending text to the file\n>> creates a new file if it does not exist\nEnter \"^Z\" to finish");
            hlp.Add("wc", "Word cound utility\nUsage: wc \"file[s]\"");
            hlp.Add("rm", "Removes the directory or file specified\nUsage: rm \"file[s]\"");
            hlp.Add("mkdir", "Creates new directory(ies)\nUsage: mkdir \"dirname[s]\"");
            hlp.Add("rmdir", "Deletes the directory\nUsage: rmdir \"dirname[s]\"");
            hlp.Add("pwd", "Display the current directory\nUsage: pwd");
            hlp.Add("ps", "Display the current processes on the system\nUsage: ps");
            hlp.Add("date", "Display the current date\nUsage: date");
            hlp.Add("ping","Pings the given IP\nUsage: ping \"ip\"");
            hlp.Add("clear", "Clears the screen\nUsage: clear");
            hlp.Add("?", "Displays commands implemented on the screen\nUsage: ?");
        }
        void promptatTerm(String user)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Terminal Emulator: v1.0");
            Console.WriteLine("Developer: Addanki Adithya");
            Console.WriteLine("Please type ? at the prompt to check the list of available commands. \nUse \"man command\" to display help regarding the command.\n [] means optional");
            Console.ForegroundColor = ConsoleColor.Green;
        }
        void manHelp(String[] hlpcom)
        {
            if (hlpcom.Length > 1)
            {
                int i = 1;
                bool flg = false;
                foreach (DictionaryEntry ent in hlp)
                {
                    i++;
                    if (hlpcom[1].Equals(ent.Key.ToString()))
                    {
                        Console.WriteLine(ent.Value.ToString());
                        flg = true;
                        break;
                    }
                }
                if (!flg)
                {
                    Console.WriteLine("Command not found.");
                }
            }
        }
        void pwdFunc()
        {
            try
            {
                DirectoryInfo di = new DirectoryInfo(System.IO.Directory.GetCurrentDirectory());
                Console.WriteLine(di.FullName);
            }
            catch (Exception)
            {
                Console.WriteLine("Unexpected Exception");
                Console.WriteLine("\n");
                Console.WriteLine(hlp["pwd"].ToString());
            }
        }
        void cdFunc(String[] hlpcom)
        {
            if (hlpcom.Length >= 1)
            {
                try
                {
                    if(hlpcom.Length==1){
                        Directory.SetCurrentDirectory(cdDefault.FullName);
                    }
                    else if (hlpcom.Length > 1 && !hlpcom[1].Equals(null) && !hlpcom[1].Equals(""))
                    {
                        if (Directory.Exists(hlpcom[1]))
                            Directory.SetCurrentDirectory(hlpcom[1]);
                        else if (hlpcom[1].Equals(".."))
                        {
                            string parent = Directory.GetCurrentDirectory();
                            DirectoryInfo parDirInf = new DirectoryInfo(parent);
                            parent = parent.Substring(0, parent.LastIndexOf("\\"));
                            Directory.SetCurrentDirectory(parent);
                        }
                        else if (hlpcom[1].Equals("."))
                        {

                        }
                        else
                            throw new DirectoryNotFoundException();
                    }
                    else
                        throw new DirectoryNotFoundException();
                }
                catch (DirectoryNotFoundException)
                {
                    Console.WriteLine("Directory not found");
                    Console.WriteLine("\n");
                    Console.WriteLine(hlp["cd"].ToString());
                }
            }
        }
        void dateFunc(String[] hlpcom)
        {
            if (hlpcom.Length == 1)
                Console.WriteLine(System.DateTime.Now);
            else
                Console.WriteLine("man date for usage");
        }
        void pingFunc(String[] hlpcom)
        {
            if (hlpcom.Length == 2){
                Ping p = new Ping();
                PingReply pr = p.Send(hlpcom[1]);
                int i = 1;
                while (i<=4)
                {
                    Console.WriteLine(pr.Status.ToString());
                    pr = p.Send(hlpcom[1]);
                    i++;
                }
            }
             else
                Console.WriteLine("man ping for usage");
        }
        void psFunc(String[] hlpcom)
        {
            if (hlpcom.Length == 1)
            {
                System.Diagnostics.Process[] ps=System.Diagnostics.Process.GetProcesses();
                int i = 0;
                while (ps.Length > 0)
                {
                    Console.WriteLine("Process ID: " + ps[i].Id + "\tProcess Name: " + ps[i].ProcessName );
                    i++;
                }
            }
            else
                Console.WriteLine("man ps for usage");
        }
        void cpFunc(String[] hlpcom)
        {
            DirectoryInfo di = new DirectoryInfo(System.IO.Directory.GetCurrentDirectory());
            if (hlpcom.Length > 2)
            {
                try
                {
                    if (hlpcom.Length >= 3)
                    {
                        for (int i = 1; i < hlpcom.Length - 1; i++)
                        {
                            if (hlpcom[i].Equals(null) || hlpcom[i].Equals("") || !File.Exists(Path.Combine(di.FullName, hlpcom[i])))
                            {
                                throw new FileNotFoundException();
                            }
                        }
                        try
                        {
                            if (hlpcom.Length == 3 && !hlpcom[2].EndsWith("\\"))
                            {
                                if (!hlpcom[1].Equals(null) && !hlpcom[1].Equals("") && File.Exists(Path.Combine(di.FullName, hlpcom[1])))
                                    File.Copy(Path.Combine(di.FullName, hlpcom[1]), Path.Combine(di.FullName, hlpcom[2]));
                                else if (!hlpcom[1].Equals(null) && !hlpcom[1].Equals("") && !File.Exists(Path.Combine(di.FullName, hlpcom[1])))
                                    Console.WriteLine("Source File not found : " + Path.Combine(di.FullName, hlpcom[1]));
                            }
                            else if (hlpcom[hlpcom.Length - 1].EndsWith("\\"))
                            {
                                if (Directory.Exists(Path.Combine(di.FullName, hlpcom[hlpcom.Length - 1])))
                                {
                                    for (int i = 1; i < hlpcom.Length - 1; i++)
                                    {
                                        if (!hlpcom[i].Equals(null) && !hlpcom[i].Equals("") && File.Exists(Path.Combine(di.FullName, hlpcom[i])))
                                            File.Copy(Path.Combine(di.FullName, hlpcom[i]), Path.Combine(di.FullName, hlpcom[hlpcom.Length - 1], hlpcom[i]));
                                        else if (!hlpcom[i].Equals(null) && !hlpcom[i].Equals("") && !File.Exists(Path.Combine(di.FullName, hlpcom[i])))
                                            Console.WriteLine("Source File not found : " + Path.Combine(di.FullName, hlpcom[i]));
                                    }
                                }
                                else
                                    throw new DirectoryNotFoundException();
                            }
                            else
                            {
                            }
                        }
                        catch (DirectoryNotFoundException)
                        {
                            if (hlpcom[hlpcom.Length - 1].EndsWith("\\"))
                            {
                                di.CreateSubdirectory(hlpcom[hlpcom.Length - 1]);
                                for (int i = 1; i < hlpcom.Length - 1; i++)
                                {
                                    if (!hlpcom[i].Equals(null) && !hlpcom[i].Equals("") && File.Exists(Path.Combine(di.FullName, hlpcom[i])))
                                        File.Copy(Path.Combine(di.FullName, hlpcom[i]), Path.Combine(di.FullName, hlpcom[hlpcom.Length - 1], hlpcom[i]));
                                    else if (!hlpcom[i].Equals(null) && !hlpcom[i].Equals("") && !File.Exists(Path.Combine(di.FullName, hlpcom[i])))
                                        Console.WriteLine("Source File not found : " + Path.Combine(di.FullName, hlpcom[i]));
                                }
                            }
                        }
                        catch (ArgumentNullException)
                        {
                            Console.WriteLine("Arguments cannot be null");
                        }
                        catch (FileNotFoundException)
                        {
                            Console.WriteLine("Source file not found");
                        }
                        catch (Exception)
                        {
                            for (int i = 1; i < hlpcom.Length - 1; i++)
                            {
                                Console.Write("Destination file already exists. Over-Write? Yes/No:  ");
                                String conf = Console.ReadLine().Trim();
                                if (conf.ToLower().Equals("yes"))
                                {
                                    if (!hlpcom[i].Equals(null) && !hlpcom[i].Equals("") && File.Exists(Path.Combine(di.FullName, hlpcom[i])))
                                    {
                                        if (!hlpcom[hlpcom.Length - 1].EndsWith("\\"))
                                            File.Copy(Path.Combine(di.FullName, hlpcom[i]), Path.Combine(di.FullName, hlpcom[hlpcom.Length - 1]), true);
                                        else if (hlpcom[hlpcom.Length - 1].EndsWith("\\"))
                                            File.Copy(Path.Combine(di.FullName, hlpcom[i]), Path.Combine(di.FullName, hlpcom[hlpcom.Length - 1], hlpcom[i]), true);
                                    }
                                    else if ((!hlpcom[i].Equals(null) && !hlpcom[i].Equals("") && !File.Exists(Path.Combine(di.FullName, hlpcom[i]))))
                                        Console.WriteLine("Source File not found : " + Path.Combine(di.FullName, hlpcom[i]));
                                }
                            }
                        }
                    }
                    else
                        throw new Exception();
                }
                catch (FileNotFoundException)
                {
                    Console.WriteLine("Source file not found");
                }
                catch (Exception)
                {
                    Console.WriteLine("Illegal Arguments");
                    Console.WriteLine("\n");
                    Console.WriteLine(hlp["cp"].ToString());
                }

            }
            else
            {
                Console.WriteLine("Illegal Arguments");
                Console.WriteLine("\n");
                Console.WriteLine(hlp["cp"].ToString());
            }
        }
        void moreFunc(String[] hlpcom)
        {
            if (hlpcom.Length == 2)
            {
                try
                {
                    FileStream fs = new FileStream(hlpcom[1], FileMode.Open);
                    StreamReader sr = new StreamReader(fs);
                    string line;
                    line = sr.ReadLine();
                    int numofLines = 1;
                    while (line != null)
                    {
                        ++numofLines;
                        line = sr.ReadLine();
                    }
                    sr.Dispose();
                    sr.Close();
                    fs = new FileStream(hlpcom[1], FileMode.Open);
                    sr = new StreamReader(fs);
                    for (int i = 0; i < (numofLines > 10 ? 10 : numofLines); i++)
                    {
                        line = sr.ReadLine();
                        Console.WriteLine(line);
                    }
                    int curLines = (numofLines > 10 ? 10 : numofLines);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("{0}%", ((curLines * 100) / numofLines));
                    Console.ForegroundColor = ConsoleColor.Green;
                    for (line = sr.ReadLine(); line != null; line = sr.ReadLine())
                    {
                        Console.ReadKey();
                        Console.WriteLine(line);
                        if (curLines == numofLines - 2)
                            ++curLines;
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("{0}%", (((++curLines) * 100) / numofLines));
                        Console.ForegroundColor = ConsoleColor.Green;
                    }
                    sr.Dispose();
                    sr.Close();
                }
                catch (FileNotFoundException)
                {
                    Console.WriteLine("Could not open file");
                }
            }
            else
            {
                Console.WriteLine("Improper Usage of more; \"man more\" for usage help");
            }
        }
        void catDispFUnc(String[] hlpcom)
        {
            if (hlpcom.Length > 1)
            {
                try
                {
                    for (int i = 1; i < hlpcom.Length; i++)
                    {
                        if (hlpcom[i].Length > 0 )
                        {
                            FileStream fs = new FileStream(hlpcom[i], FileMode.Open);
                            StreamReader sr = new StreamReader(fs);
                            Console.WriteLine("          -{0} Start-          ", hlpcom[i]);
                            string line = "";
                            line = sr.ReadLine();
                            int numofLines = 1;
                            while (line != null)
                            {
                                ++numofLines;
                                Console.WriteLine(line);
                                line = sr.ReadLine();
                            }
                            Console.WriteLine("          -{0} End-          ", hlpcom[i]);
                            sr.Dispose();
                            sr.Close();
                            fs.Close();
                        }
                    }
                }
                catch (FileNotFoundException)
                {
                    Console.WriteLine("Could not open file");
                }
                catch (Exception)
                {
                    Console.WriteLine("Argument Error");
                }
            }
        }
        void catAppendFUnc(String com)
        {
            Regex delim = new Regex(">>");
            String[] hlpcom = delim.Split(com.Trim());
            if (hlpcom.Length == 2 && hlpcom[1].Split(' ').Length == 1)
            {
                try
                {
                    FileStream fs = new FileStream(hlpcom[1], FileMode.Append);
                    StreamWriter sr = new StreamWriter(fs);
                    string line;
                    line = Console.ReadLine();
                    while (!line.Equals("^Z"))
                    {
                        sr.WriteLine(line);
                        line = Console.ReadLine();
                    }
                    sr.Dispose();
                    sr.Close();
                    fs.Close();
                }
                catch (FileNotFoundException)
                {
                    Console.WriteLine("Could not open file");
                }
                catch (ArgumentException)
                {
                    Console.WriteLine("Argument Error");
                }
            }
            else
            {
                Console.WriteLine("Improper Usage of \"cat>>\", use \"man cat\" for help");
            }
        }
        void grepFunc(String[] hlpcom)
        {
            if (hlpcom.Length == 2)
            {
                try
                {
                    String curDir = Directory.GetCurrentDirectory();
                    DirectoryInfo di = new DirectoryInfo(curDir);
                    grepDirectory(hlpcom[1], di);
                }
                catch (FileNotFoundException)
                {
                    Console.WriteLine("Could not open file");
                }
            }
            else
                Console.WriteLine("Improper Usage, Please try again.");
        }
        void rmFunc(String[] hlpcom)
        {
            if (hlpcom.Length > 1)
            {
                try
                {
                    for (int i = 1; i < hlpcom.Length; i++)
                    {
                        if (hlpcom[i].Length > 0)
                        {
                            if (File.Exists(hlpcom[i]))
                            {
                                File.Delete(hlpcom[i]);
                            }
                            else if (!File.Exists(hlpcom[i]) && hlpcom[i].Length > 0)
                                Console.WriteLine("File Not Found: " + hlpcom[i]);
                        }
                        else
                            Console.WriteLine("Improper Usage of rm; \"man rm\" for help");
                    }
                }
                catch (FileNotFoundException)
                {
                    Console.WriteLine("Could not remove file");
                }
                catch (ArgumentException)
                {
                    Console.WriteLine("Argument Error");
                }
            }
            else
            {
                Console.Write("Improper Usage of rm; \"man rm\" for help");
            }
        }
        void wcFunc(String[] hlpcom)
        {
            Regex rSpc = new Regex(" ");
            DirectoryInfo di = new DirectoryInfo(System.IO.Directory.GetCurrentDirectory());
            if (hlpcom.Length > 1)
            {
                try
                {
                    for (int i = 1; i < hlpcom.Length; i++)
                    {
                        DirectoryInfo curdD = new DirectoryInfo(Directory.GetCurrentDirectory());
                        if (hlpcom[i].Length>0 && File.Exists(Path.Combine(curdD.FullName, hlpcom[i])))
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("          -{0} Start-          ", hlpcom[i]);
                            Console.ForegroundColor = ConsoleColor.Green;
                            FileStream fs = new FileStream(Path.Combine(curdD.FullName, hlpcom[i]), FileMode.Open);
                            StreamReader sr = new StreamReader(fs);
                            String line = sr.ReadLine();
                            string[] words = rSpc.Split(line);
                            int numofLines = 0;
                            int numWords = 0;
                            int numChars = 0;
                            while (line != null)
                            {
                                ++numofLines;
                                words = rSpc.Split(line);

                                for (int wl = 0; wl < words.Length; wl++)
                                {
                                    if (words[wl].Length > 0)
                                        ++numWords;
                                    numChars += words[wl].Length;
                                }
                                numChars += (words.Length - 1);
                                line = sr.ReadLine();
                            }
                            Console.WriteLine("Lines: " + numofLines);
                            Console.WriteLine("Words: " + numWords);
                            Console.WriteLine("Chars: " + numChars);
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("          -{0} End-          ", hlpcom[i]);
                            Console.ForegroundColor = ConsoleColor.Green;
                            sr.Dispose();
                            sr.Close();
                            fs.Close();
                        }
                    }
                }
                catch (FileNotFoundException)
                {
                    Console.WriteLine("Could not open file");
                }
                catch (ArgumentException)
                {
                    Console.WriteLine("Argument Error");
                }
            }
            else 
                Console.WriteLine("Please man wc for proper usage");
        }
        void lsFunc(String[] hlpcom)
        {
            if (hlpcom.Length >= 1)
            {
                try
                {
                    DirectoryInfo di = new DirectoryInfo(System.IO.Directory.GetCurrentDirectory());
                    if (hlpcom.Length > 1 && !hlpcom[1].Equals(null) && !hlpcom[1].Equals(""))
                    {
                        di = new DirectoryInfo(hlpcom[1]);
                    }
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("{0:15D}\t{1}\t{2}", "File/DirName", "Size", "LastModified");
                    Console.ForegroundColor = ConsoleColor.Green;
                    foreach (FileInfo fi in di.GetFiles())
                    {
                        Console.WriteLine("{0:15D}\t{1}\t{2}", fi.Name, fi.Length, fi.LastWriteTime);
                    }
                    foreach (DirectoryInfo diint in di.GetDirectories())
                    {
                        Console.WriteLine("{0:15D}\t{1}\t{2}", diint.Name, "Dir", diint.LastWriteTime);
                    }
                }
                catch (ArgumentException)
                {
                    Console.WriteLine("Insufficient Arguments");
                    Console.WriteLine("\n");
                    Console.WriteLine(hlp["ls"].ToString());
                }
                catch (DirectoryNotFoundException)
                {
                    Console.WriteLine("Directory is  not found");
                    Console.WriteLine("\n");
                    Console.WriteLine(hlp["ls"].ToString());
                }
                catch (Exception)
                {
                    Console.WriteLine("Unexpected Exception");
                    Console.WriteLine("\n");
                    Console.WriteLine(hlp["ls"].ToString());
                }
            }
        }
    }
}