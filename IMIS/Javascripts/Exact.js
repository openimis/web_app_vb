$(document).ready(function () {
    createSelect2();
})

$(function () {
    Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
});

function EndRequestHandler(sender, args) {
    createSelect2();
}

function createSelect2() {
    //$("select:not(.noSelect2)").select2({
    //    dropdownAutoWidth: true
    //});

    //// This is for calendar control
    //$('.dateCheck').parent().css("position", "relative")
}

$(document).ready(function() { bindAlphaNumber(); });
function bindAlphaNumber() {
    $(".alphaOnly").unbind("keypress");
    $(".alphaOnly").keypress(function(event) {
    var e = String.fromCharCode(event.which);
        if (e.match('[^aA-zZ\\s]') && !(event.which == 8 || event.which == 0 )) {
            return false;
        }
    });

    $(".numbersOnly").unbind("keypress");
    $(".numbersOnly").keypress(function(event) {
        var e = String.fromCharCode(event.which);
        if ($(this).val().indexOf(".") > 0 && event.which == 46) return false;
        if (e.match('[^0-9.]') && !(event.which == 8 || event.which == 0)) {
            return false;
        }
    });

    $(".intNumbersOnly").unbind("keypress");
    $(".intNumbersOnly").keypress(function (event) {
        var e = String.fromCharCode(event.which);
        //if ($(this).val().indexOf(".") > 0 && event.which == 46) return false;
        if (e.match('[^0-9]') && !(event.which == 8 || event.which == 0)) {
            return false;
        }
    });

};
function RightClickJSFunction(id, org) {
    if (org == undefined) {
        org = 0
    }
    //alert(org);
    if( $(document.getElementById(id)).prop('tagName').toUpperCase() == "INPUT" )
       document.getElementById(id).value = org;
   else if (document.getElementById(id).length > 1) {
        document.getElementById(id).value = org;
    } //button1.click();
}

function isValidDate(date) {
    if (Object.prototype.toString.call(date) === "[object Date]") {
        if (isNaN(date.getTime()))
            return false;
        else
            return true;
    } else
        return false;
}
function isValidJSDate(input) {
    var isValid = /^(0[1-9]|[12][0-9]|3[01])[/](0[1-9]|1[012])[/](19|20)\d\d$/.test(input);
    if (isValid) {
        var dateParts = input.toString().split("/");
        var d = dateParts[0];
        var m = dateParts[1] - 1;
        var y = dateParts[2];
        if (new Date(y, m, d).getMonth() != m)
            isValid = false;
    }
    return isValid;
}
function isValidTime(input) {
    var isValid = /^([0-1]?[0-9]|2[0-3]):([0-5][0-9]):([0-5][0-9])?$/.test(input);
    return isValid
}
function RightClickReset($ele) {
    if ($ele.is("input[type=text]")) {
        $ele.val("");
    } else if ($ele.is("select")) {
        $ele[0].selectedIndex = 0;
        if ($ele.data("prevVal") != $ele.val()) {
            $ele.trigger("change");
        }
    }
}
function AddWaterMark(textbox, watermarktext) {
//    $(textbox).addClass("WaterMark").val(watermarktext);
//    $(textbox).focus(function() {
//        $(this).filter(function() {
//            return $(this).val() == "" || $(this).val() == watermarktext
//        }).removeClass("WaterMark").val("");
//    });

//    $(textbox).blur(function() {
//        $(this).filter(function() {
//            return $(this).val() == ""
//        }).addClass("WaterMark").val(watermarktext);
//    });
};

function queryString(key) {
    var val = "";
    var pairs = null;
    var pair = null;
    var urlParts = window.location.toString().split("?");

    if (urlParts.length > 1) {
        var qs = urlParts[1];
        pairs = qs.toString().split("&");
        for (var i in pairs) {
            pair = pairs[i].toString().split("=")
            if (pair.length > 0) {
                if (pair[0] == key) {
                    val = pair[1];
                    break;
                }
            }
        }
    }
    return val;
}


