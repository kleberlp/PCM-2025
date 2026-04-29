
const messagesData = document.getElementById('resource-messages').getAttribute('data-messages');
const messages = JSON.parse(messagesData);

var table = null;

$(document).ready(function () {

    carregarFiltro();
    carregarGrid();

    $('input, select').change(function () {
        salvarFiltro();
    });

    $('#filtrar').click(function () {
        salvarFiltro();
        carregarGrid();
    });

});

function carregarGrid() {

    var data = {
        unidade: $('#unidade').val(),
        codigo: $('#codigo').val(),
        descricao: $('#descricao').val(),
        status: $('#status').val(),
        localizacao: $('#localizacao').val()
    };

    loadGridMain({
        tableId: "#tbMain",
        data: data,
        endpoint: messages.urlLoadAsset,
        editAction: messages.editar,
        deleteAction: messages.excluir,
        warningAction: false,
        enablePaging: true,
        pageLength: 15,
        enableSearch: true,
        enableExport: true,
        textSearch: messages.search,
        textNothingRegister: messages.nothingRegister,
        enableChild: false,
        onEdit: (row) => {
            editRegister(row);
        },
        onDelete: (row) => {
            deleteRegister(row);
        }
    });

}

function salvarFiltro() {

    var filtro = {
        unidade: $('#unidade').val(),
        codigo: $('#codigo').val(),
        descricao: $('#descricao').val(),
        status: $('#status').val(),
        localizacao: $('#localizacao').val()
    };

    localStorage.setItem("asset_filtro", JSON.stringify(filtro));
}

function carregarFiltro() {

    var filtro = JSON.parse(localStorage.getItem("asset_filtro") || "{}");

    if (!filtro) return;

    $('#unidade').val(filtro.unidade || "");
    $('#codigo').val(filtro.codigo || "");
    $('#descricao').val(filtro.descricao || "");
    $('#status').val(filtro.status || "");
    $('#localizacao').val(filtro.localizacao || "");
}

function editRegister(data) {
    window.location = messages.urlAssetEdit + '?codigo=' + data.codigo;
}

async function deleteRegister(data) {

    const confirmed = await rfConfirm({
        title: messages.msgQuestionDelete,
        message: messages.msgNotPossibleReverse,
        confirmButtonText: messages.yes,
        cancelButtonText: messages.no
    });

    if (confirmed) {

        jQuery.ajax({
            method: "POST",
            url: messages.urlAssetDelete,
            async: true,
            data: {
                "codigo": data.codigo
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

                    carregarGrid();

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
