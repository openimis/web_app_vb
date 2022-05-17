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
<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="MoveLocation.aspx.vb" Inherits="IMIS.MoveLocation" Title='<%$ Resources:Resource,L_MOVELOCATIONS%>' MasterPageFile="~/IMIS.Master" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="head" runat="server">
       <script type="text/javascript">

           var RegionLocationLabel = '<%=imisgen.getMessage("L_REGIONNAME", True)%>';
           var districtLocationLabel = '<%=imisgen.getMessage("L_DistrictName", True)%>';
           var wardLocationLabel = '<%=imisgen.getMessage("L_WardName", True)%>';
           var villageLocationLabel = '<%=imisgen.getMessage("L_VillageName", True)%>';
           var locationType = '<%=imisgen.getMessage("L_DISTRICT", True)%>';
           var $row = null;
           var disableEdit = false;

           $(document).ready(function () {
               popup.acceptBTN_Text = '<%=imisgen.getMessage("L_YES", True)%>';
                popup.rejectBTN_Text = '<%=imisgen.getMessage("L_NO", True)%>';

             
               
               var locationName = $row.find("td:last").html();

                    if (locationType == '<%=imisgen.getMessage("L_REGION", True)%>')
                        popup.confirm('<%=imisgen.getMessage("M_DELETEREGIONPROMPT", True)%>' + "<br/>" + locationName, callBackFun);
                    else if (locationType == '<%=imisgen.getMessage("L_DISTRICT", True)%>')
                        popup.confirm('<%=imisgen.getMessage("M_DELETEDISTRICTPROMPT", True)%>' + "<br/>" + locationName, callBackFun);
                    else if (locationType == '<%=imisgen.getMessage("L_WARD", True)%>')
                        popup.confirm('<%=imisgen.getMessage("M_DELETEWARDPROMPT", True)%>' + "<br/>" + locationName, callBackFun);
                    else if (locationType == '<%=imisgen.getMessage("L_VILLAGE", True)%>')
                        popup.confirm('<%=imisgen.getMessage("M_DELETEVILLAGEPROMPT", True)%>' + "<br/>" + locationName, callBackFun);
                    else
                        popup.confirm('<%=imisgen.getMessage("M_DELETEPROMPT", True ) %>', callBackFun, args);
                    return false;
                });


                

                $("body").click(function (eve) {
                    if ($(eve.target).is(".LocationsButton,#LocationEditPanel,#LocationEditPanel input")) return;
                    $("#LocationEditPanel").slideUp(0);
                });

            });

            function pageLoadExtend() {
                selectAppropriateRow();

                $(".mGrid").click(function () {
                    if ($(this).find("tr").length > 1) {
                        disableEdit = false;
                        return;
                    }

                    $row = null;
                    disableEdit = true;
                    if ($(this).is("#<%=gvRegions.ClientID%>")) {
                        $("#LocationTypeLabel").html(RegionLocationLabel);
                        locationType = '<%=imisgen.getMessage("L_REGION", True)%>';
                    }
                    else if ($(this).is("#<%=gvDistricts.ClientID %>")) {
                        $("#LocationTypeLabel").html(districtLocationLabel);
                        locationType = '<%=imisgen.getMessage("L_DISTRICT", True)%>';

                    } else if ($(this).is("#<%=gvWards.ClientID %>")) {
                        $("#LocationTypeLabel").html(wardLocationLabel);
                        locationType = '<%=imisgen.getMessage("L_WARD", True)%>';

                    } else if ($(this).is("#<%=gvVillages.ClientID %>")) {
                        $("#LocationTypeLabel").html(villageLocationLabel);
                        locationType = '<%=imisgen.getMessage("L_VILLAGE", True )%>';

                    }
                    $("#<%=hfLocationType.ClientID %>").val(locationType);
                });

             
            }
            function selectAppropriateRow() {
                var $gv = $("#<%=gvDistricts.ClientID %>");

                if ($("#<%=hfLocationType.ClientID %>").val() == '<%=imisgen.getMessage("L_REGION", True)%>') {
                    $gv = $("#<%=gvRegions.ClientID%>");
                }
                else if ($("#<%=hfLocationType.ClientID %>").val() == '<%=imisgen.getMessage("L_DISTRICT", True)%>') {

                    $gv = $("#<%=gvDistricts.ClientID %>");
                } else if ($("#<%=hfLocationType.ClientID %>").val() == '<%=imisgen.getMessage("L_WARD", True)%>') {

                    $gv = $("#<%=gvWards.ClientID %>");
                } else if ($("#<%=hfLocationType.ClientID %>").val() == '<%=imisgen.getMessage("L_VILLAGE", True )%>') {

                    $gv = $("#<%=gvVillages.ClientID %>");
                }

    if ($gv.find("tr.srs").length == 0) {
        if ($gv.find("tr").length > 1) {
            if ($gv.find("tr:has(th)").length > 0)
                $row = $gv.find("tr:has(th)").next();
            else
                $row = $gv.find("tr:first");
        }
    } else
        $row = $gv.find("tr.srs").eq(0);
}
</script>

