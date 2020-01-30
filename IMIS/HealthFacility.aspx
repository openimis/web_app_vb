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
<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/IMIS.Master" CodeBehind="HealthFacility.aspx.vb" Inherits="IMIS.HealthFacility" 
    title='<%$ Resources:Resource,L_HFACILITIES%>' %>
    <%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Body" Runat="Server">
    
    <script type="text/javascript">
        function pageLoadExtend() {
            $('#<%= gvVillage.ClientID%> input').click(function () {
                var checked = $(this).is(":checked");
                var catchment = $(this).closest("tr").find($("input:text")).val()
                if (checked && catchment == "") {
                    $(this).closest("tr").find($("input:text")).val(100);
                } else {
                    $(this).closest("tr").find($("input:text")).val("");
                }
                

            });
                                            
                $("#<%=B_SAVE.ClientID%>").click(function() {
                var msg = "";
                var $focused = null;
               
                $('#<%=gvVillage.ClientID%> input[type=checkbox]').each(function () {
                    var checked = $(this).is(":checked");
                    var catchmentValue = $(this).closest("tr").find($("input:text")).val();
                   if (checked && catchmentValue > 100) {
                       msg = "<%= imisgen.getMessage("M_EXEEDPAECENTAGE", True)%>";
                   }
                   else if (checked && catchmentValue == 0) {
                    msg = "<%= imisgen.getMessage("M_CATCHMENTZERO", True)%>";
                   }
                    
                });
                if($.trim(msg) !== "" ){
                    popup.alert(msg, function () {
                        $focused.focus();
                    });
                    return false;
                }
                return true
            
            });
        }
    </script>
    <div class="divBody" >  
        <asp:Panel ID="Panel2" runat="server" ScrollBars="Auto"
            CssClass="panel" GroupingText='<%$ Resources:Resource,G_HEALTHFACILITY%>'>
            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                <ContentTemplate>
                    <div class="CatchmentLocation_" style="width: 30%; float: left">
                        <table style="float: left">

                            <tr>
                                <td class="FormLabel">
                                    <asp:Label ID="L_Legal" runat="server" Text='<%$ Resources:Resource,L_LEGALFORM%>'></asp:Label>
                                </td>
                                <td class="DataEntry">
                                    <asp:DropDownList ID="ddlLegalForm" runat="server" ValidationGroup="check">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidatorLegalForm" runat="server"
                                        Text="*" ControlToValidate="ddlLegalForm" SetFocusOnError="True"
                                        ValidationGroup="check" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                                </td>

                            </tr>
                            <tr>
                                <td class="FormLabel">
                                    <asp:Label ID="L_LEVEL" runat="server" Text='<%$ Resources:Resource,R_LEVEL%>'></asp:Label>
                                </td>
                                <td class="DataEntry">
                                    <asp:DropDownList ID="ddlHFLevel" runat="server" ValidationGroup="check">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidatorHFLevel" runat="server"
                                        Text="*" ControlToValidate="ddlHFLevel" SetFocusOnError="True"
                                        ValidationGroup="check" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td class="FormLabel">
                                    <asp:Label ID="Label1" runat="server" Text='<%$ Resources:Resource,L_SUBLEVEL%>'></asp:Label>
                                </td>
                                <td class="DataEntry">
                                    <asp:DropDownList ID="ddlSublevel" runat="server" ValidationGroup="check">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"
                                        Text="*" ControlToValidate="ddlHFLevel" SetFocusOnError="True"
                                        ValidationGroup="check" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td class="FormLabel">
                                    <asp:Label ID="L_CODE" runat="server" Text='<%$ Resources:Resource,L_CODE%>'></asp:Label>
                                </td>
                                <td class="DataEntry">
                                    <asp:TextBox ID="txtHFCode" runat="server"
                                        ValidationGroup="check" MaxLength="8"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:RequiredFieldValidator ID="RequiredFieldHFCode" runat="server"
                                        Text="*" ControlToValidate="txtHFCode" SetFocusOnError="True"
                                        ValidationGroup="check" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td class="FormLabel">
                                    <asp:Label
                                        ID="L_HFNAME"
                                        runat="server"
                                        Text='<%$ Resources:Resource,L_NAME%>'></asp:Label>
                                </td>
                                <td class="DataEntry">
                                    <asp:TextBox ID="txtFacilityName" runat="server" class="DataEntryWide"
                                        ValidationGroup="check" MaxLength="100" Width="150px"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:RequiredFieldValidator
                                        ID="RequiredFieldOtherNames" runat="server"
                                        ControlToValidate="txtFacilityName"
                                        SetFocusOnError="True"
                                        ValidationGroup="check" ForeColor="Red" Display="Dynamic"
                                        Text='*'></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td class="auto-style1">
                                    <asp:Label
                                        ID="L_ADDRESS"
                                        runat="server"
                                        Text='<%$ Resources:Resource,L_LOCATIONADDRESS%>'></asp:Label>
                                </td>
                                <td class="auto-style2">
                                    <asp:TextBox ID="txtAddress" runat="server" TextMode="MultiLine" class="DataEntryWide"
                                        ValidationGroup="check" MaxLength="100" Width="150px"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidatorAddress" runat="server"
                                        Text="*" ControlToValidate="txtAddress"
                                        SetFocusOnError="True" ValidationGroup="check" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td class="FormLabel">
                                    <asp:Label ID="L_REGION" runat="server" Text='<%$ Resources:Resource,L_REGION %>'></asp:Label>
                                </td>
                                <td class="DataEntry">
                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                        <ContentTemplate>
                                            <asp:DropDownList ID="ddlRegion" runat="server" AutoPostBack="true" ValidationGroup="check">
                                            </asp:DropDownList>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>

                                </td>
                                <td>
                                    <asp:RequiredFieldValidator ID="RequiredFieldRegion" runat="server" ControlToValidate="ddlRegion" InitialValue="0" Text="*" ValidationGroup="check" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td class="FormLabel">
                                    <asp:Label ID="L_District" runat="server" Text='<%$ Resources:Resource,L_DISTRICT%>'></asp:Label>
                                </td>
                                <td class="DataEntry">

                                    <asp:DropDownList ID="ddlDistrict" runat="server" AutoPostBack="true" ValidationGroup="check">
                                    </asp:DropDownList>

                                </td>
                                <td>
                                    <asp:RequiredFieldValidator ID="RequiredFieldDistrict" runat="server" ControlToValidate="ddlDistrict" InitialValue="0" Text="*" ValidationGroup="check" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td class="FormLabel">
                                    <asp:Label ID="L_Phone" runat="server" Text="<%$ Resources:Resource,L_PHONE%>"></asp:Label>
                                </td>
                                <td class="DataEntry">
                                    <asp:TextBox ID="txtPhone" runat="server" MaxLength="50" ValidationGroup="check" Width="150px"></asp:TextBox>
                                </td>
                                <td><%--<asp:RequiredFieldValidator ID="RequiredFieldPhone" runat="server" 
                                    ControlToValidate="txtPhone" Text="*" 
                                    ValidationGroup="check"></asp:RequiredFieldValidator>--%></td>
                            </tr>
                            <tr>
                                <td class="FormLabel">
                                    <asp:Label ID="L_FAX" runat="server" Text='<%$ Resources:Resource,L_FAX%>'></asp:Label>
                                </td>
                                <td class="DataEntry">
                                    <asp:TextBox ID="txtFax" runat="server" Width="150px" MaxLength="50"></asp:TextBox>
                                </td>
                                <td>&nbsp;</td>
                            </tr>
                            <tr>
                                <td class="FormLabel">
                                    <asp:Label ID="L_EMAIL" runat="server" Text='<%$ Resources:Resource,L_EMAIL%>'></asp:Label>
                                </td>
                                <td class="DataEntry">
                                    <asp:TextBox ID="txtEmail" runat="server" class="DataEntryWide" MaxLength="50" Width="150px"></asp:TextBox>

                                </td>
                                <td>&nbsp;</td>
                            </tr>
                            <tr>
                                <td class="FormLabel">
                                    <asp:Label ID="L_TYPE" runat="server" Text='<%$ Resources:Resource,L_CARETYPE%>'></asp:Label>
                                </td>
                                <td class="DataEntry">
                                    <asp:DropDownList ID="ddlType" runat="server" ValidationGroup="check">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidatorType" runat="server"
                                        Text="*"
                                        ControlToValidate="ddlType" SetFocusOnError="True" ValidationGroup="check" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td class="FormLabel" valign="top">
                                    <asp:Label ID="L_PLMS" runat="server" Text='<%$ Resources:Resource,G_PRICELISTSMS%>'></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddOwnPricerListService" runat="server" class="DataEntryWide" Width="150px">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <%--<asp:RequiredFieldValidator ID="RequiredFieldValidatorPLMS" runat="server" 
                                        Text="*" 
                                        ControlToValidate="ddOwnPricerListService" SetFocusOnError="True" 
                                        ValidationGroup="check" InitialValue="0"></asp:RequiredFieldValidator>--%>
                                </td>
                            </tr>
                            <tr>
                                <td class="FormLabel" valign="top">
                                    <asp:Label ID="L_PLMI" runat="server" Text='<%$ Resources:Resource,G_PRICELISTSMI%>'></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddOwnPricerListItem" runat="server" Width="150px" class="DataEntryWide">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <%--<asp:RequiredFieldValidator ID="RequiredFieldValidatorPLMI" runat="server" 
                                        Text="*" 
                                        ControlToValidate="ddOwnPricerListItem" SetFocusOnError="True" 
                                    ValidationGroup="check" InitialValue="0"></asp:RequiredFieldValidator>--%>
                                </td>
                            </tr>
                            <tr>
                                <td class="FormLabel">
                                    <asp:Label ID="L_ACCCODE" runat="server" Text='<%$ Resources:Resource,L_ACCCODE%>'></asp:Label>
                                </td>
                                <td class="DataEntry">
                                    <asp:TextBox ID="txtAccCode" runat="server" Width="150px" ValidationGroup="check"
                                        MaxLength="50"></asp:TextBox>
                                </td>
                                <td>
                                    <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                                    ControlToValidate="txtAccCode" Text="*" 
                                    ValidationGroup="check"></asp:RequiredFieldValidator>--%>
                                </td>
                            </tr>

                        </table>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
            <div class="CatchmentLocation" style="width: 70%; float: right; display: table-cell">
                <asp:UpdatePanel ID="upLocation" runat="server">
                    <ContentTemplate>
                        <table>
                            <tr>

                                <td>
                                    <asp:CheckBox ID="chkCheckAllR" Visible="false" runat="server" Text='<%$ Resources:Resource,L_CHECKALL%>' margin-left="10px" onClick="toggleCheckRegions(this.checked);" />


                                    <asp:Panel ID="pnlRegion" runat="server" ScrollBars="Auto" Height="430px" Width="130px"
                                        CssClass="panel" GroupingText='<%$ Resources:Resource, L_REGION %>'>

                                        <asp:GridView ID="gvRegion" runat="server"
                                            AutoGenerateColumns="False"
                                            ShowSelectButton="True"
                                            GridLines="None"
                                            CssClass="mGrid"
                                            PagerStyle-CssClass="pgr"
                                            DataKeyNames="checked,RegionId"
                                            EmptyDataText='<%$ Resources:Resource,M_NOREGIONS %>>'>
                                            <%--'OnCheckedChanged="FirstCellClicked" M_NODISTRICTS M_NOREGIONS--%>
                                            <Columns>
                                                <asp:TemplateField HeaderText="Select">
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkRegionSelect" OnCheckedChanged="FirstCellClicked" runat="server" HeaderStyle-Width="20px" Checked='<%# eval("Checked") %>' />
                                                    </ItemTemplate>
                                                    <HeaderTemplate>
                                                    </HeaderTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="">
                                                    <ItemTemplate>
                                                        <asp:HiddenField ID="hfRegion" runat="server" Value='<%#Eval("RegionId") %>' />
                                                    </ItemTemplate>
                                                    <HeaderTemplate>
                                                    </HeaderTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="RegionName" HeaderText='<%$ Resources:Resource,L_REGION %>' SortExpression="RegionName"
                                                    HeaderStyle-Width="110px">
                                                    <HeaderStyle Width="200px" />
                                                </asp:BoundField>


                                            </Columns>

                                            <PagerStyle CssClass="pgr" />
                                            <AlternatingRowStyle CssClass="alt" />
                                            <SelectedRowStyle CssClass="srs" />

                                        </asp:GridView>
                                        <span style="color: Red" id="Span2" />

                                        </span>

                                    </asp:Panel>

                                </td>

                                <td>
                                    <asp:CheckBox ID="chkAllD" Visible="false" runat="server" Text='<%$ Resources:Resource,L_CHECKALL%>' margin-left="10px" onClick="toggleCheckDistrict(this.checked);" />

                                    <asp:Panel ID="pnlDistrict" runat="server" ScrollBars="Auto" Height="430px" Width="130px"
                                        CssClass="panel" GroupingText='<%$ Resources:Resource, L_DISTRICT %>'>

                                        <asp:GridView ID="gvDistrict" runat="server"
                                            AutoGenerateColumns="False"
                                            ShowSelectButton="True"
                                            GridLines="None"
                                            CssClass="mGrid"
                                            PagerStyle-CssClass="pgr"
                                            DataKeyNames="checked,DistrictId,Region"
                                            EmptyDataText='<%$ Resources:Resource,M_NODISTRICTS %>'>

                                            <Columns>
                                                <asp:TemplateField HeaderText="Select">
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkDistrictSelect" OnCheckedChanged="FirstCellClicked" runat="server" HeaderStyle-Width="20px" Checked='<%# eval("Checked") %>' />

                                                    </ItemTemplate>
                                                    <HeaderTemplate>
                                                    </HeaderTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="">
                                                    <ItemTemplate>
                                                        <asp:HiddenField ID="hfRegion" runat="server" Value='<%#Eval("Region") %>' />
                                                    </ItemTemplate>
                                                    <HeaderTemplate>
                                                    </HeaderTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="Districtname" HeaderText='<%$ Resources:Resource,L_DISTRICT %>' SortExpression="Districtname"
                                                    HeaderStyle-Width="110px">
                                                    <HeaderStyle Width="200px" />
                                                </asp:BoundField>


                                            </Columns>

                                            <PagerStyle CssClass="pgr" />
                                            <AlternatingRowStyle CssClass="alt" />
                                            <SelectedRowStyle CssClass="srs" />

                                        </asp:GridView>
                                        <span style="color: Red" id="errDistrictMsg" />

                                    </asp:Panel>

                                </td>

                                <td>
                                    <asp:CheckBox ID="chkCheckAllWards" runat="server" Visible="false" Text='<%$ Resources:Resource,L_CHECKALL%>' margin-left="10px" onClick="toggleCheckWards(this.checked);" />

                                    <asp:Panel ID="pnlWards" runat="server" ScrollBars="Auto" Height="430px" Width="130px"
                                        CssClass="panel" GroupingText='<%$ Resources:Resource, L_WARD %>'>

                                        <asp:GridView ID="gvWards" runat="server"
                                            AutoGenerateColumns="False"
                                            ShowSelectButton="True"
                                            GridLines="None"
                                            CssClass="mGrid"
                                            PagerStyle-CssClass="pgr"
                                            DataKeyNames="checked,WardId"
                                            EmptyDataText='<%$ Resources:Resource,M_NOWARDS %>'>

                                            <Columns>
                                                <asp:TemplateField HeaderText="Select">
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkWardSelect" OnCheckedChanged="FirstCellClicked" runat="server" HeaderStyle-Width="20px" Checked='<%# eval("Checked") %>' />

                                                    </ItemTemplate>
                                                    <HeaderTemplate>
                                                    </HeaderTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="">
                                                    <ItemTemplate>
                                                        <asp:HiddenField ID="hfWard" runat="server" Value='<%#Eval("WardId")%>' />
                                                    </ItemTemplate>
                                                    <HeaderTemplate>
                                                    </HeaderTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="WardName" HeaderText='<%$ Resources:Resource,L_WARD %>' SortExpression="WardName"
                                                    HeaderStyle-Width="110px">
                                                    <HeaderStyle Width="200px" />
                                                </asp:BoundField>


                                            </Columns>

                                            <PagerStyle CssClass="pgr" />
                                            <%--<AlternatingRowStyle CssClass="alt" />--%>
                                            <SelectedRowStyle CssClass="srs" />

                                        </asp:GridView>
                                        <span style="color: Red" id="errWardMsg" />


                                    </asp:Panel>

                                </td>

                                <td>
                                    <asp:CheckBox ID="chkCheckAllVillages" Visible="false" runat="server" Text='<%$ Resources:Resource,L_CHECKALL%>' margin-left="10px" onClick="toggleCheckVillages(this.checked);" />



                                    <asp:Panel ID="Panel1" runat="server" ScrollBars="Auto" Height="430px" Width="180px"
                                        CssClass="panel" GroupingText='<%$ Resources:Resource, L_VILLAGE %>'>
                                        <asp:GridView ID="gvVillage" runat="server"
                                            AutoGenerateColumns="False"
                                            ShowSelectButton="True"
                                            GridLines="None"
                                            CssClass="mGrid"
                                            PagerStyle-CssClass="pgr"
                                            DataKeyNames="checked,WardId,VillageID,Catchment,HFCatchmentId"
                                            EmptyDataText='<%$ Resources:Resource,M_NOVILLAGES %>'>

                                            <Columns>
                                                <asp:TemplateField HeaderText="Select">
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkVillageSelect" runat="server" HeaderStyle-Width="20px" Checked='<%# eval("Checked") %>' />
                                                    </ItemTemplate>
                                                    <HeaderTemplate>
                                                    </HeaderTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="">
                                                    <ItemTemplate>
                                                        <asp:HiddenField ID="hfWard" runat="server" Value='<%#Eval("WardId")%>' />
                                                    </ItemTemplate>
                                                    <HeaderTemplate>
                                                    </HeaderTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="VillageName" HeaderText='<%$ Resources:Resource,L_VILLAGE%>'
                                                    HeaderStyle-Width="110px">
                                                    <HeaderStyle Width="200px" />
                                                </asp:BoundField>
                                                <asp:TemplateField ControlStyle-Width="50px">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtCatchment" runat="server" Width="65px" Text='<%# Bind("Catchment")%>' class="numbersOnly " MaxLength="3"> </asp:TextBox>

                                                    </ItemTemplate>
                                                    <HeaderTemplate>
                                                        <asp:Label ID="lblCatchment" runat="server" Text='<%$ Resources:Resource,L_HFCATCHMENT %>'></asp:Label>
                                                    </HeaderTemplate>
                                                </asp:TemplateField>

                                            </Columns>

                                            <PagerStyle CssClass="pgr" />
                                            <%--<AlternatingRowStyle CssClass="alt" />--%>
                                            <SelectedRowStyle CssClass="srs" />

                                        </asp:GridView>
                                        <span style="color: Red" id="Span1" />


                                        </span>

                                
                                    </asp:Panel>

                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>

            </div>
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
<asp:Content ID="Content2" ContentPlaceHolderID="Footer" Runat="Server">
    <asp:Label text="" runat="server" ID="lblMsg"> </asp:Label>
<asp:ValidationSummary ID="validationSummary" runat="server" HeaderText='<%$ Resources:Resource,V_SUMMARY%>' ValidationGroup="check" />
</asp:Content>

<asp:Content ID="Content3" runat="server" contentplaceholderid="head">
    <style type="text/css">
        .auto-style1 {
            width: 150px;
            text-align: right;
            color: Blue;
            font-weight: normal;
            font-family: Verdana, Arial, Helvetica, sans-serif;
            font-size: 11px;
            padding-right: 1px;
        }
        .auto-style2 {
            font-family: Arial, Helvetica, sans-serif; /*min-width: 170px;*/;
            direction: ltr;
        }
    </style>
</asp:Content>


