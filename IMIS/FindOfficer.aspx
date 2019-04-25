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
<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/IMIS.Master" CodeBehind="FindOfficer.aspx.vb" Inherits="IMIS.FindOfficer" 
    title="<%$ Resources:Resource,L_FINDOFFICER %>" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="headContent" ContentPlaceHolderID="head" runat="server">

 <script type="text/javascript" language="javascript">
     $(document).ready(function () {
         bindRowSelection();

         $('#<%= B_SEARCH.ClientID%>').click(function (e) {
             var passed = true;
             $DateControls = $('.dateCheck');
             $DateControls.each(function () {
                 if ($(this).val().length > 0 && !isValidJSDate($(this).val())) {
                     $('#<%=lblMsg.ClientID%>').html('<%= imisGen.getMessage("M_INVALIDDATE", True)%>');
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

     /** Ruzo Grid Row Selection 29 Aug 2014 >> Start **/
     function bindRowSelection() {
         var $trs = $('#<%=gvOfficers.ClientID%> tr')
        $trs.unbind("hover").hover(function () {
            if ($(this).index() < 1 || $(this).is(".pgr")) return;
            $trs.removeClass("alt");
            $(this).addClass("alt");
        }, function () {
            if ($(this).index() < 1 || $(this).is(".pgr")) return;
            $(this).removeClass("alt");
        });
        $trs.unbind("click").click(function () {
            if ($(this).index() < 1 || $(this).is(".pgr")) return;
            $trs.removeClass("srs");
            $(this).addClass("srs");
            fillSelectedRowData($(this))
        });
        if ($trs.filter(".srs").length > 0) {
            $trs.filter(".srs").eq(0).trigger("click");
        } else {
            $trs.eq(1).trigger("click");
        }
        $trs.unbind("dblclick").dblclick(function () {
            if ($(this).index() < 1 || $(this).is(".pgr")) return;
            fillSelectedRowData($(this));
            customDoPostback("<%=B_EDIT.UniqueID%>", "");
        });
    }
    function fillSelectedRowData($row) {
        var $anchor = $row.find("td").eq(0).find("a");
        var dataNavStringParts = $anchor.attr("href").split("=")
        $("#<%=hfOfficerId.ClientID%>").val(dataNavStringParts[1]);
        $("#<%=hfOfficerCode.ClientID%>").val($anchor.html());
        var $hasLogin = $row.find("td").eq(8).html()
        $("#<%=hfHasLogin.ClientID%>").val($hasLogin);
    }
    /** Ruzo Grid Row Selection 29 Aug 2014 >> End **/
 </script>

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


<asp:Content ID="Content1" ContentPlaceHolderID="Body" Runat="Server">

<asp:HiddenField ID="hfOfficerId" runat="server" />
<asp:HiddenField ID="hfOfficerCode" runat="server" />
<asp:HiddenField ID="hfHasLogin" runat="server" />
      
  <div class="divBody" >
         <table class="catlabel">
             <tr>
                <td >
                       <asp:Label  ID="Label8" runat="server" Text='<%$ Resources:Resource,L_SELECTCRITERIA %>'></asp:Label>   
               </td>
               
                </tr>
            
            </table>
        <asp:Panel ID="pnlTop" runat="server"  GroupingText='<%$ Resources:Resource,G_OFFICER %>' CssClass="panelTop" >
        <table>
            <tr>
                <td>
                      <table>
            <tr>
            
               <td class="FormLabel">
                            <asp:Label 
                            ID="L_LASTNAME"
                            runat="server" 
                            Text='<%$ Resources:Resource,L_LASTNAME %>'>
                            </asp:Label>
                        </td>
                <td class ="DataEntry">
                    <asp:TextBox ID="txtLastName" runat="server"></asp:TextBox></td>
                <td class="FormLabel">
                            <asp:Label 
                            ID="L_OTHERNAMES"
                            runat="server" 
                            Text='<%$ Resources:Resource,L_OTHERNAMES %>'>
                            </asp:Label>
                        </td>
                <td class ="DataEntry">
                    <asp:TextBox ID="txtOtherNames" Width="130px" runat="server"></asp:TextBox></td>
                 <td class ="FormLabel">
                     <asp:Label ID="L_REGION" runat="server" Text="<%$ Resources:Resource,L_REGION %>">
                     </asp:Label>
                 </td>
                 <td class="DataEntry">
                     <asp:UpdatePanel runat="server" ID="UpRegion">
                         <ContentTemplate>
                             <asp:DropDownList ID="ddlRegion" runat="server" AutoPostBack="True">
                     </asp:DropDownList>
                         </ContentTemplate>
                     </asp:UpdatePanel>
                     
                </td>
            </tr>
            <tr>
                 <td class="auto-style1">
                    <asp:Label ID="L_CODE" runat="server" Text="<%$ Resources:Resource,L_CODE %>">
                    </asp:Label>
                  
                    
                </td>
                <td class="DataEntry">
                  <asp:TextBox ID="txtCode" runat="server" MaxLength="8">
                    </asp:TextBox>
                </td>
                <td class ="auto-style1">
                    <asp:Label 
                        ID="L_DOBFROM"
                        runat="server" 
                        Text='<%$ Resources:Resource,L_BIRTHDATEFROM %>'></asp:Label>
                     </td>
                <td class ="DataEntry">
                   <asp:TextBox 
                        ID="txtDOBFROM" 
                        runat="server" Width="100px" CssClass="dateCheck"></asp:TextBox>
                    <asp:Button ID="btnDateFrom" runat="server" Class="dateButton" />
                    <asp:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtDOBFROM" PopupButtonID="btnDateFrom" Format="dd/MM/yyyy"></asp:CalendarExtender>
                  <%--  <asp:MaskedEditExtender ID="txtDOBFROM_MaskedEditExtender" runat="server" 
                            CultureDateFormat="dd/MM/YYYY"                             
                            TargetControlID="txtDOBFROM" Mask="99/99/9999" MaskType="Date" 
                            UserDateFormat="DayMonthYear">
                        </asp:MaskedEditExtender>--%>
                       
                </td>
                <td class="auto-style1">
                  
                    <asp:Label ID="L_District0" runat="server" Text="<%$ Resources:Resource,L_DISTRICT %>">
                     </asp:Label>
                  
                </td>
                <td class ="DataEntry">
                                <asp:UpdatePanel runat="server" ID="UpDistrict">
                                    <ContentTemplate>
 <asp:DropDownList ID="ddlDistrict" runat="server">
                    </asp:DropDownList>
                                    </ContentTemplate>
                                </asp:UpdatePanel>    
                   
                                    
                </td>
               
            </tr>
            <tr>
                 <td class="FormLabel">
                     <asp:Label ID="L_Email" runat="server" style="direction: ltr" Text="<%$ Resources:Resource,L_EMAIL %>"></asp:Label>
                 </td>
                <td style="direction: ltr">
                    <asp:TextBox ID="txtEmail" runat="server" MaxLength="200"></asp:TextBox>
                 </td>
                <td class ="FormLabel">
                    <asp:Label 
                        ID="L_BIRTHDATETO"
                        runat="server" 
                        Text='<%$ Resources:Resource,L_BIRTHDATETO %>'></asp:Label>
                     </td>
                <td class ="DataEntry">
                   <asp:TextBox 
                        ID="txtDOBTo" 
                        runat="server" Width="100px" CssClass="dateCheck"></asp:TextBox>
                    <asp:Button ID="btnDateTo" runat="server" Class="dateButton" />
                    <asp:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtDOBTo" PopupButtonID="btnDateTo" Format="dd/MM/yyyy"></asp:CalendarExtender>
                    <%--<asp:MaskedEditExtender ID="txtDOBTO_MaskedEditExtender" runat="server" 
                            CultureDateFormat="dd/MM/YYYY"                             
                            TargetControlID="txtDOBTo" Mask="99/99/9999" MaskType="Date" 
                            UserDateFormat="DayMonthYear">
                        </asp:MaskedEditExtender>--%>
                       
                </td>
                <td class="FormLabel">
                 
                    <asp:Label ID="Label1" runat="server" Text="<%$ Resources:Resource,L_PHONE %>">
                    </asp:Label>
                 
                </td>
                <td>
                  
                    <asp:TextBox ID="txtPhone" runat="server" width="150px"></asp:TextBox>
                  
                </td>
            </tr>
         <tr>
         <td >
         </td>
         <td>
         </td>
         <td>
         </td>
         <td>
         </td>
         <td>
         </td>
         <td>
             &nbsp;</td>
         
         
         </tr>
        </table>
                </td>
                <td>
                    <br />
                     <asp:CheckBox class="checkbox"   AutoPostBack="True"  ID="chkLegacy" runat="server" Text='<%$ Resources:Resource,L_ALL %>' />
                      <br />
                      <br />
                   <asp:Button class="button" ID="B_SEARCH" runat="server" 
                          Text='<%$ Resources:Resource,B_SEARCH %>' >
                  </asp:Button>
                  <br />
                  
                </td>
            </tr>
        </table>
                   
        </asp:Panel>
           <table class="catlabel">
             <tr>
                <td >
                       <asp:label  
                           ID="L_FOUNDUSERS"  
                           runat="server" 
                           Text='<%$ Resources:Resource,L_FOUNDOFFICERS %>'> </asp:label>   
               </td>
               
                </tr>
            
            </table>
         <asp:Panel ID="pnlGrid" runat="server" 
             CssClass="panelBody"  ScrollBars="Auto">
                                  <asp:GridView  ID="gvOfficers" runat="server"  
                        AutoGenerateColumns="False"
                        GridLines="None"
                       AllowPaging="true" PagerSettings-FirstPageText = "First Page" PagerSettings-LastPageText = "Last Page" PagerSettings-Mode ="NumericFirstLast" PageSize="15"
                        CssClass="mGrid"
                        PagerStyle-CssClass="pgr"
                        DataKeyNames="OfficerID,Code" 
                        EmptyDataText='<%$ Resources:Resource,L_NORECORDS %>'
                     
                        >
                       
                        <Columns>
                    <asp:HyperLinkField DataNavigateUrlFields = "OfficerId" DataTextField="Code" DataNavigateUrlFormatString = "Officer.aspx?o={0}" HeaderText='<%$ Resources:Resource,L_Code %>' HeaderStyle-Width ="80px" >

                        
                    </asp:HyperLinkField> 
                        <asp:CommandField  SelectText ="Select" ShowSelectButton="true" 
                                ItemStyle-CssClass = "HideButton" HeaderStyle-CssClass ="HideButton" >
                            <HeaderStyle CssClass="HideButton" />
                            <ItemStyle CssClass="HideButton" />
                            </asp:CommandField>
                               
                        <asp:BoundField DataField="LastName"  HeaderText='<%$ Resources:Resource,L_LASTNAME%>'
                                SortExpression="LastName" HeaderStyle-Width ="160px"> 
                        
                        </asp:BoundField> 
                        <asp:BoundField DataField="OtherNames"  HeaderText='<%$ Resources:Resource,L_OTHERNAMES%>'     
                                SortExpression="OtherNames" HeaderStyle-Width ="130px"> 
                       
                        </asp:BoundField> 
                        <asp:BoundField DataField="DOB" HeaderText='<%$ Resources:Resource,L_BIRTHDATE %>' SortExpression="DOB" 
                                HeaderStyle-Width="70px" DataFormatString="{0:d}"> 
                       
                        </asp:BoundField>
                    
                    
                            <asp:BoundField DataField="Phone" HeaderText='<%$ Resources:Resource,L_PHONE %>' SortExpression="Phone" HeaderStyle-Width="100px"/>
                                <asp:BoundField DataField="RegionName" HeaderText='<%$ Resources:Resource,L_REGION %>' 
                                SortExpression="RegionName" HeaderStyle-Width="70px" />
                            <asp:BoundField DataField="DistrictName" HeaderText='<%$ Resources:Resource,L_DISTRICT %>' 
                                SortExpression="DistrictName" HeaderStyle-Width="70px" />
                            <asp:BoundField DataField="HasLogin" HeaderText='<%$ Resources:Resource,L_HASLOGIN %>' SortExpression="HasLogin" 
                                HeaderStyle-Width="70px"> 
                            </asp:BoundField>
                     <asp:BoundField DataField="ValidityFrom" DataFormatString="{0:d}" HeaderText='<%$ Resources:Resource,L_VALIDFROM %>' SortExpression="ValidityFrom" HeaderStyle-Width="70px">  </asp:BoundField>
                <asp:BoundField DataField="ValidityTo" DataFormatString="{0:d}" HeaderText='<%$ Resources:Resource,L_VALIDTO %>' SortExpression="ValidityTo" HeaderStyle-Width="70px">  </asp:BoundField>
                    </Columns>
                   
                        <PagerStyle CssClass="pgr" />
                        <AlternatingRowStyle CssClass="alt" />
                        <SelectedRowStyle CssClass="srs" />
                       <RowStyle CssClass="normal" />
                    </asp:GridView>
         </asp:Panel>
</div>
         <asp:Panel ID="pnlButtons" runat="server"   CssClass="panelbuttons" >
                <table width="100%" cellpadding="10 10 10 10">
         <tr>
                
                 <td  align="left">
                <asp:Button 
                    ID="B_ADD" 
                    runat="server" 
                    Text='<%$ Resources:Resource,B_ADD%>'
                      />
                </td>
                
                
                <td align="center">
                <asp:Button 
                    
                    ID="B_EDIT" 
                    runat="server" 
                    Text='<%$ Resources:Resource,B_EDIT%>'
                    ValidationGroup="check"  />
                </td>
                  <td align="left" >
                <%--<asp:Button 
                     Visible ="false"
                    ID="B_VIEW" 
                    runat="server" 
                    Text='<%$ Resources:Resource,B_VIEW%>'
                      />--%>
                </td>
                   <td align="center">
                <asp:Button 
                    
                    ID="B_DELETE" 
                    runat="server" 
                    Text='<%$ Resources:Resource,B_DELETE%>'
                    OnClientClick="return showmodalPopup();"
                      />
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
    <asp:label id="lblMsg" runat="server"></asp:label>
</asp:Content>
