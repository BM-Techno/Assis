
$(document).ready(function () {

    $('.textbox').keydown(function (e) {
        if (e.keyCode == 13) {
            $("#advsearchTextVal").val($("#advsearchText").val()); advanceSearchtenders();
        }
    });


    $(".advSearchTextBox1").blur(function () {
        $("#advsearchTextVal").val(this.value);
    });

    $("#txtadvSearchSubDate").blur(function () { var date = this.value; if (date != "") { if (!ValidateDate(this)) { this.value = ''; } } });
    $("#txtadvSearchOpDate").blur(function () { var date = this.value; if (date != "") { if (!ValidateDate(this)) { this.value = ''; } } });

    $(".DisplayTenderType").click(function () {
        var dispTxt = this.innerHTML;
        var tenderType = this.id;
        setSearchTypeWithName(dispTxt, tenderType, '#lblTenderType', '#hdnTenderType');
    });

    $(".DisplayTenderStatus").click(function () {
        var dispTxt = this.innerHTML;
        var tenderStatus = this.id;
        setSearchTypeWithName(dispTxt, tenderStatus, '#lblTenderStatus', '#hdnTenderStatus');
    });

    $(".DisplayTenderValue").click(function () {
        var dispTxt = this.innerHTML;
        var tenderValue = this.id;
        setSearchTypeWithName(dispTxt, tenderValue, '#lblTenderValue', '#hdnTenderValue');
    });


    $(".CountryRegionName").click(function () {
        var dispTxt = this.innerHTML;
        var regionId = this.id;
        setSearchTypeWithName(dispTxt, regionId, '#btnCountryRegionName', '#hdnTendersBy');
    });

    if ($('#SelectedState').val() != undefined && $('#SelectedState').val() != '' && stateList.indexOf($('#SelectedState').val()) === -1) {
        stateList = $('#SelectedState').val().split(',');
    }
    if ($('#SelectedCity').val() != undefined && $('#SelectedCity').val() != '' && cityList.indexOf($('#SelectedCity').val()) === -1) {
        cityList = $('#SelectedCity').val().split(',');
    }
    if ($('#SelectedIndustry').val() != undefined && $('#SelectedIndustry').val() != '' && industryList.indexOf($('#SelectedIndustry').val()) === -1) {
        industryList = $('#SelectedIndustry').val().split(',');
    }
    if ($('#SelectedSubIndustry').val() != undefined && $('#SelectedSubIndustry').val() != '' && subindustryList.indexOf($('#SelectedSubIndustry').val()) === -1) {
        subindustryList = $('#SelectedSubIndustry').val().split(',');
    }

    if ($('#SelectedAgency').val() != undefined && $('#SelectedAgency').val() != '' && agencyList.indexOf($('#SelectedAgency').val()) === -1) {
        agencyList = $('#SelectedAgency').val().split(',');
    }
    if ($('#SelectedSector').val() != undefined && $('#SelectedSector').val() != '' && sectorList.indexOf($('#SelectedSector').val()) === -1) {
        sectorList = $('#SelectedSector').val().split(',');
    }
    if ($('#SelectedOwnership').val() != undefined && $('#SelectedOwnership').val() != '' && ownershipList.indexOf($('#SelectedOwnership').val()) === -1) {
        ownershipList = $('#SelectedOwnership').val().split(',');
    }


    if ($('#SelectedProduct').val() != undefined && $('#SelectedProduct').val() != '' && keywordList.indexOf($('#SelectedProduct').val()) === -1) {
        keywordList = $('#SelectedProduct').val().split(',');
    }
    if ($('#SelectedCountry').val() != undefined && $('#SelectedCountry').val() != '' && countryList.indexOf($('#SelectedCountry').val()) === -1) {
        countryList = $('#SelectedCountry').val().split(',');
    }



    if ($('#hdnOurRefNo').val() != '' && $('#hdnOurRefNo').val() != undefined) {
        $('#txtadvSearchRefNo').val($('#hdnOurRefNo').val());
    }

    if ($('#hdnTenderType').val() != '' && $('#hdnTenderType').val() != undefined && $('#hdnTenderType').val() != '0') {
        setSearchTypeWithName($('#hdnTenderTypeText').val(), $('#hdnTenderType').val(), '#lblTenderType', '#hdnTenderType');
    }
    if ($('#hdnTenderStatus').val() != '' && $('#hdnTenderStatus').val() != undefined && $('#hdnTenderStatus').val() != '0') {
        setSearchTypeWithName($('#hdnTenderStatusText').val(), $('#hdnTenderStatus').val(), '#lblTenderStatus', '#hdnTenderStatus');
    }
    if ($('#hdnTenderValue').val() != '' && $('#hdnTenderValue').val() != undefined && $('#hdnTenderValue').val() != '0') {
        setSearchTypeWithName($('#hdnTenderValueText').val(), $('#hdnTenderValue').val(), '#lblTenderValue', '#hdnTenderValue');
    }

    /*for country region*/
    //if ($('#hdnTendersBy').val() != '' && $('#hdnTendersBy').val() != undefined && $('#hdnTendersBy').val() != '0') {
    //    setSearchTypeWithName($('#hdnTenderTypeText').val(), $('#hdnTenderType').val(), '#lblTenderType', '#hdnTenderType');
    //}

    jQuery.fn.checkNumericOnly = function () {
        return this.each(function () {
            $(this).keydown(function (e) {
                var key = e.charCode || e.keyCode || 0;
                return (key == 8 || key == 9 || key == 46 || key == 110 || (key >= 37 && key <= 40) || (key >= 48 && key <= 57) || (key >= 96 && key <= 105));
            });
        });
    };

    $("#txtadvSearchTenderValue").checkNumericOnly();
    $("#txtadvSearchRefNo").checkNumericOnly();


    $('#div_ShowHideAdvSearchPanel').click(function () {
        $('#AdvanceFilterSearch_form').slideToggle('fast');

        if ($('#ShowHideAdvSearchPanel').hasClass('fa-plus-circle')) {
            $('#ShowHideAdvSearchPanel').removeClass('fa-plus-circle');
            $('#ShowHideAdvSearchPanel').addClass('fa-minus-circle');
        }
        else {
            $('#ShowHideAdvSearchPanel').removeClass('fa-minus-circle');
            $('#ShowHideAdvSearchPanel').addClass('fa-plus-circle');
        }
    });
});

