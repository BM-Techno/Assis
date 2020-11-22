var stateList = [];
var cityList = [];
var industryList = [];
var subindustryList = [];
var agencyList = [];
var sectorList = [];
var ownershipList = [];
var countryList = [];
var productList = [];

var isWithinSearch = false;

var IsIndianGlobal ='true';

$(document).ready(function () {
    IsIndianGlobal = $('#hdnIsIndianGlobal').val();

    $('#filterusersearchpanel').hide();
    $("#filterusersearch").click(function () {
        //$('#filterusersearchpanel').removeClass('hide');
        $('#filterusersearchpanel').slideToggle('slow');
    });


    $("#withinsearch").click(function ()
    { advanceSearchtenders(); });

    $("#btnLoadMoreData").click(function () {
        LoadMoreData();
    });

    $("#btnWithinSearchData").click(function () {
        isWithinSearch = true;
        SearchUserTenders();
    });

    $("#advcClearSearchbtn").click(function () {
        ShowLoading();
        ClearSearchFilters();
        $.ajax({
            type: 'POST',
            url: "/User/ClearUserSession",
            cache: false,
            success: function () {
                window.location.reload();
                HideLoading();
            }
        });
    });

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



    $("#txtadvSearchTenderValue").checkNumericOnly();
    $("#txtadvSearchRefNo").checkNumericOnly();

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
        setSearchTypeWithName(dispTxt, regionId, '#lblCountryRegion', '#hdnRegionId');
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
    if ($('#hdnRegionId').val() != '' && $('#hdnRegionId').val() != undefined && $('#hdnRegionId').val() != '0') {
        setSearchTypeWithName($('#hdnRegionText').val(), $('#hdnRegionId').val(), '#lblCountryRegion', '#hdnRegionId');
    }
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
            liId = 'lblLICityItems';
            txtSearchName = 'txtStateSearch';
            placeHolder = 'Enter State Name';
            break;
        case 2:
            uiListname = 'ulCityList';
            listarray = cityList;
            liId = 'lblLIStateItems';
            txtSearchName = 'txtCitySearch';
            placeHolder = 'Enter City Name';
            break;
        case 3:
            uiListname = 'ulIndustryList';
            listarray = industryList;
            liId = 'lblLIIndustryItems';
            txtSearchName = 'txtIndustrySearch';
            placeHolder = 'Enter Industry Name';
            break;
        case 4:
            uiListname = 'ulSubIndustryList';
            listarray = subindustryList;
            liId = 'lblLISubIndustryItems';
            txtSearchName = 'txtSubIndustrySearch';
            placeHolder = 'Enter Sub-Industry Name';
            break;
        case 5:
            uiListname = 'ulAgencyList';
            listarray = agencyList;
            liId = 'lblLIAgencyItems';
            txtSearchName = 'txtAgencySearch';
            placeHolder = 'Enter Agency Name';
            break
        case 6:
            uiListname = 'ulSectorList';
            listarray = sectorList;
            liId = 'lblLISectorItems';
            txtSearchName = 'txtSectorSearch';
            placeHolder = 'Enter Sector Name';
            break;
        case 7:
            uiListname = 'ulOwnershipList';
            listarray = ownershipList;
            break;

        case 8:
            uiListname = 'ulKeywordList';
            listarray = productList;
            liId = 'lblLIProductItems';
            txtSearchName = 'txtProductSearch';
            placeHolder = 'Enter Product Name';
            break;
        case 10:
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
        alert('Please enter text for the search');
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
    var loadDataUrl = "";
    var PermissionId = $('#hdnPermissionId').val();
    switch (type) {
        case 1:
            uiListname = 'ulStateList';
            ulLiClass = 'allstates';
            loadDataUrl = "/User/LoadStateList";
            break;
        case 2:
            uiListname = 'ulCityList';
            ulLiClass = 'allcities';
            loadDataUrl = "/User/LoadCityList";
            break;
        case 3:
            uiListname = 'ulIndustryList';
            ulLiClass = 'allindustries';
            loadDataUrl = "/User/LoadIndustryList";
            break;
        case 4:
            uiListname = 'ulSubIndustryList';
            ulLiClass = 'allsubindustries';
            loadDataUrl = "/User/LoadSubIndustryList";
            break;
        case 5:
            uiListname = 'ulAgencyList';
            ulLiClass = 'allagencies';
            loadDataUrl = "/User/LoadAgencyList";
            break;
        case 6:
            uiListname = 'ulSectorList';
            ulLiClass = 'allsectors';
            loadDataUrl = "/User/LoadSectorList";
            break;
        case 7:
            uiListname = 'ulOwnershipList';
            ulLiClass = '';
            loadDataUrl = "/User/LoadOwnershipList";
            break;

        case 8:
            uiListname = 'ulKeywordList';
            ulLiClass = 'allproducts';
            loadDataUrl = "/User/LoadKeywordList";
            break;
        case 10:
            uiListname = 'ulCountryList';
            ulLiClass = 'allcountries';
            loadDataUrl = "/User/LoadCountryList";
            break;
    }
    if (type != 7) {
        $('#' + uiListname + ' li.' + ulLiClass + '').remove();
        $('#' + uiListname + '').append('<li class="dataloading">' + loadingItems() + '</li>');
    }
    $.ajax({
        type: 'POST',
        url: loadDataUrl,
        data: { permissionId: parseInt(PermissionId), searchName: searchText, isReset: isReset },
        cache: false,
        success: function (data) {
            $('#' + uiListname + ' li.dataloading').remove();
            var dataItems = data.DataList;
            var items = "";

            $.each(dataItems, function (i, item) {
                var ischecked = checkItemInArray(item.Value, type);
                var checkboxitem = "";
                if (ischecked) {
                    checkboxitem = "<input type='checkbox' checked='" + ischecked + "' onclick='fillArrayValue(this," + type + ");' value='" + item.Value + "' />";
                }
                else {
                    checkboxitem = "<input type='checkbox' onclick='fillArrayValue(this," + type + ");' value='" + item.Value + "' />";
                }
                items += "<li class='" + ulLiClass + "'><a href='javascript:void(0);' id='" + item.Value + "'>" + checkboxitem + " &nbsp;&nbsp; " + item.Text + "</a></li>";
            });
            $("#" + uiListname).append(items);
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

        case 8://PRODUCT
            if (productList.indexOf(item) === -1) {
                ischecked = false;
            }
            break;
        case 10://COUNTRY
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
            $('#lblLIStateItems').text("All States " + selectedStateItems);
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
            $('#lblLICityItems').text("All Cities " + selectedCityItems);
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
            $('#lblLIIndustryItems').text("All Industries " + selectedIndustryItems);
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
            $('#lblLISubIndustryItems').text("All SubIndustry " + selectedSubIndustryItems);
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
            $('#lblLIAgencyItems').text("All Agencies " + selectedAgencyItems);
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
            $('#lblLISectorItems').text("All Sectors " + selectedSectorItems);
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
                if (productList.indexOf(id) === -1) {
                    productList.push(id);
                }
            }
            else {
                productList.splice($.inArray(id, productList), 1);
            }
            $('#SelectedProduct').val(productList.join());

            var selectedProductItems = ' (' + productList.length + ' Product(s) selected)';
            $('#lblLIProductItems').text("All Products " + selectedProductItems);
            break;
        case 10://country
            if (checked) {
                if (countryList.indexOf(id) === -1) {
                    countryList.push(id);
                }
            }
            else {
                countryList.splice($.inArray(id, countryList), 1);
            }
            $('#SelectedCountry').val(countryList.join());
            break;
    }
}

function LoadSearchTextBox(textboxId, searchButtonClick, placeholder, resetButtonClick) {
    var control = '<div class="input-group input-group" style="padding:1px 3px;">' +
                    '<input class="f-left no-border ac_input" id="' + textboxId + '" name="' + textboxId + '" placeholder="' + placeholder + '" type="text" value="">' +
                    '<span class="input-group-btn">' +
                        '<button type="button" onclick="' + searchButtonClick + '" class="btn btn-info SearchBtn searchsubmitbtn btn-sm filterSearchBtn">' +
                            '<span class="fa fa-search searchIcon"></span>' +
                        '</button>' +
                        '<button type="button" class="btn btn-danger searchsubmitbtn btn-sm" onclick="' + resetButtonClick + '">' +
                            '<span class="fa fa-refresh searchIcon"></span>' +
                        '</button>'
    '</span>' +
'</div>';
    return control;
}
function loadingItems() {
    var control = '<div style="padding:5px; text-align:cennter;">' +
        '<span class="fa fa-spinner fa-spin text-danger"></span>' +
        '<span style="margin: 1px 3px;" class="text-primary">Loading...</span>' +
    '</div>';
    return control;
}


function AdvanceSearchTenderList() {
    //This function called from right side Advance search panel from tender list page
    if (!AdvanceSearchValidation()) {
        alert('Please select any criteria for search');
        return false;
    }
    SearchUserTenders();
}
function AdvanceSearchValidation() {
    var isValidForSearch = false;

    var ourRefNo = $('#txtadvSearchRefNo').val();
    var stateids = $('#SelectedState').val();
    var cityids = $('#SelectedCity').val();
    var industryids = $('#SelectedIndustry').val();
    var subindustryids = $('#SelectedSubIndustry').val();
    var agencyIds = $('#SelectedAgency').val();
    var sectorIds = $('#SelectedSector').val();
    var ownershipIds = $('#SelectedOwnership').val();
    var productids = $('#SelectedProduct').val();
    var countryids = $('#SelectedCountry').val();

    var tenderType = $('#hdnTenderType').val();
    var tenderStatusFlag = $('#hdnTenderStatus').val();
    var tenderValFlag = $('#hdnTenderValue').val();


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
    if (!isValidForSearch && productids != null && productids != "" && productids != undefined)
    { isValidForSearch = true; }
    if (!isValidForSearch && countryids != null && countryids != "" && countryids != undefined)
    { isValidForSearch = true; }


    if (!isValidForSearch && tenderType != null && tenderType != "" && tenderType != "0" && tenderType != undefined)
    { isValidForSearch = true; }
    if (!isValidForSearch && tenderStatusFlag != null && tenderStatusFlag != "" && tenderStatusFlag != "0" && tenderStatusFlag != undefined)
    { isValidForSearch = true; }
    if (!isValidForSearch && tenderValFlag != null && tenderValFlag != "" && tenderValFlag != "0" && tenderValFlag != undefined)
    { isValidForSearch = true; }


    var tenderBy = 0;
    //debugger;
    if (IsIndianGlobal == 'False') {
        tenderBy = $('#hdnRegionId').val();
        if (!isValidForSearch && tenderBy != null && tenderBy != "" && tenderBy != "0" && tenderBy != undefined)
        { isValidForSearch = true; }
    }

    return isValidForSearch;
}
function SearchUserTenders() {

    var searchTextData = '';
    if (!isWithinSearch) {
        searchTextData = $("#searchTextVal").val();
    }
    else {
        searchTextData = $("#withinSearchText").val();
    }

    var stateids = $('#SelectedState').val();
    var cityids = $('#SelectedCity').val();
    var location = GenerateSingleLocation(stateids, cityids);
    $('#SelectedLocationIds').val(location);

    var industryids = $('#SelectedIndustry').val();
    var subindustryids = $('#SelectedSubIndustry').val();
    var indsubind = GenerateSingleIndustry(industryids, subindustryids);
    $('#SelectedIndustrySubIndustryIds').val(indsubind);


    var productIds = $('#SelectedProduct').val();
    var agencyIds = $('#SelectedAgency').val();
    var sectorIds = $('#SelectedSector').val();
    var ownershipIds = $('#SelectedOwnership').val();
    var countryIds = $('#SelectedCountry').val();


    var searchType = $("#hdnSearchType").val();
    var ourRefNo = $('#txtadvSearchRefNo').val();
    var tenderType = $('#hdnTenderType').val();
    var tenderStatusFlag = $('#hdnTenderStatus').val();
    var tenderValFlag = $('#hdnTenderValue').val();
    OrderBy = $('#hdnOrderBy').val();

    var AdvanceSearchPara = {
        SelectedStates: stateids,
        SelectedCities: cityids,
        SelectedLocations: location,
        SelectedIndustries: industryids,
        SelectedSubIndustries: subindustryids,
        SelectedIndsubIndustries: indsubind,
        SelectedProducts: productIds,
        SelectedAgencies: agencyIds,
        SelectedSectors: sectorIds,
        SelectedOwnerships: ownershipIds,
        SelectedCountries: countryIds,
        TenderValFlag: tenderValFlag,
        OurRefNo: ourRefNo,
        TenderTypeId: tenderType,
        TenderStatusFlag: tenderStatusFlag,
        IcbNcb: -1,
        SearchProductId: $("#searchProductID").val()
    };

    var tenderBy = 0;
    //debugger;
    if (IsIndianGlobal == 'False') { tenderBy = $('#hdnRegionId').val(); }


    var SearchUserTenderModel = {
        PermissionId: parseInt($('#hdnPermissionId').val()),
        TenderBy: tenderBy,
        Page: 1,
        Search: searchTextData,
        SType: searchType,
        IsFirst: 0,
        SearchProductId: $("#searchProductID").val(),
        AdvanceSearch: AdvanceSearchPara,
        OrderBy: OrderBy,
        OtherKeywordText: ""
    };

    ShowLoading();
    $("#divTenderInfo").html('');
    $.ajax({
        url: '/User/SearchTenders/',
        type: 'POST',
        dataType: "json",
        contentType: 'application/json',
        data: JSON.stringify(SearchUserTenderModel),
        success: function (returndata) {
            RedirectToSearch(returndata);
        },
        error: function (returndata) {
            RedirectToSearch(returndata);
        }
    });
    return true;
}
function RedirectToSearch(returndata) {
    HideLoading();
    var url = returndata.newurl;
    window.location = url;
}




function setSearchTypeWithName(dispTxt, searchType, fildId, hdnFieldId) {
    $(fildId).text(dispTxt);
    $(hdnFieldId).val(searchType);
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
$('ul.dropdown-menu-class').on('click', function (event) {
    var events = $._data(document, 'events') || {};
    events = events.click || [];

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



function CategorySearch(type, Id, permissionId) {
    ClearSearchFilters();
    switch (parseInt(type)) {
        case 1://State
            $('#SelectedState').val(Id);
            break;
        case 2://City
            $('#SelectedCity').val(Id);
            break;
        case 3://Industry
            $('#SelectedIndustry').val(Id);
            break;
        case 4://SubIndustry
            $('#SelectedSubIndustry').val(Id);
            break;
        case 5://Agency
            $('#SelectedAgency').val(Id);
            break;
        case 6://Sector
            $('#SelectedSector').val(Id);
            break;
        case 7://Ownership
            $('#SelectedOwnership').val(Id);
            break;

        case 8://Keyword
            $('#SelectedProduct').val(Id);
            break;
        case 10://Global Country
            $('#SelectedCountry').val(Id);
            break;
    }
    $('#hdnPermissionId').val(permissionId);
    $.ajax({
        type: 'POST',
        url: "/User/ClearUserSession",
        cache: false,
        success: function () {
            SearchUserTenders();
        }
    });

}


function ClearSearchFilters() {
    $("#withinSearchText").val('');
    $('#SelectedState').val('');
    $('#SelectedCity').val('');
    $('#SelectedLocationIds').val('');
    $('#SelectedIndustry').val('');
    $('#SelectedSubIndustry').val('');
    $('#SelectedIndustrySubIndustryIds').val('');
    $('#SelectedProduct').val('');
    $('#SelectedAgency').val('');
    $('#SelectedSector').val('');
    $('#SelectedOwnership').val('');
    $('#SelectedCountry').val('');
    $("#hdnSearchType").val('');
    $('#txtadvSearchRefNo').val('');
    $('#hdnTenderType').val('');
    $('#hdnTenderStatus').val('');
    $('#hdnTenderValue').val('');
    $('#hdnOrderBy').val('3');
    $("#searchProductID").val('');
}

function LoadMoreData() {
    var PermissionId = $('#PermissionId').val();
    if (NewPage > -1 && NewPage <= TotalPage) {
        console.log('NewPage - ' + NewPage);
        ShowLoading();

        $.ajax({
            type: 'GET',
            url: '/User/GetTenderResultOnLoading/',
            //async:false,
            data: "permissionId=" + PermissionId + "&&page=" + NewPage,
            success: function (data, textstatus) {
                if (data != '') {
                    $("#divTenderInfo").append(data);
                }
                else {
                    NewPage = -1;
                }
                HideLoading();
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert(errorThrown);
            }
        });
    }
}
function ChangeTenderStatus(tenderstatus) {
    var loadurl = '/User/GetTenderFromTenderStatus/';
    var PermissionId = $('#PermissionId').val();
    $('#hdnTenderStatus').val(tenderstatus);
    $("#divTenderInfo").html('');
    ShowLoading();
    $.ajax({
        type: 'GET',
        url: loadurl,
        data: "permissionId=" + PermissionId + "&&tenderStatus=" + parseInt(tenderstatus),
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
                    $('#lblAll').html('<small><b>(' + Total + ')</b></small>');
                    if (Total >= 10)
                    { $('#divloadmore').show(); }
                    else
                    { $('#divloadmore').hide(); }
                    break;
                case 1:
                    $('#linkLiveTenders').addClass('active');
                    $('#lblLive').html('<small><b>(' + TotalLive + ')</b></small>');
                    if (TotalLive >= 10)
                    { $('#divloadmore').show(); }
                    else
                    { $('#divloadmore').hide(); }
                    break;
                case 2:
                    $('#linkFreshTenders').addClass('active');
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
            alert(errorThrown);
        }
    });
}