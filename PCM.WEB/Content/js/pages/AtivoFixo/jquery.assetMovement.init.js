
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
        asset: $('#asset').val(),
        status: $('#status').val(),
        setor: $('#setor').val(),
        apartamento: $('#apartamento').val()
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
        enableChild: true,
        childRender: function (row) {
            return loadChildTable(row);
        }
    });

}

function salvarFiltro() {

    var filtro = {
        unidade: $('#unidade').val(),
        asset: $('#asset').val(),
        status: $('#status').val(),
        setor: $('#setor').val(),
        apartamento: $('#apartamento').val()
    };

    localStorage.setItem("assetMovementfiltro", JSON.stringify(filtro));
}

function carregarFiltro() {

    var filtro = JSON.parse(localStorage.getItem("assetMovementfiltro") || "{}");

    if (!filtro) return;

    $('#unidade').val(filtro.unidade || "");
    $('#asset').val(filtro.asset || "");
    $('#status').val(filtro.status || "");
    $('#setor').val(filtro.setor || "");
    $('#apartamento').val(filtro.apartamento || "");
}

function loadChildTable(data) {

    var table_info = '';

    $.ajax({
        type: "POST",
        url: messages.urlLoadAssetMovementDetails,
        async: false,
        data: {
            assetId: data.assetId
        },
        success: function (response) {

            table_info = `<div class="bg-light">
                <table class="table table-striped table-bordered">
                    <thead>
                        <tr>
                            <th>${messages.tipoMovimentacao}</th>
                            <th>${messages.documento}</th>
                            <th>${messages.setor}</th>
                            <th>${messages.apartamento}</th>
                            <th class="text-center">${messages.data}</th>
                            <th class="text-center">${messages.usuario}</th>
                        </tr>
                    </thead>
                    <tbody>`;

            response.forEach(r => {
                table_info += `
                    <tr>
                        <td>${r.tipoMovimentacao}</td>
                        <td>${r.documento}</td>
                        <td>${r.setor}</td>
                        <td>${r.apartamento}</td>
                        <td class="text-center">${r.data}</td>
                        <td class="text-center">${r.usuario}</td>
                    </tr>`;
            });

            table_info += "</tbody></table></div>";
        }
    });

    return table_info;
}