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
    public class DomainUsersInfoAccess
    {
        ProbeDBEntities _Db = DBHelper.Instance._Db;
        public DomainUsersInfoResponse UsersList_InsertData(List<DomainUsersInfo> usersList,  int ScheduleId)
        {
          var Prb_Schedule =  _Db.Prb_Schedule.Where(s => s.ScheduleId == ScheduleId).FirstOrDefault();

             DomainUsersInfoResponse response = new DomainUsersInfoResponse();
            try
            {
                if (usersList != null)
                {
                    foreach (var datarow in usersList)
                    {
                        DomainUsersInfo _DomainUsersInfo = new DomainUsersInfo()
                        {
                            //DomainUsersInfoId = datarow.DomainUsersInfoId,
                            SiteId = Prb_Schedule.SiteId,
                            ScheduleId = ScheduleId,

                            SamAccountName = datarow.SamAccountName,
                            DisplayName = datarow.DisplayName,
                            Name = datarow.Name,
                            GivenName = datarow.GivenName,
                            Surname = datarow.Surname,
                            Description = datarow.Description,
                            Enabled = datarow.Enabled,
                            EmployeeId = datarow.EmployeeId,
                            LastPasswordSet = datarow.LastPasswordSet,
                            LastBadPasswordAttempt = datarow.LastBadPasswordAttempt,
                            LastLogon = datarow.LastLogon,
                            CreatedBy = 1,
                            CreatedDate = DateTime.Now,
                            ModifiedBy = 1,
                            ModifiedDate = DateTime.Now,
                            IsDeleted = false,
                        };
                        _Db.DomainUsersInfoes.Add(_DomainUsersInfo);
                        try
                        {
                            _Db.SaveChanges();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(". Exception On Data Inserstion Dmoain Users. Message: " + ex.Message);
                        }
                    }
                    try
                    {
                        _Db.SaveChanges();
                        return response;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(". Exception On Data Inserstion Dmoain Users. Message: " + ex.Message);
                        // WriteTextFile.WriteErrorLog(". Exception On Data Inserstion for Hw_DetailInfo IpAddress: " + datarow.IPAddress + " Message: " + ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(". Exception On Data Inserstion Dmoain Users. Message: " + ex.Message);
                // WriteTextFile.WriteErrorLog(". Exception On Data Inserstion for Hw_DetailInfo IpAddress: " + datarow.IPAddress + " Message: " + ex.Message);
            }
            return response;
        }
    }
}
