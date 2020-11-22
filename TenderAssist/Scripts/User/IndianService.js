$(document).ready(function () {
    var PageType = $('#hdnPageType').val();

    if (PageType == 'ServiceListPage')
    {
        var assignedPermissions = $('#AssignedPermissions').val();
        if (assignedPermissions != '') {

            var listitems = assignedPermissions.split(',');
            for (var i = 0; i < listitems.length; i++) {
                var permission = listitems[i];
                if (permission != '' && permission != null && permission != undefined) {

                    /*Get product items*/
                    GetPermissionWithCategory(parseInt(permission), 1);

                    /*Get city items*/
                    GetPermissionWithCategory(parseInt(permission), 2);

                    /*Get state items*/
                    GetPermissionWithCategory(parseInt(permission), 3);

                    /*Get agency items*/
                    GetPermissionWithCategory(parseInt(permission), 4);

                    /*Get sector items*/
                    GetPermissionWithCategory(parseInt(permission), 5);

                    /*Get ownership items*/
                    GetPermissionWithCategory(parseInt(permission), 6);

                    /*Get industry items*/
                    GetPermissionWithCategory(parseInt(permission), 7);

                    /*Get subindustry items*/
                    GetPermissionWithCategory(parseInt(permission), 8);

                }
            }
        }
    }
    else if (PageType == 'ServiceDetailPage') {
        var permission = $('#PermissionId').val();
        if (permission != '' && permission != null && permission != undefined) {

            /*Get product items*/
            GetPermissionWithCategory(parseInt(permission), 1);

            /*Get city items*/
            GetPermissionWithCategory(parseInt(permission), 2);

            /*Get state items*/
            GetPermissionWithCategory(parseInt(permission), 3);

            /*Get agency items*/
            GetPermissionWithCategory(parseInt(permission), 4);

            /*Get sector items*/
            GetPermissionWithCategory(parseInt(permission), 5);

            /*Get ownership items*/
            GetPermissionWithCategory(parseInt(permission), 6);

            /*Get industry items*/
            GetPermissionWithCategory(parseInt(permission), 7);

            /*Get subindustry items*/
            GetPermissionWithCategory(parseInt(permission), 8);

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
        case 2://City
            divId = "City_Div_" + permissionId;
            accessUrl = '/User/GetPermissionWithCity';
            break;
        case 3://State
            divId = "State_Div_" + permissionId;
            accessUrl = '/User/GetPermissionWithState';
            break;
        case 4://Agency
            divId = "Agency_Div_" + permissionId;
            accessUrl = '/User/GetPermissionWithAgency';
            break;
        case 5://Sector
            divId = "Sector_Div_" + permissionId;
            accessUrl = '/User/GetPermissionWithSector';
            break;
        case 6://Ownership
            divId = "Ownership_Div_" + permissionId;
            accessUrl = '/User/GetPermissionWithOwnership';
            break;
        case 7://Industry
            divId = "Industry_Div_" + permissionId;
            accessUrl = '/User/GetPermissionWithIndustry';
            break;
        case 8://SubIndustry
            divId = "SubIndustry_Div_" + permissionId;
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