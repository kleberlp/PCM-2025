
const messagesData = document.getElementById('resource-messages').getAttribute('data-messages');
const messages = JSON.parse(messagesData);

jQuery(function () {
    
    Codebase.helpers(['datepicker', 'maxlength', 'select2']);

    //$("#valor").maskMoney({ prefix: 'R$ ', thousands: '.', decimal: ',', precision: 2, allowNegative: false, 'placeholder': '' });

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
        'unidade': { required: true },
        'tipoMovimentacao': { required: true },
        'dataMovimentacao': { required: true },
        'documento': { required: function () { return $('#documento').is(':enabled'); } },
        'asset': { required: true },
        'setor': { required: function () { return $('#setor').is(':enabled'); } },
        'fornecedor': { required: function () { return $('#fornecedor').is(':enabled'); } },
        'valor': { required: function () { return $('#valor').is(':enabled'); } }
    },

    messages: {
        'unidade': { required: messages.requiredField },
        'tipoMovimentacao': { required: messages.requiredField },
        'dataMovimentacao': { required: messages.requiredField },
        'documento': { required: messages.requiredField },
        'asset': { required: messages.requiredField },
        'setor': { required: messages.requiredField },
        'fornecedor': { required: messages.requiredField },
        'valor': { required: messages.requiredField }
    },

    submitHandler: function (form) {
        form.submit();
    }

});

$('#unidade').change(function () {
    loadCombo(messages.urlLoadListSetor, () => ({ unidade: $('#unidade').val() || -1 }), '#setor');
    loadCombo(messages.urlLoadListAsset, () => ({ unidade: $('#unidade').val() || -1 }), '#asset');
    $('#apartamento').empty().append('<option value=""></option>');
});

$('#setor').change(function () {
    loadCombo(messages.urlLoadListApartamento, () => ({ unidade: $('#unidade').val() || -1, setor: $('#setor').val() || -1 }), '#apartamento');
});

$('#tipoMovimentacao').change(function () {
    loadConfiguracaoTipoMovimentacao();
});

function loadConfiguracaoTipoMovimentacao() {

    if ($("#tipoMovimentacao").val() == "") {

        $("#documento").prop("disabled", true);
        $("#setor").prop("disabled", true);
        $("#apartamento").prop("disabled", true);
        $("#fornecedor").prop("disabled", true);
        $("#valor").prop("disabled", true);

    } else {

        $.ajax({
            type: "POST",
            url: messages.urlLoadConfiguracaoTipoMovimentacao,
            async: false,
            data: {
                "tipoMovimentacao": $("#tipoMovimentacao").val()
            },
            dataType: "json",
            success: async function (response) {

                if (response.success) {

                    $("#documento").prop("disabled", !response.documento);
                    $("#setor").prop("disabled", !response.setor);
                    $("#apartamento").prop("disabled", !response.apartamento);
                    $("#fornecedor").prop("disabled", !response.fornecedor);
                    $("#valor").prop("disabled", !response.valor);

                    if (!response.documento) $("#documento").val("");
                    if (!response.setor) $("#setor").val("");
                    if (!response.apartamento) $("#apartamento").val("");
                    if (!response.fornecedor) $("#fornecedor").val("");
                    if (!response.valor) $("#valor").val("");

                } else {
                    await rfAlert({
                        title: "ERRO",
                        message: response.message,
                        icon: "error",
                        confirmButtonText: messages.ok
                    });
                }

            },
            error: async function (data) {
                await rfAlert({
                    title: "ERRO",
                    message: data.statusText,
                    icon: "error",
                    confirmButtonText: messages.ok
                });
            }
        });

    }

}
