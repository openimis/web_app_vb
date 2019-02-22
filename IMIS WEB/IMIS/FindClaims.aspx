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
<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/IMIS.Master" CodeBehind="FindClaims.aspx.vb" Inherits="IMIS.FindClaims" 
Title = '<%$ Resources:Resource,L_FINDCLAIM %>'%>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="ajax" %>

<asp:Content ID="HeadContent" contentplaceholderid="head" runat="server">
    <style type="text/css">
 .selectedRow{background:#99CCFF;color:black;
              }
#popup-div-body table{
    margin:auto;
        }
        #popup-div-body table tr > td{
           text-align:right;
            }

         table#DropDownSugTable
        {
            border-width: 0px;
            border-collapse: collapse;
        }
        table#DropDownSugTable th
        {
            background: #CCC;
            color: #303030;
            border-width: 0px;
        }
        table#DropDownSugTable td
        {
            border-width: 0px;
        }
        .pnlHiddenICDCodes
        {
            display: none;
            position: absolute;
            background: #CCCCCC;
            border: 1px solid #ccc;
            font-weight: normal;
            color: #000000;
            z-index: 100;
            padding: 3px;
            height: auto;
            cursor: pointer;
            width: auto;
        }
        .popup
        {
            width: 220px;
            height: 100px;
            background-color: White;
            z-index: 1002;
            font-size: 14px;
            text-align: center;
            border: solid 2px black;
            -webkit-border-radius: 12px;
            -moz-border-radius: 12px;
            position: absolute;
            top: 40%;
            left: 40%;
            padding-top: 8px;
        }
        .backentry
        {
            height: 643px;
        }
        .footer
        {
            top:auto;
        }
        .auto-style1 {
            height: 27px;
        }