function loadList(basetype) {
    var type = parseInt(basetype);
    var uiListname = '';
    var listarray = [];
    var selectedItemCount = '';
    var liId = '';
    var txtSearchName = '';
    var placeHolder = '';

    switch (type) {
        case 1:
            uiListname = 'ulStateList';
            listarray = stateList;
            //selectedItemCount = ' (' + listarray.length + ' State(s) selected)';
            liId = 'lblLIStateItems';
            txtSearchName = 'txtStateSearch';
            placeHolder = 'Enter State Name';
            break;
        case 2:
            uiListname = 'ulCityList';
            listarray = cityList;
            //selectedItemCount = ' (' + listarray.length + ' City(s) selected)';
            liId = 'lblLICityItems';
            txtSearchName = 'txtCitySearch';
            placeHolder = 'Enter City Name';
            break;
        case 3:
            uiListname = 'ulIndustryList';
            listarray = industryList;
            //selectedItemCount = ' (' + listarray.length + ' Industry(s) selected)';
            liId = 'lblLIIndustryItems';
            txtSearchName = 'txtIndustrySearch';
            placeHolder = 'Enter Industry Name';
            break;
        case 4:
            uiListname = 'ulSubIndustryList';
            listarray = subindustryList;
            //selectedItemCount = ' (' + listarray.length + ' SubIndustry(s) selected)';
            liId = 'lblLISubIndustryItems';
            txtSearchName = 'txtSubIndustrySearch';
            placeHolder = 'Enter Sub-Industry Name';
            break;
        case 5:
            uiListname = 'ulAgencyList';
            listarray = agencyList;
            //selectedItemCount = ' (' + listarray.length + ' Agency(s) selected)';
            liId = 'lblLIAgencyItems';
            txtSearchName = 'txtAgencySearch';
            placeHolder = 'Enter Agency Name';
            break
        case 6:
            uiListname = 'ulSectorList';
            listarray = sectorList;
            //selectedItemCount = ' (' + listarray.length + ' Sector(s) selected)';
            liId = 'lblLISectorItems';
            txtSearchName = 'txtSectorSearch';
            placeHolder = 'Enter Sector Name';
            break;
        case 7:
            uiListname = 'ulOwnershipList';
            listarray = ownershipList;
            //selectedItemCount = ' (' + listarray.length + ' Ownership(s) selected)';
            break;

        case 8://keyword
            uiListname = 'ulKeywordList';
            listarray = keywordList;
            liId = 'lblLIKeywordItems';
            txtSearchName = 'txtKeywordSearch';
            placeHolder = 'Enter Keyword Name';
            break;
        case 10://country
            uiListname = 'ulCountryList';
            listarray = countryList;
            liId = 'lblLICountryItems';
            txtSearchName = 'txtCountrySearch';
            placeHolder = 'Enter Country Name';
            break;
    }

    $("#" + uiListname).empty();
    $("#" + uiListname).append("<li class='dropdown-header'></li>");
    if (type != 7) {
        $("#" + uiListname).append('<li>' + LoadSearchTextBox(txtSearchName, 'loadListBySearch(' + type + ',' + txtSearchName.toString() + ');', placeHolder, 'resetList(' + type + ',' + txtSearchName.toString() + ');') + '</li>');
        $("#" + uiListname).append("<li role='separator' class='dropdown-divider'></li>");
    }

    GetList('', false, type);
}
function loadListBySearch(type, txtSearchName) {
    var id = txtSearchName.id;
    var searchText = $('#' + id).val();
    if (searchText == null || searchText == '') {
        DisplayError('Please enter text to search');
        //alert('Please enter text for the search');
        return false;
    }
    GetList(searchText, false, type);

}
function resetList(type, txtSearchName) {
    var id = txtSearchName.id;
    $('#' + id).val('');
    $('#' + id).focus();
    GetList('', true, type);
}
function GetList(searchText, isReset, type) {
    var uiListname = '';
    var ulLiClass = '';
    var loadDataUrl = "/IndianTenders/LoadDataList";

    switch (type) {
        case 1:
            uiListname = 'ulStateList';
            ulLiClass = 'allstates';
            break;
        case 2:
            uiListname = 'ulCityList';
            ulLiClass = 'allcities';
            break;
        case 3:
            uiListname = 'ulIndustryList';
            ulLiClass = 'allindustries';
            break;
        case 4:
            uiListname = 'ulSubIndustryList';
            ulLiClass = 'allsubindustries';
            break;
        case 5:
            uiListname = 'ulAgencyList';
            ulLiClass = 'allagencies';
            break;
        case 6:
            uiListname = 'ulSectorList';
            ulLiClass = 'allsectors';
            break;
        case 7:
            uiListname = 'ulOwnershipList';
            ulLiClass = '';
            break;
        case 8:
            uiListname = 'ulKeywordList';
            ulLiClass = 'allKeywords';
            break;
        case 10:
            uiListname = 'ulCountryList';
            ulLiClass = 'allcountries';
            break;
    }
    if (type != 7) {
        $('#' + uiListname + ' li.' + ulLiClass + '').remove();
        $('#' + uiListname + '').append('<li class="dataloading">' + loadingItems() + '</li>');
    }
    $.ajax({
        type: 'POST',
        url: loadDataUrl,
        data: { searchName: searchText, isReset: isReset, type: parseInt(type) },
        cache: false,
        success: function (data) {
            $('#' + uiListname + ' li.dataloading').remove();
            var dataItems = data.DataList;
            var items = "";
            var checkedItems = "";
            var uncheckedItems = "";

            $.each(dataItems, function (i, item) {
                var ischecked = checkItemInArray(item.Value, type);
                var checkboxitem = "";
                if (ischecked) {
                    checkboxitem = "<input type='checkbox' checked='" + ischecked + "' onclick='fillArrayValue(this," + type + ");' value='" + item.Value + "' />";
                    checkedItems += "<li class='" + ulLiClass + "'><a href='javascript:void(0);' id='" + item.Value + "'>" + checkboxitem + " &nbsp;&nbsp; " + item.Text + "</a></li>";
                }
                else {
                    checkboxitem = "<input type='checkbox' onclick='fillArrayValue(this," + type + ");' value='" + item.Value + "' />";
                    uncheckedItems += "<li class='" + ulLiClass + "'><a href='javascript:void(0);' id='" + item.Value + "'>" + checkboxitem + " &nbsp;&nbsp; " + item.Text + "</a></li>";
                }
                //items += "<li class='" + ulLiClass + "'><a href='javascript:void(0);' id='" + item.Value + "'>" + checkboxitem + " &nbsp;&nbsp; " + item.Text + "</a></li>";
            });
            $("#" + uiListname).append(checkedItems).append(uncheckedItems);
        }
    });
}




