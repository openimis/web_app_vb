function nepaliCalendarPopup() {
    //var pagePathName= window.location.pathname;
    //pagePathName.substring(pagePathName.lastIndexOf("/") + 1);
    //alert(pagePathName);
    //alert(getFileName());
    if (getFileName() != 'Reports.aspx') {
        /* magic ... */

        var converter = new DateConverter();
        var adDate;
        var adDateNew;
        var npDates;
        var npDatesNew;
        // for type input have .dateCheck as class
        $(".dateCheck, #Body_txtEnrollmentDate, #Body_txtPaymentDate, #Body_txtSTARTData, #Body_txtENDData, #Body_txtBirthDate").each(function () {
            var width = $(this).width();
            var html = $(this).parent();
            //var customDateField = $('<input type="text" class="customDatePicker" style="width: ' + width + 'px;" placeholder="Nepali Date(dd/mm/yyyy)" />');
            var customDateField = $('<input type="text" class="customDatePicker" style="width: 80px;" placeholder="Nepali Date" />');
            customDateField.prependTo(html);
            customDateField.calendarsPicker({
                calendar: $.calendars.instance('nepali'),
                yearRange: '-100:+0',
                duration: "fast",
                showAnim: "",
                dateFormat: 'dd/mm/yyyy',
                firstDay: 0,
                onSelect: function (npDate) {
                    converter.setNepaliDate(npDate[0]._year, npDate[0]._month, npDate[0]._day);
                    customDateField.next('input').val(converter.toEnglishString());
                }
            });
            $(".customDatePicker").blur(function () {
                npDates = $(this).val();
                if (isValidDateCalendar(npDates)) {
                    npDatesNew = npDates.split("/");
                    converter.setNepaliDate(parseInt(npDatesNew[2]), parseInt(npDatesNew[1]), parseInt(npDatesNew[0]));
                    customDateField.next('input').val(converter.toEnglishString());
                    customDateField.next('input').change();
                }
                else {
                    //alert("Invalid Date");
                }
            });
            $(this).change(function () {
                adDate = $(this).val();
                if (adDate != "") {
                    adDateNew = adDate.split("/");
                    converter.setEnglishDate(parseInt(adDateNew[2]), parseInt(adDateNew[1]), parseInt(adDateNew[0]));
                    $(this).prev('input').val(converter.toNepaliString());
                }
                else {
                    $(this).prev('input').val('');
                }
            });
            if ($(this).val() != "") {
                adDate = $(this).val();
                adDate = adDate.split("/");
                converter.setEnglishDate(parseInt(adDate[2]), parseInt(adDate[1]), parseInt(adDate[0]));
                customDateField.val(converter.toNepaliString());
            }
        });
        //  for all span with Date Text

        $("span").each(function () {
            if (isValidDateCalendar($(this).text())) {
                adDate = $(this).text();
                adDateNew = adDate.split("/");
                converter.setEnglishDate(parseInt(adDateNew[2]), parseInt(adDateNew[1]), parseInt(adDateNew[0]));
                $(this).text('BS:' + converter.toNepaliString() + ' AD: ' + adDate + '');

            }
        });
    }
}

function isValidDateCalendar(s) {
    var bits = s.split('/');
    var d = new Date(bits[2] + '/' + bits[1] + '/' + bits[0]);
    return !!(d && (d.getMonth() + 1) == bits[1] && d.getDate() == Number(bits[0]));
}
function getFileName() {
    //this gets the full url
    var url = document.location.href;
    //this removes the anchor at the end, if there is one
    url = url.substring(0, (url.indexOf("#") == -1) ? url.length : url.indexOf("#"));
    //this removes the query after the file name, if there is one
    url = url.substring(0, (url.indexOf("?") == -1) ? url.length : url.indexOf("?"));
    //this removes everything before the last slash in the path
    url = url.substring(url.lastIndexOf("/") + 1, url.length);
    //return
    return url;
}
