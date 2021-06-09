using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class FileReader
{
    
    public static List<string> Readfile(string filepath)
    {
        List<string> ret = new List<string>();
        using (StreamReader sr = new StreamReader(filepath))
        {
            string line;

            // 从文件读取并显示行，直到文件的末尾 
            while ((line = sr.ReadLine()) != null)
            {
                ret.Add(line);
                //Console.WriteLine(line);
            }
        }
        return ret;
    }

}

