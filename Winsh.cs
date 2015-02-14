using System;
using System.Collections;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace WinShell
{
    class Winsh
    {
         static String promptDis="WINSH>";
        static void Main(string[] args)
        {
            Hashtable hlp = new Hashtable();
            hlp.Add("ls", "Lists the files in the directory specified\nUsage: ls \"[path]\"");
            hlp.Add("more", "Displays the content that fits on the screen; use \"Enter\" to display more \nUsage: more \"filename\"");
            hlp.Add("cp", "Copies the source file to destination\nUsage: cp \"srcfile[s]\" \"destfile|destFolder\\\";\n Please note the \\ symbol for telling destination as a directory");
            hlp.Add("grep", "Searches for the given pattern within the files in the directory\nUsage: grep \"pattern\"");
            hlp.Add("man", "Display help for command\nUsage: man \"command\"");
            hlp.Add("cd", "Changes the directory to the path specified (relative to current directory only)\nUsage: cd \"path\"");
            hlp.Add("cat", "Displays the content of the file\nUsage: cat \"file\"\nUse cat>>\"file\" for appending text to the file\n>> creates a new file if it does not exist");
            hlp.Add("wc", "Word cound utility\nUsage: wc \"file[s]\"");
            hlp.Add("rm", "Removes the directory or file specified\nUsage: rm \"file[s]\"");
            hlp.Add("mkdir", "Creates new directory(ies)\nUsage: mkdir \"dirname[s]\"");
            hlp.Add("rmdir", "Deletes the directory\nUsage: rmdir \"dirname[s]\"");
            hlp.Add("pwd","Display the current directory\nUsage: pwd");
            hlp.Add("clear", "Clears the screen\nUsage: clear");
            string user = Environment.UserName.ToString();
            Console.Clear();
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Title = "WINSH";
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Terminal Emulator: v1.0");
            Console.WriteLine("Developer: Addanki Adithya");
            Console.WriteLine("Please type ? at the prompt to check the list of available commands. \nUse \"man command\" to display help regarding the command.\n [] means optional");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(user+"@"+promptDis);
            String com=Console.ReadLine();
            Regex rSpc = new Regex(" ");
            while (true)
            {
                if (com.Equals("") || com.Equals(null))
                {
                    Console.Write(user + "@" + promptDis);
                    com = Console.ReadLine();
                    continue;
                }
                else if (com.Equals("?"))
                {
                    int i = 1;
                    foreach (DictionaryEntry ent in hlp)
                    {
                        Console.WriteLine((i++)+". "+ent.Key.ToString());
                    }
                    Console.Write(user + "@" + promptDis);
                    com = Console.ReadLine();
                }
                else if (com.Equals("exit"))
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.BackgroundColor = ConsoleColor.Black;
                    return;
                }
                else if (com.Equals("clear"))
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Terminal Emulator: v1.0");
		            Console.WriteLine("Developer: Addanki Adithya");
		            Console.WriteLine("Please type ? at the prompt to check the list of available commands. \nUse \"man command\" to display help regarding the command.\n [] means optional");
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write(user + "@" + promptDis);
                    com = Console.ReadLine();
                }
                else if (com.StartsWith("man "))
                {
                    String[] hlpcom = rSpc.Split(com);
                    if (hlpcom.Length > 1)
                    {
                        int i=1;
                        bool flg = false;
                        foreach (DictionaryEntry ent in hlp)
                        {
                            i++;
                            if (hlpcom[1].Equals(ent.Key.ToString())){
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
                    Console.Write(user + "@" + promptDis);
                    com = Console.ReadLine();
                }
                else if (com.StartsWith("ls"))
                {
                    String[] hlpcom = rSpc.Split(com);
                    
                    if (hlpcom.Length >= 1)
                    {
                        try
                        {
                            DirectoryInfo di = new DirectoryInfo(System.IO.Directory.GetCurrentDirectory());
                            if (hlpcom.Length>1 && !hlpcom[1].Equals(null) && !hlpcom[1].Equals(""))
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
                    Console.Write(user + "@" + promptDis);
                    com = Console.ReadLine();
                }
                else if (com.StartsWith("pwd") && com.Trim().Equals("pwd"))
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
                    Console.Write(user + "@" + promptDis);
                    com = Console.ReadLine();
                }
                else if (com.StartsWith("cd"))
                {
                    String[] hlpcom = rSpc.Split(com);
                    if (hlpcom.Length >= 1)
                    {
                        try
                        {
                            if (hlpcom.Length > 1 && !hlpcom[1].Equals(null) && !hlpcom[1].Equals(""))
                            {
                                if (Directory.Exists(hlpcom[1]))
                                    Directory.SetCurrentDirectory(hlpcom[1]);
                                else if(hlpcom[1].Equals(".."))
                                {
                                    string parent = Directory.GetCurrentDirectory();
                                    DirectoryInfo parDirInf = new DirectoryInfo(parent);
                                    parent=parent.Substring(0,parent.LastIndexOf("\\"));
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
                        Console.Write(user + "@" + promptDis);
                        com = Console.ReadLine();
                    }
                }
                else if (com.StartsWith("cp "))
                {
                    String[] hlpcom = rSpc.Split(com);
                    DirectoryInfo di = new DirectoryInfo(System.IO.Directory.GetCurrentDirectory());
                    if (hlpcom.Length > 2)
                    {
                        try
                        {
                            if (hlpcom.Length >= 3)
                            {
                                for (int i = 1; i < hlpcom.Length-1; i++)
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
                                    	if(!hlpcom[1].Equals(null) && !hlpcom[1].Equals("") && File.Exists(Path.Combine(di.FullName, hlpcom[1])))
                                        	File.Copy(Path.Combine(di.FullName, hlpcom[1]), Path.Combine(di.FullName, hlpcom[2]));
                                        else if(!hlpcom[1].Equals(null) && !hlpcom[1].Equals("") && !File.Exists(Path.Combine(di.FullName, hlpcom[1])))
                                        	Console.WriteLine("Source File not found : "+ Path.Combine(di.FullName, hlpcom[1]));
                                    }
                                    else if (hlpcom[hlpcom.Length - 1].EndsWith("\\"))
                                    {
                                        if (Directory.Exists(Path.Combine(di.FullName, hlpcom[hlpcom.Length - 1])))
                                        {
                                            for (int i = 1; i < hlpcom.Length - 1; i++)
                                            {
                                            	if(!hlpcom[i].Equals(null) && !hlpcom[i].Equals("") && File.Exists(Path.Combine(di.FullName, hlpcom[i])))
                                        	        File.Copy(Path.Combine(di.FullName, hlpcom[i]), Path.Combine(di.FullName, hlpcom[hlpcom.Length - 1], hlpcom[i]));
                                        	    else if(!hlpcom[i].Equals(null) && !hlpcom[i].Equals("") && !File.Exists(Path.Combine(di.FullName, hlpcom[i])))
                                        	    	Console.WriteLine("Source File not found : "+ Path.Combine(di.FullName, hlpcom[i]));
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
                                        	if(!hlpcom[i].Equals(null) && !hlpcom[i].Equals("") && File.Exists(Path.Combine(di.FullName, hlpcom[i])))
                                        	    File.Copy(Path.Combine(di.FullName, hlpcom[i]), Path.Combine(di.FullName, hlpcom[hlpcom.Length - 1], hlpcom[i]));
                                        	else if(!hlpcom[i].Equals(null) && !hlpcom[i].Equals("") && !File.Exists(Path.Combine(di.FullName, hlpcom[i])))
                                        	    	Console.WriteLine("Source File not found : "+ Path.Combine(di.FullName, hlpcom[i]));
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
                                        String conf = Console.ReadLine();
                                        if (conf.ToLower().Equals("yes"))
                                        {
                                        	if(!hlpcom[i].Equals(null) && !hlpcom[i].Equals("") && File.Exists(Path.Combine(di.FullName, hlpcom[i])))
                                        	{
	                                            if (!hlpcom[hlpcom.Length - 1].EndsWith("\\"))
	                                                File.Copy(Path.Combine(di.FullName, hlpcom[i]), Path.Combine(di.FullName, hlpcom[hlpcom.Length - 1]),true);
	                                            else if (hlpcom[hlpcom.Length - 1].EndsWith("\\"))
	                                                File.Copy(Path.Combine(di.FullName, hlpcom[i]), Path.Combine(di.FullName, hlpcom[hlpcom.Length - 1], hlpcom[i]), true);
                                            }
                                            else if((!hlpcom[i].Equals(null) && !hlpcom[i].Equals("") && !File.Exists(Path.Combine(di.FullName, hlpcom[i]))))
                                        	    	Console.WriteLine("Source File not found : "+ Path.Combine(di.FullName, hlpcom[i]));
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
                        Console.Write(user + "@" + promptDis);
                        com = Console.ReadLine();
                    }
                    else{
                        Console.WriteLine("Illegal Arguments");
                        Console.WriteLine("\n");
                        Console.WriteLine(hlp["cp"].ToString());
                        Console.Write(user + "@" + promptDis);
                        com = Console.ReadLine();
                    }
                }
                else if (com.StartsWith("more "))
                {
                    String[] hlpcom = rSpc.Split(com);
                    if (hlpcom.Length==2)
                    {
                        try
                        {
                            FileStream fs = new FileStream(hlpcom[1],FileMode.Open);
                            StreamReader sr = new StreamReader(fs);
                            string line;
                            line=sr.ReadLine();
                            int numofLines = 1;
                            while (line!=null)
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
                            int curLines=(numofLines > 10 ? 10 : numofLines);
                            Console.WriteLine("------{0}%------", ((curLines * 100 )/ numofLines));

                            for (line = sr.ReadLine(); line != null;line=sr.ReadLine() )
                            {
                                Console.ReadKey();
                                Console.WriteLine(line);
                                if (curLines == numofLines - 2)
                                    ++curLines;
                                Console.WriteLine("------{0}%------", (((++curLines) * 100) / numofLines));
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
                    Console.Write(user + "@" + promptDis);
                    com = Console.ReadLine();
                }
                else if (com.StartsWith("cat "))
                {
                    String[] hlpcom = rSpc.Split(com);
                    if (hlpcom.Length > 1)
                    {
                        try
                        {
                            for (int i = 1; i < hlpcom.Length; i++)
                            {
                            	if(hlpcom[i].Length>0){
	                                FileStream fs = new FileStream(hlpcom[i], FileMode.Open);
	                                StreamReader sr = new StreamReader(fs);
	                                Console.WriteLine("          -{0} Start-          ", hlpcom[i]);
	                                string line = "";
	                                line = sr.ReadLine();
	                                int numofLines = 1;
	                                while (line!=null)
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
                        catch (ArgumentException)
                        {
                            Console.WriteLine("Argument Error");
                        }
                    }
                    Console.Write(user + "@" + promptDis);
                    com = Console.ReadLine();
                }
                else if (com.StartsWith("cat>>"))
                {
                    Regex delim=new Regex(">>");
                    String[] hlpcom = delim.Split(com.Trim());
                    if (hlpcom.Length==2 && hlpcom[1].Split(' ').Length==1)
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
                    Console.Write(user + "@" + promptDis);
                    com = Console.ReadLine();
                }
                else if (com.StartsWith("grep "))
                {
                    String[] hlpcom = rSpc.Split(com);
                    if (hlpcom.Length ==2)
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
                    Console.Write(user + "@" + promptDis);
                    com = Console.ReadLine();
                }
                else if (com.StartsWith("wc "))
                {
                    String[] hlpcom = rSpc.Split(com);
                    DirectoryInfo di = new DirectoryInfo(System.IO.Directory.GetCurrentDirectory());
                    if (hlpcom.Length > 1)
                    {
                        try
                        {
                            for (int i = 1; i < hlpcom.Length; i++)
                            {
                                DirectoryInfo curdD=new DirectoryInfo(Directory.GetCurrentDirectory());
                                if (File.Exists(Path.Combine(curdD.FullName, hlpcom[i])))
                                {
                                    Console.WriteLine("          -{0} Start-          ", hlpcom[i]);
                                    FileStream fs = new FileStream(Path.Combine(curdD.FullName, hlpcom[i]), FileMode.Open);
                                    StreamReader sr = new StreamReader(fs);
                                    String line=sr.ReadLine();
                                    string[] words = rSpc.Split(line);
                                    int numofLines = 0;
                                    int numWords = 0;
                                    int numChars = 0;
                                    while (line!=null)
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
                        catch (ArgumentException)
                        {
                            Console.WriteLine("Argument Error");
                        }
                    }
                    Console.Write(user + "@" + promptDis);
                    com = Console.ReadLine();
                }
                else if (com.StartsWith("rm "))
                {
                    String[] hlpcom = rSpc.Split(com);
                    if (hlpcom.Length > 1)
                    {
                        try
                        {
                            for (int i = 1; i < hlpcom.Length; i++)
                            {
                            	if(hlpcom[i].Length>0)
                            	{
	                                if (File.Exists(hlpcom[i]))
	                                {
	                                    File.Delete(hlpcom[i]);
	                                }
	                                else if(!File.Exists(hlpcom[i]) && hlpcom[i].Length>0 )
	                                    Console.WriteLine("File Not Found: "+hlpcom[i]);
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
                    Console.Write(user + "@" + promptDis);
                    com = Console.ReadLine();
                }
                else if (com.StartsWith("mkdir "))
                {
                    String[] hlpcom = rSpc.Split(com);
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
                        catch(Exception)
                        {
                            Console.WriteLine("Please check if the directory already exists");
                        }
                    }
                    Console.Write(user + "@" + promptDis);
                    com = Console.ReadLine();
                }
                else if (com.StartsWith("rmdir "))
                {
                    String[] hlpcom = rSpc.Split(com);
                    DirectoryInfo curD = new DirectoryInfo(Directory.GetCurrentDirectory());
                    if (hlpcom.Length > 1)
                    {
                        try
                        {
                            for (int i = 1; i < hlpcom.Length; i++)
                            {
                            	if(hlpcom[i].Trim().Length>0){
	                                if (Directory.Exists(Path.Combine(curD.FullName, hlpcom[i])))
	                                {
	                                    if (Directory.GetFiles(Path.Combine(curD.FullName, hlpcom[i])).Length == 0)
	                                    {
	                                        if (Directory.GetDirectories(Path.Combine(curD.FullName, hlpcom[i])).Length == 0)
	                                            Directory.Delete(Path.Combine(curD.FullName, hlpcom[i]));
	                                        else
	                                        {
	                                            Console.Write("Directory :{0} not empty, Continue delete? Yes/No:  ", hlpcom[i]);
	                                            String conf = Console.ReadLine();
	                                            if (conf.ToLower().Equals("yes"))
	                                            {
	                                                Directory.Delete(Path.Combine(curD.FullName, hlpcom[i]), true);
	                                            }
	                                        }
	                                    }
	                                    else
	                                    {
	                                        Console.Write("Directory :{0} not empty, Continue delete? Yes/No:  ", hlpcom[i]);
	                                        String conf = Console.ReadLine();
	                                        if (conf.ToLower().Equals("yes"))
	                                        {
	                                            Directory.Delete(Path.Combine(curD.FullName, hlpcom[i]), true);
	                                        }
	                                    }
	                                }
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
                    Console.Write(user + "@" + promptDis);
                    com = Console.ReadLine();
                }
                else
                {
                    Console.WriteLine("Illegal Command, Please try again.");
                    Console.Write(user + "@" + promptDis);
                    com = Console.ReadLine();
                }
            }
        }
        static void grepDirectory(String gPattern,DirectoryInfo dir)
        {
            try
            {
                Regex rgx = new Regex(gPattern);
                DirectoryInfo di = new DirectoryInfo(dir.FullName);
                FileInfo[] fi = di.GetFiles("*.txt");
                for (int i = 0; i < fi.Length; i++)
                {
                    FileStream fs = new FileStream(fi[i].FullName, FileMode.Open);
                    StreamReader sr = new StreamReader(fs);
                    string line = "";
                    int lineNum = 1;
                    line = sr.ReadLine();
                    int j=0;
                    bool flag=false;
                    while (line != null)
                    {
                        String[] splt = rgx.Split(line);
                        if (splt.Length > 1)
                        {
                        	flag=true;
                        	if(j==0)
                        		Console.WriteLine("          -{0} Start-          ", fi[i].Name);
                            Console.Write("Line "+lineNum + ": ");
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
                        }
                        line = sr.ReadLine();
                        ++lineNum;
                        if(line == null && flag)
                        {
                        	Console.WriteLine("          -{0} End-          ", fi[i].Name);
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
        }
    }
}