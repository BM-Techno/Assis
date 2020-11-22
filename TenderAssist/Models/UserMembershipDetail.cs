using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.Data;
using TenderAssist.ViewModel;
using TenderAssist.Models.DBConnection;

namespace TenderAssist.Models
{
    [MetadataType(typeof(TbUserMembershipDetailMetaData))]
    public class UserMembershipDetail
    {
        readonly TenderAssistEntities _db = new TenderAssistEntities();
        readonly LocalTenderAssistEntities _dblocal = new LocalTenderAssistEntities();

        public tabClientDetail GetUserLoginDetail(string emailid, string pswd)
        {
            var userinfo = _db.tabClientDetails.SingleOrDefault(x => x.strEmailId1.Trim().ToLower() == emailid.Trim().ToLower() && x.strPassword == pswd
                );
            return userinfo;
        }
        public List<tabClientDetail> GetAllUsers()
        {
            var list = (from u in _db.tabClientDetails
                        where u.intClientId != 0
                        select u).OrderBy(x => x.intClientId).ToList();
            return list;
        }
        public tabClientDetail GetUsersById(int clientId)
        {
            var user = (from u in _db.tabClientDetails
                        where u.intClientId == clientId
                        select u).FirstOrDefault();
            return user;
        }


        public tabClientDetail GetUsersByUniqueId(Guid userUniqueNo)
        {
            var user = (from u in _db.tabClientDetails
                        where u.uniUniqueNo == userUniqueNo
                        select u).FirstOrDefault();
            return user;
        }
        public tabClientDuration GetUsersActivationdetailById(int clientId)
        {
            var userActivation = (from u in _db.tabClientDurations
                                  where u.intClientId == clientId
                                  select u).FirstOrDefault();
            return userActivation;
        }
        public bool GetAllUsersByEmail(string emailid, int clientId)
        {
            var list = (from u in _db.tabClientDetails
                        where u.intClientId != clientId && (u.strEmailId1.ToLower().Trim() == emailid || u.strEmailId2.ToLower().Trim() == emailid)
                        select u).OrderBy(x => x.intClientId).Any();
            return list;
        }
        public bool GetAllUsersByLoginId(string emailid, int clientId)
        {
            var list = (from u in _db.tabClientDetails
                        where u.intClientId != clientId && (u.strEmailId1.ToLower().Trim() == emailid)
                        select u).OrderBy(x => x.intClientId).Any();
            return list;
        }

        public void UpdateUserLastLoginDate(int clientId)
        {
            var user = (from u in _db.tabClientDetails
                        where u.intClientId == clientId
                        select u).FirstOrDefault();

            if (user != null)
            {
                user.dtLastLoginDate = DateTime.Now;
            }
            Update();
        }

        public void AddInquiry(tbInquiryRegForm tableName)
        {
            _db.tbInquiryRegForms.Add(tableName);
        }
        public void Update()
        {
            _db.SaveChanges();
        }

        public List<SelectListItem> StateList { get; set; }
        public List<SelectListItem> CityList { get; set; }
        public List<SelectListItem> CountryList { get; set; }

        public List<SelectListItem> ActiveTypeList { get; set; }
        public List<SelectListItem> ConfirmTypeList { get; set; }

        
        #region USER FAVOURITE TENDERS

        public List<UserFavouriteTenderList> GetAllFavTendersByClient(int clientId, int tenderType)
        {
            var list = (from f in _db.tabClientFavouriteTenders
                        where f.intClientId == clientId && f.intTenderType == tenderType

                        select new UserFavouriteTenderList
                        {
                            IntFavTenderId = f.intFavTenderId,
                            IntClientId = f.intClientId,
                            IntOurRefNo = f.intOurRefNo,
                            IntTenderType = f.intTenderType,
                            DatEnterDate = f.datEnterDate
                        }).ToList();

            return list;
        }

        public void AddUserFavTender(tabClientFavouriteTender userFavouriteTenderList)
        {
            _db.tabClientFavouriteTenders.Add(userFavouriteTenderList);
            _db.SaveChanges();
        }

        public void DeleteUserFavTender(tabClientFavouriteTender userFavouriteTenderList)
        {
            _db.tabClientFavouriteTenders.Remove(userFavouriteTenderList);
            _db.SaveChanges();
        }

        public tabClientFavouriteTender GetFavTendersById(int clientId, int tenderType, int ourRefNo)
        {
            var det = from f in _db.tabClientFavouriteTenders
                      where f.intClientId == clientId && f.intTenderType == tenderType && f.intOurRefNo == ourRefNo
                      select f;

            return det.FirstOrDefault();
        }

