function IsInteger(s) {
    var i;
    for (i = 0; i < s.length; i++) {
        // Check that current character is number.
        var c = s.charAt(i);
        if (((c < "0") || (c > "9"))) return false;
    }
    // All characters are numbers.
    return true;
}
//param1;Control ID
//param2: control type
//param3: control tag name
//param4: error message
//param4: call back function
function ValidateControl() {
    if (arguments.length < 3)
        return;
    var controlId = arguments[0];
    var controlType = arguments[1];
    var controlTagName = arguments[2];
    var errorMessage = arguments[3];
    var callbackfunction = arguments[4];
    var objControl = GetControlObject(controlId, controlTagName);
    if (objControl == null)//|| !objControl.is(':visible')
        return false;

    if (controlType == 'text') {
        if ($.trim(objControl.val()).length <= 0) {
            $.alert({ content: errorMessage, confirm: function () { } });
            objControl.focus();
            return false;
        }
    }
    else if (controlType == 'select') {
        if (objControl.val() == null || objControl.val() == "0") {
            $.alert({ content: errorMessage, confirm: function () { } });
            objControl.focus()
            return false;
        }
    }
    else if (controlType == 'checkbox') {
        if (!objControl.get(0).checked) {
            $.alert({ content: errorMessage, confirm: function () { } });
            objControl.focus()
            return false;
        }
    }
    else if (controlType == 'radio') {
        if ($("input[name *=" + controlId + "]:checked").length <= 0) {
            $.alert({ content: errorMessage, confirm: function () { } });
            objControl.focus()
            return false;
        }
    }
    else if (controlType == 'textarea') {
        if ($.trim(objControl.val()).length <= 0) {
            $.alert({ content: errorMessage, confirm: function () { } });
            objControl.focus();
            return false;
        }
    }
    else if (controlType == 'checkboxlist') {
        if (objControl.find('input:checked').length <= 0) {
            $.alert({ content: errorMessage, confirm: function () { } });
            objControl.focus();
            return false;
        }
    }
    else if (controlType == 'peoplepicker') {
        if (objControl.find('span').length <= 0) {
            $.alert({ content: errorMessage, confirm: function () { } });
            objControl.focus()
            return false;
        }

    }
    if (callbackfunction != null) {
        var arrayParams = new Array();
        for (i = 5; i < arguments.length; i++) {
            arrayParams.push(arguments[i]);
        }
        if (!callbackfunction(arrayParams))
            return false;
    }
    return true;
}

function GetControlObject(id, type) {
    var objControl = $(type + '[ID*="' + id + '"]');
    if (objControl.length > 0) {
        return objControl;
    }
    else {
        return null;
    }
}

function ValidateNoofPosition() {
    var objtxtbx = GetControlObject('txtNoPositions', 'input');

    if (!IsInteger(objtxtbx.val())) {
        $.alert({ content: 'No. of Positions must be integer and greater than 0', confirm: function () { } });
        objtxtbx.focus();
        return false;
    }
    if (objtxtbx.val() <= 0) {
        $.alert({ content: 'No. of Positions must be integer and greater than 0', confirm: function () { } });
        objtxtbx.focus();
        return false;
    }
    return true;
}

