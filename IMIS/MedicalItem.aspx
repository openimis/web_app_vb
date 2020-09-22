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

<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/IMIS.Master" CodeBehind="MedicalItem.aspx.vb" Inherits="IMIS.MedicalItem"
    Title='<%$ Resources:Resource,L_MEDICALITEMS%>' %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Body" runat="Server">



    <script type="text/javascript">
        var rbnDrug = null;
        var rbnMedicalConsumable = null;
        var rbnBxOutPatient = null;
        var rbnBxInPatient = null;
        var rbnBxBoth = null;
        var chkBxMan = null;
        var chkBxWoman = null;
        var chkBxAdult = null;
        var chkBxChild = null;


        function validateForm() {
            TypeSelected();
            CareTypeSelected();
            PatientSelected();

        }


        function TypeSelected() {

            rbnDrug = document.getElementById('<%=Me.rbDrug.ClientID%>');
            rbnMedicalConsumable = document.getElementById('<%=Me.rbMedicalConsumable.ClientID%>');;

            if (rbnDrug.checked == false && rbnMedicalConsumable.checked == false) {
                errMsgType.innerHTML = "<asp:Literal runat='Server' Text='<%$ Resources:Resource,V_SELECTTYPE%>' />";
                return false;
            }
            else {
                errMsgType.innerHTML = ""
                return true;
            }
        }


        function CareTypeSelected() {
            rbnOutPatient = document.getElementById('<%=Me.rbOutPatient.ClientID%>');
            rbInPatient = document.getElementById('<%=Me.rbInPatient.ClientID%>');
            rbnBoth = document.getElementById('<%=Me.rbBoth.ClientID%>');


            if (rbnOutPatient.checked == false && rbInPatient.checked == false && rbnBoth.checked == false) {
                errMsgCareType.innerHTML = "<asp:Literal runat='Server' Text='<%$ Resources:Resource,V_SELECTCARETYPE%>' />";
                return false;
            }
            else {
                errMsgCareType.innerHTML = "";
                return true;
            }
        }
        function PatientSelected() {

            chkBxMan = document.getElementById('<%=Me.chkMan.ClientID%>');
            chkBxWoman = document.getElementById('<%=Me.chkWoman.ClientID%>');
            chkBxAdult = document.getElementById('<%=Me.chkAdult.ClientID%>');
            chkBxChild = document.getElementById('<%=Me.chkChild.ClientID%>');


            if (chkBxMan.checked == false && chkBxWoman.checked == false && chkBxAdult.checked == false && chkBxChild.checked == false) {

                errMsgPatient.innerHTML = "<asp:Literal runat='Server' Text='<%$ Resources:Resource,V_SELECTPATIENT%>' />";
                return false;
            }
            else {
                errMsgPatient.innerHTML = "";
                return true;
            }
        }
    </script>

    <div class="divBody">
        <asp:Panel ID="Panel2" runat="server" ScrollBars="Auto"
            CssClass="panel" GroupingText='<%$ Resources:Resource,G_MEDICALITEM %>'>
            <table class="style15">
                <tr>
                    <td class="FormLabel">
                        <asp:Label ID="L_Code" runat="server" Text='<%$ Resources:Resource,L_Code %>'></asp:Label>
                    </td>
                    <td class="auto-style5">
                        <asp:TextBox ID="txtCode" runat="server" Width="150px" MaxLength="6"></asp:TextBox>
                        <asp:RequiredFieldValidator
                            ID="RequiredFieldCode" runat="server"
                            ControlToValidate="txtCode"
                            SetFocusOnError="True"
                            ValidationGroup="check" ForeColor="Red" Display="Dynamic"
                            Text="*">
                        </asp:RequiredFieldValidator>
                    </td>
                    <td></td>
                </tr>
                <tr>
                    <td class="FormLabel">
                        <asp:Label ID="L_Name" runat="server" Text='<%$ Resources:Resource,L_NAME %>'></asp:Label>
                    </td>
                    <td class="auto-style5">
                        <asp:TextBox ID="txtName" runat="server" MaxLength="100"></asp:TextBox>
                        <asp:RequiredFieldValidator
                            ID="RequiredFieldValidatorName" runat="server"
                            ControlToValidate="txtName"
                            SetFocusOnError="True"
                            ValidationGroup="check" ForeColor="Red" Display="Dynamic"
                            Text="*">
                        </asp:RequiredFieldValidator>
                    </td>
                    <td></td>
                </tr>
                <tr>
                    <td class="FormLabel">
                        <asp:Label
                            ID="L_Type"
                            runat="server"
                            Text='<%$ Resources:Resource,L_TYPE %>'></asp:Label>
                    </td>
                    <td class="auto-style5" id="chkType">
                        <asp:RadioButton ID="rbDrug" runat="server" Text='<%$ Resources:Resource,T_DRUG %>'
                            GroupName="Type" />
                        <asp:RadioButton ID="rbMedicalConsumable" runat="server" Text='<%$ Resources:Resource, T_MEDICALCONSUMABLE %>'
                            GroupName="Type" />

                    </td>
                    <td>
                        <span style="color: Red" id="errMsgType" />
                    </td>
                </tr>
                <tr>
                    <td class="FormLabel">
                        <asp:Label
                            ID="L_Package"
                            runat="server"
                            Text='<%$ Resources:Resource,L_PACKAGE %>'></asp:Label>
                    </td>
                    <td class="auto-style5">
                        <asp:TextBox ID="txtPackage" runat="server" MaxLength="255"></asp:TextBox>
                        <%--Width="150px"--%>
                        <asp:RequiredFieldValidator
                            ID="RequiredFieldValidatorPackage" runat="server"
                            ControlToValidate="txtPackage"
                            SetFocusOnError="True"
                            ValidationGroup="check" ForeColor="Red" Display="Dynamic"
                            Text="*">
                        </asp:RequiredFieldValidator>
                    </td>
                    <td></td>
                </tr>
                <tr>
                    <td class="FormLabel">
                        <asp:Label ID="L_Price" runat="server" Text='<%$ Resources:Resource,L_PRICE %>'></asp:Label>
                    </td>
                    <td class="auto-style5">
                        <asp:TextBox ID="txtPrice" runat="server" class="numbersOnly" Style="text-align: right;"></asp:TextBox>
                        <%-- Width="150px" padding-right:4px--%>

                        <asp:CompareValidator ControlToValidate="txtPrice" ID="CompareValidator1" runat="server" SetFocusOnError="true" Type="Currency" Operator="DataTypeCheck" ErrorMessage="*" ValidationGroup="check" ForeColor="Red" Display="Dynamic"> </asp:CompareValidator>
                        <asp:RequiredFieldValidator
                            ID="RequiredFieldValidator1" runat="server"
                            ControlToValidate="txtPrice"
                            SetFocusOnError="True"
                            ValidationGroup="check" ForeColor="Red" Display="Dynamic"
                            Text="*">
                        </asp:RequiredFieldValidator>
                    </td>
                </tr>

                <tr>
                    <td class="FormLabel">
                        <asp:Label ID="L_CareType" runat="server" Text='<%$ Resources:Resource,L_CARETYPE %>'></asp:Label>
                    </td>
                    <td class="auto-style6" id="rbCareType">
                        <asp:RadioButton ID="rbOutPatient" GroupName="CareType" runat="server" Text='<%$ Resources:Resource,L_OUTPATIENT %>' />
                        <asp:RadioButton ID="rbInPatient" GroupName="CareType" runat="server" Text='<%$ Resources:Resource,L_INPATIENT %>' />
                        <asp:RadioButton ID="rbBoth" GroupName="CareType" runat="server" Text='<%$ Resources:Resource,L_BOTH %>' />
                        &nbsp
                                
                    </td>
                    <td>
                        <span style="color: Red" id="errMsgCareType" />
                    </td>
                </tr>
                <tr>
                    <td class="auto-style3">
                        <asp:Label ID="L_Frequency" runat="server" Text='<%$ Resources:Resource,L_FREQUENCY %>'></asp:Label>
                    </td>
                    <td class="auto-style5">
                        <asp:TextBox ID="txtFrequency" runat="server" Class="numbersOnly" Style="text-align: right;"></asp:TextBox>
                        <%--Width="150px" padding-right:4px--%>

                        <asp:CompareValidator ControlToValidate="txtFrequency" ID="CompareValidator2" runat="server" SetFocusOnError="true" Type="Integer" Operator="DataTypeCheck" ErrorMessage="*" ValidationGroup="check" ForeColor="Red" Display="Dynamic"> </asp:CompareValidator>
                    </td>
                    <td class="auto-style4">
                        <%--<asp:RequiredFieldValidator
                                    ID="RequiredFieldValidator2" runat="server"
                                    ControlToValidate="txtFrequency"
                                    SetFocusOnError="True"
                                    ValidationGroup="check"
                                    Text="*"></asp:RequiredFieldValidator>--%>
                    </td>
                </tr>
                <tr>
                    <td class="FormLabel" valign="middle">
                        <asp:Label ID="L_Patient" runat="server" Text='<%$ Resources:Resource,L_PATIENT %>'></asp:Label>
                    </td>
                    <td class="auto-style6" id="chk">
                        <asp:CheckBox ID="chkMan" runat="server" Text='<%$ Resources:Resource,T_MAN %>' />
                        <asp:CheckBox ID="chkWoman" runat="server" Text='<%$ Resources:Resource,T_WOMAN %>' />
                        <asp:CheckBox ID="chkAdult" runat="server" Text='<%$ Resources:Resource,T_ADULT %>' />
                        <asp:CheckBox ID="chkChild" runat="server" Text='<%$ Resources:Resource,T_CHILD %>' />
                        &nbsp
                                
                    </td>
                    <td>
                        <span style="color: Red" id="errMsgPatient" />
                    </td>
                </tr>

            </table>
        </asp:Panel>
    </div>
    <asp:Panel ID="pnlButtons" runat="server" CssClass="panelbuttons">
        <table width="100%" cellpadding="10 10 10 10">
            <tr>

                <td align="left">

                    <asp:Button
                        ID="B_SAVE"
                        runat="server"
                        Text='<%$ Resources:Resource,B_SAVE%>'
                        ValidationGroup="check" OnClientClick="validateForm();" />
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
<asp:Content ID="Content2" ContentPlaceHolderID="Footer" runat="Server">
    <asp:Label Text="" runat="server" ID="lblMsg"> </asp:Label>
    <asp:ValidationSummary ID="validationSummary" runat="server" HeaderText='<%$ Resources:Resource,V_SUMMARY%>' ValidationGroup="check" />
    <asp:HiddenField ID="hfMI" runat="server" />
</asp:Content>

<asp:Content ID="Content3" runat="server" ContentPlaceHolderID="head">
    <style type="text/css">
        .auto-style1 {
            font-family: Arial, Helvetica, sans-serif; /*min-width: 170px;*/
            ;
            height: 27px;
            direction: ltr;
            width: 214px;
        }

        .auto-style2 {
            width: 214px;
        }

        .auto-style3 {
            height: 27px;
            width: 150px;
            text-align: right;
            color: Blue;
            font-weight: normal;
            font-family: Verdana, Arial, Helvetica, sans-serif;
            font-size: 11px;
            padding-right: 1px;
        }

        .auto-style4 {
            height: 27px;
        }

        .auto-style5 {
            font-family: Arial, Helvetica, sans-serif; 
            /*min-width: 170px;*/
            height: 27px;
            direction: ltr;
            width: 271px;
        }
        .auto-style6 {
            width: 271px;
        }
    </style>
</asp:Content>


