var TenderBy = 0;
var FieldId = 0;
var FieldName = "";
var isArchivedTenders = 0;
var TenderYear = '';
var OrderBy = '3';
var isAdvanceSearch = false;
var isWithinSearch = false;
var SubDate_S;
var SubDate_E;
var OpenDate_S;
var OpenDate_E;


var stateList = [];
var cityList = [];
var industryList = [];
var subindustryList = [];
var agencyList = [];
var sectorList = [];
var ownershipList = [];
var keywordList = [];
var countryList = [];

var IsGlobalSearch = false;

$(document).ready(function () {

    IsGlobalSearch = ($('#hdnIsGlobalSearch').val() == 'True') ? true : false;

    setTimeout(function () {
        searchAutoCompleteKeywords();
    }, 500);

    $("#btnSearch").click(function () {
        isWithinSearch = false;
        TenderBy = $('#hdnTendersBy').val();
        FieldId = $('#hdnFieldId').val();
        FieldName = $('#hdnFieldName').val();

        if (!IsGlobalSearch)
        { SearchTenders(); }
        else {
            SearchGlobalTenders();
        }
    });

    $('.searchfill').keydown(function (e) {
        if (e.keyCode == 13) {
            $('#SearchTextBox1').select();
            $("#btnSearch").click();
        }
    });

    $('.searchfill').blur(function (e) {
        var value = this.value.toString().trim(); if (value != '')
        { $('#searchTextVal').val(this.value); }
    });

    jQuery.fn.setTextValue = function () {
        return this.each(function () {
            $(this).keyup(function () {
                var value = this.value.toString().trim(); if (value.length >= 3)
                { $('#searchTextVal').val(this.value); }
                else
                { $('#searchTextVal').val(''); }
            });
        });
    };
    $(".SearchTextBox1").setTextValue();
    $(".SearchTextBox2").setTextValue();



    $("#btnWithinSearchData").click(function () {
        isWithinSearch = true;
        var withinSearchText = $('#txtWithinSearchData').val();
        if (withinSearchText == '') {
            DisplayError('Please enter keyword to search within');
            $('#txtWithinSearchData').focus();
            return false;
        }
        ShowLoading();
        $("#divTenderInfo").html('');

        withinSearchText = withinSearchText.replace(/\ /g, '-');
        var mainpageurl = $('#hdnPageUrl').val();
        //var url = mainpageurl + "/" + withinSearchText;
        var AdvanceSearch = ($('#hdnIsAdvanceSearch').val() == 'True' || $('#hdnIsAdvanceSearch').val() == 'true') ? true : false;

        debugger;
        var url = mainpageurl;
        if (!AdvanceSearch) {
            //url += "/" + withinSearchText;
            var selectedWithinSearchItems = window.location.pathname.split('/')[4];
            if (!(selectedWithinSearchItems == undefined || selectedWithinSearchItems == null || selectedWithinSearchItems == '')) {
                withinSearchText = selectedWithinSearchItems + ',' + withinSearchText;
            }
            url += "/" + withinSearchText;
        }
        else { url += withinSearchText; }




        window.location.href = url;
    });

    $('#txtWithinSearchData').keydown(function (e) {
        if (e.keyCode == 13)
        { $("#btnWithinSearchData").click(); }
    });


    $("#btnLoadMoreData").click(function () {
        LoadMoreData();
    });
    $("#btnReset").click(function () {
        ShowLoading();
        $.ajax({
            type: 'POST',
            url: "/IndianTenders/ClearAllSession",
            cache: false,
            data: ({}),
            success: function () {
                //debugger;
                //var PageUrl = $('#hdnPageUrl').val();
                //window.location = PageUrl;
                //HideLoading();
                TenderBy = $('#hdnTendersBy').val();
                FieldId = $('#hdnFieldId').val();
                FieldName = $('#hdnFieldName').val();

                SearchTenders();
            }
        });
    });

    $(".DisplayName").click(function () {
        var dispTxt = this.innerHTML;
        var searchType = this.id;
        setSearchTypeWithName(dispTxt, searchType, '#btnDisplayName', '#hdnSearchType');
        $("#btnDisplayName_Short").text(dispTxt.substring(0, 1).toUpperCase());
        if (searchType == 1)
        { $(".SearchTextBox1").show(); $(".SearchTextBox2").hide(); }
        else if (searchType == 2)
        { $(".SearchTextBox2").show(); $(".SearchTextBox1").hide(); }
    });
    $(".ClosedTenderYearDisplayName").click(function () {
        var dispTxt = this.innerHTML;
        var tenderYear = this.id;
        var title = '';
        var CurrentTenderYear = $('#hdnCurrentTenderYear').val();
        title = " Archived In - " + tenderYear;
        setSearchTypeWithName(title, tenderYear, '#btnClosedTenderYearDisplayName', '#hdnSelectedTenderYear');
        //$('#hdnSelectedTenderYear').val(tenderYear);
        //$('#btnClosedTenderYearDisplayName').text(title);
    });
});


