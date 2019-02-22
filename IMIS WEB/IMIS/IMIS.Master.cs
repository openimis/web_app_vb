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
using IMIS_BI;
using IMIS_EN;

namespace IMIS
{
    public partial class IMIS : System.Web.UI.MasterPage
    {
        private IMIS_BI.MasterBI MasterBI = new IMIS_BI.MasterBI();
        protected internal IMIS_Gen imisgen = new IMIS_Gen();
        private void FormatForm()
        {
            string Adjustibility = "";
            var loopTo = gvPolicy.Columns.Count;
            for (int i = 0; i <= loopTo; i++)
            {
                if (gvPolicy.Columns[i].HeaderText.Equals(imisgen.getMessage("L_PRODUCTCODE")))
                {
                    gvPolicy.Columns[i].Visible = !(Adjustibility == "N"); break;
                }
                if (gvPolicy.Columns[i].HeaderText.Equals(imisgen.getMessage("L_EXPIREDATE")))
                {
                    gvPolicy.Columns[i].Visible = !(Adjustibility == "N"); break;
                }
                if (gvPolicy.Columns[i].HeaderText.Equals(imisgen.getMessage("L_STATUS")))
                {
                    gvPolicy.Columns[i].Visible = !(Adjustibility == "N"); break;
                }
                if (gvPolicy.Columns[i].HeaderText.Equals(imisgen.getMessage("L_HDEDUCTION")))
                {
                    gvPolicy.Columns[i].Visible = !(Adjustibility == "N"); break;
                }
                if (gvPolicy.Columns[i].HeaderText.Equals(imisgen.getMessage("L_NHDEDUCTION")))
                {
                    gvPolicy.Columns[i].Visible = !(Adjustibility == "N"); break;
                }
                if (gvPolicy.Columns[i].HeaderText.Equals(imisgen.getMessage("L_HCEILING")))
                {
                    gvPolicy.Columns[i].Visible = !(Adjustibility == "N"); break;
                }
                if (gvPolicy.Columns[i].HeaderText.Equals(imisgen.getMessage("L_NHCEILING")))
                {
                    gvPolicy.Columns[i].Visible = !(Adjustibility == "N"); break;
                }
            }

            var loopTo1 = gvProduct.Columns.Count;
            for (int i = 0; i <= loopTo1; i++)
            {
                if (gvProduct.Columns[i].HeaderText.Equals(imisgen.getMessage("L_TOTALADMISSIONSLEFT")))
                {
                    gvProduct.Columns[i].Visible = !(Adjustibility == "N"); break;
                }
                if (gvProduct.Columns[i].HeaderText.Equals(imisgen.getMessage("L_TOTALVISITSLEFT")))
                {
                    gvProduct.Columns[i].Visible = !(Adjustibility == "N"); break;
                }
                if (gvProduct.Columns[i].HeaderText.Equals(imisgen.getMessage("L_TOTALCONSULTATIONSLEFT")))
                {
                    gvProduct.Columns[i].Visible = !(Adjustibility == "N"); break;
                }
                if (gvProduct.Columns[i].HeaderText.Equals(imisgen.getMessage("L_TOTALSURGERIESLEFT")))
                {
                    gvProduct.Columns[i].Visible = !(Adjustibility == "N"); break;
                }
                if (gvProduct.Columns[i].HeaderText.Equals(imisgen.getMessage("L_TOTALDELIVERIESLEFT")))
                {
                    gvProduct.Columns[i].Visible = !(Adjustibility == "N"); break;
                }
                if (gvProduct.Columns[i].HeaderText.Equals(imisgen.getMessage("L_TOTALANTENATALLEFT")))
                {
                    gvProduct.Columns[i].Visible = !(Adjustibility == "N"); break;
                }
                if (gvProduct.Columns[i].HeaderText.Equals(imisgen.getMessage("L_HOSPITALIZATIONAMOUNTLEFT")))
                {
                    gvProduct.Columns[i].Visible = !(Adjustibility == "N"); break;
                }
                if (gvProduct.Columns[i].HeaderText.Equals(imisgen.getMessage("L_CONSULTATIONAMOUNTLEFT")))
                {
                    gvProduct.Columns[i].Visible = !(Adjustibility == "N"); break;
                }
                if (gvProduct.Columns[i].HeaderText.Equals(imisgen.getMessage("L_SURGERYAMOUNTLEFT")))
                {
                    gvProduct.Columns[i].Visible = !(Adjustibility == "N"); break;
                }
                if (gvProduct.Columns[i].HeaderText.Equals(imisgen.getMessage("L_DELIVERYAMOUNTLEFT")))
                {
                    gvProduct.Columns[i].Visible = !(Adjustibility == "N"); break;
                }
                if (gvProduct.Columns[i].HeaderText.Equals(imisgen.getMessage("L_ANTENATALAMOUNTLEFT")))
                {
                    gvProduct.Columns[i].Visible = !(Adjustibility == "N"); break;
                }
            }

            // ItemCode
            Adjustibility = General.getControlSetting("lblItemCode");
            lblItemCode.Visible = !(Adjustibility == "N");

            // Item Left
            Adjustibility = General.getControlSetting("lblItemLeft");
            lblItemLeft.Visible = !(Adjustibility == "N");

            // Service Left
            Adjustibility = General.getControlSetting("lblServiceLeft");
            lblServiceLeft.Visible = !(Adjustibility == "N");

            // Service MidDate
            Adjustibility = General.getControlSetting("lblServiceMinDate");
            lblServiceMinDate.Visible = !(Adjustibility == "N");

            // Item MidDate
            Adjustibility = General.getControlSetting("lblItemMinDate");
            lblItemMinDate.Visible = !(Adjustibility == "N");
        }



