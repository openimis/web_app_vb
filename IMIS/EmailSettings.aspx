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
<%@ Page Title='<%$ Resources:Resource,L_EMAILSETTING%>' Language="vb" AutoEventWireup="false" MasterPageFile="~/IMIS.Master" CodeBehind="EmailSettings.aspx.vb" Inherits="IMIS.EmailSettings" %>
<asp:Content ID="Content3" ContentPlaceHolderID="Body" runat="server">
    <div class="divBody" >  
         <asp:Panel ID="Panel2" runat="server"  ScrollBars="Auto" 
        CssClass="panel" GroupingText='<%$ Resources:Resource,L_EMAILSETTING %>'>
                    <table class="style15">
                  
                    <tr>
                            <td class="FormLabel">
                            <asp:Label ID="L_EMAIL" runat="server" Text='<%$ Resources:Resource,L_EMAIL%>'></asp:Label>
                        </td>
                        <td class ="DataEntry">
                            <asp:TextBox ID="txtEmail" runat="server" Width="175px" MaxLength="200"></asp:TextBox>
                        </td>
                            <td class="DataEntry">
                                <asp:RequiredFieldValidator ID="rf1" runat="server" ControlToValidate="txtEmail" SetFocusOnError="True" Text="*" ValidationGroup="check"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtEmail" ErrorMessage="*" SetFocusOnError="True" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ValidationGroup="check">*</asp:RegularExpressionValidator>
                            </td>
                        </tr>
                        <tr>
                        
                            <td class="FormLabel">
                                <asp:Label ID="L_Password" runat="server" Text="<%$ Resources:Resource,L_PASSWORD%>"></asp:Label>
                            </td>
                            <td class="DataEntry"><%-- <asp:RequiredFieldValidator ID="RequiredFieldValidatorddlDistrict" runat="server" 
                                ControlToValidate="ddlDistrict" SetFocusOnError="True" 
                                Text="*" ValidationGroup="check" InitialValue="0"></asp:RequiredFieldValidator>--%>
                                <asp:TextBox ID="txtPassword" runat="server" MaxLength="50" Width="175px" TextMode="Password"></asp:TextBox>
                            </td>
                            <td class="DataEntry">
                                <asp:RequiredFieldValidator ID="rf2" runat="server" ControlToValidate="txtPassword" SetFocusOnError="True" Text="*" ValidationGroup="check"></asp:RequiredFieldValidator>
                            </td>
                            <td>&nbsp;</td>
                        
                    </tr>
                    <tr>
                        <td class="FormLabel">
                            <asp:Label ID="L_SMTPHOST" runat="server" Text='<%$ Resources:Resource,L_SMTPHOST%>'></asp:Label>
                        </td>
                        <td class ="style146">
                            <asp:TextBox ID="txtSMTPHost" runat="server" Width="175px" MaxLength="50"></asp:TextBox>
                        </td>
                        <td class="style146">
                            <asp:RequiredFieldValidator ID="rf3" runat="server" ControlToValidate="txtSMTPHost" SetFocusOnError="True" Text="*" ValidationGroup="check"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                     <tr>
                        <td class="FormLabel">
                            <asp:Label ID="L_Port" runat="server" Text='<%$ Resources:Resource,L_PORT%>'></asp:Label>
                        </td>
                        <td class ="DataEntry">
                            <asp:TextBox ID="txtPort" runat="server" Width="175px" MaxLength="8" class="numbersOnly"></asp:TextBox>
                        </td>
                         <td class="DataEntry">
                             <asp:RequiredFieldValidator ID="rf4" runat="server" ControlToValidate="txtPort" SetFocusOnError="True" Text="*" ValidationGroup="check"></asp:RequiredFieldValidator>
                         </td>
                    </tr>
                      <tr>
                        <td class="FormLabel">
                            <asp:Label ID="Label1" runat="server" Text='<%$ Resources:Resource,L_ENABLESSL%>'></asp:Label>
                        </td>
                        <td>
  <asp:CheckBox ID="chkEnableSSL" class="checkbox" Text='' runat="server" />
                       
                             </td>
                          
                          <td>&nbsp;</td>
                          
                          </tr>
                                       
                </table>            
         </asp:Panel> 
         </div>
         <asp:Panel ID="pnlButtons" runat="server"   CssClass="panelbuttons" >
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
<asp:Content ID="Content4" ContentPlaceHolderID="Footer" runat="server">
    <asp:Label text="" runat="server" ID="lblMsg"> </asp:Label>
    <asp:ValidationSummary ID="validationSummary" runat="server" HeaderText='<%$ Resources:Resource,V_SUMMARY%>' ValidationGroup="check" />
</asp:Content>