<style type="text/css">
#LocationEditPanel{display:none;position:relative;width:100%;padding:10px;padding-top:10px;padding-top:10px;}
#LocationEditPanel table{position:relative;width:500px;}
#LocationsButtonPanel input{margin-left:5px;margin-right:5px;}
    #tblLocations
    {
        width:100%;
        height:400px;
    }
        #tblLocations td
        {
            width:25%;
        }
    .catlabel
    {
        width:100% !important;
        height: 110px !important;
    }
</style>

</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="Body" Runat="Server">

    

  <asp:HiddenField ID="hfMode" value='<%$ Resources:Resource,B_EDIT%>' runat="server" />
  <asp:HiddenField ID="hfLocationType" value='<%$ Resources:Resource,L_REGION%>' runat="server" />
   
           <div class="divBody" >  
           
           <%--<div id="LocationEditPanel">
              <table>
               <tr>
                   <td>
                       <asp:Label ID="LocationTypeLabel" runat="server" Text='<%$ Resources:Resource,L_REGION%>'></asp:Label> 
                    </td>
                   <td><asp:TextBox runat="server" ID="txtLocationName" ></asp:TextBox></td>
                   <td colspan="2" align="left"><asp:Button ID="btnSave" runat="server" Text="<%$ Resources:Resource,B_SAVE%>" /></td>
               </tr>
              </table>
           </div>--%>
       
             
               <asp:UpdatePanel ID="upLocations" runat="server">
                   <ContentTemplate>
                       <table id="tblLocations">
                           <tr>
                               <td>
                                   <asp:Panel ID="pnlVillages" runat="server"
                                       ScrollBars="Auto" BorderStyle="Groove" Height="495px"
                                       CssClass="panel" Width="220px" style="overflow-x: hidden !important;">

                                       <div>
                                           <table class="catlabel" style="padding: 10px; color:#FFF !important">
                                               <%-- <tr>
                                                   <td align="center">
                                                       <asp:Label ID="L_VILLAGE" runat="server" Text='<%$ Resources:Resource,L_VILLAGE%>'></asp:Label>
                                                   </td>

                                               </tr>--%>
                                               <tr>
                                                   <td class="FormLabel" style="color:#fff !important">
                                                       <asp:Label
                                                           ID="lblRegion"
                                                           runat="server"
                                                           Text='<%$ Resources:Resource,L_REGION %>'>
                                                       </asp:Label>
                                                   </td>
                                                   <td class="DataEntry">
                                                       <asp:DropDownList ID="ddlVRegion" runat="server" Width="110px" AutoPostBack="true" />
                                                   </td>
                                               </tr>
                                               <tr>
                                                   <td class="FormLabel" style="color:#fff !important">
                                                       <asp:Label
                                                           ID="lblDistrict"
                                                           runat="server"
                                                           Text='<%$ Resources:Resource,L_DISTRICT %>'>
                                                       </asp:Label>
                                                   </td>
                                                   <td class="DataEntry">
                                                       <asp:DropDownList ID="ddlVDistrict" runat="server" Width="110px" AutoPostBack="true" />
                                                   </td>
                                               </tr>
                                               <tr>
                                                   <td class="FormLabel" style="color:#fff !important">
                                                       <asp:Label
                                                           ID="lblWrd"
                                                           runat="server"
                                                           Text='<%$ Resources:Resource,L_WARD %>'>
                                                       </asp:Label>
                                                   </td>
                                                   <td class="DataEntry">
                                                       <asp:DropDownList ID="ddlVWard" runat="server" Width="110px" AutoPostBack="true" />
                                                   </td>
                                               </tr>

                                           </table>
                                       </div>

                                       <asp:GridView ID="gvVillages" runat="server"
                                           AutoGenerateColumns="False"
                                           EmptyDataText='<%$ Resources:Resource,M_NOVILLAGES %>'
                                           CssClass="mGrid"
                                           PagerStyle-CssClass="pgr"
                                           AlternatingRowStyle-CssClass="alt"
                                           SelectedRowStyle-CssClass="srs"
                                           DataKeyNames="VillageId,VillageName">
                                           <Columns>
                                               <asp:BoundField DataField="VillageCode" HeaderText='<%$ Resources:Resource,L_VILLAGECODE%>'></asp:BoundField>
                                           </Columns>
                                           <Columns>
                                               <asp:BoundField DataField="VillageName" HeaderText='<%$ Resources:Resource,L_VILLAGE%>'></asp:BoundField>
                                           </Columns>
                                           <PagerStyle CssClass="pgr" />
                                           <SelectedRowStyle CssClass="srs" />
                                           <AlternatingRowStyle CssClass="alt" />
                                           <RowStyle CssClass="normal" />
                                       </asp:GridView>

                                       <%--  </ContentTemplate>
               </asp:UpdatePanel> --%>
                                   </asp:Panel>
                               </td>
                               <td style='width: 30px !important'>
                                   <%--  <asp:Button ID="btnVillageToDistrict" runat="server" Text=">>" style='background:#FFF; color:#000; width:35px; font-weight:900;'   />--%>
                                   <asp:ImageButton ID="ImgBtnMoveVillage" ImageUrl="~/Images/GoLtr.bmp" runat="server" />
                               </td>
                               <td>
                                   <asp:Panel ID="pnlWards" runat="server"
                                       ScrollBars="Auto" BorderStyle="Groove" Height="495px"
                                       CssClass="panel"  Width="220px" style="overflow-x: hidden !important;" >
                                       <div>
                                           <table class="catlabel">
                                               <%-- <tr>
                                                   <td>
                                                       <asp:Label ID="L_WARD" runat="server" Text='<%$ Resources:Resource,L_WARD %>'></asp:Label>
                                                   </td>--%>
                                               <tr>
                                                   <td class="FormLabel" style="color:#fff !important">
                                                       <asp:Label
                                                           ID="lblWRegion"
                                                           runat="server"
                                                           Text='<%$ Resources:Resource,L_REGION %>'>
                                                       </asp:Label>
                                                   </td>
                                                   <td class="DataEntry">
                                                       <asp:DropDownList ID="ddlWRegion" runat="server" Width="110px" AutoPostBack="true" />
                                                   </td>
                                               </tr>
                                               <tr>
                                                   <td class="FormLabel" style="color:#fff !important">
                                                       <asp:Label
                                                           ID="lblWDistrict"
                                                           runat="server"
                                                           Text='<%$ Resources:Resource,L_DISTRICT %>'>
                                                       </asp:Label>
                                                   </td>
                                                   <td class="DataEntry">
                                                       <asp:DropDownList ID="ddlWDistrict" runat="server" Width="110px" AutoPostBack="true" />
                                                   </td>
                                               </tr>
                                               <tr>
                                                   <td>&nbsp;</td>
                                                   <td>&nbsp;</td>
                                               </tr>
                                           </table>
                                       </div>



                                       <asp:GridView ID="gvWards" runat="server"
                                           AutoGenerateColumns="False"
                                           EmptyDataText='<%$ Resources:Resource,M_NOWARDS %>'
                                           DataKeyNames="WardId,WardName"
                                          
                                           CssClass="mGrid"
                                           PagerStyle-CssClass="pgr"
                                           AlternatingRowStyle-CssClass="alt"
                                           SelectedRowStyle-CssClass="srs">

                                           <Columns>
                                               <asp:BoundField DataField="WardCode" HeaderText='<%$ Resources:Resource,L_WARDCODE%>'></asp:BoundField>
                                           </Columns>
                                           <Columns>
                                               <asp:BoundField DataField="WardName" HeaderText='<%$ Resources:Resource,L_WARD%>'></asp:BoundField>
                                           </Columns>
                                           <PagerStyle CssClass="pgr" />
                                           <SelectedRowStyle CssClass="srs" />
                                           <AlternatingRowStyle CssClass="alt" />
                                           <RowStyle CssClass="normal" />
                                       </asp:GridView>



                                       <%--</ContentTemplate>
                 </asp:UpdatePanel>--%>
                                   </asp:Panel>
                               </td>
                               <td style="width: 50px">
                                   <asp:ImageButton ID="ImgbtnMoveWard" ImageUrl="~/Images/GoLtr.bmp" runat="server" />
                               </td>
                               <td>

                                   <asp:Panel ID="pnlDistricts" runat="server" ScrollBars="Auto" CssClass="panel" Height="495px"  Width="220px" style="overflow-x: hidden !important;">

                                       <div>
                                           <table class="catlabel">

                                               <tr>
                                                   <td class="FormLabel" style="color:#fff !important">
                                                       <asp:Label
                                                           ID="lblDRegion"
                                                           runat="server"
                                                           Text='<%$ Resources:Resource,L_REGION %>'>
                                                       </asp:Label>
                                                   </td>
                                                   <td class="DataEntry">
                                                       <asp:DropDownList ID="ddlDRegion" runat="server" Width="110px" AutoPostBack="true" />
                                                   </td>

                                               </tr>
                                               <tr>
                                                   <td>&nbsp;</td>
                                                   <td>&nbsp;</td>
                                               </tr>
                                               <tr>
                                                    <td>&nbsp;</td>
                                                   <td>&nbsp;</td>
                                               </tr>
                                           </table>
                                       </div>
                                       <asp:GridView ID="gvDistricts" runat="server"
                                           AutoGenerateColumns="False"
                                           GridLines="None"
                                           CssClass="mGrid"
                                           PagerStyle-CssClass="pgr"
                                           DataKeyNames="DistrictId, DistrictName"
                                           EmptyDataText='<%$ Resources:Resource,M_NODISTRICTS %>'>
                                           <Columns>
                                               <asp:BoundField DataField="DistrictCode" HeaderText='<%$ Resources:Resource,L_DISTRICTCODE%>'></asp:BoundField>
                                           </Columns>
                                           <Columns>
                                               <asp:BoundField DataField="DistrictName" HeaderText='<%$ Resources:Resource,L_DISTRICT%>'></asp:BoundField>
                                           </Columns>
                                           <SelectedRowStyle CssClass="srs" />
                                           <PagerStyle CssClass="pgr" />
                                           <AlternatingRowStyle CssClass="alt" />
                                           <RowStyle CssClass="normal" />

                                       </asp:GridView>

                                       <%-- </ContentTemplate>
                    </asp:UpdatePanel>--%>
                                   </asp:Panel>


                               </td>
                               <td style='width: 30px !important'>
                                   <asp:ImageButton ID="ImgBtnMoveDistrict" ImageUrl="~/Images/GoLtr.bmp" runat="server" OnClientClick="return showMoveDistrictPopup();"/></td>
                               <td>
                                   <asp:Panel ID="pnlRegions" runat="server" ScrollBars="Auto" CssClass="panel" Height="495px"  Width="200px" style="overflow-x: hidden !important;">

                                       <div>
                                         <table  class="catlabel">
                                               <tr>
                                                <td>&nbsp;</td>
                                                   <td>&nbsp;</td>
                                           </tr>
                                           <tr>
                                               <td>&nbsp;</td>
                                                   <td>&nbsp;</td>
                                           </tr>
                                           <tr>
                                                <td>&nbsp;</td>
                                                   <td>&nbsp;</td>
                                           </tr>
                                         </table>
                                       </div>
                                       <asp:GridView ID="gvRegions" runat="server"
                                           AutoGenerateColumns="False"
                                           GridLines="None"
                                           CssClass="mGrid"
                                           PagerStyle-CssClass="pgr"
                                           DataKeyNames="RegionId,RegionName"
                                           EmptyDataText='<%$ Resources:Resource,M_NOREGIONS %>'>

                                           <Columns>
                                               <asp:BoundField DataField="RegionCode" HeaderText='<%$ Resources:Resource,L_REGIONCODE%>'></asp:BoundField>
                                           </Columns>
                                           <Columns>
                                               <asp:BoundField DataField="RegionName" HeaderText='<%$ Resources:Resource,L_REGION%>'></asp:BoundField>
                                           </Columns>
                                           <SelectedRowStyle CssClass="srs" />
                                           <PagerStyle CssClass="pgr" />
                                           <AlternatingRowStyle CssClass="alt" />
                                           <RowStyle CssClass="normal" />

                                       </asp:GridView>

                                       <%-- </ContentTemplate>
                    </asp:UpdatePanel>--%>
                                   </asp:Panel>
                               </td>




                           </tr>
                       </table>
                   </ContentTemplate>
               </asp:UpdatePanel>
            </div>
    
     
         
               
       <asp:Panel ID="pnlButtons" runat="server"   CssClass="panelbuttons" style="position:relative;z-index:10;">
        <table width="100%" cellpadding="10 10 10 10">
             <tr>
                    
                    <%-- <td  align="left" id="LocationsButtonPanel">                    
                        <input type="button" class="LocationsButton" id="btnAdd" value='<%$ Resources:Resource,B_ADD%>' name="btnAdd" runat="server" />
                        <input type="button" class="LocationsButton" id="btnEdit" value='<%$ Resources:Resource,B_EDIT%>' name="btnEdit" runat="server" />
                        <input type="submit" id="btnDelete" value='<%$ Resources:Resource,B_DELETE%>' name="btnDelete" runat="server" />                        
                    </td>--%>
                    
                    
                    <td align="center">
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
<asp:Content ID="footer"  ContentPlaceHolderID ="Footer" runat="server" Visible="true" >
 
    <asp:ValidationSummary ID="validationSummary" runat="server" HeaderText='<%$ Resources:Resource,V_SUMMARY%>' ValidationGroup="check" />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Label Text="" runat="server" ID="lblMsg"> </asp:Label>
        </ContentTemplate>
    </asp:UpdatePanel>
 

</asp:Content>


