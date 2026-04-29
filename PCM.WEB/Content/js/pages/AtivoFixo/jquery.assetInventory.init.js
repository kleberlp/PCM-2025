
const messagesData = document.getElementById('resource-messages').getAttribute('data-messages');
const messages = JSON.parse(messagesData);

var table = null;

$(document).ready(function () {

    Codebase.helpers(['datepicker', 'maxlength', 'select2']);

});

$('#codigoSetor').change(function () {

    loadCombo(messages.urlLoadListApartamento, () => ({
        unidade: ($('#codigoUnidade').val() == "" ? "-1" : $('#codigoUnidade').val()),
        setor: ($('#codigoSetor').val() == "" ? "-1" : $('#codigoSetor').val())
    }), '#codigoApartamento');

    reloadGrid();

});

$('#codigoApartamento').change(function () {

    reloadGrid();

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
        'setor': { required: true },
        'barcode': { required: true }
    },

    messages: {
        'unidade': { required: messages.requiredField },
        'setor': { required: messages.requiredField },
        'barcode': { required: messages.requiredField }
    },

    submitHandler: async function (form, event) {

        event.preventDefault(); 

        const barcode = $("#barcode").val();

        let response = await validateAsset(barcode);

        // 1) Ativo NÃO existe
        if (!response.success) {

            const confirmed = await rfConfirm({
                title: messages.ativoNaoEncontrado,
                message: messages.msgQuestionRegistrarInventarioAtivo,
                confirmButtonText: messages.yes,
                cancelButtonText: messages.no
            });

            if (!confirmed) {
                $("#barcode").val('').focus();
                return false;
            }

            // Solicita descrição
            const result = await Swal.fire({
                title: messages.msgInformarDescricao,
                input: 'text',
                inputPlaceholder: messages.descricaoAtivoFixo,
                showCancelButton: true
            });

            if (result.dismiss === Swal.DismissReason.cancel || !result.value) {
                return false;
            }

            await insertInventory(false, result.value); // não cadastrado

            return false;
        }

        // 2) Ativo existe
        await insertInventory(true, null);

        return false;
    }

});

function validateAsset(assetCode) {

    return $.ajax({
        type: "POST",
        url: messages.urlValidaAsset,
        data: {
            unidade: $("#codigoUnidade").val(),
            assetCode: assetCode
        }
    });

}

function insertInventory(isValidAsset, descricao) {

    $.ajax({
        type: "POST",
        url: messages.urlInsertAssetInventory,
        data: {
            codigoInventario: $("#codigoInventario").val(), 
            unidade: $("#codigoUnidade").val(),
            setor: $("#codigoSetor").val(),
            apartamento: $("#codigoApartamento").val() || -1,
            assetCode: $("#barcode").val(),
            ativoCadastrado: isValidAsset,
            descricaoInformada: descricao
        },
        success: function (response) {

            if (response.success) {

                $("#barcode").val('').focus();

                reloadGrid();

            } else {

                rfAlert({
                    title: response.message,
                    icon: "error"
                });
            }

        }
    });
}

function openAssetModal(barcode) {

    Swal.fire({
        title: 'Cadastro de Ativo',
        html: `
            <input id="assetDescription" class="swal2-input" placeholder="Descrição">
        `,
        confirmButtonText: 'Salvar',
        showCancelButton: true,
        cancelButtonText: 'Cancelar',
        preConfirm: () => {

            return {
                descricao: document.getElementById('assetDescription').value
            }

        }
    }).then((result) => {

        if (result.isConfirmed) {

            insertNewAsset(barcode, result.value.descricao);

        }

    });
}

function insertNewAsset(barcode, descricao) {

    $.ajax({
        type: "POST",
        url: messages.urlInsertAsset, 
        async: false,
        data: {
            assetCode: barcode,
            descricao: descricao
        },
        success: async function (response) {

            if (response.success) {

                await insertInventory();

            } else {

                rfAlert({
                    title: response.message,
                    icon: "error"
                });

            }

        }
    });
}

function reloadGrid() {

    $.ajax({
        type: "POST",
        url: messages.urlLoadAssetInventory,
        data: {
            codigoInventario: $("#codigoInventario").val(), 
            unidade: $("#codigoUnidade").val(),
            setor: $("#codigoSetor").val(),
            apartamento: $("#codigoApartamento").val() || -1
        },
        success: function (data) {

            let tbody = $("#tbApp tbody");
            tbody.empty();

            data.forEach(x => {
                tbody.append(`
                    <tr class="${x.cssClass} font-size-sm">
                        <td class="text-center col-4">${x.asset}</td>
                        <td class="col-8">${x.descricao}</td>
                    </tr>
                `);
            });

        }
    });
}