function SearchTenders() {
    //debugger;
    var searchTextData = '';

    if (!isWithinSearch) {
        searchTextData = $("#searchTextVal").val();
    }
    else {
        searchTextData = $("#txtWithinSearchData").val();
    }

    if (searchTextData.length <= 0 && !isAdvanceSearch && isArchivedTenders == 0) {

        DisplayError('Please enter keyword to search');
        //alert('Please enter keyword to search');
        return false;
    }

    //if (isArchivedTenders == 0 && !isAdvanceSearch) {
    //    if (searchTextData.length < 3) {
    //        alert('Please enter min 3 char to search'); return false;
    //    }
    //}

    var stateids = $('#SelectedState').val();
    var cityids = $('#SelectedCity').val();
    var location = GenerateSingleLocation(stateids, cityids);
    $('#SelectedLocationIds').val(location);

    var industryids = $('#SelectedIndustry').val();
    var subindustryids = $('#SelectedSubIndustry').val();
    var indsubind = GenerateSingleIndustry(industryids, subindustryids);
    $('#SelectedIndustrySubIndustryIds').val(indsubind);

    ShowLoading();
    $("#divTenderInfo").html('');
    $.ajax({
        type: 'POST',
        url: "/IndianTenders/ClearTotalCountSession",
        cache: false,
        data: ({}),
        success: function () {
            var location = $('#SelectedLocationIds').val();
            var indsubind = $('#SelectedIndustrySubIndustryIds').val();
            var productIds = $('#SelectedProduct').val();
            var agencyIds = $('#SelectedAgency').val();
            var sectorIds = $('#SelectedSector').val();
            var ownershipIds = $('#SelectedOwnership').val();
            var searchType = $("#hdnSearchType").val();
            var ourRefNo = $('#txtadvSearchRefNo').val();
            var tenderType = $('#hdnTenderType').val();
            var tenderStatusFlag = $('#hdnTenderStatus').val();
            var tenderValFlag = $('#hdnTenderValue').val();

            TenderYear = (TenderYear == '' || TenderYear == null) ? $('#hdnSelectedTenderYear').val() : TenderYear;
            //OrderBy = $('#hdnOrderBy').val();

            var advanceSearchPara = {
                SelectedStates: $('#SelectedState').val(),
                SelectedCities: $('#SelectedCity').val(),
                SelectedLocations: location,
                SelectedIndustries: $('#SelectedIndustry').val(),
                SelectedSubIndustries: $('#SelectedSubIndustry').val(),
                SelectedIndsubIndustries: indsubind,
                SelectedAgencies: agencyIds,
                SelectedSectors: sectorIds,
                SelectedOwnerships: ownershipIds,
                TenderValFlag: tenderValFlag,
                OurRefNo: ourRefNo,
                TenderTypeId: tenderType,
                TenderStatusFlag: tenderStatusFlag,
                SubDateFrom: SubDate_S,
                SubDateTo: SubDate_E,
                OpDateFrom: OpenDate_S,
                OpDateTo: OpenDate_E,
                IcbNcb: -1,
                SearchProductId: $("#searchProductID").val(),
                SearchProductName: searchTextData
            };

            var searchIndianTenderModel;
            searchIndianTenderModel = {
                TenderBy: TenderBy,
                Page: 1,
                Search: searchTextData,
                SType: searchType,
                IsFirst: 0,
                SearchProductId: $("#searchProductID").val(),
                AdvanceSearch: advanceSearchPara,
                FId: FieldId,
                FName: FieldName,
                TenderYear: TenderYear,
                OrderBy: OrderBy
            };

            $.ajax({
                url: '/IndianTenders/SearchTender/',
                type: 'POST',
                dataType: "json",
                contentType: 'application/json',
                data: JSON.stringify(searchIndianTenderModel),
                success: function (returndata) {
                    ReturnResultdata(returndata);
                },
                error: function (returndata) {
                    ReturnResultdata(returndata);
                }
            });
        }
    }); return true;
}
function ReturnResultdata(returndata) {
    HideLoading();
    var url = returndata.newurl;
    var page = returndata.page;
    var searchText = returndata.searchText;
    var fId = returndata.fid;
    var fName = returndata.fname;
    var tenderName = returndata.TenderWordName;
    var tenderBy = returndata.tenderBy;

    window.location = url + tenderName + fName;

}

