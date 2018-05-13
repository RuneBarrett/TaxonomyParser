using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ConsoleApp1
{
    class TaxonomyParser
    {
        static void Main(string[] args)
        {
            Writer(Reader());

            // Keep the console window open in debug mode.
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }

        private static List<String> Reader()
        {
            List<String> allLines = new List<String>();
            List<String> outPut = new List<String>();
            String[] parents = new String[] {"","","","","","","","","",""}; //soso quick fix

            try
            {   // Open the text file using a stream reader.
                using (StreamReader sr = new StreamReader("sup_kat.txt", Encoding.GetEncoding("iso-8859-1")))
                {
                    // Read the stream to a string, and write the string to the console.
                    while (!sr.EndOfStream)
                    {
                        String line = sr.ReadLine();
                        allLines.Add(line);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }

            //var currentParent = "";
            var parentIndex = 0;
            for (int i = 0; i < allLines.Count; i++) {
                var l = allLines[i];               
                Console.WriteLine(l + " T"+CountTabs(l));

                var par = CountTabs(l) == 0 ? "" : parents[parentIndex].Trim();
                Console.WriteLine("----------------------------------------------------------------------------------- tag(" + allLines[i].Trim() + ") par(" + par + ") pIndex: " + parentIndex);
                outPut.Add("INSERT INTO <table> values (\""+allLines[i].Trim()+"\", \""+par+"\")");
                if (allLines.Count - 1 != i)
                { //dont look forward on last line
                    var next = CountTabs(allLines[i + 1]);

                    if (next > CountTabs(l)) //check if next line is further indented
                    {
                        parentIndex = CountTabs(l);
                        parents[parentIndex] = l;    
                    }

                    if (next < CountTabs(l))
                        parentIndex = CountTabs(l)-2;

                    if (next == CountTabs(l))
                        parentIndex = CountTabs(l)-1;

                }
            }
            return outPut;      
        }

        private static void Writer(List<string> lines)
        {
            Console.WriteLine("Writing...");
            // Set a variable to the My Documents path.
            string mydocpath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            // Write the string array to a new file named "WriteLines.txt".
            using (StreamWriter outputFile = new StreamWriter("WriteLines.txt"))
            {
                var count = 0;
                foreach (string line in lines)
                {
                    outputFile.WriteLine(line);
                    count++;
                }
                Console.WriteLine("...Done. Lines written: " + count);
            }
        }

        //------------ Helpers
        private static int CountTabs(string line)
        {
            var tabCount = 0;
            foreach (var c in line)
            {
                if (c == '\t')
                    tabCount++;
                if (c != '\t') return tabCount;
            }
            return tabCount;
        }
    }
}
