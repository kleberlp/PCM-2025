
const messagesData = document.getElementById('resource-messages').getAttribute('data-messages');
const messages = JSON.parse(messagesData);
const basePath = window.location.pathname.split("/").slice(0, 2).join("/");

$(document).ready(function () {

    // Reload DataTable
    $('#btnSave').click(function () {
        $("#form").submit();
    });

    jQuery.validator.addMethod("descricaoDB", function (value, element) {

        var unidade = ($("#unidade").val() == "") ? "-1" : $("#unidade").val();

        var dataString = {
            "unidade": messages.codigoUnidade,
            "descricao": value,
            "codigo": $("#codigo").val()
        };

        var result = false;

        jQuery.ajax({
            type: "POST",
            url: "ValidaTipoPerda",
            async: false,
            data: dataString,
            dataType: "json",
            success: function (return_data) {
                result = return_data;
            }
        });

        return result;

    }, "");

    $('#form').validate({
        onkeyup: false,
        onclick: false,
        ignore: [],
        errorClass: 'invalid-feedback animated fadeInDown',
        errorElement: 'div',
        errorPlacement: function (error, e) {
            jQuery(e).parents('.form-group > div').append(error);
        },
        highlight: function (e) {
            jQuery(e).closest('.form-group > div').removeClass('is-invalid').addClass('is-invalid');
        },
        success: function (e) {
            jQuery(e).closest('.form-group > div').removeClass('is-invalid');
            jQuery(e).remove();
        },
        rules: {
            'descricao': {
                required: true,
                descricaoDB: true
            }
        },
        messages: {
            'descricao': {
                required: messages.requiredField,
                descricaoDB: messages.validaBancoDados
            }
        },
        submitHandler: function (form) {

            return true;

        }
    });

});