function RangeValidation() {
    var arrayParams = arguments[0];
    if (arrayParams == null || arrayParams.length < 1)
        return false;
    var controlId = arrayParams[0];
    var minValue = arrayParams[1];
    var maxValue = arrayParams[2];
    var msg1 = arrayParams[3];
    var msg2 = arrayParams[4];
    var objtxtbx = GetControlObject(controlId, 'input');

    if (!IsInteger(objtxtbx.val())) {
        $.alert({ content: msg1, confirm: function () { } });
        objtxtbx.focus();
        return false;
    }
    if (objtxtbx.val() < minValue || objtxtbx.val() > maxValue) {
        $.alert({ content: msg2, confirm: function () { } });
        objtxtbx.focus();
        return false;
    }
    return true;
}
//startDateId,
//endDateId
function validateDateRange() {
    var arrayParams = arguments[0];
    if (arrayParams == null || arrayParams.length < 1)
        return false;
    var startDateId = arrayParams[0];
    var endDateId = arrayParams[1];
    var msg = arrayParams[2];
    var objStartDateControl = GetControlObject(startDateId, 'input');
    var objEndDateControl = GetControlObject(endDateId, 'input');
    if (!isValidDate(objStartDateControl.val()))
        return false;
    if (!isValidDate(objEndDateControl.val()))
        return false;
    var format = 'mm/dd/yy';
    var minDate = new Date(2010, 1, 1, 0, 0, 0, 0);
    var maxDate = new Date(2020, 1, 1, 0, 0, 0, 0);
    var startDate = $.datepicker.parseDate(format, objStartDateControl.val(), null);
    var endDate = $.datepicker.parseDate(format, objEndDateControl.val(), null);
    var today = new Date();
    var param = getParameterByName('Op');
    if (param != null) {
        param = param.toLowerCase();
    }
    else {
        param = getParameterByName('formOp');
        if (param != null) {
            param = param.toLowerCase();
        }
    }
    if (param == "new") {
        if (startDate < today) {
            objStartDateControl.focus();
            $.alert({ content: 'Please enter date greater than today\'s date!', confirm: function () { } });
            return false;
        }
        if (endDate < today) {
            objEndDateControl.focus();
            $.alert({ content: 'Please enter date greater than today\'s date!', confirm: function () { } });
            return false;
        }
    }
    if (startDate < minDate || startDate > maxDate) {
        objStartDateControl.focus();
        $.alert({ content: 'Start Date is out of range.', confirm: function () { } });
        return false;
    }
    if (endDate < minDate || endDate > maxDate) {
        objEndDateControl.focus();
        $.alert({ content: 'End Date is out of range.', confirm: function () { } });
        return false;
    }
    if (startDate >= endDate) {
        objStartDateControl.focus();
        $.alert({ content: msg, confirm: function () { } });
        return false;
    }
    return true;
}

function RegExValidator() {
    var arrayParams = arguments[0];
    var id = arrayParams[0];
    var regexp = arrayParams[1];
    var msg = arrayParams[2];
    var objtxtbx = GetControlObject(id, 'input');
    if (!objtxtbx.val().match(regexp)) {
        $.alert({ content: msg, confirm: function () { } });
        objtxtbx.focus();
        return false;
    }
}
function isValidDate(val) {
    var msg = "Please enter valid date in mm/dd/yyyy format.";
    var format = 'mm/dd/yy'
    var regexp = /^\d{1,2}\/\d{1,2}\/\d{4}$/;
    if (!regexp.test(val)) {
        $.alert({ content: msg, confirm: function () { } });
        return false;
    }
    else {
        try {
            $.datepicker.parseDate(format, val);
            return true;
        }
        catch (Error) {
            $.alert({ content: msg, confirm: function () { } });
            return false;
        }
    }
}

function OnReqCancelClick() {
    var redirectUrl = getParameterByName('source');
    var url = "/HR/SitePages/Requirements.aspx";
    if (redirectUrl != null && redirectUrl != "") {
        redirectUrl = redirectUrl.replace('~', '');
        window.location.href = redirectUrl;
    }
    else {
        window.location.href = url;
    }
}
function RedirectToPage(url) {
    window.location.href = url;
}
function getParameterByName(name) {
    name = name.replace(/[\[]/, "\\\[").replace(/[\]]/, "\\\]");
    var regexS = "[\\?&]" + name + "=([^&#]*)";
    var regex = new RegExp(regexS);
    var results = regex.exec(window.location.href);
    if (results == null)
        return "";
    else
        return decodeURIComponent(results[1].replace(/\+/g, " "));
}
function GetDiffBetweenTwoDates(fromTxtBxId, ToTxtBxId) {

    var fromTxtBx = GetControlObject(fromTxtBxId, 'input');
    var ToTxtBx = GetControlObject(ToTxtBxId, 'input');
    var format = 'mm/dd/yy';
    var d1 = $.datepicker.parseDate(format, fromTxtBx.val(), null);
    var d2 = $.datepicker.parseDate(format, ToTxtBx.val(), null);
    var diff = 0;
    if (d1 && d2) {
        diff = Math.floor((d2.getTime() - d1.getTime()) / 86400000); // ms per day
        return (diff / 30).toFixed(2);
    }
    return diff;
}
function SetDuration(durationTxtBxId, fromTxtBxId, ToTxtBxId) {
    var duration = GetDiffBetweenTwoDates(fromTxtBxId, ToTxtBxId);
    var durationTxtBx = GetControlObject(durationTxtBxId, 'input');
    durationTxtBx.val(duration);
}

