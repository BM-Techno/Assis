
$(function () {



    var start = moment().subtract(29, 'days');
    var end = moment();

    function SetSubDate(start, end) {
        $('#advSearchSubDate span').html(start.format('MMMM D, YYYY') + ' - ' + end.format('MMMM D, YYYY'));
        SubDate_S = $('#advSearchSubDate ').data('daterangepicker').startDate._d;
        SubDate_E = $('#advSearchSubDate').data('daterangepicker').endDate._d;
    }
    function SetOpenDate(start, end) {
        $('#advSearchOpenDate span').html(start.format('MMMM D, YYYY') + ' - ' + end.format('MMMM D, YYYY'));
        OpenDate_S = $('#advSearchOpenDate ').data('daterangepicker').startDate._d;
        OpenDate_E = $('#advSearchOpenDate').data('daterangepicker').endDate._d;
    }

    $('#advSearchSubDate').daterangepicker({
        startDate: start,
        endDate: end,
        ranges: {
            'Today': [moment(), moment()],
            'Yesterday': [moment().subtract(1, 'days'), moment().subtract(1, 'days')],
            'Last 7 Days': [moment().subtract(6, 'days'), moment()],
            'Last 30 Days': [moment().subtract(29, 'days'), moment()],
            'This Month': [moment().startOf('month'), moment().endOf('month')],
            'Last Month': [moment().subtract(1, 'month').startOf('month'), moment().subtract(1, 'month').endOf('month')]
        }
    }, SetSubDate);

    $('#advSearchOpenDate').daterangepicker({
        startDate: start,
        endDate: end,
        ranges: {
            'Today': [moment(), moment()],
            'Yesterday': [moment().subtract(1, 'days'), moment().subtract(1, 'days')],
            'Last 7 Days': [moment().subtract(6, 'days'), moment()],
            'Last 30 Days': [moment().subtract(29, 'days'), moment()],
            'This Month': [moment().startOf('month'), moment().endOf('month')],
            'Last Month': [moment().subtract(1, 'month').startOf('month'), moment().subtract(1, 'month').endOf('month')]
        }
    }, SetOpenDate);

    //SetSubDate(start, end);
    //SetOpenDate(start, end);


    var subStart = ''; var subEnd = '';
    if ($('#hdnSubDateFrom').val() != '' && $('#hdnSubDateFrom').val() != null && $('#hdnSubDateFrom').val() != undefined) {
        var subStart = new Date($('#hdnSubDateFrom').val());
    }
    if ($('#hdnSubDateTo').val() != '' && $('#hdnSubDateTo').val() != null && $('#hdnSubDateTo').val() != undefined) {
        var subEnd = new Date($('#hdnSubDateTo').val());
    }

    if (subStart != '' && subEnd != '') {
        var start_s = moment(subStart);
        var end_s = moment(subEnd);
        SetSubDate(start_s, end_s);
    }

    var opStart = ''; var opEnd = '';
    if ($('#hdnOpDateFrom').val() != '' && $('#hdnOpDateFrom').val() != null && $('#hdnOpDateFrom').val() != undefined) {
        var opStart = new Date($('#hdnOpDateFrom').val());
    }
    if ($('#hdnOpDateTo').val() != '' && $('#hdnOpDateTo').val() != null && $('#hdnOpDateTo').val() != undefined) {
        var opEnd = new Date($('#hdnOpDateTo').val());
    }

    if (opStart != '' && opEnd != '') {
        var start_o = moment(opStart);
        var end_o = moment(opEnd);
        SetOpenDate(start_o, end_o);
    }
});


//$(document).ready(function () {

//    setTimeout(function () {
//        var subStart = ''; var subEnd = '';
//        if ($('#hdnSubDateFrom').val() != null && $('#hdnSubDateFrom').val() != undefined) {
//            var subStart = new Date($('#hdnSubDateFrom').val());
//        }
//        if ($('#hdnSubDateTo').val() != null && $('#hdnSubDateTo').val() != undefined) {
//            var subEnd = new Date($('#hdnSubDateTo').val());
//        }

//        SetSubDate(subStart, subEnd);

//    }, 1000);


//});