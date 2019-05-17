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
<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/IMIS.Master" CodeBehind="FindMedicalItem.aspx.vb" Inherits="IMIS.FindMedicalItem" 
    title='<%$ Resources:Resource,L_FINDMEDICALITEM %>' %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="headContent" ContentPlaceHolderID="head" runat="server">

 <script type="text/javascript" language="javascript">
     $(document).ready(function () {
         bindRowSelection();
     });

     /** Ruzo Grid Row Selection 29 Aug 2014 >> Start **/
     function bindRowSelection() {
         var $trs = $('#<%=gvMedicalItems.ClientID%> tr')
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
        var $anchor = $row.find("td").eq(1).find("a");
        var dataNavStringParts = $anchor.attr("href").split("=")
        $("#<%=hfItemId.ClientID%>").val(dataNavStringParts[1]);
         $("#<%=hfItemCode.ClientID%>").val($anchor.html());
    }
    /** Ruzo Grid Row Selection 29 Aug 2014 >> End **/
 </script>

</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="Body" Runat="Server">
<asp:HiddenField ID="hfItemId" runat="server" />
      <asp:HiddenField ID="hfItemCode" runat="server" />

     <div class="divBody" >
    <table class="catlabel">
             <tr>
                <td >
                       <asp:Label  ID="Label8" runat="server" Text='<%$ Resources:Resource,L_SELECTCRITERIA %>'></asp:Label>   
               </td>
               
                </tr>
            
            </table>
    <asp:Panel ID="pnlTop" runat="server"  CssClass="panelTop" GroupingText='<%$ Resources:Resource,G_MEDICALITEM %>' >
        <table>
            <tr>
                <td>
                      <table>
            <tr>
            
                <td class="FormLabel">
                            <asp:Label 
                            ID="L_ITEMCODE"
                            runat="server" 
                            Text='<%$ Resources:Resource,L_Code %>'>
                            </asp:Label>
                        </td>
                <td class ="DataEntry">
                      <asp:TextBox ID="txtItemCode" runat="server" Width="130px" MaxLength="6"></asp:TextBox>
                      </td>
               <td class="FormLabel">
                            <asp:Label 
                            ID="L_ITEMNAME"
                            runat="server" 
                            Text='<%$ Resources:Resource,L_NAME %>'>
                            </asp:Label>
                        </td>
                <td class ="DataEntry">
                    <asp:TextBox ID="txtItemName" runat="server" MaxLength="100"></asp:TextBox></td>
                        


                        
                      
                 <td class ="FormLabel">
                     <asp:Label                     
                     ID="L_TYPE" 
                     runat="server" 
                     Text='<%$ Resources:Resource,L_TYPE %>'>
                     </asp:Label>
                 </td>
                 <td class="DataEntry">
                     <asp:DropDownList ID="ddlType" runat="server">
                     </asp:DropDownList>
                 </td>
            </tr>
             <tr>
            
               
                <td class="FormLabel">
                            <asp:Label 
                            ID="Label1"
                            runat="server" 
                            Text='<%$ Resources:Resource,L_PACKAGE %>'>
                            </asp:Label>
                        </td>
                <td class ="DataEntry">
                      <asp:TextBox ID="txtPackage" runat="server" Width="130px" MaxLength="25"></asp:TextBox>
                      </td>
               <td class="FormLabel">
                          
                        </td>
                <td class ="DataEntry">
                    </td>
                        


                        
                      
                 <td class ="FormLabel">
                   
                 </td>
                 <td class="DataEntry">
                    
                 </td>
            </tr>
             <tr>
            
                <td class="FormLabel">
                    
                        </td>
                <td class ="DataEntry">
                     </td>
               <td class="FormLabel">
                         
                        </td>
                <td class ="DataEntry">
                    </td>
                        


                        
                      
                 <td class ="FormLabel">
                    
                 </td>
                 <td class="DataEntry">
                    
                 </td>
            </tr>
              <tr>
            
                <td class="FormLabel">
                    
                        </td>
                <td class ="DataEntry">
                     </td>
               <td class="FormLabel">
                         
                        </td>
                <td class ="DataEntry">
                    </td>
                        


                        
                      
                 <td class ="FormLabel">
                    
                 </td>
                 <td class="DataEntry">
                    
                 </td>
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
                           ID="L_FOUNDITEMS"  
                           runat="server" 
                           Text='<%$ Resources:Resource,L_FOUNDITEMS %>'> </asp:label>   
               </td>
               
                </tr>
            
            </table>                                  
    <asp:Panel ID="pnlGrid" runat="server" ScrollBars="Auto" 
             CssClass="panelBody">
             <asp:GridView  ID="gvMedicalItems" runat="server"  
                        AutoGenerateColumns="False"
                        GridLines="None"
                       AllowPaging="true" PagerSettings-FirstPageText = "First Page" PagerSettings-LastPageText = "Last Page" PagerSettings-Mode ="NumericFirstLast"
                        CssClass="mGrid"
                        PagerStyle-CssClass="pgr"
                        EmptyDataText='<%$ Resources:Resource,L_NORECORDS %>'
                        DataKeyNames="ItemID,ItemCode" PageSize="15">
                       
                        <Columns>
                    
                        <asp:CommandField  SelectText ="Select" ShowSelectButton="true" 
                                ItemStyle-CssClass = "HideButton" HeaderStyle-CssClass ="HideButton" >
                            <HeaderStyle CssClass="HideButton" />
                            <ItemStyle CssClass="HideButton" />
                            </asp:CommandField>
                             <asp:HyperLinkField DataNavigateUrlFields="ItemUUID" DataNavigateUrlFormatString="MedicalItem.aspx?i={0}" DataTextField="ItemCode"  HeaderText='<%$ Resources:Resource,L_Code %>' SortExpression="ItemCode" HeaderStyle-Width ="60px"/>
                          
                        <asp:BoundField DataField="ItemName"  HeaderText='<%$ Resources:Resource,L_NAME %>' SortExpression="ItemName"> 
                       
                        </asp:BoundField> 
                        <asp:BoundField DataField="ItemType"  HeaderText='<%$ Resources:Resource,L_TYPE%>' SortExpression="ItemType" 
                                HeaderStyle-Width ="110px"> 
                        <HeaderStyle Width="110px" />
                        </asp:BoundField> 
                        <asp:BoundField DataField="ItemPackage" HeaderText='<%$ Resources:Resource,L_PACKAGE %>' SortExpression="ItemPackage"> 
                        
                        </asp:BoundField>
                    
                    
                            <asp:BoundField DataField="ItemPrice" HeaderText='<%$ Resources:Resource,L_PRICE %>' 
                                SortExpression="ItemPrice"  HeaderStyle-Width="80px" DataFormatString="{0:n2}" ItemStyle-HorizontalAlign="Right"/>
                    
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
    <asp:Label text="" runat="server" ID="lblMsg"> </asp:Label>
<asp:ValidationSummary ID="validationSummary" runat="server" HeaderText='<%$ Resources:Resource,V_SUMMARY%>' ValidationGroup="check" />
</asp:Content>

