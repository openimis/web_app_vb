// 'Copyright (c) 2016-2017 Swiss Agency for Development and Cooperation (SDC)
// '
// 'The program users must agree to the following terms:
// '
// 'Copyright notices
// 'This program is free software: you can redistribute it and/or modify it under the terms of the GNU AGPL v3 License as published by the 
// 'Free Software Foundation, version 3 of the License.
// 'This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of 
// 'MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU AGPL v3 License for more details www.gnu.org.
// '
// 'Disclaimer of Warranty
// 'There is no warranty for the program, to the extent permitted by applicable law; except when otherwise stated in writing the copyright 
// 'holders and/or other parties provide the program "as is" without warranty of any kind, either expressed or implied, including, but not 
// 'limited to, the implied warranties of merchantability and fitness for a particular purpose. The entire risk as to the quality and 
// 'performance of the program is with you. Should the program prove defective, you assume the cost of all necessary servicing, repair or correction.
// '
// 'Limitation of Liability 
// 'In no event unless required by applicable law or agreed to in writing will any copyright holder, or any other party who modifies and/or 
// 'conveys the program as permitted above, be liable to you for damages, including any general, special, incidental or consequential damages 
// 'arising out of the use or inability to use the program (including but not limited to loss of data or data being rendered inaccurate or losses 
// 'sustained by you or third parties or a failure of the program to operate with any other programs), even if such holder or other party has been 
// 'advised of the possibility of such damages.
// '
// 'In case of dispute arising out or in relation to the use of the program, it is subject to the public law of Switzerland. The place of jurisdiction is Berne.
// 
// 
// 

using System.Web.SessionState;
using System.Configuration;
using System.Web.Caching;
using System.Collections.Specialized;
using System.Web.UI.WebControls.WebParts;
using System.Data;
using System.Diagnostics;
using Microsoft.VisualBasic;
using System.Web.UI.WebControls;
using System.Text;
using System.Web.Profile;
using System.Web;
using System.Linq;
using System.Web.Security;
using System.Collections.Generic;
using System.Collections;
using System.Web.UI.HtmlControls;
using System.Web.UI;
using System.Text.RegularExpressions;
using System;
using System.Xml.Linq;
using System.IO;

namespace IMIS_BL
{
    public class InsureeBL
    {
        private IMIS_DAL.InsureeDAL Insuree = new IMIS_DAL.InsureeDAL();
        private GeneralBL imisgen = new GeneralBL();
        public DataTable GetInsureesByFamily(int FamilyId, string Language = "en")
        {
            return Insuree.GetInsureesByFamily(FamilyId);
        }
        public int SaveInsuree(IMIS_EN.tblInsuree eInsuree, bool Activate)
        {
            if (InsureeExists(eInsuree))
                return 1;
            if (eInsuree.InsureeID == 0)
            {
                Insuree.InsertInsuree(ref eInsuree);
                AddInsuree(eInsuree.InsureeID, Activate);
                IMIS_DAL.PolicyDAL Policy = new IMIS_DAL.PolicyDAL();

                return 0;
            }
            else
            {
                Insuree.ModifyInsuree(eInsuree);
                return 2;
            }
        }
        public void AddInsuree(int InsureeId, bool Activate = false)
        {
            IMIS_BL.InsureePolicyBL IP = new IMIS_BL.InsureePolicyBL();
            IP.AddInsuree(InsureeId, Activate);
        }