</style>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="Body" Runat="Server">
    <script type="text/javascript">
    var ClaimStatus;
    var ClaimID;
    //var previousRow;
    //var previousClass;
  
    /** Ruzo Grid Row Selection 29 Aug 2014 >> Start **/
    function bindRowSelection() {
        var $trs = $('#<%=gvClaims.ClientID%> tr')
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
     }
     function fillSelectedRowData($row) {
         var claimId = $row.find("td").eq(9).html();
         $("#<%=hfClaimID.ClientID%>").val(claimId);
    }
    /** Ruzo Grid Row Selection 29 Aug 2014 >> End **/
    
   
    function toggleCheck(target) {
//        if (target.checked) {
            $('#<%=gvClaims.ClientID  %> tr').each(function(i) {
                $row = $(this); // current selected row reference.
                $cell1 = $row.find("td").eq(7); //   cell7 in the current row.
                $cell2 = $row.find("td").eq(8); //   cell8 in the current row.
                ClaimStatus = $.trim($cell1.html()).replace("&nbsp;", "");
                var $checkbx = $cell2.find("input[type=checkbox]").eq(0);
                if (ClaimStatus == '<%=imisgen.getMessage("T_ENTERED", True ) %>') {
                    $checkbx.attr("checked", target.checked);
                }
            });
            $('.ConditionCheck').trigger("change");
      }

        $(document).ready(function ()
        {
            var prm = Sys.WebForms.PageRequestManager.getInstance();
            prm.add_initializeRequest(InitializeRequest);
            prm.add_endRequest(EndRequest);

            InitAutoCompl();
        });

        function InitializeRequest(sender, args) {
        }

        function EndRequest(sender, args) {
            // after update occur on UpdatePanel re-init the Autocomplete
            InitAutoCompl();
        }

        function InitAutoCompl() {
            $("#<%=txtICDCode.ClientID %>").focus(function () {
                var datasource;
                $.ajax({
                    url: 'AutoCompleteHandlers/AutoCompleteHandler.ashx',
                    // data: JSON.stringify({ prefix: request.term }),
                    dataType: "json",
                    type: "GET",
                    async: false,
                    cache: false,

                    success: function (data) {
                        // console.log(data);
                        datasource = data;
                    }

                });
                var ds = new AutoCompletedataSource(datasource);
                console.log(datasource);
                $("#<%=txtICDCode.ClientID %>").autocomplete({
                    source: function (request, response) {
                        var data = ds.filter(request);

                        response($.map(data, function (item, id) {

                            return {
                                label: item.ICDNames, value: item.ICDNames, value2: item.ICDCode, id: item.ICDID
                            };
                        }));

                    },
                    select: function (e, u) {
                        $('#<% = hfICDID.ClientID%>').val(u.item.id);
                        $('#<% = hfICDCode.ClientID%>').val(u.item.value2);
                    }
                });
            });
        }
         
    

 
   

    $(document).ready(function () {
        if ($("#<%=hfSubmitClaims.ClientID %>").val() == "") {
             $("#<%=hfSubmitClaims.ClientID %>").val(0);
         }
         $("#<%=hfdeleteClaim.ClientID %>").val("");

      $("#<%=hfClaimAdminAdjustibility.ClientID %>").val(("<%= IMIS.General.getControlSetting("ClaimAdministrator") %>"));

     });
      
      function HandleDelete(btn){
          if (btn == "ok") {
              __doPostBack("<%=B_DELETE.ClientID %>", "");
          } else if (btn == "cancel") {
             // $("#<%=lblMsg.ClientID %>").html("");
          }
      }
      function SubmitClaimStatusFn(btn) {
          if (btn == "ok") {
              __doPostBack("<%=B_SUBMIT.ClientID %>", "");
          } else if (btn == "cancel") {
            //return false;
          }
      }
      var ClearflagMsg = false;
      function pageLoadExtend() {

          bindRowSelection();

          showInsureePopupSearchResult();
          $('#btnCancel').click(function() {
              $('#SelectPic').hide();
          });
          if ($('#<%=gvClaims.ClientID  %> tr:not(:first)').length != 0) {
              ClearflagMsg = true;
          }
          if (ClearflagMsg == true && $("#<%=hfdeleteClaim.ClientID %>").val() !=1) {
              $("#<%=lblMsg.ClientID %>").html("");
              ClearflagMsg = false;
          }
          $("#<%=btnSearch.ClientID %>").click(function () {
              //ChangeClass($('#<%=gvClaims.ClientID  %> tr:not(:first)').eq(0).attr("id"), 1);
              $("#<%=hfdeleteClaim.ClientID %>").val("");
              $("#<%=lblMsg.ClientID %>").html("");


              var passed = true;
              $DateControls = $('.dateCheck');
              $DateControls.each(function () {
                  if ($(this).val().length > 0 && !isValidJSDate($(this).val())) {
                      $('#<%=lblMsg.ClientID%>').html('<%= imisgen.getMessage("M_INVALIDDATE", True)%>');
                      $(this).focus();
                      passed = false;
                      return false;
                  }
              });
              if (passed == false) {
                  return false;
              }


          });

          if ($("#<%=hfSubmitClaims.ClientID %>").val() != 0) {
              popup.acceptBTN_Text = '<%=imisgen.getMessage("L_OK", True)%>';
              popup.alert($("#<%=hfSubmitClaims.ClientID %>").val());
              $("#<%=hfSubmitClaims.ClientID %>").val("");
          }
          $("#<%=B_SUBMIT.ClientID %>").click(function() {
              $("#<%=lblMsg.ClientID %>").html("");
              var flagSubmitClaim = false;

              $('#<%=gvClaims.ClientID  %> tr:not(:first)').each(function() {
                  if ($(this).find("td").eq(8).find("input[type=checkbox]").is(":checked")) {
                      flagSubmitClaim = true;
                      return;
                  }
              });
              var htmlMsgSubmit = "";
              if (flagSubmitClaim == true) {

                  htmlMsgSubmit = '<%= imisgen.getMessage("M_CONFIRMSUBMITCLAIMS", True)%>';

                  popup.acceptBTN_Text = '<%=imisgen.getMessage("L_YES", True)%>';
                  popup.rejectBTN_Text = '<%=imisgen.getMessage("L_NO", True)%>';
                  popup.confirm(htmlMsgSubmit, SubmitClaimStatusFn);
                  return false;

              } else {
                  $("#<%=lblMsg.ClientID %>").html('<%= imisgen.getMessage("M_VALIDATESUBMITCLAIMS", True)%>');
                  return false;
              }
          });

          $("#<%=B_ADD.ClientID %>").click(function () {
              if ($("#<%=ddlDistrict.ClientID %>").val() == 0) {
                 $("#<%=ddlDistrict.ClientID %>").focus();
                  $('#<%=lblMsg.ClientID %>').html('<%= imisgen.getMessage("M_VALIDATEADDBUTTONDISTRICT", True)%>');
                  return false;
              } else if ($("#<%=ddlHFCode.ClientID %>").val() == 0) {
                  $("#<%=ddlHFCode.ClientID %>").focus();
                  $('#<%=lblMsg.ClientID %>').html('<%= imisgen.getMessage("M_VALIDATEADDBUTTONHFCode", True)%>');
                  return false;
              } else if (($("#<%=ddlClaimAdmin.ClientID %>").val() == 0 || $("#<%=ddlClaimAdmin.ClientID %>").val() == null) && $("#<%=hfClaimAdminAdjustibility.ClientID %>").val() == "M") {
                  $("#<%=ddlClaimAdmin.ClientID %>").focus();
                  $('#<%=lblMsg.ClientID %>').html('<%= imisgen.getMessage("M_VALIDATEADDBUTTONCLAIMADMIN", True)%>');
                  return false;
              }
              return true;
          });

          $("#<%=B_DELETE.ClientID %>").click(function() {
          var htmlMsg = '<%= imisgen.getMessage("M_CONFIRMCLAIMDELETE", True)%>';
          popup.acceptBTN_Text = '<%=imisgen.getMessage("L_YES", True)%>';
          popup.rejectBTN_Text = '<%=imisgen.getMessage("L_NO", True) %>';
              popup.confirm(htmlMsg, HandleDelete);
              return false;
          });
          //alert("This page load");
          var $element = $('.ConditionCheck');
          $element.change(function() {
            $("#<%=lblMsg.ClientID %>").html("");
              $StatusCell = $(this).parent().prev()
              //  alert($.trim($StatusCell.html()).replace("&nbsp;", "") == "Entered");
              if ($.trim($StatusCell.html()).replace("&nbsp;", "") != '<%=imisgen.getMessage("T_ENTERED", True) %>') {
                  //alert($(this).prop("tagName"));
                  $(this).find("input[type=checkbox]").attr("checked", false);
              }
          });
      }


      function pageLoadExtend() {
          dropdown.init($("#DropDownSugTable"), function () {
              $('.ClaimValue').eq(0).trigger("change");
          });

          $(".disabled a").unbind("click").mouseover(function () {
              $(this).css("opacity", 0.2);
          });
          showInsureePopupSearchResult();
          $('#btnCancel').click(function () {
              $('#SelectPic').hide();
          });

        }



