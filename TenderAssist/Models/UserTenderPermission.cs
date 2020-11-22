using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using TenderAssist.Models.DBConnection;

namespace TenderAssist.Models
{
    [MetadataType(typeof(TbMemberUserTenderPermissionMetaData))]
    public class UserTenderPermission
    {
        readonly TenderAssistEntities _db = new TenderAssistEntities();

        public bool IndianTenderServiceAccess(int clientId)
        {
            bool isAccess = false;
            var list = (from u in _db.tabClientPermissions
                        where u.intClientId == clientId && u.bitActive && u.bitIndianOrGlobal
                        select u).OrderByDescending(x => x.intPermissionId).ToList();

            if (list != null)
            {
                if (list.Any())
                {
                    isAccess = true;
                }
            }

            return isAccess;
        }
        public bool GlobalTenderServiceAccess(int clientId)
        {
            bool isAccess = false;
            var list = (from u in _db.tabClientPermissions
                        where u.intClientId == clientId && u.bitActive && u.bitIndianOrGlobal == false
                        select u).OrderByDescending(x => x.intPermissionId).ToList();

            if (list != null)
            {
                if (list.Any())
                {
                    isAccess = true;
                }
            }

            return isAccess;
        }

        
        public List<tabClientPermission> GetIndianTenderPermissionList(int clientId)
        {
            var list = (from u in _db.tabClientPermissions
                        where u.intClientId == clientId && u.bitActive && u.bitIndianOrGlobal
                        select u).OrderByDescending(x => x.intPermissionId).ToList();
            return list;
        }
        public List<tabClientPermission> GetGlobalTenderPermissionList(int clientId)
        {
            var list = (from u in _db.tabClientPermissions
                        where u.intClientId == clientId && u.bitActive && u.bitIndianOrGlobal == false
                        select u).OrderByDescending(x => x.intPermissionId).ToList();
            return list;
        }
        public tabClientPermission GetUsersTenderPermissionById(int permissionId)
        {
            var permissin = (from u in _db.tabClientPermissions
                             where u.intPermissionId == permissionId
                             select u).FirstOrDefault();
            return permissin;
        }


        public List<tabClientPermissionWithProduct> GetUserPermissionWithProduct(int permissionId)
        {
            var permissionWithProduct = (from u in _db.tabClientPermissionWithProducts
                                         where u.intPermissionId == permissionId
                                         select u).ToList();
            return permissionWithProduct;
        }
        public List<tabClientPermissionWithLocation> GetUserPermissionWithLocation(int permissionId)
        {
            var permissionWithlocation = (from u in _db.tabClientPermissionWithLocations
                                          where u.intPermissionId == permissionId
                                          select u).ToList();
            return permissionWithlocation;
        }
        public List<tabClientPermissionWithAgency> GetUserPermissionWithAgency(int permissionId)
        {
            var permissionWithAgency = (from u in _db.tabClientPermissionWithAgencies
                                        where u.intPermissionId == permissionId
                                        select u).ToList();
            return permissionWithAgency;
        }
        public List<tabClientPermissionWithSector> GetUserPermissionWithSector(int permissionId)
        {
            var permissionWithSector = (from u in _db.tabClientPermissionWithSectors
                                        where u.intPermissionId == permissionId
                                        select u).ToList();
            return permissionWithSector;
        }
        public List<tabClientPermissionWithOwnership> GetUserPermissionWithOwnership(int permissionId)
        {
            var permissionWithOwnership = (from u in _db.tabClientPermissionWithOwnerships
                                           where u.intPermissionId == permissionId
                                           select u).ToList();
            return permissionWithOwnership;
        }
        public List<tabClientPermissionWithIndSubIndustry> GetUserPermissionWithIndSubIndustry(int permissionId)
        {
            var permissionWithIndSubIndustry = (from u in _db.tabClientPermissionWithIndSubIndustries
                                                where u.intPermissionId == permissionId
                                                select u).ToList();
            return permissionWithIndSubIndustry;
        }

        public void Update()
        {
            _db.SaveChanges();
        }

                

        public int UserId { get; set; }

        public List<tabClientPermission> UserAllTenderPermissionDetails { get; set; }
        public List<tabClientPermissionWithOwnership> UserPermissionWithOwnershipDetails { get; set; }
        public List<tabClientPermissionWithSector> UserPermissionWithSectorDetails { get; set; }
        public List<tabClientPermissionWithAgency> UserPermissionWithAgencyDetails { get; set; }

        public string SelectedCountry { get; set; }
        public string SelectedWord { get; set; }
        public string SelectedState { get; set; }
        public string SelectedCity { get; set; }
        public string SelectedOwnership { get; set; }
        public string SelectedSector { get; set; }
        public string SelectedAgency { get; set; }
        public string SelectedIndustry { get; set; }
        public string SelectedSubIndustry { get; set; }
        public string SelectedProduct { get; set; }
        public string SelectedInformationSource { get; set; }


    }

    public class TbMemberUserTenderPermissionMetaData
    {
        [ScaffoldColumn(false)]
        [DisplayName("PermissionID")]
        public int PermissionId { get; set; }

        [DisplayName("UserID")]
        public int UserId { get; set; }

        [DisplayName("Permission Name")]
        [Required(ErrorMessage = "Permission Name is required.")]
        public string PermissionName { get; set; }

        [DisplayName("CreatedBy")]
        public string CreatedBy { get; set; }

        [DisplayName("CreatedDate")]
        public DateTime CreatedDate { get; set; }

        [DisplayName("UpdatedBy")]
        public string UpdatedBy { get; set; }

        [DisplayName("CreatedDate")]
        public DateTime UpdatedDate { get; set; }

        [DisplayName("Is Active")]
        public bool IsActive { get; set; }

        [DisplayName("Buy")]
        public bool Buy { get; set; }

        [DisplayName("Sell")]
        public bool Cell { get; set; }

        [DisplayName("Contract")]
        public bool Contract { get; set; }

        [DisplayName("Is Indian Or Global")]
        public bool IsIndianOrGlobal { get; set; }

        [DisplayName("Other Keywords")]
        public string OtherKeywords { get; set; }

        [DisplayName("Is ICB")]
        public bool IsIcb { get; set; }

        [DisplayName("Not Used Keywords")]
        public string NotUsedKeywords { get; set; }

        [DisplayName("Tender Amount Type")]
        public int TenderAmountType { get; set; }

        [DisplayName("Tender Amount")]
        public float TenderAmount { get; set; }
    }
}