        public bool MoveInsuree(IMIS_EN.tblInsuree eInsureeNew, bool Activate)
        {
            IMIS_DAL.InsureeDAL InsureeDAL = new IMIS_DAL.InsureeDAL();

            if (InsureeDAL.MoveInsuree(eInsureeNew) == true)
            {
                DeletePolicyInsuree(eInsureeNew.InsureeID);
                AddInsuree(eInsureeNew.InsureeID, Activate);
                return true;
            }
            else
                return false;
        }
        public void LoadInsuree(ref IMIS_EN.tblInsuree eInsuree)
        {
            Insuree.LoadInsuree(ref eInsuree);
        }
        public void GetInsureesByCHFID(ref IMIS_EN.tblInsuree einsuree)
        {
            Insuree.GetInsureesByCHFID(ref einsuree);
        }
        public DataTable GetCHFNumbers()
        {
            return Insuree.GetCHFNumbers();
        }
        public void FindInsuree(ref IMIS_EN.tblInsuree eInsuree, bool All = false, Int16 PhotoAssigned = 1, string Language = "en")
        {
            return Insuree.GetInsureeFullSearch(eInsuree, All, PhotoAssigned, Language);
        }
        public bool InsureeExists(IMIS_EN.tblInsuree eInsuree)
        {
            DataTable dt = Insuree.InsureeExists(eInsuree);
            if (dt.Rows.Count > 0)
                return true;
            else
                return false;
        }
        public DataTable FetchNewImages(string ImagePath, string CHFID)
        {
            FileInfo[] Images;

            DataTable dt = new DataTable();
            dt.Columns.Add("ImagePath");
            dt.Columns.Add("TakenDate", System.Type.GetType("System.DateTime"));

            DataRow dr = dt.NewRow();

            if (!(CHFID == null))
            {
                DirectoryInfo DirInfo = new DirectoryInfo(ImagePath);
                Images = DirInfo.GetFiles(CHFID + "*");
                var loopTo = Images.Count() - 1;
                // Images = Directory.GetFiles(ImagePath, CHFID & "*")

                for (int i = 0; i <= loopTo; i++)
                {
                    IMIS_BL.FamilyBL Family = new IMIS_BL.FamilyBL();
                    if (Family.getOfficerID(Images[i].Name) == -1)
                        continue;

                    dr["ImagePath"] = Images[i].Name;

                    dr["TakenDate"] = Family.ExtractDate(Images[i].Name);
                    dt.Rows.Add(dr);
                    dr = dt.NewRow();
                }
            }

            return dt;
        }
        public void MoveImageToFolder(string SrcPath, string DestPath, string FileName)
        {
            Directory.Move(SrcPath + FileName, DestPath + FileName);
        }
        public void UpdateImage(ref IMIS_EN.tblPhotos ePhotos, bool UpdateInDatabase = true)
        {
            MoveImageToFolder(HttpContext.Current.Server.MapPath(IMIS_EN.AppConfiguration.SubmittedFolder), HttpContext.Current.Server.MapPath(IMIS_EN.AppConfiguration.UpdatedFolder), ePhotos.PhotoFileName);
            if (UpdateInDatabase == true)
                Insuree.UpdateImage(ref ePhotos);
        }
        public void FindInsureeByCHFID(string CHFID)
        {
            return Insuree.FindInsureeByCHFID(CHFID);
        }
        public string verifyCHFIDandReturnName(string CHFID, ref int insureeid)
        {
            return Insuree.verifyCHFIDandReturnName(CHFID, ref insureeid);
        }
        public bool ChangeHead(IMIS_EN.tblInsuree eInsureeOLD, IMIS_EN.tblInsuree eInsureeNew)
        {
            IMIS_DAL.InsureeDAL InsureeDAL = new IMIS_DAL.InsureeDAL();
            return InsureeDAL.ChangeHead(eInsureeOLD, eInsureeNew);
        }
        public int DeleteInsuree(IMIS_EN.tblInsuree eInsuree)
        {
            IMIS_DAL.InsureeDAL insuree = new IMIS_DAL.InsureeDAL();
            DataTable dt = insuree.CheckCanBeDeleted(eInsuree.InsureeID);
            if (dt.Rows.Count > 0)
                return 2;

            if (insuree.DeleteInsuree(eInsuree))
            {
                DeletePolicyInsuree(eInsuree.InsureeID);
                return 1;
            }
            else
                return 0;
        }
        public void DeletePolicyInsuree(int InsureeId)
        {
            IMIS_BL.InsureePolicyBL IP = new IMIS_BL.InsureePolicyBL();
            IP.DeletePolicyInsuree(InsureeId, 0);
        }
        public DataTable GetPhotoAssigned()
        {
            DataTable dt = new DataTable();
            DataRow dr;

            dt.Columns.Add("PhotoAssignedValue");
            dt.Columns.Add("PhotoAssignedText");

            dr = dt.NewRow();
            dr["PhotoAssignedValue"] = 1;
            dr["PhotoAssignedText"] = imisgen.getMessage("T_ALL");
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["PhotoAssignedValue"] = 2;
            dr["PhotoAssignedText"] = imisgen.getMessage("T_YES");
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["PhotoAssignedValue"] = 3;
            dr["PhotoAssignedText"] = imisgen.getMessage("T_NO");
            dt.Rows.Add(dr);

            return dt;
        }
        public bool GetInsureeOfflineValue(int InsureeID)
        {
            return Insuree.GetInsureeOfflineValue(InsureeID);
        }
        public DataTable GetRelations()
        {
            GeneralBL Gen = new GeneralBL();
            IMIS_DAL.RelationsDAL DAL = new IMIS_DAL.RelationsDAL();
            DataTable dt = DAL.GetRelations();
            DataRow dr;
            dr = dt.NewRow();
            dr["RelationId"] = 0;
            dr["Relation"] = Gen.getMessage("M_SELECTRELATION");
            dr["AltLanguage"] = Gen.getMessage("M_SELECTRELATION");
            dt.Rows.InsertAt(dr, 0);
            return dt;
        }
        public DataTable GetProfession()
        {
            GeneralBL Gen = new GeneralBL();

            IMIS_DAL.ProfessionsDAL DAL = new IMIS_DAL.ProfessionsDAL();
            DataTable dt = DAL.GetProfessions();
            DataRow dr;
            dr = dt.NewRow();
            dr["ProfessionId"] = 0;
            dr["Profession"] = Gen.getMessage("M_SELECTPROFESSION");
            dr["AltLanguage"] = Gen.getMessage("M_SELECTPROFESSION");

            dt.Rows.InsertAt(dr, 0);
            return dt;
        }
        public DataTable GetEducation()
        {
            GeneralBL Gen = new GeneralBL();
            IMIS_DAL.EducationsDAL DAL = new IMIS_DAL.EducationsDAL();
            DataTable dt = DAL.GetEducations();
            DataRow dr;
            dr = dt.NewRow();
            dr["EducationId"] = 0;
            dr["Education"] = Gen.getMessage("M_SELECTEDUCATION");
            dr["AltLanguage"] = Gen.getMessage("M_SELECTEDUCATION");
            dt.Rows.InsertAt(dr, 0);
            return dt;
        }
        public DataTable GetInsureeProductDetails(Dictionary<string, object> oDict, string CHFID, string ItemCode, string ServiceCode)
        {
            return Insuree.GetInsureeProductDetails(oDict, CHFID, ItemCode, ServiceCode);
        }
        public DataTable GetMaxMemberCount(int FamilyId)
        {
            IMIS_DAL.InsureeDAL Insuree = new IMIS_DAL.InsureeDAL();
            return Insuree.GetMaxMemberCount(FamilyId);
        }

        public DataTable GetTypeOfIdentity()
        {
            GeneralBL Gen = new GeneralBL();
            IMIS_DAL.IdentificationTypesDAL DAL = new IMIS_DAL.IdentificationTypesDAL();
            DataTable dt = DAL.GetIdentificationTypes();
            DataRow dr;
            dr = dt.NewRow();
            dr["IdentificationCode"] = "";
            dr["IdentificationTypes"] = Gen.getMessage("M_SELECTIDENTITY");
            dr["AltLanguage"] = Gen.getMessage("M_SELECTIDENTITY");
            dt.Rows.InsertAt(dr, 0);
            return dt;
        }
    }
}
