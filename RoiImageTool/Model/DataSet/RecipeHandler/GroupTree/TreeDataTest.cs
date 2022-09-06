using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;

namespace ClipXmlReader.Model.DataSet.RecipeHandler.GroupTree
{

    public class CoordPairItem
    {
        public int Index;
        public int CoordIdRef;
        public int CoordIdOut;
        public int CoordIdOutReplacement;

        public bool IsReplacement;
        public bool IsBranch;
        public Group.MeasureGroup Context;
    }

    public class TreeFormTest
    {
        public TreeFormTest()
        {

        }

        public void MakeTree(Group.MeasuresGroup group)
        {
            List<CoordPairItem> list = new List<CoordPairItem>();
            //Dictionary<int, int> coordidout_list = new Dictionary<int, int>();
            List<int> coordidout_list = new List<int>();

            int index = 0;
            int replaceindex = 0;
            foreach (var obj in group.RecipeItemGroup)
            {
                int coordidout = -1;
                int coordidorgout = -1;
                int coordidref = -1;

                var measureparamlist = obj.MeasureParamsGroupObject.MeasureParameterArray;
                foreach (var mparam in measureparamlist)
                {
                    if (mparam.Name == "CoordId")
                    {
                        coordidout = coordidorgout = int.Parse(mparam.Value.ToString());
                    }
                }

                coordidref = obj.CoordinateGroupObject.CoordId;

                var item = new CoordPairItem()
                {
                    CoordIdRef = coordidref,
                    CoordIdOut = coordidout,
                    CoordIdOutReplacement = coordidout_list.Contains(coordidout) ? replaceindex++ : -1,
                    IsReplacement = coordidout_list.Contains(coordidout),
                    IsBranch = coordidout != -1,
                    Index = index++,
                    Context = obj
                };
                list.Add(item);

                if (coordidout != -1)
                {
                    coordidout_list.Add(coordidout);
                }
            }
            

            // 一旦先頭は必ずCoordId=0と仮定して、再帰する
            TreeDataTest root = new TreeDataTest() { _coorditem = new CoordPairItem() { Index = -1, IsBranch = true, CoordIdRef = -1, CoordIdOut = 0, IsReplacement = false, Context = null } };
            foreach (var item in list)
            {
                root.Retrieve(item);
            }


            // 木構造を表示する
            StringBuilder builder = new StringBuilder();
            root.Print(builder, 0);

            File.WriteAllText("log.txt", builder.ToString());
        }


    }


    public class TreeDataTest
    {
        public List<TreeDataTest> _children;
        public CoordPairItem _coorditem;

        public TreeDataTest()
        {
            _children = new List<TreeDataTest>();
        }


        public bool Retrieve(CoordPairItem next)
        {
            if (_coorditem.CoordIdOut == next.CoordIdRef)
            {
                _children.Add(new TreeDataTest() { _coorditem = next });
                return true;
            }
            else
            {
                foreach (var child in _children)
                {
                    if( child.Retrieve(next) )
                    {
                        return true;
                    }
                }

                return false;                
            }
        }

        public void Print(StringBuilder builder, int depth)
        {
            for (int i = 0; i < depth; i++)
            {
                builder.Append("\t");
            }
            builder.AppendFormat("{0}Index:{1},Ref:{2},{3}{4},{5}\n", 
                _coorditem.IsReplacement ? "* " : "",
                _coorditem.Index, 
                _coorditem.CoordIdRef, 
                _coorditem.IsBranch ? "Out:" + _coorditem.CoordIdOut : "",
                _coorditem.IsReplacement ? "->" + _coorditem.CoordIdOutReplacement + "'" : "",
                _coorditem.Context != null ? _coorditem.Context.GetParameter(_coorditem.Context.Key_Name) : "null"
                );
            foreach (var child in _children)
            {
                child.Print(builder, depth + 1);
            }

        }       
    }







}
