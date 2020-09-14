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
<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/IMIS.Master" CodeBehind="ClaimAdministrator.aspx.vb" Inherits="IMIS.ClaimAdministrator" 
    title='<%$ Resources:Resource,L_CLAIMADMINISTRATORS %>' %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Body" Runat="Server">
    <script type="text/ecmascript" language="javascript">
        $(document).ready(function () {
            var chk = document.getElementById('<%= chkIncludeLogin.ClientID %>');
            toggleUserInfoForm(chk.checked);
            if (chk.checked) {
                userInfoValidation(true);
            } else {
                userInfoValidation(false);
            }
        });


        function fireCheckChanged() { 
            var chk = document.getElementById('<%= chkIncludeLogin.ClientID %>');
            toggleUserInfoForm(chk.checked);
            if (chk.checked) {
                userInfoValidation(true);
            } else {
                if (hasNumber(document.getElementById('<%= hfUserID.ClientID %>').value)) {
                    ShowdeletePopUp()
                }
                else {
                    userInfoValidation(false);
                }
            }         
        };

        function hasNumber(myString) {
            return /\d/.test(myString);
        }
       
        function ShowdeletePopUp() {
            popup.acceptBTN_Text = '<%=ImisGen.getMessage("L_YES", True)%>';
            popup.rejectBTN_Text = '<%=ImisGen.getMessage("L_NO", True)%>';
            popup.confirm('<%=ImisGen.getMessage("M_DELETEUSERLOGIN", True)%>', DoPostBack);
            return false;
        }

        function DoPostBack(btn, args) {
            if (btn == "ok") {
                userInfoValidation(false);
                __doPostBack('', 'Delete')
            } else {
                var chk = document.getElementById('<%= chkIncludeLogin.ClientID %>');
                chk.checked = true;
                toggleUserInfoForm(true);
            }
        } 

        function userInfoValidation(value) {
            ValidatorEnable(document.getElementById("<%=RequiredFieldLanguage.ClientID %>"), value);
            var shouldChangePassword = value;
            if (document.getElementById('<%= hfUserID.ClientID %>').value != "0" ) {
                shouldChangePassword = false;
            }
            ValidatorEnable(document.getElementById("<%=RequiredFieldPassword.ClientID %>"), shouldChangePassword);
            ValidatorEnable(document.getElementById("<%=RequiredFieldConfirmPassword.ClientID %>"), shouldChangePassword);
        }

        function toggleUserInfoForm(visible) {
            var Password = document.getElementById("LoginInfo");
            Password.style.display = visible ? "block" : "none";
        }
    </script>
    <asp:HiddenField ID="hfUserID" runat="server" />
         <div class="divBody" >  
         <asp:Panel ID="pnlDetails" runat="server"  ScrollBars="Auto" 
        CssClass="panel" GroupingText='<%$ Resources:Resource,G_CLAIMADMINISTRATOR %>'>
                    <table class="style15">
                    <tr>
                        <td class="FormLabel">
                            <asp:Label ID="lblCode" runat="server" Text='<%$ Resources:Resource,L_CODE %>'></asp:Label>
                        </td>
                        <td class ="DataEntry">
                            <asp:TextBox ID="txtCode" runat="server" Width="150px" MaxLength="8"></asp:TextBox>
                        <asp:RequiredFieldValidator 
                        ID="RequiredFieldCode" runat="server" 
                        ControlToValidate="txtCode" 
                        SetFocusOnError="False"
                        ValidationGroup="check"   ForeColor="Red" Display="Dynamic"
                        Text='*'></asp:RequiredFieldValidator>
                        </td>
                        <td>
                        </td>
                    </tr>
                        
                        <tr>
                        <td class="FormLabel">
                        <asp:Label 
                        ID="lblOtherNames"
                        runat="server" 
                        Text='<%$ Resources:Resource,L_OTHERNAMES %>'></asp:Label>
                        </td>
                        <td class ="DataEntry">
                        <asp:TextBox ID="txtOtherNames" runat="server" Width="150px" MaxLength="100"></asp:TextBox>                                      
                        <asp:RequiredFieldValidator 
                        ID="RequiredFieldOtherNames" runat="server" 
                        ControlToValidate="txtOtherNames" 
                        SetFocusOnError="True" 
                        ValidationGroup="check" ForeColor="Red" Display="Dynamic"
                        Text='*'></asp:RequiredFieldValidator>
                        </td>
                        <td>
                        </td>
                    </tr>
                        <tr>
                            <td class="FormLabel">
                                <asp:Label ID="lblLastName" runat="server" Text='<%$ Resources:Resource,L_LASTNAME %>'></asp:Label>
                            </td>
                            <td class="DataEntry">
                                <asp:TextBox ID="txtLastName" runat="server" Width="150px" MaxLength="100"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldLastName" runat="server" 
                                    ControlToValidate="txtLastName" Text="*" ForeColor="Red" Display="Dynamic"
                                    ValidationGroup="check" SetFocusOnError="True" ></asp:RequiredFieldValidator>
                            </td>
                            <td>
                            </td>
                        </tr>
                    <tr>
                        <td class="FormLabel">
                        <asp:Label ID="lblDOB"
                        runat="server" 
                        Text='<%$ Resources:Resource,L_BIRTHDATE %>'></asp:Label>
                        </td>
                        <td class ="DataEntry">
                        <asp:TextBox ID="txtDOB" runat="server" Width="130px"></asp:TextBox>
                        <asp:MaskedEditExtender ID="txtDob_MaskedEditExtender" runat="server" 
                            CultureDateFormat="dd/MM/YYYY"                             
                            TargetControlID="txtDob" Mask="99/99/9999" MaskType="Date" 
                            UserDateFormat="DayMonthYear">
                        </asp:MaskedEditExtender>
                            <asp:Button ID="btnDOB_Extender" runat="server" Height="20px" Width="20px"  />


                          <asp:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtDOB" PopupButtonID="btnDOB_Extender" Format="dd/MM/yyyy"></asp:CalendarExtender>
                          <asp:RegularExpressionValidator ID="RegularExpressionValidatortxtDob" runat="server" 
                            ControlToValidate="txtDOB" Text="*" SetFocusOnError="True" 
                            ValidationExpression="^(0[1-9]|[12][0-9]|3[01])[/](0[1-9]|1[012])[/](19|20)\d\d$" 
                            ValidationGroup="check" ForeColor="Red" Display="Dynamic"></asp:RegularExpressionValidator>
                        </td>
                        <td style="direction: ltr">
                        </td>
                    </tr>
                    <tr>
                        <td class="FormLabel">
                            <asp:Label ID="lblPhone" runat="server" Text='<%$ Resources:Resource,L_PHONE %>'></asp:Label>
                        </td>
                        <td class ="DataEntry">
                            <asp:TextBox ID="txtPhone" runat="server" Width="150px" MaxLength="50"></asp:TextBox>
                        </td>
                        <td class="auto-style2"></td>
                     </tr>
                        <tr>
                            <td class="FormLabel">
                                <asp:Label ID="L_Email" runat="server" style="direction: ltr" Text="<%$ Resources:Resource,L_EMAIL %>"></asp:Label>
                            </td>
                            <td class="DataEntry">
                                <asp:TextBox ID="txtEmail" runat="server" MaxLength="200" Width="150px"></asp:TextBox>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" 
                                    ControlToValidate="txtEmail" ErrorMessage="<%$ Resources:Resource,L_INVALIDEMAIL %>" 
                                    SetFocusOnError="True" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ValidationGroup="check"
                                    ForeColor="Red" Display="Dynamic"></asp:RegularExpressionValidator>
                            </td>
                            <td style="direction: ltr">
                            </td>
                        </tr>
                     <tr>
                        <td class="FormLabel">
                           <asp:Label ID="lblHFCode" runat="server" Text='<%$ Resources:Resource,L_HF%>'></asp:Label>
                        </td>
                        <td class ="DataEntry">
                            <asp:DropDownList ID="ddlHFCode" runat="server" Width="150px" ></asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldHFCode" runat="server" 
                                ControlToValidate="ddlHFCode" Text="*" InitialValue="0"
                                ValidationGroup="check" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                        </td>
                     </tr>                                         
                </table>            
         </asp:Panel> 
         <br />
         <br />
               <asp:Panel ID="pnlClaimAdmiLogin" runat="server" Height="200px"
               GroupingText='<%$ Resources:Resource,G_CLAIMADMINISTRATORLOGIN %>'
               style="border:1px solid Gray;margin:3px 3px 1px 3px ;padding: 1px 5px 2px 5px">
               <asp:CheckBox ID="chkIncludeLogin" runat="server" OnCheckedChanged="chkIncludeLogin_CheckedChanged" onclick="fireCheckChanged()" 
                   Text='<%$ Resources:Resource, L_INCLUDELOGIN %>' Font-Size="9pt" 
                   ForeColor="Blue" Style="direction: ltr;padding:50px"   />              
                    <table class="style15" style="display:none" id="LoginInfo">
                          <tr>
                                <td class="FormLabel">
                            <asp:Label ID="lblLanguage" runat="server"
                            Text="<%$ Resources:Resource,L_LANGUAGE %>" ViewStateMode="Disabled"></asp:Label>
                        </td>
                         <td class ="DataEntry">
                            <asp:DropDownList ID="ddlLanguage" runat="server" Width="150px" ViewStateMode="Enabled" ></asp:DropDownList>
                               <asp:RequiredFieldValidator 
                        ID="RequiredFieldLanguage" runat="server" 
                        ControlToValidate="ddlLanguage" InitialValue="-1"
                        SetFocusOnError="True"  ForeColor="Red"
                        ValidationGroup="check" Display="Dynamic"
                        Text='*'></asp:RequiredFieldValidator>
                        </td>
                        <td>
                        </td>
                    </tr>
                        <tr>         
                            <td class="FormLabel">
                                <asp:Label ID="lblPassword" runat="server" Text="<%$ Resources:Resource,L_PASSWORD %>" ViewStateMode="Disabled"></asp:Label>
                            </td>
                        <td class ="DataEntry">
                        <asp:TextBox ID="txtPassword" runat="server" MaxLength="100" Width="150px" TextMode="Password"></asp:TextBox>                     
                            <asp:RequiredFieldValidator ID="RequiredFieldPassword" runat="server" ControlToValidate="txtPassword" ErrorMessage='<%$ Resources:Resource, M_WEAKPASSWORD %>' SetFocusOnError="True" ForeColor="Red" Display="Dynamic" ValidationExpression="^(?=.*\d)(?=.*[A-Za-z\W]).{8,}$" ValidationGroup="check">*
                            </asp:RequiredFieldValidator>
                        </td>
                    </tr>
                        <tr>          
                            <td class="FormLabel">
                                <asp:Label ID="lblConfirmPassword" runat="server" Text="<%$ Resources:Resource,L_CONFIRMPASSWORD %>" ></asp:Label>
                            </td>
                            <td class="DataEntry">
                                <asp:TextBox ID="txtConfirmPassword" runat="server" MaxLength="100" Width="150px" TextMode="Password" ValidationGroup="check" ></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldConfirmPassword" runat="server" 
                                ControlToValidate="txtConfirmPassword" Text="*" ForeColor="Red" ControlToCompare="txtPassword"
                                ValidationGroup="check">
                            </asp:RequiredFieldValidator>                
                                <asp:CompareValidator ID="ComparePassword" runat="server"
                                        ControlToCompare="txtPassword" ControlToValidate="txtConfirmPassword" ValidationGroup="check"
                                        Operator="Equal" SetFocusOnError="True" ForeColor="Red" Display="Dynamic" 
                                        Text='<%$ Resources:Resource,V_CONFIRMPASSWORD%>'> 
                                </asp:CompareValidator>
                            </td>  
                        </tr>
                    <tr>
                        <td >
                        <asp:MaskedEditExtender ID="MaskedEditExtender1" runat="server" 
                            CultureDateFormat="dd/MM/YYYY"                             
                            TargetControlID="txtDob" Mask="99/99/9999" MaskType="Date" 
                            UserDateFormat="DayMonthYear">
                        </asp:MaskedEditExtender>
                        <asp:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtDOB" PopupButtonID="btnDOB_Extender" Format="dd/MM/yyyy"></asp:CalendarExtender>
                        </td>
                        <td style="direction: ltr">
                            &nbsp;
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
                                   CausesValidation ="true"
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
<asp:Content ID="Content2" ContentPlaceHolderID="Footer" Runat="Server">
    <asp:label id="lblmsg" runat="server"></asp:label>
    <asp:ValidationSummary ID="validationSummary1" runat="server" HeaderText='<%$ Resources:Resource,V_SUMMARY%>' ValidationGroup="check" style="padding-left:15px;"/>
</asp:Content>