function RangeValidationDouble() {
    var arrayParams = arguments[0];
    if (arrayParams == null || arrayParams.length < 1)
        return false;
    var controlId = arrayParams[0];
    var minValue = arrayParams[1];
    var maxValue = arrayParams[2];
    var msg1 = arrayParams[3];
    var msg2 = arrayParams[4];
    var objtxtbx = GetControlObject(controlId, 'input');

    if (isNaN(objtxtbx.val())) {
        $.alert({ content: msg1, confirm: function () { } });
        return false;
    }
    if (objtxtbx.val() < minValue || objtxtbx.val() > maxValue) {
        $.alert({ content: msg2, confirm: function () { } });
        return false;
    }
    return true;
}

function CalculateEstimatedAmount() {

    if (!ValidateControl('txtAmount', 'text', 'input', 'Please Enter Pay Rate.', RangeValidationDouble, 'txtAmount', 0, 99999999999, "'Pay rate' must be numeric and positive.", "'Pay rate' must be numeric, positive and less than 99999999999"))
        return false;
    if (!ValidateControl('ddlPayRate', 'select', 'select', 'Please Select Pay type.'))
        return false;
    if (!ValidateControl('txtDuration', 'text', 'input', 'Please Enter Duration.', RangeValidationDouble, 'txtDuration', 0, 999, "'Duration' must be numeric and positive.", "'Duration' must be numeric, positive and less than 999"))
        return false;



    var txtDuration = GetControlObject('txtDuration', 'input');
    var txtAmount = GetControlObject('txtAmount', 'input');
    var txtPayRateHoursAndDaysPerWeek = GetControlObject('txtPayRateHoursAndDaysPerWeek', 'input');
    var txtTotalAmount = GetControlObject('txtTotalAmount', 'input');
    var ddlPayRate = GetControlObject('ddlPayRate', 'select');

    var months = txtDuration.val();
    var weeks = months * 4 + (months / 6 * 2);
    var selectedText = ddlPayRate.find('option:selected').text();
    var selectedText = selectedText.replace(/^\s+/, "");  //Triming Left Space
    var selectedText = selectedText.replace(/\s+$/, "");  //Triming Right Space
    var calculatedVal = null;
    if (selectedText.toLowerCase() == 'per hour' || selectedText.toLowerCase() == 'per day') {
        calculatedVal = txtPayRateHoursAndDaysPerWeek.val() * txtAmount.val() * weeks;
    }
    else if (selectedText.toLowerCase() == 'per month') {
        calculatedVal = txtAmount.val() * months;
    }
    else if (selectedText.toLowerCase() == 'per year') {
        calculatedVal = txtPayRateHoursAndDaysPerWeek.val() * txtAmount.val();
    }
    txtTotalAmount.val("$" + calculatedVal.toFixed(2));
}

function AttachMandatoryFieldEvntHndlr(classname) {

    $("." + classname).each(function (index) {

        var strtagName = $(this).prop("tagName").toLowerCase();
        if (strtagName == "input") {
            OnMandatoryFieldOnChange($(this), classname);
            $(this).unbind("blur");
            $(this).blur(function () {
                OnMandatoryFieldOnChange($(this), classname)
            });
        }
        else if (strtagName == "textarea") {
            OnMandatoryFieldOnChange($(this), classname);
            $(this).unbind("change");
            $(this).change(function () {
                OnMandatoryFieldOnChange($(this), classname)
            });
        }
        else if (strtagName == "select") {
            OnMandatoryFieldOnChange($(this), classname);
            $(this).unbind("change");
            $(this).change(function () {
                OnMandatoryFieldOnChange($(this), classname)
            });
        }
        else if (strtagName == "table") {

            if ($(this).is("id*='OuterTable'")) {
                ValidatePeoplePicker($(this), classname);
                $(this).unbind("change");
                $(this).change(function () {
                    ValidatePeoplePicker($(this), classname)
                });
            }
            else if ($(this).find("input[type='checkbox'").length > 0) { //checkbox listbox
                ValidateListControl($(this), classname);
                $(this).unbind("change");
                $(this).change(function () {
                    ValidateListControl($(this), classname);
                });
            }
            else if ($(this).find("input[type='radio'").length > 0) {//radio button listbox
                ValidateListControl($(this), classname);
                $(this).unbind("change");
                $(this).change(function () {
                    ValidateListControl($(this), classname);
                });
            }
        }
    });
}
function OnMandatoryFieldOnChange(control, classname) {

    var strtagName = control.prop("tagName").toLowerCase();
    var strType = control.prop("type").toLowerCase();
    if (strtagName == "input") {
        if (strType == 'text') {
            if ($.trim(control.val()).length <= 0) {
                AddValidAttribute(control, classname, false);
            }
            else {
                AddValidAttribute(control, classname, true);
            }
        }
        else if (strType == 'checkbox') {
            if (!control.get(0).checked) {
                AddValidAttribute(control, classname, false);
            }
            else {
                AddValidAttribute(control, classname, true);
            }
        }
        else if (strType == 'radio') {
            if (!control.is(":checked")) {
                AddValidAttribute(control, classname, false);
            } else {
                AddValidAttribute(control, classname, true);
            }
        }

    }
    else if (strtagName == 'textarea') {
        if ($.trim(control.val()).length <= 0) {
            AddValidAttribute(control, classname, false);
        } else {
            AddValidAttribute(control, classname, true);
        }
    }
    else if (strtagName == 'select') {
        if (control.val() == null || control.val() == "0") {
            AddValidAttribute(control, classname, false);
        } else {
            AddValidAttribute(control, classname, true);
        }
    }
    EnableDisableSaveButton(classname);
}

