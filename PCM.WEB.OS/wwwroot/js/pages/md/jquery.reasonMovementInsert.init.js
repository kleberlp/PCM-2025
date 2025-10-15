
const messagesData = document.getElementById('resource-messages').getAttribute('data-messages');
const messages = JSON.parse(messagesData);
var jsonData = [];

document.addEventListener('DOMContentLoaded', function () {

    $(".select2").select2({ width: '100%', closeOnSelect: false });

    $('#btnSave').on('click', function () {

        $("#form").submit();

    });

});

$.validator.addMethod("validPK", function (value, element) {

    var result = false;

    jQuery.ajax({
        method: "POST",
        url: "isValidReasonMovement",
        async: false,
        data: {
            "code": value
        },
        dataType: "json",
        success: function (response) {
            result = response;
        }
    });

    return result;

}, messages.validPK);

$('#form').validate({
    ignore: [],
    errorClass: 'invalid-feedback animated fadeInDown',
    errorElement: 'div',
    onkeyup: false,
    onclick: false,
    errorPlacement: function (error, element) {
        if (element.hasClass('select2-hidden-accessible')) {
            error.insertAfter(element.next('span.select2-container'));
        }
        else {
            element.closest('.mb-9').append(error);
        }
    },
    highlight: function (element) {
        $(element).closest('.mb-9').find('.form-control, .form-select').addClass('is-invalid');
    },
    success: function (label, element) {
        $(element).closest('.mb-9').find('.is-invalid').removeClass('is-invalid');
        label.remove();
    },
    rules: {
        'code': {
            required: true,
            validPK: true
        },
        'description': {
            required: true
        },
        'active': {
            required: true
        }
    },
    messages: {
        'code': {
            required: messages.requiredField,
            validPK: messages.validPK
        },
        'description': {
            required: messages.requiredField
        },
        'active': {
            required: messages.requiredField
        }
    },
    submitHandler: function (form) {
        return true;
    },
    invalidHandler: function (e, validation) {
        $("#btnSave").prop('disabled', false);
    }
});
