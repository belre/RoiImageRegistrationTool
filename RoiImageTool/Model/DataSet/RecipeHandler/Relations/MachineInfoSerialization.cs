using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Xml.Serialization;

using ClipMeasure.Wrapper.Managed;

namespace ClipXmlReader.Model.DataSet.RecipeHandler.Relations
{
    /// <summary>
    /// Machine情報を表すシリアライズされたテンプレートを表します。
    /// </summary>
    public class MachineInfoSerialization : RelationSerialization
    {
        /// <summary>
        /// MachineのIDを表します。
        /// </summary>
        [XmlAttribute]
        public int ID
        {
            get;
            set;
        }

        /// <summary>
        /// Machine名(機種名)を表します。
        /// </summary>
        [XmlElement]
        public string Name
        {
            get;
            set;
        }



        /// <summary>
        /// キャプチャした画像のオフセット座標
        /// </summary>
        public System.Drawing.PointF CapturedImageOffset
        {
            get;
            set;
        }

        /// <summary>
        /// 分解能
        /// </summary>
        public System.Drawing.PointF Resolution
        {
            get;
            set;
        }


        public WpMachineInfo ConvertWrapper()
        {
            WpMachineInfo machineinfo = new WpMachineInfo();
            machineinfo.CapturedImageOffset = CapturedImageOffset;

            return machineinfo;
        }

    }
}