function ValidateListControl(control, classname) {
    if (control.find('input:checked').length <= 0) {
        AddValidAttribute(control, classname, false);
    } else {
        AddValidAttribute(control, classname, true);
    }
    EnableDisableSaveButton(classname);
}
function ValidatePeoplePicker(control, classname) {
    if (control.find("div[id *= 'upLevelDiv'] span").length <= 0) {
        AddValidAttribute(control, classname, false);
    } else {
        AddValidAttribute(control, classname, true);
    }
    EnableDisableSaveButton(classname);
}

function DisableSaveButton(id) {
    var objSaveBtn = GetControlObject(id, 'input');
    if (objSaveBtn != null) {
        objSaveBtn.attr("disabled", "disabled");
    }
}
function EnableSaveButton(id) {
    var objSaveBtn = GetControlObject(id, 'input');
    if (objSaveBtn != null)
        objSaveBtn.removeAttr('disabled');
}
function AddValidAttribute(control, mandatorygroup, valid) {
    if (control != null) {
        if (valid) {
            control.attr(mandatorygroup, "true");
        }
        else {
            control.attr(mandatorygroup, "false");
        }
    }
}
function EnableDisableSaveButton(mandatorygroup) {
    if ($("table[class*='" + mandatorygroup + "'][" + mandatorygroup + "='false']").length <= 0 && $("select[class*='" + mandatorygroup + "'][" + mandatorygroup + "='false']").length <= 0 && $("input[class*='" + mandatorygroup + "'][" + mandatorygroup + "='false']").length <= 0 && $("textarea[class*='" + mandatorygroup + "'][" + mandatorygroup + "='false']").length <= 0) {
        EnableSaveButton('btnSaveConsulting');
        if (GetSelectedReqCategory().toLowerCase() == "solutions") {
            EnableSaveButton('btnSubmitApproval');
            EnableSaveButton('btnSalesOrder');
        }
    }
    else {
        DisableSaveButton('btnSaveConsulting');
        if (GetSelectedReqCategory().toLowerCase() == "solutions") {
            DisableSaveButton('btnSubmitApproval');
            DisableSaveButton('btnSalesOrder');
        }
    }
}

function GetSelectedReqCategory() {
    var selectedReqCategory = $('table[ID *="rdobtnlstReqCategory"] input:checked').parent().find("label").html();
    return selectedReqCategory;
}
function ValidateSoultionReqCriteria(index) {

    if (index === 2 || index === 3 || index === 4 || index === 5 || index === 6)//Validate primary div
    {
        // id,type,tagname, msg
        if (!ValidateControl('txtTitle', 'text', 'input', 'Please Enter Opportunity Name.'))
            return false;
        if (!ValidateControl('txtStartDate', 'text', 'input', 'Please Enter Start date.'))
            return false;
        if (!ValidateControl('txtEndDate', 'text', 'input', 'Please Select End date.', validateDateRange, 'txtStartDate', 'txtEndDate', "Start Date' shouldn't be greater than 'Opportunity End Date'"))
            return false;
        if (!ValidateControl('ddlEndClient', 'select', 'select', 'Please Select End client.'))
            return false;
        if (!ValidateControl('txtCloseDate', 'text', 'input', 'Please Select Close date.', validateDateRange, 'txtCloseDate', 'txtStartDate', "'Close Date' must be placed before 'Opportunity Start Date' and 'Opportunity End Date'"))
            return false;
    }
    if (index === 3 || index === 4 || index === 5 || index === 6) {

        var arrayParams = new Array();
        arrayParams.push('txtStagePercent');
        arrayParams.push(0);
        arrayParams.push(100);
        arrayParams.push("'Opportunity Stage' must be numeric and positive.");
        arrayParams.push("'Opportunity Stage' must between 0 to 100");
        if (!RangeValidation(arrayParams))
            return false;
    }
    if (index === 4 || index === 5 || index === 6) {
    }
    if (index === 5 || index === 6) {
        if (!ValidateControl('ddlState', 'select', 'select', 'Please Select State.'))
            return false;
    }
    return true;
}

