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
<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/IMIS.Master" CodeBehind="FeedbackPrompt.aspx.vb" Inherits="IMIS.FeedbackPrompt" 
    title='<%$ Resources:Resource,L_FEEDBACKPROMPT %>' %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content3" ContentPlaceHolderID="Body" runat="server">




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
                            <td>
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                            <td width="300px">
                                &nbsp;</td>
                        </tr>
                        <tr id="tr_sms">
                            <td class="FormLabel">
                                <asp:Label ID="L_SMSStatus" runat="server" Text='<%$ Resources:Resource,L_SMSSTATUS %>'></asp:Label>
                            </td>
                            <td class="DataEntry">
                                <asp:DropDownList ID="ddlSMSStatus" runat="server">
                                    <%--<asp:ListItem Value="-1">Select status</asp:ListItem>
                                    <asp:ListItem Value="0">Received</asp:ListItem>
                                    <asp:ListItem Value="1">Failed</asp:ListItem>--%>
                                </asp:DropDownList>
                            </td>
                            <td>
                                &nbsp;</td>
                        </tr >
                        <tr>
                            <td class="FormLabel">
                                <asp:Label ID="L_REGION" runat="server" Text='<%$ Resources:Resource,L_REGION %>'></asp:Label>
                            </td>
                            <td class="DataEntry">
                                <asp:UpdatePanel runat="server" ID="UpRegion">
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
                                <asp:UpdatePanel runat="server" ID="UpDistrict">
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
                                <asp:UpdatePanel runat="server" ID="UpWard">
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
                                <asp:UpdatePanel runat="server" ID="UpVillage">
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
                        <td class="DataEntry">
                            <asp:UpdatePanel runat="server" ID="UpOfficer">
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


                            <asp:Button ID="Button1" runat="server" class="dateButton" />
                            <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd/MM/yyyy" 
                                PopupButtonID="Button1" TargetControlID="txtFromDate">
                            </asp:CalendarExtender>


                        </td>
                        <td>
                            <asp:RequiredFieldValidator ID="RequiredFieldBirthDate" runat="server" 
                                ControlToValidate="txtFromDate" SetFocusOnError="True" 
                                Text='<%$ Resources:Resource,M_SELECTFRMDATE %>' ValidationGroup="check" 
                                Display="Dynamic"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" 
                                ControlToValidate="txtFromDate" 
                                ErrorMessage='<%$ Resources:Resource,M_DATEFORMAT %>' SetFocusOnError="false" 
                                ValidationExpression="^(0[1-9]|[12][0-9]|3[01])[/](0[1-9]|1[012])[/](19|20)\d\d$" 
                                ValidationGroup="check" Display="Dynamic"></asp:RegularExpressionValidator>
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
                            <asp:Button ID="Button2" runat="server"  class="dateButton" />
                        </td>
                        <td>
                            <asp:RequiredFieldValidator ID="RequiredFieldBirthDate0" runat="server" 
                                ControlToValidate="txtToDate" SetFocusOnError="True" 
                                Text='<%$ Resources:Resource,M_SELECTTODATE %>' ValidationGroup="check"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server" 
                                ControlToValidate="txtToDate" ErrorMessage='<%$ Resources:Resource,M_DATEFORMAT %>' 
                                SetFocusOnError="false" 
                                ValidationExpression="^(0[1-9]|[12][0-9]|3[01])[/](0[1-9]|1[012])[/](19|20)\d\d$" 
                                ValidationGroup="check"></asp:RegularExpressionValidator>
                            <br />
                            <asp:CompareValidator ID="CompareValidator1" runat="server" 
                                ControlToCompare="txtFromDate" ControlToValidate="txtToDate" Display="Dynamic" 
                                ErrorMessage='<%$ Resources:Resource,M_TODATEGREATER %>' 
                                Operator="GreaterThanEqual" SetFocusOnError="True" Type="Date" 
                                ValidationGroup="check"></asp:CompareValidator>
                        </td>
                        <td><asp:Button ID="btnSendSMS" runat="server" Text="Send SMS" Visible="True" /></td>
                    </tr>
                    
                        
                    
                </table>            
         </asp:Panel> 
     </div> 
  <asp:Panel ID="pnlButtons" runat="server"  CssClass="panel" >
                <table width="100%" cellpadding="10 10 10 10">
                 <tr>
                        
                         <td align="left" >
                        
                             <asp:Button 
                            
                            ID="btnPreview" 
                            runat="server" 
                            Text='<%$ Resources:Resource,B_PREVIEW %>' ValidationGroup="check"
                            />
                        </td>
                        <td align="center">
                               
                              
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
