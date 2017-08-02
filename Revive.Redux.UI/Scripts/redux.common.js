$(document).ready(function () {

    // theasciicode.com.ar

    $(".numericOnly").on("keypress keyup blur", function (e) {
        //$(this).val($(this).val().replace(/[^\d].+/, ""));
        var regex = /[^\d].+/;
        if (regex.test($(this).val())) {
            $(this).val($(this).val().replace(regex, ''));
        }
        if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) {
            return false;
        }
    });

    $(".decimalOnly").on("keypress keyup blur", function (e) {
        //$(this).val($(this).val().replace(/[^0-9\.]/g, ''));
        var regex = /[^0-9\.]/g;
        if (regex.test($(this).val())) {
            $(this).val($(this).val().replace(regex, ''));
        }
        if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57) && e.which != 46) {
            return false;
        }
    });

    $(".alphaNumericOnly").on("keypress keyup blur", function (e) {
        var initVal = $(this).val();
        outputVal = initVal.replace(/[^0-9a-zA-Z]/g, "");
        if (initVal != outputVal) {
            $(this).val(outputVal);
        }
    });



});
$(function () {
    $('.Kendogrid div:nth-child(2)').css("height", "auto");

    $('.Kendogrid div:nth-child(2)').css("max-height", "150px");


});

// Namespace
var Revive = Revive || {};
Revive.Redux = Revive.Redux || {};

Revive.Redux.Common = {
    SetPhoneFormat: function (txtboxId) {
        $('#' + txtboxId).focus(function () {
            if ($('#' + txtboxId).val() == "" || $('#' + txtboxId).val().length == 0) {
                $(".abc").css("display", "none");
            }
            else {
                $(".abc").css("display", "block");
            }
        });
        $('#' + txtboxId).focusout(function () {
            if ($('#' + txtboxId).val() == "" || $('#' + txtboxId).val() == "(___) ___-____") {
                $(".abc").css("display", "none");
            }
            else {
                $(".abc").css("display", "block");
            }
        });

    },

    SetAutoCompleteOff: function () {
        $("input[type='text']").each(function () {
            $(this).attr("autocomplete", "off");
        });
    },
    SetKendoGridHeight: function () {
        $('.Kendogrid div:nth-child(2)').css("height", "auto");
        $('.Kendogrid div:nth-child(2)').css("max-height", "300px");
    },
    SetParentMenuSelected: function (menuId) {
        // add selected class to the menu id.
        $('div.navbar-collapse ul.navbar-nav li#' + menuId).addClass('active');
        // get all menus which are parent and has active class.
        var activeParents = $("div.navbar-collapse ul.navbar-nav li[isparent*='yes'][class*='active']");
        if (activeParents.length > 0) {
            // traverse all parents.
            activeParents.each(function () {
                // if it is not the same parent as sent from source, remove the active class.
                if (activeParents.attr('id') != menuId)
                    $(this).removeClass('active');
            });
        }
    },
    ExportToExcel: function (gridInstance, gridTitle, createExcelUrl, downloadExcelUrl, report_Id) {
        var data = {
            model: JSON.stringify(gridInstance.columns),
            data: JSON.stringify(gridInstance.dataSource.data().toJSON()),
            title: gridTitle,
            ReportId: report_Id
        };       
        // Create the spreadsheet.
        $.post(createExcelUrl, data, function () {
            // Download the spreadsheet.
            window.location.replace(kendo.format("{0}?title={1}",
                downloadExcelUrl,
                gridTitle));

        });
    }
};