        private void Page_Load(object sender, System.EventArgs e)
        {
            FormatForm();

            string Referer = Request.ServerVariables["HTTP_REFERER"];
            if (string.IsNullOrEmpty(Referer))
            {
                if (!(Request.QueryString["x"] == 1))
                    Server.Transfer("Redirected.htm");
            }

            Label lbl = FindControl("FooterID").FindControl("Footer").FindControl("lblMsg");
            if (lbl == null)
            {
                lbl = new Label();
                lbl.ID = "lblMsg";
                FindControl("FooterID").FindControl("Footer").Controls.Add(lbl);
            }

            if (Session["User"] == null)
                Response.Redirect("Default.aspx");
            if (!(Session["Msg"] == null))
            {
                if (Session["Msg"].ToString().Length > 0)
                {
                    if (Strings.Trim(lbl.Text).Length > 0)
                        lbl.Text = lbl.Text + Session["Msg"];
                    else
                        lbl.Text = Session["Msg"] + "<br/>";

                    Session["Msg"] = "";
                }
                else if (lbl.Text.Length == 0)
                {
                    Session["Msg"] = "";
                    lbl.Text = "";
                }
            }
            else if (lbl.Text.Length == 0)
            {
                Session["Msg"] = "";
                lbl.Text = "";
            }


            if (!(IsPostBack == true))
                Loadsecurity();

            if (ScriptManager.GetCurrent(this.Page).IsInAsyncPostBack())
            {
                string js = "<script type='text/javascript'> $(document).ready(function(){ if( typeof(BindEventsonRowAdd) == 'function' )BindEventsonRowAdd();  }); </script>";
                ScriptManager.RegisterStartupScript(btnSearch, this.GetType(), "insureePopup", js, false);
            }
            Page.Header.DataBind();

            if (IsPostBack)
                return;

            // Dim eDefaults As New IMIS_EN.tblIMISDefaults
            // If IMIS_Gen.OfflineCHF = True Or IMIS_Gen.offlineHF = True Then
            // MasterBI.GetDefaults(eDefaults)
            // End If

            if (IMIS_Gen.offlineHF == true)
            {
                if (IMIS_Gen.HFCode == null)
                {
                    IMIS_EN.tblHF eHF = new IMIS_EN.tblHF();
                    eHF.HfID = IMIS_Gen.HFID;
                    MasterBI.getHFCodeAndName(ref eHF);
                    IMIS_Gen.HFCode = eHF.HFCode;
                    IMIS_Gen.HFName = eHF.HFName;
                }
                lblinfo.Text = "OFF-LINE HF " + IMIS_Gen.HFCode + ":" + IMIS_Gen.HFName;
                FooterID.Style.Add("background", "#FFCFC4");
                lblinfo.Style.Add("color", "Red");
            }
            else if (IMIS_Gen.OfflineCHF == true)
            {
                lblinfo.Text = "OFF-LINE | Scheme: " + IMIS_Gen.CHFID; // eDefaults.OfflineCHF
                FooterID.Style.Add("background", "#DBFFB7");
                lblinfo.Style.Add("color", "Red");
            }
            else
                lblinfo.Text = "";
        }