function ChangeTenderStatus(tenderstatus) {
    var loadurl = '';
    var AdvanceSearchParameter = {};
    TenderBy = $('#hdnTendersBy').val();

    if (!IsGlobalSearch) {
        loadurl = '/IndianTenders/GetTenderFromTenderStatus/';
        AdvanceSearchParameter = {
            SearchProductId: $('#hdnSearchProuctId').val(),
            SelectedStates: $('#SelectedState').val(),
            SelectedCities: $('#SelectedCity').val(),
            SelectedIndustries: $('#SelectedIndustry').val(),
            SelectedSubIndustries: $('#SelectedSubIndustry').val(),
            SelectedAgencies: $('#SelectedAgency').val(),
            SelectedSectors: $('#SelectedSector').val(),
            SelectedOwnerships: $('#SelectedOwnership').val(),

            SelectedProducts: $('#SelectedProduct').val(),
            SelectedLocations: $('#SelectedLocationIds').val(),
            SelectedIndsubIndustries: $('#SelectedIndustrySubIndustryIds').val(),

            TenderStatusFlag: parseInt(tenderstatus),
            TenderBy: TenderBy,
            WithinSearchedText: $('#hdnWithinSearchedText').val(),
        };
        if (TenderBy == 0 || TenderBy == 8) {
            AdvanceSearchParameter.SearchProductName = $('#hdnSearchtext').val();
        }
    }
    else { loadurl = '/GlobalTenders/GetTenderFromTenderStatus/'; }

    $('#hdnTenderStatus').val(tenderstatus);

    $("#divTenderInfo").html('');
    ShowLoading();
    $.ajax({
        type: 'POST',
        url: loadurl,
        dataType: "json",
        contentType: 'application/json',
        data: JSON.stringify(AdvanceSearchParameter),
        //data: "tenderBy=" + TenderBy + "&&tenderStatus=" + parseInt(tenderstatus) + "&&advanceSearchPara=" + AdvanceSearchParameter,
        success: function (data, textstatus) {
            if (data != '') {
                $("#divTenderInfo").html(data);
            }

            var tabCatLinks = document.getElementsByClassName("tabCatLinks");
            for (i = 0; i < tabCatLinks.length; i++) {
                tabCatLinks[i].className = tabCatLinks[i].className.replace("active", "");
            }
            $('.tabCatLinksSpans').text('');

            switch (parseInt(tenderstatus)) {
                case 0:
                    $('#linkAllTenders').addClass('active');
                    //var Total = $('#hdnTotalCount').val();
                    $('#lblAll').html('<small><b>(' + Total + ')</b></small>');
                    if (Total >= 10)
                    { $('#divloadmore').show(); }
                    else
                    { $('#divloadmore').hide(); }
                    break;
                case 1:
                    $('#linkLiveTenders').addClass('active');
                    //var Live = $('#hdnTotalLive').val();
                    $('#lblLive').html('<small><b>(' + TotalLive + ')</b></small>');
                    if (TotalLive >= 10)
                    { $('#divloadmore').show(); }
                    else
                    { $('#divloadmore').hide(); }
                    break;
                case 2:
                    $('#linkFreshTenders').addClass('active');
                    //var Fresh = $('#hdnTotalFresh').val();
                    $('#lblFresh').html('<small><b>(' + TotalFresh + ')</b></small>');
                    if (TotalFresh >= 10)
                    { $('#divloadmore').show(); }
                    else
                    { $('#divloadmore').hide(); }
                    break;
                case 3:
                    $('#linkArchiveTenders').addClass('active');
                    $('#lblArchive').html('<small><b>(' + TotalClosed + ')</b></small>');
                    if (TotalClosed >= 10)
                    { $('#divloadmore').show(); }
                    else
                    { $('#divloadmore').hide(); }
                    break;
            }

            HideLoading();
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            console.log(errorThrown);
            if (XMLHttpRequest.responseText != '') {
                $("#divTenderInfo").html(XMLHttpRequest.responseText);
            }

            var tabCatLinks = document.getElementsByClassName("tabCatLinks");
            for (i = 0; i < tabCatLinks.length; i++) {
                tabCatLinks[i].className = tabCatLinks[i].className.replace("active", "");
            }
            $('.tabCatLinksSpans').text('');

            switch (parseInt(tenderstatus)) {
                case 0:
                    $('#linkAllTenders').addClass('active');
                    //var Total = $('#hdnTotalCount').val();
                    $('#lblAll').html('<small><b>(' + Total + ')</b></small>');
                    if (Total >= 10)
                    { $('#divloadmore').show(); }
                    else
                    { $('#divloadmore').hide(); }
                    break;
                case 1:
                    $('#linkLiveTenders').addClass('active');
                    //var Live = $('#hdnTotalLive').val();
                    $('#lblLive').html('<small><b>(' + TotalLive + ')</b></small>');
                    if (TotalLive >= 10)
                    { $('#divloadmore').show(); }
                    else
                    { $('#divloadmore').hide(); }
                    break;
                case 2:
                    $('#linkFreshTenders').addClass('active');
                    //var Fresh = $('#hdnTotalFresh').val();
                    $('#lblFresh').html('<small><b>(' + TotalFresh + ')</b></small>');
                    if (TotalFresh >= 10)
                    { $('#divloadmore').show(); }
                    else
                    { $('#divloadmore').hide(); }
                    break;
                case 3:
                    $('#linkArchiveTenders').addClass('active');
                    $('#lblArchive').html('<small><b>(' + TotalClosed + ')</b></small>');
                    if (TotalClosed >= 10)
                    { $('#divloadmore').show(); }
                    else
                    { $('#divloadmore').hide(); }
                    break;
            }

            HideLoading();
            //alert(errorThrown);
        }
    });
}
function SearchTenderByTenderYear() {
    isArchivedTenders = 1;
    TenderBy = $('#hdnTendersBy').val();
    FieldId = $('#hdnFieldId').val();
    FieldName = $('#hdnFieldName').val();

    var currentTenderYear = $('#hdnCurrentTenderYear').val();
    var selectedTenderYear = $('#hdnSelectedTenderYear').val();

    TenderYear = $('#hdnSelectedTenderYear').val();

    if (parseInt(currentTenderYear) == parseInt(selectedTenderYear))
    { $('#hdnTenderStatus').val('3'); }

    SearchTenders();
}


