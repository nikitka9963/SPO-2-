using System;
using System.IO;

namespace spo
{
	class Program
	{
		static void Main()
		{
		    System.IO.StreamReader file =
                new System.IO.StreamReader("input.txt");
            string line = "";
            string line1 = "";
		  
 
            while((line = file.ReadLine()) != null)  
            {  
                line1 += line;
            } 
            
			Parser parser = new Parser(line1);
			Console.WriteLine(parser.Parse());
		}
	}
}
