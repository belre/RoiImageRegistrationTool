using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Xml;

namespace ClipXmlReader.Model.DataSet.RecipeHandler.Base
{
    public class BaseXmlNode
    {
        /// <summary>
        /// 内部で保持されているオブジェクトの一覧を表します。
        /// このオブジェクトはnewされない限り、永続的に保持されます。
        /// オーバーライドがない限り、ここに登録されている値をXMLとして出力します。
        /// 処理が必要な場合は、オーバーライドしてください。
        /// </summary>
        protected Dictionary<object, DataType.GenericDataType> _params = new Dictionary<object, DataType.GenericDataType>();




        /// <summary>
        /// 要素名を表します。
        /// 例：Coord, Measure-params, ...
        /// </summary>
        public virtual string ElementName
        {
            get
            {
                return _element_name;
            }
        }
        protected string _element_name;

        /// <summary>
        /// trueの場合、XMLではElementNameを使用したデータの読み取りを行います。
        /// 継承によって切り替えます。
        /// </summary>
        protected virtual bool IsBoundElementName
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// 型を指定して、すでに登録済みのデータを取得します。
        /// </summary>
        /// <typeparam name="T">保持されているオブジェクトの型</typeparam>
        /// <param name="key">取得しようとしているオブジェクトのキー</param>
        /// <returns></returns>
        public T GetParameter<T>(object key)
        {
            return (T)_params[key].Contents;
        }

        /// <summary>
        /// パラメータが含まれているかどうかを表します。
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool IsIncludeParameter(object key)
        {
            return _params.ContainsKey(key);
        }

        /// <summary>
        /// 型を指定して、データを登録します。
        /// </summary>
        /// <typeparam name="T">保持しようとしているオブジェクトの型</typeparam>
        /// <param name="key">指定された型</param>
        /// <param name="value">値</param>
        public void SetParameter<T>(object key, T value)
        {
            _params[key] = new DataType.GenericDataType(value, value.GetType());
        }

        /// <summary>
        /// 列挙型を明示的に登録します。
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="enumtype"></param>
        public void SetParameterEnum(object key, object value, Type enumtype)
        {
            if (enumtype.IsEnum)
            {
                _params[key] = new DataType.GenericDataType(Enum.ToObject(enumtype, value), enumtype);
            }
        }

        /// <summary>
        /// サブクラスでオリジナルに指定された型表現により、値を取得します。
        /// </summary>
        /// <param name="key">キー</param>
        /// <returns></returns>
        public DataType.GenericDataType GetOriginalParameter(object key)
        {
            return _params[key];
        }

        /// <summary>
        /// 登録されている値を取得します。
        /// </summary>
        /// <param name="key">キー</param>
        /// <returns></returns>
        public object GetParameter(object key)
        {
            return _params[key].Contents;
        }

        /// <summary>
        /// サブクラスでオリジナルに指定された型表現で、値を登録します。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="valueobj"></param>
        public void SetOriginalParameter(object key, DataType.GenericDataType valueobj)
        {
            _params[key] = valueobj;
        }

        /// <summary>
        /// 型を指定して、データを登録します。
        /// このとき、値に指定される型は一般の文字列表現を適用します。
        /// なお、取得可能な型はenum(intで制約), int, double, decimal, stringです。
        /// </summary>
        /// <param name="key">キー</param>
        /// <param name="valtext">値(文字列表現)</param>
        /// <param name="valuetype">値の型</param>
        public void SetParameterDynamically(object key, string valtext, Type valuetype)
        {
            {
                if (valuetype.IsEnum)
                {
                    int value = 0;
                    if (int.TryParse(valtext, out value))
                    {
                        SetParameterEnum(key, value, valuetype);
                    }
                }
                else if (valuetype == typeof(int))
                {
                    int value = 0;
                    if (int.TryParse(valtext, out value))
                    {
                        SetParameter<int>(key, value);
                    }

                }
                else if (valuetype == typeof(double))
                {
                    double value = 0;
                    if (double.TryParse(valtext, out value))
                    {
                        SetParameter<double>(key, value);
                    }
                }
                else if (valuetype == typeof(decimal))
                {
                    decimal value = 0;
                    if (decimal.TryParse(valtext, out value))
                    {
                        SetParameter<decimal>(key, value);
                    }
                }
                else if (valuetype == typeof(string))
                {
                    SetParameter<string>(key, valtext);
                }
                else
                {
                    throw new InvalidOperationException("Failed : SetParameterDynamically");
                }
            }
        }

        /// <summary>
        /// XMLのうち、属性として定義されるキーを返します。
        /// XMLでの読み取りを実装する場合、オーバーライドします。
        /// </summary>
        public virtual DataType.GenericDataType[] AttributeName
        {
            get
            {
                return new DataType.GenericDataType[] { };
            }
        }

        /// <summary>
        /// XMLのうち、要素として定義されるキーを返します。
        /// XMLでの読み取りを実装する場合、オーバーライドします。
        /// </summary>
        protected virtual List<DataType.GenericDataType> ownedElement
        {
            get
            {
                return new List<DataType.GenericDataType> { };
            }
        }

