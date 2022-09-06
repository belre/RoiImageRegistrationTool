using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ClipMeasure.Wrapper;
using ClipMeasure.Wrapper.Managed;

namespace ClipXmlReader.Model.DataSet.RecipeHandler.ClipMeasure
{
    /// <summary>
    /// 自動座標演算のためのシングルトンクラスを表します。
    /// </summary>
    public sealed class AutoCoordinateCalculator : IDisposable
    {
        /***
         * [設計上の補足]
         * * Singletonパターンを使用している。
         * 　グローバル変数によるメモリリーク回避のため。
         * * ClipMeasure名前空間に含まれているクラスは、Managed以外はすべてIDisposableを継承して、Disposeメソッドでデータを破棄する
         * * 状態を表すフィールド・プロパティをなるべく持たせないように配慮すること.
         * */

        /// <summary>
        /// 自動座標演算のモードを表します。
        /// </summary>
        public enum EModeCoordinateCalculator
        {
            NONE,
            COORD,
            COORD3,
            COORD4,
            COORDGc
        }

        /// <summary>
        /// 自動座標演算をサポートするCLIのラッパーオブジェクトを表します。
        /// </summary>
        private WpCoordConversion _coord_conversion_object;               
        
        /// <summary>
        /// シングルトンクラスを取得します。
        /// </summary>
        public static AutoCoordinateCalculator GetInstance()
        {
            return _singletonInstance;
        }
        private static AutoCoordinateCalculator _singletonInstance = new AutoCoordinateCalculator();
        

        private AutoCoordinateCalculator()
        {
            _coord_conversion_object = new WpCoordConversion();
        }

        /// <summary>
        /// 現在登録されている座標系を初期状態に戻します。
        /// </summary>
        public void ClearCoordinate()
        {
            _coord_conversion_object.ClearCoordinate();
        }

        /// <summary>
        /// 登録済みのmachineidを使用して、機種情報を設定します。
        /// </summary>
        /// <param name="machineid"></param>
        /// <param name="relations_object"></param>
        public void SetMachineInfo(int machineid, Relations.XmlRootSerialization relations_object)
        {
            //var wrapper = relations_object.Machines.MeasureDict[machineid].ConvertWrapper();
            //_coord_conversion_object.SetMachineInfo(wrapper);
        }

        public void SetPrintingOffset(float offsetx, float offsety)
        {
            WpMachineInfo info = new WpMachineInfo();
            info.CapturedImageOffset = new System.Drawing.PointF(offsetx, offsety);

            _coord_conversion_object.SetMachineInfo(info);
        }

        /// <summary>
        /// 座標系を設定します。
        /// </summary>
        /// <param name="ItemType">Measure ID</param>
        /// <param name="relations_object">レシピテンプレートオブジェクト</param>
        /// <param name="regions">領域</param>
        /// <param name="measureparams">Measure Params</param>
        /// <param name="CoordId_Ref">参照座標ID</param>
        /// <returns>成功時はtrue</returns>
        public bool SetCoordinate(int ItemType, int seqindex, Relations.XmlRootSerialization relations_object, List<WpAutoCoordinateRegion> regions, List<WpParameter> measureparams, uint CoordId_Ref )
        {
            WpMeasureParams measureparams_obj = new WpMeasureParams() { Parameter = measureparams };

            var mode = relations_object.Measures.MeasureDict[ItemType].AutoCoordCalculationMode;

            if (mode != EModeCoordinateCalculator.NONE)
            {
                return _coord_conversion_object.SetCoordinateAsCoord(ItemType, seqindex, regions, CoordId_Ref, measureparams_obj);
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 設定した座標系に基づいて、既存の絶対座標を相対座標に変換します。
        /// </summary>
        /// <param name="absolute">絶対座標</param>
        /// <param name="coordid_ref">参照座標ID</param>
        /// <returns>相対座標</returns>
        public WpAutoCoordinateRegion GetRelativeCoordinate(WpAutoCoordinateRegion absolute, uint coordid_ref)
        {
            if(coordid_ref < 0)
            {
                return null;
            }

            return _coord_conversion_object.CalculateAbsoluteToRelative(absolute);
        }

        /// <summary>
        /// 設定した座標系に基づいて、既存の相対座標を絶対座標に変換します。
        /// </summary>
        /// <param name="relative">相対座標</param>
        /// <param name="coordid_ref">参照座標ID</param>
        /// <returns>絶対座標</returns>
        public WpAutoCoordinateRegion GetAbsoluteCoordinate(WpAutoCoordinateRegion relative, uint coordid_ref, Coordinate.TransformStatus coordstatus)
        {
            if (coordid_ref < 0)
            {
                return null;
            }

            if (relative == null)
            {
                return null;
            }

            return _coord_conversion_object.CalculateRelativeToAbsolute(relative, 0, 0);
        }

        public WpCoordinateInfo GetCoordinateInfo(uint seqindex, uint coordid_ref)
        {
            if( coordid_ref < 0 )
            {
                return null;
            }

            return _coord_conversion_object.GetCoordinateInfo(seqindex, coordid_ref);
        }



        #region [IDisposable]

        /**
         * https://clickan.click/idisposable/
         * */


        //private IntPtr unmanagedResource;
        //private StreamWriter managedResource;
 
        private bool disposed = false;

        ~AutoCoordinateCalculator()
        {
            this.Dispose(false);
        }
 
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }
 
        private void Dispose(bool isDisposing)
        {
            if (!this.disposed)
            {
                //this.Free(this.unmanagedResource);
                _coord_conversion_object.Dispose();


                if (isDisposing)
                {
                    //if (this.managedResource != null)
                    {
                        //this.managedResource.Dispose();
                    }
                }
 
                this.disposed = true;
            }
        }

        #endregion

    }
}
