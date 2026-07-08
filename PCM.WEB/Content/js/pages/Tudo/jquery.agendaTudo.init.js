// ==========================================================================
// Tudo em Dia — Agenda de vencimentos + auto-planejamento
//  - grid agrupado por dia (loadGridMain)
//  - Gerar planejamento (auto-distribuição por responsável / capacidade)
//  - Limpar planejamento em aberto
//  - Executar: abre a tela de apontamento do local
// ==========================================================================
const messagesData = document.getElementById('resource-messages').getAttribute('data-messages');
const messages = JSON.parse(messagesData);

$(document).ready(function () {

    if ($.fn.mask) { $(".js-datepicker").mask("99/99/9999"); }
    if (window.Codebase && Codebase.helpers) { Codebase.helpers(['datepicker']); }

    carregarGrid();

    $('#filtrar').click(function () { recarregarPagina(); });
    $('#unidade, #funcionario').change(function () { recarregarPagina(); });

    $('#gerar').click(function () { gerarPlanejamento(); });
    $('#limpar').click(function () { limparPlanejamento(); });
});

function filtros() {
    return {
        unidade: $('#unidade').val(),
        funcionario: $('#funcionario').val() || -1,
        data_inicio: $('#data_inicio').val(),
        data_termino: $('#data_termino').val()
    };
}

function recarregarPagina() {
    var f = filtros();
    var cap = $('#capacidade').val() || 10;
    window.location = messages.urlAgenda +
        '?unidade=' + f.unidade +
        '&funcionario=' + f.funcionario +
        '&data_inicio=' + encodeURIComponent(f.data_inicio) +
        '&data_termino=' + encodeURIComponent(f.data_termino) +
        '&capacidade=' + cap;
}

function carregarGrid() {

    var botoes = [
        {
            action: "executar",
            class: "btn btn-sm btn-outline-secondary",
            icon: "fa fa-play",
            title: messages.executar,
            onClick: function (row) { executar(row); }
        }
    ];

    loadGridMain({
        tableId: "#tbMain",
        data: filtros(),
        endpoint: messages.urlLoad,
        customAction: true,
        customButtons: botoes,
        enablePaging: true,
        pageLength: 25,
        enableSearch: true,
        enableExport: true,
        textSearch: messages.search,
        textNothingRegister: messages.nothingRegister
    });
}

function executar(row) {
    window.location = messages.urlApontamento +
        '?codigo_unidade=' + row.codigo_unidade +
        '&codigo_apartamento=' + row.codigo_apartamento +
        '&origem=agenda';
}

function gerarPlanejamento() {

    var f = filtros();
    var token = $('input[name="__RequestVerificationToken"]').val();

    $.ajax({
        url: messages.urlGerar,
        type: "POST",
        data: {
            unidade: f.unidade,
            data_inicio: f.data_inicio,
            data_termino: f.data_termino,
            capacidade: $('#capacidade').val() || 10,
            __RequestVerificationToken: token
        },
        success: function (resp) {
            if (resp && resp.success) {
                if (window.rfAlert) { rfAlert({ title: "", message: messages.gerarOk + " (" + resp.total + ")", icon: "success" }); }
                recarregarPagina();
            } else {
                if (window.rfAlert) { rfAlert({ title: "", message: (resp && resp.message) || "Erro", icon: "error" }); }
            }
        }
    });
}

async function limparPlanejamento() {

    var ok = true;
    if (window.rfConfirm) {
        ok = await rfConfirm({ title: messages.limpar, message: messages.confirmaLimpar, confirmButtonText: messages.limpar, cancelButtonText: "Cancelar", icon: "warning" });
    } else {
        ok = confirm(messages.confirmaLimpar);
    }
    if (!ok) return;

    var f = filtros();
    var token = $('input[name="__RequestVerificationToken"]').val();

    $.ajax({
        url: messages.urlLimpar,
        type: "POST",
        data: {
            unidade: f.unidade,
            data_inicio: f.data_inicio,
            data_termino: f.data_termino,
            __RequestVerificationToken: token
        },
        success: function (resp) {
            if (resp && resp.success) { recarregarPagina(); }
        }
    });
}