        /// <summary>
        /// Dictionaryで指定された文字列表現をデータとして入力します。
        /// 注意として、すでに初期化でキーが登録されている必要があります。
        /// </summary>
        /// <param name="attr_list"></param>
        public virtual void SetAttribute(Dictionary<object, string> attr_list)
        {
            foreach (var attr in attr_list)
            {
                foreach (var source in AttributeName)
                {
                    if (attr.Key == source)
                    {
                        SetParameterDynamically(attr.Key, attr.Value.ToString(), source.ValueType);
                    }
                }
            }
        }

        /// <summary>
        /// XMLレシピからデータを読み取ります。
        /// </summary>
        /// <param name="reader">ストリーム</param>
        /// <param name="hierarchical">入れ子構造として現在登録されている項目</param>
        public virtual void ParseXmlRecipe(XmlReader reader, Stack<string> hierarchical)
        {

            /***
             * 処理のイメージ 入れ子のトップがAAAの場合
             * 
             * <AAA>                // ここはすでに読み込まれている。
             *  <BBB>           // オープンタグ検出
             *   3              // テキスト検出
             *  </BBB>          // クローズタグ検出
             * </AAA>           // クローズタグ検出, 入れ子のトップと一致しているので、処理を終了
             * 
             * */

            // 入れ子のトップの名称を取得

            var first_element = hierarchical.Pop();
            hierarchical.Push(first_element);
            InitParseProcedures(reader);

            // ElementNameと一致しない場合は、終了する。
            // (IsBoundElementName=trueに相当するクラス、オブジェクトのみ)
            if (IsBoundElementName && !first_element.Equals(ElementName))
            {
                return;
            }

            {
                bool IsParsing = true;          // データ取得完了?
                
                // データ取得が完了するか、readerがEOFになるまで実行する                
                while (IsParsing && reader.Read())
                {
                    // オープンタグ
                    if (reader.NodeType == XmlNodeType.Element)
                    {
                        // 入れ子構造に追加した後、クラス固有の処理を実行
                        hierarchical.Push(reader.Name);
                        ParseBeginElement(reader, hierarchical);
                    }
                    // クローズタグ
                    else if (reader.NodeType == XmlNodeType.EndElement)
                    {
                        // 入れ子構造で名称が一致している場合は、
                        // 処理を終了する
                        if (reader.Name.Equals(first_element))
                        {
                            IsParsing = false;
                        }

                        // クラス固有の処理を実行した後、入れ子構造の先頭を削除
                        ParseEndElement(reader, hierarchical);
                        hierarchical.Pop();
                    }
                    // テキスト
                    else if (reader.NodeType == XmlNodeType.Text)
                    {
                        // 現在の先頭の要素名を取得
                        var use_element = hierarchical.Pop();
                        hierarchical.Push(use_element);

                        // ownedElementが登録されている場合、
                        // use_elementで定義されている値を取得して読み取る
                        // 同名要素が連続する場合は上書き
                        foreach (var keyobj in ownedElement)
                        {
                            if (use_element.Equals(keyobj.Contents.ToString()))
                            {
                                SetParameterDynamically(keyobj, reader.Value, keyobj.ValueType);
                            }
                        }

                        // クラス固有の処理を実行
                        ParseText(reader, use_element);
                    }
                }
            }
        }


        /// <summary>
        /// XMLパース処理前に行われる処理を表します。
        /// </summary>
        /// <param name="reader"></param>
        protected virtual void InitParseProcedures(XmlReader reader)
        {

        }

        /// <summary>
        /// XMLレシピで、オープンタグを検出したときに実行する処理を表します。
        /// 何か処理をする場合は、オーバーライドします。
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="hierarchical"></param>
        protected virtual void ParseBeginElement(XmlReader reader, Stack<string> hierarchical)
        {

        }

        /// <summary>
        /// XMLレシピで、クローズタグを検出したときに実行する処理を表します。
        /// 何か処理をする場合は、オーバーライドします。
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="hierarchical"></param>
        protected virtual void ParseEndElement(XmlReader reader, Stack<string> hierarchical)
        {

        }

        /// <summary>
        /// XMLレシピで、テキストを検出したときに実行する処理を表します。
        /// 何か処理をする場合は、オーバーライドします。
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="hierarchical"></param>
        protected virtual void ParseText(XmlReader reader, string elementname)
        {

        }

        /// <summary>
        /// 出力するときに必要なXmlDocumentを出力します。
        /// </summary>
        /// <param name="document">出力されるdocument</param>
        /// <param name="parent">一つ上位の要素</param>
        public virtual void MakeXmlNode(XmlDocument document, XmlElement parent)
        {
        }

        /// <summary>
        /// 要素名の属性出力を終えた後に、行う処理を表します。
        /// 自分自身に対する子オブジェクトに対してに使用します。
        /// 使用する場合はオーバーライドします。
        /// </summary>
        /// <param name="document">出力されるdocument</param>
        /// <param name="current">現在の要素</param>
        protected virtual void DelegateGroup(XmlDocument document, XmlElement current)
        {

        }

        /// <summary>
        /// パラメータの要素の登録を終えた後に、行う処理を表します。
        /// 自分自身に対する子オブジェクトに対してに使用します。
        /// 使用する場合はオーバーライドします。
        /// </summary>
        /// <param name="document">出力されるdocument</param>
        /// <param name="current">現在の要素</param>
        protected virtual void DelegateSubGroup(XmlDocument document, XmlElement current)
        {

        }



    }
}
