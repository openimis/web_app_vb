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
<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/IMIS.Master" CodeBehind="Insuree.aspx.vb" Inherits="IMIS.Insuree" Title= '<%$ Resources:Resource,L_INSUREE%>' %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="contentHead" ContentPlaceHolderID="head" runat="server" >
    <script type="text/javascript" language="javascript">

    function pageLoad(sender, args) {
        $(document).ready(function() {

            $('#<%=btnBrowse.ClientID %>').click(function(e) {

                $("#SelectPic").show();

                e.preventDefault();
            });



            $("#btnCancel").click(function() {

                $("#SelectPic").hide();

            });


        });
    }

    $(document).ready(function() {
        $("#btnCancel").hide();
        $('#<%=btnBrowse.ClientID %>').click(function(e) {

            $("#SelectPic").show();
            $("#btnCancel").show();
            e.preventDefault();
        });



        $("#btnCancel").click(function() {

            $("#SelectPic").hide();
            $("#btnCancel").hide();
        });


    });
        function msgOkay(btn) {
            if (btn == "ok") {
                $("#<%=hfOK.ClientID%>").val(0);
                $("#<%=B_SAVE.ClientID %>").click();
            }
        }

    function promptInsureeAdd(btn) {
        if (btn === "ok") {
            $("#<%=hfActivate.ClientID%>").val(1);
            //theForm.__EVENTTARGET.value = $("#<%=B_SAVE.ClientID %>");
            //theForm.submit();
            
        } else {
           //var familyId = '<%=HttpContext.Current.Request.QueryString("f") %>';
            //window.location = "OverviewFamily.aspx?f=" + familyId;
            $("#<%=hfActivate.ClientID%>").val(0);
        }
        $("#<%=hfCheckMaxInsureeCount.ClientID %>").val(0);
        $("#<%=B_SAVE.ClientID %>").click();
    }  
   

        
</script>
<style type="text/css">
        #SelectPic
        {
        	padding-top:10%;
            width: 100%;
        	margin:auto;
         	text-align:center;
            vertical-align:bottom;
        	position:fixed;
	        top:0;
	        left:0;
	        height:100%;
	        z-index:1001;
	        background-color:Gray;
	        opacity:0.9;
	        display:none;
	    }
    </style>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="Body" Runat="Server">
    <asp:HiddenField ID="hfOK" Value="1" runat="server" />
    <asp:HiddenField ID="hfCheckMaxInsureeCount" Value="1" runat="server" />
    <asp:HiddenField ID="hfActivate" Value="0" runat="server" />