function checkItemInArray(item, type) {
    var ischecked = true;

    switch (parseInt(type)) {
        case 1://STATE
            if (stateList.indexOf(item) === -1) {
                ischecked = false;
            }
            break;
        case 2://CITY
            if (cityList.indexOf(item) === -1) {
                ischecked = false;
            }
            break;
        case 3://INDUSTRY
            if (industryList.indexOf(item) === -1) {
                ischecked = false;
            }
            break;
        case 4://SUBINDUSTRY
            if (subindustryList.indexOf(item) === -1) {
                ischecked = false;
            }
            break;
        case 5://AGENCY
            if (agencyList.indexOf(item) === -1) {
                ischecked = false;
            }
            break;
        case 6://SECTOR
            if (sectorList.indexOf(item) === -1) {
                ischecked = false;
            }
            break;
        case 7://OWNERSHIP
            if (ownershipList.indexOf(item) === -1) {
                ischecked = false;
            }
            break;
        case 8://KEYWORDS
            if (keywordList.indexOf(item) === -1) {
                ischecked = false;
            }
            break;
        case 10://Country
            if (countryList.indexOf(item) === -1) {
                ischecked = false;
            }
            break;
    }

    return ischecked;
}
function fillArrayValue(e, type) {
    var id = e.value;
    var checked = e.checked;

    switch (parseInt(type)) {
        case 1://STATE
            if (checked) {
                if (stateList.indexOf(id) === -1) {
                    stateList.push(id);
                }
            }
            else {
                stateList.splice($.inArray(id, stateList), 1);
            }
            $('#SelectedState').val(stateList.join());

            var selectedStateItems = ' (' + stateList.length + ' State(s) selected)';
            $('#lblLIStateItems').html(selectedStateItems);
            break;
        case 2://CITY
            if (checked) {
                if (cityList.indexOf(id) === -1) {
                    cityList.push(id);
                }
            }
            else {
                cityList.splice($.inArray(id, cityList), 1);
            }
            $('#SelectedCity').val(cityList.join());

            var selectedCityItems = ' (' + cityList.length + ' City(s) selected)';
            $('#lblLICityItems').html(selectedCityItems);
            break;
        case 3://INDUSTRY
            if (checked) {
                if (industryList.indexOf(id) === -1) {
                    industryList.push(id);
                }
            }
            else {
                industryList.splice($.inArray(id, industryList), 1);
            }
            $('#SelectedIndustry').val(industryList.join());

            var selectedIndustryItems = ' (' + industryList.length + ' Industry(s) selected)';
            $('#lblLIIndustryItems').html(selectedIndustryItems);
            break;
        case 4://SUBINDUSTRY
            if (checked) {
                if (subindustryList.indexOf(id) === -1) {
                    subindustryList.push(id);
                }
            }
            else {
                subindustryList.splice($.inArray(id, subindustryList), 1);
            }
            $('#SelectedSubIndustry').val(subindustryList.join());

            var selectedSubIndustryItems = ' (' + subindustryList.length + ' SubIndustry(s) selected)';
            $('#lblLISubIndustryItems').html(selectedSubIndustryItems);
            break;
        case 5://AGENCY
            if (checked) {
                if (agencyList.indexOf(id) === -1) {
                    agencyList.push(id);
                }
            }
            else {
                agencyList.splice($.inArray(id, agencyList), 1);
            }
            $('#SelectedAgency').val(agencyList.join());

            var selectedAgencyItems = ' (' + agencyList.length + ' Agency(s) selected)';
            $('#lblLIAgencyItems').html(selectedAgencyItems);
            break;
        case 6://SECTOR
            if (checked) {
                if (sectorList.indexOf(id) === -1) {
                    sectorList.push(id);
                }
            }
            else {
                sectorList.splice($.inArray(id, sectorList), 1);
            }
            $('#SelectedSector').val(sectorList.join());

            var selectedSectorItems = ' (' + sectorList.length + ' Sector(s) selected)';
            $('#lblLISectorItems').html(selectedSectorItems);
            break;
        case 7://OWNERSHIP
            if (checked) {
                if (ownershipList.indexOf(id) === -1) {
                    ownershipList.push(id);
                }
            }
            else {
                ownershipList.splice($.inArray(id, ownershipList), 1);
            }
            $('#SelectedOwnership').val(ownershipList.join());
            break;
        case 8://PRODUCT
            if (checked) {
                if (keywordList.indexOf(id) === -1) {
                    keywordList.push(id);
                }
            }
            else {
                keywordList.splice($.inArray(id, keywordList), 1);
            }
            $('#SelectedProduct').val(keywordList.join());

            var selectedProductItems = ' (' + keywordList.length + ' Keyword(s) selected)';
            $('#lblLIKeywordItems').html(selectedProductItems);
            break;
        case 10://COUNTRY
            if (checked) {
                if (countryList.indexOf(id) === -1) {
                    countryList.push(id);
                }
            }
            else {
                countryList.splice($.inArray(id, countryList), 1);
            }
            $('#SelectedCountry').val(countryList.join());

            var selectedCountryItems = ' (' + countryList.length + ' Country(s) selected)';
            $('#lblLICountryItems').html(selectedCountryItems);
            break;
    }
}


