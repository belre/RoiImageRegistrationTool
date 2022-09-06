using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Xml;

namespace ClipXmlReader.Model.DataSet.RecipeHandler.Group
{
    /// <summary>
    /// 計測項目を表すクラスです。
    /// </summary>
    public class MeasuresGroup : Base.BaseTreeGroup
    {
        /// <summary>
        /// 現在管理されている計測項目の一覧を表します。
        /// </summary>
        public List<MeasureGroup> RecipeItemGroup
        {
            get;
            set;
        }

        /// <summary>
        /// 現在設定されている座標系が絶対座標系かどうかを表します。
        /// </summary>
        public bool IsAbsoluteCoordinate
        {
            get;
            set;
        }

        /// <summary>
        /// 実体化された親クラスを表します。
        /// </summary>
        public virtual RecipeEntityGroup SpecifiedParent
        {
            get;
            protected set;
        }

        /// <summary>
        /// 汎用的な親クラスを表します。
        /// </summary>
        public override Base.BaseTreeGroup Parent
        {
            get
            {
                return this.SpecifiedParent;
            }
        }

        public double AllOffsetX
        {
            get;
            set;
        }

        public double AllOffsetY
        {
            get;
            set;
        }

        public Dictionary<int, Coordinate.TransformStatus> UserTransformParameter
        {
            get;
            set;
        }


        public MeasuresGroup( RecipeEntityGroup parent)
            : base()
        {
            _element_name = "Measures";
            SpecifiedParent = parent;

            IsAbsoluteCoordinate = false;

            RecipeItemGroup = new List<MeasureGroup>();
            UserTransformParameter = new Dictionary<int, Coordinate.TransformStatus>();
        }

        public void GetOffsetFromObject(MeasuresGroup obj)
        {
            this.AllOffsetX = obj.AllOffsetX;
            this.AllOffsetY = obj.AllOffsetY;

            this.UserTransformParameter = new Dictionary<int,Coordinate.TransformStatus>(obj.UserTransformParameter);
        }


        /// <summary>
        /// オープンタグを読み取った後に、処理を実行します。
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="hierarchical"></param>
        protected override void ParseBeginElement(XmlReader reader, Stack<string> hierarchical)
        {
            var item = new Group.MeasureGroup(this);           

            // Measureタグを検出
            if (reader.Name.Equals(item.ElementName))
            {

                item.InitAttributes(reader);

                // MeasureGroupに処理を委譲する
                item.ParseXmlRecipe(reader, hierarchical);

                // MeasureGroupを追加
                RecipeItemGroup.Add(item);
            }
        }

        /// <summary>
        /// 現在の階層のXMLの値を集計します。
        /// </summary>
        /// <param name="document"></param>
        /// <param name="current"></param>
        protected override void DelegateSubGroup(XmlDocument document, XmlElement current)
        {
            foreach (var itemgroup in RecipeItemGroup)
            {
                // MeasureObjectに移譲
                itemgroup.MakeXmlNode(document, current);
            }
        }

        /// <summary>
        /// 計測項目が追加された時の処理を表します。
        /// </summary>
        /// <param name="targetgen">このオブジェクトの前後に新規追加する</param>
        /// <param name="isappendnext"></param>
        public override void InsertGroup(Base.BaseTreeGroup targetgen, bool isappendnext = false)
        {
            Group.MeasureGroup target = (Group.MeasureGroup)targetgen;

            // 初期化
            MeasureGroup newitem = new MeasureGroup(this);
            newitem.SetParameter<string>(newitem.Key_Name, target.GetParameter<string>(target.Key_Name));          
            newitem.ItemType = target.GetParameter<int>(target.Key_ItemType);


            // 指定されたオブジェクトの前後のindexを取得
            int index = RecipeItemGroup.IndexOf(target);
            if (index == -1 )
            {
                return;
            }

            if( isappendnext )
            {
                index++;
            }

            // 追加
            if( index == RecipeItemGroup.Count)
            {
                RecipeItemGroup.Add(newitem);
            }
            else
            {
                RecipeItemGroup.Insert(index, newitem);
            }
        }

        /// <summary>
        /// 指定されたオブジェクトを削除します。
        /// </summary>
        /// <param name="targetgen">削除するオブジェクト</param>
        public override void DeleteGroup(Base.BaseTreeGroup targetgen)
        {
            Group.MeasureGroup target = (Group.MeasureGroup)targetgen;
                      
            RecipeItemGroup.Remove(target);
        }

        /// <summary>
        /// 自動座標変換をすべての計測項目で実行します。
        /// </summary>
        public void RunCoordinateCalculator()
        {
            // シングルトンオブジェクトを取得
            var singletonobj = ClipMeasure.AutoCoordinateCalculator.GetInstance();

            // 座標系のクリア
            singletonobj.ClearCoordinate();

            // 機種情報の登録
            singletonobj.SetMachineInfo( int.Parse(Properties.Resources.MachineID), RelationsObject);


            singletonobj.SetPrintingOffset((float)AllOffsetX, (float)AllOffsetY);

            // 反復操作
            foreach( var itemgroup in RecipeItemGroup)
            {

#if false
                // 絶対座標系取得
                var regionlist = itemgroup.RegionsGroupObject.RelativeCoordinateRegionList;

                if( itemgroup.CoordinateGroupObject.CoordId == -1)
                {
                    continue;
                }


                // 座標変換実行
                var errorstatus = singletonobj.SetCoordinate(itemgroup.ItemType, RelationsObject, regionlist, itemgroup.MeasureParamsGroupObject.MeasureParameterArray, (uint)itemgroup.CoordinateGroupObject.CoordId;
#endif

                itemgroup.SetCoordinate();
            
            }
        }

        public void SaveTransformParameter(string filepath)
        {

            StringBuilder builder = new StringBuilder();

            builder.AppendFormat("CoordID,{0},{1}\n", AllOffsetX, AllOffsetY);
            foreach ( var item in UserTransformParameter)
            {
                builder.AppendFormat( "{0},{1},{2}\n", item.Key, item.Value.OffsetX, item.Value.OffsetY);
            }

            System.IO.File.WriteAllText(filepath, builder.ToString());
        }

        public void LoadTransformParameter(string filepath)
        {         
            string[] alltexts = System.IO.File.ReadAllLines(filepath, Encoding.UTF8);

            if (alltexts.Length == 0)
            {
                return;
            }

            int line = 0;
            Func<string, string[]> splitconv = (str) =>
            {
                if(str == null) {
                    return null;
                }

                string[] splitstr = str.Split(',');
                if(splitstr.Length < 3) {
                    return null;
                }

                return splitstr;
            };

            var texts = splitconv(alltexts[line++]);
            AllOffsetX = Double.Parse(texts[1]);
            AllOffsetY = Double.Parse(texts[2]);

            while (line < alltexts.Length)
            {
                var splitstr = splitconv(alltexts[line++]);
                if( splitstr != null)
                {
                    int id = int.Parse(splitstr[0]);
                    UserTransformParameter[id] = new Coordinate.TransformStatus()
                    {
                        OffsetX = Double.Parse(splitstr[1]),
                        OffsetY = Double.Parse(splitstr[2])
                    };
                }
            }

        }



    }
}
