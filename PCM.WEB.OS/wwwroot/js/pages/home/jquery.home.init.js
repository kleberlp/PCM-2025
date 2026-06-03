
$(function () {

    if ($("#message").val() != "") {

        Swal.fire({
            title: $("#message").val(),
            icon: "success",
            showDenyButton: false,
            showCancelButton: false,
        });
    }

});
