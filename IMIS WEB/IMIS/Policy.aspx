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
<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/IMIS.Master" CodeBehind="Policy.aspx.vb" Inherits="IMIS.Policy" Title= '<%$ Resources:Resource,L_POLICY %>'  %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="contentHead" ContentPlaceHolderID="head" runat="server" >
    <script type="text/javascript" language="javascript">
    function promptPolicyAdd(btn) {
        if (btn === "ok") {
            $("#<%=hfCheckMaxInsureeCount.ClientID %>").val(0);
            
            $("#<%=B_SAVE.ClientID %>").click();
        } else {
           var familyId = '<%=HttpContext.Current.Request.QueryString("f") %>';
            window.location = "OverviewFamily.aspx?f=" + familyId;
        }
    }  
    function promptPolicyRenewal(btn) {
        if (btn === "ok") {
            $("#<%=hfIsRenewalLate.ClientID %>").val(0);
            
            $("#<%=B_SAVE.ClientID %>").click();
        } else {
            var familyId = '<%=HttpContext.Current.Request.QueryString("f") %>';
            window.location = "OverviewFamily.aspx?f=" + familyId;
        }
    }
    
    
    
</script>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="Body" Runat="Server">
   
<asp:HiddenField ID="hfCheckMaxInsureeCount" Value="1" runat="server" />
<asp:HiddenField ID="hfIsRenewalLate" Value="1" runat="server" />
<asp:HiddenField ID="hfDistrictID" Value="0" runat="server" />
 <asp:HiddenField ID="hfLocationId" Value="0" runat="server" />
    <asp:HiddenField ID="hfRegionId" Value="0" runat="server" />
    <div class="divBody" >  
    <asp:Panel ID="L_FAMILYPANEL" runat="server" height="130px" ScrollBars="Auto" 
             CssClass="panel" 
         GroupingText='<%$ Resources:Resource,L_FAMILYPANEL %>'  >
           
           <table class="style15">
                    <tr>
                       
                         <td class="FormLabel">
                            <asp:Label 
                                ID="lblHeadCHFID"
                                runat="server" 
                                Text='<%$ Resources:Resource,L_CHFID %>'>
                            </asp:Label>
                        </td>
                        <td class="ReadOnlyText">
                            <asp:Label ID="txtHeadCHFID" runat="server"  />
                        </td>
                         <td class="FormLabel">
                             <asp:Label ID="L_REGION" runat="server" Text="<%$ Resources:Resource,L_REGION %>"></asp:Label>
                        </td>
                        <td class="ReadOnlyText">
                            <asp:Label ID="txtRegion" runat="server" />
                        </td>
                         <td class="FormLabel">
                             <asp:Label ID="L_CONFIRMATIONNO0" runat="server" Text="<%$ Resources:Resource, L_CONFIRMATIONTYPE %>"></asp:Label>
                         </td>
                         <td class="ReadOnlyText">
                             <asp:Label ID="txtConfirmationType" runat="server" />
                         </td>
                    </tr>
                    
                    <tr>
                      
                         <td class="FormLabel">
                            <asp:Label 
                                ID="lblHeadLastName"
                                runat="server" 
                                Text='<%$ Resources:Resource,L_LASTNAME %>'>
                            </asp:Label>
                        </td>
                        <td class="ReadOnlyText">
                        <asp:label ID="txtHeadLastName" runat="server"  />
                        </td>
                            <td class="FormLabel">
                                <asp:Label ID="L_DISTRICT" runat="server" Text="<%$ Resources:Resource,L_DISTRICT %>"></asp:Label>
                        </td>
                        <td class="ReadOnlyText">
                            <asp:Label ID="txtDistrict" runat="server" />
                        </td>
                      
                         <td class="FormLabel">
                             <asp:Label ID="L_CONFIRMATIONNO" runat="server" Text="<%$ Resources:Resource, L_CONFIRMATIONNO %>"></asp:Label>
                         </td>
                         <td class="ReadOnlyText" style="vertical-align:top;padding-top:5px;">
                             <asp:Label ID="txtConfirmationNo1" runat="server" style="direction: ltr" />
                         </td>
                      
                    </tr>
                    <tr>
                        
                     
                         <td class="FormLabel">
                            <asp:Label 
                                ID="lblHeadOtherNames"
                                runat="server" 
                                Text='<%$ Resources:Resource,L_OTHERNAMES %>'>
                            </asp:Label>
                        </td>
                        <td class="ReadOnlyText">
                            <asp:Label ID="txtHeadOtherNames" runat="server" width="150px" />
                        </td>
                          <td class="FormLabel">
                              <asp:Label ID="L_WARD" runat="server" Text="<%$ Resources:Resource,L_WARD %>"></asp:Label>
                        </td>
                        <td class="ReadOnlyText">
                            <asp:Label ID="txtWard" runat="server"></asp:Label>
                        </td>
                       
                         <td class="FormLabel">
                             <asp:Label ID="L_ADDRESS0" runat="server" Text="<%$ Resources:Resource, L_PARMANENTADDRESS %>"></asp:Label>
                         </td>
                       
                         <td class="ReadOnlyText" rowspan="2" style="vertical-align:top;padding-top:5px;">
                             <asp:Label ID="txtPermanentAddress" runat="server"></asp:Label>
                         </td>
                       
                    </tr>
                    <tr>
                       
                        <td class="FormLabel">
                            <asp:Label ID="lblPoverty" runat="server" Text="<%$ Resources:Resource,L_POVERTY %>">
                            </asp:Label>
                        </td>
                        <td class="ReadOnlyText">
                            <asp:Label ID="txtPoverty" runat="server"></asp:Label>
                        </td>
                         <td class="FormLabel">
                             <asp:Label ID="L_VILLAGE" runat="server" Text="<%$ Resources:Resource,L_VILLAGE %>"></asp:Label>
                        </td>
                        <td class="ReadOnlyText">
                            <asp:Label ID="txtVillage" runat="server"></asp:Label>
                        </td>
                        <td class="ReadOnlyText">&nbsp;</td>
                    </tr>
                </table>     
                    
                    
         </asp:Panel>
   <asp:Panel ID="pnlBody" runat="server"  ScrollBars="Auto" CssClass="panel" GroupingText='<%$ Resources:Resource,L_POLICYDETAILS %>'>
                    <asp:UpdatePanel ID="upCHFID" runat="server">
                                <ContentTemplate>
                                     <asp:HiddenField ID="hfInsurancePeriod" runat="server" />
                    <table class="style15">
                    <tr>
                        <td class="FormLabel">
                        <asp:Label ID="lblEnrolmentDate" runat="server" Text='<%$ Resources:Resource,L_ENROLMENTDATE %>'></asp:Label>
                        </td>
                        <td class ="DataEntry">
                        <asp:TextBox ID="txtEnrollmentDate" runat="server" Width="132px" Height="16px" 
                                AutoPostBack="True"></asp:TextBox>                                      
                        <asp:Button ID="btnEnrollmentDate" runat="server" Height="20px" Width="20px"/>
                        <asp:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtEnrollmentDate" PopupButtonID="btnEnrollmentDate" Format="dd/MM/yyyy"></asp:CalendarExtender>
                        <asp:MaskedEditExtender ID="txtEnrolDate_MaskedEditExtender" runat="server" 
                            CultureDateFormat="dd/MM/YYYY"                             
                            TargetControlID="txtEnrollmentDate" Mask="99/99/9999" MaskType="Date" 
                            UserDateFormat="DayMonthYear">
                        </asp:MaskedEditExtender>
                       <asp:RegularExpressionValidator ID="RegularExpressionValidatortxtDob" runat="server" 
                    ControlToValidate="txtEnrollmentDate" SetFocusOnError="True" Text="*"  
                    ValidationExpression="^(0[1-9]|[12][0-9]|3[01])[/](0[1-9]|1[012])[/](19|20)\d\d$" 
                    ValidationGroup="check"></asp:RegularExpressionValidator>
                    <asp:RequiredFieldValidator ID="RequiredfieldValidator1" runat="Server" ControlToValidate="txtEnrollmentDate" SetFocusOnError="true" ForeColor="Red" Display="Dynamic"
                            Text='*' ></asp:RequiredFieldValidator>
                        </td>                        
                       <td style="width:210px"> &nbsp;</td>
                           <td class="FormLabel" style="text-align:left">
                            <asp:Label ID="lblPolicyStatus" runat="server" Text='<%$ Resources:Resource,L_POLICYSTATUS %>'></asp:Label>
                           </td>
                       
                    </tr>
                    <tr>
                        <td class="FormLabel">
                            <asp:Label ID="lblProduct" runat="server" Text='<%$ Resources:Resource,L_PRODUCT %>'></asp:Label>
                           </td>
                        <td class ="DataEntry">
                              <%--<asp:UpdatePanel ID="upCHFID" runat="server">
                                <ContentTemplate>--%>
                                     <asp:DropDownList ID="ddlProduct" runat="server" Width="150px" AutoPostBack="True"></asp:DropDownList>
                                     <asp:RequiredFieldValidator ID="RequiredfieldValidator3" runat="Server" ControlToValidate="ddlProduct" SetFocusOnError="true" Text="*" InitialValue="0" ValidationGroup="check"  ForeColor="Red" Display="Dynamic" ></asp:RequiredFieldValidator>     
                               <%--</ContentTemplate>
                           </asp:UpdatePanel>--%>
                                                  
                        <asp:RequiredFieldValidator 
                        ID="RequiredFieldValidatorPackage" runat="server" 
                        ControlToValidate="ddlProduct" 
                        SetFocusOnError="True" 
                        ForeColor="Red" Display="Dynamic"
                        Text='*'></asp:RequiredFieldValidator>
                        </td>
                        <td>
                        &nbsp;
                        </td>
                        <td>
                        &nbsp;
                        </td>
                    </tr>
                    <tr>
                     <td class="FormLabel">
                        <asp:Label ID="lblEffectiveDate" runat="server" Text='<%$ Resources:Resource,L_EFFECTIVEDATE %>'></asp:Label>
                        </td>
                        <td class ="DataEntry">
                        <asp:TextBox ID="txtEffectiveDate" runat="server" Width="150px" Height="16px"></asp:TextBox>                                      
                        <asp:CalendarExtender ID="CalendarExtender4" runat="server" TargetControlID="txtEffectiveDate" PopupButtonID="btnEffectiveDate" Format="dd/MM/yyyy"></asp:CalendarExtender>
                        <asp:MaskedEditExtender ID="MaskedEditExtender1" runat="server" 
                            CultureDateFormat="dd/MM/YYYY"                             
                            TargetControlID="txtEffectiveDate" Mask="99/99/9999" MaskType="Date" 
                            UserDateFormat="DayMonthYear">
                        </asp:MaskedEditExtender>
                       <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" 
                           ControlToValidate="txtEffectiveDate" SetFocusOnError="True" 
                           ValidationExpression="^(0[1-9]|[12][0-9]|3[01])[/](0[1-9]|1[012])[/](19|20)\d\d$" 
                           ForeColor="Red" Display="Dynamic"
                           Text='*'></asp:RegularExpressionValidator>
                        </td>
                        <td>
                        &nbsp;
                        </td>
                        <td class ="DataEntry">
                            <asp:Textbox ID="txtPolicyStatus" runat="server" Width="80px" ReadOnly="True" Enabled="false" ></asp:Textbox>
                        </td>
                    </tr>
                     
                    <tr>
                        <td class="FormLabel">
                        <asp:Label ID="lblStartDate" runat="server" Text='<%$ Resources:Resource,L_STARTDATE %>'></asp:Label>
                        </td>
                        <td class ="DataEntry">
                         
                        <asp:TextBox ID="txtStartDate" AutoPostBack="true" runat="server" Width="150px" ></asp:TextBox>
                          <asp:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtStartDate" PopupButtonID="btnStartDate" Format="dd/MM/yyyy"></asp:CalendarExtender>
                       <asp:MaskedEditExtender ID="MaskedEditExtender2" runat="server" 
                            CultureDateFormat="dd/MM/YYYY"                             
                            TargetControlID="txtStartDate" Mask="99/99/9999" MaskType="Date" 
                            UserDateFormat="DayMonthYear">
                        </asp:MaskedEditExtender>
                       <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" 
                            ControlToValidate="txtStartDate" SetFocusOnError="True" 
                            ValidationExpression="^(0[1-9]|[12][0-9]|3[01])[/](0[1-9]|1[012])[/](19|20)\d\d$" 
                            ForeColor="Red" Display="Dynamic"
                            Text='*'></asp:RegularExpressionValidator>
                    <asp:RequiredFieldValidator ID="RequiredfieldValidator2" runat="Server" ControlToValidate="txtStartDate" SetFocusOnError="true" Text="*" ValidationGroup="check" ForeColor="Red" Display="Dynamic" ></asp:RequiredFieldValidator>
                        </td>
                         <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td class="FormLabel">
                        <asp:Label ID="lblExpiryDate" runat="server" Text='<%$ Resources:Resource,L_EXPIRYDATE %>'></asp:Label>
                        </td>
                        <td class ="DataEntry">
                        <asp:TextBox ID="txtExpiryDate" runat="server" Width="150px" ></asp:TextBox>
                          <asp:CalendarExtender ID="CalendarExtender3" runat="server" TargetControlID="txtExpiryDate" PopupButtonID="btnExpiryDate" Format="dd/MM/yyyy"></asp:CalendarExtender>
                       <asp:MaskedEditExtender ID="MaskedEditExtender3" runat="server" 
                            CultureDateFormat="dd/MM/YYYY"                             
                            TargetControlID="txtExpiryDate" Mask="99/99/9999" MaskType="Date" 
                            UserDateFormat="DayMonthYear">
                        </asp:MaskedEditExtender>
                       <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" 
                            ControlToValidate="txtExpiryDate"  SetFocusOnError="True" 
                            ValidationExpression="^(0[1-9]|[12][0-9]|3[01])[/](0[1-9]|1[012])[/](19|20)\d\d$" 
                            ForeColor="Red" Display="Dynamic"
                            Text='*'></asp:RegularExpressionValidator>
                        </td>
                         <td>
                            &nbsp;
                        </td>
                    </tr>
                    
                    <tr>
                    <td class="FormLabel">
                            <asp:Label ID="lblEnrolementOfficer" runat="server" Text='<%$ Resources:Resource,L_ENROLMENTOFFICERS %>'></asp:Label>
                           </td>
                        <td class ="DataEntry">
                            <asp:DropDownList ID="ddlEnrolementOfficer" Width="150px"  runat="server"></asp:DropDownList>
                        <asp:RequiredFieldValidator 
                        ID="RequiredFieldValidatorRegistrationAuthority" runat="server" 
                        ControlToValidate="ddlEnrolementOfficer" 
                        SetFocusOnError="True" 
                        ForeColor="Red" Display="Dynamic"
                        Text='*'></asp:RequiredFieldValidator>
                        <asp:RequiredFieldValidator ID="RequiredfieldValidator4" runat="Server" ControlToValidate="ddlEnrolementOfficer" SetFocusOnError="true" Text="*" InitialValue="0" ValidationGroup="check" ForeColor="Red" Display="Dynamic" ></asp:RequiredFieldValidator>
                        </td>
                        <td>
                        &nbsp;
                        </td>
                    
                    </tr>
                    <tr>
                    <td> &nbsp; </td>    
                    </tr>
                    </table>
                    </ContentTemplate>
                           </asp:UpdatePanel>
                    <table>
                   <tr>                   
                   <td>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>
                   <asp:Panel ID="PlnBody3" runat="server" ScrollBars="Auto" CssClass="panel" GroupingText="" >
                     <table>
                    <tr>
                    <td> &nbsp;</td>
                    
                    <td class="FormLabel" style="text-align:left">
                            <asp:Label ID="lblPolicyValue" runat="server" Text='<%$ Resources:Resource,L_POLICYVALUE %>'></asp:Label>
                           </td>
                        
                    <td class="FormLabel" style="text-align:left">
                            <asp:Label ID="lblPremiumPaid" runat="server" Text='<%$ Resources:Resource,L_PREMIUMPAID %>'></asp:Label>
                           </td>
                           
                    <td class="FormLabel" style="text-align:left">
                            <asp:Label ID="lblBalance" runat="server" Text='<%$ Resources:Resource,L_BALANCE%>'></asp:Label>
                           </td>
                        
                    </tr>
                    <tr>
                     <td class="FormLabel">
                                <asp:Label ID="Label1" runat="server" Text='<%$ Resources:Resource,L_REMUNERATEDHEALTHCARE %>' Visible="false"></asp:Label></td>
                   
                    <td class ="DataEntry">
                    
                            <asp:Textbox ID="txtPolicyValue" runat="server" Width="80px"  ReadOnly="True" Enabled="false" style="text-align:right; padding-right:1px;"></asp:Textbox>
                        </td>
                                           
                    <td class ="DataEntry">
                            <asp:Textbox ID="txtPremiumPaid" runat="server" Width="80px" ReadOnly="True" Enabled="false" style="text-align:right; padding-right:1px"></asp:Textbox>
                        </td>
                                          
                    <td class ="DataEntry">
                            <asp:Textbox ID="txtBalance" runat="server" Width="80px" ReadOnly="True" Enabled="false" style="text-align:right; padding-right:1px"></asp:Textbox>
                        </td>
                    </tr>
                    </table>
                    </asp:Panel>
                    </ContentTemplate>
                    </asp:UpdatePanel>
                    </td></tr>
                    </table>
                    <table>
                    <tr>                    
                    <td>
                     <asp:Panel ID="PnlBody4" runat="server"  ScrollBars="Auto" CssClass="panel" GroupingText="" >    
                        <table>
                        <tr>
                        <td> &nbsp;</td>                           
                            <td class="FormLabel" style="text-align:left">
                            <asp:Label ID="lblGeneral" runat="server" Text='<%$ Resources:Resource,L_GENERAL %>'></asp:Label>
                            </td>
                            <td class="FormLabel" style="text-align:left">
                            <asp:Label ID="lblInpatient" runat="server" Text='<%$ Resources:Resource,L_INPATIENT %>'></asp:Label>
                            </td>
                             <td class="FormLabel" style="text-align:left">
                             <asp:Label ID="lblOutpatient" runat="server" Text='<%$ Resources:Resource,L_OUTPATIENT %>'></asp:Label>
                             </td>
                             </tr>
                             <tr>
                            <td class="FormLabel">
                                <asp:Label ID="lblDeductable" runat="server" Text='<%$ Resources:Resource,L_DEDUCTABLE %>'></asp:Label></td>                                
                               
                            <td class="DataEntry">
                                <asp:TextBox ID="txtPaidDeductable" runat="server" MaxLength="18" 
                                    Text-align="right" Width="80px" style="text-align:right; padding-right:1px" 
                                    class="numbersOnly" ReadOnly="True" Enabled="false"></asp:TextBox>
                             <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToValidate="txtPaidDeductable" SetFocusOnError="true" Operator="DataTypeCheck" Type="Double" ForeColor="Red" Display="Dynamic"
                                    Text='*'></asp:CompareValidator>
                             </td>
                             <td class="DataEntry">
                                <asp:TextBox ID="txtPaidDeductableIP" runat="server" MaxLength="18" 
                                     Text-align="right" Width="80px" style="text-align:right; padding-right:1px" 
                                     class="numbersOnly" ReadOnly="True" Enabled="false"></asp:TextBox>
                                 <asp:CompareValidator ID="CompareValidator2" runat="server" ControlToValidate="txtPaidDeductableIP" SetFocusOnError="true" Operator="DataTypeCheck" Type="Double" ForeColor="Red" Display="Dynamic"
                                        Text='*'></asp:CompareValidator>
                                </td>
                                <td class="DataEntry">
                                <asp:TextBox ID="txtPaidDeductableOP" runat="server" MaxLength="18" 
                                        Text-align="right" Width="80px" style="text-align:right; padding-right:1px" 
                                        class="numbersOnly" ReadOnly="True" Enabled="false"></asp:TextBox>
                                 <asp:CompareValidator ID="CompareValidator3" runat="server" ControlToValidate="txtPaidDeductableOP" SetFocusOnError="true" Operator="DataTypeCheck" Type="Double" ForeColor="Red" Display="Dynamic"
                                        Text='*'></asp:CompareValidator>
                                </td>    
                                </tr>
                                <tr>
                            <td class="FormLabel">
                                <asp:Label ID="lblRemuneratedHealthCare" runat="server" Text='<%$ Resources:Resource,L_REMUNERATEDHEALTHCARE %>'></asp:Label></td>
                               
                            <td class="DataEntry">
                                <asp:TextBox ID="txtRemuneratedHealthCare" runat="server" MaxLength="18" 
                                    Text-align="right" Width="80px" style="text-align:right; padding-right:1px" 
                                    class="numbersOnly" ReadOnly="True" Enabled="false"></asp:TextBox>
                                  <asp:CompareValidator ID="CompareValidator4" runat="server" ControlToValidate="txtRemuneratedHealthCare" SetFocusOnError="true" Operator="DataTypeCheck" Type="Double" ForeColor="Red" Display="Dynamic"
                                        Text='*'></asp:CompareValidator>                                
                            </td> 
                            <td class="DataEntry">
                                <asp:TextBox ID="txtRemuneratedIP" runat="server" MaxLength="18" 
                                    text-align="right" Width="80px" style="text-align:right; padding-right:1px" 
                                    class="numbersOnly" ReadOnly="True" Enabled="false"></asp:TextBox>
                             <asp:CompareValidator ID="CompareValidator5" runat="server" ControlToValidate="txtRemuneratedIP" SetFocusOnError="true" Type="Double" Operator="DataTypeCheck" ForeColor="Red" Display="Dynamic"
                                        Text='*'></asp:CompareValidator>
                            </td>
                            <td class="DataEntry">
                            <asp:TextBox ID="txtRemuneratedOP" runat="server" MaxLength="18" Text-align="right" 
                                    Width="80px" style="text-align:right; padding-right:1px" class="numbersOnly" 
                                    ReadOnly="True" Enabled="false"></asp:TextBox>                                   
                               <asp:CompareValidator ID="CompareValidator6" runat="server" ControlToValidate="txtRemuneratedOP" SetFocusOnError="true" Type="Double" Operator="DataTypeCheck" ForeColor="Red" Display="Dynamic"
                                        Text='*'></asp:CompareValidator>
                              </td>
                              </tr>
                            </table>
                    </asp:Panel> 
                    </td></tr> 
                    </table>
                    <table> 
                                <tr>
                            
                            <td>
                                &nbsp;</td></tr><tr>
                        <td>
                        <asp:HiddenField ID="hfFamilyID" runat="server"/>
                            <asp:HiddenField ID="hfPolicyStage" runat="server" />
                        </td>
                        </tr>                   
                   
                </table>            
         </asp:Panel>  
      </div>
   <asp:Panel ID="pnlButtons" runat="server"  CssClass="panelbuttons" >
                <table width="100%" cellpadding="10 10 10 10">
                 <tr>
                        
                         <td align="left" >
                        
                               <asp:Button 
                            
                            ID="B_SAVE" 
                            runat="server" 
                            Text='<%$ Resources:Resource,B_SAVE%>'
                            ValidationGroup="check"  />
                        </td>
                        
                        
                        <td  align="right">
                       <asp:Button 
                            
                            ID="B_CANCEL" 
                            runat="server" 
                            Text='<%$ Resources:Resource,B_CANCEL%>'
                              />
                        </td>
                        
                    </tr>
                </table>             
         </asp:Panel>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Footer" Runat="Server">
    <asp:Label text="" runat="server" ID="lblMsg"> </asp:Label>
    <asp:ValidationSummary ID="validationSummary" runat="server" HeaderText='<%$ Resources:Resource,V_SUMMARY%>' ValidationGroup="check" />
</asp:Content>
