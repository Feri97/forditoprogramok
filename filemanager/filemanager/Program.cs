using System;

namespace filemanager
{
    class Program
    {
        static void Main(string[] args)
        {
            sourceHandler s = new sourceHandler("C:/Users/Feri/file.txt", "C:/Users/Feri/file2.txt");
            s.openFileToRead();
            Console.WriteLine("text: " + s.content);
            s.replaceContent();
            s.replaceFirst();
            //s.replaceContent("... this is a text which we append the file with.");
            Console.WriteLine("new text: " + s.content);
            s.openFileToWrite();
            Console.WriteLine("done");
            Console.ReadLine();

        }
    }
}
