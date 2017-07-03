using System;
using System.IO;

namespace SimpleCrypter
{
    class Program
    {

        static void ShowUsage()
        {
            Console.WriteLine("Usage: ");
            Console.WriteLine("    SimpleCrypter <executable file> <key> <output file>\r\n");
        }

        static void Main(string[] args)
        {
            if(args.Length != 3)
            {
                ShowUsage();
                return;
            }

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

            FileStream stub = new FileStream("stub.bin", FileMode.Open);
            BinaryReader stubReader = new BinaryReader(stub);
            FileStream target = new FileStream(args[0], FileMode.Open);
            BinaryReader targetReader = new BinaryReader(target);
            FileStream output = new FileStream(args[2], FileMode.Create);
            BinaryWriter outputWriter = new BinaryWriter(output);

            /* Write Stub binary */
            outputWriter.Write(stubReader.ReadBytes((int)stub.Length));
            /* Write key */
            outputWriter.Write(args[1]);
            /* Write target length (int32) */
            outputWriter.Write((int)target.Length);
            /* Encrypt target binary */
            byte[] targetBinary = targetReader.ReadBytes((int)target.Length);
            byte[] key = System.Text.Encoding.ASCII.GetBytes(args[1]);
            byte[] cryptogram = RC4.Encrypt(key, targetBinary);
            /* Write the cryptogram */
            outputWriter.Write(cryptogram);
            outputWriter.Flush();
            outputWriter.Close();
            output.Close();
            target.Close();
            stub.Close();
        }
    }
}
