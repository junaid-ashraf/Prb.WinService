using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prb.DTO;
using Prb.DTO.Request;
using Prb.DTO.Response;
using System.Data.Common;
using System.Data;
using System.Xml.Serialization;
using System.IO;

namespace Prb.SqliteDAL
{
    public class Hw_MasterInfoDataAccess 
    {
        ProbeDBEntities _Db = DBHelper.Instance._Db;
        public Hw_MasterInfoResponse Hw_MasterInfo_InsertData(List<Hw_MasterInfoDTO> LstHw_MasterInfoDTO)
        {
            Hw_MasterInfoResponse response = new Hw_MasterInfoResponse();
            try
            {
                foreach (Hw_MasterInfoDTO datarow in LstHw_MasterInfoDTO)
                {
                    datarow.StatusId = 1;
                    Hw_MasterInfo _Prb_Schedule = new Hw_MasterInfo()
                    {
                        HwMasterInfoId = datarow.HwMasterInfoId,
                        SiteId = datarow.SiteId,
                        ScheduleId = datarow.ScheduleId,
                        HwTypeId = datarow.HwTypeId,
                        DevType = datarow.DevType,
                        DevCategory = datarow.DevCategory,
                        ComType = datarow.ComType,
                        HwName = datarow.HwName,
                        Description = datarow.Description,
                        IPAddress = datarow.IPAddress,
                        MacAddress = datarow.MacAddress,
                        StatusId = datarow.StatusId,
                        CreatedBy = 1,
                        CreatedDate = DateTime.Now,
                        ModifiedBy = 1,
                        ModifiedDate = DateTime.Now,
                        IsDeleted = false,
                    };
                    _Db.Hw_MasterInfo.Add(_Prb_Schedule);
                }

                _Db.SaveChanges();

                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public Hw_MasterInfoResponse GetHw_MasterInfoData(int SiteId)
        {
            Hw_MasterInfoResponse response = new Hw_MasterInfoResponse();
            try
            {

                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // By Waheed
        //public Hw_MasterInfoResponse Hw_MasterInfo_InsertData(List<Hw_MasterInfoDTO> LstHw_MasterInfoDTO)
        //{
        //    Hw_MasterInfoResponse response = new Hw_MasterInfoResponse();
        //    try
        //    {
        //        XmlSerializer serializer = new XmlSerializer(LstHw_MasterInfoDTO.GetType());
        //        MemoryStream memStream = new MemoryStream();
        //        string replace = "<?xml version=\"1.0\"?>";
        //        string xmlMasterInfo = string.Empty;

        //        serializer.Serialize(memStream, LstHw_MasterInfoDTO);

        //        memStream.Close();

        //        xmlMasterInfo = Encoding.UTF7.GetString(memStream.ToArray());
        //        xmlMasterInfo = xmlMasterInfo.Replace(replace, "");

        //        using (DbCommand dbCommand = PrbDataBase.GetStoredProcCommand(DBConstant.Hw_MasterInfo.SP_HW_MASTERINFO_INSERTDATA))
        //        {
        //            PrbDataBase.AddInParameter(dbCommand, "@MasterInfoXml", DbType.String, xmlMasterInfo);

        //            DataSet ds = PrbDataBase.ExecuteDataSet(dbCommand);
        //            if (ds.Tables != null && ds.Tables.Count > 0)
        //            {
        //                response.Hw_MasterInfoList = MapDataTableToList<Hw_MasterInfoDTO>(ds.Tables[0]).ToList();
        //            }
        //        }
        //        return response;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        //public Hw_MasterInfoResponse GetHw_MasterInfoData(int SiteId)
        //{
        //    Hw_MasterInfoResponse response = new Hw_MasterInfoResponse();
        //    try
        //    {
        //        using (DbCommand dbCommand = PrbDataBase.GetStoredProcCommand(DBConstant.Hw_MasterInfo.SP_HW_MASTERINFO))
        //        {
        //            PrbDataBase.AddInParameter(dbCommand, "@SiteId", DbType.Int32, SiteId);
        //            PrbDataBase.AddInParameter(dbCommand, "@OperationId", DbType.Int32, 1);

        //            DataSet ds = PrbDataBase.ExecuteDataSet(dbCommand);
        //            if (ds.Tables != null && ds.Tables.Count > 0)
        //            {
        //                response.Hw_MasterInfoList = MapDataTableToList<Hw_MasterInfoDTO>(ds.Tables[0]).ToList();
        //            }
        //        }
        //        return response;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
    }
}
