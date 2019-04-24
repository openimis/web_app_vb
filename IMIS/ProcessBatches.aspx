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
<%@ Page Language="vb" AutoEventWireup="false"  MasterPageFile="~/IMIS.Master" CodeBehind="ProcessBatches.aspx.vb" Inherits="IMIS.ProcessRelIndex" 
title='<%$ Resources:Resource,L_PROCESSPAGETITLE%>' EnableEventValidation="false" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="ajax" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="head" Runat="Server">

<script type="text/javascript" language="javascript">
    
    
    function ProcessFn(btn) {
        if (btn == "ok") {
                     __doPostBack("<%=btnProcess.ClientID %>", "");
        } else if (btn == "cancel") {
            return false;
        }
    }
    
    $(document).ready(function() {
        $('#<%=hfPeriod.ClientID %>').val($('#<%=ddlMonthFilter.ClientID %>').val());
    });
    
    function pageLoadExtend() {
        $("#<%=btnProcess.ClientID %>").click(function () {
            var htmlMsg1 = '<%= imisgen.getMessage("M_ARESURETOPROCESSBATCH", True)%>' + "<br>";
            
            var LocationName = "";
           
            var Region = $('#<%=ddlRegionBatch.ClientID%>').val();
            var $ddlRegion=$('#<%=ddlRegionBatch.ClientID %>');
            var RegionName = $ddlRegion.find("option").eq($ddlRegion[0].selectedIndex).html();
            var DistrictId =  $('#<%=ddlDistrictsBatch.ClientID%>').val();
            var $ddlDistrict = $('#<%=ddlDistrictsBatch.ClientID %>');
            var DistrName = $ddlDistrict.find("option").eq($ddlDistrict[0].selectedIndex).html();
            var $ddlMonth = $('#<%=ddlMonthProcess.ClientID %>');
            var Month = $ddlMonth.find("option").eq($ddlMonth[0].selectedIndex).html();
            var $ddlYear = $('#<%=ddlYearProcess.ClientID %>');
            var Year = $ddlYear.find("option").eq($ddlYear[0].selectedIndex).html();
            
            if (DistrictId > 0) {
                LocationName=DistrName
            }
            else {
                LocationName=RegionName
            }
            var htmlMsg = htmlMsg1 + LocationName + " " + Month + " " + Year;
            
            var MonthId = $('#<%=ddlMonthProcess.ClientID%>').val();
            var YearId = $('#<%=ddlYearProcess.ClientID%>').val();

            
          
            if (Region != 0 && MonthId>0 && YearId > 0) {
                popup.acceptBTN_Text = '<%=imisgen.getMessage("L_YES", True)%>';
                popup.rejectBTN_Text = '<%=imisgen.getMessage("L_NO", True ) %>';
                popup.confirm(htmlMsg, ProcessFn);
            }
            return false;
        });

       
        $('#<%=ddlMonthFilter.ClientID %>').change(function() {
            $('#<%=hfPeriod.ClientID %>').val($('#<%=ddlMonthFilter.ClientID %>').val());
        });
        
//        $('#<%=btnPreview.ClientID %>').click(function() {
//            if ($('#<%=ddlBatchAAC.ClientID %>').val() > 0) {
//                return true;
//            } else {
//                if ($('#<%=txtSTARTData.ClientID %>').val() > 0) {
//                    return true;
//                } else {
//                    return false;
//                }
//            }
//        });


    }
    
</script>

<style type="text/css">

table{width:100%;}
table td.DataEntry{text-align:left;}
table td.FormLabel{text-align:right;}
table tr td.DataEntry{width:auto;}
table tr td.FormLabel{width:auto;}
#process-table td.Empty{width:540px;}
.Month{width:90px;}
.Year{width:60px;}

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

    .auto-style2 {
        font-family: Arial, Helvetica, sans-serif; /*min-width: 170px;*/
        height: 27px;
        direction: ltr;
        width: 151px;
    }
    .auto-style3 {
        height: 23px;
        width: 151px;
        text-align: right;
        color: Blue;
        font-weight: normal;
        font-family: Verdana, Arial, Helvetica, sans-serif;
        font-size: 11px;
        padding-right: 1px;
    }

    .auto-style4 {
        width: 6px;
    }

</style>

