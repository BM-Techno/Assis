using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TenderAssist.ViewModel
{

    public class InquiryRegFormFields
    {
        public int ID { get; set; }
        public int InquiryTypeID { get; set; }
        public string Name { get; set; }
        public string EmailID { get; set; }
        public string Designation { get; set; }
        public string CompName { get; set; }
        public string Address { get; set; }
        public int Country { get; set; }
        public int State { get; set; }
        public int City { get; set; }
        public string MobNo { get; set; }
        public string PhoneNo { get; set; }
        public string InterestedTenders { get; set; }
        public string Website { get; set; }
        public int intClientPurpose { get; set; }
        public int OurRefNo { get; set; }
        public int NewID { get; set; }
        public int Flag { get; set; }
        public int ModuleType { get; set; }
        public int LinkId { get; set; }
        public string BrowserLink { get; set; }
        public string FormTitle { get; set; }
        public string ClientIPAddress { get; set; }


    }
}