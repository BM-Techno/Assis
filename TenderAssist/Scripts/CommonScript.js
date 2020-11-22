function ShowLoading() {
    $('#loader').show();
}
function HideLoading() {
    $('#loader').hide();
}

function ValidEmail(email) {
    var filter = /^([\w-\.]+@([\w-]+\.)+[\w-]{2,4})?$/;;
    return filter.test(email);
}

function setSearchTypeWithName(dispTxt, searchType, fildId, hdnFieldId) {
    $(fildId).text(dispTxt);
    $(hdnFieldId).val(searchType);
}

function LoadCaptha() {
    var eve = 'Captcha_Code';
    var captchaId = '#img_' + eve;
    var hdncaptchaId = '#hdn_' + eve;
    $.ajax({
        type: 'POST', url: "/Home/ReloadCaptcha",
        cache: !1,
        data: ({}),
        success: function (data) {
            $(captchaId).html(data.imageDataURL);
            $(hdncaptchaId).val(data.imageDataURL);
        },
        error: function (data) {
            $(captchaId).html(data.imageDataURL);
            $(hdncaptchaId).val(data.imageDataURL);
        }
    });
}


function DisplayError(error) {
    var href = $('#btnErrorMsg').attr('href');
    $('#errorTitle').html('<i class="fa fa-times-circle"></i> ERROR');
    $('#errorPopupContent').html(error);
    window.location.href = href;
    return false;
}

$(document).ready(function () {

    jQuery.fn.checkNumeric = function () {
        return this.each(function () {
            $(this).keydown(function (e) {
                var key = e.charCode || e.keyCode || 0;
                return (key == 8 || key == 9 || key == 46 || key == 110 || (key >= 37 && key <= 40) || (key >= 48 && key <= 57) || (key >= 96 && key <= 105));
            });
        });
    };

    jQuery.fn.checkNumericOnly = function () {
        return this.each(function () {
            $(this).keydown(function (e) {
                var key = e.charCode || e.keyCode || 0;
                return (key == 8 || key == 9 || key == 46 || key == 110 || (key >= 37 && key <= 40) || (key >= 48 && key <= 57) || (key >= 96 && key <= 105));
            });
        });
    };

});