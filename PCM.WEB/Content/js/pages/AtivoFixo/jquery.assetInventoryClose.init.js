
const messagesData = document.getElementById('resource-messages').getAttribute('data-messages');
const messages = JSON.parse(messagesData);

var table = null;

$(document).ready(function () {

    Codebase.helpers(['datepicker', 'maxlength', 'select2']);

    carregarGrid();

});

// Atualiza contador: o widget countTo do Codebase cacheia o data-to inicial (0) e
// atropela o texto ao animar; aqui atualizamos .data/.attr/.text e re-disparamos
// a animação com o valor correto
function setCountTo(sel, val) {

    var $el = $(sel);

    $el.data("to", val).attr("data-to", val).text(val);

    if ($.fn.countTo) {
        $el.countTo({ from: 0, to: val, speed: 500, refreshInterval: 50 });
    }

}

function carregarGrid() {

    var data = {
        codigoInventario: $("#codigoInventario").val()
    };

    loadGridMain({
        tableId: "#tbAssetInventoried",
        data: data,
        endpoint: messages.urlLoadAssetInventoried,
        editAction: false,
        deleteAction: false,
        warningAction: false,
        customAction: false,
        enablePaging: true,
        pageLength: 15,
        enableSearch: true,
        enableExport: true,
        textSearch: messages.search,
        textNothingRegister: messages.nothingRegister,
        enableChild: false,
        onLoaded: function (response, rows) {
            setCountTo("#ativoInventariado", rows.length);
        }
    });

    loadGridMain({
        tableId: "#tbAssetNotFinded",
        data: data,
        endpoint: messages.urlLoadAssetNotFinded,
        editAction: false,
        deleteAction: false,
        warningAction: false,
        customAction: true,
        enablePaging: true,
        pageLength: 15,
        enableSearch: true,
        enableExport: true,
        textSearch: messages.search,
        textNothingRegister: messages.nothingRegister,
        enableChild: false,
        customButtons: [
            {
                action: "manutencao",
                icon: "fa fa-check",
                class: "btn btn-sm btn-secondary",
                title: messages.clickToManagerInventory,
                onClick: (row) => {
                    managerInventoryAsset(row);
                }
            }
        ],
        onLoaded: function (response, rows) {
            setCountTo("#ativoNaoEncontrado", rows.length);
        }
    });

    loadGridMain({
        tableId: "#tbAssetNotRegistered",
        data: data,
        endpoint: messages.urlLoadAssetNotRegistered,
        editAction: false,
        deleteAction: true,
        warningAction: false,
        customAction: false,
        enablePaging: true,
        pageLength: 15,
        enableSearch: true,
        enableExport: true,
        textSearch: messages.search,
        textNothingRegister: messages.nothingRegister,
        enableChild: false,
        onLoaded: function (response, rows) {
            setCountTo("#ativoNaoCadastrado", rows.length);
        }
    });

}

async function managerInventoryAsset(data) {

    $("#codigo").val($("#codigoInventario").val());
    $("#assetCode").val(data.asset_code);
    $("#modalManagerAsset").modal("show");
}

async function closeInventory() {

    const confirmed = await rfConfirm({
        title: messages.msgQuestionCloseInventory,
        message: messages.msgNotPossibleReverse,
        confirmButtonText: messages.yes,
        cancelButtonText: messages.no
    });

    if (confirmed) {

        jQuery.ajax({
            method: "POST",
            url: messages.urlCloseInventory,
            async: true,
            data: {
                "unidade": $("#unidade").val()
            },
            dataType: "json",
            success: async function (response) {

                if (response.success) {

                    await rfAlert({
                        title: response.message,
                        message: "",
                        icon: "success",
                        confirmButtonText: messages.ok
                    });

                    window.location = messages.urlAssetInventoryMng;

                } else {

                    await rfAlert({
                        title: response.message,
                        message: "",
                        icon: "error",
                        confirmButtonText: messages.ok
                    });
                }
            }
        });

    }

}

// Cancela o inventário aberto (ponto 1 do teste): confirma, cancela e volta ao gerenciamento
async function cancelInventory() {

    const confirmed = await rfConfirm({
        title: messages.msgQuestionCancelInventory,
        message: messages.msgNotPossibleReverse,
        confirmButtonText: messages.yes,
        cancelButtonText: messages.no
    });

    if (confirmed) {

        jQuery.ajax({
            method: "POST",
            url: messages.urlCancelInventory,
            async: true,
            data: {
                "unidade": $("#unidade").val()
            },
            dataType: "json",
            success: async function (response) {

                await rfAlert({
                    title: response.message,
                    message: "",
                    icon: response.success ? "success" : "error",
                    confirmButtonText: messages.ok
                });

                if (response.success) {
                    window.location = messages.urlAssetInventoryMng;
                }
            }
        });

    }

}

$('#setor').change(function () {
    loadCombo(messages.urlLoadListApartamento, () => ({ unidade: $('#unidade').val() || -1, setor: $('#setor').val() || -1 }), '#apartamento');
});

$('#tipoMovimentacao').change(function () {
    loadConfiguracaoTipoMovimentacao();
});

$("#btnSaveMovement").on("click", function () {

    if ($("#formMovement").valid()) {
        insertInventoryMovement();
    }

});

function Arquivo() {
    document.getElementById("lblArquivo").textContent = document.getElementById("arquivo").files[0].name;
}

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

$('#formMovement').on('submit', function (e) {
    e.preventDefault();
});

$('#formMovement').validate({

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

    submitHandler: async function (form) {

        await insertInventoryMovement();

        return false;
    }

});

async function insertInventoryMovement() {

    const confirmed = await rfConfirm({
        title: messages.msgQuestionConfirmMovement,
        message: messages.msgNotPossibleReverse,
        confirmButtonText: messages.yes,
        cancelButtonText: messages.no
    });

    if (confirmed) {

        let formData = new FormData();

        formData.append("codigo", $("#codigoInventario").val());
        formData.append("tipoMovimentacao", $("#tipoMovimentacao").val());
        formData.append("dataMovimentacao", $("#dataMovimentacao").val() || "");
        formData.append("documento", $("#documento").val() || "");
        formData.append("assetCode", $("#assetCode").val() || "");
        formData.append("setor", $("#setor").val() || -1);
        formData.append("apartamento", $("#apartamento").val() || -1);
        formData.append("fornecedor", $("#fornecedor").val() || -1);
        formData.append("valor", $("#valor").val() || "R$ 0");
        formData.append("observacao", $("#observacao").val() || "");

        const fileInput = $("#arquivo")[0];

        if (fileInput.files.length > 0) {
            formData.append("arquivo", fileInput.files[0]);
        }

        $.ajax({
            method: "POST",
            url: messages.urlInsertInventoryMovementAsset,
            data: formData,
            processData: false,
            contentType: false,
            cache: false,

            headers: {
                "RequestVerificationToken":
                    $('input[name="__RequestVerificationToken"]').val()
            },

            success: async function (response) {

                if (response.success) {

                    await rfAlert({
                        title: response.message,
                        message: "",
                        icon: "success",
                        confirmButtonText: messages.ok
                    });

                    carregarGrid();

                    $("#modalManagerAsset").modal("hide");

                } else {

                    await rfAlert({
                        title: response.message,
                        message: "",
                        icon: "error",
                        confirmButtonText: messages.ok
                    });
                }
            },

            error: async function (xhr) {

                await rfAlert({
                    title: "Erro",
                    message: xhr.responseText,
                    icon: "error",
                    confirmButtonText: messages.ok
                });

            }
        });

    }

}