<div class="divBody" >  
     <asp:Panel ID="L_FAMILYPANEL" runat="server" 
             CssClass="panel" 
         GroupingText='<%$ Resources:Resource,L_FAMILYPANEL %>' >
           
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
                                <asp:Label ID="L_DISTRICT0" runat="server" Text="<%$ Resources:Resource,L_DISTRICT %>">
                            </asp:Label>
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
                              <asp:Label ID="L_WARD0" runat="server" Text="<%$ Resources:Resource,L_WARD %>">
                            </asp:Label>
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
                             <asp:Label ID="L_VILLAGE0" runat="server" Text="<%$ Resources:Resource,L_VILLAGE %>">
                            </asp:Label>
                        </td>
                        <td class="ReadOnlyText">
                            <asp:Label ID="txtVillage" runat="server"></asp:Label>
                        </td>
                        <td class="ReadOnlyText">&nbsp;</td>
                    </tr>
                </table>      
                    
                    
         </asp:Panel>
     <asp:Panel ID="Panel2" runat="server" ScrollBars="Auto" CssClass="panelBody" style="height:auto; " GroupingText='<%$ Resources:Resource,L_INSUREE%>'>
        
                 <table width="100%">
                <tr>
                    <td valign="top">
                        <%--<asp:UpdatePanel ID="upCHFID" runat="server">
                            <ContentTemplate>--%>
                                <table width="100%">
                                    <tr id="trRelation" runat="server">
                                        <td class="FormLabel">
                                            <asp:Label ID="L_Relation" runat="server" Text="<%$ Resources:Resource, L_RELATION %>"></asp:Label>
                                        </td>
                                        <td class="DataEntry">

                                            <asp:DropDownList ID="ddlRelation" runat="server" Width="150px">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="rfRelation" runat="server" ControlToValidate="ddlRelation" InitialValue="" SetFocusOnError="True" ValidationGroup="check" ForeColor="Red" Display="Dynamic"
                                        Text='*'></asp:RequiredFieldValidator>
                                                    </td>
                                             <td class="FormLabel">&nbsp;</td>
                                        <td class="DataEntry">&nbsp;</td>
                                    
                                    </tr>
                                    <tr>
                                        <td class="FormLabel">
                                            <asp:Label ID="L_CHFID" runat="server" Text="<%$ Resources:Resource,L_CHFID%>">
                                            </asp:Label>
                                        </td>
                                        <td class="DataEntry">
                                            <asp:UpdatePanel ID="up1" runat="server">
                                                <ContentTemplate>
                                                    <asp:TextBox ID="txtCHFID" runat="server" AutoPostBack="True" CssClass="numbersOnly" MaxLength="12" Width="150px"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldCHFID0" runat="server" ControlToValidate="txtCHFID" SetFocusOnError="True" ValidationGroup="check" ForeColor="Red" Display="Dynamic" Text='*'>
                                                    </asp:RequiredFieldValidator>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                            
                                        </td>
                                        <td class="FormLabel">&nbsp;</td>
                                        <td class="DataEntry">&nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td class="FormLabel">
                                            <asp:Label ID="L_OTHERNAMES0" runat="server" Text="<%$ Resources:Resource,L_OTHERNAMES %>">
                                            </asp:Label>
                                        </td>
                                        <td class="DataEntry">
                                            <asp:TextBox ID="txtOtherNames" runat="server"   Width="150px"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldOtherNames1" runat="server" ControlToValidate="txtOtherNames" SetFocusOnError="True" ValidationGroup="check" ForeColor="Red" Display="Dynamic"
                                            Text='*'>
                                            </asp:RequiredFieldValidator>
                                             
                                        </td>
                                        <td class="FormLabel">&nbsp;</td>
                                        <td class="DataEntry">&nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td class="FormLabel">
                                            <asp:Label ID="L_LASTNAME0" runat="server" Text="<%$ Resources:Resource,L_LASTNAME %>">
                                            </asp:Label>
                                        </td>
                                        <td class="DataEntry">
                                            <asp:TextBox ID="txtLastName" runat="server"   Width="150px"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldLastName2" runat="server" ControlToValidate="txtLastName" SetFocusOnError="True" ValidationGroup="check" ForeColor="Red" Display="Dynamic" Text='*'>
                                            </asp:RequiredFieldValidator>
                                             
                                        </td>
                                        <td class="FormLabel">&nbsp;</td>
                                        <td class="DataEntry">&nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td class="FormLabel">
                                            <asp:Label
                                                ID="L_BIRTHDATE"
                                                runat="server"
                                                Text='<%$ Resources:Resource,L_BIRTHDATE %>'>
                                            </asp:Label>
                                        </td>
                                        <td class="DataEntry">
                                            <asp:TextBox ID="txtBirthDate" runat="server" Width="132px"></asp:TextBox>
                                           <%-- <asp:MaskedEditExtender ID="txtBirthDate_MaskedEditExtender" runat="server"
                                                CultureDateFormat="dd/MM/YYYY"
                                                TargetControlID="txtBirthDate" Mask="99/99/9999" MaskType="Date"
                                                UserDateFormat="DayMonthYear">
                                            </asp:MaskedEditExtender>--%>

                                            <asp:Button ID="Button1" runat="server" Height="20px" Width="20px" />


                                            <asp:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtBirthDate" PopupButtonID="Button1" Format="dd/MM/yyyy"></asp:CalendarExtender>
                                            <asp:RequiredFieldValidator ID="RequiredFieldBirthDate0" runat="server" ControlToValidate="txtBirthDate" SetFocusOnError="True" ValidationGroup="check" ForeColor="Red" Display="Dynamic" Text='*'>
                                            </asp:RequiredFieldValidator>
                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator7" runat="server" ControlToValidate="txtBirthDate" ErrorMessage="*" SetFocusOnError="false" ValidationExpression="^(0[1-9]|[12][0-9]|3[01])[/](0[1-9]|1[012])[/](19|20)\d\d$" ValidationGroup="check" ForeColor="Red" Display="Dynamic"></asp:RegularExpressionValidator>
                                        </td>
                                        <td class="FormLabel">
                                            <br />
                                        </td>
                                        <td class="DataEntry">&nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td class="FormLabel">
                                            <asp:Label
                                                ID="L_GENDER"
                                                runat="server"
                                                Text='<%$ Resources:Resource,L_GENDER %>'>
                                            </asp:Label>
                                        </td>
                                        <td class="DataEntry">
                                            <asp:DropDownList ID="ddlGender" runat="server" Width="150px">
                                            </asp:DropDownList>

                                            <asp:RequiredFieldValidator ID="RequiredFieldGender0" runat="server" ControlToValidate="ddlGender" InitialValue="" SetFocusOnError="True" ValidationGroup="check" ForeColor="Red" Display="Dynamic"
                                                Text='*'>
                                            </asp:RequiredFieldValidator>

                                        </td>
                                        <td class="FormLabel">
                                            &nbsp;</td>
                                        <td class="DataEntry">
                                            &nbsp;</td>
                                    </tr>
                                    <tr id="trMaritalStatus" runat="server">
                                        <td class="FormLabel" >
                                            <asp:Label
                                                ID="L_MARITAL"
                                                runat="server"
                                                Text='<%$ Resources:Resource,L_MARITAL %>'
                                                visible="false">
                                                
                                            </asp:Label>
                                        </td>
                                        <td class="DataEntry"  >
                                            <asp:DropDownList ID="ddlMarital" runat="server" Width="150px" Visible="false">
                                            </asp:DropDownList>

                                            <asp:RequiredFieldValidator ID="rfMaritalStatus" runat="server" ControlToValidate="ddlMarital" InitialValue="" SetFocusOnError="True" ValidationGroup="check" ForeColor="Red" Display="Dynamic"
                                                Text='*'>
                                            </asp:RequiredFieldValidator>

                                        </td>
                                        <td class="FormLabel">
                                            <%--<asp:RequiredFieldValidator ID="RequiredFieldMarital" runat="server" 
                        ControlToValidate="ddlMarital" 
                        InitialValue="" 
                        SetFocusOnError="True" 
                        ValidationGroup="check"
                        Text="*">
                        </asp:RequiredFieldValidator>--%>
                                        </td>
                                        <td class="DataEntry">
                                            &nbsp;</td>
                                    </tr>

                                    <tr id="trBeneficiary" runat="server">
                                        <td class="FormLabel">
                                            <asp:Label
                                                ID="L_CARD"
                                                runat="server"
                                                Text='<%$ Resources:Resource,L_CARD%>'>
                                            </asp:Label>
                                        </td>
                                        <td class="DataEntry">
                                            <asp:DropDownList
                                                ID="ddlCardIssued"
                                                runat="server"
                                                Width="150px">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="rfBeneficiary" runat="server" ControlToValidate="ddlCardIssued" InitialValue="" SetFocusOnError="True" ValidationGroup="check" ForeColor="Red" Display="Dynamic"
                                                Text='*'>
                                            </asp:RequiredFieldValidator>
                                        </td>
                                        <td class="FormLabel">
                                            &nbsp;</td>
                                        <td class="DataEntry">
                                            &nbsp;</td>
                                    </tr>

                                    <tr id="trCurrentRegion" runat="server">
                                        <td class="FormLabel">
                                            <asp:Label ID="lblCurrentRegion" runat="server" Text="<%$ Resources:Resource, L_CREGION %>"></asp:Label>
                                        </td>
                                        <td class="DataEntry">
                                            <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                                <ContentTemplate>
                                                    <asp:DropDownList ID="ddlCurrentRegion" Width="150px" runat="server" AutoPostBack="True">

                                                    </asp:DropDownList>
                                                    <asp:RequiredFieldValidator ID="rfCurrentRegion" runat="server" ControlToValidate="ddlCurrentRegion" InitialValue="0" SetFocusOnError="True" style="direction: ltr" ValidationGroup="check" ForeColor="Red" Display="Dynamic"
                                                        Text='*'>
                                                    </asp:RequiredFieldValidator>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                        <td class="FormLabel">&nbsp;</td>
                                        <td class="DataEntry">&nbsp;</td>
                                    </tr>

                                    <tr id="trCurrentDistrict" runat="server">
                                        <td class="FormLabel">
                                            <asp:Label ID="lblCurrentDistrict1" runat="server" Text="<%$ Resources:Resource, L_CDISTRICT %>"></asp:Label>
                                        </td>
                                        <td class="DataEntry">
                                            <asp:UpdatePanel ID="upCurDistrict" runat="server">
                                                <ContentTemplate>
                                                    <asp:DropDownList ID="ddlCurrentDistrict" Width="150px" runat="server" AutoPostBack="True">

                                                    </asp:DropDownList>
                                                    <asp:RequiredFieldValidator ID="rfCurrentDistrict" runat="server" ControlToValidate="ddlCurrentDistrict" InitialValue="0" SetFocusOnError="True" style="direction: ltr" ValidationGroup="check" ForeColor="Red" Display="Dynamic"
                                                        Text='*'>
                                                    </asp:RequiredFieldValidator>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                        <td class="FormLabel">&nbsp;</td>
                                        <td class="DataEntry">&nbsp;</td>
                                    </tr>
                                    <tr id="trCurrentMunicipality" runat="server">
                                        <td class="FormLabel">
                                            <asp:Label ID="lblCurrentVDC1" runat="server" Text="<%$ Resources:Resource, L_CVDC %>"></asp:Label>
                                        </td>
                                        <td class="DataEntry">
                                            <asp:UpdatePanel ID="upCurVDC" runat="server">
                                                <ContentTemplate>
                                                    <asp:DropDownList ID="ddlCurentWard" runat="server"  Width="150px" AutoPostBack="True"></asp:DropDownList>
                                                    <asp:RequiredFieldValidator ID="rfCurrentVDC" runat="server" ControlToValidate="ddlCurentWard" InitialValue="0" SetFocusOnError="True" style="direction: ltr" ValidationGroup="check" ForeColor="Red" Display="Dynamic"
                                                        Text='*'>
                                                    </asp:RequiredFieldValidator>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                        <td class="FormLabel">&nbsp;</td>
                                        <td class="DataEntry">&nbsp;</td>
                                    </tr>
                                    <tr id="trCurrentVillage" runat="server">
                                        <td class="FormLabel">
                                            <asp:Label ID="lblCurrentWard1" runat="server" Text="<%$ Resources:Resource, L_CWARD %>"></asp:Label>
                                        </td>
                                        <td class="DataEntry">
                                            <asp:UpdatePanel ID="upCurWard" runat="server">
                                                <ContentTemplate>
                                                    <asp:DropDownList ID="ddlCurrentVillage"  Width="150px" runat="server"></asp:DropDownList>
                                                    <asp:RequiredFieldValidator ID="rfCurrentVillage" runat="server" ControlToValidate="ddlCurrentVillage" InitialValue="0" SetFocusOnError="True" style="direction: ltr" ValidationGroup="check" ForeColor="Red" Display="Dynamic"
                                                    Text='*'></asp:RequiredFieldValidator>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                        <td class="FormLabel">&nbsp;</td>
                                        <td class="DataEntry">&nbsp;</td>
                                    </tr>
                                    <tr id="trCurrentAddress" runat="server"    >
                                        <td class="FormLabel" style="vertical-align: top">
                                            <asp:Label ID="lblCurrentAddress0" runat="server" Text="<%$ Resources:Resource, L_CURRENTADDRESS %>"></asp:Label>
                                        </td>
                                        <td class="DataEntry">
                                            <asp:TextBox ID="txtCurrentAddress" runat="server" Height="40px" MaxLength="25" style="resize:none;" TextMode="MultiLine" Width="150px"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfCurrentAddress" runat="server" ControlToValidate="txtCurrentAddress" InitialValue="" SetFocusOnError="True" ValidationGroup="check" ForeColor="Red" Display="Dynamic"
                                                Text='*'>
                                            </asp:RequiredFieldValidator>
                                        </td>
                                        <td class="FormLabel">&nbsp;</td>
                                        <td class="DataEntry">&nbsp;</td>
                                    </tr>
                                    <tr id="trProfession" runat="server">
                                            <td class="FormLabel">
                                            <asp:Label ID="L_PROFESSION0" runat="server" Text="<%$ Resources:Resource, L_PROFESSION %>"> </asp:Label>
                                        </td>
                                        <td class="DataEntry">
                                            <asp:DropDownList ID="ddlProfession" runat="server" Width="150px">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="rfProfession" runat="server" ControlToValidate="ddlProfession" InitialValue="0" SetFocusOnError="True" ValidationGroup="check" ForeColor="Red" Display="Dynamic"
                                                Text='*'>
                                            </asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr id="trEducation" runat="server">
                                        <td class="FormLabel">
                                            <asp:Label ID="L_EDUCATION0" runat="server" Text="<%$ Resources:Resource, L_EDUCATION %>" ></asp:Label>
                                        </td>
                                        <td class="DataEntry">
                                            <asp:DropDownList ID="ddlEducation" runat="server" Width="150px">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="rfEducation" runat="server" ControlToValidate="ddlEducation" InitialValue="0" SetFocusOnError="True" ValidationGroup="check" ForeColor="Red" Display="Dynamic"
                                            Text='*'></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="FormLabel">
                                            <asp:Label
                                                ID="L_PHONE"
                                                runat="server"
                                                Text='<%$ Resources:Resource,L_PHONE%>'>
                                            </asp:Label>
                                        </td>
                                        <td class="DataEntry">
                                            <asp:TextBox ID="txtPhone" runat="server" Width="150px"></asp:TextBox>
                                        </td>
                                        <td class="FormLabel" style="vertical-align: top">
                                            &nbsp;</td>
                                        <td class="DataEntry">
                                            &nbsp;</td>
                                    </tr>
                                    <tr id="trEmail" runat="server">
                                        <td class="FormLabel">
                                            <asp:Label ID="L_EMAIL" runat="server" Text="<%$ Resources:Resource, L_EMAIL %>"></asp:Label>
                                        </td>
                                        <td class="DataEntry">
                                            <asp:TextBox ID="txtEmail" runat="server" Width="150px"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfEmail" runat="server" ControlToValidate="txtEmail" SetFocusOnError="True" ValidationGroup="check" ForeColor="Red" Display="Dynamic"
                                            Text='*'></asp:RequiredFieldValidator>