        #endregion


        #region LOCAL DATA
        public tabClientDetail GetUsersByPermissionId_Local(int permissionId)
        {
            var user = (from u in _dblocal.tabClientDetails
                        join p in _dblocal.tabClientPermissions on u.intClientId equals p.intClientId
                        where (p.intPermissionId == permissionId)
                        select u).FirstOrDefault();
            return user;
        }
        public List<tabEmpEmailId> GetLocalEmpEamilId_Local(int clientId, string display, string purpose)
        {
            var user = new List<tabEmpEmailId>();
            switch (display)
            {
                case "local":
                    user = (from ee in _dblocal.tabEmpEmailIds
                            join cd in _dblocal.tabClientDetails on ee.intEmpId equals cd.intSalesEmpId
                            where (cd.intClientId == clientId) 
                            //&& (!(ee.strEmailId.Contains(purpose)))
                            select ee).ToList();
                    if (user.Count == 0)
                    {
                        user = (from ee in _dblocal.tabEmpEmailIds
                                join cd in _dblocal.tabClientDetails on ee.intEmpId equals cd.intCareEmpId
                                where (cd.intClientId == clientId) 
                                //&& (!(ee.strEmailId.Contains(purpose)))
                                select ee).ToList();
                    }
                    break;
                case "care":
                    user = (from ee in _dblocal.tabEmpEmailIds
                            join cd in _dblocal.tabClientDetails on ee.intEmpId equals cd.intCareEmpId
                            where (cd.intClientId == clientId) 
                            //&& (!(ee.strEmailId.Contains(purpose)))
                            select ee).ToList();
                    break;

            }
            return user;
        }
        public tabEmpDetail GetLocalEmpDetails_Local(int clientId, string display)
        {
            var user = new tabEmpDetail();
            switch (display)
            {
                case "local":
                    user = (from ee in _dblocal.tabEmpDetails
                            join cd in _dblocal.tabClientDetails on ee.intEmpId equals cd.intSalesEmpId
                            where (cd.intClientId == clientId)
                            select ee).FirstOrDefault();
                    if (user == null)
                    {
                        user = (from ee in _dblocal.tabEmpDetails
                                join cd in _dblocal.tabClientDetails on ee.intEmpId equals cd.intCareEmpId
                                where (cd.intClientId == clientId)
                                select ee).FirstOrDefault();
                    }
                    break;
                case "care":
                    user = (from ee in _dblocal.tabEmpDetails
                            join cd in _dblocal.tabClientDetails on ee.intEmpId equals cd.intCareEmpId
                            where (cd.intClientId == clientId)
                            select ee).FirstOrDefault();
                    break;

            }
            return user;
        }
        public tabEmpDetail GetLocalEmpEamilIdWithEmpId_Local(int empId)
        {
            var user = (from ed in _dblocal.tabEmpDetails
                        join ee in _dblocal.tabEmpEmailIds
                        on ed.intEmpId equals ee.intEmpId
                        where ed.intEmpId == empId
                        select ed).FirstOrDefault();
            return user;
        }
        public tabEmpEmailId GetLocalEmpEamilIdWithEmailId_Local(string emailId)
        {
            var user = (from ee in _dblocal.tabEmpEmailIds
                        where ee.strEmailId == emailId
                        select ee).FirstOrDefault();
            return user;
        }
        public tabClientPermission GetUsersTenderPermissionById_Local(int permissionId)
        {
            var permissin = (from u in _dblocal.tabClientPermissions
                             where u.intPermissionId == permissionId
                             select u).FirstOrDefault();
            return permissin;
        }
        public List<tabClientPermissionWithAgency> GetUserPermissionWithAgency_Local(int permissionId)
        {
            var permissionWithAgency = (from u in _dblocal.tabClientPermissionWithAgencies
                                        where u.intPermissionId == permissionId
                                        select u).ToList();
            return permissionWithAgency;
        }
        public List<tabClientPermissionWithOwnership> GetUserPermissionWithOwnership_Local(int permissionId)
        {
            var permissionWithOwnership = (from u in _dblocal.tabClientPermissionWithOwnerships
                                           where u.intPermissionId == permissionId
                                           select u).ToList();
            return permissionWithOwnership;
        }
        public List<tabClientPermissionWithSector> GetUserPermissionWithSector_Local(int permissionId)
        {
            var permissionWithSector = (from u in _dblocal.tabClientPermissionWithSectors
                                        where u.intPermissionId == permissionId
                                        select u).ToList();
            return permissionWithSector;
        }
        public List<tabClientPermissionWithLocation> GetUserPermissionWithLocation_Local(int permissionId)
        {
            var permissionWithlocation = (from u in _dblocal.tabClientPermissionWithLocations
                                          where u.intPermissionId == permissionId
                                          select u).ToList();
            return permissionWithlocation;
        }
        public List<tabClientPermissionWithIndSubIndustry> GetUserPermissionWithIndSubIndustry_Local(int permissionId)
        {
            var permissionWithIndSubIndustry = (from u in _dblocal.tabClientPermissionWithIndSubIndustries
                                                where u.intPermissionId == permissionId
                                                select u).ToList();
            return permissionWithIndSubIndustry;
        }
        public List<tabClientPermissionWithProduct> GetUserPermissionWithProduct_Local(int permissionId)
        {
            var permissionWithProduct = (from u in _dblocal.tabClientPermissionWithProducts
                                         where u.intPermissionId == permissionId
                                         select u).ToList();
            return permissionWithProduct;
        }
        #endregion
    }

