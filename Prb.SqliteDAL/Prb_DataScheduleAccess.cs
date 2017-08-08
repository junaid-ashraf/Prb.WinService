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

namespace Prb.SqliteDAL
{
    public class Prb_ScheduleDataAccess 
    {
        public int ProcessScheduleDataAdd(Prb_ScheduleRequest request)
        {
            ProbeDBEntities _Db = DBHelper.Instance._Db;;
            try
            {
                Prb_Schedule _Prb_Schedule = new Prb_Schedule()
                {
                    SiteId = request.Prb_Schedule.SiteId,
                    //ScheduleId = request.Prb_Schedule.ScheduleId,
                    SettingId = request.Prb_Schedule.SettingId,
                    StartDateTime = request.Prb_Schedule.StartDateTime,
                    EndDateTime = request.Prb_Schedule.EndDateTime,
                    Description = request.Prb_Schedule.Description,
                    StatusId = request.Prb_Schedule.StatusId,
                    CreatedBy = 1,
                    CreatedDate = DateTime.Now,
                    ModifiedBy = 1,
                    ModifiedDate = DateTime.Now,
                    IsDeleted = false,
                };
                _Db.Prb_Schedule.Add(_Prb_Schedule);

                _Db.SaveChanges();
                return Convert.ToInt32(_Prb_Schedule.ScheduleId.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
                // return 0;
            }
        }
        public long ProcessScheduleDataGet(Prb_ScheduleRequest request)
        {
            ProbeDBEntities _Db = DBHelper.Instance._Db;;
            try
            {
                Prb_SiteResponse response = new Prb_SiteResponse();
                var _Prb_ScheduleTemp = (from s in _Db.Prb_Schedule
                                     where s.ScheduleId == request.Prb_Schedule.ScheduleId && s.IsDeleted == false
                                     select new
                                     {
                                         SiteId =s.SiteId,
                                         ScheduleId =s.ScheduleId,
                                         Description = s.Description,
                                         StartDateTime = s.StartDateTime,
                                         EndDateTime = s.EndDateTime,
                                         StatusId =s.StatusId,
                                     }
                );
                var _Prb_Schedule = _Prb_ScheduleTemp.ToList().Select( prb=>new Prb_ScheduleDTO()
                {
                    SiteId = Convert.ToInt32(prb.SiteId.ToString()),
                    ScheduleId = Convert.ToInt32(prb.ScheduleId.ToString()),
                    Description = prb.Description,
                    StartDateTime = prb.StartDateTime,
                    EndDateTime = prb.EndDateTime,
                    StatusId = Convert.ToInt32(prb.StatusId.ToString()),
                }
                    ).AsQueryable();
                return _Prb_Schedule.Select(x => x.ScheduleId).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
                // return 0;
            }
        }

        public int ProcessScheduleDataUpdate(Prb_ScheduleRequest request)
        {
            ProbeDBEntities _Db = DBHelper.Instance._Db;;
            try
            {
                Prb_SiteResponse response = new Prb_SiteResponse();
                var _Prb_Schedule = (from s in _Db.Prb_Schedule
                                     where s.ScheduleId == request.Prb_Schedule.ScheduleId && s.IsDeleted == false
                                     select s).FirstOrDefault();

                if (_Prb_Schedule != null)
                {
                    _Prb_Schedule.StatusId = request.Prb_Schedule.StatusId;
                    _Prb_Schedule.EndDateTime = request.Prb_Schedule.EndDateTime;
                    _Prb_Schedule.ModifiedBy = 1;
                    _Prb_Schedule.ModifiedDate = DateTime.Now;

                    _Db.SaveChanges();
                }
                return Convert.ToInt32(_Prb_Schedule.ScheduleId.ToString());
            }
            catch (Exception ex)
            {
                //  throw ex;
                return 0;
            }
        }



        // By Waheed
        //public int ProcessScheduleData(Prb_ScheduleRequest request)
        //{
        //    int ScheduleId = 0;
        //    try
        //    {
        //        using (DbCommand dbCommand = PrbDataBase.GetStoredProcCommand(DBConstant.Prb_Schedule.SP_PRB_SCHEDULE))
        //        {
        //            PrbDataBase.AddOutParameter(dbCommand, "@ReturnValue", DbType.Int32, 3);
        //            PrbDataBase.AddInParameter(dbCommand, "@ScheduleId", DbType.Int32, request.Prb_Schedule.ScheduleId);
        //            PrbDataBase.AddInParameter(dbCommand, "@SiteId", DbType.Int32, request.Prb_Schedule.SiteId);
        //            PrbDataBase.AddInParameter(dbCommand, "@StartDateTime", DbType.DateTime, request.Prb_Schedule.StartDateTime);
        //            PrbDataBase.AddInParameter(dbCommand, "@EndDateTime", DbType.DateTime, request.Prb_Schedule.EndDateTime);
        //            PrbDataBase.AddInParameter(dbCommand, "@Description", DbType.String, request.Prb_Schedule.Description);
        //            PrbDataBase.AddInParameter(dbCommand, "@StatusId", DbType.Int32, request.Prb_Schedule.StatusId);
        //            PrbDataBase.AddInParameter(dbCommand, "@OperationId", DbType.Int32, request.Prb_Schedule.OperationId);
        //            PrbDataBase.ExecuteNonQuery(dbCommand);

        //            ScheduleId = (int)dbCommand.Parameters["@ReturnValue"].Value;
        //        }
        //        return ScheduleId;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
    }
}