function formatServerDateToJSDate(ServerDate) { //ServerDate as string
    var day = ServerDate.substr(0, 2);
    var Month = ServerDate.substr(3, 2);
    var Year = ServerDate.substr(6, 4);
    jsDate = Month + "/" + day + "/" + Year;
    return jsDate;
}
function sendAjax($ajax) {
    $.ajax($ajax).done($ajax.done).fail(function(jqxhr, textStatus, errowThrown) {

    }).always(function(data_jqxhr, textStatus, jqxhr_errorThrown) {

    });
}     


function customDoPostback(eventTarget, eventArgument) {
    if (typeof __doPostBack == "function") {
        __doPostBack(eventTarget, eventArgument)

    } else {
        var theForm = document.forms['aspnetForm'];
        if (!theForm) {
            theForm = document.aspnetForm;

        }
        if (!theForm.onsubmit || (theForm.onsubmit() != false)) {
            $('<input type="hidden" name="__EVENTTARGET">').appendTo($(theForm));
            $('<input type="hidden" name="__EVENTARGUMENT">').appendTo($(theForm));
            theForm.__EVENTTARGET.value = eventTarget;
            theForm.__EVENTARGUMENT.value = eventArgument;
            theForm.submit();
        }
    }
}

/** popup object has confirm and alert modal popup windows functionality ACCESS :  popup.alert('message text') / popup.confirm('message text')
/*-- The following properties should be filled on the public scope, so that they can be used through out your code **/
/**
1. popup.shadeBG_ID = " < id of the element to disable the background, as string > " ---- default is nothing
2. popup.acceptBTN_Text = " < text to dispaly on the accepting button which means ok,yes for the next action >" --- default is OK
3. popup.rejectBTN_Text = " < text to display on the rejecting button which means cancel,no for the next action >" ---default is CANCEL
        
**/

