using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClipXmlReader.Model.IO.Recipes
{
    /// <summary>
    /// テンプレートの管理を行うクラスです。
    /// </summary>
    public class RecipeTemplateManager
    {
        /// <summary>
        /// 相対パスを表します。
        /// </summary>
        public string RelativeDirectory
        {
            get;
            set;
        }

        /// <summary>
        /// シリアライズ化されたテンプレートファイルを表します。
        /// </summary>
        public DataSet.RecipeHandler.Relations.XmlRootSerialization XmlTemplate
        {
            get;
            protected set;
        }

        /// <summary>
        /// Measureのテンプレートを表します。
        /// </summary>
        public string MeasureTemplatePath
        {
            get
            {
                return RelativeDirectory + @"\" + "_meas_template.xml";
            }
        }

        /// <summary>
        /// Regionのテンプレートを表します。
        /// </summary>
        public string RegionTemplatePath
        {
            get
            {
                return RelativeDirectory + @"\" + "_region_template.xml";
            }
        }

        /// <summary>
        /// Filterのテンプレートを表します。
        /// </summary>
        public string FilterTemplatePath
        {
            get
            {
                return RelativeDirectory + @"\" + "_filter_template.xml";
            }
        }

        /// <summary>
        /// Machineのテンプレートを表します。
        /// </summary>
        public string MachineTemplatePath
        {
            get
            {
                return RelativeDirectory + @"\" + "_machine_template.xml";
            }
        }




        public RecipeTemplateManager()
        {
            RelativeDirectory = ".";
            XmlTemplate = new DataSet.RecipeHandler.Relations.XmlRootSerialization();
        }

        /// <summary>
        /// デフォルト値を使用したテンプレートロード用シリアライズ関数を実行します。
        /// </summary>
        public void LoadByInternalScript()
        {
            XmlTemplate.Measures = (new Model.DataSet.RecipeHandler.Default.MeasureIDSerializationMaker()).MakeSerialization();
            XmlTemplate.Regions = (new Model.DataSet.RecipeHandler.Default.RegionIDSerializationMaker()).MakeSerialization();
            XmlTemplate.Filters = (new Model.DataSet.RecipeHandler.Default.FilterIDSerializationMaker()).MakeSerialization();
        }

        /// <summary>
        /// テンプレートを読み込みます。
        /// </summary>
        public void LoadTemplate()
        {
            if (!System.IO.File.Exists(MeasureTemplatePath)) return;
            if (!System.IO.File.Exists(RegionTemplatePath)) return;
            if (!System.IO.File.Exists(FilterTemplatePath)) return;
            if (!System.IO.File.Exists(MachineTemplatePath)) return;


            using (System.IO.StreamReader reader = new System.IO.StreamReader(MeasureTemplatePath, System.Text.Encoding.GetEncoding("Shift_jis")))
            {
                System.Xml.Serialization.XmlSerializer xmls = new System.Xml.Serialization.XmlSerializer(XmlTemplate.Measures.GetType());
                XmlTemplate.Measures = (DataSet.RecipeHandler.Relations.MeasureRootSerialization)xmls.Deserialize(reader);
            }
            using (System.IO.StreamReader reader = new System.IO.StreamReader(RegionTemplatePath, System.Text.Encoding.GetEncoding("Shift_jis")))
            {
                System.Xml.Serialization.XmlSerializer xmls = new System.Xml.Serialization.XmlSerializer(XmlTemplate.Regions.GetType());
                XmlTemplate.Regions = (DataSet.RecipeHandler.Relations.RegionRootSerialization)xmls.Deserialize(reader);
            }

            using (System.IO.StreamReader reader = new System.IO.StreamReader(FilterTemplatePath, System.Text.Encoding.GetEncoding("Shift_jis")))
            {
                System.Xml.Serialization.XmlSerializer xmls = new System.Xml.Serialization.XmlSerializer(XmlTemplate.Filters.GetType());
                XmlTemplate.Filters = (DataSet.RecipeHandler.Relations.FilterRootSerialization)xmls.Deserialize(reader);
            }

            using (System.IO.StreamReader reader = new System.IO.StreamReader(MachineTemplatePath, System.Text.Encoding.GetEncoding("Shift_jis")))
            {
                System.Xml.Serialization.XmlSerializer xmls = new System.Xml.Serialization.XmlSerializer(XmlTemplate.Machines.GetType());
                XmlTemplate.Machines = (DataSet.RecipeHandler.Relations.MachineSettingSerialization)xmls.Deserialize(reader);
            }


            Environments.OperationStatus.IsDevelopmentMode = XmlTemplate.Machines.DevelopmentMode;
        }

        /// <summary>
        /// テンプレートを保存します。
        /// </summary>
        public void SaveTemplate()
        {
            string temporary_relative = RelativeDirectory + @"\tmp\";

            if (!System.IO.Directory.Exists(temporary_relative))
            {
                System.IO.Directory.CreateDirectory(temporary_relative);
            }

            using (System.IO.StreamWriter writer = new System.IO.StreamWriter(temporary_relative + MeasureTemplatePath, false, System.Text.Encoding.GetEncoding("Shift_jis")))
            {
                System.Xml.Serialization.XmlSerializer xmls = new System.Xml.Serialization.XmlSerializer(XmlTemplate.Measures.GetType());
                xmls.Serialize(writer, XmlTemplate.Measures);
            }

            using (System.IO.StreamWriter writer = new System.IO.StreamWriter(temporary_relative + RegionTemplatePath, false, System.Text.Encoding.GetEncoding("Shift_jis")))           
            {
                System.Xml.Serialization.XmlSerializer xmls = new System.Xml.Serialization.XmlSerializer(XmlTemplate.Regions.GetType());
                xmls.Serialize(writer, XmlTemplate.Regions);
            }

            using (System.IO.StreamWriter writer = new System.IO.StreamWriter(temporary_relative + FilterTemplatePath, false, System.Text.Encoding.GetEncoding("Shift_jis")))
            {
                System.Xml.Serialization.XmlSerializer xmls = new System.Xml.Serialization.XmlSerializer(XmlTemplate.Filters.GetType());
                xmls.Serialize(writer, XmlTemplate.Filters);
            }

            if( XmlTemplate.Machines == null )
            {
                XmlTemplate.Machines = new DataSet.RecipeHandler.Relations.MachineSettingSerialization();
                XmlTemplate.Machines.Machine.Add(new DataSet.RecipeHandler.Relations.MachineInfoSerialization() { ID = 1, CapturedImageOffset = new System.Drawing.Point(10, 10) });
            }

            using (System.IO.StreamWriter writer = new System.IO.StreamWriter(temporary_relative + MachineTemplatePath, false, System.Text.Encoding.GetEncoding("Shift_jis")))
            {
                System.Xml.Serialization.XmlSerializer xmls = new System.Xml.Serialization.XmlSerializer(XmlTemplate.Machines.GetType());
                xmls.Serialize(writer, XmlTemplate.Machines);
            }
        }



    }
}