</script>


<asp:UpdatePanel ID="upClaim" runat="server" RenderMode="Inline" > 
<Triggers>
<asp:PostBackTrigger ControlID="B_SUBMIT" />
<asp:PostBackTrigger ControlID="B_ADD" />
</Triggers>
<ContentTemplate>
  <div class="divBody" >
      <asp:HiddenField ID="hfICDID" runat="server"/>
       <asp:HiddenField ID="hfICDCode" runat="server"/>
        <asp:HiddenField ID="hfClaimAdminAdjustibility" runat="server" Value="" />
        <table class="catlabel">
            <tr>
                <td >
                   <asp:label  
                           ID="L_SELECTCRITERIA"  
                           runat="server" 
                           Text='<%$ Resources:Resource,L_SELECTCRITERIA %>'>
                   </asp:label>   
                    
                </td>
            </tr>
        </table>
       
 

        <asp:Panel ID="pnlTop" runat="server"  CssClass="panelTop"  Height="165px"  GroupingText='<%$ Resources:Resource,L_CLAIMDETAILS %>' oncontextmenu="return false;">
               
            <table id="DropDownSugTable" border="0px" style="display: none; width: 100%; border-collapse: collapse;
                border: 0px solid #CCC;">
                <tr style="color: #303030; background: #C0C0C0;">
                    <th>
                        <%=imisgen.getMessage("L_CODE", True)%>
                    </th>
                    <th>
                        <%=imisgen.getMessage("L_NAME", True)%>
                    </th>
                   
                </tr>
            </table> 
      <table >
          <tr>
            <td class ="FormLabel">
                     <asp:Label ID="L_REGION" runat="server" Text="<%$ Resources:Resource,L_REGION %>"></asp:Label>
                 </td>
                <td class="DataEntry">
                     <asp:DropDownList ID="ddlRegion" runat="server" AutoPostBack="true">
                     </asp:DropDownList>
              </td>               
                <td class ="FormLabel">
                     <asp:Label ID="lblHFName" runat="server" Text='<%$ Resources:Resource,L_HFNAME%>'></asp:Label>                    
                </td>
                <td class ="DataEntry">
                <asp:TextBox ID="txtHFName" runat="server" Text="" MaxLength="100"></asp:TextBox> 
                    
                 </td>
            <td class="FormLabel">
                     <asp:Label ID="lblVisitDateFrom" runat="server" Text='<%$ Resources:Resource,L_VISITDATEFROM%>'></asp:Label>
                </td>
            <td class ="DataEntry" style="width:190px">
                <asp:TextBox ID="txtVisitDateFrom" runat="server" Text="" width="100px" CssClass="dateCheck"></asp:TextBox>
                  <asp:Button ID="btnClaimFrom" runat="server"  padding-bottom="3px" 
                       Class="dateButton" />
                    <ajax:CalendarExtender ID="CalendarExtender1" runat="server" 
                        Format="dd/MM/yyyy" PopupButtonID="btnClaimFrom" TargetControlID="txtVisitDateFrom">
                    </ajax:CalendarExtender>
                   
                    <asp:Label ID="lblVisitDateTo" runat="server" Text='<%$ Resources:Resource,L_TO%>' class ="FormLabel" style="margin-left:15px"></asp:Label>
                  </td>
             <td class ="DataEntry">
                  <asp:TextBox ID="txtVisitDateTo" runat="server" Text="" width="100px" CssClass="dateCheck"></asp:TextBox>
                  
                 <asp:Button ID="btnClaimTo" runat="server"  Class="dateButton" padding-bottom="3px" 
                       />
                    <ajax:CalendarExtender ID="txtClaimDate_CalendarExtender" runat="server" 
                        Format="dd/MM/yyyy" PopupButtonID="btnClaimTo" TargetControlID="txtVisitDateTo">
                    </ajax:CalendarExtender>
                 </td>  
            </tr>
          <tr>
            <td class="FormLabel">
                   <asp:Label ID="L_District" runat="server" Text="<%$ Resources:Resource,L_DISTRICT %>"></asp:Label>
                </td>
                <td class ="DataEntry">
                    <asp:DropDownList ID="ddlDistrict" runat="server" AutoPostBack="true">
                    </asp:DropDownList>
                    
                </td>
                <td class ="FormLabel">
                    <asp:Label ID="lblReviewStatus" runat="server" 
                        Text='<%$ Resources:Resource,L_REVIEWSTATUS%>'></asp:Label>
                </td>
                <td class="DataEntry">
                     <asp:DropDownList ID="ddlReviewStatus" runat="server" >
                     </asp:DropDownList>
                 </td>                             
               <td class="FormLabel">
                    <asp:Label ID="lblClaimedDateFrom" runat="server" Text='<%$ Resources:Resource,L_CLAIMDATEFROM%>'></asp:Label>
               </td>
               <td class="DataEntry" style="width:190px">
                   <asp:TextBox ID="txtClaimedDateFrom" runat="server" Text="" width="100px" CssClass="dateCheck"></asp:TextBox>
                   <asp:Button ID="btnClaimedDateFrom" runat="server"  padding-bottom="3px" 
                        Class="dateButton" />
                    <ajax:CalendarExtender ID="CalendarExtender2" runat="server" 
                        Format="dd/MM/yyyy" PopupButtonID="btnClaimedDateFrom" TargetControlID="txtClaimedDateFrom">
                    </ajax:CalendarExtender>
                     <asp:Label ID="lblClaimedDateTo" runat="server" Text='<%$ Resources:Resource,L_TO%>' class ="FormLabel" style="margin-left:15px"></asp:Label>
               </td>
               <td class ="DataEntry">
                  <asp:TextBox ID="txtClaimedDateTo" runat="server" Text="" width="100px" CssClass="dateCheck"></asp:TextBox>                  
                   <asp:Button ID="btnClaimedDateTo" runat="server"  Class="dateButton" padding-bottom="3px" 
                       />
                    <ajax:CalendarExtender ID="CalendarExtender3" runat="server" 
                        Format="dd/MM/yyyy" PopupButtonID="btnClaimedDateTo" TargetControlID="txtClaimedDateTo">
                    </ajax:CalendarExtender>
                 </td>
           </tr>
          <tr>
            <td class ="FormLabel">
                <asp:Label ID="lblHFCode" runat="server" Text="<%$ Resources:Resource,L_HFCODE %>"></asp:Label>
            </td>
            <td class="DataEntry">
                <asp:DropDownList ID="ddlHFCode" runat="server" AutoPostBack="true">
                </asp:DropDownList>
            </td>
            <td class ="FormLabel">
                     <asp:Label ID="lblFBStatus" runat="server" 
                        Text='<%$ Resources:Resource,L_FBSTATUS%>'></asp:Label>
                 </td>
            <td class="DataEntry">
                     <asp:DropDownList ID="ddlFBStatus" runat="server" >
                     </asp:DropDownList>
                 </td>
            <td class ="FormLabel">
                     <asp:Label ID="lblICD" runat="server" 
                        Text='<%$ Resources:Resource,L_ICD%>'></asp:Label>
                 </td>
            <td class="DataEntry">
                <%-- <asp:TextBox ID="txtICDCode" runat="server" MaxLength="6"></asp:TextBox>--%>
                 <%--    <asp:DropdownList ID="ddlICD" runat="server"  class="cmb autosuggest ddlICD" autocomplete="off" >
                     </asp:DropdownList>--%>
            <asp:TextBox ID="txtICDCode" runat="server" MaxLength="8"  class="cmb txtICDCode" autocomplete="off"></asp:TextBox>

            </td>                  
            <td class="DataEntry" >
                  
            </td>                          
           </tr>
           <tr>           
              <td class="FormLabel">
                  <asp:Label ID="lblClaimAdmin0" runat="server" class="FormLabel" Text="<%$ Resources:Resource,L_CLAIMADMIN%>"></asp:Label>
              </td>
              <td class ="DataEntry">
                  <asp:DropDownList ID="ddlClaimAdmin" runat="server">
                  </asp:DropDownList>
              </td>
              <td class="FormLabel">
                      <asp:Label ID="lblClaimStatus" runat="server" 
                        Text='<%$ Resources:Resource,L_CLAIMSTATUS%>'></asp:Label>
                  </td>
              <td class="DataEntry">
                    <asp:DropDownList ID="ddlClaimStatus" runat="server">
                     </asp:DropDownList>
                </td>
              <td class ="FormLabel">
                     <asp:Label ID="lblBatchRun" runat="server" 
                        Text='<%$ Resources:Resource,L_BATCHRUN%>'></asp:Label>
                 </td>
              <td class="DataEntry">
                     <asp:DropDownList ID="ddlBatchRun" runat="server" >
                     </asp:DropDownList>
                 </td>
              <td class="FormLabel">                
                   <%--<asp:Button class="button" ID="btnSearch" runat="server" 
                          Text='<%$ Resources:Resource,B_SEARCH %>' >
                    </asp:Button>--%>
                 </td>
           </tr>            
           <tr>
                <td class="FormLabel">
                     <asp:Label ID="lblCHFID" runat="server" Text="<%$ Resources:Resource,L_CHFID %>"></asp:Label>
                </td>
                <td class="DataEntry">
                    <%--<asp:DropDownList ID="ddlClaimCode" runat="server"  ></asp:DropDownList>--%>
                    <asp:TextBox ID="txtCHFID" runat="server" maxlength="12"></asp:TextBox>
                </td>
                <td class="FormLabel">
                    <asp:Label ID="lblClaimCode0" runat="server" Text="<%$ Resources:Resource,L_CLAIMCODE%>"></asp:Label>
                </td>
                <td class="DataEntry">
                    <asp:TextBox ID="txtClaimCode" runat="server" MaxLength="8"></asp:TextBox>
                </td>
                <td class="FormLabel">
                    
                    <asp:Label ID="lblVisitType" runat="server" Text="<%$ Resources:Resource,L_VISITTYPE %>"></asp:Label>
                    
                 </td>
                <td class="DataEntry">
                    
                    <asp:DropDownList ID="ddlVisitType" runat="server" width="150px">
                    </asp:DropDownList>
                    
                </td>
                <td class="FormLabel">
                    <asp:Button class="button" ID="btnSearch" runat="server" 
                          Text='<%$ Resources:Resource,B_SEARCH %>' >
                    </asp:Button>
                </td>
            </tr>
