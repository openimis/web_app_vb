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
<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/IMIS.Master" CodeBehind="PolicyRenewal.aspx.vb" Inherits="IMIS.PolicyRenewal" 
    title='<%$ Resources:Resource,L_POLICYRENEWALS %>' %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content3" ContentPlaceHolderID="Body" runat="server">


<script type="text/javascript" language="javascript">
    $(document).ready(function() {

        if ($('.rb input').attr("Checked") == 'checked') {
            $('#tr_policy').show();
            $('#tr_sms').hide();
            $('#tr_journal').hide();
        } else {
            $('#tr_policy').hide();
            $('#tr_sms').show();
            $('#tr_journal').show();
        }

        $('.rb input').change(function() {
            switch ($(this).val()) {
                case 'rbPreview':
                    $('#tr_policy').show();
                    $('#tr_sms').hide();
                    $('#tr_journal').hide();
                    break;
                case 'rbJournal':
                    $('#tr_policy').hide();
                    $('#tr_sms').show();
                    $('#tr_journal').show();
                    break;
            }
        });
    });
</script>

<div class="divBody" >  
   <asp:Panel ID="pnlBody" runat="server"  ScrollBars="Auto" CssClass="panelBody" GroupingText='<%$ Resources:Resource,L_SELECTCRITERIA %>' >
                    <table>
                    <tr>
                        <td>
                            &nbsp;</td>
                        <td>
           
                            &nbsp;</td>
                
                        <td>
                            &nbsp;</td>
                
                    </tr>
                        <tr>
                            <td colspan="3" style="text-align:left;padding-left:160px" >
                            <asp:RadioButton ID="rbPreview" GroupName="ReportType" 
                                    runat="server" Text='<%$ Resources:Resource,B_PREVIEW %>' CssClass="rb" />
                            <asp:RadioButton ID="rbJournal" GroupName="ReportType" runat="server" 
                                    Text="Journal" CssClass="rb" />
                            </td>
                            
                            
                            
                            
                            
                        </tr>
                        <tr>
                            <td>
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                            <td width="300px">
                                &nbsp;</td>
                        </tr>
                        <tr id="tr_sms">
                            <td class="FormLabel">
                                <asp:Label ID="L_SMSStatus" runat="server" Text="SMS Status"></asp:Label>
                            </td>
                            <td class="DataEntry">
                                <asp:DropDownList ID="ddlSMSStatus" runat="server">
                                    <%--<asp:ListItem Value="-1">Select status</asp:ListItem>
                                    <asp:ListItem Value="0">Delivered</asp:ListItem>
                                    <asp:ListItem Value="1">Failed</asp:ListItem>--%>
                                </asp:DropDownList>
                            </td>
                            <td>
                                &nbsp;</td>
                        </tr >
                        <tr id="tr_policy">
                            <td class="FormLabel">
                                <asp:Label ID="L_PolicyStatus" runat="server" Text='<%$ Resources:Resource,L_POLICYSTATUS %>'></asp:Label>
                            </td>
                            <td class="DataEntry">
                                <asp:DropDownList ID="ddlPolicyStatus" runat="server">
                                    <%--<asp:ListItem Value="0">Select Status</asp:ListItem>
                                    <asp:ListItem Value="1">Idle</asp:ListItem>
                                    <asp:ListItem Value="2">Active</asp:ListItem>
                                    <asp:ListItem Value="4">Suspended</asp:ListItem>
                                    <asp:ListItem Value="8">Expired</asp:ListItem>--%>
                                </asp:DropDownList>
                            </td>
                            <td>
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td class="FormLabel">
                                <asp:Label ID="L_REGION" runat="server" Text='<%$ Resources:Resource,L_REGION %>'></asp:Label>
                            </td>
                            <td class="DataEntry">
                                <asp:UpdatePanel ID="UpRegion" runat="server">
                                    <ContentTemplate>
 <asp:DropDownList ID="ddlRegion" runat="server" AutoPostBack="True">
                                </asp:DropDownList>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                               
                            </td>
                            <td>
                                &nbsp;</td>
                        </tr>
                    
                        <tr>
                            <td class="FormLabel">
                                <asp:Label ID="L_Distict" runat="server" Text="<%$ Resources:Resource,L_DISTRICT %>"></asp:Label>
                            </td>
                            <td class="DataEntry">
                                <asp:UpdatePanel ID="UpDistrict" runat="server">
                                    <ContentTemplate>
                                         <asp:DropDownList ID="ddlDistrict" runat="server" AutoPostBack="True">
                                </asp:DropDownList>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                                                           </td>
                            <td>&nbsp;</td>
                        </tr>
                    
                        <tr>
                            <td class="FormLabel">
                                <asp:Label ID="L_Village0" runat="server" Text='<%$ Resources:Resource,L_WARD %>'></asp:Label>
                            </td>
                            <td class="DataEntry">
                                  <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                    <ContentTemplate>
                                <asp:DropDownList ID="ddlWard" runat="server" AutoPostBack="True">
                                </asp:DropDownList>
                                             </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                &nbsp;</td>
                        </tr>
                        <tr>
                        <td class="FormLabel">
                            <asp:Label ID="L_Village" runat="server" Text='<%$ Resources:Resource,L_VILLAGE %>'></asp:Label>
                        </td>
                        <td class="DataEntry">
                            <asp:UpdatePanel ID="UpVillage" runat="server">
                                <ContentTemplate>
                                       <asp:DropDownList ID="ddlVillage" runat="server" AutoPostBack="true">
                            </asp:DropDownList>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                         
                        </td>
                        <td>
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td class="FormLabel">
                            <asp:Label ID="L_Officer" runat="server" Text='<%$ Resources:Resource,L_ENROLMENTOFFICERS %>'></asp:Label>
                        </td>
                        <td class ="DataEntry">
                             <asp:UpdatePanel ID="UpOfficer" runat="server">
                                <ContentTemplate>
                                    <asp:DropDownList ID="ddlOfficer" runat="server">
                            </asp:DropDownList>
                                    </ContentTemplate>
                                    </asp:UpdatePanel>
                            

                        </td>
                        <td>
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td class="FormLabel">
                        <asp:Label 
                        ID="L_FromDate"
                        runat="server" 
                        Text='<%$ Resources:Resource,L_DATEFROM %>'></asp:Label>
                        </td>
                        <td class ="DataEntry">
                        <asp:TextBox ID="txtFromDate" runat="server" Width="120px" MaxLength="10"></asp:TextBox>


                            <asp:Button ID="Button1" runat="server" Height="20px" Width="20px" />
                            <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd/MM/yyyy" 
                                PopupButtonID="Button1" TargetControlID="txtFromDate">
                            </asp:CalendarExtender>


                        </td>
                        <td>
                            <asp:RequiredFieldValidator ID="RequiredFieldBirthDate" runat="server" 
                                ControlToValidate="txtFromDate" SetFocusOnError="True" 
                                Text='*' ValidationGroup="check" 
                                Display="Dynamic" ForeColor="Red"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" 
                                ControlToValidate="txtFromDate" 
                                ErrorMessage='<%$ Resources:Resource,M_DATEFORMAT %>' SetFocusOnError="false" 
                                ValidationExpression="^(0[1-9]|[12][0-9]|3[01])[/](0[1-9]|1[012])[/](19|20)\d\d$" 
                                ValidationGroup="check" Display="Dynamic" ForeColor="Red"></asp:RegularExpressionValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="FormLabel">
                            <asp:Label ID="L_DateTo" runat="server" Text='<%$ Resources:Resource,L_DATETO %>'></asp:Label>
                        </td>
                        <td class ="DataEntry">
                            <asp:TextBox ID="txtToDate" runat="server" Width="120px" MaxLength="10"></asp:TextBox>
                            <asp:CalendarExtender ID="txtToDate_CalendarExtender" runat="server" 
                                Format="dd/MM/yyyy" PopupButtonID="Button2" TargetControlID="txtToDate">
                            </asp:CalendarExtender>
                            <asp:Button ID="Button2" runat="server" Height="20px" Width="20px" />
                        </td>
                        <td>
                            <asp:RequiredFieldValidator ID="RequiredFieldBirthDate0" runat="server" 
                                ControlToValidate="txtToDate" SetFocusOnError="True" 
                                Text='*' ValidationGroup="check" ForeColor="Red"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server" 
                                ControlToValidate="txtToDate"  ErrorMessage='*'                            
                                SetFocusOnError="false" 
                                ValidationExpression="^(0[1-9]|[12][0-9]|3[01])[/](0[1-9]|1[012])[/](19|20)\d\d$" 
                                ValidationGroup="check" ForeColor="Red"></asp:RegularExpressionValidator>                           
                            <asp:CompareValidator ID="CompareValidator1" runat="server" 
                                ControlToCompare="txtFromDate" ControlToValidate="txtToDate" Display="Dynamic"                             
                                   ErrorMessage='*'
                                Operator="GreaterThanEqual" SetFocusOnError="True" Type="Date" 
                                ValidationGroup="check" ForeColor="Red"></asp:CompareValidator>
                        </td>
                        <td><asp:Button ID="btnSendSMS" runat="server" Text='<%$ Resources:Resource,M_SENDSMS %>' Visible="True" /></td>
                    </tr>
                    
                        
                    
                        <tr id="tr_journal">
                            <td class="FormLabel">
                                <asp:Label ID="L_DateTo0" runat="server" Text='<%$ Resources:Resource,L_JOURNALON %>'></asp:Label>
                            </td>
                            <td class="DataEntry">
                                <asp:DropDownList ID="ddlOn" runat="server">
                                    <%--<asp:ListItem Value="1">Prompt</asp:ListItem>
                                    <asp:ListItem Value="2">Expiry</asp:ListItem>--%>
                                </asp:DropDownList>
                            </td>
                            <td class="DataEntry">
                                &nbsp;</td>
                        </tr>
                    
                        
                    
                </table>            
         </asp:Panel> 
     </div> 
  <asp:Panel ID="pnlButtons" runat="server"  CssClass="panel" >
                <table width="100%" cellpadding="10 10 10 10">
                 <tr>
                        
                         <td align="left" >
                        
                               <asp:Button 
                            
                            ID="btnUpdateRenewals" 
                            runat="server" 
                            Text='<%$ Resources:Resource,B_UPDATE %>' ValidationGroup="check"
                            />
                        </td>
                        <td align="center">
                               <asp:Button 
                            
                            ID="btnPreview" 
                            runat="server" 
                            Text='<%$ Resources:Resource,B_PREVIEW %>' ValidationGroup="check"
                            />
                              
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
<asp:Content ID="Content2" ContentPlaceHolderID="Footer" Runat="Server">
    <asp:label id="lblMsg" runat="server"></asp:label>
</asp:Content>