        private void Loadsecurity()
        {

            // Dim RoleID As Integer = imisgen.getRoleId(Session("User"))
            // 'Policies and Insurees
            // SubAddFamily.Enabled = MasterBI.checkRoles(IMIS_EN.Enums.Rights.AddFamily, RoleId)
            // SubFindfamily.Enabled = MasterBI.checkRoles(IMIS_EN.Enums.Rights.FindFamily, RoleId)
            // SubFindInsuree.Enabled = MasterBI.checkRoles(IMIS_EN.Enums.Rights.FindInsuree, RoleId)
            // SubFindPolicy.Enabled = MasterBI.checkRoles(IMIS_EN.Enums.Rights.FindPolicy, RoleId)
            // SubFindPremium.Enabled = MasterBI.checkRoles(IMIS_EN.Enums.Rights.FindContribution, RoleId)
            // SubFindPayment.Enabled = MasterBI.checkRoles(IMIS_EN.Enums.Rights.FindPayment, RoleId)

            /// Claims
            // SubClaimOverview.Enabled = MasterBI.checkRoles(IMIS_EN.Enums.Rights.FindClaim, RoleId)
            // SubReview.Enabled = MasterBI.checkRoles(IMIS_EN.Enums.Rights.ClaimOverview, RoleId)
            // SubBatchRun.Enabled = MasterBI.checkRoles(IMIS_EN.Enums.Rights.ValuateClaim, RoleId) ' need to check this

            /// Administration
            // SubUser.Enabled = MasterBI.checkRoles(IMIS_EN.Enums.Rights.FindUser, RoleId)
            // subPayer.Enabled = MasterBI.checkRoles(IMIS_EN.Enums.Rights.FindPayer, RoleId)
            // subPolicyRenewal.Enabled = MasterBI.checkRoles(IMIS_EN.Enums.Rights.RenewPolicy, RoleId)
            // SubPriceList.Enabled = MasterBI.checkRoles(IMIS_EN.Enums.Rights.FindPriceListMedicalItems, RoleId)
            // SubPLMI.Enabled = MasterBI.checkRoles(IMIS_EN.Enums.Rights.FindPriceListMedicalItems, RoleId)
            // SubPLMS.Enabled = MasterBI.checkRoles(IMIS_EN.Enums.Rights.FindPriceListMedicalServices, RoleId)
            // SUBMI.Enabled = MasterBI.checkRoles(IMIS_EN.Enums.Rights.FindMedicalItem, RoleId)
            // SubMS.Enabled = MasterBI.checkRoles(IMIS_EN.Enums.Rights.FindMedicalService, RoleId)
            // SubHF.Enabled = MasterBI.checkRoles(IMIS_EN.Enums.Rights.FindHealthFacility, RoleId)
            // subOfficer.Enabled = MasterBI.checkRoles(IMIS_EN.Enums.Rights.FindOfficer, RoleId)
            // SubClaimAdministrator.Enabled = MasterBI.checkRoles(IMIS_EN.Enums.Rights.FindClaimAdministrator, RoleId)
            // SubProducts.Enabled = MasterBI.checkRoles(IMIS_EN.Enums.Rights.FindProduct, RoleId)
            // subLocation.Enabled = MasterBI.checkRoles(IMIS_EN.Enums.Rights.FindLocations, RoleId)

            // subEmailSetting.Enabled = MasterBI.checkRoles(IMIS_EN.Enums.Rights.EmailSettings, RoleId)

            /// Tools
            // UploadICD.Enabled = MasterBI.checkRoles(IMIS_EN.Enums.Rights.UploadICD, RoleId)
            // subPolicyRenewal.Enabled = MasterBI.checkRoles(IMIS_EN.Enums.Rights.RenewPolicy, RoleId)
            // subFeedbackPrompt.Enabled = MasterBI.checkRoles(IMIS_EN.Enums.Rights.FeedbackPrompt, RoleId)
            // If RoleId = 8 And (IMIS_Gen.offlineHF Or IMIS_Gen.OfflineCHF) Then
            // subIMISExtracts.Enabled = True
            // Else
            // subIMISExtracts.Enabled = MasterBI.checkRoles(IMIS_EN.Enums.Rights.IMISExtracts, RoleId)
            // End If
            // subUtilities.Enabled = MasterBI.checkRoles(IMIS_EN.Enums.Rights.Utilities, RoleId)
            // subReports.Enabled = MasterBI.checkRoles(IMIS_EN.Enums.Rights.Reports, RoleId)
            // subFunding.Enabled = MasterBI.checkRoles(Enums.Rights.AddFund, RoleId)

            int UserID = imisgen.getUserId(Session["User"]);
            SubAddFamily.Enabled = MasterBI.checkRights(IMIS_EN.Enums.Rights.AddFamily, UserID);
            SubFindfamily.Enabled = MasterBI.checkRights(IMIS_EN.Enums.Rights.Family, UserID);
            SubFindInsuree.Enabled = MasterBI.checkRights(IMIS_EN.Enums.Rights.Insuree, UserID);
            SubFindPolicy.Enabled = MasterBI.checkRights(IMIS_EN.Enums.Rights.FindPolicy, UserID);
            SubFindPremium.Enabled = MasterBI.checkRights(IMIS_EN.Enums.Rights.FindContribution, UserID);
            SubFindPayment.Enabled = MasterBI.checkRights(IMIS_EN.Enums.Rights.FindPayment, UserID);

            // 'Claims 
            SubClaimOverview.Enabled = MasterBI.checkRights(IMIS_EN.Enums.Rights.FindClaim, UserID);
            SubReview.Enabled = MasterBI.checkRights(IMIS_EN.Enums.Rights.ReviewClaim, UserID);
            SubBatchRun.Enabled = MasterBI.checkRights(IMIS_EN.Enums.Rights.Batch, UserID);

            // ' Administration
            SubUser.Enabled = MasterBI.checkRights(IMIS_EN.Enums.Rights.Users, UserID);
            SubUserProfile.Enabled = MasterBI.checkRights(IMIS_EN.Enums.Rights.userProfiles, UserID);
            subPayer.Enabled = MasterBI.checkRights(IMIS_EN.Enums.Rights.Payer, UserID);
            subPolicyRenewal.Enabled = MasterBI.checkRights(IMIS_EN.Enums.Rights.RenewPolicy, UserID);
            SubPriceList.Enabled = MasterBI.checkRights(IMIS_EN.Enums.Rights.PriceListMedicalItems, UserID);    // Not correct
            SubPLMI.Enabled = MasterBI.checkRights(IMIS_EN.Enums.Rights.PriceListMedicalItems, UserID);
            SubPLMS.Enabled = MasterBI.checkRights(IMIS_EN.Enums.Rights.PriceListMedicalServices, UserID);
            SUBMI.Enabled = MasterBI.checkRights(IMIS_EN.Enums.Rights.MedicalItem, UserID);
            SubMS.Enabled = MasterBI.checkRights(IMIS_EN.Enums.Rights.MedicalService, UserID);
            SubHF.Enabled = MasterBI.checkRights(IMIS_EN.Enums.Rights.HealthFacility, UserID);
            subOfficer.Enabled = MasterBI.checkRights(IMIS_EN.Enums.Rights.Officer, UserID);
            SubClaimAdministrator.Enabled = MasterBI.checkRights(IMIS_EN.Enums.Rights.ClaimAdministrator, UserID);
            SubProducts.Enabled = MasterBI.checkRights(IMIS_EN.Enums.Rights.Product, UserID);
            subLocation.Enabled = MasterBI.checkRights(IMIS_EN.Enums.Rights.Locations, UserID);

            subEmailSetting.Enabled = MasterBI.checkRights(IMIS_EN.Enums.Rights.EmailSettings, UserID);

            // ' Tools
            UploadICD.Enabled = MasterBI.checkRights(IMIS_EN.Enums.Rights.UploadICD, UserID);
            subPolicyRenewal.Enabled = MasterBI.checkRights(IMIS_EN.Enums.Rights.RenewPolicy, UserID);
            subFeedbackPrompt.Enabled = MasterBI.checkRights(IMIS_EN.Enums.Rights.FeedbackPrompt, UserID);
            if (UserID == 8 & (IMIS_Gen.offlineHF | IMIS_Gen.OfflineCHF))
                subIMISExtracts.Enabled = true;
            else
                subIMISExtracts.Enabled = MasterBI.checkRights(IMIS_EN.Enums.Rights.Extracts, UserID);
            subUtilities.Enabled = MasterBI.checkRights(IMIS_EN.Enums.Rights.Utilities, UserID);
            subReports.Enabled = MasterBI.checkRights(IMIS_EN.Enums.Rights.Reports, UserID);
            subFunding.Enabled = MasterBI.checkRights(Enums.Rights.AddFund, UserID);
        }
        private void BindData()
        {
            FillRepeater();
            FillPolicyGrid();
            FillProductDetails();
        }