var page = 0;
function LoadMoreData() {
    var AdvanceSearchParameter = {};
    TenderBy = $('#hdnTendersBy').val();

    var loadurl = '';
    if (!IsGlobalSearch) {
        var tenderstatus = $('#hdnTenderStatus').val();
        loadurl = '/IndianTenders/GetTenderResultOnLoading/';
        AdvanceSearchParameter = {
            SearchProductId: $('#hdnSearchProuctId').val(),
            SelectedStates: $('#SelectedState').val(),
            SelectedCities: $('#SelectedCity').val(),
            SelectedIndustries: $('#SelectedIndustry').val(),
            SelectedSubIndustries: $('#SelectedSubIndustry').val(),
            SelectedAgencies: $('#SelectedAgency').val(),
            SelectedSectors: $('#SelectedSector').val(),
            SelectedOwnerships: $('#SelectedOwnership').val(),

            SelectedProducts: $('#SelectedProduct').val(),
            SelectedLocations: $('#SelectedLocationIds').val(),
            SelectedIndsubIndustries: $('#SelectedIndustrySubIndustryIds').val(),

            TenderStatusFlag: parseInt(tenderstatus),
            TenderBy: TenderBy,
            WithinSearchedText: $('#hdnWithinSearchedText').val(),
        };
        if (TenderBy == 0 || TenderBy == 8) {
            AdvanceSearchParameter.SearchProductName = $('#hdnSearchtext').val();
        }
    }
    else { loadurl = '/GlobalTenders/GetTenderResultOnLoading/'; }

    if (NewPage > -1 && NewPage <= TotalPage) {
        console.log('NewPage - ' + NewPage);
        AdvanceSearchParameter.NewPage = NewPage;
        //ShowLoading();
        //var toplength = $('#divTenderInfo').height();
        $.ajax({
            type: 'POST',
            url: loadurl,
            dataType: "json",
            contentType: 'application/json',
            data: JSON.stringify(AdvanceSearchParameter),
            //data: "tenderBy=" + TenderBy + "&&page=" + NewPage,
            success: function (data, textstatus) {
                if (data != '') {
                    $("#divTenderInfo").append(data);
                    //$(window).scrollTop($('#divTenderInfo').offset().top + toplength);
                }
                else {
                    NewPage = -1;
                }
                //inCallback = false;
                //HideLoading();
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                console.log(errorThrown);
                if (XMLHttpRequest.responseText != '') {
                    $("#divTenderInfo").append(XMLHttpRequest.responseText);
                }
                else {
                    NewPage = -1;
                }
                //HideLoading();
            }
        });
    }
}


