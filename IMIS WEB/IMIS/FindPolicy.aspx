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
<%@ Page Language="vb" MasterPageFile="~/IMIS.Master" AutoEventWireup="false" CodeBehind="FindPolicy.aspx.vb" Inherits="IMIS.FindPolicy" 
 Title='<%$ Resources:Resource,P_FINDPOLICY %>'  %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="ajax" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Body" Runat="Server">

    <script type="text/javascript">
        $(document).ready(function () {
            $('#<%= btnSearch.ClientID %>').click(function (e) {
                var passed = true;
                $DateControls = $('.dateCheck');
                $DateControls.each(function () {
                    if ($(this).val().length > 0 && !isValidJSDate($(this).val())) {
                        $('#<%=lblMsg.ClientID%>').html('<%= imisgen.getMessage("M_INVALIDDATE", True ) %>');
                        $(this).focus();
                        passed = false;
                        return false;
                    }
                });
                if (passed == false) {
                    return false;
                }
            });
        });
    </script>
   
    
    <div class="divBody" >
    <table class="catlabel">
             <tr>
                <td >
                       <asp:Label  ID="Label8" runat="server" Text='<%$ Resources:Resource,L_SELECTCRITERIA %>'></asp:Label>   
               </td>
               
                </tr>
            
            </table>
   <asp:Panel ID="pnlTop" runat="server"  CssClass="panelTop" GroupingText='<%$ Resources:Resource,L_POLICY %>'>
     <asp:UpdatePanel ID="upDistrict" runat="server"  >    
        <Triggers>
            <asp:PostBackTrigger ControlID="btnSearch" />
            <asp:PostBackTrigger ControlID="chkLegacy" />
            <asp:PostBackTrigger ControlID="chkOffline" />
        </Triggers>                             
        <ContentTemplate> 
            <table>
                <tr>
                    <td>
            <table>
               <tr>
               <td class="FormLabel">
                            <asp:Label ID="L_EnrolmentDateFrom" runat="server" Text='<%$ Resources:Resource,L_ENROLMENTDATEFROM %>'></asp:Label>
                        </td>
                <td class ="DataEntry">
                    <asp:TextBox 
                        ID="txtEnrolmentDateFrom" 
                        runat="server" 
                        Width="85px" CssClass="dateCheck"></asp:TextBox >
                    <asp:Button  
                        ID="btnEnrollDateFrom" 
                        runat="server" 
                       Height="18px" 
                        Width="18px"
                        padding-bottom="3px" />
                        
                    <ajax:CalendarExtender 
                        ID="CalendarExtender2" 
                        runat="server" 
                        TargetControlID="txtEnrolmentDateFrom" 
                        Format="dd/MM/yyyy" 
                        PopupButtonID="btnEnrollDatefrom" ClearTime="True" >
                    </ajax:CalendarExtender>
                </td>
                    <td class="FormLabel">
                        <asp:Label ID="lblEffectiveDateFrom" runat="server" Text='<%$ Resources:Resource,L_EFFECTIVEDATEFROM %>'></asp:Label></td>
                <td class ="DataEntry">
                     <asp:TextBox ID="txtEffectiveDateFrom" runat="server" Width="85px" CssClass="dateCheck"></asp:TextBox >
                    <asp:Button ID="btnEffectiveDateFrom" runat="server" Height="18px" Width="18px" padding-bottom="3px" />
                        
                    <ajax:CalendarExtender 
                        ID="CalendarExtender1" 
                        runat="server" 
                        TargetControlID="txtEffectiveDateFrom" 
                        Format="dd/MM/yyyy" 
                        PopupButtonID="btnEffectiveDateFrom" >
                    </ajax:CalendarExtender>
                    
                 </td>
              <%--Date one--%>
                       <td class ="FormLabel">
                           <asp:Label ID="lblRegion" runat="server" Text="<%$ Resources:Resource,L_REGION %>"></asp:Label>
                   </td>
                <td class ="DataEntry">
                    <asp:DropDownList ID="ddlRegion" runat="server" AutoPostBack="True">
                    </asp:DropDownList>
                   </td>
            </tr>
               <tr>
                 <td class="FormLabel">
                            <asp:Label ID="L_ENROLMENTDATETO" runat="server" Text='<%$ Resources:Resource,L_ENROLMENTDATETO %>'></asp:Label>
                        </td>
                <td class ="DataEntry">
                    <asp:TextBox 
                        ID="txtEnrolmentDateTo" 
                        runat="server" 
                        Width="85px" CssClass="dateCheck"></asp:TextBox >
                    <asp:Button  
                        ID="btnEnrollDateTo" 
                        runat="server" 
                       Height="18px" 
                        Width="18px"
                        padding-bottom="3px" />
                        
                    <ajax:CalendarExtender 
                        ID="CalendarExtender5" 
                        runat="server" 
                        TargetControlID="txtEnrolmentDateTo" 
                        Format="dd/MM/yyyy" 
                        PopupButtonID="btnEnrollDateTo" >
                    </ajax:CalendarExtender>
                </td>
               <td class="FormLabel">
                        <asp:Label ID="lblEffectiveDateTo" runat="server" Text='<%$ Resources:Resource,L_EFFECTIVEDATETO %>'></asp:Label></td>
                <td class ="DataEntry">
                     <asp:TextBox ID="txtEffectiveDateTo" runat="server" Width="85px" CssClass="dateCheck"></asp:TextBox >
                    <asp:Button ID="btnEffectiveDateTo" runat="server" Height="18px" Width="18px" padding-bottom="3px" />
                        
                    <ajax:CalendarExtender 
                        ID="CalendarExtender6" 
                        runat="server" 
                        TargetControlID="txtEffectiveDateTo" 
                        Format="dd/MM/yyyy" 
                        PopupButtonID="btnEffectiveDateTo" >
                    </ajax:CalendarExtender>
                    
                 </td>
                  
                    <td class="FormLabel" >
                        <asp:Label ID="lblDistrict" runat="server" Text="<%$ Resources:Resource,L_DISTRICT %>"></asp:Label>
                </td>
                <td >
                    <asp:DropDownList ID="ddlDistrict" runat="server" AutoPostBack="True">
                    </asp:DropDownList>
                </td>
                        
                 <%--<td class="FormLabel">
                        &nbsp;</td>
                <td>
                    &nbsp;</td>--%>
              
                        </tr>
               <tr>
                        <td class ="FormLabel">
                    <asp:Label ID="lblStartDateFrom" runat="server" Text='<%$ Resources:Resource,L_STARTDATEFrom %>'></asp:Label>
                </td>
                <td class ="DataEntry">
                    <asp:TextBox ID="txtStartDateFrom" runat="server" Width="85px" CssClass="dateCheck"></asp:TextBox >
                    <asp:Button  
                        ID="btnStartDateFrom" 
                        runat="server" 
                      Height="18px" 
                        Width="18px"
                        padding-bottom="3px" />
                        
                    <ajax:CalendarExtender 
                        ID="CalendarExtender4" 
                        runat="server" 
                        TargetControlID="txtStartDateFrom" 
                        Format="dd/MM/yyyy" 
                        PopupButtonID="btnStartDateFrom" >
                    </ajax:CalendarExtender>
                   </td>
                       <td class="FormLabel">
                        <asp:Label ID="lblExpiryDateFrom" runat="server" Text='<%$ Resources:Resource,L_EXPIRYDATEFROM %>'></asp:Label></td>
                <td class ="DataEntry">
                     <asp:TextBox ID="txtExpiryDateFrom" runat="server" Width="85px" CssClass="dateCheck"></asp:TextBox >
                    <asp:Button ID="btnExpiryDateFrom" runat="server" CssClass="dateButton"   padding-bottom="3px" />
                        
                    <ajax:CalendarExtender 
                        ID="CalendarExtender3" 
                        runat="server" 
                        TargetControlID="txtExpiryDateFrom" 
                        Format="dd/MM/yyyy" 
                        PopupButtonID="btnExpiryDateFrom" >
                    </ajax:CalendarExtender>
                    
                 </td>        
                   <td class="FormLabel" >
                   &nbsp;
                       <asp:Label ID="lblType" runat="server" Text="<%$ Resources:Resource,L_TYPE %>"></asp:Label>
                </td>
                <td >
                   &nbsp;
                    <asp:DropDownList ID="ddlType" runat="server" AutoPostBack="false">
                    </asp:DropDownList>
                    </td>                   
                        </tr>
               <tr>
                    <td class ="auto-style1">
                    <asp:Label ID="lblStartDateTo" runat="server" Text='<%$ Resources:Resource,L_STARTDATETO %>'></asp:Label>
                </td>
                <td class ="DataEntry">
                    <asp:TextBox ID="txtStartDateTo" runat="server" Width="85px" CssClass="dateCheck"></asp:TextBox >
                    <asp:Button  
                        ID="btnStartDateTo" 
                        runat="server" 
                      Height="18px" 
                        Width="18px"
                        padding-bottom="3px" />
                        
                    <ajax:CalendarExtender 
                        ID="CalendarExtender8" 
                        runat="server" 
                        TargetControlID="txtStartDateTo" 
                        Format="dd/MM/yyyy" 
                        PopupButtonID="btnStartDateTo" >
                    </ajax:CalendarExtender>
                   </td>
                   <td class="auto-style1">
                        <asp:Label ID="lblExpiryDateTo" runat="server" Text='<%$ Resources:Resource,L_EXPIRYDATETO %>'></asp:Label></td>
                <td class ="DataEntry">
                     <asp:TextBox ID="txtExpiryDateTo" runat="server" Width="85px" CssClass="dateCheck"></asp:TextBox >
                    <asp:Button ID="btnExpiryDateTo" runat="server" CssClass="dateButton"   padding-bottom="3px" />
                        
                    <ajax:CalendarExtender 
                        ID="CalendarExtender7" 
                        runat="server" 
                        TargetControlID="txtExpiryDateTo" 
                        Format="dd/MM/yyyy" 
                        PopupButtonID="btnExpiryDateTo" >
                    </ajax:CalendarExtender>
                    
                 </td> 
                 <td class ="auto-style1">
                     &nbsp;</td>
                   <td class ="DataEntry">
                       <asp:CheckBox ID="chkDeactivatedPolicies" runat="server" class="checkbox" Text="<%$ Resources:Resource,L_INACTIVEADHERENTS %>" />
                   </td>                 
                        </tr>
                <tr>
                      <td class="FormLabel">
                            <asp:Label 
                            ID="lblEnrolmentOfficers"
                            runat="server" 
                            Text='<%$ Resources:Resource,L_ENROLMENTOFFICERS %>'></asp:Label>
                        </td>
                <td class ="DataEntry">
                     <asp:DropDownList ID="ddlEnrolmentOfficers" runat="server">
                    </asp:DropDownList>
                    </td>
                
                 <td class="FormLabel">
                        <asp:Label 
                        ID="lblProduct"
                        runat="server" 
                        Text='<%$ Resources:Resource,L_PRODUCT %>'></asp:Label></td>
                 <td>
                    <asp:DropDownList ID="ddProduct" runat="server">
                    </asp:DropDownList></td>
                     <td class="FormLabel">
                         <asp:Label ID="lblBalance" runat="server" text="<%$ Resources:Resource,L_BALANCE %>"></asp:Label>
                 </td>
                 <td class="DataEntry">
                     <asp:TextBox ID="txtBalance" runat="server"></asp:TextBox>
                 </td>      
                </tr>
                <tr>
                    <td class="FormLabel">
                        <asp:Label ID="lblPolicyStatus" runat="server" Text="<%$ Resources:Resource,L_POLICYSTATUS %>"></asp:Label>
                    </td>
                    <td class="DataEntry">
                        <asp:DropDownList ID="ddlPolicyStatus" runat="server">
                        </asp:DropDownList>
                    </td>
                    <td class="FormLabel">&nbsp;</td>
                    <td>&nbsp;</td>
                    <td class="FormLabel">&nbsp;</td>
                    <td class="DataEntry">&nbsp;</td>
                </tr>
            </table>
         
            </td>
            <td>
                    <asp:CheckBox class="checkbox" ID="chkOffline" runat="server" Text='<%$ Resources:Resource,L_OFFLINE %>' AutoPostBack="True" />
                    <br />
                    <br />
                     <asp:CheckBox class="checkbox" ID="chkLegacy" runat="server" Text='<%$ Resources:Resource,L_ALL %>' AutoPostBack="true" />
                      <br />
                      <br />
                   <asp:Button class="button" ID="btnSearch" runat="server" 
                          Text='<%$ Resources:Resource,B_SEARCH %>' >
                  </asp:Button>
                  <br />
                  
                </td>
            </tr>
            </table>
         </ContentTemplate>      
     </asp:UpdatePanel>        
        </asp:Panel>
    <table class="catlabel">
             <tr>
                <td >
                       <asp:label  
                           ID="L_FOUNDPOLICY"  
                           runat="server" 
                           Text='<%$ Resources:Resource,L_POLICIESFOUND %>'>
                       </asp:label>   
               </td>
               
                </tr>
            
            </table>
    <asp:Panel ID="pnlBody" runat="server"  CssClass="panelBody" 
            style="overflow-y:auto;">
                     <asp:GridView ID="gvPolicies" runat="server" Width="100%"
            AutoGenerateColumns="False"
            GridLines="None"
          AllowPaging="True" PagerSettings-FirstPageText = "First Page" 
                         PagerSettings-LastPageText = "Last Page" PagerSettings-Mode ="NumericFirstLast" PageSize="15"
            CssClass="mGrid"
            EmptyDataText='<%$ Resources:Resource,M_NOPOLICIES %>'
            PagerStyle-CssClass="pgr" 
            AlternatingRowStyle-CssClass="alt"
            SelectedRowStyle-CssClass="srs" DataKeyNames="FamilyID,PolicyID" >
                <Columns>
                    <asp:CommandField 
                        HeaderStyle-CssClass="HideButton" ItemStyle-CssClass="HideButton" 
                        SelectText="Select" ShowSelectButton="true">
                        <HeaderStyle CssClass="HideButton" />
                        <ItemStyle CssClass="HideButton" />
                    </asp:CommandField>
                    <asp:HyperLinkField DataNavigateUrlFields="FamilyID,PolicyID" 
                        DataNavigateUrlFormatString="OverViewFamily.aspx?f={0}&po={1}" 
                        DataTextField="EnrollDate" DataTextFormatString="{0:d}" 
                       HeaderText='<%$ Resources:Resource,L_ENROLDATE %>' 
                        HeaderStyle-Width ="60px">                      
                        <HeaderStyle Width="60px" />
                    </asp:HyperLinkField>
                    <asp:BoundField DataField="FamilyName" HeaderText='<%$ Resources:Resource,L_NAME %>' 
                        SortExpression="FamilyName"  HeaderStyle-Width ="170px"> 
                        <HeaderStyle Width="170px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="EffectiveDate" HeaderText='<%$ Resources:Resource,L_EFFECTIVEDATE %>'  SortExpression="EffectiveDate" DataFormatString="{0:d}" HeaderStyle-Width="60px" >
                        <HeaderStyle Width="60px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="StartDate" HeaderText='<%$ Resources:Resource,L_STARTDATE %>' SortExpression="StartDate" DataFormatString="{0:d}" HeaderStyle-Width="60px" >
                        <HeaderStyle Width="60px" />
                    </asp:BoundField>                    
                    <asp:BoundField DataField="ExpiryDate" HeaderText='<%$ Resources:Resource,L_EXPIRYDATE %>'
                        SortExpression="ExpiryDate" DataFormatString="{0:d}">
                        <HeaderStyle Width="60px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="ProductCode" HeaderText='<%$ Resources:Resource,L_PRODUCT %>' 
                        SortExpression="ProductCode" HeaderStyle-Width ="60px"> 
                        <HeaderStyle Width="60px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="OfficerName" HeaderText='<%$ Resources:Resource,L_ENROLMENTOFFICERS %>'
                        SortExpression="OfficerName" HeaderStyle-Width ="230px"> 
                        <HeaderStyle Width="230px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="PolicyStatus" HeaderText='<%$ Resources:Resource,L_POLICYSTATUS %>' SortExpression="PolicyStatus" HeaderStyle-Width="50px" >
                        <HeaderStyle Width="50px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="PolicyValue" HeaderText='<%$ Resources:Resource,L_POLICYVALUE %>' SortExpression="PolicyValue" DataFormatString="{0:n2}" HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="Right">
                        <HeaderStyle Width="60px" />
                        <ItemStyle HorizontalAlign="Right" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Balance" HeaderText='<%$ Resources:Resource,L_BALANCE %>' SortExpression="Balance" DataFormatString="{0:n2}"  HeaderStyle-Width="60px"  ItemStyle-HorizontalAlign="Right">
                        <HeaderStyle Width="60px" />
                        <ItemStyle HorizontalAlign="Right" />
                    </asp:BoundField>
                    <asp:BoundField DataField="PolicyStage" HeaderText="<%$ Resources:Resource,L_TYPE %>" >
                       <ItemStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                <asp:BoundField DataField="ValidityFrom" DataFormatString="{0:d}" HeaderText='<%$ Resources:Resource,L_VALIDFROM %>' SortExpression="ValidityFrom" HeaderStyle-Width="60px">  
                    <HeaderStyle Width="60px" />
                    </asp:BoundField>
                <asp:BoundField DataField="ValidityTo" DataFormatString="{0:d}" HeaderText='<%$ Resources:Resource,L_VALIDTO %>' SortExpression="DOB" HeaderStyle-Width="60px">  
                    <HeaderStyle Width="60px" />
                    </asp:BoundField>
                
                
                </Columns>
                         <PagerStyle CssClass="pgr" />
                         <SelectedRowStyle CssClass="srs" />
                         <AlternatingRowStyle CssClass="alt" />
                         <PagerSettings FirstPageText="First Page" LastPageText="Last Page" 
                             Mode="NumericFirstLast" />
                         <RowStyle CssClass="normal" Wrap="False" />
    </asp:GridView>
                     </asp:Panel>
    </div>
     <asp:Panel ID="pnlButtons" runat="server"   CssClass="panelbuttons" >
        <table width="100%" cellpadding="10 10 10 10">
             <tr>
                    
                     <td  align="left">
                    <%--<asp:Button 
                        ID="B_ADD" 
                        runat="server" 
                        Text='<%$ Resources:Resource,B_ADD%>'
                          />--%>
                    </td>                   
                    
                    <td align="center">
                    
                <asp:Button 
                     Visible ="false"
                    ID="B_VIEW" 
                    runat="server" 
                    Text='<%$ Resources:Resource,B_VIEW%>'
                      />
                </td>
                <td>
                   <%-- <asp:Button 
                        
                        ID="B_EDIT" 
                        runat="server" 
                        Text='<%$ Resources:Resource,B_EDIT%>'
                        ValidationGroup="check"  />--%>
                    </td>
                       <td align="right">
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
            height: 27px;
            width: 150px;
            text-align: right;
            color: Blue;
            font-weight: normal;
            font-family: Verdana, Arial, Helvetica, sans-serif;
            font-size: 11px;
            padding-right: 1px;
        }
    </style>
</asp:Content>

