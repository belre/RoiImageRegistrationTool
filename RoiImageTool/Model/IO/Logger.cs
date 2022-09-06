using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClipXmlReader.Model.IO
{
    public class Logger
    {
        public bool SaveFile(string path)
        {
            string[] textline = new string[]{ "Hello", "World"};

            try
            {
                System.IO.File.WriteAllLines(path, textline);

                return true;
            }
            catch (System.IO.IOException)
            {
                return false;
            }
        }



    }
}
