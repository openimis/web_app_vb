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
<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/IMIS.Master" CodeBehind="ClaimOverview.aspx.vb" Inherits="IMIS.ClaimOverview" 
Title = '<%$ Resources:Resource,L_CLAIMOVERVIEW %>'%>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="ajax" %>

<asp:Content ID="content4" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
#popup-div table
{
    margin:auto;
        }
        #popup-div table tr > td
        {
           text-align:right
            }
  #popup-div-body table
{
    margin:auto;
        }
        #popup-div-body table tr > td
        {
           text-align:right
            }

</style>
<script type="text/javascript">
        DEFAULT_REVIEW_RANDOM = "5";
        DEFAULT_REVIEW_VALUE = "40000";
        DEFAULT_REVIEW_VARIANCE = "50";

 var ClaimStatus;
 var clearflag = false;
 
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
     if (target.checked) {
         $('#<%=gvClaims.ClientID  %> tr:not(:first)').each(function(i) {
             $row = $(this); // current selected row reference.
             $cell7 = $row.find("td").eq(7); //   cell7 in the current row.
             $cell8 = $row.find("td").eq(8); //   cell8 in the current row.
             $cell3 = $row.find("td").eq(3); //   cell3 in the current row.
             $cell4 = $row.find("td").eq(4); //   cell4 in the current row.
             ClaimStatus = $.trim($cell7.html()).replace("&nbsp;", "");
             var $checkbx = $cell8.find("input[type=checkbox]").eq(0);
             var $ddlFeedbackStatus = $cell3.find("Select").eq(0);
             var $ddlReviewStatus = $cell4.find("Select").eq(0);
             var FeedbackStatus = $ddlFeedbackStatus.val();
             var ReviewStatus = $ddlReviewStatus.val();
             if (ClaimStatus == '<%=imisgen.getMessage("T_CHECKED", True)%>' && FeedbackStatus != 1 && ReviewStatus != 1) {
                 $checkbx.attr("checked", true);
                 $('.ConditionCheck').trigger("change");
             }
         });
          } else {
     $('#<%=gvClaims.ClientID  %> tr:not(:first)').each(function(i) {
         $row = $(this); // current selected row reference.
         $cell7 = $row.find("td").eq(7); //   cell7 in the current row.
         $cell8 = $row.find("td").eq(8); //   cell8 in the current row.
         $cell3 = $row.find("td").eq(3); //   cell3 in the current row.
         $cell4 = $row.find("td").eq(4); //   cell4 in the current row.
         ClaimStatus = $.trim($cell7.html()).replace("&nbsp;", "");
         var $checkbx = $cell8.find("input[type=checkbox]").eq(0);
         var $ddlFeedbackStatus = $cell3.find("Select").eq(0);
         var $ddlReviewStatus = $cell4.find("Select").eq(0);
         
         var FeedbackStatus = $ddlFeedbackStatus.val();
         var ReviewStatus = $ddlReviewStatus.val();
         if (ClaimStatus == '<%=imisgen.getMessage("T_CHECKED", True)%>' && FeedbackStatus != 1 && ReviewStatus != 1) {
             $checkbx.attr("checked", false);
             $('.ConditionCheck').trigger("change");
         }
     });
    
     }

    }