var popup = {

    $pop: null,
    lock: false, // flag varible to lock the current process, if window is open don't open another,unless closed
    fn: "", // name or reference to the call back function,to be called when OK Or CANCEL clicked..
    fnArgs: new Array(), // array as argument to pass to the call back function...
    queue: { // stores values of the call for a new popup whn another popup window is being displayed.
        msg: new Array(), // messages
        fx: new Array(), // stores this object popup window functions - alert / confirm
        fn: new Array(), // the call back functions
        fnArgs: new Array() // array of arrays of arguments for each function in the queue..
    },
    shadeBG_ID: "SelectPic",
    acceptBTN_Text: "OK",
    middleBTN_Text: "",
    middle1BTN_Text: "",
    middle2BTN_Text: "",
    closeBTN_Text: "",
    rejectBTN_Text: "CANCEL",
    alertTitle: "INFORMATION",
    confirmTitle: "CONFIRM",
    promptTitle: "PROMPT",
    promptTxtboxName: "promptTxtboxName",
    isPrompt: false,
    createInputField: true,

    autoAssociateTextFields: true, // decides on whether/not to create corresponding hidden fields to the form for a popup containing textfields
    postBackForm: "", // The name of the form where the hidden fields should be created,names of the created hidden fields will be same as popup textfields names


    alignStyle: { /** alignment style for the popup div itself **/
        height: "auto",
        width: "450px",
        position: "absolute",
        left: parseInt($(window).width()) / 2 - parseInt("400px") / 2 + "px",
        top: parseInt($(window).height()) / 2 - 200 + "px",
        margin: "auto",
        overflow: "hidden",
        display: "none",
        background: "#FFFFFF",
        border: "1px solid #444",
        zIndex: 1200
    },


    innerContentStyle: { /** an object contains objects with style properties for each corresponding popup div inner elements **/

        header: {
            position: "relative",
            padding: "5px 2px 5px 2px",
            width: "100%",
            height: "auto",
            background: "#70A5DA"
            

        },

        body: {
            padding: "20px 0px",
            width: "auto",
            height: "auto",
            textAlign: "center",
            fontSize: "16px",
            color: "#000000"
        },

        promptTxtboxWrapper: {
            border: "1px solid #70A5DA",
            padding: "6px",
            width: "350px",
            margin: "auto",
            marginTop: "10px"
        },

        promptTxtbox: {
            borderWidth: "0px",
            outlineWidth: "0px",
            width: "350px",
            fontSize: "16px"
        },

        footer: {
            position: "relative",
            height: "30px",
            width: "100%",
            background: "#FFFFFF",
            color: "#000000"
        },

        ok: { /** styles for the ok wrapper div **/
            position: "absolute",
            height: "30px",
            width: "auto",
            padding: "2px 0px 2px 0px",
            left: "2px",
            bottom: "-5px"
        },
        middle: { /** styles for the middle wrapper div **/
            position: "absolute",
            height: "30px",
            width: "auto",
            padding: "2px 0px 2px 0px",
            left: "180px",
            bottom: "-5px"
        },

        middle1: { /** styles for the middle1 wrapper div **/
            position: "absolute",
            height: "30px",
            width: "auto",
            padding: "2px 0px 2px 0px",
            left: "120px",
            bottom: "-5px"
        },

        middle2: { /** styles for the middle2 wrapper div **/
            position: "absolute",
            height: "30px",
            width: "auto",
            padding: "2px 0px 2px 0px",
            left: "240px",
            bottom: "-5px"
        },

        cancel: { /** styles for the cancel wrapper div **/
            position: "absolute",
            height: "30px",
            width: "auto",
            padding: "2px 0px 2px 0px",
            right: "2px",
            bottom: "-5px"
        },

        closeButtonWrapper: { /** styles for the closebutton wrapper div **/
            margin: "1px 1px 1px 1px",
            right: "2px",
            position: "absolute",
            bottom: "9px",
            right: "5px"
        },

        titleWrapper: { /** styles for the title wrapper div **/
            position: "relative",
            height: "auto",
            width: "auto",
            padding: "2px 0px 2px 0px",
            left: "2px",
            bottom: "-5px",
            color: "#000000"
        }

    },

    getStyle: function() {
        try {
            /** styling the popup div itself **/
            this.$pop.css(this.alignStyle);

            /** geting the reference to the inner elements of the popup div **/
            $popHeader = this.$pop.find("div#popup-div-header");
            $popBody = this.$pop.find("div#popup-div-body");
            $popPromptTxtboxWrapper = this.$pop.find("div#popup-txtbox-wrapper");
            $popPromptTxtbox = this.$pop.find("input#promptTxtbox");
            $popFooter = this.$pop.find("div#popup-div-footer");
            $popOK = this.$pop.find("div#btnOkWrapper");
            $popMiddle = this.$pop.find("div#btnMiddleWrapper");
            $popMiddle1 = this.$pop.find("div#btnMiddle1Wrapper");
            $popMiddle2 = this.$pop.find("div#btnMiddle2Wrapper");
            $popCancel = this.$pop.find("div#btnCancelWrapper");
            $popcloseButtonWrapper = this.$pop.find("div#closeButtonWrapper");
            $poptitleWrapper = this.$pop.find("div#titleWrapper");

            /** styling the inner elemnts in the popup div **/
            $popHeader.css(this.innerContentStyle.header);
            $popBody.css(this.innerContentStyle.body);
            $popPromptTxtboxWrapper.css(this.innerContentStyle.promptTxtboxWrapper);
            $popPromptTxtbox.css(this.innerContentStyle.promptTxtbox);
            $popFooter.css(this.innerContentStyle.footer);
            $popOK.css(this.innerContentStyle.ok);
            $popMiddle.css(this.innerContentStyle.middle);
            $popMiddle1.css(this.innerContentStyle.middle1);
            $popMiddle2.css(this.innerContentStyle.middle2);
            $popCancel.css(this.innerContentStyle.cancel);
            $popcloseButtonWrapper.css(this.innerContentStyle.closeButtonWrapper);
            $poptitleWrapper.css(this.innerContentStyle.titleWrapper);

        } catch (ex) { }
    },

    createPopup: function(heading) { /** create the popup div ( jquery object ) and its inner elements **/
        try {
            this.$pop = $('<div id="popup-div">');
            html = '<div id="popup-div-header"><div id="titleWrapper"><h3>' + heading + '</h3></div><div id="closeButtonWrapper"><input name="btnClose" id="btnClose" src="Images/Erase.png" type="image" onClick="closePop();"></div></div></div>';
            html += '<div id="popup-div-body">';

            if (this.isPrompt) {
                if (this.createInputField)
                    html += '<div id="popup-txtbox-wrapper" ><input type="text" id="promptTxtbox" name="' + this.promptTxtboxName + '" /></div>';
            }

            html += '</div>';
            html += '<div id="popup-div-footer" >';
            html += '<div id="btnOkWrapper"><input type="button" id="btnOK" value="' + this.acceptBTN_Text + '" /></div>';
            html += '<div id="btnMiddleWrapper"><input type="button" id="btnMiddle" value="' + this.middleBTN_Text + '" /></div>';
            html += '<div id="btnMiddle1Wrapper"><input type="button" id="btnMiddle1" value="' + this.middle1BTN_Text + '" /></div>';
            html += '<div id="btnMiddle2Wrapper"><input type="button" id="btnMiddle2" value="' + this.middle2BTN_Text + '" /></div>';
            html += '<div id="btnCancelWrapper"><input type="button" id="btnCancel" value="' + this.rejectBTN_Text + '" /></div>';
            html += '</div>'

            this.$pop.html(html);
            this.getStyle();
            this.$pop.appendTo("body");

            //bind click events to the controls.....
            var that = this;  // for late binding, store the instance in a public variable
            this.$pop.find("input#btnOK").click(function() { that.closePopup($(this).parent().parent().parent(), "ok", false,that.acceptBTN_Text); return true; });
            this.$pop.find("input#btnMiddle").click(function () { that.closePopup($(this).parent().parent().parent(), "middle", false, that.middleBTN_Text); return true; });
            this.$pop.find("input#btnMiddle1").click(function () { that.closePopup($(this).parent().parent().parent(), "middle1", false, that.middle1BTN_Text); return true; });
            this.$pop.find("input#btnMiddle2").click(function () { that.closePopup($(this).parent().parent().parent(), "middle2", false, that.middle2BTN_Text); return true; });
            this.$pop.find("input#btnCancel").click(function () { that.closePopup($(this).parent().parent().parent(), "cancel", false, that.rejectBTN_Text); return false; })
            //this.$pop.find("input#btnClose").click(function () { that.closePopup($(this).parent().parent().parent(), "cancel", false, that.closeBTN_Text); return false; })
        } catch (ex) { }
    },

    closePopup: function($curPopup, btnSource, animate,btnSourceText) {
        try {
            if (this.isPrompt)
                this.getPromptTextValue();
            else if (btnSource == "ok")
                this.getDataFromTextField();

            if (this.autoAssociateTextFields && btnSource == "ok")//when textfields were created in the popup and their filled data have to be automatically incoporated to the form for posting
                this.associateTextFieldsDataToTheForm();


            if (!animate) { // no animation on hiding the popup
                $curPopup.remove(); // clear the popup window
                $("#" + this.shadeBG_ID).hide(); //enable the background
            } else {

                /** reduce all the styles to zero with animation and atlast destroy the popup.... */
                var that = this

                this.$pop.animate({
                    width: "0px",
                    left: parseInt(that.alignStyle.left) + parseInt(that.alignStyle.width) / 2 + "px"
                }, "slow", function() {
                    $curPopup.remove(); // clear the popup window
                    $("#" + this.shadeBG_ID).hide(); //enable the background
                });
            }


            if (typeof (window[this.fn]) == "function")
                window[this.fn](btnSource, this.fnArgs, btnSourceText); // call the call back function and pass agrg : ok or cancel.
            else if (typeof (this.fn) == "function")
                this.fn(btnSource, this.fnArgs, btnSourceText);    // call the call back function and pass agrg : ok or cancel.


            // reset the lock,the popup window object and read the queue..
            this.lock = false;
            this.$pop = null;
            this.loadQueue(); //check if there are popup window function calls in the queue and process...

        } catch (ex) { }
    },

    getPromptTextValue: function() {//get the value of the textbox and push it to the argument array as last element of the array,only when one text field is created in the popup.PROMPT functionality
        if (typeof (this.fnArgs) == "object" || typeof (this.fnArgs) == "array")
            this.fnArgs.push(this.$pop.find("input#promptTxtbox").val());
    },

    getDataFromTextField: function() {//get the data from text fields and write them back to the data object in argument array, to be accessed later,
        // when a number of text fields where created in the popup and had to be written to the args array to be sent...
        ///back to the call back function.
        var txtName = "";
        var that = this;
        if (typeof (this.fnArgs) == "object" || typeof (this.fnArgs) == "array") {
            $("#popup-div-body").find("input").each(function() {
                if ($(this).attr("type") == "text") {
                    txtName = $(this).attr("name");
                    for (i = 0; i < that.fnArgs.length; i++) {

                        if (typeof (that.fnArgs[i]) == "object") {

                            for (x in that.fnArgs[i]) {

                                if (x == txtName) {
                                    that.fnArgs[i][x] = $(this).val();
                                }
                            }
                        }
                    }
                }

            })
        }
    },

    associateTextFieldsDataToTheForm: function() { // creates hidden fields in the desired form with the input values
        var $hf = null;
        var that = this;
        $("#popup-div-body").find("input").each(function() {
            if ($(this).attr("type") == "text") {
                $hf = $('<input type="hidden" name="' + $(this).attr("name") + '" value="' + $(this).val() + '" />');
                $form = null;
                if (typeof (that.postBackForm) == "string" && that.postBackForm.length > 0)
                    $form = $("#" + that.postBackForm).length > 0 ? $("#" + that.postBackForm) : $("." + that.postBackForm).length > 0 ? $("." + that.postBackForm) : null;
                else if (typeof (that.postBackForm) == "object")
                    $form = that.postBackForm
                else {
                    if ($("form").length > 0)
                        $form = $("form").eq(0);
                }

                if ($form != null)
                    $form.prepend($hf);

            }

        });
    },

    loadQueue: function() {
        try {
            var n = this.queue.fx.length;
            if (n > 0) {
                this[this.queue.fx.shift()](this.queue.msg.shift(), this.queue.fn.shift(), this.queue.fnArgs.shift());  // calls the very first function in the queue...
            }
        } catch (ex) { }
    },

    display: function(animate) {
        try {
            $("#" + this.shadeBG_ID).find("img,button,input,div").remove(); // shading element should not contain child elements,clear everything off.

            if (this.$pop.find("input[type=text]").length > 0)
                this.$pop.find("input[type=text]").eq(0).focus();  // give focus to the very first input textbox if any exists

            if (!animate) { // no animation on displaying popup window..
                $("#" + this.shadeBG_ID).show(); //disable the background
                this.$pop.show();
                return;
            }

            /** reset the styles a bit to zero and animate to the original style **/
            this.$pop.css("width", "0px");
            this.$pop.css("left", parseInt(this.alignStyle.left) + parseInt(this.alignStyle.width) / 2 + "px");
            this.$pop.show();

            /** show the popup slowly with animation **/
            var that = this;  // for late binding, store the instance in a public variable
            $("#" + this.shadeBG_ID).show();
            this.$pop.animate({
                width: that.alignStyle.width,
                left: parseInt(that.alignStyle.left) - parseInt(that.alignStyle.width) / 2 + "px"
            }, "slow");
        } catch (ex) { }

    },

    confirm: function(msg, fn, fnArgs, Isqueued, showMiddleBtn, twoMiddleButtons) { /** this will popup a confirmation box, in order to allow a certain process **/
        try {
            if (typeof (Isqueued) != "undefined") {
                if (!Isqueued) {
                    if ($("#popup-div").length > 0) return;
                }
            }
            
            if (this.lock) {
                this.queue.fx.push("confirm");
                this.queue.msg.push(msg);
                this.queue.fn.push(fn);
                this.queue.fnArgs.push(fnArgs);
                return;
            }

            this.createPopup(this.confirmTitle);
            this.$pop.find("div#btnMiddleWrapper").hide(); // hides out the middle button part.
            this.$pop.find("div#btnMiddle1Wrapper").hide(); // hides out the middle button part.
            this.$pop.find("div#btnMiddle2Wrapper").hide(); // hides out the middle button part.
            if (typeof (showMiddleBtn) != "undefined") {
                if (showMiddleBtn) {
                    this.$pop.find("div#btnMiddleWrapper").show(); // hides out the middle button part.
                }
            } 

            if (typeof (twoMiddleButtons) != "undefined") {
                if (twoMiddleButtons) {
                    this.$pop.find("div#btnMiddle1Wrapper").show(); // shows out the middle button1 part.
                    this.$pop.find("div#btnMiddle2Wrapper").show(); // shows out the middle button2 part.
                }
            } 

            this.$pop.find("div#popup-div-body").prepend(msg);
            this.fn = fn;
            this.fnArgs = fnArgs;
            this.display(false);
            this.lock = true;
        } catch (ex) { }
    },

    alert: function(msg, fn, fnArgs, Isqueued) {  /** this will popup an alert box as a result of a certain process **/
        try {
            if (typeof (Isqueued) != "undefined") {
                if (!Isqueued) {
                    if ($("#popup-div").length > 0) return;
                }
            }
            if (this.lock) {
                this.queue.fx.push("alert");
                this.queue.msg.push(msg);
                this.queue.fn.push(fn);
                this.queue.fnArgs.push(fnArgs);
                return;
            }
            this.createPopup(this.alertTitle);
            this.$pop.find("div#popup-div-body").prepend(msg);
            this.$pop.find("div#btnCancelWrapper").hide(); // hides out the cancel button part.
            this.$pop.find("div#btnMiddleWrapper").hide(); // hides out the middle button part.
            this.$pop.find("div#btnMiddle1Wrapper").hide(); // shows out the middle button1 part.
            this.$pop.find("div#btnMiddle2Wrapper").hide(); // shows out the middle button2 part.
            this.fn = fn;
            this.fnArgs = fnArgs;
            this.display(false);
            this.lock = true;
        } catch (ex) { }
    },

    prompt: function(msg, fn, fnArgs, createInputField, Isqueued) {  /** this will popup a prompt box as a result of a certain process for user to input something, IT IS ONLY FOR SINGLE TEXT BOX FUNCTIONALITY **/
        try {
            this.isPrompt = true;

            if (typeof (createInputField) != "undefined") {
                this.createInputField = createInputField;
            }

            if (typeof (Isqueued) != "undefined") {
                if (!Isqueued) {
                    if ($("#popup-div").length > 0) return;
                }
            }
            if (this.lock) {
                this.queue.fx.push("prompt");
                this.queue.msg.push(msg);
                this.queue.fn.push(fn);
                this.queue.fnArgs.push(fnArgs);
                return;
            }
            this.createPopup(this.promptTitle);
            this.$pop.find("div#btnMiddleWrapper").hide(); // hides out the middle button part.
            this.$pop.find("div#btnMiddle1Wrapper").hide(); // shows out the middle button1 part.
            this.$pop.find("div#btnMiddle2Wrapper").hide(); // shows out the middle button2 part.
            this.$pop.find("div#popup-div-body").prepend(msg);
            this.fn = fn;
            this.fnArgs = fnArgs;
            this.display(false);
            this.lock = true;
        } catch (ex) { }
    }
}

