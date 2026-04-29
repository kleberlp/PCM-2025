const messagesData = document.getElementById('resource-messages').getAttribute('data-messages');
const messages = JSON.parse(messagesData);

var table = null;

$(document).ready(function () {

    Codebase.helpers(['datepicker', 'maxlength', 'select2']);

    carregarFiltro();
    carregarGrid();

    $('#filtrar').click(function () {
        salvarFiltro();
        carregarGrid();
    });

});

function carregarGrid() {

    var data = {
        unidade: $('#unidade').val() || -1,
        tipoMovimentacao: $('#tipoMovimentacao').val() || -1,
        assetCode: $('#assetCode').val(),
        documento: $('#documento').val(),
        dataInicio: $('#dataInicio').val(),
        dataTermino: $('#dataTermino').val(),
        origem: $('#origem').val(),
        destino: $('#destino').val()
    };

    loadGridMain({
        tableId: "#tbMain",
        data: data,
        endpoint: messages.urlLoadAssetMovement,
        editAction: false,
        deleteAction: false,
        warningAction: false,
        enablePaging: true,
        pageLength: 15,
        enableSearch: true,
        enableExport: true,
        textSearch: messages.search,
        textNothingRegister: messages.nothingRegister,
        enableChild: false
    });

}

function salvarFiltro() {

    var filtro = {
        unidade: $('#unidade').val(),
        tipoMovimentacao: $('#tipoMovimentacao').val(),
        assetCode: $('#assetCode').val(),
        documento: $('#documento').val(),
        dataInicio: $('#dataInicio').val(),
        dataTermino: $('#dataTermino').val(),
        origem: $('#origem').val(),
        destino: $('#destino').val()
    };

    localStorage.setItem("assetMovementFiltro", JSON.stringify(filtro));
}

function carregarFiltro() {

    var filtro = JSON.parse(localStorage.getItem("assetMovementFiltro") || "{}");

    if (!filtro) return;

    $('#unidade').val(filtro.unidade || "");
    $('#tipoMovimentacao').val(filtro.tipoMovimentacao || "");
    $('#assetCode').val(filtro.assetCode || "");
    $('#documento').val(filtro.documento || "");
    $('#dataInicio').val(filtro.dataInicio || "");
    $('#dataTermino').val(filtro.dataTermino || "");
    $('#origem').val(filtro.origem || "");
    $('#destino').val(filtro.destino || "");
}
