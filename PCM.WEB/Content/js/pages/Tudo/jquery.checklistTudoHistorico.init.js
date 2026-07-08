// ==========================================================================
// Tudo em Dia — Histórico (grid dinâmico loadGridMain)
//  - Visualizar: abre a tela de apontamento em modo somente leitura
//  - Excluir: remove o apontamento e recalcula a próxima execução
// ==========================================================================
const messagesData = document.getElementById('resource-messages').getAttribute('data-messages');
const messages = JSON.parse(messagesData);

$(document).ready(function () {

    if ($.fn.mask) { $(".js-datepicker").mask("99/99/9999"); }
    if (window.Codebase && Codebase.helpers) { Codebase.helpers(['datepicker']); }

    carregarGrid();

    $('#filtrar').click(function () { carregarGrid(); });
    $('#unidade, #apartamento').change(function () { carregarGrid(); });
});

function carregarGrid() {

    var data = {
        unidade: $('#unidade').val(),
        apartamento: $('#apartamento').val() || -1,
        data_inicio: $('#data_inicio').val(),
        data_termino: $('#data_termino').val()
    };

    var botoes = [
        {
            action: "visualizar",
            class: "btn btn-sm btn-outline-secondary",
            icon: "fa fa-eye",
            title: messages.visualizar,
            onClick: function (row) { visualizar(row); }
        }
    ];

    if (messages.podeExcluir) {
        botoes.push({
            action: "excluir",
            class: "btn btn-sm btn-danger",
            icon: "fa fa-times",
            title: messages.excluir,
            onClick: function (row) { excluir(row); }
        });
    }

    loadGridMain({
        tableId: "#tbMain",
        data: data,
        endpoint: messages.urlLoad,
        customAction: true,
        customButtons: botoes,
        enablePaging: true,
        pageLength: 15,
        enableSearch: true,
        enableExport: true,
        textSearch: messages.search,
        textNothingRegister: messages.nothingRegister
    });
}

function visualizar(row) {
    window.location = messages.urlApontamento +
        '?codigo_unidade=' + row.codigo_unidade +
        '&codigo_apartamento=' + row.codigo_apartamento +
        '&codigo_apontamento=' + row.codigo +
        '&visualizar=true' +
        '&origem=historico';
}

async function excluir(row) {

    var ok = true;

    if (window.rfConfirm) {
        ok = await rfConfirm({
            title: messages.excluir,
            message: messages.confirmaExclusao,
            confirmButtonText: messages.excluir,
            cancelButtonText: "Cancelar",
            icon: "warning"
        });
    } else {
        ok = confirm(messages.confirmaExclusao);
    }

    if (!ok) return;

    var token = $('input[name="__RequestVerificationToken"]').val();

    $.ajax({
        url: messages.urlExcluir,
        type: "POST",
        data: { codigo_unidade: row.codigo_unidade, codigo: row.codigo, __RequestVerificationToken: token },
        success: function (resp) {
            if (resp && resp.success) {
                if (window.rfAlert) { rfAlert({ title: "", message: messages.registroExcluido, icon: "success" }); }
                carregarGrid();
            } else {
                if (window.rfAlert) { rfAlert({ title: "", message: messages.erroExcluir, icon: "error" }); }
                else { alert(messages.erroExcluir); }
            }
        },
        error: function () {
            if (window.rfAlert) { rfAlert({ title: "", message: messages.erroExcluir, icon: "error" }); }
            else { alert(messages.erroExcluir); }
        }
    });
}
