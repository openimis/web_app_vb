<%-- Copyright (c) 2016-2017 Swiss Agency for Development and Cooperation (SDC)

The program users must agree to the following terms:

Copyright notices
This program is free software: you can redistribute it and/or modify it under the terms of the GNU AGPL v3 License as published by the 
Free Software Foundation, version 3 of the License.
This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of 
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU AGPL v3 License for more details www.gnu.org.

Disclaimer of Warranty
There is no warranty for the program, to the extent permitted by applicable law; except when otherwise stated in writing the copyright 
holders and/or other parties provide the program "as is" without warranty of any kind, either expressed or implied, including, but not 
limited to, the implied warranties of merchantability and fitness for a particular purpose. The entire risk as to the quality and 
performance of the program is with you. Should the program prove defective, you assume the cost of all necessary servicing, repair or correction.

Limitation of Liability 
In no event unless required by applicable law or agreed to in writing will any copyright holder, or any other party who modifies and/or 
conveys the program as permitted above, be liable to you for damages, including any general, special, incidental or consequential damages 
arising out of the use or inability to use the program (including but not limited to loss of data or data being rendered inaccurate or losses 
sustained by you or third parties or a failure of the program to operate with any other programs), even if such holder or other party has been 
advised of the possibility of such damages.

In case of dispute arising out or in relation to the use of the program, it is subject to the public law of Switzerland. The place of jurisdiction is Berne.--%>
<%@ Page Language="vb" AutoEventWireup="false"  MasterPageFile="~/IMIS.Master" CodeBehind="ClaimFeedback.aspx.vb" Inherits="IMIS.ClaimFeedback" 
title='<%$ Resources:Resource,L_FEEDBACKPAGETITLE%>' %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="ajax" %>


<asp:Content ID="Content1" ContentPlaceHolderID="Body" Runat="Server">

    <script type="text/javascript" language="javascript" >
    $(document).ready(function() {

        var $EnrolID = $("#<%=ddlEnrolmentOfficer.ClientID %>")
        var $CareRed = $('#<%=ddlCareRendered.ClientID %>');
        var $Pay = $('#<%=ddlPaymentAsked.ClientID %>');
        var $DPre = $('#<%=ddlDrugsPrescribed.ClientID %>');
        var $DRec = $('#<%=ddlDrugsReceived.ClientID %>');
        var $FedD = $('#<%=txtFeedbackDate.ClientID %>');
        var Initial = false;
        $("#<%=rbOverallAsessmentLevels.ClientID %>").find("input[type=radio]").each(function() {
            $(this).data("OVA", $(this).is(":checked"));
            
        });

        $("#<%=ddlEnrolmentOfficer.ClientID %>").data("EnrolOf", $("#<%=ddlEnrolmentOfficer.ClientID %>").val());
        $("#<%=ddlCareRendered.ClientID %>").data("CaRe", $("#<%=ddlCareRendered.ClientID %>").val());
        $("#<%=ddlPaymentAsked.ClientID %>").data("Pa", $("#<%=ddlPaymentAsked.ClientID %>").val());
        $("#<%=ddlDrugsPrescribed.ClientID %>").data("DgPre", $("#<%=ddlDrugsPrescribed.ClientID %>").val());
        $("#<%=ddlDrugsReceived.ClientID %>").data("DrRe", $("#<%=ddlDrugsReceived.ClientID %>").val());
        $("#<%=txtFeedbackDate.ClientID %>").data("FdD", $("#<%=txtFeedbackDate.ClientID %>").val());

        $('.fb').change(function() {
            $("#<%=lblMsg.ClientID %>").html("");
        });

        $("#<%=B_SAVE.ClientID %>").click(function() {
            var flagSAve = true;

            $("#<%=rbOverallAsessmentLevels.ClientID %>").find("input[type=radio]").each(function() {
            if ($(this).data("OVA") != $(this).is(":checked")) {
                    Initial = true;
                }
            });

            if ($EnrolID.val() == $EnrolID.data("EnrolOf") && $CareRed.val() == $CareRed.data("CaRe") && $Pay.val() == $Pay.data("Pa") && $DPre.val() == $DPre.data("DgPre") && $DRec.val() == $DRec.data("DrRe") && Initial == false && $FedD.val() == $FedD.data("FdD")) {
                $("#<%=lblMsg.ClientID %>").html('<%=imisgen.getMessage("M_VALIDATEFEEDBACKSAVE", True)%>');
                flagSAve = false;
            } else {
                $("#<%=lblMsg.ClientID %>").html("");
            }
            return flagSAve;
        });
    });
       
