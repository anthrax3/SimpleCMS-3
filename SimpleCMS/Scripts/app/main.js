// Main js for Simple CMS

// functions 
var logoutUser = function () {
    $.ajax({
        url: "api/Account/Logout",
        type: "POST",
        success: function (data) {
            window.location.reload();
        }
    })
}

// jQuery stuff 
$(function () {
    $("#logoutUser").click(function () {
        logoutUser(); 
    });
});