function GenerateSingleLocation(stateid, cityid) {
    var SelectedLocationIds = "";
    $.ajax({
        url: '/IndianTenders/GenerateSingleLocation',
        type: 'POST',
        data: JSON.stringify({ stateIds: stateid, LocIds: cityid }),
        dataType: "json",
        async: false,
        contentType: 'application/json',
        success: function (returndata) {
            SelectedLocationIds = returndata;
        },
        error: function (a, b, c) {
            //alert('error');
            SelectedLocationIds = "";
        }
    });

    return SelectedLocationIds;
}
function GenerateSingleIndustry(indid, subindid) {
    var SelectedIndustrySubIndustryIds = "";
    $.ajax({
        url: '/IndianTenders/GenerateSingleIndustry',
        type: 'POST',
        data: JSON.stringify({ indIds: indid, subIndIds: subindid }),
        dataType: "json",
        async: false,
        contentType: 'application/json',
        success: function (returndata) {
            SelectedIndustrySubIndustryIds = returndata;
        },
        error: function (a, b, c) {
            //alert('error');
            SelectedIndustrySubIndustryIds = "";
        }
    });

    return SelectedIndustrySubIndustryIds;
}


function DownloadTender(OurRefNo) {
    var InquiryTypeName = $('#InquiryTypeName').val();
    $('#formTitle').text(InquiryTypeName + ' - #' + OurRefNo);
    $('#TenderID').val(OurRefNo);
}
function RemoveWithinSearch(removeText) {
    ShowLoading();
    debugger;
    var selectedItems = $('#hdnWithinSearchedText').val().split(',');
    var index = selectedItems.indexOf(removeText);
    if (index > -1) {
        selectedItems.splice(index, 1);
    }

    lastWithinSearch=   selectedItems.join(',');
    //var replaceItem = selectedItems.includes(",") ? removeText + ',' : removeText;
    //var lastWithinSearch = selectedItems.replace(replaceItem, "");
    //lastWithinSearch = selectedItems.replace(",", "").trim();

    var PageUrl = $('#hdnPageUrl').val();
    if (lastWithinSearch == '')
        $.ajax({
            type: 'POST',
            url: "/IndianTenders/ClearAllSession",
            cache: false,
            data: ({}),
            success: function () {
                window.location = PageUrl;
            }
        });
    else {
        //window.location.href = url = PageUrl + "/" + lastWithinSearch;
        var AdvanceSearch = ($('#hdnIsAdvanceSearch').val() == 'True' || $('#hdnIsAdvanceSearch').val() == 'true') ? true : false;

        var url = PageUrl;
        if (!AdvanceSearch)
        { url += "/" + lastWithinSearch; }
        else
        { url += lastWithinSearch; }

        window.location.href = url;// = PageUrl + "/" + lastWithinSearch;
    }

    //$.ajax({
    //    type: 'POST',
    //    url: "/IndianTenders/RemoveWithinSearchWords",
    //    cache: false,
    //    data: ({ removeText: removeText }),
    //    success: function (lastWithinSearch) {
    //        HideLoading();
    //        var PageUrl = $('#hdnPageUrl').val();
    //        if (lastWithinSearch == '')
    //            $.ajax({
    //                type: 'POST',
    //                url: "/IndianTenders/ClearAllSession",
    //                cache: false,
    //                data: ({}),
    //                success: function () {
    //                    window.location = PageUrl;
    //                }
    //            });
    //        else {
    //            //window.location.href = url = PageUrl + "/" + lastWithinSearch;
    //            var AdvanceSearch = ($('#hdnIsAdvanceSearch').val() == 'True' || $('#hdnIsAdvanceSearch').val() == 'true') ? true : false;

    //            var url = PageUrl;
    //            if (!AdvanceSearch)
    //            { url += "/" + lastWithinSearch; }
    //            else
    //            { url += lastWithinSearch; }

    //            window.location.href = url;// = PageUrl + "/" + lastWithinSearch;
    //        }

    //    }
    //});
}

