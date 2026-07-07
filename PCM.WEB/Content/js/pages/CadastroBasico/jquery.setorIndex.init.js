
const messagesData = document.getElementById('resource-messages').getAttribute('data-messages');
const messages = JSON.parse(messagesData);

$(document).ready(function () {

    carregarFiltro();
    carregarGrid();

    $('#unidade, #descricao').change(function () {
        salvarFiltro();
    });

    $('#descricao').on('keypress', function (e) {
        if (e.keyCode === 13) {
            e.preventDefault();
            salvarFiltro();
            carregarGrid();
        }
    });

    $('#filtrar').click(function () {
        salvarFiltro();
        carregarGrid();
    });

});

function carregarGrid() {

    var data = {
        unidade: $('#unidade').val(),
        descricao: $('#descricao').val()
    };

    loadGridMain({
        tableId: "#tbMain",
        data: data,
        endpoint: messages.urlLoadSetor,
        editAction: messages.editar,
        deleteAction: messages.excluir,
        enablePaging: true,
        pageLength: 15,
        enableSearch: true,
        enableExport: true,
        textSearch: messages.search,
        textNothingRegister: messages.nothingRegister,
        enableChild: true,
        childRender: renderLocais,
        onEdit: (row) => editRegister(row),
        onDelete: (row) => deleteRegister(row)
    });

}

function salvarFiltro() {

    var filtro = {
        unidade: $('#unidade').val(),
        descricao: $('#descricao').val()
    };

    localStorage.setItem("setor_filtro", JSON.stringify(filtro));
}

function carregarFiltro() {

    var filtro = JSON.parse(localStorage.getItem("setor_filtro") || "{}");

    if (!filtro) return;

    if (filtro.unidade) $('#unidade').val(filtro.unidade);
    $('#descricao').val(filtro.descricao || "");
}

function editRegister(row) {
    window.location = messages.urlSetorEdit + '?codigo=' + row.codigo;
}

function deleteRegister(row) {
    window.location = messages.urlSetorDelete + '?codigo=' + row.codigo;
}

// =========================
// CHILD (+): locais do setor
// =========================
function renderLocais(row) {

    var containerId = "locais_" + row.codigo;

    // dispara o carregamento assíncrono após o child ser inserido
    setTimeout(function () { carregarLocais(row.codigo, row.codigo_unidade, containerId); }, 0);

    return "<div id='" + containerId + "' class='p-2'>" +
                "<i class='fa fa-spinner fa-spin'></i> ..." +
           "</div>";
}

function carregarLocais(codigo, unidade, containerId) {

    var token = $('input[name="__RequestVerificationToken"]').val();

    $.ajax({
        url: messages.urlLoadSetorLocal,
        type: "POST",
        data: {
            codigo: codigo,
            unidade: unidade || -1,
            __RequestVerificationToken: token
        },
        success: function (list) {
            $("#" + containerId).html(buildLocaisTable(list || []));
        },
        error: function () {
            $("#" + containerId).html("<div class='text-danger'>" + messages.nothingRegister + "</div>");
        }
    });
}

function buildLocaisTable(list) {

    if (!list.length) {
        return "<div class='text-muted p-2'>" + messages.nothingRegister + "</div>";
    }

    var html = "<table class='table table-sm table-bordered table-striped mb-0'>";
    html += "<thead><tr class='table-secondary'>" +
                "<th>" + messages.local + "</th>" +
                "<th>" + messages.checklist + "</th>" +
                "<th>" + messages.periodicidade + "</th>" +
                "<th class='text-center'>" + messages.intervalo + "</th>" +
            "</tr></thead><tbody>";

    list.forEach(function (item) {
        html += "<tr>" +
                    "<td>" + safe(item.local) + "</td>" +
                    "<td>" + safe(item.checklist) + "</td>" +
                    "<td>" + safe(item.periodicidade) + "</td>" +
                    "<td class='text-center'>" + (item.intervalo > 0 ? item.intervalo : "") + "</td>" +
                "</tr>";
    });

    html += "</tbody></table>";

    return html;
}

function safe(value) {
    if (value === null || value === undefined) return "";
    return String(value);
}
