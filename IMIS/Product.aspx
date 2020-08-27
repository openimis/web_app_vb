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
<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/IMIS.Master" CodeBehind="Product.aspx.vb" Inherits="IMIS.Product" 
 Title = '<%$ Resources:Resource,P_PRODUCT %>'%>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>


<asp:Content ID="StyleContent" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">

     function toggleCheckMI(status) {
         $('#<%=gvMedicalItems.clientId %> input[type=checkbox]').each(function() {
             $(this).attr("Checked", status);
         });
     }

        function pageLoadExtend() {
            $('#<%=gvMedicalItems.clientId %> input[type=checkbox]').each(function () {
                if (this.checked != true) {
                    $('#<%= Checkbox2.ClientID%>').attr("checked", false);
                    return false;
                } else {
                    $('#<%= checkbox2.ClientId %>').attr("checked", true);
                }
            });

            $('#<%=gvMedicalItems.clientId %> input[type=checkbox]').unbind("click").click(function () {
                $('#<%=gvMedicalItems.clientId %> input[type=checkbox]').each(function () {
                    if (this.checked != true) {
                        $('#<%= Checkbox2.ClientID%>').attr("checked", false);
                        return false;
                    } else {
                        $('#<%= Checkbox2.ClientID%>').attr("checked", true);
                    }
                });
            });

            $('#<%=gvMedicalServices.clientId %> input[type=checkbox]').each(function () {
                if (this.checked != true) {
                    $('#<%= Checkbox1.ClientID%>').attr("checked", false);
                 return false;
             } else {
                 $('#<%= Checkbox1.ClientID%>').attr("checked", true);
             }
            });
             $('#<%=gvMedicalServices.clientId %> input[type=checkbox]').unbind("click").click(function () {
                    $('#<%=gvMedicalServices.clientId %> input[type=checkbox]').each(function () {
                     if (this.checked != true) {
                         $('#<%= Checkbox1.ClientID%>').attr("checked", false);
                         return false;
                     } else {
                         $('#<%= checkbox1.ClientId %>').attr("checked", true);
                     }
                 });
             });

            $(".alphaOnlyL").off("keydown").on("keydown",function (event) {
                var e = String.fromCharCode(event.which);
                if (!(e.match('[Ff|Cc]') || event.which == 8 || event.which == 9 || event.which == 37 || event.which == 46)) {
                    return false;
                } else {
                    $(this).data("changed", true);
                    //if (event.which != 8)
                    //    $(this).val("").val(e);
                }
            });

            $(".alphaOnlyC").off("keydown").on("keydown", function (event) {
                var e = String.fromCharCode(event.which);
                if (!(e.match('[Hh|Nn|Bb]') || event.which == 8 || event.which == 9 || event.which == 37 || event.which == 46)) {
                    return false;
                } else {
                    
                    $(this).data("changed", true);
                    //if (event.which != 8)
                    //    $(this).val("").val(e);
                }
            })


            $(".alphaOnlyP").off("keydown").on("keydown", function (event) {
                var e = String.fromCharCode(event.which);
                if (!(e.match('[Pp|Oo|Rr]') || event.which == 8 || event.which == 9 || event.which == 37 || event.which == 46)) {
                    return false;
                } else {
                    //if (event.which != 8)
                    //    $(this).val("").val(e);
                }
            });

            $(".LimitationType").off("blur").on("blur",function () {
                if (typeof ($(this).data("changed")) != "undefined")
                    if ($(this).data("changed"))
                        adultChildLimitValidating($(this));

                $(this).data("changed", false);
            });
            /* grid row selection */
            $("#<%=gvMedicalItems.ClientID %> tr,#<%=gvMedicalServices.ClientID %> tr").removeClass("alt");

            $("#<%=gvMedicalItems.ClientID %> tr,#<%=gvMedicalServices.ClientID %> tr").unbind("click").click(function () {
                var $row = $(this);
                var $selected = null;
                if ($row.parents("#<%=gvMedicalItems.ClientID %>").length > 0) {
                    $("#<%=gvMedicalItems.ClientID %>").find("tr.srs").removeClass("srs");
                } else if ($row.parents("#<%=gvMedicalServices.ClientID %>").length > 0) {
                    $("#<%=gvMedicalServices.ClientID %>").find("tr.srs").removeClass("srs");
                }
                $row.addClass("srs");
            });
            /* end grid row selection */

            //$(".loadGrid").click(function () { $(this).hide(); });
        }
     //});
     /********************** MIN,MAX LIMIT CHILD/ADULT FILL:  validating min and max filling of limit child and limit adult values **/
     var defaultC = {
         min: 0,
         max: 100
     }
     var defaultF = {
         min: 0,
         max: 100000
     }

     function adultChildLimitValidating($limitationTypeSource) {

         var limitationType = $limitationTypeSource.val();
         var $limitAdultTxtbox = $limitationTypeSource.parent().next().next().find("input.limitAdult");
         var $limitChildTxtbox = $limitationTypeSource.parent().next().next().next().find("input.limitChild");

         if (limitationType.toUpperCase() == "C") {
             $limitAdultTxtbox.val(defaultC.max);
             $limitChildTxtbox.val(defaultC.max);
         } else if (limitationType.toUpperCase() == "F") {
             $limitAdultTxtbox.val(defaultF.min);
             $limitChildTxtbox.val(defaultF.min);
         }
     }
     /*------------ END of MIN,MAX LIMIT CHILD/ADULT FILL **/
      


     function toggleCheckMS(status) {
         $('#<%=gvMedicalServices.clientId %> input[type=checkbox]').each(function() {
             $(this).attr("Checked", status);
         });
     }

     $(document).ready(function() {
         $('#<%=B_SAVE.clientId %>').click(function() {
             /*var $ddlC1d = $('#<=ddlCycle1Day.clientId %>');
             var $ddlC1M = $('#<=ddlCycle1Month.clientId %>');
             var $ddlC2d = $('#<=ddlCycle2Day.clientId %>');
             var $ddlC2M = $('#<=ddlCycle2Month.clientId %>');
             */
             //if (($ddlC1d[0].selectedIndex < 1 || $ddlC1M[0].selectedIndex < 1) && ($ddlC2d[0].selectedIndex < 1 || $ddlC2M[0].selectedIndex < 1)) {
             //popup.alert('<=imisgen.getMessage("M_PROVIDECYCLEPROMPT", True )%>');
                 //return false;
             //}

             /*if ($ddlC1d[0].selectedIndex > 0 && $ddlC1M[0].selectedIndex > 0) {
                 var jsDate = $ddlC1M.val() + "/" + $ddlC1d.val() + "/" + "2013";
                 if ((new Date(jsDate)).getMonth() != (parseInt($ddlC1M.val()) - 1)) {
                     popup.alert('<= imisgen.getMessage("M_INVALIDCYCLE1DATE", True ) %>');
                     $ddlC1d.focus();
                     return false;
                 }
         }*/
            /*
             if ($ddlC2d[0].selectedIndex > 0 && $ddlC2M[0].selectedIndex > 0) {
                 var jsDate = $ddlC2M.val() + "/" + $ddlC2d.val() + "/" + "2013";
                 //alert((new Date(jsDate)).getMonth() + "---" + (parseInt($ddlC2M.val())));
                 if ((new Date(jsDate)).getMonth() != ($ddlC2M.val() - 1)) {
                     popup.alert('<= imisgen.getMessage("M_INVALIDCYCLE2DATE", True ) %>');
                     $ddlC2d.focus();
                     return false;
                 }
             }*/
             return isValidData();
         });
     });
    function isValidData() {
        var msg = "";
        var thold = 0;
        var maxMembers = 0;
        var renewDiscPerc = 0;
        var maxPolicy = 0;
        var maxIPPolicy = 0;
        var maxOPPolicy = 0;
        var maxMC = 0;
        var maxIPMC = 0;
        var maxOPMC = 0;
        var shareContribution = 0;
        var enrolDiscPerc = 0;
        if( $.trim($("#<%=txtThresholdMembers.ClientID%>").val()) !== "" && ! isNaN($("#<%=txtThresholdMembers.ClientID%>").val())) 
            thold = parseInt(format.numberWithoutCommas( $("#<%=txtThresholdMembers.ClientID%>").val()));
        if ($.trim($("#<%=txtMaxNoOfMembers.ClientID%>").val()) !== "" && ! isNaN($("#<%=txtMaxNoOfMembers.ClientID%>").val())) 
            maxMembers = parseInt(format.numberWithoutCommas($("#<%=txtMaxNoOfMembers.ClientID%>").val()));
        if ($.trim($("#<%=txtRenewalDiscountPercentage.ClientID%>").val()) !== "" && ! isNaN($("#<%=txtRenewalDiscountPercentage.ClientID%>").val())) 
            renewDiscPerc = parseInt(format.numberWithoutCommas($("#<%=txtRenewalDiscountPercentage.ClientID%>").val()));

        if ($.trim($("#<%=txtMaxPolicy.ClientID%>").val()) !== "" && !isNaN($("#<%=txtMaxPolicy.ClientID%>").val()))
            maxPolicy = parseFloat(format.numberWithoutCommas($("#<%=txtMaxPolicy.ClientID%>").val()));
        if ($.trim($("#<%=txtMaxIPPOlicy.ClientID%>").val()) !== "" && !isNaN($("#<%=txtMaxIPPOlicy.ClientID%>").val()))
            maxIPPolicy = parseFloat(format.numberWithoutCommas($("#<%=txtMaxIPPOlicy.ClientID%>").val()));
        if ($.trim($("#<%=txtMaxOPPolicy.ClientID%>").val()) !== "" && !isNaN($("#<%=txtMaxOPPolicy.ClientID%>").val()))
            maxOPPolicy = parseFloat(format.numberWithoutCommas($("#<%=txtMaxOPPolicy.ClientID%>").val()));
        if ($.trim($("#<%=txtMaxPolicyMC.ClientID%>").val()) !== "" && !isNaN($("#<%=txtMaxPolicyMC.ClientID%>").val()))
            maxMC = parseFloat(format.numberWithoutCommas($("#<%=txtMaxPolicyMC.ClientID%>").val()));
        if ($.trim($("#<%=txtMaxIPPolicyMC.ClientID%>").val()) !== "" && !isNaN($("#<%=txtMaxIPPolicyMC.ClientID%>").val()))
            maxIPMC = parseFloat(format.numberWithoutCommas($("#<%=txtMaxIPPolicyMC.ClientID%>").val()));
        if ($.trim($("#<%=txtMaxOPPolicyMC.ClientID%>").val()) !== "" && !isNaN($("#<%=txtMaxOPPolicyMC.ClientID%>").val()))
            maxOPMC = parseFloat(format.numberWithoutCommas($("#<%=txtMaxOPPolicyMC.ClientID%>").val()));

        if ($.trim($("#<%=txtEnrolmentDiscountPerc.ClientID%>").val()) !== "" && !isNaN($("#<%=txtEnrolmentDiscountPerc.ClientID%>").val()))
            enrolDiscPerc = parseInt(format.numberWithoutCommas($("#<%=txtEnrolmentDiscountPerc.ClientID%>").val()));

        if ($.trim($("#<%=txtShareContribution.ClientID%>").val()) !== "" && !isNaN($("#<%=txtShareContribution.ClientID%>").val()))
            shareContribution = parseInt(format.numberWithoutCommas($("#<%=txtShareContribution.ClientID%>").val()));


        var $focused = null;
        if (thold > maxMembers.replace(/\,/g, "")) {
            msg = "<%= imisgen.getMessage("M_THRESHOLDMEMBERSEXCEEDMAXMEMBERS", True)%>";
            $focused = $("#<%=txtThresholdMembers.ClientID%>");
        } else if (renewDiscPerc > 100) {
            msg = "<%= imisgen.getMessage("M_RENEWDISCOUNTPERCEXCEEDPERC", True)%>";
            $focused = $("#<%=txtRenewalDiscountPercentage.ClientID%>");
        } else if (enrolDiscPerc > 100) {
            msg = "<%= imisgen.getMessage("M_ENROLDISCOUNTPERCEXCEEDPERC", True)%>";
            $focused = $("#<%=txtEnrolmentDiscountPerc.ClientID%>");
        } else if ($.trim($("#<%=txtMaxPolicyMC.ClientID%>").val()) !== "" && maxPolicy > maxMC) {
            msg = "<%= imisgen.getMessage("M_POLICYCEILINGEXCEEDMAXCEILING", True)%>";
            $focused = $("#<%=txtMaxPolicy.ClientID%>");
        } else if ($.trim($("#<%=txtMaxIPPolicyMC.ClientID%>").val()) !== "" && maxIPPolicy > maxIPMC) {
            msg = "<%= imisgen.getMessage("M_POLICYIPCEILINGEXCEEDMAXIPCEILING", True)%>";
            $focused = $("#<%=txtMaxIPPOlicy.ClientID%>");
        } else if ($.trim($("#<%=txtMaxOPPolicyMC.ClientID%>").val()) !== "" && maxOPPolicy > maxOPMC) {
            msg = "<%= imisgen.getMessage("M_POLICYOPCEILINGEXCEEDMAXOPCEILING", True )%>";
            $focused = $("#<%=txtMaxOPPolicy.ClientID%>");
        } else if (shareContribution > 100 || shareContribution == 0 ){
            msg = "<%= imisgen.getMessage("M_SHAREOFCONTRIBUTIONRANGE", True)%>";
            $focused = $("#<%=txtMaxOPPolicy.ClientID%>");
        }
        //alert(maxPolicy + " , " + maxMC + " , " + (maxPolicy > maxMC).toLocaleString() + " , " + ($.trim($("#<%=txtMaxPolicyMC.ClientID%>").val()) !== "" ).toString() );
        if( $.trim(msg) !== "" ){
            popup.alert(msg, function () {
                $focused.focus();
            });
            return false;
        }
        return true
    }
     function disableBody() {
         $shadebgDiv = $("<div id='SelectPic' style='display:block;'></div>");


         $shadebgDivContent = $("<div id='shade-bg-div-content'></div>");
         $shadebgDivContent.css({
             "position": "absolute",
             "width": "100%",
             "height": "100%",
             "top": "0px",
             "left": "0px",
             "z-index": 1002
         });

         $shadebgDiv.appendTo("body");
         $shadebgDivContent.appendTo("body");
     }


     var $source_ddl = null;
     var ddlDistrB_selected_val = "";
     var ddlDistrIP_selected_val = "";
     var ddlDistrOP_selected_val = "";
     var DistrCareType = ""
     var DistrType = ""
     var oldVal = "";
     $(document).ready(function() {
        
         ddlDistrB_selected_val = $("#<%=ddlDistribution.ClientID %>").val();
         ddlDistrIP_selected_val = $('#<%=ddlDistributionIP.ClientID %>').val();
         ddlDistrOP_selected_val = $("#<%=ddlDistributionOP.ClientID %>").val();

         $(".distributionGrid").click(function() {
             if ($(this).attr("id") == '<%=gvDistrB.ClientID %>')
                 showDistributionPopup($('#<%=ddlDistribution.ClientID %>'))
             else if ($(this).attr("id") == '<%=gvDistrI.ClientID %>')
                 showDistributionPopup($('#<%=ddlDistributionIP.ClientID %>'));
             else if ($(this).attr("id") == '<%=gvDistrO.ClientID %>')
                 showDistributionPopup($('#<%=ddlDistributionOP.ClientID %>'));
         });

         $('.ddlpopup').change(function() { showDistributionPopup($(this)); });

         function showDistributionPopup($ddl_associated) {
             $source_ddl = $ddl_associated;
             $popBox = $("<div id='distributionPopupDiv'></div>");
             $popControlsContainer = $("<div></div>");
             $popOkControl = $("<div style='float:left'></div>");
             $popCancelControl = $("<div></div>");


             $popOkControl.html("<input id='OK' type='submit' value='OK' style='width:40px' />");
             $popCancelControl.html("<input type='submit' value='CANCEL' style='width:80px' />");

             var html = getDistributionPopupElelements($source_ddl.val(), $source_ddl.attr("id"))
             if (html == "none") {

                 switch ($source_ddl.attr("id")) {
                     case '<%=ddlDistribution.ClientID %>':
                         ddlDistrB_selected_val = $source_ddl.val();
                         EmptyGridView('<%=gvDistrB.ClientID %>');
                         break;
                     case '<%=ddlDistributionIP.ClientID %>':
                         ddlDistrIP_selected_val = $source_ddl.val();
                         EmptyGridView('<%=gvDistrI.ClientID %>');
                         break;
                     case '<%=ddlDistributionOP.ClientID %>':
                         ddlDistrOP_selected_val = $source_ddl.val();
                         EmptyGridView('<%=gvDistrO.ClientID %>');
                         break;
                 }
                 return;
             }

             $popBox.html(html)
             $popBox.append("<span id='msg' style='color:#00F'></span>");
             $popControlsContainer.appendTo($popBox)
             $popOkControl.appendTo($popControlsContainer)
             $popCancelControl.appendTo($popControlsContainer)

             $popOkControl.click(function() {
                 switch ($source_ddl.attr("id")) {
                     case '<%=ddlDistribution.ClientID %>':
                         ddlDistrB_selected_val = $source_ddl.val();
                         fillBackDistributionPopupData('<%=gvDistrB.ClientID %>', "<%= hfGvDistrBState.ClientID %>");
                         break;
                     case '<%=ddlDistributionIP.ClientID %>':
                         ddlDistrIP_selected_val = $source_ddl.val();
                         fillBackDistributionPopupData('<%=gvDistrI.ClientID %>', "<%= hfGvDistrIPState.ClientID %>");
                         break;
                     case '<%=ddlDistributionOP.ClientID %>':
                         ddlDistrOP_selected_val = $source_ddl.val();
                         fillBackDistributionPopupData('<%=gvDistrO.ClientID %>', "<%= hfGvDistrOPState.ClientID %>");
                         break;
                 }

             });


             $popCancelControl.bind("click", function() { CloseDistributionPopup("cancel"); });
             $popBox.find(".DistrPercInput").keydown(function(e) {
                 var c = e.which;
                 if (isNaN(c)) return false;
                 c = parseInt(c);
                 if (!((c >= 48 && c <= 57) || (c >= 37 && c <= 40) || c == 8 || c == 13 || c == 46 || c == 9 || c == 190)) {
                     return false;
                 } else {
                     if (c == 190) {
                         if ($(this).val().indexOf(".") > -1) return false;
                     }
                 }
             });

             $popBox.find(".DistrPercInput").keyup(function(e) {
                 var c = e.which
                 if (parseInt($(this).val()) > 200 || parseInt($(this).val()) < 0) {
                     $(this).val(oldVal);
                     return false
                 }
                 oldVal = $(this).val();

                 //format = /(^[0-9]{1,2}.[0-9]{18}$) || (^[0-9]{1,2}$) || (^[1][0]{2}$)/;
                 // alert(format.test($(this).val()));
             });

             $popBox.css({
                 "width": "130px",
                 "height": "auto",
                 "padding": "3px",
                 "position": "absolute",
                 "top": $(window).height() / 2 - 25 + "px",
                 "left": $(window).width() / 2 - 60 + "px",
                 "background": "#70a5c8",
                 "text-align": "center",
                 "opacity": 1
             });
             disableBody();
             $popBox.appendTo($("#shade-bg-div-content"));
         }



     });

     function EmptyGridView(gv) {
         $('#' + gv + ' tr').each(function(i) {
             if (i > 0) {

                 $(this).children().first().html("");
                 $(this).children().last().html("");
             }
         });
     }

     function getDistributionPopupElelements(x, appendedNameSource) {
         var end = 0;

         switch (appendedNameSource) {
             case '<%=ddlDistribution.ClientID %>':
                 DistrCareType = "B";
                 break;
             case '<%=ddlDistributionIP.ClientID %>':
                 DistrCareType = "I";
                 break;
             case '<%=ddlDistributionOP.ClientID %>':
                 DistrCareType = "O";
                 break;
         }

         switch (x) {
             case 'M':
                 end = 11;
                 break;
             case 'Q':
                 end = 3;
                 break;
             case 'Y':
                 end = 0;
                 break;
             case '0':
                 return "none";
         }

         DistrType = end + 1;

         var DistrDataObj = fetchDataFromHiddenDistributionTable();

         var html = "";
         html += "<table cellspacing='5' border='0px' ID='detailsTable'>";
         html += "<tr><th>Period</th><th>Percent</th></tr>";
         for (i = 0; i <= end; i++) {
             html += "<tr>";

             DistrDataObj.perc[i] = isNaN(DistrDataObj.perc[i]) ? "" : DistrDataObj.perc[i]; //alert(  DistrDataObj.perc[i] );
             html += "<td>" + (i + 1) + "</td><td><input class='DistrPercInput' type='text' value='" + DistrDataObj.perc[i] + "' style='border-width:1px;width:40px' maxlength='21' /></td>";

             html += "</tr>";
         }

         html += "</table>";
         return html
     }

     function fetchDataFromHiddenDistributionTable() {
         var DistrData = {
             perc: new Array()
         }
         var $td;
         $("#<%=gvHiddenDistribution.ClientID %> tr").each(function(i) {
             if (i > 0) {//jump first row TH

                 $td = new Array();
                 $(this).children().each(function(j) { $td.push($(this)); });
                 //alert(DistrType + " : " + $td[1].html() + " : " + DistrCareType + " : " + $td[2].html());
                 if ($td[1].html() == DistrType && $td[2].html() == DistrCareType) {
                     percVal = $td[4].find("input").val();
                     if (isNaN(percVal) || percVal.length == 0) {
                         percVal = 0
                     }
                     percVal = percVal * 100
                     percVal = percVal.toFixed(2)
                     DistrData.perc.push(percVal); // take percentage value
                     //alert(DistrType + " : " + $td[1].html() + " : " + DistrCareType + " : " + $td[2].html() +"Perc: "+$td[4].find("input").val() );
                 }
             }
         });

         return DistrData;
     }

     var periodValues;
     var percentValues;
     function fillBackDistributionPopupData(gv, register) {
         periodValues = new Array();
         percentValues = new Array();

         $("#detailsTable tr").each(function(i) {
             if (i > 0) {
                 if ($(this).children().last().find("input").val().length > 0)
                     percentValues[i] = $(this).children().last().find("input").val();
                 else
                     percentValues[i] = 0;

                 periodValues[i] = $(this).children().first().html();
             }
         });

       
         for (j = 1; j < percentValues.length; j++) {
             if (!isNaN(percentValues[j])) {
                 if (parseFloat(percentValues[j]) > 200) {
                     message = "Percent Entry is greater than 200";
                     $("#distributionPopupDiv #msg").html(message);
                     setTimeout(function() { $('#distributionPopupDiv #msg').html(''); }, 4000);
                     return;
                 }
             }
         }

//         if (percentTotal > 100) {
//             message = "Total Percent is greater than 100";
//             $("#distributionPopupDiv #msg").html(message);
//             setTimeout(function() { $('#distributionPopupDiv #msg').html(''); }, 4000);
//             return;
//         }


         var rows = $('#' + gv + ' tr').length;

         if (rows < periodValues.length) {

             for (k = 0; k < (periodValues.length - rows); k++) {
                 $newrow = $("<tr></tr>");
                 $newrow.html("<td></td><td align='right'></td>");
                 $('#' + gv).append($newrow);
             }
         } else if (rows > periodValues.length) {
             for (k = (rows - periodValues.length); k > 0; k--) {
                 $('#' + gv).children().last().children().last().remove();
             }
         } else { }


         $('#' + gv + ' tr').each(function(i) {
             if (i > 0) {

                 $(this).children().first().html(periodValues[i]);
                 $(this).children().last().html(isNaN(percentValues[i]) ? "0.00" : percentValues[i] + " %");
             }
         });

         copyChangesToHiddenDistributionTable(percentValues);

         RegisterChanges(register);
         CloseDistributionPopup("ok");
     }

     function copyChangesToHiddenDistributionTable(percentValues) {

         var $td;
         $("#<%=gvHiddenDistribution.ClientID %> tr").each(function(i) {
             if (i > 0) {//jump first row TH
                 $td = new Array();
                 $(this).children().each(function(j) { $td.push($(this)); });
                 if ($td[1].html() == DistrType && $td[2].html() == DistrCareType)
                     $td[4].find("input").val(percentValues[parseInt($td[3].html())] / 100); // put new percentage value

             }
         });
     }

     function RegisterChanges(registerID) {
         $("#" + registerID).val($source_ddl.val());
     }

     function CloseDistributionPopup(status) {
         $("#SelectPic").remove();
         $("#SelectPic").remove();
         $("#shade-bg-div-content").remove();

         if (status == "cancel") {
             $("#<%=ddlDistribution.ClientID %>").val(ddlDistrB_selected_val);
             $('#<%=ddlDistributionIP.ClientID %>').val(ddlDistrIP_selected_val);
             $("#<%=ddlDistributionOP.ClientID %>").val(ddlDistrOP_selected_val);
         } else {
             ddlDistrB_selected_val = $("#<%=ddlDistribution.ClientID %>").val();
             ddlDistrIP_selected_val = $('#<%=ddlDistributionIP.ClientID %>').val();
             ddlDistrOP_selected_val = $("#<%=ddlDistributionOP.ClientID %>").val();
         }

         DistrType = "";
         DistrCareType = "";
     }
       