</script>
    <div class="divBody" >
        <table class="catlabel">
            <tr>
                <td >
                   <asp:label  
                           ID="L_CLAIM"  
                           runat="server" 
                           Text='<%$ Resources:Resource,L_CLAIM %>'>
                   </asp:label>   
                    
                </td>
            </tr>
        </table>
        <asp:Panel ID="pnlTop" runat="server"  CssClass="panelTop" GroupingText='<%$ Resources:Resource,L_CLAIMDETAILS %>'>
           <table>
            <tr>
                <td>
                   <table>
            <tr>
             <td class="auto-style1">
                     <asp:Label ID="lblClaimID" runat="server" Text='<%$ Resources:Resource,L_CLAIMCODE%>'></asp:Label>
                </td>
                <td class ="DataEntry">
                    <asp:Label ID="lblClaimIDData" runat="server" 
                        Text=""></asp:Label>
                </td>
              <%-- <td class="FormLabel">
                   <asp:Label ID="lblBatchCode" runat="server" Text='<%$ Resources:Resource,L_BATCHCODE%>'></asp:Label>
                </td>
                <td class ="DataEntry">
                    <asp:Label ID="lblBatchCodeData" runat="server" 
                        Text=""></asp:Label>
                </td>--%>
                <td class="auto-style1">
                     <asp:Label ID="lblCHFID" runat="server" Text='<%$ Resources:Resource,L_CHFID%>'></asp:Label>
                </td>
                <td class ="DataEntry">
                    <asp:Label ID="lblCHFIDData" runat="server" 
                        Text=""></asp:Label>
                </td>
               <%-- <td class="FormLabel">
                     <asp:Label ID="lblClaimDate" runat="server" Text='<%$ Resources:Resource,L_CLAIMDATELABEL%>'></asp:Label>
                </td>
                <td class ="DataEntry">
                    <asp:Label ID="lblClaimDateData" runat="server" 
                        Text=""></asp:Label>
                </td>--%>
                 <td class="auto-style1">
                   <asp:Label ID="lblName" runat="server" Text='<%$ Resources:Resource,L_LASTNAME%>'></asp:Label>
                </td>
                <td class ="DataEntry">
                    <asp:Label ID="lblNameData" runat="server" 
                        Text=""></asp:Label>
                </td>
                 
                <td class="auto-style1">
                    <asp:Label ID="lblOtherNames" runat="server" Text="<%$ Resources:Resource,L_OTHERNAMES%>"></asp:Label>
                </td>
                 
                <td class="DataEntry">
                    <asp:Label ID="lblOtherNamesData" runat="server" Text=""></asp:Label>
                </td>
                 
            </tr>
            <tr>
                <td class ="FormLabel">
                     <asp:Label ID="lblHFCode" runat="server" Text='<%$ Resources:Resource,L_HFCODE%>'></asp:Label>                    
                </td>
                <td class ="DataEntry">
                    <asp:Label ID="lblHFCodeData" runat="server" 
                        Text=""></asp:Label>
                 </td>
                <td class="FormLabel">
                    <asp:Label ID="lblHFName" runat="server" Text="<%$ Resources:Resource,L_HFNAME %>"></asp:Label>
                </td>
                 <td class ="DataEntry">
                     <asp:Label ID="lblHFNameData" runat="server" Text=""></asp:Label>
                </td>
                 <td class ="FormLabel">
                     <asp:Label ID="lblStartDate" runat="server" Text="<%$ Resources:Resource,L_START %>"></asp:Label>
                 </td>
                 <td class="DataEntry">
                      <asp:Label ID="lblStartDateData" runat="server" Text=""></asp:Label>
                 </td>
               
                <td class="FormLabel">
                    <asp:Label ID="lblClaimStatus" runat="server" Text="<%$ Resources:Resource,L_CLAIMSTATUSLABEL%>"></asp:Label>
                </td>
               
                <td class="DataEntry">
                    <asp:Label ID="lblClaimStatusData" runat="server" Text=""></asp:Label>
                </td>
               
           </tr>
            <tr>
            
                 <td class="FormLabel">
                     <asp:Label ID="lblClaimDate" runat="server" Text='<%$ Resources:Resource,L_CLAIMDATELABEL%>'></asp:Label>
                </td>
                <td class ="DataEntry">
                    <asp:Label ID="lblClaimDateData" runat="server" 
                        Text=""></asp:Label>
                </td>
                
                <td class="FormLabel">
                     <asp:Label ID="lblEndDate" runat="server" Text='<%$ Resources:Resource,L_END%>'></asp:Label>
                </td>
                <td class ="DataEntry">
                    <asp:Label ID="lblEndDateData" runat="server" 
                        Text=""></asp:Label>
                </td>
                 <td class ="FormLabel">
                    <asp:Label ID="lblReviewStatus" runat="server" 
                        Text='<%$ Resources:Resource,L_REVIEWSTATUS%>'></asp:Label>
                </td>
                <td class="DataEntry">
                    <asp:Label ID="lblReviewStatusData" runat="server" 
                        Text=""></asp:Label>
                 </td>
                 
                 <td class="FormLabel">
                     <asp:Label ID="lblFeedbackStatus" runat="server" Text="<%$ Resources:Resource,L_FEEDBACKSTATUS%>"></asp:Label>
                 </td>
                 
                 <td class="DataEntry">
                     <asp:Label ID="lblFeedbackStatusData" runat="server" Text=""></asp:Label>
                 </td>
                 
             </tr>
            <tr>
            
               <%-- <td class="FormLabel">
                     <asp:Label ID="lblClaimID" runat="server" Text='<%$ Resources:Resource,L_CLAIMCODE%>'></asp:Label>
                </td>
                <td class ="DataEntry">
                    <asp:Label ID="lblClaimIDData" runat="server" 
                        Text=""></asp:Label>
                </td>--%>
   
                <td class="FormLabel">
                   <asp:Label ID="lblClaimAdminCode" runat="server" 
                        Text='<%$ Resources:Resource,L_CLAIMADMIN%>'></asp:Label>
               </td>
               <td class ="DataEntryWide" colspan="5" >
                  <asp:Textbox ID="txtClaimAdminCode" runat="server" Enabled="false" width="100px" 
                       BorderStyle="None" ></asp:Textbox> 
               </td>                   
                <td class="DataEntryWide">&nbsp;</td>
                <td class="DataEntryWide">&nbsp;</td>
                <td class="FormLabel">
                     <asp:HiddenField ID="hfBatchId" runat="server" />
                </td>
                <td class ="DataEntry">
                    <asp:HiddenField ID="hdnFeedbackID" runat="server" />
                </td>
                <td class="FormLabel">
                      &nbsp;</td>
                  <td class="DataEntry">
                      &nbsp;</td>
             </tr> 
           
            