</table>
        </asp:Panel>
      
        <table>
        <tr>
        <td>
        <table class="catlabel">
             <tr>
                <td >
                       <asp:label  
                           ID="L_CLAIMSFOUND"  
                           runat="server" 
                           Text='<%$ Resources:Resource,L_CLAIMSFOUND %>'></asp:label>   
               </td>
               
                </tr>
            
            </table>
        </td>
        <td align="right" >
       <asp:Label ID="lblSelectToSubmit" runat="server" Text='<%$ Resources:Resource,L_SELECTTOSUBMIT %>' style="margin-left:579px" CssClass="FormLabel"></asp:Label>
       <asp:CheckBox ID="chkboxSubmitAll" runat="server" onClick="toggleCheck(this);"/>
        </td>
        
        </tr>
        </table>
        
        <asp:Panel ID="pnlBody" runat="server"  CssClass="panelBody" Height="300px" 
            ScrollBars="Vertical" >
            <asp:GridView ID="gvClaims" runat="server" 
                AutoGenerateColumns="False"
                GridLines="None"
                AllowPaging="false"
                CssClass="mGrid"
                EmptyDataText='<%$ Resources:Resource,M_NOCLAIMS %>'
                PagerStyle-CssClass="pgr"
                DataKeyNames="ClaimID,RowID,HfID"
                
                AlternatingRowStyle-CssClass="alt"
                SelectedRowStyle-CssClass="srs" PageSize="15" >
                <Columns>
                  
                    <asp:HyperLinkField DataNavigateUrlFields = "ClaimID" DataTextField="ClaimCode" DataNavigateUrlFormatString = "Claim.aspx?c={0}" HeaderText='<%$ Resources:Resource,L_CLAIMCODE %>' HeaderStyle-Width ="100px"  >
                         <HeaderStyle Width="50px" />
                     </asp:HyperLinkField>
                      <asp:BoundField DataField="HFName"  HeaderText='<%$ Resources:Resource,L_HFName %>' SortExpression="HFName" HeaderStyle-Width="150px">  
                          <HeaderStyle Width="150px" />
                    </asp:BoundField> 
                    <asp:BoundField DataField="DateClaimed"  DataFormatString="{0:d}" HeaderText='<%$ Resources:Resource,L_DATECLAIMED %>' SortExpression="DateClaimed" HeaderStyle-Width="70px">  
                        <HeaderStyle Width="70px" />
                    </asp:BoundField>
                     <asp:BoundField DataField="FeedbackStatus" HeaderText='<%$ Resources:Resource,L_FEEDBACKSTATUS %>' SortExpression="FeedbackStatus" HeaderStyle-Width="70px">  
                         <HeaderStyle Width="70px" />
                    </asp:BoundField>
                      <asp:BoundField DataField="ReviewStatus"  HeaderText='<%$ Resources:Resource,L_REVIEWSTATUS %>' SortExpression="ReviewStatus" HeaderStyle-Width="70px">  
                          <HeaderStyle Width="70px" />
                    </asp:BoundField>                
    
                    <asp:BoundField DataField="Claimed"  DataFormatString="{0:n2}" HeaderText='<%$ Resources:Resource,L_CLAIMED %>' SortExpression="Claimed" ItemStyle-HorizontalAlign="Right">
                     <HeaderStyle Width="50px" />  
                        <ItemStyle HorizontalAlign="Right" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Approved"  DataFormatString="{0:n2}" HeaderText='<%$ Resources:Resource,L_APPROVED %>' SortExpression="Approved" HeaderStyle-Width="70px" ItemStyle-HorizontalAlign="Right">  
                        <HeaderStyle Width="70px" />
                        <ItemStyle HorizontalAlign="Right" />
                    </asp:BoundField>
                     <asp:BoundField DataField="ClaimStatus"   HeaderText='<%$ Resources:Resource,L_CLAIMSTATUS %>' SortExpression="ClaimStatus" >
                     <HeaderStyle Width="60px" /> </asp:BoundField>
                    <asp:TemplateField  >
                                    <ItemTemplate >
                                    <asp:CheckBox ID="chkbgridSubmit" runat="server"  CssClass="ConditionCheck" Checked="false"  /> 
                                       
                                    </ItemTemplate>
                                    <%--<HeaderTemplate >
                                     <asp:Label ID="lblSelectToProcess" runat="server" Text="" ></asp:Label> 
                                    </HeaderTemplate>--%>
                                    <ItemStyle Width="15px"  />
                     </asp:TemplateField>
                    <asp:BoundField DataField="ClaimID" > <ItemStyle CssClass="hidecol" /><HeaderStyle CssClass="hidecol"  /></asp:BoundField >
               <asp:BoundField DataField="RowID" > <ItemStyle CssClass="hidecol" /><HeaderStyle CssClass="hidecol"  /></asp:BoundField >
               <asp:BoundField DataField="HfID" > <ItemStyle CssClass="hidecol" /><HeaderStyle CssClass="hidecol"  /></asp:BoundField >
                </Columns>
                <PagerStyle CssClass="pgr" />
                <SelectedRowStyle CssClass="srs" />
                <AlternatingRowStyle CssClass="alt" />
                <RowStyle CssClass="normal" Wrap="False" />
            </asp:GridView>
            <asp:HiddenField ID="hfClaimID" runat="server" />
             <asp:HiddenField ID="hfHFID" runat="server" />
             <asp:HiddenField ID="hfdeleteClaim" runat="server" />
             <asp:HiddenField ID="hfSubmitClaims" runat="server" />
        </asp:Panel>
        </div>
          


       <asp:Panel ID="pnlButtons" runat="server"   CssClass="panelbuttons" >
        <table width="100%" cellpadding="10 10 10 10" align="center">
             <tr align="center">
                    
                    <td  align="left">
                       <asp:Button 
                        ID="B_ADD" 
                        runat="server" 
                        Text='<%$ Resources:Resource,B_ADD%>'
                          />
                    </td>
                    <td align="center">
                       <asp:Button 
                        ID="B_LOAD" 
                        runat="server" 
                        Text='<%$ Resources:Resource,B_LOAD%>'
                         ValidationGroup="check"  />
                  
                    </td>
                   
                     <td  align="center">
                       <asp:Button 
                        ID="B_DELETE" 
                        runat="server" 
                        Text='<%$ Resources:Resource,B_DELETE%>'
                          />
                    </td>
                     <td  align="center">
                       <asp:Button 
                        ID="B_SUBMIT" 
                        runat="server" style="font-weight:bold; color:Red" 
                        Text='<%$ Resources:Resource,B_SUBMIT%>'
                          />
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
   </ContentTemplate></asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Footer" Runat="Server" Visible="true" >
    <asp:UpdatePanel ID="uplblMsg" runat="server"><ContentTemplate>
    <asp:Label text="" runat="server" ID="lblMsg"> </asp:Label>
    </ContentTemplate></asp:UpdatePanel>
</asp:Content>
