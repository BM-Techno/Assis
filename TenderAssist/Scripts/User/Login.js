$(document).ready(function () {
    $('#LoginUserDetails_strEmailId1').focus();
    $('#LoginUserDetails_strEmailId1').keydown(function (e) {
        if (e.keyCode == 13) {
            logindet();
        }
    });
    $('#LoginUserDetails_strPassword').keydown(function (e) {
        if (e.keyCode == 13) {
            logindet();
        }
    });
    $('#btnLogin').click(function () {
        logindet();
    });
});


function logindet() {
    var username = $('#LoginUserDetails_strEmailId1').val();
    var password = $('#LoginUserDetails_strPassword').val();

    $('.error_msg').html('');
    $(".textbox").removeClass("errorPointer");

    var isvalid = true;

    if (username == "") {
        $('#LoginUserDetails_strEmailId1').focus();


        $('#LoginUserDetails_strEmailId1').attr("title", "Username is required");
        $('#LoginUserDetails_strEmailId1').addClass("errorPointer");

        isvalid = false;
    }
    if (password == "") {
        $('#LoginUserDetails_strPassword').focus();


        $('#LoginUserDetails_strPassword').attr("title", "Password is required");
        $('#LoginUserDetails_strPassword').addClass("errorPointer");
        isvalid = false;
    }

    if (isvalid) {

        $('#btnLogin').attr("style", "display:none");
        $('#divWait').attr("style", "display:inline");
        $('#divWait').attr('disabled', 'disabled');

        //if (username == "nationaltender@gmail.com")
        {
            var errormsg = '';
            $.ajax({
                type: 'POST',
                url: "/User/CheckUserLogin",
                cache: false,
                data: ({ uName: username, uPswd: password }),
                success: function (data) {
                    var msg = data.msg;
                    if (msg == "error") {
                        if (data.username != "") {

                            $('#LoginUserDetails_strEmailId1').attr("title", data.username);
                            $('#LoginUserDetails_strEmailId1').addClass("errorPointer");

                            errormsg = data.username;
                        }
                        if (data.password != "") {

                            $('#LoginUserDetails_strPassword').attr("title", data.password);
                            $('#LoginUserDetails_strPassword').addClass("errorPointer");

                            errormsg = data.password;
                        }

                        if (errormsg == '') { errormsg = data.errormsg; }

                        $('#errmsg').html(errormsg);

                        $('#btnLogin').attr("style", "display:inline");
                        $('#divWait').attr("style", "display:none");
                        $('#divWait').attr('disabled', 'disabled');
                    }
                    else if (msg == "ok") {
                        /*Code to update user last login date*/
                        $.ajax({
                            type: 'POST',
                            url: "/User/UpdateUserLastLogin",
                            cache: false,
                            success: function (data) {
                                var status = data.status;
                                if (status == 'OK') {
                                    window.location.href = '/User/MyDashboard';
                                }
                            },
                            error: function () {
                                $('#btnLogin').attr("style", "display:inline");
                                $('#divWait').attr("style", "display:none");
                                $('#divWait').attr('disabled', 'disabled');

                                alert('Login error');
                            }
                        });
                    }
                }
            });
        }

    }
}