/************  USAGE OF OBJECT  'POPUP'  FOR ALERT AND CONFIRM POPUP WINDOW FUNCTIONALIY *******
 
You access either alert / confirm popup window by
example 1:  popup.alert( <message to display>,<a call back function name as string with two arguments><array of arguments to pass to callback function> )
example 2:  popup.alert( <message to display>,<a call back function definition with two arguments><array of arguments to pass to callback function> )
--- a call back function will be after button ok / button cancel is clicked and....
---- first argument passed can be used to know which button of popup window was pressed and decide the action....
---- second argument passed is an array of a number of elements of your choice.
 
*/


var format = {

    delimeter: "",

    numberWithCommas: function(num) {
        try {
            var intPart = "";
            var decPart = "";
            var numParts = "";
            var formatedNum = "";
            var commaPosCounter = 1;
            this.delimeter = ",";

            if (typeof (num) == "number" || typeof (num) == "string")
                num = num.toString();
            else
                throw ("Invalid Number Format");

            if ($.trim(num) == "")
                return 0;

            numParts = $.trim(num).replace(/[^0-9^\.^\-]/ig, "").split(".");
            if (numParts.length == 1)
                intPart = numParts[0];
            else if (numParts.length == 2) {
                intPart = numParts[0];
                decPart = numParts[1];
            } else
                throw ("Invalid Number Format " + num);

            var digits = intPart.split("");

            for (i = digits.length; i > 0; i--) {

                formatedNum = digits[i - 1] + formatedNum;

                if ((commaPosCounter % 3 == 0) && commaPosCounter < digits.length) {
                    if ((i - 1) == 1) {
                        if (!isNaN(digits[0]))
                            formatedNum = this.delimeter + formatedNum;
                    } else
                        formatedNum = this.delimeter + formatedNum;
                }

                commaPosCounter++;
            }
            if (decPart.length > 0)
                formatedNum += "." + decPart;

            return formatedNum;
        } catch (ex) { if (typeof (popup) == "object") popup.alert("Error: " + ex); else alert("Error: " + ex); }
    },

    numberWithoutCommas: function(num) {
        try {
            var formatedNum = "";

            if (typeof (num) == "number" || typeof (num) == "string")
                num = num.toString();
            else
                throw ("Invalid Number Format");

            if ($.trim(num) == "")
                return 0;

            formatedNum = $.trim(num).replace(/,/ig, "").replace(/[^0-9^\.]/ig, "");

            return formatedNum;

        } catch (ex) { if (typeof (popup) == "object") popup.alert("Error: " + ex); else alert("Error: " + ex); }
    }

}


function NoImage(obj){
        $(obj).attr('src', 'Images/noimage.jpg');
}

function HighlightMandatoryFields() {
    try{
        if (Page_Validators != undefined && Page_Validators != null) {
            for (i = 0; i < Page_Validators.length; i++) {
                
                var ControlName = Page_Validators[i].controltovalidate;
                var Enabled = Page_Validators[i].enabled;
                var isRequiredFieldValidator = Page_Validators[i].evaluationfunction.name == "RequiredFieldValidatorEvaluateIsValid";

            
                if (Enabled != false && isRequiredFieldValidator == true){
                    $('#' + ControlName).addClass('requiedField');
                }
            }
        }
    } catch (ex) {
    }
}

function closePop()
{
    $("#btnCancel").click();
}