        // Private Sub FillRepeater()
        // If Not IsNumeric(txtSearch.Text) Then Return
        // Dim dt As New DataTable
        // Dim Insuree As New IMIS_BI.InsureeBI
        // dt = Insuree.GetInsureeByCHFID(txtSearch.Text)
        // rptInsuree.DataSource = dt
        // rptInsuree.DataBind()

        // End Sub
        private void FillRepeater()
        {
            if (!Information.IsNumeric(txtSearch.Text))
                return;
            DataTable dt = new DataTable();
            IMIS_BI.InsureeBI Insuree = new IMIS_BI.InsureeBI();
            dt = Insuree.GetInsureeByCHFID(txtSearch.Text, Request.Cookies["CultureInfo"].Value == "en" ? "Education" : "AltLanguage");
            rptInsuree.DataSource = dt;
            rptInsuree.DataBind();

            HiddenField hf = (HiddenField)upDL.FindControl("hfPanelHasData");
            if (dt.Rows.Count > 0)
                hf.Value = "Yes";
            else
                hf.Value = "No";
        }
        private void FillPolicyGrid()
        {
            if (!Information.IsNumeric(txtSearch.Text))
                return;
            DataTable dt = new DataTable();
            IMIS_BI.InsureeBI Insuree = new IMIS_BI.InsureeBI();
            dt = Insuree.GetInsureeByCHFIDGrid(txtSearch.Text);
            gvPolicy.DataSource = dt;
            gvPolicy.DataBind();
        }

