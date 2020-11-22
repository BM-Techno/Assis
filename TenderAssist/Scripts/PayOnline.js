$(document).ready(function () {
    $("#btnPayNow").click(function () {
        SubmitData();
    });

    $('.textbox').keydown(function (e) {
        if (e.keyCode == 13) {
            SubmitData();
        }
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
    $(".payonlinetext").checkNumericOnly();
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

    if (!isValid) { 
        return false;
    }

    $('#btnPayNow').attr("style", "display:none");
    ShowLoading();

    $('#PayContactName').val('');
    $('#PayMobNo').val('');
    $('#PayEmail').val('');

    $.ajax({
        cache: false,
        async: true,
        type: "POST",
        url: myform.attr('action'),
        data: myform.serialize(),
        success: function (data) {
            var msg = data.msg;
            if (msg == "ok") {
                var usermail = data.userEmail;
                var userid = data.id;

                $('#PayContactName').val($('#txtName').val());
                $('#PayMobNo').val($('#txtMobileNo').val());
                $('#PayEmail').val($('#txtEmailId').val());

                $('#errmsg').attr('class', 'mar5 text-success pagetitle');
                $('#errmsg').html('Thank you for inquiry with us. Please select anyone of the displayed plan below and we are redirecting you for the payment.');

                $('#divPaymentPanel').slideDown('slow');
                HideLoading();

            }
            else if (msg == "error") {
                HideLoading();
                $('#btnPayNow').attr("style", "display:inline");

                $('#errmsg').attr('class', 'mar5 text-danger pagetitle');
                $('#errmsg').html('Your inquiry has not been send yet. Please try again.');
            }          
        }
    });

    return true;
}

function PlanSelection(id) {
    var isValid = true;   
    $(".payonlinetext").removeClass("errorPointer");
    $('.planSelected').hide();

    var planTitle = '#planTitle_' + id;
    var paynowtitle = $(planTitle).text();
   
    var selectediconid = '#selectedPlan_' + id;
    $(selectediconid).show();
       
    var planamount = '#txtAmount_' + id;

    var amount = $(planamount).val();
    if (amount == '' || amount == undefined || amount == '0') {
        isValid = false;
        $(planamount).attr("title", "Amount is required");
        $(planamount).addClass("errorPointer");
    }

    //switch (parseInt(id)) {
    //    case 1: amount = '7000';
    //        break;
    //    case 2: amount = '10000';
    //        break;
    //    case 3: amount = '15000';
    //        break;
    //    case 4: amount = '45000';
    //        break;
    //}
    if (!isValid) {
        return false;
    }
    $('#PayCharges').val(amount);
    $('#PayPlanId').val(parseInt(id));
    $('#paymentmessage').html('Thank you for selecting the plan : ' + paynowtitle + '. We are redirecting you for the payment process...');
    setTimeout(function () {
        MakePayment();
    }, 500);
}

function MakePayment() {
    var payContactName = $('#PayContactName').val();
    var payMobNo = $('#PayMobNo').val();
    var payEmail = $('#PayEmail').val();
    var payCharges = $('#PayCharges').val();

    var isValid = true;

    var myform = $('#frmPaymentOnline');
    $('#paymentloader').show();

    $.ajax({
        cache: false,
        async: true,
        type: "POST",
        url: myform.attr('action'),
        data: myform.serialize(),
        success: function (data) {
            $('#paymentmessage').html('');
            if (data.isvalid) {
                var paymenturl = data.url;
                window.location.href = paymenturl;
            }
            else {
                HideLoading();
                DisplayError(data.msg);
                //alert(data.msg);
                $('#btnPayNow').attr("style", "display:block;");
            }
        }
    });
    return true;

}