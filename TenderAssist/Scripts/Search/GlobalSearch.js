$(document).ready(function () {
    if (isHomePage) {
        GetGlobalTenderList(1);
    }
});

function GetGlobalTenderList(tenderStatus) {
    $("div#displayPanel").html('');
    ShowLoading();

    $.ajax({
        type: 'POST',
        url: loadDataList,
        cache: false,
        data: ({ tenderStatus: tenderStatus }),
        success: function (data) {
            HideLoading();

            $("div#displayPanel").html(data);

            var tabCatLinks = document.getElementsByClassName("tabCatLinks");
            for (i = 0; i < tabCatLinks.length; i++) {
                tabCatLinks[i].className = tabCatLinks[i].className.replace("active", "");
            }
            switch (parseInt(tenderStatus)) {
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
        },
        error: function () {
            HideLoading();
            DisplayError('Error in loading data! Please try again.');
        }
    });
}

function LoadRegionDataList(menuId) {
    var tablinks = document.getElementsByClassName("tablinks");
    for (i = 0; i < tablinks.length; i++) {
        tablinks[i].className = tablinks[i].className.replace("active", "");
    }
    $('#linkregion_' + menuId).addClass('active');

    var divlinks = document.getElementsByClassName("regionPanel");
    for (i = 0; i < divlinks.length; i++) {
        divlinks[i].classList.add('hide')
    }
    $('#region_' + menuId).removeClass('hide');

}