    public class TbUserMembershipDetailMetaData
    {
        [ScaffoldColumn(false)]
        [DisplayName("UserID")]
        public int UserId { get; set; }

        [DisplayName("First Name")]
        [Required(ErrorMessage = "First Name is required.")]
        public string FName { get; set; }

        [DisplayName("Last Name")]
        public string LName { get; set; }

        [DisplayName("Company")]
        [Required(ErrorMessage = "Company Name is required.")]
        public string CompanyName { get; set; }

        [DisplayName("Designation")]
        public string Designation { get; set; }

        [DisplayName("Address")]
        public string Address { get; set; }

        [DisplayName("Country")]
        [Required(ErrorMessage = "Country is required.")]
        public int Country { get; set; }

        [DisplayName("State")]
        [Required(ErrorMessage = "State is required.")]
        public int State { get; set; }

        [DisplayName("City")]
        [Required(ErrorMessage = "City is required.")]
        public int City { get; set; }

        [DisplayName("PIN")]
        public string Pin { get; set; }

        [DisplayName("Phone No")]
        [Required(ErrorMessage = "Phone No is required.")]
        public string PhoneNo { get; set; }

        [DisplayName("Mobile No")]
        [Required(ErrorMessage = "Mobile No is required.")]
        public string MobileNo { get; set; }

        [DisplayName("FAX")]
        public string Fax { get; set; }

        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "Email ID is required.")]
        [RegularExpression("^[a-zA-Z][\\w\\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\\w\\.-]*[a-zA-Z0-9]\\.[a-zA-Z][a-zA-Z\\.]*[a-zA-Z]$", ErrorMessage = "Email provided is invalid.")]
        [Display(Name = "Email ID")]
        public string EmailId { get; set; }

        [DataType(DataType.EmailAddress)]
        [Display(Name = "Second Email ID")]
        [RegularExpression("^[a-zA-Z][\\w\\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\\w\\.-]*[a-zA-Z0-9]\\.[a-zA-Z][a-zA-Z\\.]*[a-zA-Z]$", ErrorMessage = "Email provided is invalid.")]
        public string EmailId2 { get; set; }

        [DisplayName("Website")]
        [DataType(DataType.Url)]
        public string WebSite { get; set; }

        [DisplayName("Products")]
        [Required(ErrorMessage = "Products is required.")]
        public string Products { get; set; }

        [DisplayName("Remarks")]
        public string Remarks { get; set; }

        [DisplayName("EnterBy")]
        public string EnterBy { get; set; }

        [DisplayName("EnterDate")]
        public DateTime EnterDate { get; set; }

        [DisplayName("UpdateBy")]
        public string UpdateBy { get; set; }

        [DisplayName("UpdateDate")]
        public DateTime UpdateDate { get; set; }

        [DisplayName("LoginID")]
        [Required(ErrorMessage = "Login ID is required.")]
        public string LoginId { get; set; }

        [DisplayName("Password")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }

        [DisplayName("Active")]
        public int Active { get; set; }

        [DisplayName("Member Confirm")]
        public int MemberConfirm { get; set; }

        [DisplayName("Email Alert")]
        public int EmailAlert { get; set; }

        [DisplayName("Email Verify")]
        public int EmailVerify { get; set; }

        [DisplayName("Valid From")]
        public DateTime ValidFrom { get; set; }

        [DisplayName("Expired Date")]
        public DateTime ExpiredDate { get; set; }

        [DisplayName("Membership Duration")]
        public int MembershipDuration { get; set; }
    }
}