        private void txtSearch_TextChanged(object sender, System.EventArgs e)
        {
            if (!Information.IsNumeric(txtSearch.Text))
                return;
            IMIS_BI.MasterBI Insuree = new IMIS_BI.MasterBI();
            if (!(Insuree.CheckCHFID(txtSearch.Text) == true))
                return;
            BindData();
        }

        protected void btnSearch_Click(object sender, System.EventArgs e)
        {
            if (!Information.IsNumeric(txtSearch.Text))
                return;
            BindData();
        }
        private void FillProductDetails()
        {
            if (Information.IsNumeric(txtSearch.Text))
            {
                Dictionary<string, object> oDict = new Dictionary<string, object>();
                DataTable dt = MasterBI.GetInsureeProductDetails(oDict, txtSearch.Text, txtItemCode.Value, txtServiceCode.Value);
                lblItemCode.Visible = (txtItemCode.Value.Trim() != string.Empty);
                lblItemCodeL.Visible = (txtItemCode.Value.Trim() != string.Empty);
                lblItemLeft.Visible = (txtItemCode.Value.Trim() != string.Empty);
                lblItemLeftL.Visible = (txtItemCode.Value.Trim() != string.Empty);
                lblItemMinDate.Visible = (txtItemCode.Value.Trim() != string.Empty);
                lblItemMinDateL.Visible = (txtItemCode.Value.Trim() != string.Empty);
                imgItemIsOk.Visible = (!(oDict["IsItemOk"].ToString() == string.Empty ? false : oDict["IsItemOk"]) & txtItemCode.Value.Trim() != string.Empty);

                lblItemCode.Text = txtItemCode.Value;
                lblItemLeft.Text = oDict["ItemLeft"].ToString().Trim() == string.Empty ? "....." : oDict["ItemLeft"].ToString();
                if (oDict["MinDateItem"].ToString().Trim() != string.Empty)
                    lblItemMinDate.Text = Strings.Format((DateTime)oDict["MinDateItem"], "dd/MM/yyyy");
                else
                    lblItemMinDate.Text = ".....";

                lblServiceCode.Visible = (txtServiceCode.Value.Trim() != string.Empty);
                lblServiceCodeL.Visible = (txtServiceCode.Value.Trim() != string.Empty);
                lblServiceLeft.Visible = (txtServiceCode.Value.Trim() != string.Empty);
                lblServiceLeftL.Visible = (txtServiceCode.Value.Trim() != string.Empty);
                lblServiceMinDate.Visible = (txtServiceCode.Value.Trim() != string.Empty);
                lblServiceMinDateL.Visible = (txtServiceCode.Value.Trim() != string.Empty);
                imgServiceIsOk.Visible = (!(oDict["IsServiceOk"].ToString() == string.Empty ? false : oDict["IsServiceOk"]) & txtServiceCode.Value.Trim() != string.Empty);

                lblServiceCode.Text = txtServiceCode.Value;
                lblServiceLeft.Text = oDict["ServiceLeft"].ToString().Trim() == string.Empty ? "....." : oDict["ServiceLeft"].ToString();
                if (oDict["MinDateService"].ToString().Trim() != string.Empty)
                    lblServiceMinDate.Text = Strings.Format((DateTime)oDict["MinDateService"], "dd/MM/yyyy");
                else
                    lblServiceMinDate.Text = ".....";

                gvProduct.DataSource = dt;
                gvProduct.DataBind();

                HiddenField hf = (HiddenField)upDL.FindControl("hfPanelHasData");
            }
        }