</script>

<style type="text/css">
  .numbersOnly,.decimalNumber{padding-right:4px;}
 
  #distribution-table input{padding-right:4px;width:20px;}
  /* #distribution-table span{width:4px;position:absolute;}*/
    .loadGrid
    {
        font-size: 16px;
        font-weight: bold;
        color: #2965f5 !important;
        position: absolute;
        margin: auto;
        left: 0px;
        right: 0px;
        top: 0px;
        bottom: 0px;
        height: 30px;
        overflow: visible;
        cursor: pointer;
        display: block;
        text-align: center;
    }
    input.loadGrid{
        background:none !important;
        border-width:0px !important;
        width:auto !important;
        box-shadow:none !important;
        text-shadow:none !important;
    }
    input.loadGrid:hover
    {
        text-decoration: underline !important;
    }
    .FormLabel, .DataEntry
    {
        width: auto;
    }
    
    .progress
    {
        background-image: initial;
        background-attachment: initial;
        height: 90%;
        width: 98%;
        background-color: rgba(0, 0, 0, 0.699219);
        border-radius:10px;
        display: none;
    }
</style>

</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="Body" runat="Server">

    <asp:HiddenField ID="hfcaller" runat="server" />
    <div class="divBody" style="overflow: auto; overflow-x: hidden;">
        <asp:Panel ID="pnlProduct" runat="server"
            CssClass="panel" GroupingText='<%$ Resources:Resource,L_PRODUCTDETAILS %>' Style="height: auto">
            <table style="position: relative; left: -14px;">
                <tr>
                    <td>  
                        <table>
                            <tr>
                                <td class="FormLabel">
                                    <asp:Label ID="L_PRODUCTCODE" runat="server" Width="140px" Text='<%$ Resources:Resource,L_PRODUCTCODE %>'></asp:Label>
                                </td>
                                <td class="DataEntry">
                                    <asp:TextBox ID="txtProductCode" runat="server" MaxLength="8"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:RequiredFieldValidator
                                        ID="RequiredFieldCode" runat="server"
                                        ControlToValidate="txtProductCode"
                                        SetFocusOnError="True"
                                        ValidationGroup="check" ForeColor="Red" Display="Dynamic"
                                        Text='*'></asp:RequiredFieldValidator>
                                </td>

                            </tr>
                            <tr>
                                <td class="FormLabel">
                                    <asp:Label ID="L_PRODUCTNAME" runat="server" Width="140px" Text='<%$ Resources:Resource,L_PRODUCTNAME %>'></asp:Label>
                                </td>
                                <td class="DataEntry">
                                    <asp:TextBox ID="txtProductName" runat="server"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:RequiredFieldValidator ID="RequiredFieldName" runat="server"
                                        ControlToValidate="txtProductName"
                                        ValidationGroup="check" ForeColor="Red" Display="Dynamic"
                                        Text='*'></asp:RequiredFieldValidator>
                                </td>

                            </tr>
                            <tr>
                                <td class="FormLabel">
                                    <asp:Label ID="L_REGION" runat="server" Width="140px" Text='<%$ Resources:Resource,L_REGION %>'></asp:Label>
                                </td>
                                <%--    --%>
                                <td class="DataEntry">
                                    <asp:UpdatePanel ID="upRegion" runat="server">
                                        <ContentTemplate>
                                            <asp:DropDownList ID="ddlRegion" runat="server" AutoPostBack="true">
                                            </asp:DropDownList>

                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>

                                <td>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValRegion" runat="server" ControlToValidate="ddlRegion" InitialValue="0" ValidationGroup="check" ForeColor="Red" Display="Dynamic"
                                        Text='*'></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td class="FormLabel">
                                    <asp:Label ID="L_District" runat="server" Width="140px" Text="<%$ Resources:Resource,L_DISTRICT %>"></asp:Label>
                                </td>
                                <td class="DataEntry">
                                    <asp:UpdatePanel ID="upDistrict" runat="server">
                                        <ContentTemplate>
                                            <asp:DropDownList ID="ddlDistrict" runat="server" AutoPostBack="false">
                                            </asp:DropDownList>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <%-- --%>
                                <td>
                                </td>
                            </tr>
                            <tr>
                                <td class="FormLabel">
                                    <asp:Label
                                        ID="L_DateFrom"
                                        runat="server" Width="140px"
                                        Text='<%$ Resources:Resource,L_DATEFROM %>'></asp:Label>
                                </td>
                                <td class="DataEntry">
                                    <asp:TextBox ID="txtDateFrom" runat="server" Width="130px"></asp:TextBox>

                                    <asp:MaskedEditExtender ID="txtDateFrom_MaskedEditExtender" runat="server"
                                        CultureDateFormat="dd/MM/YYYY"
                                        TargetControlID="txtDateFrom" Mask="99/99/9999" MaskType="Date"
                                        UserDateFormat="DayMonthYear">
                                    </asp:MaskedEditExtender>

                                    <asp:Button ID="btnDateFrom" runat="server" Height="15px"
                                        Width="20px" />
                                    <asp:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtDateFrom" PopupButtonID="btnDateFrom" Format="dd/MM/yyyy"></asp:CalendarExtender>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtDateFrom" SetFocusOnError="True"
                                        ValidationGroup="check" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server"
                                        ControlToValidate="txtDateFrom" ErrorMessage="*" SetFocusOnError="True"
                                        ValidationExpression="^(0[1-9]|[12][0-9]|3[01])[/](0[1-9]|1[012])[/](19|20)\d\d$"
                                        ValidationGroup="check" ForeColor="Red" Display="Dynamic">
                                    </asp:RegularExpressionValidator>
                                </td>

                            </tr>
                            <tr>
                                <td class="FormLabel">
                                    <asp:Label ID="L_Date" runat="server" Width="140px" Text='<%$ Resources:Resource,L_DATETO %>'></asp:Label>
                                </td>
                                <td class="DataEntry">
                                    <asp:TextBox ID="txtDateTo" runat="server" Width="130px"></asp:TextBox>
                                    <asp:MaskedEditExtender ID="MaskedEditExtender1" runat="server"
                                        CultureDateFormat="dd/MM/YYYY"
                                        TargetControlID="txtDateTo" Mask="99/99/9999" MaskType="Date" UserDateFormat="DayMonthYear">
                                    </asp:MaskedEditExtender>
                                    <asp:Button ID="btnDateTo" runat="server" Height="15px"
                                        Width="20px" />

                                    <asp:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtDateTo" PopupButtonID="btnDateTo" Format="dd/MM/yyyy"></asp:CalendarExtender>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtDateTo" SetFocusOnError="True"
                                        ValidationGroup="check" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server"
                                        ControlToValidate="txtDateTo" ErrorMessage="*" SetFocusOnError="True"
                                        ValidationExpression="^(0[1-9]|[12][0-9]|3[01])[/](0[1-9]|1[012])[/](19|20)\d\d$"
                                        ValidationGroup="check" ForeColor="Red" Display="Dynamic">
                                    </asp:RegularExpressionValidator>

                                </td>

                             
                            </tr>
                            <tr>
                                <td class="FormLabel">
                                    <asp:Label ID="L_Conversion" runat="server" Width="140px" Text='<%$ Resources:Resource,L_CONVERSION %>'></asp:Label>
                                </td>
                                <td class="DataEntry">
                                    <asp:DropDownList ID="ddlConversion" runat="server" Width="132px">
                                        <asp:ListItem Value="0" Text="<%$ Resources:Resource,L_CONVERSIONCLICKTOLOAD %>" Selected="True" style="font-size: 12px;"></asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:Button ID="btnConversion" runat="server" Height="15px"
                                        Width="20px" />
                                </td> 
                            </tr> 
                            <%--</table>
                            <table>--%>
                            <tr> 
                                <td class="FormLabel">
                                    <asp:Label ID="L_LumpSum" runat="server" Width="140px" Text='<%$ Resources:Resource,L_LUMPSUM %>'></asp:Label>
                                </td>
                                <td class="DataEntry">
                                    <asp:TextBox ID="txtLumpSum" runat="server" Style="text-align: right" class="numbersOnly" ></asp:TextBox>

                                </td>
                                <td>
                                </td> 
                            </tr>
                            <tr>
                                <td class="FormLabel">
                                    <asp:Label ID="lblThresholdMembers" runat="server" Width="140px" Text='<%$ Resources:Resource,L_THRESHOLDMEMBERS %>'></asp:Label>
                                </td>
                                <td class="DataEntry">
                                    <asp:TextBox ID="txtThresholdMembers" runat="server" Style="text-align: right" class="numbersOnly"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:CompareValidator ID="CompareValidator27" runat="server" ControlToValidate="txtThresholdMembers" SetFocusOnError="true" Type="Integer" Operator="DataTypeCheck" ErrorMessage="*" ValidationGroup="check" ForeColor="Red" Display="Dynamic">
                                    </asp:CompareValidator>
                                </td>
                            </tr>
                            <tr>
                                <td class="FormLabel">
                                    <asp:Label ID="lblMaxNoOfMembers" runat="server" Width="140px" Text='<%$ Resources:Resource,L_MEMBERS %>'></asp:Label>
                                </td>
                                <td class="DataEntry">
                                    <asp:TextBox ID="txtMaxNoOfMembers" runat="server" Style="text-align: right" class="numbersOnly"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtMaxNoOfMembers" SetFocusOnError="True"
                                        ValidationGroup="check" ForeColor="Red" Display="Dynamic"
                                        Text='*'></asp:RequiredFieldValidator>
                                </td>

                            </tr>
                            <tr>
                                <td class="FormLabel">
                                    <asp:Label ID="L_PREMIUMADULT" runat="server" Width="140px"  Text='<%$ Resources:Resource,L_PREMIUMADULT %>'></asp:Label>
                                </td>
                                <td class="DataEntry">
                                    <asp:TextBox ID="txtAdultPremium" runat="server" Style="text-align: right" class="numbersOnly"></asp:TextBox>

                                </td>
                                <td>
                                    <asp:CompareValidator ControlToValidate="txtAdultPremium" ID="CompareValidator2" runat="server" SetFocusOnError="true" Type="Currency" Operator="DataTypeCheck" ErrorMessage="*" ValidationGroup="check" ForeColor="Red" Display="Dynamic">
                                    </asp:CompareValidator>
                                </td>


                                <%--  <td> <asp:RequiredFieldValidator ID="RequiredFieldValidatorAdultPremium" 
                                runat="server" ControlToValidate="txtAdultPremium" 
                                ErrorMessage="Please enter the Premium for Adult" ValidationGroup="check"></asp:RequiredFieldValidator> </td>--%>
                            </tr>
                            <tr>
                                <td class="FormLabel">
                                    <asp:Label ID="L_PREMIUMCHILD" runat="server" Width="140px" Text='<%$ Resources:Resource,L_PREMIUMCHILD %>'></asp:Label>
                                </td>
                                <td class="DataEntry">
                                    <asp:TextBox ID="txtChildPremium" runat="server" Style="text-align: right" class="numbersOnly"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:CompareValidator ID="CompareValidator3" runat="server" ControlToValidate="txtChildPremium" SetFocusOnError="true" Type="Currency" Operator="DataTypeCheck" ErrorMessage="*" ValidationGroup="check" ForeColor="Red" Display="Dynamic">
                                    </asp:CompareValidator>
                                </td>

                            </tr>
                            <tr>
                                <td class="FormLabel">
                                    <asp:Label ID="L_INSURANCEPERIOD" runat="server" Width="140px" Text='<%$ Resources:Resource,L_INSURANCEPERIOD %>'></asp:Label>
                                </td>
                                <td class="DataEntry">
                                    <asp:TextBox ID="txtInsurancrePeriod" runat="server" Style="text-align: right" class="numbersOnly"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" Text="*" runat="server" ControlToValidate="txtInsurancrePeriod" SetFocusOnError="True"
                                        ValidationGroup="check" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                                    <asp:CompareValidator ControlToValidate="txtInsurancrePeriod" ID="CompareValidator5" runat="server" SetFocusOnError="true" Type="Integer" Operator="DataTypeCheck" ErrorMessage="*" ValidationGroup="check" ForeColor="Red" Display="Dynamic">
                                    </asp:CompareValidator>
                                </td>
                            </tr>
                            <tr>
                                <td class="FormLabel">
                                    <asp:Label ID="lblAdministrationPeriod" runat="server" width="140" Text='<%$ Resources:Resource,L_ADMINISTRATIONPERIOD %>'></asp:Label>
                                </td>
                                <td class="DataEntry">
                                    <asp:TextBox ID="txtAdministrationPeriod" runat="server" Style="text-align: right" class="numbersOnly"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:CompareValidator ID="CompareValidator30" runat="server" ControlToValidate="txtAdministrationPeriod" SetFocusOnError="true" Type="Integer" Operator="DataTypeCheck" ErrorMessage="*" ValidationGroup="check" ForeColor="Red" Display="Dynamic">
                                    </asp:CompareValidator>
                                </td>
                            </tr>
                            <tr>
                                <td class="FormLabel">
                                    <asp:Label ID="lblMaxInstallments" runat="server" Text='<%$ Resources:Resource,L_MAXINSTALLMENTS %>'></asp:Label>
                                </td>
                                <td class="DataEntry">
                                    <asp:TextBox ID="txtMaxInstallments" runat="server" Style="text-align: right" class="numbersOnly"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator7" Text="*" runat="server" ControlToValidate="txtMaxInstallments" SetFocusOnError="True"
                                        ValidationGroup="check" ForeColor="Red" Display="Dynamic">
                                        </asp:RequiredFieldValidator>
                                    <%--<asp:CompareValidator ControlToValidate="txtMaxInstallments" ID="CompareValidator25"  runat="server" SetFocusOnError ="true"  Type="Integer"  Operator="DataTypeCheck" ErrorMessage="*" ValidationGroup ="check"> </asp:CompareValidator>--%>
                                </td>
                            </tr>
                            <tr>
                                <td class="FormLabel">
                                    <asp:Label ID="lblWaitingPeriod" runat="server"  Width="140px" Text='<%$ Resources:Resource,L_WAITINGPERIOD %>'></asp:Label>
                                </td>
                                <td class="DataEntry">
                                    <asp:TextBox ID="txtWaitingPeriod" runat="server" Style="text-align: right" class="numbersOnly"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator8" Text="*" runat="server" ControlToValidate="txtWaitingPeriod" SetFocusOnError="True"
                                        ValidationGroup="check" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                                    <asp:CompareValidator ControlToValidate="txtWaitingPeriod" ID="CompareValidator26" runat="server" SetFocusOnError="true" Type="Integer" Operator="DataTypeCheck" ErrorMessage="*" ValidationGroup="check" ForeColor="Red" Display="Dynamic">
                                    </asp:CompareValidator>
                                </td>
                            </tr>
                            <tr>
                                <td class="FormLabel">
                                    <asp:Label ID="L_GRACEPERIOD" runat="server" Width="140px" Text='<%$ Resources:Resource,L_GRACEPERIOD %>'></asp:Label>
                                </td>
                                <td class="DataEntry">
                                    <asp:TextBox ID="txtGracePeriod" runat="server" Style="text-align: right" class="numbersOnly"></asp:TextBox>

                                </td>
                                <td>
                                    <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator6" text="*" runat="server" ControlToValidate="txtGracePeriod" SetFocusOnError="True" 
                    ValidationGroup="check"  ></asp:RequiredFieldValidator>--%>
                                    <asp:CompareValidator ControlToValidate="txtGracePeriod" ID="CompareValidator4" runat="server" SetFocusOnError="true" Type="Integer" Operator="DataTypeCheck" ErrorMessage="*" ValidationGroup="check" ForeColor="Red" Display="Dynamic">
                                    </asp:CompareValidator>
                                </td>
                            </tr>
                            <tr>
                                <td class="FormLabel">
                                    <asp:Label ID="lblGracePeriodRenewal" runat="server" Width="140px"
                                        Text='<%$ Resources:Resource,L_GRACEPERIODRENEWAL %>'></asp:Label>
                                </td>
                                <td class="DataEntry">
                                    <asp:TextBox ID="txtGracePeriodRenewal" runat="server" Style="text-align: right"
                                        class="numbersOnly"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="FormLabel">
                                    <asp:Label ID="lblRenewalDiscountPercentage" runat="server" Text='<%$ Resources:Resource,L_RENEWALDISCOUNTPERCENTAGE %>'></asp:Label>
                                </td>
                                <td class="DataEntry">
                                    <asp:TextBox ID="txtRenewalDiscountPercentage" runat="server" Style="text-align: right" class="numbersOnly"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:CompareValidator ID="CompareValidator28" runat="server" ControlToValidate="txtRenewalDiscountPercentage" SetFocusOnError="true" Type="Integer" Operator="DataTypeCheck" ErrorMessage="*" ValidationGroup="check" ForeColor="Red" Display="Dynamic">
                                    </asp:CompareValidator>
                                </td>
                            </tr>
                            <tr>
                                <td class="FormLabel">
                                    <asp:Label ID="lblRenewalDiscountPeriod" runat="server" Width="140px" Text='<%$ Resources:Resource,L_RENEWALDISCOUNTPERIOD %>'></asp:Label>
                                </td>
                                <td class="DataEntry">
                                    <asp:TextBox ID="txtRenewalDiscountPeriod" runat="server" Style="text-align: right" class="numbersOnly"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:CompareValidator ID="CompareValidator29" runat="server" ControlToValidate="txtRenewalDiscountPeriod" SetFocusOnError="true" Type="Integer" Operator="DataTypeCheck" ErrorMessage="*" ValidationGroup="check" ForeColor="Red" Display="Dynamic">
                                    </asp:CompareValidator>
                                </td>
                            </tr>

                            <tr>
                                <td class="FormLabel">
                                    <asp:Label ID="lblEnrolmentDiscountPerc" runat="server" Text='<%$ Resources:Resource,L_ENROLMENTDISCOUNTPERC %>'></asp:Label>
                                </td>
                                <td class="DataEntry">
                                    <asp:TextBox ID="txtEnrolmentDiscountPerc" runat="server" Style="text-align: right" class="numbersOnly"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:CompareValidator ID="CompareValidator25" runat="server" ControlToValidate="txtEnrolmentDiscountPerc" SetFocusOnError="true" Type="Integer" Operator="DataTypeCheck" ErrorMessage="*" ValidationGroup="check" ForeColor="Red" Display="Dynamic"> </asp:CompareValidator>
                                </td>
                            </tr>
                            <tr>
                                <td class="FormLabel">
                                    <asp:Label ID="lblEnrolmentDiscountPeriod" runat="server" Text='<%$ Resources:Resource,L_ENROLMENTDISCOUNTPERIOD %>'></asp:Label>
                                </td>
                                <td class="DataEntry">
                                    <asp:TextBox ID="txtEnrolmentDiscountPeriod" runat="server" Style="text-align: right" class="numbersOnly"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:CompareValidator ID="CompareValidator31" runat="server" ControlToValidate="txtEnrolmentDiscountPeriod" SetFocusOnError="true" Type="Integer" Operator="DataTypeCheck" ErrorMessage="*" ValidationGroup="check" ForeColor="Red" Display="Dynamic"> </asp:CompareValidator>
                                </td>
                            </tr>
                        </table> 
                    </td>


                    <td valign="top">
                        <table>
                            <tr>
                                <td colspan="4" style="position: relative;">
                                    <asp:UpdatePanel ID="upMedicalServices" runat="server" style="position: relative;">
                                        <ContentTemplate>
                                            <asp:CheckBox ID="Checkbox1" runat="server" Text='<%$ Resources:Resource,L_CHECKALL %>' Style="margin-left: 14px;" onClick="toggleCheckMS(this.checked);" Visible="false" />
                                            <asp:Panel ID="pnlMedicalServices" runat="server" Height="140px" Width="600px"
                                                CssClass="panel" GroupingText='<%$ Resources:Resource,L_MEDICALSERVICES %>'
                                                ScrollBars="Auto">
                                                <asp:GridView ID="gvMedicalServices" runat="server" AllowPaging="false"
                                                    AutoGenerateColumns="False" CssClass="mGrid"
                                                    DataKeyNames="ProdServiceId,ServiceID,LimitationType,PriceOrigin,LimitAdult,LimitChild,WaitingPeriodAdult,WaitingPeriodChild,LimitNoAdult,LimitNoChild,LimitationTypeR,LimitationTypeE,LimitAdultR,LimitAdultE,LimitChildR,LimitChildE,CeilingExclusionAdult,CeilingExclusionChild"
                                                    EmptyDataText="<%$ Resources:Resource,M_NOMEDICALSERVICES %>" GridLines="None"
                                                    PagerStyle-CssClass="pgr" PageSize="12" ShowSelectButton="True" RowStyle-Wrap="False">
                                                    <RowStyle Wrap="False" />
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="Select">
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="chkSelect" runat="server" HeaderStyle-Width="30px" />
                                                            </ItemTemplate>
                                                            <HeaderTemplate>
                                                            </HeaderTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="ServCode" HeaderStyle-Width="80px" HeaderText="<%$ Resources:Resource,L_CODE %>"
                                                            SortExpression="ServCode">
                                                            <HeaderStyle Width="50px" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="ServName" HeaderStyle-Width="200px"
                                                            HeaderText="<%$ Resources:Resource,L_NAME %>" SortExpression="ServName">
                                                            <HeaderStyle Width="200px" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="ServType" HeaderStyle-Width="200px"
                                                            HeaderText="<%$ Resources:Resource,L_TYPE %>" SortExpression="ServType">
                                                            <HeaderStyle Width="80px" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="ServLevel" HeaderStyle-Width="100px"
                                                            HeaderText="<%$ Resources:Resource,L_LEVEL %>" SortExpression="ServLevel">
                                                            <HeaderStyle Width="100px" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="ServPrice" HeaderStyle-Width="100px"
                                                            HeaderText="<%$ Resources:Resource,L_PRICE %>" SortExpression="ServPrice">
                                                            <HeaderStyle Width="80px" />
                                                        </asp:BoundField>
                                                        <%-- Changes for Nepal >> Start --%>
                                                        <asp:TemplateField ControlStyle-Width="50px">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtLimitationTypeServiceO" runat="server" Width="100%" MaxLength="1" Text='<%# Bind("LimitationType") %>' class="cmbb alphaOnlyL LimitationType"> </asp:TextBox>
                                                                <asp:RegularExpressionValidator ID="rgExpValidLimitationTypeService1" runat="server"
                                                                    ControlToValidate="txtLimitationTypeServiceO" Text="*" SetFocusOnError="True"
                                                                    ValidationExpression="([Ff|Cc]*)" Display="Dynamic"
                                                                    ValidationGroup="check" ForeColor="Red" ></asp:RegularExpressionValidator>
                                                            </ItemTemplate>
                                                            <HeaderTemplate>
                                                                <asp:Label ID="lblLimitationTypeO" runat="server" Text='<%$ Resources:Resource,L_LIMITATIONTYPE %>'> </asp:Label>
                                                            </HeaderTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField ControlStyle-Width="50px">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtLimitationTypeServiceR" runat="server" Width="100%" MaxLength="1" Text='<%# Bind("LimitationTypeR")%>' class="cmbb alphaOnlyL LimitationType"> </asp:TextBox>
                                                                <asp:RegularExpressionValidator ID="rgExpValidLimitationTypeService2" runat="server"
                                                                    ControlToValidate="txtLimitationTypeServiceR" Text="*" SetFocusOnError="True"
                                                                    ValidationExpression="([Ff|Cc]*)" Display="Dynamic"
                                                                    ValidationGroup="check" ForeColor="Red" ></asp:RegularExpressionValidator>
                                                            </ItemTemplate>
                                                            <HeaderTemplate>
                                                                <asp:Label ID="lblLimitationTypeR" runat="server" Text='<%$ Resources:Resource,L_LIMITATIONTYPER %>'> </asp:Label>
                                                            </HeaderTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField ControlStyle-Width="50px">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtLimitationTypeServiceE" runat="server" Width="100%" MaxLength="1" Text='<%# Bind("LimitationTypeE")%>' class="cmbb alphaOnlyL LimitationType"> </asp:TextBox>
                                                                <asp:RegularExpressionValidator ID="rgExpValidLimitationTypeService3" runat="server"
                                                                    ControlToValidate="txtLimitationTypeServiceE" Text="*" SetFocusOnError="True"
                                                                    ValidationExpression="([Ff|Cc]*)" Display="Dynamic"
                                                                    ValidationGroup="check" ForeColor="Red" ></asp:RegularExpressionValidator>
                                                            </ItemTemplate>
                                                            <HeaderTemplate>
                                                                <asp:Label ID="lblLimitationTypeE" runat="server" Text='<%$ Resources:Resource,L_LIMITATIONTYPEE %>'> </asp:Label>
                                                            </HeaderTemplate>
                                                        </asp:TemplateField>
                                                        <%-- Changes for Nepal >> End --%>
                                                        <asp:TemplateField ControlStyle-Width="50px">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtPriceOriginService" runat="server" Width="100%" MaxLength="1" Text='<%# Bind("PriceOrigin") %>' class="cmbb alphaOnlyP"> </asp:TextBox>
                                                                <asp:RegularExpressionValidator ID="RegularExpressionValidatorP2" runat="server"
                                                                    ControlToValidate="txtPriceOriginService" Text="*" SetFocusOnError="True"
                                                                    ValidationExpression="([Pp|Oo|Rr]*)" Display="Dynamic"
                                                                    ValidationGroup="check" ForeColor="Red" ></asp:RegularExpressionValidator>
                                                            </ItemTemplate>
                                                            <HeaderTemplate>
                                                                <asp:Label ID="lblPriceOrigin" runat="server" Text='<%$ Resources:Resource,L_PRICEORIGIN %>'> </asp:Label>
                                                            </HeaderTemplate>
                                                        </asp:TemplateField>
                                                        <%-- Changes for Nepal >> Start --%>
                                                        <asp:TemplateField ControlStyle-Width="50px">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtLimitAdultServiceO" runat="server" Width="100%" Text='<%# Bind("LimitAdult") %>' class="cmbb numbersOnly limitAdult"> </asp:TextBox>
                                                            </ItemTemplate>
                                                            <HeaderTemplate>
                                                                <asp:Label ID="lblLimitAdultO" runat="server" Text='<%$ Resources:Resource,L_LIMITADULT %>'> </asp:Label>
                                                            </HeaderTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField ControlStyle-Width="50px">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtLimitAdultServiceR" runat="server" Width="100%" Text='<%# Bind("LimitAdultR")%>' class="cmbb numbersOnly limitAdult"> </asp:TextBox>
                                                            </ItemTemplate>
                                                            <HeaderTemplate>
                                                                <asp:Label ID="lblLimitAdultR" runat="server" Text='<%$ Resources:Resource,L_LIMITADULTR %>'> </asp:Label>
                                                            </HeaderTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField ControlStyle-Width="50px">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtLimitAdultServiceE" runat="server" Width="100%" Text='<%# Bind("LimitAdultE")%>' class="cmbb numbersOnly limitAdult"> </asp:TextBox>
                                                            </ItemTemplate>
                                                            <HeaderTemplate>
                                                                <asp:Label ID="lblLimitAdultE" runat="server" Text='<%$ Resources:Resource,L_LIMITADULTE %>'> </asp:Label>
                                                            </HeaderTemplate>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField ControlStyle-Width="50px">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtLimitChildServiceO" runat="server" Width="100%" Text='<%# Bind("LimitChild") %>' class="cmbb numbersOnly limitChild"> </asp:TextBox>
                                                            </ItemTemplate>
                                                            <HeaderTemplate>
                                                                <asp:Label ID="lblLimitChildO" runat="server" Text='<%$ Resources:Resource,L_LIMITCHILD %>'> </asp:Label>
                                                            </HeaderTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField ControlStyle-Width="50px">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtLimitChildServiceR" runat="server" Width="100%" Text='<%# Bind("LimitChildR")%>' class="cmbb numbersOnly limitChild"> </asp:TextBox>
                                                            </ItemTemplate>
                                                            <HeaderTemplate>
                                                                <asp:Label ID="lblLimitChildR" runat="server" Text='<%$ Resources:Resource,L_LIMITCHILDR %>'> </asp:Label>
                                                            </HeaderTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField ControlStyle-Width="50px">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtLimitChildServiceE" runat="server" Width="100%" Text='<%# Bind("LimitChildE")%>' class="cmbb numbersOnly limitChild"> </asp:TextBox>
                                                            </ItemTemplate>
                                                            <HeaderTemplate>
                                                                <asp:Label ID="lblLimitChildE" runat="server" Text='<%$ Resources:Resource,L_LIMITCHILDE %>'> </asp:Label>
                                                            </HeaderTemplate>
                                                        </asp:TemplateField>
                                                        <%-- Changes for Nepal >> End --%>
                                                        <%-- Extra for BEPHA: START --%>
                                                        <asp:TemplateField ControlStyle-Width="50px">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtLimitNoAdult" runat="server" Width="100%" Text='<%# Bind("LimitNoAdult") %>' class="cmbb numbersOnly"> </asp:TextBox>

                                                            </ItemTemplate>
                                                            <HeaderTemplate>
                                                                <asp:Label ID="lblLimitNoAdult" runat="server" Text='<%$ Resources:Resource,L_LIMITNOADULT %>'> </asp:Label>
                                                            </HeaderTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField ControlStyle-Width="50px">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtLimitNoChild" runat="server" Width="100%" Text='<%# Bind("LimitNoChild") %>' class="cmbb numbersOnly"> </asp:TextBox>

                                                            </ItemTemplate>
                                                            <HeaderTemplate>
                                                                <asp:Label ID="lblLimitNoChild" runat="server" Text='<%$ Resources:Resource,L_LIMITNOCHILD %>'> </asp:Label>
                                                            </HeaderTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField ControlStyle-Width="50px">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtWaitingPeriodAdult" runat="server" Width="100%" Text='<%# Bind("WaitingPeriodAdult") %>' class="cmbb numbersOnly"> </asp:TextBox>

                                                            </ItemTemplate>
                                                            <HeaderTemplate>
                                                                <asp:Label ID="lblWaitingPeriodAdult" runat="server" Text='<%$ Resources:Resource,L_WAITINGPERIODADULT %>'> </asp:Label>
                                                            </HeaderTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField ControlStyle-Width="50px">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtWaitingPeriodChild" runat="server" Width="100%" Text='<%# Bind("WaitingPeriodChild") %>' class="cmbb numbersOnly"> </asp:TextBox>

                                                            </ItemTemplate>
                                                            <HeaderTemplate>
                                                                <asp:Label ID="lblWaitingPeriodChild" runat="server" Text='<%$ Resources:Resource,L_WAITINGPERIODCHILD %>'> </asp:Label>
                                                            </HeaderTemplate>
                                                        </asp:TemplateField>
                                                        <%-- Extra for BEPHA: END --%>
                                                        <%-- Changes for Nepal >> Start --%>
                                                        <asp:TemplateField ControlStyle-Width="50px">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtCeilingExclusionAdult" runat="server" MaxLength="1" Width="100%" Text='<%# Bind("CeilingExclusionAdult")%>' class="cmbb alphaOnlyC"> </asp:TextBox>
                                                                <%--<asp:RegularExpressionValidator ID="rgExpCeilingAdult" runat="server"
                                                 ControlToValidate="txtCeilingExclusionAdult" Text="*" SetFocusOnError="True"
                                                 ValidationExpression="([Hh|Nn|Bb]*)" Display="Dynamic"
                                                 ValidationGroup="check"></asp:RegularExpressionValidator>--%>
                                                            </ItemTemplate>
                                                            <HeaderTemplate>
                                                                <asp:Label ID="lblCeilingExclusionAdult" runat="server" Text='<%$ Resources:Resource,L_CEILINGADULT %>'> </asp:Label>
                                                            </HeaderTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField ControlStyle-Width="50px">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtCeilingExclusionChild" runat="server" MaxLength="1" Width="100%" Text='<%# Bind("CeilingExclusionChild")%>' class="cmbb alphaOnlyC"> </asp:TextBox>
                                                                <%--<asp:RegularExpressionValidator ID="rgExpCeilingChild" runat="server"
                                                 ControlToValidate="txtCeilingExclusionChild" Text="*" SetFocusOnError="True"
                                                 ValidationExpression="([Hh|Nn|Bb]*)" Display="Dynamic"
                                                 ValidationGroup="check"></asp:RegularExpressionValidator>--%>
                                                            </ItemTemplate>
                                                            <HeaderTemplate>
                                                                <asp:Label ID="lblCeilingExclusionChild" runat="server" Text='<%$ Resources:Resource,L_CEILINGCHILD %>'> </asp:Label>
                                                            </HeaderTemplate>
                                                        </asp:TemplateField>
                                                        <%-- Changes for Nepal >> End --%>
                                                    </Columns>
                                                    <PagerStyle CssClass="pgr" />
                                                    <AlternatingRowStyle CssClass="alt" />
                                                    <SelectedRowStyle CssClass="srs" />
                                                </asp:GridView>
                                            </asp:Panel>
                                            <asp:Button ID="btnLoadMedicalServices" class="loadGrid" Text='<%$ Resources:Resource,L_LOADMEDICALSERVICES %>' runat="server" />
                                            <asp:UpdateProgress ID="progress1" class="progress loadGrid" runat="server" AssociatedUpdatePanelID="upMedicalServices">
                                                <ProgressTemplate>
                                                    <img src="Images/progress.gif" alt="Loading..." style="width: 50px; height: auto;" class="loadGrid" />
                                                </ProgressTemplate>
                                            </asp:UpdateProgress>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>

                            <tr>
                                <td colspan="4" style="position: relative;">
                                    <asp:UpdatePanel ID="upMedicalItems" runat="server" style="position: relative;">
                                        <ContentTemplate>
                                            <asp:CheckBox ID="Checkbox2" runat="server" Text='<%$ Resources:Resource,L_CHECKALL %>' Style="margin-left: 14px;" onClick="toggleCheckMI(this.checked);" Visible="false" />
                                            <asp:Panel ID="pnlMedicalItems" runat="server" ScrollBars="Auto" Height="140px" Width="600px"
                                                CssClass="panel" GroupingText='<%$ Resources:Resource,L_MEDICALITEMS %>'>
                                                <asp:GridView ID="gvMedicalItems" runat="server"
                                                    AutoGenerateColumns="False"
                                                    ShowSelectButton="true"
                                                    GridLines="None"
                                                    AllowPaging="false"
                                                    CssClass="mGrid"
                                                    PagerStyle-CssClass="pgr"
                                                    DataKeyNames="ProdItemID,ItemId,LimitationType,PriceOrigin,LimitAdult,LimitChild,WaitingPeriodAdult,WaitingPeriodChild,LimitNoAdult,LimitNoChild,LimitationTypeR,LimitationTypeE,LimitAdultR,LimitAdultE,LimitChildR,LimitChildE,CeilingExclusionAdult,CeilingExclusionChild" PageSize="12"
                                                    EmptyDataText='<%$ Resources:Resource,M_NOMEDICALITEMS %>'>

                                                    <RowStyle Wrap="False" />

                                                    <Columns>
                                                        <asp:TemplateField HeaderText="Select">
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="chkSelect" runat="server" HeaderStyle-Width="10px" />
                                                            </ItemTemplate>
                                                            <HeaderTemplate>
                                                            </HeaderTemplate>
                                                        </asp:TemplateField>

                                                        <asp:BoundField DataField="ItemCode" HeaderText="<%$ Resources:Resource,L_CODE %>" SortExpression="ItemCode"
                                                            HeaderStyle-Width="50px"></asp:BoundField>
                                                        <asp:BoundField DataField="ItemName" HeaderText="<%$ Resources:Resource,L_NAME %>" SortExpression="ItemName"
                                                            HeaderStyle-Width="160px"></asp:BoundField>
                                                        <asp:BoundField DataField="ItemType" HeaderText="<%$ Resources:Resource,L_TYPE %>" SortExpression="ItemType"
                                                            HeaderStyle-Width="100px"></asp:BoundField>
                                                        <asp:BoundField DataField="ItemPackage" HeaderText="<%$ Resources:Resource,L_PACKAGE %>" SortExpression="ItemPackage"
                                                            HeaderStyle-Width="120px"></asp:BoundField>
                                                        <asp:BoundField DataField="ItemPrice" HeaderText="<%$ Resources:Resource,L_PRICE %>" SortExpression="ItemPrice"
                                                            HeaderStyle-Width="100px"></asp:BoundField>
                                                        <%-- Changes for Nepal >> Start --%>
                                                        <asp:TemplateField ControlStyle-Width="50px">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtLimitationTypeItemO" runat="server" Width="100%" MaxLength="1" Text='<%# Bind("LimitationType") %>' class="cmbb alphaOnlyL LimitationType"> </asp:TextBox>
                                                                <asp:RegularExpressionValidator ID="rgExpValidLimitationTypeItem1" runat="server"
                                                                    ControlToValidate="txtLimitationTypeItemO" Text="*" SetFocusOnError="True"
                                                                    ValidationExpression="([Ff|Cc]*)" Display="Dynamic"
                                                                    ValidationGroup="check" ForeColor="Red" >
                                                                </asp:RegularExpressionValidator>
                                                            </ItemTemplate>
                                                            <HeaderTemplate>
                                                                <asp:Label ID="lblLimitationTypeO" runat="server" Text='<%$ Resources:Resource,L_LIMITATIONTYPE %>'></asp:Label>
                                                            </HeaderTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField ControlStyle-Width="50px">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtLimitationTypeItemR" runat="server" Width="100%" MaxLength="1" Text='<%# Bind("LimitationTypeR")%>' class="cmbb alphaOnlyL LimitationType"> </asp:TextBox>
                                                                <asp:RegularExpressionValidator ID="rgExpValidLimitationTypeItem2" runat="server"
                                                                    ControlToValidate="txtLimitationTypeItemR" Text="*" SetFocusOnError="True"
                                                                    ValidationExpression="([Ff|Cc]*)" Display="Dynamic"
                                                                    ValidationGroup="check" ForeColor="Red" ></asp:RegularExpressionValidator>
                                                            </ItemTemplate>
                                                            <HeaderTemplate>
                                                                <asp:Label ID="lblLimitationTypeR" runat="server" Text='<%$ Resources:Resource,L_LIMITATIONTYPER %>'></asp:Label>
                                                            </HeaderTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField ControlStyle-Width="50px">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtLimitationTypeItemE" runat="server" Width="100%" MaxLength="1" Text='<%# Bind("LimitationTypeE")%>' class="cmbb alphaOnlyL LimitationType"> </asp:TextBox>
                                                                <asp:RegularExpressionValidator ID="rgExpValidLimitationTypeItem3" runat="server"
                                                                    ControlToValidate="txtLimitationTypeItemE" Text="*" SetFocusOnError="True"
                                                                    ValidationExpression="([Ff|Cc]*)" Display="Dynamic"
                                                                    ValidationGroup="check" ForeColor="Red" ></asp:RegularExpressionValidator>
                                                            </ItemTemplate>
                                                            <HeaderTemplate>
                                                                <asp:Label ID="lblLimitationTypeE" runat="server" Text='<%$ Resources:Resource,L_LIMITATIONTYPEE %>'></asp:Label>
                                                            </HeaderTemplate>
                                                        </asp:TemplateField>
                                                        <%-- Changes for Nepal >> End --%>
                                                        <asp:TemplateField ControlStyle-Width="50px">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtPriceOriginItem" runat="server" Width="100%" MaxLength="1" Text='<%# Bind("PriceOrigin") %>' class="cmbb alphaOnlyP"> </asp:TextBox>
                                                                <asp:RegularExpressionValidator ID="RegularExpressionValidatorP1" runat="server"
                                                                    ControlToValidate="txtPriceOriginItem" Text="*" SetFocusOnError="True"
                                                                    ValidationExpression="([Pp|Oo|Rr]*)" Display="Dynamic"
                                                                    ValidationGroup="check" ForeColor="Red" ></asp:RegularExpressionValidator>
                                                            </ItemTemplate>
                                                            <HeaderTemplate>
                                                                <asp:Label ID="lblPriceOrigin" runat="server" Text='<%$ Resources:Resource,L_PRICEORIGIN %>'> </asp:Label>
                                                            </HeaderTemplate>
                                                        </asp:TemplateField>
                                                        <%-- Changes for Nepal >> Start --%>
                                                        <asp:TemplateField ControlStyle-Width="50px">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtLimitAdultItemO" runat="server" Width="100%" Text='<%# Bind("LimitAdult") %>' class="cmbb numbersOnly limitAdult"> </asp:TextBox>
                                                            </ItemTemplate>
                                                            <HeaderTemplate>
                                                                <asp:Label ID="lblLimitAdultO" runat="server" Text='<%$ Resources:Resource,L_LIMITADULT %>'> </asp:Label>
                                                            </HeaderTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField ControlStyle-Width="50px">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtLimitAdultItemR" runat="server" Width="100%" Text='<%# Bind("LimitAdultR")%>' class="cmbb numbersOnly limitAdult"> </asp:TextBox>
                                                            </ItemTemplate>
                                                            <HeaderTemplate>
                                                                <asp:Label ID="lblLimitAdultR" runat="server" Text='<%$ Resources:Resource,L_LIMITADULTR %>'> </asp:Label>
                                                            </HeaderTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField ControlStyle-Width="50px">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtLimitAdultItemE" runat="server" Width="100%" Text='<%# Bind("LimitAdultE")%>' class="cmbb numbersOnly limitAdult"> </asp:TextBox>
                                                            </ItemTemplate>
                                                            <HeaderTemplate>
                                                                <asp:Label ID="lblLimitAdultE" runat="server" Text='<%$ Resources:Resource,L_LIMITADULTE %>'> </asp:Label>
                                                            </HeaderTemplate>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField ControlStyle-Width="50px">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtLimitChildItemO" runat="server" Width="100%" Text='<%# Bind("LimitChild") %>' class="cmbb numbersOnly limitChild"> </asp:TextBox>
                                                            </ItemTemplate>
                                                            <HeaderTemplate>
                                                                <asp:Label ID="lblLimitChildO" runat="server" Text='<%$ Resources:Resource,L_LIMITCHILD %>'> </asp:Label>
                                                            </HeaderTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField ControlStyle-Width="50px">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtLimitChildItemR" runat="server" Width="100%" Text='<%# Bind("LimitChildR")%>' class="cmbb numbersOnly limitChild"> </asp:TextBox>
                                                            </ItemTemplate>
                                                            <HeaderTemplate>
                                                                <asp:Label ID="lblLimitChildR" runat="server" Text='<%$ Resources:Resource,L_LIMITCHILDR %>'> </asp:Label>
                                                            </HeaderTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField ControlStyle-Width="50px">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtLimitChildItemE" runat="server" Width="100%" Text='<%# Bind("LimitChildE")%>' class="cmbb numbersOnly limitChild"> </asp:TextBox>
                                                            </ItemTemplate>
                                                            <HeaderTemplate>
                                                                <asp:Label ID="lblLimitChildE" runat="server" Text='<%$ Resources:Resource,L_LIMITCHILDE %>'> </asp:Label>
                                                            </HeaderTemplate>
                                                        </asp:TemplateField>
                                                        <%-- Changes for Nepal >> End --%>
                                                        <%-- Extra for BEPHA: START --%>
                                                        <asp:TemplateField ControlStyle-Width="50px">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtLimitNoAdultItem" runat="server" Width="100%" Text='<%# Bind("LimitNoAdult") %>' class="cmbb numbersOnly"> </asp:TextBox>

                                                            </ItemTemplate>
                                                            <HeaderTemplate>
                                                                <asp:Label ID="lblLimitNoAdultItem" runat="server" Text='<%$ Resources:Resource,L_LIMITNOADULT %>'> </asp:Label>
                                                            </HeaderTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField ControlStyle-Width="50px">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtLimitNoChildItem" runat="server" Width="100%" Text='<%# Bind("LimitNoChild") %>' class="cmbb numbersOnly"> </asp:TextBox>

                                                            </ItemTemplate>
                                                            <HeaderTemplate>
                                                                <asp:Label ID="lblLimitNoChildItem" runat="server" Text='<%$ Resources:Resource,L_LIMITNOCHILD %>'> </asp:Label>
                                                            </HeaderTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField ControlStyle-Width="50px">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtWaitingPeriodAdultItem" runat="server" Width="100%" Text='<%# Bind("WaitingPeriodAdult") %>' class="cmbb numbersOnly"> </asp:TextBox>

                                                            </ItemTemplate>
                                                            <HeaderTemplate>
                                                                <asp:Label ID="lblWaitingPeriodAdultItem" runat="server" Text='<%$ Resources:Resource,L_WAITINGPERIODADULT %>'> </asp:Label>
                                                            </HeaderTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField ControlStyle-Width="50px">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtWaitingPeriodChildItem" runat="server" Width="100%" Text='<%# Bind("WaitingPeriodChild") %>' class="cmbb numbersOnly"> </asp:TextBox>
                                                            </ItemTemplate>
                                                            <HeaderTemplate>
                                                                <asp:Label ID="lblWaitingPeriodChildItem" runat="server" Text='<%$ Resources:Resource,L_WAITINGPERIODCHILD %>'> </asp:Label>
                                                            </HeaderTemplate>
                                                        </asp:TemplateField>
                                                        <%-- Extra for BEPHA: END --%>
                                                        <%-- Changes for Nepal >> Start --%>
                                                        <asp:TemplateField ControlStyle-Width="50px">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtItemCeilingExclusionAdult" runat="server" MaxLength="1" Width="100%" Text='<%# Bind("CeilingExclusionAdult")%>' class="cmbb"> </asp:TextBox>
                                                            </ItemTemplate>
                                                            <HeaderTemplate>
                                                                <asp:Label ID="lblItemCeilingExclusionAdult" runat="server" Text='<%$ Resources:Resource,L_CEILINGADULT %>'> </asp:Label>
                                                            </HeaderTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField ControlStyle-Width="50px">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtItemCeilingExclusionChild" runat="server" MaxLength="1" Width="100%" Text='<%# Bind("CeilingExclusionChild")%>' class="cmbb"> </asp:TextBox>
                                                            </ItemTemplate>
                                                            <HeaderTemplate>
                                                                <asp:Label ID="lblItemCeilingExclusionChild" runat="server" Text='<%$ Resources:Resource,L_CEILINGCHILD %>'> </asp:Label>
                                                            </HeaderTemplate>
                                                        </asp:TemplateField>
                                                        <%-- Changes for Nepal >> End --%>
                                                    </Columns>

                                                    <PagerStyle CssClass="pgr" />
                                                    <AlternatingRowStyle CssClass="alt" />
                                                    <SelectedRowStyle CssClass="srs" />

                                                </asp:GridView>
                                            </asp:Panel>
                                            <asp:Button ID="btnLoadMedicalItems" class="loadGrid" Text='<%$ Resources:Resource,L_LOADMEDICALITEMS %>' runat="server" />
                                            <asp:UpdateProgress ID="UpdateProgress1" class="progress loadGrid" runat="server" AssociatedUpdatePanelID="upMedicalItems">
                                                <ProgressTemplate>
                                                    <img src="Images/progress.gif" alt="Loading..." style="width: 50px; height: auto;" class="loadGrid" />
                                                </ProgressTemplate>
                                            </asp:UpdateProgress>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td class="FormLabel">
                                    <asp:Label ID="L_ACCOUNTCODEREMUNERATION" runat="server" Width="180px" Text='<%$ Resources:Resource,L_ACCCODEREMUNERATION %>'></asp:Label>
                                </td>
                                <td class="DataEntry">
                                    <asp:TextBox ID="txtAccCodeRemuneration" runat="server" Width="100px"></asp:TextBox>
                                </td>
                                <td class="FormLabel">
                                    <asp:Label ID="L_ACCCODEPREMIUM" runat="server" Width="180px" Text='<%$ Resources:Resource,L_ACCCODEPREMIUM %>'></asp:Label>
                                </td>
                                <td class="DataEntry">
                                    <asp:TextBox ID="txtAccCodePremiums" runat="server" Width="100px"></asp:TextBox>
                                </td>
                            </tr>
                            <%-- Extra for BEPHA: START --%>
                            <tr>
                                <td class="FormLabel">
                                    <asp:Label ID="lblRegLumpSum" runat="server" Width="150px" Text='<%$ Resources:Resource,L_REGISTRATIONLUMPSUM %>'></asp:Label>
                                </td>
                                <td class="DataEntry">
                                    <asp:TextBox ID="txtRegLumpSum" runat="server" Style="text-align: right; width: 100px;" class="numbersOnly"></asp:TextBox>
                                </td>
                                <td class="FormLabel">
                                    <asp:Label ID="lblGenAssemblyLumpsum" runat="server" Width="150px" Text='<%$ Resources:Resource,L_GENERALASSEMBLYLUMPSUM %>'></asp:Label>
                                </td>
                                <td class="DataEntry">
                                    <asp:TextBox ID="txtGenAssemblyLumpsum" runat="server" Style="text-align: right; width: 100px;" class="numbersOnly"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="FormLabel">
                                    <asp:Label ID="lblRegFee" runat="server" Width="150px" Text='<%$ Resources:Resource,L_REGISTRATIONFEE %>'></asp:Label>
                                </td>
                                <td class="DataEntry">
                                    <asp:TextBox ID="txtRegFee" runat="server" Style="text-align: right; width: 100px;" class="numbersOnly"></asp:TextBox>
                                </td>
                                <td class="FormLabel">
                                    <asp:Label ID="lblGenAssemblyFee" runat="server" Width="150px" Text='<%$ Resources:Resource,L_GENERALASSEMBLYFEE %>'></asp:Label>
                                </td>
                                <td class="DataEntry">
                                    <asp:TextBox ID="txtGenAssemblyFee" runat="server" Style="text-align: right; width: 100px;" class="numbersOnly"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="FormLabel">
                                    <asp:Label ID="lblRecurrence" runat="server" Text="<%$ Resources:Resource,L_RECURRENCE %>" Width="150px"></asp:Label>
                                </td>
                                <td class="DataEntry">
                                    <asp:TextBox ID="txtRecurrence" runat="server" class="numbersOnly" Style="text-align: right; width: 100px;"></asp:TextBox>
                                </td>
                                <td class="FormLabel">&nbsp;</td>
                                <td class="DataEntry">&nbsp;</td>
                            </tr>
                            <tr>
                                <td class="FormLabel">
                                    <asp:Label ID="lblStartCycle1" runat="server" Width="150px"
                                        Text='<%$ Resources:Resource,L_STARTCYCLE1 %>'></asp:Label>
                                </td>
                                <td class="DataEntry">
                                    <div style="width: 115px;">
                                        <asp:DropDownList ID="ddlCycle1Day" runat="server" Width="50px"></asp:DropDownList>
                                        <asp:DropDownList ID="ddlCycle1Month" runat="server" Width="60px"></asp:DropDownList>
                                    </div>
                                </td>
                                <td class="FormLabel">
                                    <asp:Label ID="lblStartCycle3" runat="server" Width="150px"
                                        Text='<%$ Resources:Resource,L_STARTCYCLE3 %>'></asp:Label>
                                </td>
                                <td class="DataEntry">
                                    <div style="width: 115px;">
                                        <asp:DropDownList ID="ddlCycle3Day" runat="server" Width="50px"></asp:DropDownList>
                                        <asp:DropDownList ID="ddlCycle3Month" runat="server" Width="60px"></asp:DropDownList>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td class="FormLabel">
                                    <asp:Label ID="lblStartCycle2" runat="server" Width="150px"
                                        Text='<%$ Resources:Resource,L_STARTCYCLE2 %>'></asp:Label>
                                </td>
                                <td class="DataEntry">
                                    <div style="width: 115px;">
                                        <asp:DropDownList ID="ddlCycle2Day" runat="server" Width="50px"></asp:DropDownList>
                                        <asp:DropDownList ID="ddlCycle2Month" runat="server" Width="60px"></asp:DropDownList>
                                    </div>
                                </td>
                                <td class="FormLabel">
                                    <asp:Label ID="lblStartCyle4" runat="server" Width="150px"
                                        Text='<%$ Resources:Resource,L_STARTCYCLE4 %>'></asp:Label>
                                </td>
                                <td class="DataEntry">
                                    <div style="width: 115px;">
                                        <asp:DropDownList ID="ddlCycle4Day" runat="server" Width="50px"></asp:DropDownList>
                                        <asp:DropDownList ID="ddlCycle4Month" runat="server" Width="60px"></asp:DropDownList>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td class="FormLabel">
                                    <asp:Label ID="lblCeilingInterpritation" runat="server" Width="150px"
                                        Text='<%$ Resources:Resource,L_CEILINGINTERPRETATION %>'></asp:Label></td>
                                <td class="DataEntry">
                                    <div style="width: 115px;">
                                        <asp:DropDownList ID="ddlCeilingInterpretation" runat="server" Width="100px"></asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredCeilingInterpretation" Text="*" runat="server" ControlToValidate="ddlCeilingInterpretation" SetFocusOnError="True"
                                        ValidationGroup="check" ForeColor="Red" Display="Dynamic">
                                    </asp:RequiredFieldValidator>
                                    </div>

                                </td>
                                <td>
                                    
                                </td>
                            </tr>

                            <%-- Extra for BEPHA: END --%>
                            
                          
                             
                        </table>
                    </td>
                </tr>

            </table>
            <table cellpadding="5px" id="distribution-table" style="position: relative; left: -14px;">
                <tr>
                    <td>&nbsp;<asp:CompareValidator ControlToValidate="txtDedutibleForTreatment" ID="CompareValidator7" runat="server" SetFocusOnError="true" Type="Currency" Operator="DataTypeCheck" ErrorMessage="*" ValidationGroup="check"> </asp:CompareValidator><asp:CompareValidator ControlToValidate="txtMaxTreatment" ID="CompareValidator8" runat="server" SetFocusOnError="true" Type="Currency" Operator="DataTypeCheck" ErrorMessage="*" ValidationGroup="check"> </asp:CompareValidator><asp:CompareValidator ControlToValidate="txtDedIPTreatment" ID="CompareValidator9" runat="server" SetFocusOnError="true" Type="Currency" Operator="DataTypeCheck" ErrorMessage="*" ValidationGroup="check"> </asp:CompareValidator><asp:CompareValidator ControlToValidate="txtMaxIPTreatment" ID="CompareValidator10" runat="server" SetFocusOnError="true" Type="Currency" Operator="DataTypeCheck" ErrorMessage="*" ValidationGroup="check"> </asp:CompareValidator><asp:CompareValidator ControlToValidate="txtDedOPTreatment" ID="CompareValidator11" runat="server" SetFocusOnError="true" Type="Currency" Operator="DataTypeCheck" ErrorMessage="*" ValidationGroup="check"> </asp:CompareValidator><asp:CompareValidator ControlToValidate="txtMaxOPTreatment" ID="CompareValidator12" runat="server" SetFocusOnError="true" Type="Currency" Operator="DataTypeCheck" ErrorMessage="*" ValidationGroup="check"> </asp:CompareValidator><asp:CompareValidator ControlToValidate="txtDedInsuree" ID="CompareValidator13" runat="server" SetFocusOnError="true" Type="Currency" Operator="DataTypeCheck" ErrorMessage="*" ValidationGroup="check"> </asp:CompareValidator><asp:CompareValidator ControlToValidate="txtMaxInsuree" ID="CompareValidator14" runat="server" SetFocusOnError="true" Type="Currency" Operator="DataTypeCheck" ErrorMessage="*" ValidationGroup="check"> </asp:CompareValidator><asp:CompareValidator ControlToValidate="txtDedIPInsuree" ID="CompareValidator15" runat="server" SetFocusOnError="true" Type="Currency" Operator="DataTypeCheck" ErrorMessage="*" ValidationGroup="check"> </asp:CompareValidator><asp:CompareValidator ControlToValidate="txtMaxIPInsuree" ID="CompareValidator16" runat="server" SetFocusOnError="true" Type="Currency" Operator="DataTypeCheck" ErrorMessage="*" ValidationGroup="check"> </asp:CompareValidator><asp:CompareValidator ControlToValidate="txtDedOPInsuree" ID="CompareValidator17" runat="server" SetFocusOnError="true" Type="Currency" Operator="DataTypeCheck" ErrorMessage="*" ValidationGroup="check"> </asp:CompareValidator><asp:CompareValidator ControlToValidate="txtMaxOPInsuree" ID="CompareValidator18" runat="server" SetFocusOnError="true" Type="Currency" Operator="DataTypeCheck" ErrorMessage="*" ValidationGroup="check"> </asp:CompareValidator><asp:CompareValidator ControlToValidate="txtDedPolicy" ID="CompareValidator19" runat="server" SetFocusOnError="true" Type="Currency" Operator="DataTypeCheck" ErrorMessage="*" ValidationGroup="check"> </asp:CompareValidator><asp:CompareValidator ControlToValidate="txtMaxPolicy" ID="CompareValidator20" runat="server" SetFocusOnError="true" Type="Currency" Operator="DataTypeCheck" ErrorMessage="*" ValidationGroup="check"> </asp:CompareValidator><asp:CompareValidator ControlToValidate="txtDedIPPolicy" ID="CompareValidator21" runat="server" SetFocusOnError="true" Type="Currency" Operator="DataTypeCheck" ErrorMessage="*" ValidationGroup="check"> </asp:CompareValidator><asp:CompareValidator ControlToValidate="txtMaxIPPOlicy" ID="CompareValidator22" runat="server" SetFocusOnError="true" Type="Currency" Operator="DataTypeCheck" ErrorMessage="*" ValidationGroup="check"> </asp:CompareValidator><asp:CompareValidator ControlToValidate="txtDedOPPolicy" ID="CompareValidator23" runat="server" SetFocusOnError="true" Type="Currency" Operator="DataTypeCheck" ErrorMessage="*" ValidationGroup="check"> </asp:CompareValidator><asp:CompareValidator ControlToValidate="txtMaxOPPolicy" ID="CompareValidator24" runat="server" SetFocusOnError="true" Type="Currency" Operator="DataTypeCheck" ErrorMessage="*" ValidationGroup="check"> </asp:CompareValidator>
                    </td>
                    <td align="center">
                        <asp:Label ID="Label6" runat="server"
                            Text='<%$ Resources:Resource,L_DEDUCTABLE %>'>
                        </asp:Label></td>
                    <td align="center">
                        <asp:Label ID="Label3" runat="server" Width="150px"
                            Text='<%$ Resources:Resource,L_CEILING %>'>
                        </asp:Label></td>
                    <td align="center" colspan="2">
                        <asp:Label ID="Label5" runat="server" Width="240px"
                            Text='<%$ Resources:Resource,L_DEDUCTABLEINCEILING %>'>
                        </asp:Label></td>
                    <td>
                        <asp:Label ID="Label1" runat="server" Width="10px"
                            Text="">
                        </asp:Label></td>
                    <td align="center" colspan="2">
                        <asp:Label ID="Label4" runat="server"
                            Text='<%$ Resources:Resource,L_DEDUCTABLEOUTCIELING %>'>
                        </asp:Label></td>


                </tr>
                <tr>
                    <td class="FormLabel">
                        <asp:Label ID="Label13" runat="server"
                            Text='<%$ Resources:Resource,L_TREATMENT %>'>
                        </asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtDedutibleForTreatment" runat="server"
                            Style="text-align: right" Width="120px" class="numbersOnly"></asp:TextBox>

                    </td>
                    <td>
                        <asp:TextBox ID="txtMaxTreatment" runat="server" Style="text-align: right"
                            Width="120px" class="numbersOnly"></asp:TextBox>

                    </td>
                    <td>
                        <asp:TextBox ID="txtDedIPTreatment" runat="server" Style="text-align: right"
                            Width="120px" class="numbersOnly"></asp:TextBox>

                    </td>
                    <td>
                        <asp:TextBox ID="txtMaxIPTreatment" runat="server" Style="text-align: right"
                            Width="120px" right-margin="20px" class="numbersOnly"></asp:TextBox>

                    </td>
                    <td>
                        <asp:Label ID="Label2" runat="server" Width="10px"
                            Text="">
                        </asp:Label></td>
                    <td>
                        <asp:TextBox ID="txtDedOPTreatment" runat="server" Style="text-align: right"
                            Width="120px" class="numbersOnly"></asp:TextBox>

                    </td>
                    <td>
                        <asp:TextBox ID="txtMaxOPTreatment" runat="server" Style="text-align: right" Width="120px" class="numbersOnly"></asp:TextBox>

                    </td>

                </tr>
                <tr>
                    <td class="FormLabel">
                        <asp:Label ID="Label14" runat="server"
                            Text='<%$ Resources:Resource,L_INSUREE %>'>
                        </asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtDedInsuree" runat="server" Style="text-align: right"
                            Width="120px" class="numbersOnly"></asp:TextBox>

                    </td>
                    <td>
                        <asp:TextBox ID="txtMaxInsuree" runat="server" Style="text-align: right" class="numbersOnly"
                            Width="120px"></asp:TextBox>

                    </td>
                    <td>
                        <asp:TextBox ID="txtDedIPInsuree" runat="server" Style="text-align: right" class="numbersOnly"
                            Width="120px"></asp:TextBox>

                    </td>
                    <td>
                        <asp:TextBox ID="txtMaxIPInsuree" runat="server" Style="text-align: right" class="numbersOnly"
                            Width="120px" right-margin="20px"></asp:TextBox>

                    </td>
                    <td>
                        <asp:Label ID="Label8" runat="server" Width="20px"
                            Text="">
                        </asp:Label></td>
                    <td>
                        <asp:TextBox ID="txtDedOPInsuree" runat="server" Style="text-align: right" class="numbersOnly"
                            Width="120px"></asp:TextBox>

                    </td>
                    <td>
                        <asp:TextBox ID="txtMaxOPInsuree" runat="server" Style="text-align: right" Width="120px" class="numbersOnly"></asp:TextBox>

                    </td>

                </tr>
                <tr>
                    <td class="FormLabel">
                        <asp:Label ID="Label15" runat="server" Text='<%$ Resources:Resource,L_POLICY %>'></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtDedPolicy" runat="server" Style="text-align: right" class="numbersOnly" Width="120px"></asp:TextBox>
                    </td>
                    <td>
                        <asp:TextBox ID="txtMaxPolicy" runat="server" Style="text-align: right" class="numbersOnly" Width="120px"></asp:TextBox>
                    </td>
                    <td>
                        <asp:TextBox ID="txtDedIPPolicy" runat="server" Style="text-align: right" Width="120px" class="numbersOnly"></asp:TextBox>
                    </td>
                    <td>
                        <asp:TextBox ID="txtMaxIPPOlicy" runat="server" Style="text-align: right" Width="120px" right-margin="20px" class="numbersOnly"></asp:TextBox>
                    </td>
                    <td>
                        <asp:Label ID="Label9" runat="server" Width="20px" Text=""></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtDedOPPolicy" runat="server" Style="text-align: right" Width="120px" class="numbersOnly"></asp:TextBox>
                    </td>
                    <td>
                        <asp:TextBox ID="txtMaxOPPolicy" runat="server" Style="text-align: right" Width="120px" class="numbersOnly"></asp:TextBox>
                    </td>
                </tr>
                <%-- Addition for  Nepal >> Start --%>
                <tr>
                    <td class="FormLabel">
                        <asp:Label ID="lblExtraMemberCeiling" runat="server" Text='<%$ Resources:Resource,L_EXTRAMEMBERCEILING %>'></asp:Label>
                    </td>
                    <td></td>
                    <td>
                        <asp:TextBox ID="txtMaxPolicyExtraMember" runat="server" Style="text-align: right" class="numbersOnly" Width="120px"></asp:TextBox>
                    </td>
                    <td></td>
                    <td>
                        <asp:TextBox ID="txtMaxIPPolicyExtraMember" runat="server" Style="text-align: right" Width="120px" class="numbersOnly"></asp:TextBox>
                    </td>
                    <td></td>
                    <td></td>
                    <td>
                        <asp:TextBox ID="txtMaxOPPolicyExtraMember" runat="server" Style="text-align: right" Width="120px" class="numbersOnly"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="FormLabel">
                        <asp:Label ID="Label19" runat="server" Text='<%$ Resources:Resource,L_MAXIMUMCEILING %>'></asp:Label>
                    </td>
                    <td></td>
                    <td>
                        <asp:TextBox ID="txtMaxPolicyMC" runat="server" Style="text-align: right" class="numbersOnly" Width="120px"></asp:TextBox>
                    </td>
                    <td></td>
                    <td>
                        <asp:TextBox ID="txtMaxIPPolicyMC" runat="server" Style="text-align: right" Width="120px" right-margin="20px" class="numbersOnly"></asp:TextBox>
                    </td>
                    <td></td>
                    <td></td>
                    <td>
                        <asp:TextBox ID="txtMaxOPPolicyMC" runat="server" Style="text-align: right" Width="120px" class="numbersOnly"></asp:TextBox>
                    </td>
                </tr>
                <%-- Addition for  Nepal >> End --%>
                <%-- frequency & ceilings: START --%>
                <tr>
                    <%--top labels--%>
                    <td></td>
                    <td style="text-align: center;">
                        <asp:Label ID="Label18" runat="server"
                            Text='<%$ Resources:Resource,L_CONSULTATIONS %>'>
                        </asp:Label>
                    </td>
                    <td style="text-align: center;">
                        <asp:Label ID="Label20" runat="server"
                            Text='<%$ Resources:Resource,L_SURGERIES %>'>
                        </asp:Label>
                    </td>
                    <td style="text-align: center;">
                        <asp:Label ID="Label22" runat="server"
                            Text='<%$ Resources:Resource,L_DELIVERIES %>'>
                        </asp:Label>
                    </td>
                    <td style="text-align: center;">
                        <asp:Label ID="Label10" runat="server"
                            Text='<%$ Resources:Resource,L_HOSPITALIZATIONS %>'>
                        </asp:Label>
                    </td>
                    <td style="text-align: center;">
                        <asp:Label ID="Label17" runat="server" Width="10px" Text=""></asp:Label>
                    </td>
                    <td style="text-align: center;">
                        <asp:Label ID="Label21" runat="server"
                            Text='<%$ Resources:Resource,T_ANTENATAL %>'>
                        </asp:Label>
                    </td>
                    <td style="text-align: center;">
                        <asp:Label ID="Label12" runat="server"
                            Text='<%$ Resources:Resource,L_VISITS %>'>
                        </asp:Label>
                    </td>
                    <td></td>
                </tr>
                <tr>
                    <%--number--%>
                    <td class="FormLabel" align="center">
                        <asp:Label ID="lblNumber" runat="server"
                            Text='<%$ Resources:Resource,L_NUMBER %>'></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtNumConsultations" runat="server"
                            Style="text-align: right" Width="120px" class="numbersOnly"></asp:TextBox>
                    </td>
                    <td>
                        <asp:TextBox ID="txtNumSurgeries" runat="server" Style="text-align: right"
                            Width="120px" class="numbersOnly"></asp:TextBox>
                    </td>
                    <td>
                        <asp:TextBox ID="txtNumDeliveries" runat="server" Style="text-align: right" class="numbersOnly"
                            Width="120px"></asp:TextBox>
                    </td>
                    <td>
                        <asp:TextBox ID="txtNumHospitalizations" runat="server" Style="text-align: right" class="numbersOnly"
                            Width="120px"></asp:TextBox>
                    </td>
                    <td>
                        <asp:Label ID="Label11" runat="server" Width="20px" Text=""></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtNumAntenatal" runat="server" Style="text-align: right" class="numbersOnly"
                            Width="120px"></asp:TextBox>
                    </td>
                    <td>
                        <asp:TextBox ID="txtNumVisits" runat="server" Style="text-align: right" class="numbersOnly"
                            Width="120px"></asp:TextBox>
                    </td>
                    <td></td>
                </tr>
                <tr>
                    <%--ceilings--%>
                    <td class="FormLabel" align="center">
                        <asp:Label ID="lblCeiling" runat="server" Width="150px"
                            Text='<%$ Resources:Resource,L_CEILING %>'></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtMaxAmountConsultations" runat="server" Style="text-align: right"
                            Width="120px" class="numbersOnly"></asp:TextBox>
                    </td>
                    <td>
                        <asp:TextBox ID="txtMaxAmountSurgeries" runat="server" Style="text-align: right" class="numbersOnly"
                            Width="120px"></asp:TextBox>
                    </td>
                    <td>
                        <asp:TextBox ID="txtMaxAmountDeliveries" runat="server" Style="text-align: right" class="numbersOnly"
                            Width="120px"></asp:TextBox>
                    </td>
                    <td>
                        <asp:TextBox ID="txtMaxAmountHospitalizations" runat="server" Style="text-align: right" class="numbersOnly"
                            Width="120px"></asp:TextBox>
                    </td>
                    <td>
                        <asp:Label ID="Label16" runat="server" Width="20px" Text=""></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtMaxAmountAntenatal" runat="server" Style="text-align: right" class="numbersOnly"
                            Width="120px"></asp:TextBox>
                    </td>
                    <td></td>
                    <td></td>
                </tr>
                <%-- frequency & ceilings: END --%>
            </table>
            <table style="position: relative; left: 70px;">
                <tr>
                    <td class="FormLabel" valign="top">
                        <asp:Label ID="Label7" runat="server"
                            Text='<%$ Resources:Resource,L_DISTRIBUTION %>'>
                        </asp:Label>
                    </td>
                    <td valign="top">
                        <asp:DropDownList ID="ddlDistribution" runat="server" Width="125" class="ddlpopup"></asp:DropDownList></td>
                    <td>
                        <asp:Panel ID="Panel1" runat="server" Width="130" Height="65px" Style="margin-right: 20px" ScrollBars="Vertical">
                            <asp:GridView ID="gvDistrB" runat="server" CssClass="distributionGrid" AutoGenerateColumns="False">
                                <Columns>
                                    <asp:BoundField DataField="Period" HeaderStyle-Width="55px" HeaderText="<%$ Resources:Resource,L_PERIOD %>" SortExpression="Period"></asp:BoundField>
                                    <asp:BoundField DataField="DISTRPERC" HeaderStyle-Width="55px" HeaderText="<%$ Resources:Resource,L_PERCENT %>" SortExpression="DISTRPERC" DataFormatString="{0:P}" ItemStyle-HorizontalAlign="Right"></asp:BoundField>
                                </Columns>


                            </asp:GridView>
                        </asp:Panel>
                    </td>
                    <td valign="top">
                        <asp:DropDownList ID="ddlDistributionIP" runat="server" Width="125" class="ddlpopup"></asp:DropDownList></td>
                    <td valign="top">
                        <asp:Panel ID="Panel2" runat="server" Width="130" Height="65px" Style="margin-right: 20px" ScrollBars="Vertical">
                            <asp:GridView ID="gvDistrI" runat="server" AutoGenerateColumns="False" CssClass="distributionGrid">
                                <Columns>
                                    <asp:BoundField DataField="Period" HeaderStyle-Width="55px" HeaderText="<%$ Resources:Resource,L_PERIOD %>" SortExpression="Period"></asp:BoundField>
                                    <asp:BoundField DataField="DISTRPERC" HeaderStyle-Width="55px" HeaderText="<%$ Resources:Resource,L_PERCENT %>" SortExpression="DISTRPERC" DataFormatString="{0:P}" ItemStyle-HorizontalAlign="Right"></asp:BoundField>
                                </Columns>
                            </asp:GridView>
                        </asp:Panel>
                    </td>
                    <td valign="top">
                        <asp:DropDownList ID="ddlDistributionOP" runat="server" Width="125" class="ddlpopup"></asp:DropDownList></td>
                    <td valign="top">
                        <asp:Panel ID="Panel3" runat="server" Width="125" Height="65px" ScrollBars="Vertical">
                            <asp:GridView ID="gvDistrO" runat="server" AutoGenerateColumns="False" CssClass="distributionGrid">
                                <Columns>
                                    <asp:BoundField DataField="Period" HeaderStyle-Width="55px" HeaderText="<%$ Resources:Resource,L_PERIOD %>" SortExpression="Period"></asp:BoundField>
                                    <asp:BoundField DataField="DISTRPERC" HeaderStyle-Width="55px" HeaderText="<%$ Resources:Resource,L_PERCENT %>" SortExpression="DISTRPERC" DataFormatString="{0:P}" ItemStyle-HorizontalAlign="Right"></asp:BoundField>
                                </Columns>
                            </asp:GridView>
                        </asp:Panel>
                    </td>
                </tr>
                
            </table>
             <asp:Panel ID="Panel4" runat="server"   GroupingText='<%$ Resources:Resource,T_CAPITATIONPAYMENT %>'
                                                ScrollBars="Auto" Width="960px">
            <table>
                  <%--
                                //************************** Start Un comment this for Version 5.1**************************//
                             --%>



                            <tr>
                                <td class="FormLabel">
                                    <asp:Label ID="Level1" runat="server" Text='<%$ Resources:Resource,L_LEVEL1 %>'></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlLevel1" runat="server" Width="130"></asp:DropDownList>
                                </td>
                                <td class="FormLabel">
                                    <asp:Label ID="lblLevel2" runat="server" Text='<%$ Resources:Resource,L_LEVEL2 %>'></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlLevel2"  runat="server" Width="130" ></asp:DropDownList>
                                </td>
                                 <td class="FormLabel">
                                    <asp:Label ID="lblLevel3" runat="server" Text='<%$ Resources:Resource,L_LEVEL3 %>'></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlLevel3" runat="server" Width="130" ></asp:DropDownList>
                                </td>
                                <td class="FormLabel">
                                    <asp:Label ID="lblLevel4" runat="server" Text='<%$ Resources:Resource,L_LEVEL4 %>'></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlLevel4" runat="server" Width="130"></asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="FormLabel">
                                    <asp:Label ID="lblSubLevel1" runat="server" Text='<%$ Resources:Resource,L_SUBLEVEL1 %>'></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlSubLevel1" runat="server" Width="130"></asp:DropDownList>
                                </td>
                                <td class="FormLabel">
                                    <asp:Label ID="lblSubLevel2" runat="server" Text='<%$ Resources:Resource,L_SUBLEVEL2 %>'></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlSubLevel2" runat="server" Width="130"></asp:DropDownList>
                                </td>
                                 <td class="FormLabel">
                                    <asp:Label ID="lblSubLevel3" runat="server" Text='<%$ Resources:Resource,L_SUBLEVEL3 %>'></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlSubLevel3" runat="server" Width="130" ></asp:DropDownList>
                                </td>
                                <td class="FormLabel">
                                    <asp:Label ID="lblSubLevel4" runat="server" Text='<%$ Resources:Resource,L_SUBLEVEL4 %>'></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlSubLevel4" runat="server" Width="130"></asp:DropDownList>
                                </td>

                            </tr>
                            <tr>
                                <td class="FormLabel">
                                    <asp:Label ID="lblShareContribution" runat="server" Text='<%$ Resources:Resource,L_SHAREOFCONTRIBUTION %>'></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtShareContribution" class="numbersOnly" MaxLength="3" runat="server" Width="130" ></asp:TextBox>
                                     <asp:RequiredFieldValidator
                                        ID="RequiredFieldValidator3" runat="server"
                                        ControlToValidate="txtShareContribution"
                                        SetFocusOnError="True"
                                        ValidationGroup="check"
                                        Text="*"></asp:RequiredFieldValidator>
                                  
                                </td>
                            
                                <td class="FormLabel">
                                    <asp:Label ID="lblWeightOfPopulation" runat="server" Text='<%$ Resources:Resource,L_WEIGHTOFPOPUATION %>'></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtWeightOfPopulation" class="numbersOnly" MaxLength="3" runat="server" Width="130" ></asp:TextBox>
                                </td>
                                <td class="FormLabel">
                                    <asp:Label ID="lblNoOfFamilies" runat="server" Text='<%$ Resources:Resource,L_WEGHTOFNUMBERFAMILIES %>'></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtNumberOfFamilies" class="numbersOnly" MaxLength="3" runat="server" Width="130" ></asp:TextBox>
                                </td>
                             
                                <td class="FormLabel">
                                    <asp:Label ID="lblNoOfInsuredPopulation" runat="server" Text='<%$ Resources:Resource,L_WEIGHTOFINSUREDPOPULATION %>'></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtNoOfInsuredPopulation" class="numbersOnly" MaxLength="3" runat="server" Width="130" ></asp:TextBox>
                                </td>
                            </tr>
                            

                            <tr>
                                <td class="FormLabel">
                                    <asp:Label ID="lblNoOfInseredFamilies" runat="server" Text='<%$ Resources:Resource,L_WEIGHTOFNUMBERINSUREDFAMILIES
 %>'></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtNoOfInseredFamilies" class="numbersOnly" MaxLength="3" runat="server" Width="130" ></asp:TextBox>
                                </td>
                             
                                <td class="FormLabel">
                                    <asp:Label ID="lblNoOfClaims" runat="server" Text='<%$ Resources:Resource,L_WEIGHTOFNUMBERVISITS
 %>'></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtNumberOfClaims" class="numbersOnly" MaxLength="3" runat="server" Width="130" ></asp:TextBox>
                                </td>
                                <td class="FormLabel">
                                    <asp:Label ID="lblAdjustedAmount" runat="server" Text='<%$ Resources:Resource,L_WEIGHTOFADJUSTEDAMOUT
 %>'></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtAdjustedAmount" class="numbersOnly" MaxLength="3" Width="130" runat="server"  ></asp:TextBox>
                                </td>
                            </tr>

                             
                     

                            <%--*****************************************END OF COMMENT**********************************************************--%>
            </table>
                 </asp:Panel>
            <div id="gridViewsCopiesContainer" style="display: none;">
                <%-- <table id="gvDistrBCopy"></table>
             <table id="gvDistrIPCopy"></table>
             <table id="gvDistrOPCopy"></table>--%>
                <asp:GridView ID="gvHiddenDistribution" runat="server" AutoGenerateColumns="False" DataKeyNames="DistrID" BackColor="#FFFFFF" GridLines="Horizontal">
                    <Columns>
                        <asp:BoundField DataField="DistrID"></asp:BoundField>
                        <asp:BoundField DataField="DistrType"></asp:BoundField>
                        <asp:BoundField DataField="DistrCareType"></asp:BoundField>
                        <asp:BoundField DataField="Period"></asp:BoundField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:TextBox ID="txtDistrPerc" runat="server" Text='<%#Bind("DistrPerc") %>'></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="DistrPerc"></asp:BoundField>
                        <asp:BoundField DataField="RowV"></asp:BoundField>
                    </Columns>
                </asp:GridView>
            </div>
            <asp:HiddenField ID="hfGvDistrBState" Value="0" runat="server" />

            <asp:HiddenField ID="hfGvDistrIPState" Value="0" runat="server" />

            <asp:HiddenField ID="hfGvDistrOPState" Value="0" runat="server" />

        </asp:Panel>
    </div>

    <asp:Panel ID="pnlButtons" runat="server" CssClass="panelbuttons">
        <table width="100%" cellpadding="10 10 10 10">
            <tr>
                <td align="left">
                    <asp:Button
                        ID="B_SAVE"
                        runat="server"
                        Text='<%$ Resources:Resource,B_SAVE%>'
                        ValidationGroup="check" />
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



</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="Footer" Runat="Server" Visible="true" >
     <asp:ValidationSummary ID="validationSummary" runat="server" HeaderText='<%$ Resources:Resource,V_SUMMARY%>' ValidationGroup="check" />
    <asp:Label text="" runat="server" ID="lblMsg"> </asp:Label>
</asp:Content>
