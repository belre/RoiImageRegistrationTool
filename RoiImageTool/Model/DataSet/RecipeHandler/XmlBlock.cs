using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClipXmlReader.Model.Control.MainWindow.RecipeHandler
{
    public class XmlBlock
    {
        public string Element
        {
            get;
            set;
        }

        List<string> Attributes
        {
            get;
            set;
        }

    }
}
