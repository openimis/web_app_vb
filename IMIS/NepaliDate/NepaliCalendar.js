function nepaliCalendarPopup() {
    $(".dateCheck").each(function () {
        var width = $(this).width();
        var html = $(this).parent();
        var customDateField = $('<input type="text" class="customDatePicker" style="width: ' + width + 'px;" placeholder="Nepali Date" />');
        customDateField.prependTo(html);
        customDateField.nepaliDatePicker({
            dateFormat: "%d/%m/%y",
            closeOnDateSelect: true
        });
        var adDate;
        $(this).change(function () {
            adDate = $(this).val();
            adDate = adDate.split("/");
            var today = calendarFunctions.getBsDateByAdDate(parseInt(adDate[2]), parseInt(adDate[1]), parseInt(adDate[0]));
            today = today.bsDate + '/' + today.bsMonth + '/' + today.bsYear;
            $(this).prev('input').val(today);
            $(this).prev('input').trigger("change");
            $(this).prev('input').trigger("blur");
        });
        if ($(this).val() != "") {
            adDate = new Date($(this).val());
            var customDate = calendarFunctions.getBsDateByAdDate(adDate.getFullYear(), adDate.getMonth() + 1, adDate.getDate());
            customDate = calendarFunctions.bsDateFormat("%d/%m/%y", customDate.bsYear, customDate.bsMonth, customDate.bsDate);
            customDateField.val(customDate);
        }

        customDateField.on("dateSelect", function (event) {
            var today = calendarFunctions.formattedAd(event.datePickerData.adDate);
            customDateField.next('input').val(today);
            customDateField.next('input').trigger("change");
            customDateField.next('input').trigger("blur");
        });
    });
}