        protected void rptInsuree_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                // Reference the Repeater Item.
                RepeaterItem item = e.Item;
                var Adjustibility = string.Empty;
                Adjustibility = General.getControlSetting("CHFID");
                // Reference the Controls.
                Label InsuranceNumber = item.FindControl("chf_ID") as Label;
                InsuranceNumber.Visible = !(Adjustibility == "N");

                // Last Name
                Adjustibility = General.getControlSetting("LastName");
                Label LastName = item.FindControl("Last_Name") as Label;
                LastName.Visible = !(Adjustibility == "N");


                // Region Of FSP
                Adjustibility = General.getControlSetting("RegionOfFSP");
                Label RegionOFSP = ((item.FindControl("regionalFSP") as Label));
                RegionOFSP.Visible = !(Adjustibility == "N");

                // Other Names
                Adjustibility = General.getControlSetting("OtherNames");
                Label OtherName = item.FindControl("other_names") as Label;
                OtherName.Visible = !(Adjustibility == "N");

                // District OF FSP
                Adjustibility = General.getControlSetting("DistrictOfFSP");
                Label DistrictOfFSP = item.FindControl("district_of_fsp") as Label;
                DistrictOfFSP.Visible = !(Adjustibility == "N");

                // Date Of Birth
                Adjustibility = General.getControlSetting("DOB");
                Label DOB = item.FindControl("dob") as Label;
                DOB.Visible = !(Adjustibility == "N");

                // Age
                Adjustibility = General.getControlSetting("Age");
                Label Age = item.FindControl("age") as Label;
                Age.Visible = !(Adjustibility == "N");

                // HF Level
                Adjustibility = General.getControlSetting("HFLevel");
                Label HFLevel = item.FindControl("hfLevel") as Label;
                HFLevel.Visible = !(Adjustibility == "N");

                // Gender
                Adjustibility = General.getControlSetting("Gender");
                Label Gender = item.FindControl("gender") as Label;
                Gender.Visible = !(Adjustibility == "N");

                // FSP
                Adjustibility = General.getControlSetting("FirstServicePoint");
                Label FSP = item.FindControl("fsp") as Label;
                FSP.Visible = !(Adjustibility == "N");
            }
        }
    }
}
