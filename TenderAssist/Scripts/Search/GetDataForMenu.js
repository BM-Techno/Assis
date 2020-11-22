$(document).ready(function () {
    if (isHomePage) {        
        GetDataList(1);
        GetCategoryList(StateId);
        GetCategoryList(CityId);
        GetCategoryList(IndustryId);
        GetCategoryList(SubIndustryId);
        GetCategoryList(AgencyId);
        GetCategoryList(SectorId);
        GetCategoryList(OwnershipId);
        GetCategoryList(KeywordId);
    }
    else { GetDataList(7, 1); }
});


function GetDataList(menuId, categoryId) {
    $("div#displayPanel").html('');
    if (menuId == 7)
    { ShowLoading(); }

    $.ajax({
        type: 'POST',
        url: loadDataList,
        cache: false,
        data: ({ menuBy: menuId, categoryBy: categoryId }),
        success: function (data) {
            $("div#displayPanel").html(data);

            var tablinks = document.getElementsByClassName("tablinks");
            for (i = 0; i < tablinks.length; i++) {
                tablinks[i].className = tablinks[i].className.replace("active", "");
            }

            switch (parseInt(menuId)) {
                case 1:
                    $('#linkSampleTenders').addClass('active');
                    break;
                case 2:
                    $('#linkResult').addClass('active');
                    break;
                case 3:
                    $('#linkProject').addClass('active');
                    break;
                case 5:
                    $('#linkFreeTender').addClass('active');
                    break;
                case 6:
                    $('#linkPrivateTender').addClass('active');
                    break;
                case 7:
                    HideLoading();
                    var tabCatLinks = document.getElementsByClassName("tabCatLinks");
                    for (i = 0; i < tabCatLinks.length; i++) {
                        tabCatLinks[i].className = tabCatLinks[i].className.replace("active", "");
                    }
                    switch (parseInt(categoryId)) {
                        case 1:
                            $('#linkLiveTenders').addClass('active');
                            break;
                        case 2:
                            $('#linkFreshTenders').addClass('active');
                            break;
                        case 3:
                            $('#linkArchiveTenders').addClass('active');
                            break;
                    }
                    break;
            }
        },
        error: function () {
            HideLoading();
            DisplayError('Error in loading data! Please try again.');
            //alert('error: loading data');
        }
    });
}



function GetCategoryList(categoryId) {
    $.ajax({
        type: 'POST',
        url: loadTendersByList,
        cache: false,
        data: ({ type: categoryId }),
        async: true,
        success: function (data) {
            var resultData = data.ResultLinks;
            switch (parseInt(categoryId)) {
                case StateId:
                    $('#stateList').html(resultData);
                    break;
                case CityId:
                    $('#cityList').html(resultData);
                    break;
                case IndustryId:
                    $('#indList').html(resultData);
                    break;
                case SubIndustryId:
                    $('#subIndList').html(resultData);
                    break;
                case AgencyId:
                    $('#agencyList').html(resultData);
                    break;
                case SectorId:
                    $('#sectorList').html(resultData);
                    break;
                case OwnershipId:
                    $('#ownershipList').html(resultData);
                    break;
                case KeywordId:
                    $('#keywordList').html(resultData);
                    break;
            }
        },
        error: function () {
            HideLoading();
            DisplayError('Error in loading category! Please try again.');
            //alert('error: loading category');
        }
    });
}