function RemoveGlobalWithinSearch(removeText) {
    ShowLoading();
    $.ajax({
        type: 'POST',
        url: "/GlobalTenders/RemoveWithinSearchWords",
        cache: false,
        data: ({ removeText: removeText }),
        success: function (lastWithinSearch) {
            HideLoading();
            var PageUrl = $('#hdnPageUrl').val();
            if (lastWithinSearch == '')
                $.ajax({
                    type: 'POST',
                    url: "/GlobalTenders/ClearAllSession",
                    cache: false,
                    data: ({}),
                    success: function () {
                        window.location = PageUrl;
                    }
                });
            else {
                window.location.href = url = PageUrl + "/" + lastWithinSearch;
            }

        }
    });
}


function SearchGlobalTenders() {
    //debugger;
    var searchTextData = '';
    searchTextData = $("#searchTextVal").val();

    if (searchTextData.length <= 0 && !isAdvanceSearch) {
        DisplayError('Please enter keyword to search');
        return false;
    }

    var tenderType = $('#hdnTenderType').val();
    var tenderStatusFlag = $('#hdnTenderStatus').val();
    var productIds = $('#SelectedProduct').val();
    var countryIds = $("#SelectedCountry").val();
    var agencyIds = $('#SelectedAgency').val();
    var ourRefNo = $('#txtadvSearchRefNo').val();
    var searchType = $("#hdnSearchType").val();

    ShowLoading();
    $("#divTenderInfo").html('');
    $.ajax({
        type: 'POST',
        url: "/GlobalTenders/ClearTotalCountSession",
        cache: false,
        data: ({}),
        success: function () {

            var advanceSearchPara = {
                SelectedCountries: countryIds,
                SelectedProducts: productIds,
                SelectedAgencies: agencyIds,
                OurRefNo: ourRefNo,
                TenderTypeId: tenderType,
                TenderStatusFlag: tenderStatusFlag,
                SubDateFrom: SubDate_S,
                SubDateTo: SubDate_E,
                OpDateFrom: OpenDate_S,
                OpDateTo: OpenDate_E,
                SearchProductId: $("#searchProductID").val()
            };

            var searchGlobalTenderModel = {
                TenderBy: TenderBy,
                Page: 1,
                Search: searchTextData,
                SType: searchType,
                IsFirst: 0,
                AdvanceSearch: advanceSearchPara,
                FId: FieldId,
                FName: FieldName,
                TenderStatusFlag: tenderStatusFlag,
                //TenderYear: TenderYear,
                //OrderBy: OrderBy
            };

            $.ajax({
                url: '/GlobalTenders/SearchTender/',
                type: 'POST',
                dataType: "json",
                contentType: 'application/json',
                data: JSON.stringify(searchGlobalTenderModel),
                success: function (returndata) {
                    ReturnResultdata(returndata);
                },
                error: function (returndata) {
                    ReturnResultdata(returndata);
                }
            });
        }
    }); return true;
}