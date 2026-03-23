
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
        codigo: $('#assetCode').val(),
        descricao: $('#descricao').val(),
        status: $('#status').val(),
        localizacao: $('#localizacao').val()
    };

    loadGridMain(
        table,
        data,
        messages.urlLoadAsset,
        messages.editar,
        messages.excluir,
        false
    );

}

function salvarFiltro() {

    var filtro = {
        unidade: $('#unidade').val(),
        codigo: $('#assetCode').val(),
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
    $('#assetCode').val(filtro.codigo || "");
    $('#descricao').val(filtro.descricao || "");
    $('#status').val(filtro.status || "");
    $('#localizacao').val(filtro.localizacao || "");
}