</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="Body" Runat="Server">
  <div class="divBody" >
      <asp:HiddenField ID="hfPeriod" runat="server" Value="0" />
     
        <table class="catlabel">
            <tr>
                <td >
                   <asp:label  
                           ID="L_PROCESS"  
                           runat="server" 
                           Text='<%$ Resources:Resource,L_PROCESS %>'></asp:label>   
                    
                </td>
            </tr>
        </table>
        <asp:Panel ID="pnlTop" runat="server"  CssClass="panel" GroupingText="" oncontextmenu="return false;">
         
           <table>
            <tr>
                <td>
                   <table id="CriteriaTableProcess" class="CriteriaTable">
                        <tr>
                            <td  class="FormLabel">

                                <asp:Label ID="L_REGION" runat="server" Text="<%$ Resources:Resource,L_REGION %>"></asp:Label>

                            </td>
                             <td>

                                 <asp:DropDownList ID="ddlRegionBatch" runat="server" AutoPostBack="true">
                                 </asp:DropDownList> 
                            </td>
                            <td class="auto-style4">
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                            ControlToValidate="ddlRegionBatch" SetFocusOnError="True" ValidationGroup="proc" InitialValue="0" ForeColor="Red" Display="Dynamic"
                                        Text='*'></asp:RequiredFieldValidator>
                        </td>
                        <td class="FormLabel">   
                            <asp:Label ID="Label2" runat="server"  Text='<%$ Resources:Resource,L_DISTRICT%>'></asp:Label> 
                        </td>
                        <td>  
                            <asp:DropDownList  ID="ddlDistrictsBatch" runat="server" AutoPostBack="true"> </asp:DropDownList> 
                        </td>
                        
                        <td class="FormLabel">   
                            <asp:Label ID="lblMonthProcess" runat="server"  Text='<%$ Resources:Resource,L_MONTH%>'></asp:Label> 
                        </td>
                        <td >  
                            <asp:DropDownList class="Month" ID="ddlMonthProcess" runat="server"> </asp:DropDownList>  
                            
                        </td>
                        <td>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                                    ControlToValidate="ddlMonthProcess" 
                                ValidationGroup="proc" InitialValue="0" ForeColor="Red" Display="Dynamic"
                                        Text='*'></asp:RequiredFieldValidator>
                        </td>
                            <td class="FormLabel">   
                                <asp:Label ID="lblYearProcess" runat="server" Text='<%$ Resources:Resource,L_YEAR%>'></asp:Label> 
                            </td>
                        <td >  
                                <asp:DropDownList class="Year" ID="ddlYearProcess" runat="server" Width="70px" > </asp:DropDownList> 
                                
                        </td>
                         <td>
                         <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" 
                                    ControlToValidate="ddlYearProcess" ValidationGroup="proc" 
                                    InitialValue="--Year--" ForeColor="Red" Display="Dynamic"
                                        Text='*'></asp:RequiredFieldValidator>
                         </td>
                            <td class="FormLabel Empty">   </td>
     
                            
                            <td align="right"> <asp:Button ID="btnProcess" runat="server" class="btnCriteria" 
                                    Text='<%$ Resources:Resource,B_PROCESS %>' ValidationGroup="proc" /> </td>
                        </tr>
            
                  </table>
                </td>
            </tr>
        </table>
        </asp:Panel>
        
        <table class="catlabel">
            <tr>
                <td >
                   <asp:label  
                           ID="L_FILTERFORRELINDEX"  
                           runat="server" 
                           Text='<%$ Resources:Resource,L_FILTER %>' ></asp:label>   
                    
                </td>
            </tr>
        </table>
        <asp:Panel ID="pnlMiddle" runat="server"  CssClass="panel" height="60px" GroupingText="" oncontextmenu="return false;">
             <asp:UpdatePanel ID="upDistrictRelIndex" runat="server"  > 
                    <Triggers>
                          <asp:PostBackTrigger ControlID="btnFilter" />
                    </Triggers>
                    <ContentTemplate>
                        <table align="center">
            <tr align="center">
                <td>
                   <table align="left" id="CriteriaTableRelIndex" class="CriteriaTable">
         
                        <tr >
                           
                            <td class="auto-style1">
                                <asp:Label ID="lblPeriod" runat="server" 
                                    Text='<%$ Resources:Resource,L_TYPE%>'  Width="30px"></asp:Label>
                            </td>
                            <td class ="DataEntry">
                                <asp:DropDownList ID="ddlPeriod" runat="server" width="150px" AutoPostBack="true"> </asp:DropDownList>
                            </td>
                           
                            <td class="auto-style1">
                                <asp:Label ID="lblYearFilter" runat="server" 
                                    Text='<%$ Resources:Resource,L_YEAR%>' ></asp:Label>
                            </td>
                            <td class ="DataEntry"  >
                                <asp:DropDownList class="Year" ID="ddlYearFilter" runat="server" width="150px" > </asp:DropDownList>
                            </td>
                            <td class="auto-style1">
                                <asp:Label ID="lblMonthFilter" runat="server" 
                                    Text='<%$ Resources:Resource,L_PERIOD%>'></asp:Label>
                            </td>
                            <td class ="DataEntry">
                                <asp:DropDownList class="Month" width="150px" ID="ddlMonthFilter" runat="server"> </asp:DropDownList>
                            </td>
                            </tr>
                            <tr>
                                <td class="FormLabel">
                                    <asp:Label ID="L_REGIONFILTER" runat="server" Text="<%$ Resources:Resource,L_REGION %>"></asp:Label>
                                </td>
                                <td class ="DataEntry">
                                    <asp:DropDownList ID="ddlRegionFilter" runat="server" AutoPostBack="True">
                                    </asp:DropDownList>
                                </td>
                             <td class="FormLabel">
                                <asp:Label ID="L_DISTRICTREL" runat="server" 
                                    Text='<%$ Resources:Resource,L_DISTRICT%>'></asp:Label>
                            </td>
                            <td class ="DataEntry">
                                <asp:DropDownList ID="ddlDistrictFilter" runat="server" 
                                    AutoPostBack="True"> </asp:DropDownList>
                            </td>
                             
                            <td class="FormLabel">
                                <asp:Label ID="lblProductFilter" runat="server" 
                                    Text='<%$ Resources:Resource,L_PRODUCT %>'></asp:Label>
                            </td>
                            <td class ="DataEntry">
                                <asp:DropDownList ID="ddlProductFilter" runat="server"> </asp:DropDownList>
                            </td>
                            <td class="FormLabel">
                                <asp:Label ID="lblHFLevelFilter" runat="server" 
                                    Text='<%$ Resources:Resource,L_HFLEVEL%>'></asp:Label>
                            </td>
                            <td class ="DataEntry">
                                <asp:DropDownList ID="ddlHFLevelFilter" runat="server"> </asp:DropDownList>
                            </td>
                            <td align="right">
                                <asp:Button ID="btnFilter" runat="server" Text='<%$ Resources:Resource,L_FILTER %>'  class="btnCriteria" />
                            </td>
                        </tr>  
                    </table>
                </td>
            </tr>
        </table>
                        <div id="divPopupScript" runat="server" style="display:none;"></div>
                    </ContentTemplate>
             </asp:UpdatePanel>
        </asp:Panel>
        
        <table class="catlabel">
             <tr>
                <td >
                       <asp:label  
                           ID="L_DISPLAY"  
                           runat="server" 
                           Text='<%$ Resources:Resource,L_DISPLAY %>'></asp:label>   
               </td>
               
                </tr>
            
            </table>
        <asp:Panel ID="pnlBody" runat="server"  CssClass="panelBody" Height="130px" 
          ScrollBars="Vertical" >
            <asp:GridView ID="gvRelIndex" runat="server" 
                AutoGenerateColumns="False"
                GridLines="None"
                AllowPaging="False"
                CssClass="mGrid"
                EmptyDataText='<%$ Resources:Resource,M_NORESULTSFOUND %>'
                PagerStyle-CssClass="pgr"
                
                AlternatingRowStyle-CssClass="alt"
                SelectedRowStyle-CssClass="srs" PageSize="6" RowStyle-Wrap="False">
                <Columns>
                  <asp:BoundField DataField="RelYear"  HeaderText='<%$ Resources:Resource,L_YEAR %>' SortExpression="" HeaderStyle-Width="70px">  
                    <HeaderStyle Width="70px" />
                    </asp:BoundField>
                  <asp:BoundField DataField="RelPeriod"  HeaderText='<%$ Resources:Resource,L_PERIOD %>' SortExpression="" HeaderStyle-Width="70px">  
                    <HeaderStyle Width="70px" />
                    </asp:BoundField>               
                  <asp:BoundField DataField="ProductName"  HeaderText='<%$ Resources:Resource,L_PRODUCT %>' SortExpression="" HeaderStyle-Width="70px">  
                    <HeaderStyle Width="70px" />
                    </asp:BoundField>                
                  <asp:BoundField DataField="RelCareType"  HeaderText='<%$ Resources:Resource,L_HFLEVEL %>' SortExpression="" HeaderStyle-Width="70px">  
                    <HeaderStyle Width="70px" />
                    </asp:BoundField>
                  <asp:BoundField DataField="CalcDate" DataFormatString="{0:d}" HeaderText='<%$ Resources:Resource,L_CALCDATE %>' SortExpression="" HeaderStyle-Width="70px">  
                    <HeaderStyle Width="70px" />
                    </asp:BoundField>
                  <asp:BoundField DataField="RelIndex"  DataFormatString="{0:N2}" 
                        HeaderText='<%$ Resources:Resource,L_INDEX %>' SortExpression="" 
                        HeaderStyle-Width="70px" ItemStyle-HorizontalAlign="Right">  
                    <HeaderStyle Width="70px" />
                    <ItemStyle HorizontalAlign="Right" />
                    </asp:BoundField>
                </Columns>
                <PagerStyle CssClass="pgr" />
                <SelectedRowStyle CssClass="srs" />
                <AlternatingRowStyle CssClass="alt" />
                <RowStyle CssClass="normal" />
            </asp:GridView>
        </asp:Panel>
           <table class="catlabel">
             <tr>
                <td >
                       <asp:label  
                           ID="Label1"  
                           runat="server" 
                           Text='<%$ Resources:Resource,L_FILTERACCOUNTS %>'></asp:label>   
               </td>
               
                </tr>
            
            </table>
            
               <asp:Panel ID="Panel1" runat="server"  CssClass="panelBody" Height="150px" oncontextmenu="return false;">
               <asp:UpdatePanel ID="upDistrictAccount" runat="server"  > 
                    <Triggers>
                          <asp:PostBackTrigger ControlID="btnPreview" />
                    </Triggers>
                    <ContentTemplate>
                        <table>
            <tr>
                <td>
                    <table id="CriteriaTableAccount" class="CriteriaTable">
                        <tr>
                            <td colspan="2">
                                <fieldset style="padding-left: 10px; width: 250px">
                                    <legend>Group By</legend>

                                    <asp:RadioButton CssClass="checkbox" ID="rbHF" runat="server" GroupName="groupby" Checked="true" TextAlign="Right" Style="padding-bottom: 1px; padding-top: 1px" Width="100px" Text='<%$ Resources:Resource,L_HF %>' />
                                    <asp:RadioButton CssClass="checkbox" ID="rbProduct" runat="server" GroupName="groupby" Checked="false" TextAlign="Right" Style="padding-bottom: 1px; padding-top: 1px" Width="100px" Text='<%$ Resources:Resource,L_PRODUCT%>' />

                                </fieldset>
                            </td>
                            <td class="FormLabel" colspan="2" style="text-align:left;color:maroon;font-weight:bold;">
                                <asp:CheckBox ID="chkClaims" runat="server" Text='<%$ Resources:Resource,L_SHOWCLAIM%>' />
                            </td>
                        </tr>
                        <tr>

                            <td class="FormLabel">
                                <asp:Label ID="L_REGIONACC" runat="server" Text="<%$ Resources:Resource,L_REGION %>"></asp:Label>
                               
                            </td>
                            <td class="DataEntry">
                               <asp:DropDownList ID="ddlRegionACC" runat="server" AutoPostBack="true">
                                </asp:DropDownList></td>
                            <td class="FormLabel">
                                <asp:Label ID="Label6" runat="server"
                                    Text='<%$ Resources:Resource,L_PRODUCT %>'></asp:Label>
                            </td>
                            <td class="DataEntry">
                                <asp:DropDownList ID="ddlProductAAC" runat="server"></asp:DropDownList>
                            </td>
                            <td class="FormLabel">
                                <asp:Label ID="lblHFCode" runat="server" Text='<%$ Resources:Resource,L_HF %>'></asp:Label>
                            </td>
                            <td class="DataEntry">
                                <asp:DropDownList ID="ddlHF" runat="server"></asp:DropDownList>
                            </td>
                            <td class="FormLabel">
                                <asp:Label ID="Label7" runat="server"
                                    Text='<%$ Resources:Resource,L_LEVEL%>'></asp:Label>
                            </td>
                            <td class="auto-style2">
                                <asp:DropDownList ID="ddlHFLevel" runat="server"></asp:DropDownList>
                            </td>
                            <td class="FormLabel" align="right">
                                <asp:CheckBox ID="chkShowAll" runat="server"
                                    Text='<%$ Resources:Resource,L_SHOWALL%>'></asp:CheckBox>
                            </td>





                        </tr>
                        <tr>
                            <td class="FormLabel">
                                <asp:Label ID="L_District" runat="server" Text="<%$ Resources:Resource,L_DISTRICT %>"></asp:Label>
                            </td>
                            <td class="DataEntry">
                                <asp:DropDownList ID="ddlDistrictACC" runat="server" AutoPostBack="true">
                                </asp:DropDownList>
                            </td>


                            <td class="FormLabel">
                                <asp:Label ID="Label8" runat="server" Text="<%$ Resources:Resource,L_BATCH%>"></asp:Label>
                            </td>
                            <td class="DataEntry">
                                <asp:DropDownList ID="ddlBatchAAC" runat="server" class="Month" width="150px">
                                </asp:DropDownList>
                            </td>
                            <td class="FormLabel">
                                <asp:Label ID="lblSTART0" runat="server" Text="<%$ Resources:Resource,L_DATEFROM %>"></asp:Label>
                            </td>
                            <td class="DataEntry">
                                <asp:TextBox ID="txtSTARTData" runat="server" size="10" Width="120px"></asp:TextBox>
                                <ajax:CalendarExtender ID="txtSTARTData_CalendarExtender" runat="server" Format="dd/MM/yyyy" PopupButtonID="btnSTARTData" TargetControlID="txtSTARTData">
                                </ajax:CalendarExtender>
                                <ajax:MaskedEditExtender ID="txtSTARTData_MaskedEditExtender" runat="server" CultureDateFormat="dd/MM/YYYY" Mask="99/99/9999" MaskType="Date" TargetControlID="txtSTARTData" UserDateFormat="DayMonthYear">
                                </ajax:MaskedEditExtender>
                                <asp:Button ID="btnSTARTData" runat="server" class="dateButton" padding-bottom="3px" />
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator" runat="server" ControlToValidate="txtSTARTData" ErrorMessage="*" SetFocusOnError="True" ValidationExpression="^(0[1-9]|[12][0-9]|3[01])[/](0[1-9]|1[012])[/](19|20)\d\d$" ValidationGroup="check"></asp:RegularExpressionValidator>
                                <%-- <asp:RequiredFieldValidator ID="txtSTARTData_RequiredFieldValidator" 
                       runat="server" ErrorMessage="*" ControlToValidate="txtSTARTData" 
                       ValidationGroup="check" Visible="True"></asp:RequiredFieldValidator>--%>
                            </td>
                            <td class="FormLabel" align="right">
                                <asp:Label ID="lblEND" runat="server" Text="<%$ Resources:Resource,L_DATETO %>"></asp:Label>
                            </td>
                            <td class="auto-style3" align="right">
                                <asp:TextBox ID="txtENDData" runat="server" Size="10" Width="120px">
                                </asp:TextBox>
                                <asp:Button ID="btnENDData" runat="server" class="dateButton" />
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server" ControlToValidate="txtENDData" ErrorMessage="*" SetFocusOnError="True" ValidationExpression="^(0[1-9]|[12][0-9]|3[01])[/](0[1-9]|1[012])[/](19|20)\d\d$" ValidationGroup="check" ForeColor="Red" Display="Dynamic">
                                </asp:RegularExpressionValidator>
                                <ajax:CalendarExtender ID="txtENDData_CalendarExtender" runat="server" Format="dd/MM/yyyy" PopupButtonID="btnENDData" TargetControlID="txtENDData">
                                </ajax:CalendarExtender>
                                <ajax:MaskedEditExtender ID="txtENDData_MaskedEditExtender" runat="server" CultureDateFormat="dd/MM/YYYY" Mask="99/99/9999" MaskType="Date" TargetControlID="txtENDData" UserDateFormat="DayMonthYear">
                                </ajax:MaskedEditExtender>
                            </td>

                            <td align="right">
                                <asp:Button ID="btnPreview" runat="server"
                                    Text='<%$ Resources:Resource,B_PREVIEW %>' ValidationGroup="check" class="btnCriteria" />
                            </td>
                        </tr>

                    </table>
                </td>
            </tr>
        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
        
        </asp:Panel>
     
        </div>
       <asp:Panel ID="pnlButtons" runat="server"   CssClass="panelbuttons" >
        <table width="100%" cellpadding="10 10 10 10" align="center">
             <tr align="center">
                 
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
<asp:Content ID="Content2" ContentPlaceHolderID="Footer" Runat="Server">
    <asp:Label text="" runat="server" ID="lblMsg">
    </asp:Label>
     <asp:ValidationSummary ID="validationSummary" runat="server" HeaderText='<%$ Resources:Resource,V_SUMMARY%>' ValidationGroup="proc" />
</asp:Content>
