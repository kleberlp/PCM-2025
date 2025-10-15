
const messagesData = document.getElementById('resource-messages').getAttribute('data-messages');
const messages = JSON.parse(messagesData);


$(document).ready(function () {

    $("#postalCode").mask("99.999-999");
    $("#taxId").mask("99.999.999/9999-99");
    $("#phoneNumber").mask("(99) 9999-9999");

    $('#postalCode').on('blur', function () {

        var cep = $(this).val().replace(/\D/g, '');

        if (cep.length === 8) {

            $("#street").val("");
            $("#district").val("");
            $("#city").val("");
            $("#province").val("");

            $.ajax({
                url: `https://viacep.com.br/ws/${cep}/json/`,
                type: 'GET',
                dataType: 'json',
                success: function (data) {
                    if (!data.erro) {
                        // Preenche os campos com os dados do CEP
                        $('#street').val(data.logradouro);
                        $('#district').val(data.bairro);
                        $('#city').val(data.localidade);
                        $('#province').val(data.uf);
                    } else {
                        Swal.fire({
                            icon: 'error',
                            title: 'Erro',
                            text: messages.validCep,
                        });
                        $("#postalCode").val("");
                    }
                },
                error: function () {
                    Swal.fire({
                        icon: 'error',
                        title: 'Erro',
                        text: messages.validCep,
                    });
                    $("#postalCode").val("");
                }
            });
        } else if (cep.length > 0) {
            Swal.fire({
                icon: 'warning',
                title: messages.warning,
                text: messages.validCep,
            });
            $("#postalCode").val("");
        }
    });

    $('#btnSaveUser').on('click', function () {

        $("#formUser").submit();

    });

    $('#btnSave').on('click', function () {

        $("#form").submit();

    });

});

$.validator.addMethod("validateCNPJ", function (cnpj, element) {

    cnpj = cnpj.replace(/[^\d]/g, '');

    var numeros, digitos, soma, resultado, pos, tamanho,
        digitos_iguais = true;

    if (cnpj.length < 14 && cnpj.length > 15)
        return false;

    for (var i = 0; i < cnpj.length - 1; i++)
        if (cnpj.charAt(i) != cnpj.charAt(i + 1)) {
            digitos_iguais = false;
            break;
        }

    if (!digitos_iguais) {
        tamanho = cnpj.length - 2
        numeros = cnpj.substring(0, tamanho);
        digitos = cnpj.substring(tamanho);
        soma = 0;
        pos = tamanho - 7;

        for (i = tamanho; i >= 1; i--) {
            soma += numeros.charAt(tamanho - i) * pos--;
            if (pos < 2)
                pos = 9;
        }

        resultado = soma % 11 < 2 ? 0 : 11 - soma % 11;

        if (resultado != digitos.charAt(0))
            return false;

        tamanho = tamanho + 1;
        numeros = cnpj.substring(0, tamanho);
        soma = 0;
        pos = tamanho - 7;

        for (i = tamanho; i >= 1; i--) {
            soma += numeros.charAt(tamanho - i) * pos--;
            if (pos < 2)
                pos = 9;
        }

        resultado = soma % 11 < 2 ? 0 : 11 - soma % 11;

        if (resultado != digitos.charAt(1))
            return false;

        return true;
    }

    return false;
}, messages.validCNPJ);

$('#form').validate({
    ignore: [],
    errorClass: 'invalid-feedback animated fadeInDown',
    errorElement: 'div',
    onkeyup: true,
    onclick: false,
    onfocusout: false,
    errorPlacement: function (error, e) {
        jQuery(e).parents('.form-group > div > div').append(error);
    },
    highlight: function (e) {
        jQuery(e).closest('.form-group > div > div').removeClass('is-invalid').addClass('is-invalid');
    },
    success: function (e) {
        jQuery(e).closest('.form-group > div > div').removeClass('is-invalid');
        jQuery(e).remove();
    },
    rules: {
        'branch': {
            required: true
        },
        'taxId': {
            required: true,
            validateCNPJ: true
        },
        'partnerId': {
            required: true
        },
        'partnerName': {
            required: true
        },
        'postalCode': {
            required: true
        },
        'number': {
            required: true
        }
    },
    messages: {
        'branch': {
            required: messages.requiredField
        },
        'taxId': {
            required: messages.requiredField,
            validateCNPJ: messages.validCNPJ
        },
        'partnerId': {
            required: messages.requiredField
        },
        'partnerName': {
            required: messages.requiredField
        },
        'postalCode': {
            required: messages.requiredField
        },
        'number': {
            required: messages.requiredField
        }
    },
    submitHandler: function (form) {
        $('#street').prop('disabled', false);
        $('#district').prop('disabled', false);
        $('#city').prop('disabled', false);
        $('#province').prop('disabled', false);
        return true;
    },
    invalidHandler: function (e, validation) {
        $("#btnSave").prop('disabled', false);
    }
});
