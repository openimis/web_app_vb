<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/IMIS.Master" CodeBehind="FindProfile.aspx.vb" Inherits="IMIS.FindProfile"
 Title='<%$ Resources:Resource,L_FINDROLE %>' %>
 <%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>

<asp:Content ID="headContent" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" language="javascript">
    $(document).ready(function () {
        bindRowSelection();
    });

    /** Ruzo Grid Row Selection 29 Aug 2014 >> Start **/
    function bindRowSelection() {
        var $trs = $('#<%=gvRole.ClientID%> tr')
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
        $("#<%=hfRoleId.ClientID%>").val(dataNavStringParts[1]);
         $("#<%=hfRoleName.ClientID%>").val($anchor.html());
    }
    /** Ruzo Grid Row Selection 29 Aug 2014 >> End **/
 </script>
 </asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="Body" Runat="Server">

    <div class="divBody" >
     <asp:HiddenField ID="hfRoleId" runat="server" />
      <asp:HiddenField ID="hfRoleName" runat="server" />
      
        <table class="catlabel">
             <tr>
                <td >
                       <asp:Label  ID="Label8" runat="server" Text='<%$ Resources:Resource,L_SELECTCRITERIA %>'></asp:Label>   
               </td>
               
                </tr>
            
            </table>
        
       <asp:Panel ID="Panel1"  runat="server" CssClass="panelTop" GroupingText='<%$ Resources:Resource,G_ROLE_DETAILS %>'> 
        <table>
            <tr>
                <td>
                    <table>
                        <tr>
            
               <td class="FormLabel">
                            <asp:Label 
                            ID="L_ROLENAME"
                            runat="server" 
                            Text='<%$ Resources:Resource,L_ROLENAME %>'>
                            </asp:Label>
                        </td>
                <td class ="DataEntry">
                    <asp:TextBox ID="txtRolename" runat="server">
                    </asp:TextBox></td>
                <td class="FormLabel">
                          <%--  <asp:Label 
                            ID="L_OTHERNAMES"
                            runat="server" 
                            Text='<%$ Resources:Resource,L_OTHERNAMES %>'>
                            </asp:Label>--%>
                        </td>
                <td class ="DataEntry">
                  <%--  <asp:TextBox ID="txtOtherNames" runat="server">
                    </asp:Tex--%>
                    </td>
                 <td class ="FormLabel">
                    <%-- <asp:Label ID="L_REGION" runat="server" Text="<%$ Resources:Resource,L_REGION %>"></asp:Label>--%>
                 </td>
                 <td class="DataEntry">                    
                    
                 </td>
                           <%-- <caption>
                                --&gt;
                            </caption>--%>
            </tr>
                        <tr>
                <td class ="FormLabel">
                    <asp:Label 
                        ID="L_SYSTEM"
                        runat="server" 
                        Text='<%$ Resources:Resource,L_SYSTEM %>'>
                       </asp:Label>
                     </td>
                <td class ="DataEntry">
                     <asp:DropDownList ID="ddlSystem" runat="server">
                         <asp:ListItem></asp:ListItem>
                         <asp:ListItem>True</asp:ListItem>
                         <asp:ListItem>False</asp:ListItem>
                     </asp:DropDownList>
                  <%-- <asp:TextBox 
                        ID="txtRolename" 
                        runat="server">
                    </asp:--%>

                </td>
                <td class ="FormLabel">
                    <%-- <asp:Label                     
                     ID="L_ROLES" 
                     runat="server" 
                     Text='<%$ Resources:Resource,L_ROLES %>'>
                     </asp:Label>--%>
                 </td>
                 <td class="DataEntry">
                    <%-- <asp:DropDownList ID="ddlRole" runat="server">
                     </asp:DropDownList>--%>
                 </td>
               <td class ="FormLabel">
                    <%-- <asp:Label ID="L_District0" runat="server" Text="<%$ Resources:Resource,L_DISTRICT %>">
                     </asp:Label>--%>
                 </td>
                 <td class="DataEntry">
                     <asp:UpdatePanel runat="server" ID="UpDistrict">
                         <ContentTemplate>
                    <%--         <asp:DropDownList ID="ddlDistrict" runat="server">
                     </asp:D--%>
                         </ContentTemplate>
                     </asp:UpdatePanel>
                     
                 </td>
            </tr>
                        <tr>
                 <td class="FormLabel">
                    <asp:Label 
                        ID="Label1" 
                        runat="server" 
                        Text='<%$ Resources:Resource,L_BLOCKED %>'>
                    </asp:Label>
                 </td>
                <td class ="FormLabel">
                 <%--   <asp:TextBox 
                        ID="txtPhone" 
                        runat="server">
                    </asp:TextBo--%>
                       <asp:DropDownList ID="ddlBlocked" runat="server">
                           <asp:ListItem></asp:ListItem>
                           <asp:ListItem>True</asp:ListItem>
                           <asp:ListItem>False</asp:ListItem>
                     </asp:DropDownList>
                </td>
               <td class ="FormLabel">
                    <%-- <asp:Label                     
                     ID="L_HFNAME" 
                     runat="server" 
                     Text='<%$ Resources:Resource,L_HFNAME %>'>
                     </asp:Label>--%>
                 </td>
                 <td class="DataEntry">
                  <%--   <asp:DropDownList ID="ddlHFName" runat="server">
                     </asp:DropDownList>--%>
                 </td>
                <td class="FormLabel">
                 
                  <%--  <asp:Label ID="L_LANGUAGE0" runat="server" Text="<%$ Resources:Resource,L_LANGUAGE %>">
                     </asp:Label>--%>
                 
                </td>
                <td>
                 <%-- 
                    <asp:DropDownList ID="ddlLanguage" runat="server">
                    </asp:DropDownList>
                  --%>
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
    
     <%--<asp:Label ID="L_Email0" runat="server" style="direction: ltr" Text="<%$ Resources:Resource,L_EMAIL %>"></asp:Label>--%>
    
 </td>
 <td class="DataEntry">
    
     <%--<asp:TextBox ID="txtEmail" runat="server"></asp:TextBox>--%>
    
 </td>
            </tr>
                     <%--   <caption>
                            --&gt;
                        </caption>--%>
                    </table>
                </td>                    
                <td>
                    <br />
                     <asp:CheckBox class="checkbox" AutoPostBack="True" ID="chkLegacy" runat="server" Text='<%$ Resources:Resource,L_ALL %>' />
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
                           ID="L_ROLEFOUNDS"  
                           runat="server" 
                           Text='<%$ Resources:Resource,L_ROLEFOUNDS %>'> </asp:label>   
               </td>
               
                </tr>
            
            </table>