function LoadSearchTextBox(textboxId, searchButtonClick, placeholder, resetButtonClick) {
    var control = '<div class="input-group input-group" style="padding:1px 3px;">' +
                    '<input class="f-left no-border ac_input" id="' + textboxId + '" name="' + textboxId + '" placeholder="' + placeholder + '" type="text" value="">' +
                    '<span class="input-group-btn">' +
                        '<button type="button" onclick="' + searchButtonClick + '" class="btn btn-primary SearchBtn searchsubmitbtn filterSearchBtn btn-sm" style="margin-right: 5px;">' +
                            '<span class="fa fa-search searchIcon"></span>' +
                        '</button>' +
                        '<button type="button" class="btn btn-warning searchsubmitbtn btn-sm" onclick="' + resetButtonClick + '">' +
                            '<span class="fa fa-refresh searchIcon"></span>' +
                        '</button>'
    '</span>' +
'</div>';
    return control;
}
function loadingItems() {
    var control = '<div style="padding:5px; text-align:cennter;">' +
        //'<span class="fa fa-spinner fa-spin text-danger"></span>' +
        //'<span style="margin: 1px 3px;" class="text-primary">Loading...</span>' +
        '<div class="spinner">' +
            '<div class="bounce1"></div>' +
            '<div class="bounce2"></div>' +
            '<div class="bounce3"></div>' +
        '</div>' +
    '</div>';
    return control;
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

function AdvanceSearchTenderList() {
    //This function called from right side Advance search panel from tender list page

    isAdvanceSearch = true;

    //SearchTenders();

    TenderBy = $('#hdnTendersBy').val(); FieldId = $('#hdnFieldId').val(); FieldName = $('#hdnFieldName').val();
    var stateids = $('#SelectedState').val();
    var cityids = $('#SelectedCity').val();
    var location = GenerateSingleLocation(stateids, cityids);
    $('#SelectedLocationIds').val(location);

    var industryids = $('#SelectedIndustry').val();
    var subindustryids = $('#SelectedSubIndustry').val();
    var indsubind = GenerateSingleIndustry(industryids, subindustryids);
    $('#SelectedIndustrySubIndustryIds').val(indsubind);

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
    var searchProductName = $("#hdnSearchtext").val();

    TenderYear = (TenderYear == '' || TenderYear == null) ? $('#hdnSelectedTenderYear').val() : TenderYear;

    var AdvanceSearchParameter = {
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
        TenderBy: TenderBy,
        SearchProductId: $("#searchProductID").val(),
        SearchProductName: searchProductName,
        WithinSearchedText: $('#hdnWithinSearchedText').val(),
    };
    var loadurl = '/IndianTenders/GetTenderFromAdvanceSearch/';
    $("#divTenderInfo").html('');
    ShowLoading();
    $.ajax({
        type: 'POST',
        url: loadurl,
        dataType: "json",
        contentType: 'application/json',
        data: JSON.stringify(AdvanceSearchParameter),
        success: function (data, textstatus) {
            if (data != '') {
                $("#divTenderInfo").html(data);
            }

            var tabCatLinks = document.getElementsByClassName("tabCatLinks");
            for (i = 0; i < tabCatLinks.length; i++) {
                tabCatLinks[i].className = tabCatLinks[i].className.replace("active", "");
            }
            $('.tabCatLinksSpans').text('');

            switch (parseInt(tenderStatusFlag)) {
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

            switch (parseInt(tenderStatusFlag)) {
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
$("#advcClearSearchbtn").click(function () {
    window.location.reload();
});

function AdvanceSearchTender() {
    //This function called from Advance search page

    isAdvanceSearch = true;
    var isValidForSearch = false;

    var searchType = $("#hdnSearchType").val();
    var searchTextData = $("#searchTextVal").val();
    var searchProductId = $("#searchProductID").val();

    var ourRefNo = $('#txtadvSearchRefNo').val();
    var stateids = $('#SelectedState').val();
    var cityids = $('#SelectedCity').val();
    var industryids = $('#SelectedIndustry').val();
    var subindustryids = $('#SelectedSubIndustry').val();
    var agencyIds = $('#SelectedAgency').val();
    var sectorIds = $('#SelectedSector').val();
    var ownershipIds = $('#SelectedOwnership').val();
    var tenderType = $('#hdnTenderType').val();
    var tenderStatusFlag = $('#hdnTenderStatus').val();
    var tenderValFlag = $('#hdnTenderValue').val();

    if (!isValidForSearch && searchProductId != undefined && parseInt(searchProductId) != 0)
    { isValidForSearch = true; }
    if (!isValidForSearch && searchTextData != "" && searchTextData != null && searchTextData != undefined)
    { isValidForSearch = true; }
    if (!isValidForSearch && ourRefNo != null && ourRefNo != "" && ourRefNo != undefined)
    { isValidForSearch = true; }
    if (!isValidForSearch && stateids != null && stateids != "" && stateids != undefined)
    { isValidForSearch = true; }
    if (!isValidForSearch && cityids != null && cityids != "" && cityids != undefined)
    { isValidForSearch = true; }
    if (!isValidForSearch && industryids != null && industryids != "" && industryids != undefined)
    { isValidForSearch = true; }
    if (!isValidForSearch && subindustryids != null && subindustryids != "" && subindustryids != undefined)
    { isValidForSearch = true; }
    if (!isValidForSearch && agencyIds != null && agencyIds != "" && agencyIds != undefined)
    { isValidForSearch = true; }
    if (!isValidForSearch && sectorIds != null && sectorIds != "" && sectorIds != undefined)
    { isValidForSearch = true; }
    if (!isValidForSearch && ownershipIds != null && ownershipIds != "" && ownershipIds != undefined)
    { isValidForSearch = true; }
    if (!isValidForSearch && tenderType != null && tenderType != "" && tenderType != "0" && tenderType != undefined)
    { isValidForSearch = true; }
    if (!isValidForSearch && tenderStatusFlag != null && tenderStatusFlag != "" && tenderStatusFlag != "0" && tenderStatusFlag != undefined)
    { isValidForSearch = true; }
    if (!isValidForSearch && tenderValFlag != null && tenderValFlag != "" && tenderValFlag != "0" && tenderValFlag != undefined)
    { isValidForSearch = true; }
    if (!isValidForSearch && SubDate_S != null && SubDate_S != "" && SubDate_S != undefined)
    { isValidForSearch = true; }
    if (!isValidForSearch && SubDate_E != null && SubDate_E != "" && SubDate_E != undefined)
    { isValidForSearch = true; }
    if (!isValidForSearch && OpenDate_S != null && OpenDate_S != "" && OpenDate_S != undefined)
    { isValidForSearch = true; }
    if (!isValidForSearch && OpenDate_E != null && OpenDate_E != "" && OpenDate_E != undefined)
    { isValidForSearch = true; }

    if (!isValidForSearch) {
        DisplayError('Please select any criteria for search');
        //alert('Please select any criteria for search');
        return false;
    }

    TenderBy = $('#hdnTendersBy').val(); FieldId = $('#hdnFieldId').val(); FieldName = $('#hdnFieldName').val();
    SearchTenders();


}

function AdvanceSearchTenderList_Global() {
    //This function called from right side Advance search panel from tender list page

    isAdvanceSearch = true;
    TenderBy = $('#hdnTendersBy').val(); FieldId = $('#hdnFieldId').val(); FieldName = $('#hdnFieldName').val();
    SearchGlobalTenders();
}

$('ul.dropdown-menu-class').on('click', function (event) {
    var events = $._data(document, 'events') || {};
    events = events.click || [];
    //debugger;
    //alert('drop');
    for (var i = 0; i < events.length; i++) {
        if (events[i].selector) {

            //Check if the clicked element matches the event selector
            if ($(event.target).is(events[i].selector)) {
                events[i].handler.call(event.target, event);
            }

            // Check if any of the clicked element parents matches the 
            // delegated event selector (Emulating propagation)
            $(event.target).parents(events[i].selector).each(function () {
                events[i].handler.call(this, event);
            });
        }
    }
    event.stopPropagation(); //Always stop propagation
});
