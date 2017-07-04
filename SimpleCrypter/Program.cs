using System;
using System.IO;

namespace SimpleCrypter
{
    class Program
    {
        /// <summary>
        /// It shows the usage of the program
        /// </summary>
        static void ShowUsage()
        {
            Console.WriteLine("Usage: ");
            Console.WriteLine("    SimpleCrypter <executable file> <key> <output file>\r\n");
        }

        /// <summary>
        /// Entry point of the program
        /// </summary>
        static void Main(string[] args)
        {
            // Checking arguments count
            if(args.Length != 3)
            {
                ShowUsage();
                return;
            }

            // Checking whether the required files exist
            if (!File.Exists(args[0]))
            {
                Console.WriteLine("No such file: " + args[1]);
                return;
            }
            if (!File.Exists("stub.bin"))
            {
                Console.WriteLine("No such file: stub.bin");
                return;
            }

            // Opening required files
            FileStream stub = new FileStream("stub.bin", FileMode.Open);
            BinaryReader stubReader = new BinaryReader(stub);
            FileStream target = new FileStream(args[0], FileMode.Open);
            BinaryReader targetReader = new BinaryReader(target);
            FileStream output = new FileStream(args[2], FileMode.Create);
            BinaryWriter outputWriter = new BinaryWriter(output);

            // Create a new file using the following format:
            // [ stub binary ]
            // [ encryption key]
            // [ length of the target binary ]
            // [ target binary (encrypted) ]
            // [ length of the stub (Last 4 bytes)]
            outputWriter.Write(stubReader.ReadBytes((int)stub.Length));
            outputWriter.Write(args[1]);
            outputWriter.Write((int)target.Length);
            byte[] targetBinary = targetReader.ReadBytes((int)target.Length);
            byte[] key = System.Text.Encoding.ASCII.GetBytes(args[1]);
            byte[] cryptogram = RC4.Encrypt(key, targetBinary);
            outputWriter.Write(cryptogram);
            outputWriter.Write((int)stub.Length);
            outputWriter.Flush();

            // clean up everything
            outputWriter.Close();
            output.Close();
            target.Close();
            stub.Close();
        }
    }
}