function ValidateConsultingReqCriteria(index) {
    if (index === 2 || index === 3 || index === 4 || index === 5 || index === 6)//Validate primary div
    {
        // id,type,tagname, msg
        if (!ValidateControl('txtTitle', 'text', 'input', 'Please Enter Requirement Name.'))
            return false;
        if (!ValidateControl('txtNoPositions', 'text', 'input', 'Please Enter No. Positions.', ValidateNoofPosition))
            return false;
        if (!ValidateControl('txtStartDate', 'text', 'input', 'Please Enter Start date.'))
            return false;
        if (!ValidateControl('txtEndDate', 'text', 'input', 'Please Select End date.', validateDateRange, 'txtStartDate', 'txtEndDate', "Start Date' shouldn't be greater than 'Job End Date'"))
            return false;
        var selectedChannel = $('table[ID *="rdoChannelDirect"] input:checked').val();
        if (selectedChannel == "Channel") {
            if (!ValidateControl('ddlMidClient', 'select', 'select', 'Please Select Mid client.'))
                return false;
        }
        if (selectedChannel == "Direct") {
            if (!ValidateControl('ddlEndClient', 'select', 'select', 'Please Select End client.'))
                return false;
        }

        if (!ValidateControl('ddlInterviewType', 'select', 'select', 'Please Select Interview Type.'))
            return false;
        if (!ValidateControl('ddlReqStatus', 'select', 'select', 'Please Select Status.'))
            return false;
        if (!ValidateControl('txtCloseDate', 'text', 'input', 'Please Select Close date.', validateDateRange, 'txtCloseDate', 'txtStartDate', "'Close Date' must be placed before 'Job Start Date' and 'Job End Date'"))
            return false;

    }
    if (index === 3 || index === 4 || index === 5 || index === 6) {
        // id,type,tagname, msg 
        if (!ValidateControl('txtCombo', 'text', 'input', 'Please Select Requirement Type.'))//requirement type
            return false;
        if (!ValidateControl('txtAmount', 'text', 'input', 'Please Enter Pay Rate.', RangeValidationDouble, 'txtAmount', 0, 99999999999, "'Pay rate' must be numeric and positive.", "'Pay rate' must be numeric, positive and less than 99999999999"))
            return false;
        if (!ValidateControl('ddlPayRate', 'select', 'select', 'Please Select Pay type.'))
            return false;
        if (!ValidateControl('txtPayRateHoursAndDaysPerWeek', 'text', 'input', 'Please Enter Hours or Days Per Week.', RangeValidationDouble, 'txtPayRateHoursAndDaysPerWeek', 0, 999, "'Duration' must be numeric and positive.", "'Duration' must be numeric, positive and less than 999"))
            return false;



        if (!ValidateControl('txtDuration', 'text', 'input', 'Please Enter Duration.', RangeValidationDouble, 'txtDuration', 0, 999, "'Duration' must be numeric and positive.", "'Duration' must be numeric, positive and less than 999"))
            return false;
    }
    if (index === 4 || index === 5 || index === 6) {
        // id,type,tagname, msg
        if (!ValidateControl('peOwner_upLevelDiv', 'peoplepicker', 'div', 'Please Enter Requirement Owner.'))//for people picker , pass id as id + '_upLevelDiv'
            return false;
        if (!ValidateControl('ddlPrimarySkills', 'select', 'select', 'Please Select Primary Skills.'))
            return false;
        if (!ValidateControl('txtReqDescription', 'textarea', 'textarea', 'Please Enter Description.'))
            return false;
        if (!ValidateControl('chkHRPeople', 'checkboxlist', 'table', 'Please Select Assign to.'))
            return false;
        if (!ValidateControl('txtSecondarySkills', 'textarea', 'textarea', 'Please Enter Mandatory Skills.'))
            return false;

    }
    if (index === 5 || index === 6) {
        if (!ValidateControl('ddlState', 'select', 'select', 'Please Select State.'))
            return false;
    }
    return true;
}