// ICDCode AutoComplete TextBox Control Start
    $(document).ready(function () {
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_initializeRequest(InitializeRequest);
        prm.add_endRequest(EndRequest);

        InitAutoCompl();
    });

    function InitializeRequest(sender, args) {
    }

    function EndRequest(sender, args) {
        InitAutoCompl();
    }


    function InitAutoCompl() {
        $("#<%=txtICDCode.ClientID %>").autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: 'AutoCompleteHandlers/AutoCompleteHandler.ashx',
                    // data: JSON.stringify({ prefix: request.term }),
                    data: { ICDCode: $("#<%=txtICDCode.ClientID %>").val() },

                    dataType: "json",
                    type: "POST",

                    success: function (data) {
                        response($.map(data, function (item, id) {
                            return { label: item.ICDNames, value: item.ICDNames, id: item.ICDID };
                        }));
                    },
                    error: function (response) {
                        alert(response.responseText);
                    },
                    failure: function (response) {
                        alert(response.responseText);
                    }
                });
            },
            select: function (e, u) {
                $('#<% = hfICDID.ClientID%>').val(u.item.id);

            },
            minLength: 1
        });
    }

 $(document).ready(function() {
    
    if( $("#<%=hfSelectionExecute.ClientID %>").val() == ""){
        $("#<%=hfSelectionExecute.ClientID %>").val(0);
    }
     if ($("#<%=hfProcessClaims.ClientID %>").val() == "") {
         $("#<%=hfProcessClaims.ClientID %>").val(0);
     }
     
     var flagSelectedRow = false;

     $('#<%=gvClaims.ClientID  %> tr:not(:first)').each(function(i) {
         if ($(this).find("td").eq(9).html() == $("#<%=hfClaimID.ClientID %>").val()) {
             //ChangeClass($(this).attr("id"), i, true);
             var yposTR = $(this).offset().top;
             var ypos = $('#<%=gvClaims.ClientID  %>').offset().top;
             $('#<%=gvClaims.ClientID  %>').parent().parent().scrollTop(yposTR - ypos - 200);
             flagSelectedRow = true;
             return false;
         }
     });

 });
 function SelectionUpdateCriteria() {
         if ($("#<%=chkRandom.ClientID %>").is(":checked")) {
             $("#<%=chkValue.ClientID %>").attr("disabled", true);
             $("#<%=chkVariance.ClientID %>").attr("disabled", true);
         } else {
             $("#<%=chkValue.ClientID %>").attr("disabled", false);
             $("#<%=chkVariance.ClientID %>").attr("disabled", false);
                 }
         if ($("#<%=chkValue.ClientID %>").is(":checked") || $("#<%=chkVariance.ClientID %>").is(":checked")) {
             $("#<%=chkRandom.ClientID %>").attr("disabled", true);
         } else {
            $("#<%=chkRandom.ClientID %>").attr("disabled", false);
         }
     }
     function ClaimCriteriaDetails() {
         var $ddlSelect = document.getElementById('<%=ddlSelectionType.ClientID %>');
         if ($ddlSelect.options[$ddlSelect.selectedIndex].value == 0) {
             EmptyAndDisableCriteria();
         } else {
             $("#<%=chkRandom.ClientID %>").attr("disabled", false);
             $("#<%=chkValue.ClientID %>").attr("disabled", false);
             $("#<%=chkVariance.ClientID %>").attr("disabled", false);
         }
     }

     function EmptyAndDisableCriteria() {
         $("#<%=chkRandom.ClientID %>").attr("checked", false);
         $("#<%=chkValue.ClientID %>").attr("checked", false);
         $("#<%=chkVariance.ClientID %>").attr("checked", false);
         $("#<%=chkRandom.ClientID %>").attr("disabled", true);
         $("#<%=chkValue.ClientID %>").attr("disabled", true);
         $("#<%=chkVariance.ClientID %>").attr("disabled", true);
         $("#<%=txtVariance.ClientID %>").val("");
         $("#<%=txtValue.ClientID %>").val("");
         $("#<%=txtRandom.ClientID %>").val("");
     }

     function ProcessClaimStatusFn(btn) {
         if (btn == "ok") {
             __doPostBack('<%=B_ProcessClaimStatus.ClientID %>', "");
         } else if (btn == "cancel") {
         return false;
         } 
     }

    function pageLoadExtend() {

        bindRowSelection();

         SelectionUpdateCriteria();
         
         showInsureePopupSearchResult();
         $('#btnCancel').click(function() {
             $('#SelectPic').hide();
         });
         if (clearflag == true) {
             $("#<%=lblMsg.ClientID %>").html("");
             clearflag = false;
         }
         if ($('#<%=gvClaims.ClientID  %> tr:not(:first)').length != 0 && $("#<%=hfClaimID.ClientID %>").val() == "") {
             //ChangeClass($('#<%=gvClaims.ClientID  %> tr:not(:first)').eq(0).attr("id"), 1, true);
            // $("#<%=lblMsg.ClientID %>").html("");
         }
         
         if ($("#<%=hfSelectionExecute.ClientID %>").val() != 0) {
             popup.acceptBTN_Text = '<%=imisgen.getMessage("L_OK", True)%>';
             popup.alert($("#<%=hfSelectionExecute.ClientID %>").val());
             $("#<%=hfSelectionExecute.ClientID %>").val("");
             
         }
         if ($("#<%=hfProcessClaims.ClientID %>").val() != 0) {
             popup.acceptBTN_Text = '<%=imisgen.getMessage("L_OK", True)%>';
             popup.alert($("#<%=hfProcessClaims.ClientID %>").val());
             $("#<%=hfProcessClaims.ClientID %>").val("");
         }
         $("#<%=B_ProcessClaimStatus.ClientID %>").click(function() {
             $("#<%=lblMsg.ClientID %>").html("");
             var flagProcessClaim = false;
             var RevBoolean = false;
             var FedBoolean = false;
             $('#<%=gvClaims.ClientID  %> tr:not(:first)').each(function() {
                 if ($(this).find("td").eq(8).find("input[type=checkbox]").is(":checked")) {
                     flagProcessClaim = true;
                     return;
                 }
             });
             var htmlMsgProcess = "";
             var htmlMsgProcess1 = "";
             var htmlMsgProcess2 = "";
             var htmlMsgProcess3 = "";
             if (flagProcessClaim == true) {
                 $('#<%=gvClaims.ClientID  %> tr:not(:first)').each(function() {
                     if ($(this).find("td").eq(8).find("input[type=checkbox]").is(":checked")) {
                         if ($(this).find("td").eq(3).find("select").data("currenctStatusFed") == 4) {
                             FedBoolean = true;
                         }
                         if ($(this).find("td").eq(4).find("select").data("currenstatusRev") == 4) {
                             RevBoolean = true;
                         }
                     }
                 });

                 if (FedBoolean == true && RevBoolean == false) {
                     htmlMsgProcess1 = '<%=imisgen.getMessage("M_ONEMORECLMFDPENDING", True)%>';
                 } else if (FedBoolean == false && RevBoolean == true) {
                 htmlMsgProcess2 = '<%=imisgen.getMessage("M_ONEMORECLMNOTREVIEWED", True)%>';
                 } else if (FedBoolean == true && RevBoolean == true) {
                 htmlMsgProcess3 = '<%=imisgen.getMessage("M_ONEMORENOTREVIEWEDFDPENDS", True)%>';
                 }
                 htmlMsgProcess = htmlMsgProcess1 + htmlMsgProcess2 + htmlMsgProcess3 + '<%=imisgen.getMessage("M_WLDLIKETOPROCESSCLAIMS", True)%>';

                 popup.acceptBTN_Text = '<%=imisgen.getMessage("L_YES", True)%>';
                 popup.rejectBTN_Text = '<%=imisgen.getMessage("L_NO", True)%>';
                 popup.confirm(htmlMsgProcess, ProcessClaimStatusFn);
                 return false;

             } else {
             $("#<%=lblMsg.ClientID %>").html('<%=imisgen.getMessage("M_NOCLMTOBEPROCESSED", True ) %>');
                 return false;
             }
         }); 
        var $ddlSelect = document.getElementById('<%=ddlSelectionType.ClientID %>');
        if ($('#<%=gvClaims.ClientID  %> tr:not(:first)').length == 0 && $ddlSelect.options[$ddlSelect.selectedIndex].value == 0) {
            EmptyAndDisableCriteria();
             $('#<%=ddlSelectionType.ClientID %>').attr("disabled", true);
         } else {

         
         if ($ddlSelect.options[$ddlSelect.selectedIndex].value != 0) {
             $("#<%=B_FEEDBACK.ClientID %>").hide();
             $("#<%=B_REVIEW.ClientID %>").hide();
             $(".ddlFeedBck").attr("disabled", true);
             $(".ddlReview").attr("disabled", true);
             $("#<%=B_ProcessClaimStatus.ClientID %>").hide();
             $("#<%=btnUpdateClaims.ClientID %>").hide();
             $(".ConditionCheck").attr("disabled", true);
             $("#<%=chkboxSelectToProcess.ClientID %>").hide();
             $("#<%=lblSelectToProcess.ClientID %>").hide();
             $('#<%=ddlSelectionType.ClientID %>').attr("disabled", false);    
         } else {
         EmptyAndDisableCriteria();
         $('#<%=ddlSelectionType.ClientID %>').attr("disabled", false);
             $("#<%=B_FEEDBACK.ClientID %>").show();
             $("#<%=B_REVIEW.ClientID %>").show();
             $(".ddlFeedBck").attr("disabled", false);
             $(".ddlReview").attr("disabled", false);
             $("#<%=B_ProcessClaimStatus.ClientID %>").show();
             $("#<%=btnUpdateClaims.ClientID %>").show();
             $(".ConditionCheck").attr("disabled", false);
             $("#<%=chkboxSelectToProcess.ClientID %>").show();
             $("#<%=lblSelectToProcess.ClientID %>").show();
         } 
         }

         $('#<%=btnSearch.ClientID %>').click(function() {
             $("#<%=hfClaimID.ClientID %>").val("");
             EmptyAndDisableCriteria();
             $('#<%=ddlSelectionType.ClientID %>').val(0);
             $('#<%=chkboxSelectToProcess.ClientID %>').attr("checked", false);
             toggleCheck($('#<%=chkboxSelectToProcess.ClientID %>'));
             //ChangeClass($('#<%=gvClaims.ClientID  %> tr:not(:first)').eq(0).attr("id"), 1);

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
         $('#<%=btnUpdateClaims.ClientID %>').click(function() {
             $("#<%=hfClaimID.ClientID %>").val("");
         });
         $('#<%=ddlSelectionType.ClientID %>').change(function() {
             ClaimCriteriaDetails();
             clearflag = true;
         });
     $(".numbersOnly").unbind("keypress");
     $(".numbersOnly").keypress(function(event) {
         var e = String.fromCharCode(event.which);
         if (e.match('[^0-9.]')) {
             return false;
         }
     });
     $(".ddlFeedBck").each(function() {
         $(this).data("currenctStatusFed", $(this).val());
     });

     $(".ddlReview").each(function() {
         $(this).data("currenstatusRev", $(this).val());
     });

     $('.ddlFeedBck').change(function() {

         if ($(this).data("currenctStatusFed") == 1) {
             if ($(this).val() == 8 || $(this).val() == 16) {
                 $(this).val(1);
                 return;
             }
         } else if ($(this).data("currenctStatusFed") == 2) {
                 if ($(this).val() == 8 || $(this).val() == 16) {
                     $(this).val(2);
                     return;
                   }
             }else if ($(this).data("currenctStatusFed") == 4) {
                 if ($(this).val() == 8 || $(this).val() == 16) {
                     $(this).val(4);
                     return;
                   }
             }else if ($(this).data("currenctStatusFed") == 8) {
                 if ($(this).val() == 16) {
                     $(this).val(8);
                     return;
             }
         }

     });

     $('.ddlReview').change(function() {
         if ($(this).data("currenstatusRev") == 1) {
             if ($(this).val() == 8 || $(this).val() == 16) {
                 $(this).val(1);
                 return;
             }
         } else if ($(this).data("currenstatusRev") == 2) {
             if ($(this).val() == 8 || $(this).val() == 16) {
                 $(this).val(2);
                 return;
             }
         } else if ($(this).data("currenstatusRev") == 4) {
             if ($(this).val() == 8 || $(this).val() == 16) {
                 $(this).val(4);
                 return;
             }
         } else if ($(this).data("currenstatusRev") == 8) {
             if ($(this).val() == 16) {
                 $(this).val(8);
                 return;
             }

         }
     });
     
     $("#<%=B_FEEDBACK.ClientID %>").click(function() {
         var flagFeedBack = true;
        
         $('#<%=gvClaims.ClientID  %> tr').each(function(i) {
             if ($(this).is(".srs")) {                                            
                 // alert($(this).prop("tagName"));
                 var $ddlFeedbck = $(this).find("td").eq(3).find("select");
                 var SelectedVal = $ddlFeedbck.find("option").eq($ddlFeedbck[0].selectedIndex).html();
                 //alert($ddlFeedbck.find("option").eq($ddlFeedbck[0].selectedIndex).html());
                 if (!(SelectedVal == '<%= imisgen.getMessage("T_DELIVERED", True)%>' || SelectedVal == '<%= imisgen.getMessage("T_SELECTEDFORFEEDBACK", True)%>')) {
                     $("#<%=lblMsg.ClientID %>").html('<%= imisgen.getMessage("M_CLAIMNOTFORFEEDBACKPROCESS", True ) %>');                   
                     flagFeedBack = false;
                 }
             }
         });
         return flagFeedBack;
     });

     $("#<%=B_REVIEW.ClientID %>").click(function() {
         var flagReview = true;
         $("#<%=hfReview.ClientID %>").val('<%= imisgen.getMessage("M_REVIEW", True)%>');
         $('#<%=gvClaims.ClientID  %> tr').each(function(i) {
             if ($(this).is(".srs")) {
                 var $ddlReview = $(this).find("td").eq(4).find("select");                 
                 var SelectedVal = $ddlReview.find("option").eq($ddlReview[0].selectedIndex).html();
                 //alert(SelectedVal);
                 //if (SelectedVal == '<%= imisgen.getMessage("T_IDLE", True ) %>') {
                     //$("#<%=lblMsg.ClientID %>").html('<%= imisgen.getMessage("M_CLAIMNOTFORREVIEWPROCESS", True ) %>');
                     //flagReview = false;
                 //} else 
                 if (SelectedVal == '<%= imisgen.getMessage("T_REVIEWED", True)%>') {
                    $("#<%=hfReview.ClientID %>").val('<%= imisgen.getMessage("T_REVIEWED", True)%>');
             } else if (SelectedVal != '<%= imisgen.getMessage("T_SELECTEDFORREVIEW", True)%>') {
                    $("#<%=hfReview.ClientID %>").val('<%= imisgen.getMessage("T_REVIEWNOT", True ) %>');
                 }
             }
         });
         return flagReview;
     });


     $("#<%=btnSelectionExecute.ClientID %>").click(function() {
         //alert("click");
         $("#<%=lblMsg.ClientID %>").html("");
         var flagUpdateSelect = true;
         var $ddlSelectType = document.getElementById("<%=ddlSelectionType.ClientID %>");
         var SelectedType = $ddlSelectType.options[$ddlSelectType.selectedIndex].value;
         //alert(SelectedType);
         if (SelectedType == 0) {
             $("#<%=lblMsg.ClientID %>").html('<%= imisgen.getMessage("M_SELECTREVIEWFEEDBACK", True)%>');
             flagUpdateSelect = false;
         } else {
             if ($('#<%=gvClaims.ClientID  %> tr:not(:first)').length == 0) {
                 // alert($('#<%=gvClaims.ClientID  %> tr:not(:first)').length);
                 return false;
             }
             var Statuslbl;
             var htmlMsg;
             var htmlMsg1;

             if (SelectedType == 1) {
                 htmlMsg = '<%= imisgen.getMessage("M_WLDLIKEREVIEWSELECTION", True)%>';

                 //For random selection
                 if ($('#<%=chkRandom.ClientID %>').is(":checked")) {
                     htmlMsg = '<%= imisgen.getMessage("M_REVIEWSELECTION", True)%>' + '<%=imisgen.getMessage("L_RANDOM", True ) %>' + ' : ' + $("#<%=txtRandom.ClientID %>").val() + '%';
                 }

                 //Value is selected and Variance is not
                 if ($('#<%=chkValue.ClientID %>').is(":checked") && $('#<%=chkVariance.ClientID %>').prop("checked") == false) {
                     htmlMsg = '<%= imisgen.getMessage("M_REVIEWSELECTION", True)%>' + '<%=imisgen.getMessage("L_VALUE", True)%>' + ' : ' + $("#<%=txtValue.ClientID %>").val();
                 }

                 //Only variance is selected
                 if ($('#<%=chkValue.ClientID %>').prop("checked") == false && $('#<%=chkVariance.ClientID %>').prop("checked") == true) {
                     htmlMsg = '<%= imisgen.getMessage("M_REVIEWSELECTION", True)%>' + '<%=imisgen.getMessage("L_VARIANCE", True)%>' + " : " + $("#<%=txtVariance.ClientID %>").val() + "%";
                 }
                 //Value and Variance both are selected
                 if ($('#<%=chkValue.ClientID %>').is(":checked") && $('#<%=chkVariance.ClientID %>').prop("checked") == true) {
                     htmlMsg = '<%= imisgen.getMessage("M_REVIEWSELECTION", True)%>' + '<%=imisgen.getMessage("L_VALUE", True)%>' + ' : ' + $("#<%=txtValue.ClientID %>").val() + "<br>" + '<%=imisgen.getMessage("L_VARIANCE", True)%>' + " : " + $("#<%=txtVariance.ClientID %>").val() + "%";
                 }

             } else {
                 htmlMsg = '<%= imisgen.getMessage("M_WLDLIKEFEEDBACKELECTION", True)%>'

                 //For random selection
                 if ($('#<%=chkRandom.ClientID %>').is(":checked")) {
                     htmlMsg = '<%= imisgen.getMessage("M_FEEDBACKSELECTION", True)%>' + '<%=imisgen.getMessage("L_RANDOM", True ) %>' + ' : ' + $("#<%=txtRandom.ClientID %>").val() + '%';
                 }

                 //Value is selected and Variance is not
                 if ($('#<%=chkValue.ClientID %>').is(":checked") && $('#<%=chkVariance.ClientID %>').prop("checked") == false) {
                     htmlMsg = '<%= imisgen.getMessage("M_FEEDBACKSELECTION", True)%>' + '<%=imisgen.getMessage("L_VALUE", True)%>' + ' : ' + $("#<%=txtValue.ClientID %>").val();
                 }

                 //Only variance is selected
                 if ($('#<%=chkValue.ClientID %>').prop("checked") == false && $('#<%=chkVariance.ClientID %>').prop("checked") == true) {
                     htmlMsg = '<%= imisgen.getMessage("M_FEEDBACKSELECTION", True)%>' + '<%=imisgen.getMessage("L_VARIANCE", True)%>' + " : " + $("#<%=txtVariance.ClientID %>").val() + "%";
                     }
                 //Value and Variance both are selected
                     if ($('#<%=chkValue.ClientID %>').is(":checked") && $('#<%=chkVariance.ClientID %>').prop("checked") == true) {
                     htmlMsg = '<%= imisgen.getMessage("M_FEEDBACKSELECTION", True)%>' + '<%=imisgen.getMessage("L_VALUE", True)%>' + ' : ' + $("#<%=txtValue.ClientID %>").val() + "<br>" + '<%=imisgen.getMessage("L_VARIANCE", True)%>' + " : " + $("#<%=txtVariance.ClientID %>").val() + "%";
                 }

             }


             popup.acceptBTN_Text = '<%=imisgen.getMessage("L_YES", True)%>';
             popup.rejectBTN_Text = '<%=imisgen.getMessage("L_NO", True ) %>';
             popup.confirm(htmlMsg, UpdateSelectTypeFN);
             return false;
         }
         return flagUpdateSelect;
     }); 
     
     $("#<%=chkRandom.ClientID %>").change(function() {

         var $txtboxRandom = $(this).parent().next().find("input[type=text]");
         if ($(this).is(":checked")) {
             if ($txtboxRandom.prop("disabled")) {

                 $txtboxRandom.attr("disabled", false);
                 $txtboxRandom.val(DEFAULT_REVIEW_RANDOM);
                 
             }
         } else {
             $txtboxRandom.attr("disabled", true);
             $txtboxRandom.val("");
            
         }
         SelectionUpdateCriteria();
     });

     $("#<%=chkValue.ClientID %>").change(function() {
         var $txtboxValue = $(this).parent().next().find("input[type=text]");
         if ($(this).is(":checked")) {
             if ($txtboxValue.prop("disabled")) {
                 $txtboxValue.attr("disabled", false);
                 $txtboxValue.val(DEFAULT_REVIEW_VALUE);
             }
         } else {
             $txtboxValue.attr("disabled", true);
             $txtboxValue.val("");
         }
         SelectionUpdateCriteria();
     });
     if ($("#<%=chkValue.ClientID %>").is(":checked")) {
         $("#<%=chkRandom.ClientID %>").attr("disabled", true);
    }
    
     $("#<%=chkVariance.ClientID %>").change(function() {
         var $txtboxVariance = $(this).parent().next().find("input[type=text]");
         if ($(this).is(":checked")) {
             if ($txtboxVariance.prop("disabled")) {
                 $txtboxVariance.attr("disabled", false);
                 $txtboxVariance.val(DEFAULT_REVIEW_VARIANCE);
             }
         } else {
             $txtboxVariance.attr("disabled", true);
             $txtboxVariance.val("");
         }
         SelectionUpdateCriteria();
     });
    
     var $element = $('.ConditionCheck');
     $element.change(function() {
         $StatusCell = $(this).parent().prev(); //Finds the ClaimStatus boundField
         $ReviewStat = $(this).parent().prev().prev().prev().prev().find("select"); //Finds the ReviewStatus dropdownlist control
         $FeedbackStat = $(this).parent().prev().prev().prev().prev().prev().find("select"); //Finds the FeedbackStatus dropdownlist control

         if ($FeedbackStat.data("currenctStatusFed") == 1 || $ReviewStat.data("currenstatusRev") == 1 || $StatusCell.html() != '<%=imisgen.getMessage("T_CHECKED", True ) %>') {
             $(this).find("input[type=checkbox]").attr("checked", false);
         }
        
     });

     if ($('#<%=gvClaims.ClientID  %> tr:not(:first)').length != 0) {
         $('#<%=gvClaims.ClientID  %> tr:not(:first)').each(function() {
             var ClmStatus = $(this).find("td").eq(7).html();
             if (ClmStatus == '<%= imisgen.getMessage("T_VALUATED", True)%>' || ClmStatus == '<%= imisgen.getMessage("T_PROCESSED", True)%>' || ClmStatus == '<%= imisgen.getMessage("T_REJECTED", True ) %>') {
                 $(this).find("td").eq(3).find("select").prop("disabled", true);
                 $(this).find("td").eq(4).find("select").attr("disabled", true);
             }
         });
     }
 }
 function UpdateSelectTypeFN(btn) {
     if (btn == "ok") {
         __doPostBack('<%=btnSelectionExecute.ClientID %>', "");
        
     } else if (btn == "cancel") {
        
     } 
 }
</script>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="Body" Runat="Server">
    <asp:UpdatePanel ID="upClaimDetail" runat="server" RenderMode="Inline">
 <Triggers>
 
 <asp:PostBackTrigger ControlID="B_ProcessClaimStatus" />
 <asp:PostBackTrigger ControlID="btnSelectionExecute" />
 <%--<asp:PostBackTrigger ControlID="btnSearch" />--%>
 <asp:PostBackTrigger ControlID="btnUpdateClaims" />
 </Triggers>
 <ContentTemplate>
 
  <div class="divBody" >
        <asp:HiddenField ID="hfICDID" runat="server"/>
        <asp:HiddenField ID="hfICDCode" runat="server"/>
        <table class="catlabel">
            <tr>
                <td >
                   <asp:label  
                           ID="L_SELECTCRITERIA"  
                           runat="server" 
                           Text='<%$ Resources:Resource,L_SELECTCRITERIA %>'> </asp:label>   
                    
                </td>
            </tr>
        </table>
       
        <asp:Panel ID="pnlTop" runat="server"  CssClass="panelTop" Height="165px"  GroupingText='<%$ Resources:Resource,L_CLAIMDETAILS%>' oncontextmenu="return false;">
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
                <td class ="DataEntry">
                <asp:TextBox ID="txtClaimFrom" runat="server" Text="" width="100px" CssClass="dateCheck"></asp:TextBox>
                  <asp:Button ID="btnClaimFrom" runat="server"   Class="dateButton" padding-bottom="3px" 
                         />
                    <ajax:CalendarExtender ID="CalendarExtender1" runat="server" 
                        Format="dd/MM/yyyy" PopupButtonID="btnClaimFrom" TargetControlID="txtClaimFrom">
                    </ajax:CalendarExtender>
                   
                    <asp:Label ID="lblVisitDateTo" runat="server" Text='<%$ Resources:Resource,L_TO%>' class ="FormLabel" style="margin-left:5px"></asp:Label>
                  </td>
                  <td >
                  <asp:TextBox ID="txtClaimTo" runat="server" Text="" width="100px" CssClass="dateCheck" ></asp:TextBox>
                  
                 <asp:Button ID="btnClaimTo" runat="server"  padding-bottom="3px" 
                        Class="dateButton" />
                    <ajax:CalendarExtender ID="txtClaimDate_CalendarExtender" runat="server" 
                        Format="dd/MM/yyyy" PopupButtonID="btnClaimTo" TargetControlID="txtClaimTo">
                    </ajax:CalendarExtender>
                 </td>  
            </tr>
            <tr>
            <td class="FormLabel">
                   <asp:Label ID="L_District" runat="server" Text="<%$ Resources:Resource,L_DISTRICT %>"> </asp:Label>
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
               <td class="DataEntry">
               <asp:TextBox ID="txtClaimedDateFrom" runat="server" Text="" width="100px" CssClass="dateCheck" ></asp:TextBox>
               <asp:Button ID="btnClaimedDateFrom" runat="server"  padding-bottom="3px" 
                         Class="dateButton"  />
                    <ajax:CalendarExtender ID="CalendarExtender2" runat="server" 
                        Format="dd/MM/yyyy" PopupButtonID="btnClaimedDateFrom" TargetControlID="txtClaimedDateFrom">
                    </ajax:CalendarExtender>
                     <asp:Label ID="lblClaimedDateTo" runat="server" Text='<%$ Resources:Resource,L_TO%>' class ="FormLabel" style="margin:5px"></asp:Label>
               </td>
               <td >
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
                     <asp:DropDownList ID="ddlHFCode" runat="server" autopostback="true">
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
                 <asp:TextBox ID="txtICDCode" runat="server" MaxLength="8"  class="cmb txtICDCode" autocomplete="off"></asp:TextBox>
                 </td>            
                  
              
                <td class="FormLabel">
                
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
                    <asp:DropDownList ID="ddlClaimStatus" runat="server" >
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
                
                  <%-- <asp:Button class="button" ID="btnSearch" runat="server" 
                          Text='<%$ Resources:Resource,B_SEARCH %>' >
                    </asp:Button>--%>
                
                </td>  
             </tr>         
            <tr>
                <td class="FormLabel">
                    <asp:Label ID="lblCHFID0" runat="server" Text="<%$ Resources:Resource,L_CHFID%>"></asp:Label>
                </td>
                <td class="DataEntry">
                  <%--<asp:DropDownList ID="ddlClaimCode" runat="server" ></asp:DropDownList>--%>
                    <asp:TextBox ID="txtCHFID" runat="server" maxlength="12"></asp:TextBox>
                </td>
                <td class="FormLabel">
                    <asp:Label ID="Label2" runat="server" Text="<%$ Resources:Resource,L_CLAIMCODE%>"></asp:Label>
                </td>
                <td class="DataEntry">
                    <asp:TextBox ID="txtClaimCode" runat="server" MaxLength="8"></asp:TextBox>
                </td>
                <td class="FormLabel">
                    <asp:Label ID="lblVisitType0" runat="server" Text="<%$ Resources:Resource,L_VISITTYPE %>"></asp:Label>
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
        
        <table class="catlabel">
            <tr>
                <td >
                   <asp:label  
                           ID="L_CLAIMSELECTIONUPDATE"  
                           runat="server" 
                           Text='<%$ Resources:Resource,L_CLAIMSELECTIONUPDATE %>'> </asp:label>                     
                </td>
                
            </tr>
        </table>
        <asp:Panel ID="pnlMiddle" runat="server"  CssClass="panel" height="50px" GroupingText='<%$ Resources:Resource,L_CRITERIADETAILS %>'>
           <table align="center">
            <tr align="center">
                <td>
                   <table align="center">
         
            <tr align="center">
            <td class="DataEntry">
                     <asp:DropDownList ID="ddlSelectionType" runat="server" 
                         style="text-align:center" AutoPostBack="True">
                     </asp:DropDownList>
                 </td>
               <td class="FormLabel">
                    <asp:Label ID="lblRandom" runat="server" 
                        Text='<%$ Resources:Resource,L_RANDOM%>'></asp:Label>
                </td>
                <td class ="DataEntry">
                    <asp:CheckBox ID="chkRandom" runat="server" />
                </td>
                <td class ="DataEntry" style="width:150px">
                    <asp:TextBox ID="txtRandom" runat="server" size="3" MaxLength="3"  style="text-align:right" Enabled="false" CssClass="numbersOnly" Width ="100px"></asp:TextBox>
                    <asp:Label ID="lblRandomPercentageMark" runat="server" 
                        Text='%'></asp:Label>
                </td>
                <td class="FormLabel">
                     <asp:Label ID="lblValue" runat="server" Text='<%$ Resources:Resource,L_VALUE%>'></asp:Label>
                </td>
                <td class ="DataEntry">
                    <asp:CheckBox ID="chkValue" runat="server"  />
                </td>
                <td class ="DataEntry">
                    <asp:TextBox ID="txtValue" runat="server" MaxLength="20" size="10" style="text-align:right" 
                         CssClass="numbersOnly" ></asp:TextBox>
                    
                </td>
                 <td class ="FormLabel">
                     <asp:Label ID="lblVariance" runat="server" 
                        Text='<%$ Resources:Resource,L_VARIANCE%>'></asp:Label>
                 </td>
                 <td class="DataEntry">
                     <asp:CheckBox ID="chkVariance" runat="server" />
                 </td>
                 <td class ="DataEntry" style="width:150px">
                    <asp:TextBox ID="txtVariance" runat="server" size="3" MaxLength="3" style="text-align:right" Enabled="false" CssClass="numbersOnly" Width="100px"></asp:TextBox>
                    <asp:Label ID="lblVariancePercentageMark" runat="server" 
                        Text='%'></asp:Label>
                </td>
                
                 <td align="right">
                     <asp:Button ID="btnSelectionExecute" runat="server" Text='<%$ Resources:Resource,B_UPDATE%>' />
                 </td>
            </tr>
            
        </table>
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
                           ID="L_CLAIMSSELECTED"  
                           runat="server" 
                           Text='<%$ Resources:Resource,L_CLAIMSSELECTED %>'></asp:label>   
               </td>
              
             
                </tr>
            
            </table>
        </td>
        
        
        <td align="right" >
        <asp:Label ID="lblSelectToProcess" runat="server" Text='<%$ Resources:Resource,L_SELECTTOPROCESS %>' style="margin-left:573px" CssClass="FormLabel"></asp:Label>
       <asp:CheckBox ID="chkboxSelectToProcess" runat="server" onClick="toggleCheck(this);"/>
        </td>
        </tr>
        </table>
        
        <asp:Panel ID="pnlBody" runat="server"  CssClass="panelBody" Height="200px"  ScrollBars ="Vertical">
            <asp:GridView ID="gvClaims" runat="server"
                AutoGenerateColumns="False"
                GridLines="None"
                AllowPaging="false"
                CssClass="mGrid"
                EmptyDataText='<%$ Resources:Resource,M_NOCLAIMS %>'
                PagerStyle-CssClass="pgr"
                
                
                AlternatingRowStyle-CssClass="alt"
                SelectedRowStyle-CssClass="srs" PageSize="8" DataKeyNames="ClaimID,ReviewStatus,FeedbackStatus,RowID" >
                <Columns>

                    <%--<asp:HyperLinkField DataNavigateUrlFields = "ClaimID" DataTextField="ClaimCode" DataNavigateUrlFormatString = "ClaimReview.aspx?c={0}" HeaderText='<%$ Resources:Resource,L_CLAIMCODE %>' HeaderStyle-Width ="30px" > 
                    <HeaderStyle Width="30px" />
                    </asp:HyperLinkField>--%>
                    <asp:BoundField DataField="ClaimCode"   HeaderText='<%$ Resources:Resource,L_CLAIMCODE %>' SortExpression="ClaimCode" HeaderStyle-Width="30px">  
                       <HeaderStyle Width="30px" />
                    </asp:BoundField>
                     <asp:BoundField DataField="HFName"   HeaderText='<%$ Resources:Resource,L_HFNAME %>' SortExpression="HFName" HeaderStyle-Width="70px">  
                       <HeaderStyle Width="95px" />
                    </asp:BoundField>                   
                    
                   <asp:BoundField DataField="DateClaimed"  DataFormatString="{0:d}" HeaderText='<%$ Resources:Resource,L_DATECLAIMED %>' SortExpression="DateClaimed" >  
                       <HeaderStyle Width="30px" />
                    </asp:BoundField>
                   <%-- <asp:BoundField DataField="FeedbackStatus"  HeaderText='<%$ Resources:Resource,L_CLAIMFEEDBACK %>' SortExpression="Feedback" HeaderStyle-Width="70px">  
                        <HeaderStyle Width="70px" />
                    </asp:BoundField>--%>  
                        <asp:TemplateField  >
                                    <ItemTemplate >
                                        <asp:DropDownList ID="ddlClaimFeedback" runat="server" CssClass="cmb ddlFeedBck"  ></asp:DropDownList>
                                    </ItemTemplate>
                                    <HeaderTemplate >
                                     <asp:Label ID="lblClaimFeedbackStatus" runat="server" Text='<%$ Resources:Resource, L_CLAIMFEEDBACK %>' ></asp:Label> 
                                    </HeaderTemplate>
                                    <ItemStyle Width="105px" />
                     </asp:TemplateField>           
                   <asp:TemplateField  >
                                    <ItemTemplate >
                                        <asp:DropDownList ID="ddlClaimReviewStatus" runat="server" CssClass="cmb ddlReview"  ></asp:DropDownList>
                                    </ItemTemplate>
                                    <HeaderTemplate >
                                     <asp:Label ID="lblReviewStatus" runat="server" Text='<%$ Resources:Resource, L_CLAIMREVIEW %>' ></asp:Label> 
                                    </HeaderTemplate>
                                    <ItemStyle Width="95px" />
                     </asp:TemplateField> 
                  
                    <asp:BoundField DataField="Claimed"  DataFormatString="{0:n2}" HeaderText='<%$ Resources:Resource,L_CLAIMED %>' SortExpression="Claimed"  ItemStyle-HorizontalAlign="Right">  
                         <HeaderStyle Width="20px" />
                        <ItemStyle  Width="20px"/>
                    </asp:BoundField>
                    <asp:BoundField DataField="Approved"  DataFormatString="{0:n2}" HeaderText='<%$ Resources:Resource,L_APPROVED %>' SortExpression="Approved" HeaderStyle-Width="70px" ItemStyle-HorizontalAlign="Right">  
                        <HeaderStyle Width="30px" />
                        <ItemStyle HorizontalAlign="Right" Width="30px"/>
                    </asp:BoundField>
                     <asp:BoundField DataField="ClaimStatus"  HeaderText='<%$ Resources:Resource,L_CLAIMSTATUS %>' SortExpression="ClaimStatus" > 
                         <HeaderStyle Width="55px" />
                    </asp:BoundField>
                    <asp:TemplateField  >
                                    <ItemTemplate >
                                    <asp:CheckBox ID="chkbgridSelectToProcess" runat="server" CssClass="ConditionCheck" /> 
                                       
                                    </ItemTemplate>
                                
                                    <ItemStyle Width="15px"  />
                     </asp:TemplateField> 
                <asp:BoundField DataField="ClaimID" > <ItemStyle CssClass="hidecol" /><HeaderStyle CssClass="hidecol"  /></asp:BoundField >
                <asp:BoundField DataField="RowID" > <ItemStyle CssClass="hidecol" /><HeaderStyle CssClass="hidecol"  /></asp:BoundField >
                </Columns>
                <PagerStyle CssClass="pgr" />
                <SelectedRowStyle CssClass="srs" />
                <AlternatingRowStyle CssClass="alt" />
                <RowStyle CssClass="normal" />
            </asp:GridView>
            <asp:HiddenField ID="hfDistrictID" runat="server" /> 
            <asp:HiddenField ID="hfClaimID" runat="server" /> 
            <asp:HiddenField ID="hfReview" runat="server" /> 
            <asp:HiddenField ID="hfReviewddlValue" runat="server" /> 
            <asp:HiddenField ID="hfFeedBackddlValue" runat="server" /> 
            <asp:HiddenField ID="hfSelectionExecute" runat="server" />
            <asp:HiddenField ID="hfProcessClaims" runat="server" />
        </asp:Panel>
        </div>
       <asp:Panel ID="pnlButtons" runat="server"   CssClass="panelbuttons" >
        <table width="100%" cellpadding="10 10 10 10" align="center">
             <tr align="center">
                    
                    <td  align="left">
                       <asp:Button 
                        ID="B_REVIEW" 
                        runat="server" 
                        Text='<%$ Resources:Resource,B_REVIEW%>'
                          />
                    </td>
                    <td  align="left">
                       <asp:Button 
                        ID="B_FEEDBACK" 
                        runat="server" 
                        Text='<%$ Resources:Resource,B_FEEDBACK%>'
                          />
                    </td>
                    <td align="center">
                       <asp:Button 
                        ID="btnUpdateClaims" 
                        runat="server" 
                        Text='<%$ Resources:Resource,B_UPDATE%>'
                         ValidationGroup="check"  />
                  
                    </td>
                    <td>
                    <asp:Button ID="B_ProcessClaimStatus" runat="server" style="color:Red; font-weight:bold" 
                         Text='<%$ Resources:Resource,B_PROCESS%>' />
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
   </ContentTemplate> </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Footer" Runat="Server">
    <asp:UpdatePanel ID="uplblMsg" runat="server"><ContentTemplate>
    <asp:Label text="" runat="server" ID="lblMsg"> </asp:Label>
    </ContentTemplate></asp:UpdatePanel>
</asp:Content>
