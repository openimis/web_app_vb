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
<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/IMIS.Master"  CodeBehind="Payer.aspx.vb" Inherits="IMIS.Payer" title='<%$ Resources:Resource,L_PAYER %>' %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>


<asp:Content ID="Content1" ContentPlaceHolderID="Body" Runat="Server">
   

    <script type="text/javascript" language="javascript">
//       function PreventBack() { window.history.forward(); }
//       setTimeout("PreventBack();", 0);
//       window.onunload = function() { null };
   </script>
         <div class="divBody" >  
         <asp:Panel ID="Panel2" runat="server"  ScrollBars="Auto" 
        CssClass="panel" GroupingText='<%$ Resources:Resource,G_PAYER %>'>
                    <table class="style15">
                    <tr>
                        <td class="FormLabel">
                            <asp:Label ID="L_TypeOfPayer" runat="server" Text='<%$ Resources:Resource,L_TYPE%>'></asp:Label>
                        </td>
                        <td class ="DataEntry">
                            <asp:DropDownList ID="ddlTypeOfPayer" runat="server" Width="200px">
                            </asp:DropDownList>
          
                            <asp:RequiredFieldValidator ID="RequiredFieldTypeOfPayer" runat="server" 
                                ControlToValidate="ddlTypeOfPayer" SetFocusOnError="True" 
                                Text="*" ValidationGroup="check"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                            <td class="FormLabel">
                                <asp:Label ID="L_NameOfPayer" runat="server" Text='<%$ Resources:Resource,L_NAME%>'></asp:Label>
                            </td>
                            <td class="DataEntry">
                                <asp:TextBox ID="txtNameOfPayer" runat="server" Width="200px" MaxLength="100"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldName" runat="server" 
                                    ControlToValidate="txtNameOfPayer" Text="*" 
                                    ValidationGroup="check"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                    <tr>
                        <td class="FormLabel">
                        <asp:Label ID="L_Address" runat="server" Text='<%$ Resources:Resource,L_LOCATIONADDRESS%>'></asp:Label>
                        </td>
                        <td class ="DataEntry">
                            <asp:TextBox ID="txtAddress" runat="server" TextMode="MultiLine" 
                                ValidationGroup="check" height="100px" Width="200px" MaxLength="100"></asp:TextBox>
                        <asp:RequiredFieldValidator 
                        ID="RequiredFieldOtherAddress" runat="server" 
                        ControlToValidate="txtAddress" 
                        SetFocusOnError="True" 
                        ValidationGroup="check"
                        Text='*'></asp:RequiredFieldValidator>
                            
                        </td>
                    </tr>
                    <tr>
                        <td class="FormLabel">
                        <asp:Label 
                        ID="L_REGION"
                        runat="server" 
                        Text='<%$ Resources:Resource,L_REGION%>'></asp:Label>
                        </td>
                        <td class ="DataEntry">
                            <asp:DropDownList ID="ddlRegion" runat="server" Width="200px" AutoPostBack="True">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidatorddlRegion" runat="server" ControlToValidate="ddlRegion" InitialValue="0" SetFocusOnError="True" Text="*" ValidationGroup="check"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                              
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                    </tr>
                        <tr>
                            <td class="FormLabel">
                                <asp:Label ID="L_District" runat="server" Text="<%$ Resources:Resource,L_DISTRICT%>"></asp:Label>
                            </td>
                            <td class="DataEntry">
                                <asp:DropDownList ID="ddlDistrict" runat="server" AutoPostBack="true" Width="200px">
                                </asp:DropDownList>
                              <%--  <asp:RequiredFieldValidator ID="RequiredFieldValidatorddlDistrict" runat="server" ControlToValidate="ddlDistrict" InitialValue="0" SetFocusOnError="True" Text="*" ValidationGroup="check"></asp:RequiredFieldValidator>--%>
                            </td>
                            <td></td>
                            <td>&nbsp;</td>
                        </tr>
                    <tr>
                        <td class="FormLabel">
                            <asp:Label ID="L_PHONE" runat="server" Text='<%$ Resources:Resource,L_PHONE%>'></asp:Label>
                        </td>
                        <td class ="style146">
                            <asp:TextBox ID="txtPhone" runat="server" Width="200px" MaxLength="50"></asp:TextBox>
                        </td>
                    </tr>
                     <tr>
                        <td class="FormLabel">
                            <asp:Label ID="L_FAX" runat="server" Text='<%$ Resources:Resource,L_FAX%>'></asp:Label>
                        </td>
                        <td class ="DataEntry">
                            <asp:TextBox ID="txtFax" runat="server" Width="200px" MaxLength="50"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="FormLabel">
                            <asp:Label ID="L_EMAIL" runat="server" Text='<%$ Resources:Resource,L_EMAIL%>'></asp:Label>
                        </td>
                        <td class ="DataEntry">
                            <asp:TextBox ID="txtEmail" runat="server" Width="200px" MaxLength="50"></asp:TextBox>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtEmail" ErrorMessage="*" SetFocusOnError="True" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ValidationGroup="check">*</asp:RegularExpressionValidator>
                        </td>
                        <td style="direction: ltr">

                        

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
