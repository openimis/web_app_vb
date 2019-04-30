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
<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/IMIS.Master" CodeBehind="FindProduct.aspx.vb" Inherits="IMIS.FindProduct"   title='<%$ Resources:Resource,P_FINDPRODUCT %>' %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="headContent" ContentPlaceHolderID="head" runat="server">

 <script type="text/javascript" language="javascript">
    $(document).ready(function() {
        bindRowSelection();
    });
 
     /** Ruzo Grid Row Selection 29 Aug 2014 >> Start **/
    function bindRowSelection() {
        var $trs = $('#<%=gvProducts.ClientID  %> tr')
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
            customDoPostback("<%=B_EDIT.UniqueID%>","");
        });
    }
    function fillSelectedRowData($row) {
         var $anchor = $row.find("td").eq(1).find("a");
         var dataNavStringParts = $anchor.attr("href").split("=")
         $("#<%=hfProdId.ClientID %>").val(dataNavStringParts[1]);
         $("#<%=hfProdCode.ClientID %>").val($anchor.html());
     }
     /** Ruzo Grid Row Selection 29 Aug 2014 >> End **/
 </script>
 <style type="text/css" >
     .mGrid tr
     {
         cursor:default;
     }
 </style>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="Body" Runat="Server">

    <div class="divBody" >
    
    <asp:HiddenField ID="hfProdId" runat="server" />
      <asp:HiddenField ID="hfProdCode" runat="server" />
      
    <table class="catlabel">
             <tr>
                <td >
                       <asp:Label  ID="Label8" runat="server" Text='<%$ Resources:Resource,L_SELECTCRITERIA %>'></asp:Label>   
               </td>
               
                </tr>
            
            </table>
    <asp:Panel ID="pnlTop" runat="server"   CssClass="panelTop" GroupingText='<%$ Resources:Resource,L_PRODUCTS %>'>
        <table>
            <tr>
                <td>
                      <table>
            <tr>
            
               <td class="FormLabel">
                            <asp:Label 
                            ID="L_PRODUCTCODE"
                            runat="server" 
                            Text='<%$ Resources:Resource,L_PRODUCTCODE %>'>
                            </asp:Label>
                        </td>
                <td class ="DataEntry">
                    <asp:TextBox ID="txtProductCode" runat="server" Width="150px" MaxLength="8"></asp:TextBox></td>
                <td class="FormLabel">
                            <asp:Label 
                            ID="L_PRODUCTNAME"
                            runat="server" 
                            Text='<%$ Resources:Resource,L_PRODUCTNAME %>'>
                            </asp:Label>
                        </td>
                <td class ="DataEntry">
                    <asp:TextBox ID="txtProductName" runat="server" MaxLength="100"></asp:TextBox></td>
                 <td class ="FormLabel">
                     <asp:Label ID="L_REGION" runat="server" Text="<%$ Resources:Resource,L_REGION %>"></asp:Label>
                 </td>
                <td class="DataEntry">
                    <asp:UpdatePanel ID="UpRegion" runat="server">
                        <ContentTemplate>
                            <asp:DropDownList ID="ddlRegion" runat="server" AutoPostBack="true">
                            </asp:DropDownList>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td class ="FormLabel">
                    <asp:Label 
                        ID="L_USERNAME"
                        runat="server" 
                        Text='<%$ Resources:Resource,L_DATEFROM %>'>
                       </asp:Label>
                     </td>
                 <td class ="DataEntry">
                    <asp:TextBox ID="txtDateFrom" runat="server" Width="120px"></asp:TextBox>
                        <%--<asp:MaskedEditExtender ID="txtDateFrom_MaskedEditExtender" runat="server" 
                            CultureDateFormat="dd/MM/YYYY"                             
                            TargetControlID="txtDateFrom" Mask="99/99/9999" MaskType="Date" 
                            UserDateFormat="DayMonthYear">
                        </asp:MaskedEditExtender>--%>
                    
                        <asp:Button ID="btnDateFrom" runat="server" class="dateButton" />
                             <asp:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtDateFrom" PopupButtonID="btnDateFrom" Format="dd/MM/yyyy"></asp:CalendarExtender>
                        </td>
                    <td>
                      <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" 
                    ControlToValidate="txtDateFrom" ErrorMessage="*" SetFocusOnError="True" 
                    ValidationExpression="^(0[1-9]|[12][0-9]|3[01])[/](0[1-9]|1[012])[/](19|20)\d\d$" 
                    ValidationGroup="check"></asp:RegularExpressionValidator>
                    </td>
                <td class="FormLabel">
                  
                </td>
                <td class ="FormLabel">
                                    
                    <asp:Label  ID="L_District" runat="server" Text="<%$ Resources:Resource,L_DISTRICT %>">
                     </asp:Label>
                                    
                </td>
                <td class="FormLabel">
                    <asp:UpdatePanel ID="UpDistrict" runat="server">
                        <ContentTemplate>
                                  <asp:DropDownList ID="ddlDistrict" runat="server">
                    </asp:DropDownList>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    
              
                    
                    
                </td>
                <td class="DataEntry">
                  
                </td>
            </tr>
            <tr>
                 <td class="FormLabel">
                    <asp:Label 
                        ID="Label1" 
                        runat="server" 
                        Text='<%$ Resources:Resource,L_DATETO %>'>
                    </asp:Label>
                 </td>
                 <td class ="DataEntry">
                    <asp:TextBox ID="txtDateTo" runat="server" Width="120px"></asp:TextBox>
                        <%--<asp:MaskedEditExtender ID="MaskedEditExtender1" runat="server" 
                            CultureDateFormat="dd/MM/YYYY"                             
                            TargetControlID="txtDateTo" Mask="99/99/9999" MaskType="Date" 
                            UserDateFormat="DayMonthYear">
                        </asp:MaskedEditExtender>--%>
                    
                        <asp:Button ID="btnDateTo" runat="server" class="dateButton" />
                             <asp:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtDateTo" PopupButtonID="btnDateTo" Format="dd/MM/yyyy"></asp:CalendarExtender>
                        </td>
                    <td>
                      <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" 
                    ControlToValidate="txtDateTo" ErrorMessage="*" SetFocusOnError="True" 
                    ValidationExpression="^(0[1-9]|[12][0-9]|3[01])[/](0[1-9]|1[012])[/](19|20)\d\d$" 
                    ValidationGroup="check"></asp:RegularExpressionValidator>
                    </td>
                <td class="FormLabel">
                 
                </td>
                <td class="FormLabel">
                   
                    
                    
                </td>
                <td class="FormLabel">
                 
                </td>
                <td>
                  
                </td>
            </tr>
         <tr>
        <td class="FormLabel">
         </td>
         <td class="DataEntry">
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
                     <asp:CheckBox class="checkbox" ID="chkLegacy" runat="server" 
                        Text='<%$ Resources:Resource,L_ALL %>' AutoPostBack="True" />
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
                           ID="L_FOUNDPRODUCTS"  
                           runat="server" 
                           Text='<%$ Resources:Resource,L_FOUNDPRODUCTS %>'> </asp:label>   
               </td>
               
                </tr>
            
            </table>
        <asp:Panel ID="pnlGrid" runat="server" ScrollBars="Auto"
            CssClass="panelBody">
            <asp:GridView ID="gvProducts" runat="server"
                AutoGenerateColumns="False"
                GridLines="None"
                AllowPaging="true" PagerSettings-FirstPageText="First Page" PagerSettings-LastPageText="Last Page" PagerSettings-Mode="NumericFirstLast"
                CssClass="mGrid"
                PagerStyle-CssClass="pgr"
                DataKeyNames="prodID,ProductCode" PageSize="15"
                EmptyDataText='<%$ Resources:Resource,M_NOPRODUCTS %>'
                RowStyle-Wrap="False">

                <Columns>

                    <asp:CommandField SelectText="Select" ShowSelectButton="true"
                        ItemStyle-CssClass="HideButton" HeaderStyle-CssClass="HideButton">
                        <HeaderStyle CssClass="HideButton" />
                        <ItemStyle CssClass="HideButton" />
                    </asp:CommandField>
                    <asp:HyperLinkField DataNavigateUrlFields="ProdID"
                        DataNavigateUrlFormatString="Product.aspx?p={0}" DataTextField="ProductCode"
                        HeaderText='<%$ Resources:Resource,L_PRODUCTCODE %>' HeaderStyle-Width="150px">
                        <HeaderStyle Width="100px" />
                    </asp:HyperLinkField>
                    <%-- <asp:BoundField DataField="ProductCode"  HeaderText='<%$ Resources:Resource,L_PRODUCTCODE %>' SortExpression="Code" 
                        HeaderStyle-Width ="120px">  
                   
                    </asp:BoundField> --%>
                    <asp:BoundField DataField="ProductName" HeaderText='<%$ Resources:Resource,L_PRODUCTNAME %>'
                        HeaderStyle-Width="200px">

                        <HeaderStyle Width="200px" />

                    </asp:BoundField>
                    <asp:BoundField DataField="RegionName" HeaderText='<%$ Resources:Resource,L_REGION %>'
                        SortExpression="RegionName" HeaderStyle-Width="110px">
                        <HeaderStyle Width="110px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="DistrictName" HeaderText='<%$ Resources:Resource,L_DISTRICT %>'
                        SortExpression="DistrictName" HeaderStyle-Width="110px">
                        <HeaderStyle Width="110px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="DateFrom" HeaderText='<%$ Resources:Resource,L_DATEFROM %>'
                        SortExpression="DateFrom" DataFormatString="{0:d}" HeaderStyle-Width="120px">

                        <HeaderStyle Width="120px" />

                    </asp:BoundField>


                    <asp:BoundField DataField="DateTo" HeaderText='<%$ Resources:Resource,L_DATETO %>'
                        SortExpression="DateTo" DataFormatString="{0:d}" />
                    <asp:BoundField DataField="MemberCount" HeaderText='<%$ Resources:Resource,L_MEMBERS %>'
                        SortExpression="MemberCount" DataFormatString="{0:n0}"
                        ItemStyle-HorizontalAlign="Right">
                        <ItemStyle HorizontalAlign="Right" />
                    </asp:BoundField>
                    <asp:BoundField DataField="LumpSum" HeaderText='<%$ Resources:Resource,L_LUMPSUM %>'
                        SortExpression="LumpSum" DataFormatString="{0:n2}"
                        ItemStyle-HorizontalAlign="Right">
                        <ItemStyle HorizontalAlign="Right" />
                    </asp:BoundField>
                    <asp:BoundField DataField="PremiumAdult" HeaderText='<%$ Resources:Resource,L_PREMIUMADULT %>'
                        SortExpression="PremiumAdult" DataFormatString="{0:n2}"
                        ItemStyle-HorizontalAlign="Right">
                        <ItemStyle HorizontalAlign="Right" />
                    </asp:BoundField>
                    <asp:BoundField DataField="PremiumChild" HeaderText='<%$ Resources:Resource,L_PREMIUMCHILD %>'
                        SortExpression="PremiumChild" DataFormatString="{0:n2}"
                        ItemStyle-HorizontalAlign="Right">

                        <ItemStyle HorizontalAlign="Right" />
                    </asp:BoundField>

                    <asp:BoundField DataField="InsurancePeriod" HeaderText='<%$ Resources:Resource,L_INSURANCEPERIOD %>'
                        SortExpression="InsurancePeriod" DataFormatString="{0:n0}"
                        ItemStyle-HorizontalAlign="Right">

                        <ItemStyle HorizontalAlign="Right" />
                    </asp:BoundField>
                    <asp:BoundField DataField="GracePeriod" HeaderText='<%$ Resources:Resource,L_GRACEPERIOD %>'
                        SortExpression="GracePeriod" DataFormatString="{0:n0}"
                        ItemStyle-HorizontalAlign="Right">

                        <ItemStyle HorizontalAlign="Right" />
                    </asp:BoundField>
                    <asp:BoundField DataField="ValidityFrom" DataFormatString="{0:d}" HeaderText='<%$ Resources:Resource,L_VALIDFROM %>' SortExpression="ValidityFrom" HeaderStyle-Width="70px">
                        <HeaderStyle Width="70px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="ValidityTo" DataFormatString="{0:d}" HeaderText='<%$ Resources:Resource,L_VALIDTO %>' SortExpression="ValidityTo" HeaderStyle-Width="70px">
                        <HeaderStyle Width="70px" />
                    </asp:BoundField>

                </Columns>

                <PagerStyle CssClass="pgr" />
                <AlternatingRowStyle CssClass="alt" />
                <SelectedRowStyle CssClass="srs" />
                <RowStyle CssClass="normal" Wrap="False" />
            </asp:GridView>
        </asp:Panel>
    </div>
    <asp:Panel ID="pnlButtons" runat="server"  CssClass="panelbuttons" >
                <table width="100%" cellpadding="10 10 10 10">
         <tr>
                
                 <td  align="left">
                <asp:Button 
                    ID="B_ADD" 
                    runat="server" 
                    Text='<%$ Resources:Resource,B_ADD%>' 
                      />
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
                    
                    ID="B_EDIT" 
                    runat="server" 
                    Text='<%$ Resources:Resource,B_EDIT%>'
                    ValidationGroup="check"  />
                </td>
                  <td align="center">
                <asp:Button 
                    
                    ID="B_DUPLICATE" 
                    runat="server" 
                    Text='<%$ Resources:Resource,B_DUPLICATE%>'
                      />
                </td>
                   <td align="center">
                <asp:Button 
                    
                    ID="B_DELETE" 
                    runat="server" 
                    Text='<%$ Resources:Resource,B_DELETE%>'
                    OnClientClick="return showmodalPopup();" Visible="False"
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
