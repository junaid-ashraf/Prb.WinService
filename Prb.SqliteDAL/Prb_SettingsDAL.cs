using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prb.DTO;


namespace Prb.SqliteDAL
{
    public class Prb_SettingsDAL
    {
        Prb_Setting data = new Prb_Setting();
        public int AddProbeSetting(Prb_SettingDTO obj)
        {
            ProbeDBEntities _Db = DBHelper.Instance._Db;// DBHelper.Instance._Db;;
            try
            {
                data.SiteId = obj.SiteId;
                data.DomainName = obj.CustomerName;
                data.DomainAdminUser = obj.DomainAdminUser;
                data.Password = obj.Password;
                data.DomainType = obj.DomainType;
                data.WindowsAccess = obj.WindowsAccess;
                data.MacAccess = obj.MacAccess;
                data.LinuxAccess = obj.LinuxAccess;
                data.PrintersAccess = obj.PrintersAccess;
                data.RoutersSwitchesAccess = obj.RoutersSwitchesAccess;
                data.OthersAccess = obj.OthersAccess;
                data.HardwareDetail = obj.HardwareDetail;
                data.SoftwareDetail = obj.SoftwareDetail;

                data.CreatedDate = DateTime.Now;
                data.ModifiedDate = DateTime.Now;
                data.CreatedBy = 0;
                data.ModifiedBy = 0;
                data.IsActive = true;
                data.IsDeleted = false;
                data.IsRead = 0;
                data.ProbeRunningMachine = Environment.MachineName;
                data.ProbeRunningDomain = Environment.UserDomainName;
                _Db.Prb_Setting.Add(data);
                _Db.SaveChanges();

                // Probe Site Data


                return Convert.ToInt32(data.SettingId.ToString());
            }
            catch (Exception ex)
            {
                return Convert.ToInt32(data.SettingId.ToString());

                //                return ex.Message;
            }
        }
        // Select Probe Setting
        public Prb_SettingDTO GetProbeSetting(Prb_SettingDTO obj)
        {
            // Domian Name, Domain Admin, Pass
            string _DomainName, _DomainAdminUser, _DomianPass = "";

            ProbeDBEntities _Db = DBHelper.Instance._Db;;

            //int maxSettingId = Convert.ToInt32(_Db.Prb_Setting.Max(x => x.SettingId));
            var ResData = (from s in _Db.Prb_Setting
                           where s.IsActive == true && s.IsDeleted == false && s.IsRead == 0
                           select s).OrderByDescending(s => s.CreatedDate).FirstOrDefault();



            var ResSiteData = new Prb_Site();
            var ResSiteCredData = new Prb_SiteCredential();
            //if (ResData.DomainType == obj.DomainType)
            //{
            //    ResSiteData = (from s in _Db.Prb_Site
            //                   where s.IsActive == true && s.IsDeleted == false && s.CustomerName == obj.DomainName
            //                   select s).OrderByDescending(s => s.SiteId).FirstOrDefault();


            //    ResSiteCredData = (from s in _Db.Prb_SiteCredential
            //                       where s.IsActive == true && s.IsDeleted == false && s.SiteId == obj.SiteId
            //                       select s).FirstOrDefault();
            //    // Set Cred from Db if User Set Setting as BySite Domain Credential
            //    _DomainName = ResSiteData.CustomerName;
            //    _DomainAdminUser = ResSiteCredData.Username;
            //    _DomianPass = ResSiteCredData.Password;
            //}
            //else
            //{
            // Set Cred from Db if User Set Setting as Custom(ByUser) Domain Credential
            if (ResData != null)
            {
                _DomainName = ResData.DomainName;
                _DomainAdminUser = ResData.DomainAdminUser;
                _DomianPass = ResData.Password;
                //}

                long? IsRead = ResData.IsRead;
                // Return Probe Setting Credentials

                Prb_SettingDTO resDto = new Prb_SettingDTO
                {
                    SettingId = Convert.ToInt32(ResData.SettingId.ToString()),
                    SiteId = Convert.ToInt32(ResData.SiteId.ToString()),
                    DomainType = ResData.DomainType,
                    SiteName = _DomainName,
                    DomainName = _DomainName,
                    DomainAdminUser = _DomainAdminUser,
                    Password = _DomianPass,
                    WindowsAccess = ResData.WindowsAccess,
                    MacAccess = ResData.MacAccess,
                    LinuxAccess = ResData.LinuxAccess,
                    PrintersAccess = ResData.PrintersAccess,
                    RoutersSwitchesAccess = ResData.RoutersSwitchesAccess,
                    OthersAccess = ResData.OthersAccess,
                    HardwareDetail = ResData.HardwareDetail,
                    SoftwareDetail = ResData.SoftwareDetail,
                    IsRead = IsRead != null ? Convert.ToInt32(IsRead) : 1
                };
                return resDto;
            }
            else
                return new Prb_SettingDTO();
        }



        // Select Probe Setting
        public bool ProbeSettingUpdateStatusAfterComplete(Prb_SettingDTO obj)
        {
            ProbeDBEntities _Db = DBHelper.Instance._Db;;

            var ResData = (from s in _Db.Prb_Setting
                           where s.IsActive == true && s.IsDeleted == false && s.SettingId == obj.SettingId
                           select s).FirstOrDefault();

            // Update Status After Probe Is running completed
            if (ResData != null)
            {
                ResData.IsActive = false;
            }
            try
            {
                _Db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
                return false;
            }
        }

        public void UpdateIsRead(int settingId)
        {
            ProbeDBEntities _Db = DBHelper.Instance._Db;;
            var setting = (from s in _Db.Prb_Setting
                           where s.SettingId == settingId
                           select s).FirstOrDefault();
            setting.IsRead = 1;
            _Db.Entry(setting).State = System.Data.Entity.EntityState.Modified;
            _Db.SaveChanges();
        }
        public void AddProbeFailure(Prb_ADConnectionFailureDTO obj)
        {
            Prb_ADConnectionFailure data = new Prb_ADConnectionFailure();
            ProbeDBEntities _Db = DBHelper.Instance._Db;;
            try
            {
                data.SettingId = obj.SettingId;
                data.FailureReason = obj.Description;
                _Db.Prb_ADConnectionFailure.Add(data);
                _Db.SaveChanges();
            }
            catch (Exception e)
            {

            }
        }
    }
}