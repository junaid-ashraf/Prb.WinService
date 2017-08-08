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
//using AutoMapper;
using System.Data.SqlClient;

namespace Prb.SqliteDAL
{
    public class Prb_SiteDataAccess
    {
        public IQueryable<Prb_SiteDTO> GetSiteData()
        {
            ProbeDBEntities _Db = DBHelper.Instance._Db;;
            Prb_SiteResponse response = new Prb_SiteResponse();
            Prb_SiteDTO prb_SiteDTO = new Prb_SiteDTO();

          
            var Prb_SiteTemp = (from s in _Db.Prb_Site
                            where s.IsActive == true && s.IsDeleted == false
                            select new
                            {
                                SiteId = s.SiteId,
                                CustomerName = s.CustomerName,
                                Description = s.Description,
                                IsActive = s.IsActive
                            }
            );
            var Prb_Site = Prb_SiteTemp.ToList().Select(s=>new Prb_SiteDTO()
            {
                SiteId = Convert.ToInt32(s.SiteId.ToString()),
                CustomerName = s.CustomerName,
                Description = s.Description,
                IsActive = s.IsActive
            }
                ).AsQueryable();
            return Prb_Site;
        }

        // Wrriten by waheed

        public Prb_SiteResponse GetSiteData_Waheed()
        {
            Prb_SiteResponse response = new Prb_SiteResponse();
            try
            {
                //using (DbCommand dbCommand = PrbDataBase.GetStoredProcCommand(DBConstant.Prb_Site.SP_PRB_SITE))
                //{
                //    PrbDataBase.AddInParameter(dbCommand, "@OperationId", DbType.Int32, 1);

                //    DataSet ds = PrbDataBase.ExecuteDataSet(dbCommand);
                //    if (ds.Tables != null && ds.Tables.Count > 0)
                //    {
                //        response.Prb_SiteList = MapDataTableToList<Prb_SiteDTO>(ds.Tables[0]).ToList();
                //    }
                //}
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
