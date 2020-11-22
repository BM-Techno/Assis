$(document).ready(function () {
    LoadCaptha();
    $("#btnReloadCaptcha").click(function () {
        LoadCaptha();
    });

    $("#btnInquiryData").click(function () {
        SubmitData();
    });

    $('.textbox').keydown(function (e) {
        if (e.keyCode == 13) {
            SubmitData();
        }
    });

    $("#drpCountry").change(function () {
        var countryId = this.value;
        $("#drpCity").html("<option value='0'>[CITY]</option>");
        $("#drpState").html("<option value='0'>[CITY]</option>");

        $.ajax({
            type: 'POST',
            url: "/InquiryForms/GetStateByCountry",
            cache: false,
            data: ({ countryId: countryId }),
            success: function (data) {
                var items = "";
                $.each(data.StateList, function (i, item) {
                    items += "<option value='" + item.Value + "'>" + item.Text + "</option>";
                });
                $("#drpState").html(items);
            }
        });
    });
    $("#drpState").change(function () {
        $("#drpCity").html("<option value='0'>[CITY]</option>");

        var stateId = this.value;
        $.ajax({
            type: 'POST',
            url: "/InquiryForms/GetCityByState",
            cache: false,
            data: ({ stateId: stateId }),
            success: function (data) {
                var items = "";
                $.each(data.CityList, function (i, item) {
                    items += "<option value='" + item.Value + "'>" + item.Text + "</option>";
                });
                $("#drpCity").html(items);
            }
        });
    });

    jQuery.fn.checkNumericOnly = function () {
        return this.each(function () {
            $(this).keydown(function (e) {
                var key = e.charCode || e.keyCode || 0;
                return (key == 8 || key == 9 || key == 46 || key == 110 || (key >= 37 && key <= 40) || (key >= 48 && key <= 57) || (key >= 96 && key <= 105));
            });
        });
    };

    $("#txtMobileNo").checkNumericOnly();
});


function SubmitData() {

    $(".redtxt").text('');
    $(".textbox").removeClass("errorPointer");

    var isValid = true;
    var myform = $('#SubmitInquiry');

    var FormType = $('#FormType').val();

    if ($("#txtName").val().trim() == '') {
        isValid = false;
        $('#txtName').attr("title", "Name is required");
        $('#txtName').addClass("errorPointer");
    }
    if ($("#txtMobileNo").val().trim() == '') {
        isValid = false;
        $('#txtMobileNo').attr("title", "Mobile is required");
        $('#txtMobileNo').addClass("errorPointer");
    }
    if ($("#txtInterestedTenders").val().trim() == '') {
        isValid = false;
        $('#txtInterestedTenders').attr("title", "Description is required");
        $('#txtInterestedTenders').addClass("errorPointer");
    }
    if ($("#txtEmailId").val().trim() == '') {
        isValid = false;
        $('#txtEmailId').attr("title", "Email is required");
        $('#txtEmailId').addClass("errorPointer");
    }
    else if (!ValidEmail($("#txtEmailId").val())) {
        isValid = false;
        $('#txtEmailId').attr("title", "Email provided is invalid");
        $('#txtEmailId').addClass("errorPointer");
    }

    if (parseInt(FormType) != 2) {
        if ($("#txtCompanyName").val().trim() == '') {
            isValid = false;
            $('#txtCompanyName').attr("title", "Company Name is required");
            $('#txtCompanyName').addClass("errorPointer");
        }
        if ($("#drpState").val().trim() == '0') {
            isValid = false;
            $('#drpState').attr("title", "State is required");
            $('#drpState').addClass("errorPointer");
        }
        if ($("#drpCity").val().trim() == '0') {
            isValid = false;
            $('#drpCity').attr("title", "City is required");
            $('#drpCity').addClass("errorPointer");
        }

        if (parseInt(FormType) == 1) {

            if ($("#drpCountry").val().trim() == '0') {
                isValid = false;
                $('#drpCountry').attr("title", "Country is required");
                $('#drpCountry').addClass("errorPointer");
            }
        }
    }


    if ($("#txtCaptcha").val().trim() == '') {
        isValid = false;
        $("#txtCaptcha").addClass("errorPointer");
        $('#txtCaptcha').attr("title", "Verification is required");
    }


    if (!isValid) {
        LoadCaptha();
        return false;
    }

    $('#btnInquiryData').attr("style", "display:none");
    ShowLoading();

    $.ajax({
        cache: false,
        async: true,
        type: "POST",
        url: myform.attr('action'),
        data: myform.serialize(),
        success: function (data) {
            var msg = data.msg;
            var loginUserId = data.LoginUserId;

            if (msg == "ok") {
                var usermail = data.userEmail;
                var userid = data.id;

                $(".txtBox").removeClass("errorInfo");
                $(".txtBox").val("");

                if (loginUserId == 0)
                { window.location = "/InquiryForms/Thanks?email=" + usermail; }
                else
                {
                    HideLoading();
                    $('#errmsg').attr('class', 'error_msg text-green');
                    $('#errmsg').html('<i class="fa fa-check"></i> Congratulations! Your Renewal information has beed sent to the Administration successfully.' +
                        '<br/> Your Renewal process will be completed within 24 hours..' +
                        '<br/> You have been received our mail at Email Address ' + usermail);
                }
            }
            else if (msg == "error") {
                $('#btnInquiryData').attr("style", "display:inline");

                HideLoading();
                $('#errmsg').attr('class', 'error_msg text-red');

                if (loginUserId == 0)
                { $('#errmsg').html('Your inquiry has not been send yet. Please try again.'); }
                else
                { $('#errmsg').html('<i class="fa fa-times-circle-o"></i> Your Renewal information could not submitted, Please try again.'); }
            }
            else if (msg == "captchaerror") {
                $('#btnInquiryData').attr("style", "display:inline");

                HideLoading();

                $("#txtCaptcha").addClass("errorPointer");
                $('#txtCaptcha').attr("title", "Verification is invalid! Please try again.");

                $('#errmsg').attr('class', 'error_msg redtxt');
                $('.error_msg').html('Verification is invalid! Please try again.');
            }
            LoadCaptha();
        }
    });
    return true;
}