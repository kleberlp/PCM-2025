
const messagesData = document.getElementById('resource-messages').getAttribute('data-messages');
const messages = JSON.parse(messagesData);

jQuery(function () {
    Codebase.helpers(['datepicker']);
});

$('#form').validate({

    onkeyup: false,
    onclick: false,
    ignore: ":hidden:not(select)",

    errorClass: 'invalid-feedback animated fadeInDown',
    errorElement: 'div',

    errorPlacement: function (error, e) {
        $(e).parents('.form-group > div').append(error);
    },

    highlight: function (e) {
        $(e).closest('.form-group > div').addClass('is-invalid');
    },

    success: function (e) {
        $(e).closest('.form-group > div').removeClass('is-invalid');
        $(e).remove();
    },

    rules: {
        'codigoUnidade': { required: true },
        'assetCode': { required: true },
        'descricao': { required: true }
    },

    messages: {
        'codigoUnidade': { required: messages.requiredField },
        'assetCode': { required: messages.requiredField },
        'descricao': { required: messages.requiredField }
    },

    submitHandler: function (form) {
        form.submit();
    }

});

$('#codigoUnidade').change(function () {

    loadCombo(messages.urlLoadListSetor, () => ({
        unidade: ($('#codigoUnidade').val() == "" ? "-1": $('#codigoUnidade').val())
    }), '#codigoSetor');

    // limpa apartamento
    $('#codigoApartamento').empty().append('<option value=""></option>');
});

$('#codigoSetor').change(function () {

    loadCombo(messages.urlLoadListApartamento, () => ({
        unidade: ($('#codigoUnidade').val() == "" ? "-1" : $('#codigoUnidade').val()),
        setor: ($('#codigoSetor').val() == "" ? "-1" : $('#codigoSetor').val())
    }), '#codigoApartamento');

});
