$(document).ready(function () {
    var PageType = $('#hdnPageType').val();

    if (PageType == 'ServiceListPage') {
        var assignedPermissions = $('#AssignedPermissions').val();
        if (assignedPermissions != '') {

            var listitems = assignedPermissions.split(',');
            for (var i = 0; i < listitems.length; i++) {
                var permission = listitems[i];
                if (permission != '' && permission != null && permission != undefined) {
                    
                    /*Get product items*/
                    GetPermissionWithCategory(parseInt(permission), 1);

                    /*Get country items*/
                    GetPermissionWithCategory(parseInt(permission), 10);
                }
            }
        }
    }
    else if (PageType == 'ServiceDetailPage') {
        var permission = $('#PermissionId').val();
        if (permission != '' && permission != null && permission != undefined) {

            /*Get product items*/
            GetPermissionWithCategory(parseInt(permission), 1);

            /*Get country items*/
            GetPermissionWithCategory(parseInt(permission), 10);
        }
    }

});

function ShowHideAssignedCategory(e) {
    //debugger;
    var id = e.id;
    var panelid = id.replace('_Link_', '_Panel_')

    if ($('#' + panelid).hasClass('hidden')) {
        $('#' + panelid).fadeIn("slow").removeClass('hidden');
        $('#' + panelid).fadeIn("slow").addClass('display-block');
        //$('#' + id).html('<label class="seperator">››</label> Hide');
        $('#' + id).html('<i class="fa fa-minus-circle"></i> Hide');
    }
    else {
        $('#' + panelid).fadeIn("slow").removeClass('display-block');
        $('#' + panelid).fadeIn("slow").addClass('hidden');
        //$('#' + id).html('<label class="seperator">››</label> Show');
        $('#' + id).html('<i class="fa fa-plus-circle"></i> Show');
    }
}
function GetPermissionWithCategory(permissionId, type) {

    var divId = '';
    var accessUrl = '';

    switch (parseInt(type)) {
        case 1://Keyword
            divId = "Keyword_Div_" + permissionId;
            accessUrl = '/User/GetPermissionWithProduct';
            break;
       
        case 10://Country
            divId = "Country_Div_" + permissionId;
            accessUrl = '/User/GetPermissionWithSubIndustry';
            break;

    }
    if (accessUrl != '') {
        $.ajax({
            type: 'GET',
            url: accessUrl,
            data: { permissionId: permissionId },
            cache: false,
            success: function (data) {
                if (data == null) {
                    $("#" + divId).html('');
                    $("#" + divId).hide();
                }
                else {
                    $("#" + divId).html(data);
                }
            }
        });
    }
}