<asp:Panel ID="pnlGrid" runat="server"  ScrollBars="Auto" CssClass="panelBody">
    <asp:GridView  ID="gvRole" runat="server"  AutoGenerateColumns="False" GridLines="None" AllowPaging="true" PagerSettings-FirstPageText = "First Page" PagerSettings-LastPageText = "Last Page" PagerSettings-Mode ="NumericFirstLast" CssClass="mGrid" PageSize="15" DataKeyNames="RoleID,RoleName" EmptyDataText='<%$ Resources:Resource,L_NORECORDS %>'>
        <Columns>        
            <asp:HyperLinkField DataNavigateUrlFields="RoleUUID" 
                DataNavigateUrlFormatString="Role.aspx?r={0}" DataTextField="RoleName"  
                HeaderText='<%$ Resources:Resource,L_ROLENAME %>' HeaderStyle-Width ="60px">
                <HeaderStyle Width="60px" />
            </asp:HyperLinkField>
            <asp:BoundField DataField="System"  HeaderText='<%$ Resources:Resource,L_SYSTEM %>' SortExpression="System" HeaderStyle-Width ="110px"> 
                <HeaderStyle Width="110px" />
            </asp:BoundField> 
            <asp:BoundField DataField="Blocked"  HeaderText='<%$ Resources:Resource,L_BLOCKED %>' SortExpression="Blocked" HeaderStyle-Width ="110px"> 
                <HeaderStyle Width="110px" />
            </asp:BoundField> 
          
            <asp:BoundField DataField="ValidityFrom" DataFormatString="{0:d}" HeaderText='<%$ Resources:Resource,L_VALIDFROM %>' SortExpression="ValidityFrom" HeaderStyle-Width="70px">  
                <HeaderStyle Width="70px" />
            </asp:BoundField>
            <asp:BoundField DataField="ValidityTo" DataFormatString="{0:d}" HeaderText='<%$ Resources:Resource,L_VALIDTO %>' SortExpression="ValidityTo" HeaderStyle-Width="70px">  
                <HeaderStyle Width="70px" />
            </asp:BoundField>
            <asp:BoundField DataField="RoleId" ><ItemStyle CssClass="hidecol"/><HeaderStyle CssClass="hidecol" />  </asp:BoundField>
        </Columns>
        <PagerStyle CssClass="pgr" />
        <AlternatingRowStyle CssClass="alt" /> 
        <SelectedRowStyle CssClass="srs" />
        <RowStyle CssClass="normal" Wrap="False" />
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
    <asp:label id="lblMsg" runat="server"></asp:label><asp:ValidationSummary ID="validationSummary" runat="server" HeaderText='<%$ Resources:Resource,V_SUMMARY%>' ValidationGroup="check" />
</asp:Content>