</table>
                </td>
               <%--  <td>
                    <br />
                     <br />
                      <br />
                      <br />
                   <asp:Button class="button" ID="btnSearch" runat="server" 
                          Text='<%$ Resources:Resource,B_SEARCH %>' >
                  </asp:Button>
                  <br />
                  
                </td>--%>
            </tr>
        </table>
        </asp:Panel>
              
        <table class="catlabel">
             <tr>
                <td >
                       <asp:label  
                           ID="L_FEEDBACK"  
                           runat="server" 
                           Text='<%$ Resources:Resource,L_FEEDBACK %>'></asp:label>   
               </td>
               
                </tr>
            
            </table>
        <asp:Panel ID="pnlBody" runat="server"  CssClass="panelBody" >
            <table>
                <tr>
                <td class="FormLabel">
                       <asp:Label ID="lblEnrolmentOfficer" runat="server" Text='<%$ Resources:Resource,L_ENROLMENTOFFICERS %>'></asp:Label>
                   </td>
                   <td class="DataEntry">
                       <asp:DropDownList ID="ddlEnrolmentOfficer" runat="server" Width="300px" CssClass="fb"></asp:DropDownList>
                       </td>
                       <td>
                       <asp:RequiredFieldValidator ID="RequiredEnrolmentOfficer" runat="server" SetFocusOnError="true" Text="*" 
                   ValidationGroup="check" InitialValue="0" ControlToValidate="ddlEnrolmentOfficer"></asp:RequiredFieldValidator>
                   </td>
                   </tr>
                   <tr>
                   <td class="FormLabel">
                       <asp:Label ID="lblCareRendered" runat="server" Text='<%$ Resources:Resource,L_CARERENDERED %>'></asp:Label>
                   </td>
                   <td class="DataEntry">
                       <asp:DropDownList ID="ddlCareRendered" runat="server" CssClass="fb"></asp:DropDownList>
                               </td>
                       <td>
                           <asp:RequiredFieldValidator ID="RequiredCareRendered" runat="server" SetFocusOnError="true" Text="*" 
                   ValidationGroup="check" ControlToValidate="ddlCareRendered"></asp:RequiredFieldValidator>
                       </td>
                </tr>
                
                <tr>
                   <td  class="FormLabel">
                       <asp:Label ID="lblPaymentAsked" runat="server" Text='<%$ Resources:Resource,L_PAYMENTASKED %>'></asp:Label>
                   </td>
                   <td class="DataEntry">
                       <asp:DropDownList ID="ddlPaymentAsked" runat="server" CssClass="fb"></asp:DropDownList>
                                         </td>
                    <td>
                         <asp:RequiredFieldValidator ID="RequiredPaymentAsked" runat="server" SetFocusOnError="true" Text="*" 
                   ValidationGroup="check" ControlToValidate="ddlPaymentAsked"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                   <td class="FormLabel">
                       <asp:Label ID="lblDrugsPrescribed" runat="server" Text='<%$ Resources:Resource,L_DRUGSPRESCRIBED %>'></asp:Label>
                   </td>
                   <td class="DataEntry">
                       <asp:DropDownList ID="ddlDrugsPrescribed" runat="server" CssClass="fb"></asp:DropDownList>
                                         </td>
                    <td>
                         <asp:RequiredFieldValidator ID="RequiredDrugsPrescribed" runat="server" SetFocusOnError="true" Text="*" 
                   ValidationGroup="check" ControlToValidate="ddlDrugsPrescribed"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                
                <tr>
                   <td  class="FormLabel">
                       <asp:Label ID="lblDrugsReceived" runat="server" Text='<%$ Resources:Resource,L_DRUGSRECEIVED %>'></asp:Label>
                   </td>
                   <td  class="DataEntry">
                       <asp:DropDownList ID="ddlDrugsReceived" runat="server" CssClass="fb"></asp:DropDownList>
                                          </td>
                    <td>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" SetFocusOnError="true" Text="*" 
                   ValidationGroup="check" ControlToValidate="ddlDrugsReceived"></asp:RequiredFieldValidator>
                    </td>
                  
                </tr>
                <tr>
                   <td  class="FormLabel">
                       <asp:Label ID="lblOverallAsessment" runat="server" Text='<%$ Resources:Resource,L_OVERALLASESSMENT %>'></asp:Label>
                   </td>
                   <td class="DataEntry">
                       <table cellpadding="0px" cellspacing="0px">
                         
                          <tr>
                          <td class="DataEntry">
                            <asp:RadioButtonList ID="rbOverallAsessmentLevels" runat="server" RepeatDirection="Horizontal" ValidationGroup="check" CssClass="FormLabel fb">
                            <asp:ListItem Text="0" Value="0"></asp:ListItem>
                            <asp:ListItem Text="1" Value="1"></asp:ListItem>
                            <asp:ListItem Text="2" Value="2"></asp:ListItem>
                            <asp:ListItem Text="3" Value="3"></asp:ListItem>
                            <asp:ListItem Text="4" Value="4"></asp:ListItem>
                            <asp:ListItem Text="5" Value="5"></asp:ListItem>
                            </asp:RadioButtonList>
                            
                          </td>
                          <td>
                          <asp:RequiredFieldValidator ID="RequiredOverallAsessmentLevels" runat="server" Display="Dynamic" Text="*" SetFocusOnError="true"  
                                 ValidationGroup="check"  ControlToValidate="rbOverallAsessmentLevels"></asp:RequiredFieldValidator>
                          </td>
                             
                          </tr>                                    
                       </table>
                   </td>
                </tr>
                <tr>
                   <td  class="FormLabel">
                       <asp:Label ID="lblFeedbackDate" runat="server" Text='<%$ Resources:Resource,L_FEEDBACKDATE %>'></asp:Label>
                   </td>
                   <td  class="DataEntry">
                       <asp:TextBox ID="txtFeedbackDate" runat="server" Width="120px" CssClass="fb"> </asp:TextBox>
                        <asp:Button ID="btnFeedbackDate" runat="server" Class="dateButton" />
                        
                        <asp:RequiredFieldValidator ID="RequiredFeedbackDate" runat="server" SetFocusOnError="true" Text="*" 
                                 ValidationGroup="check" ControlToValidate="txtFeedbackDate"></asp:RequiredFieldValidator>
                   
                      <ajax:CalendarExtender ID="txtFeedbackDate_CalendarExtender" runat="server" 
                        Format="dd/MM/yyyy" PopupButtonID="btnFeedbackDate" TargetControlID="txtFeedbackDate">
                       </ajax:CalendarExtender>
                        <ajax:MaskedEditExtender ID="txtFeedbackDate_MaskedEditExtender" runat="server" 
                            CultureDateFormat="dd/MM/YYYY"                             
                            TargetControlID="txtFeedbackDate" Mask="99/99/9999" MaskType="Date" 
                            UserDateFormat="DayMonthYear">
                      </ajax:MaskedEditExtender>
                   </td>
                </tr>
            
            </table>
        </asp:Panel>
          <asp:HiddenField ID="hfClaimAdminId" runat="server" />
        </div>
       <asp:Panel ID="pnlButtons" runat="server"   CssClass="panelbuttons" >
        <table width="100%" cellpadding="10 10 10 10" align="center">
             <tr align="center">
                    
                    <td  align="left">
                       <asp:Button 
                        ID="B_SAVE" 
                        runat="server" 
                        Text='<%$ Resources:Resource,B_SAVE%>' ValidationGroup="check"
                          />
                    </td>
                   
                    <td align="right">
                          <asp:Button 
                            
                            ID="B_CANCEL" 
                            runat="server" 
                            Text='<%$ Resources:Resource,B_CANCEL%>' />
                              
                    </td>                 
                </tr>
            </table>             
        </asp:Panel> 
   
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Footer" Runat="Server" Visible="true" >
    <asp:Label text="" runat="server" ID="lblMsg"> </asp:Label>
</asp:Content>
<asp:Content ID="Content3" runat="server" contentplaceholderid="head">
    <style type="text/css">
        .auto-style1
        {
            height: 27px;
            width: 150px;
            text-align: right;
            color: Blue;
            font-weight: normal;
            font-family: Verdana, Arial, Helvetica, sans-serif;
            font-size: 11px;
            padding-right: 1px;
        }
    </style>
</asp:Content>