<%--                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server" ControlToValidate="txtEmail" ErrorMessage="Invalid Email Id" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ValidationGroup="check">*</asp:RegularExpressionValidator>--%>
                                        </td>
                                        <td class="FormLabel" style="vertical-align: top">&nbsp;</td>
                                        <td class="DataEntry">&nbsp;</td>
                                    </tr>
                                    <tr id="trIdentificationType" runat="server">
                                        <td class="FormLabel">
                                            <asp:Label ID="L_IDTYPE" runat="server" Text="<%$ Resources:Resource, L_IDTYPE %>"></asp:Label>
                                        </td>
                                        <td class="DataEntry">
                                            <asp:DropDownList ID="ddlIdType" runat="server" Width="150px">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="rfIdType" runat="server" ControlToValidate="ddlIdType" InitialValue="" SetFocusOnError="True" ValidationGroup="check" ForeColor="Red" Display="Dynamic"
                                            Text='*'></asp:RequiredFieldValidator>
                                        </td>
                                        <td class="FormLabel">
                                            &nbsp;</td>
                                        <td class="DataEntry">
                                            &nbsp;</td>
                                    </tr>
                                    <tr id="trIdentificationNo" runat="server">
                                        <td class="FormLabel">
                                            <asp:Label ID="L_PASSPORT1" runat="server" Text="<%$ Resources:Resource,L_PASSPORT%>">
                                            </asp:Label>
                                        </td>
                                        <td class="DataEntry">
                                            <asp:TextBox ID="txtPassport" runat="server" MaxLength="12" Width="150px"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfIdNo" runat="server" ControlToValidate="txtPassport" InitialValue="" SetFocusOnError="True" ValidationGroup="check" ForeColor="Red" Display="Dynamic"
                                            Text='*'></asp:RequiredFieldValidator>
                                        </td>
                                        <td class="FormLabel">
                                            &nbsp;</td>
                                        <td class="DataEntry">
                                            &nbsp;</td>
                                    </tr>

                                    <tr id="trVulnerability" runat="server">
                                        <td class="FormLabel">
                                            <asp:Label
                                                ID="Label1"
                                                runat="server"
                                                Text='<%$ Resources:Resource,L_VULNERABILITY%>'>
                                            </asp:Label>
                                        </td>
                                        <td class="DataEntry">
                                            <asp:DropDownList
                                                ID="ddlVulnerability"
                                                runat="server"
                                                Width="150px">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="rfVulnerability" runat="server" ControlToValidate="ddlVulnerability" InitialValue="" SetFocusOnError="True" ValidationGroup="check" ForeColor="Red" Display="Dynamic"
                                                Text='*'>
                                            </asp:RequiredFieldValidator>
                                        </td>
                                        <td class="FormLabel">
                                            &nbsp;</td>
                                        <td class="DataEntry">
                                            &nbsp;</td>
                                    </tr>


                                    <tr id="trFSPRegion" runat="server">
                                        <td class="FormLabel">
                                            <asp:Label ID="lblFSPRegion" runat="server" Text="<%$ Resources:Resource, L_FSPREGION %>"></asp:Label>
                                        </td>
                                        <td class="DataEntry">
                                            <asp:DropDownList ID="ddlFSPRegion" runat="server" AutoPostBack="True" Width="150px">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="rfRegionFSP" runat="server" ControlToValidate="ddlFSPRegion" InitialValue="0" SetFocusOnError="True" Style="direction: ltr" ValidationGroup="check" ForeColor="Red" Display="Dynamic"
                                            Text='*'></asp:RequiredFieldValidator>
                                        </td>
                                        <td>&nbsp;</td>
                                        <td class="DataEntry">&nbsp;</td>
                                    </tr>
                                    <tr id="trFSPDistrict" runat="server">
                                        <td class="FormLabel">
                                            <asp:Label ID="lblFSPDistrict" runat="server" Text="<%$ Resources:Resource, L_FSPDISTRICT %>"></asp:Label>
                                        </td>
                                        <td class="DataEntry">
                                            <asp:UpdatePanel ID="upDist" runat="server">
                                                <ContentTemplate>
                                                    <asp:DropDownList ID="ddlFSPDistrict" runat="server" AutoPostBack="True" Width="150px">
                                                    </asp:DropDownList>
                                                    <asp:RequiredFieldValidator ID="rfDistrictFSP" runat="server" ControlToValidate="ddlFSPDistrict" InitialValue="0" SetFocusOnError="True" style="direction: ltr" ValidationGroup="check" ForeColor="Red" Display="Dynamic"
                                                    Text='*'></asp:RequiredFieldValidator>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                            
                                        </td>
                                        <td class="FormLabel">
                                            &nbsp;</td>
                                        <td class="DataEntry">
                                            &nbsp;</td>
                                    </tr>
                                    <tr id="trFSPCategory" runat="server">
                                        <td class="FormLabel">
                                            <asp:Label ID="lblFSPCategory" runat="server" Text="<%$ Resources:Resource, L_FSPCATEGORY %>"></asp:Label>
                                        </td>
                                        <td class="DataEntry">
                                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                <ContentTemplate>
                                            <asp:DropDownList ID="ddlFSPCateogory" runat="server" AutoPostBack="True" Width="150px">
                                            </asp:DropDownList>
                                                    <asp:RequiredFieldValidator ID="rfFSPCategory" runat="server" ControlToValidate="ddlFSPCateogory" InitialValue="0" SetFocusOnError="True" style="direction: ltr" ValidationGroup="check" ForeColor="Red" Display="Dynamic"
                                                    Text='*'></asp:RequiredFieldValidator>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                        </td>
                                        <td class="FormLabel">&nbsp;</td>
                                        <td class="DataEntry">&nbsp;</td>
                                    </tr>
                                    <tr id="trFSP" runat="server">
                                        <td class="FormLabel">
                                            <asp:Label ID="lblFSP" runat="server" Text="<%$ Resources:Resource, L_FSP %>"></asp:Label>
                                        </td>
                                        <td class="DataEntry">
                                            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                                <ContentTemplate>
                                            <asp:DropDownList ID="ddlFSP" runat="server" Width="150px">
                                            </asp:DropDownList>
                                                    <asp:RequiredFieldValidator ID="rfFSP" runat="server" ControlToValidate="ddlFSP" InitialValue="0" SetFocusOnError="True" style="direction: ltr" ValidationGroup="check" ForeColor="Red" Display="Dynamic"
                                                    Text='*'></asp:RequiredFieldValidator>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                        </td>
                                        <td class="FormLabel">
                                            &nbsp;</td>
                                        <td class="DataEntry">
                                            &nbsp;</td>
                                    </tr>

                                    

                                </table>
                            <%--</ContentTemplate>
                        </asp:UpdatePanel>    --%>   
                    </td>
                    <td valign="top" style="width:225px;">
                        <table width="200px">

                            <tr>
                                <td valign="top">
                                <asp:UpdatePanel ID="upImage" runat="server" style="width:200px; height:200px; text-align:center">
                                    <ContentTemplate>
                                        <asp:Image ID="Image1" runat="server" style="max-height:200px" ImageAlign="Middle" onerror="NoImage(this);" />
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                                    
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:Button runat="server" ID="btnBrowse" Text='<%$ Resources:Resource,B_BROWSE%>' />
                                 
                                    <asp:HiddenField ID="hfFamilyId" runat="server" />
                                 
                                    </td>
                            </tr>
                        </table>
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

            
               <asp:UpdatePanel ID="upDL" runat="server">
                    <ContentTemplate>
                    <table id="SelectPic">
                    <tr>
                        <td align="center">
                        <asp:Panel ID="pnlImages" runat="server" Width="500px" Height="450px" BackColor="White" ScrollBars="Auto">
                        <asp:DataList ID="dlImages" runat="server" RepeatColumns="4" RepeatDirection="Horizontal" DataKeyField="ImagePath" OnSelectedIndexChanged="dlImages_SelectedIndexChanged">
                            
                            <ItemTemplate>
                                <table width="100px" style="height:100px">
                                <tr>
                                    <td align="center">
                                        
                                        On: <%#Eval("TakenDate")%>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <img alt="" width="100px" height="100px" src='Images\Submitted\<%#eval("ImagePath") %>' />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                       <asp:LinkButton ID="lnkSelect" runat="server" CommandName="Select">Select</asp:LinkButton>
                                    </td>
                                </tr>
                            </table>
                            </ItemTemplate>
                            
                        </asp:DataList>
                           
                                                       
                        </asp:Panel>
                
                <br />
                
                            
                 <input type="button" id="btnCancel" value="Cancel" />
                        </td>
                    </tr>
                    
                </table>    
                    </ContentTemplate>
               </asp:UpdatePanel> 
                
              
                
                
                  

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Footer" Runat="Server">
    <asp:ValidationSummary ID="validationSummary" runat="server" HeaderText='<%$ Resources:Resource,V_SUMMARY%>' ValidationGroup="check" />
    <asp:label id="lblMsg" runat="server"></asp:label>
</asp:Content>
