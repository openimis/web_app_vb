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
<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ChangePassword.aspx.vb" Inherits="IMIS.ChangePassword" MasterPageFile="~/IMIS.Master" Title='<%$ Resources:Resource, L_CHANGEPASSWORD %>' %>
<asp:Content ID="Content1" ContentPlaceHolderID="Body" Runat="Server">
  <script type="text/javascript">
      $(document).ready(function () {
         // alert(1);
         // $('#txtCurrentPassword').val('dd');
      });
  </script>
    <div class="divBody" >  
         <asp:Panel ID="pnChangePassword" runat="server"  ScrollBars="Auto" 
        CssClass="panel" GroupingText='<%$ Resources:Resource,L_CHANGEPASSWORD %>'>
                    <table class="ChangePassword">
                    <tr>
                        <td class="FormLabel">
                            <asp:Label ID="L_CURRENTPASSWORD" runat="server" Text='<%$ Resources:Resource,L_CURRENTPASSWORD%>'></asp:Label>
                        </td>
                        <td class="DataEntry">
                               <asp:TextBox ID="txtDummyPasswordHiden" runat="server" TextMode="Password" style="display:none"  MaxLength="25"></asp:TextBox>
                                <asp:TextBox ID="txtCurrentPassword" runat="server" TextMode="Password"  MaxLength="25"></asp:TextBox>
                                
                            </td>
                        <td>
                            <asp:RequiredFieldValidator ID="RequiredFieldCurrentPassword" runat="server" ControlToValidate="txtCurrentPassword" Text="*" ValidationGroup="check" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                             <td class="FormLabel">
                                <asp:Label ID="L_NEWPASSWORD" runat="server" Text='<%$ Resources:Resource,L_NEWPASSWORD %>'></asp:Label>
                            </td>
                            <td class="DataEntry">
                                <asp:TextBox ID="txtNewPassword" runat="server" TextMode="Password"  Text=""  MaxLength="25" 
                                    Width="150px" ></asp:TextBox>
                            </td>
                            <td>
                                <asp:RequiredFieldValidator ID="RequiredFieldPassword" runat="server" 
                                    ValidationGroup="check" ForeColor="Red" Display="Dynamic" ControlToValidate="txtNewPassword" Text="*"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="rePasswordStrength" runat="server" ControlToValidate="txtNewPassword"  ErrorMessage='<%$ Resources:Resource, M_WEAKPASSWORD %>' SetFocusOnError="True" ValidationExpression="^(?=.*\d)(?=.*[A-Za-z\W]).{8,}$" ValidationGroup="check" ForeColor="Red" Display="Dynamic">*</asp:RegularExpressionValidator>
                            </td>
                        </tr>
                        <tr>
                            <td class="FormLabel">
                                <asp:Label ID="L_ConfirmPassword" runat="server"  Text='<%$ Resources:Resource,L_CONFIRMPASSWORD%>'></asp:Label>
                            </td>
                            <td class="DataEntry">
                                <asp:TextBox ID="txtConfirmNewPassword" runat="server" Text="" TextMode="Password" 
                                    MaxLength="25" Width="150px"></asp:TextBox>
                            </td>
                            <td class="Validate">
                                <asp:RequiredFieldValidator ID="RequiredFieldConfirmPassoward" runat="server"    
                                    ControlToValidate="txtConfirmNewPassword" Text="*" ValidationGroup="check" ForeColor="Red" Display="Dynamic"> </asp:RequiredFieldValidator>
                                <asp:CompareValidator ID="ComparePassword" runat="server"  
                                    ControlToCompare="txtNewPassword" ControlToValidate="txtConfirmNewPassword" ValidationGroup="check"
                                    Text='<%$ Resources:Resource,V_CONFIRMPASSWORD%>' ForeColor="Red" Display="Dynamic"></asp:CompareValidator>
                            </td>
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
<asp:Content ID="Content2" runat="server" contentplaceholderid="head">

    <style type="text/css">
        .style146
        {
            width: 627px;
            height: 30px;
        }
    </style>


</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="Footer" Runat="Server">
    <asp:Label text="" runat="server" ID="lblMsg"> </asp:Label>
    <asp:ValidationSummary ID="validationSummary" runat="server" HeaderText='<%$ Resources:Resource,V_SUMMARY%>' ValidationGroup="check" />
</asp:Content>
