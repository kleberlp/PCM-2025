// ==========================================================================
// Tudo em Dia — Agenda (calendário estilo Outlook via FullCalendar)
//  - Eventos por dia (execuções planejadas)
//  - Gerar planejamento: pega tudo que falta no mês exibido, divide pelos dias
//    disponíveis e distribui por responsável sem estourar o vencimento
//  - Clicar no evento abre o apontamento do local
// ==========================================================================
const messagesData = document.getElementById('resource-messages').getAttribute('data-messages');
const messages = JSON.parse(messagesData);

var calendar = null;

$(document).ready(function () {

    calendar = new FullCalendar.Calendar(document.getElementById('tudo-calendar'), {
        themeSystem: 'bootstrap',
        locale: 'pt-br',
        firstDay: 0,
        height: alturaCalendario(),   // altura fixa -> cabeçalho da página fica visível
        stickyHeaderDates: true,      // mantém os nomes dos dias ao rolar o calendário
        dayMaxEvents: 4,              // mostra até 4 por dia; o resto vira "+N mais"
        moreLinkText: function (n) { return "+" + n + " mais"; },
        initialView: 'dayGridMonth',
        headerToolbar: {
            left: 'prev,next today',
            center: 'title',
            right: 'dayGridMonth,dayGridWeek'
        },
        events: carregarEventos,
        eventClick: function (info) {
            var p = info.event.extendedProps;
            window.location = messages.urlApontamento +
                '?codigo_unidade=' + p.codigo_unidade +
                '&codigo_apartamento=' + p.codigo_apartamento +
                '&origem=agenda';
        }
    });

    calendar.render();

    // reajusta a altura do calendário quando a janela muda de tamanho
    $(window).on('resize', function () {
        if (calendar) calendar.setOption('height', alturaCalendario());
    });

    $('#unidade, #funcionario').change(function () { calendar.refetchEvents(); });
    $('#gerar').click(function () { gerarPlanejamento(); });
    $('#limpar').click(function () { limparPlanejamento(); });
});

// altura do calendário = espaço abaixo do topo da tela (mantém cabeçalho visível)
function alturaCalendario() {
    var el = document.getElementById('tudo-calendar');
    var top = el ? el.getBoundingClientRect().top : 320;
    return Math.max(460, window.innerHeight - top - 24);
}

// -------------------------------------------------------------------
// Eventos do calendário
// -------------------------------------------------------------------
function carregarEventos(fetchInfo, success, failure) {

    var token = $('input[name="__RequestVerificationToken"]').val();

    $.ajax({
        url: messages.urlCalendario,
        type: "POST",
        data: {
            unidade: $('#unidade').val(),
            funcionario: $('#funcionario').val() || -1,
            data_inicio: fmt(fetchInfo.start),
            data_termino: fmt(fetchInfo.end),
            __RequestVerificationToken: token
        },
        success: function (rows) {
            rows = rows || [];
            atualizarKpis(rows);
            success(rows.map(toEvent));
        },
        error: function () { failure(); }
    });
}

function toEvent(row) {
    return {
        title: row.local + (row.responsavel ? " · " + row.responsavel : ""),
        start: row.data,
        allDay: true,
        backgroundColor: corStatus(row),
        borderColor: corStatus(row),
        textColor: "#fff",
        extendedProps: {
            codigo: row.codigo,
            codigo_unidade: row.codigo_unidade,
            codigo_apartamento: row.codigo_apartamento,
            situacao: row.situacao,
            responsavel: row.responsavel
        }
    };
}

function corStatus(row) {
    if (row.status_codigo === 1) return "#22c55e"; // concluído
    if (row.status_codigo === 2) return "#9ca3af"; // cancelado
    if (row.atrasado) return "#ef4444";            // planejado atrasado
    if (hoje(row.data)) return "#f59e0b";          // planejado hoje
    return "#3b82f6";                              // planejado futuro
}

// -------------------------------------------------------------------
// KPIs (do mês exibido)
// -------------------------------------------------------------------
function atualizarKpis(rows) {
    var ini = calendar.view.currentStart;
    var fim = calendar.view.currentEnd; // exclusivo
    var hojeStr = fmtIso(new Date());

    var planejados = 0, atrasados = 0, paraHoje = 0, concluidos = 0;

    rows.forEach(function (r) {
        var d = new Date(r.data + "T00:00:00");
        if (d < ini || d >= fim) return; // só o mês visível
        if (r.status_codigo === 1) { concluidos++; return; }
        if (r.status_codigo === 0) {
            planejados++;
            if (r.atrasado) atrasados++;
            if (r.data === hojeStr) paraHoje++;
        }
    });

    $('#kpiPlanejados').text(planejados);
    $('#kpiAtrasados').text(atrasados);
    $('#kpiHoje').text(paraHoje);
    $('#kpiConcluidos').text(concluidos);
}

// -------------------------------------------------------------------
// Planejamento
// -------------------------------------------------------------------
function mesVisivel() {
    var ini = calendar.view.currentStart;             // 1º dia do mês
    var fimExcl = calendar.view.currentEnd;           // 1º dia do mês seguinte
    var fim = new Date(fimExcl.getTime() - 86400000); // último dia do mês
    return { inicio: fmt(ini), termino: fmt(fim) };
}

function gerarPlanejamento() {

    var mes = mesVisivel();
    var token = $('input[name="__RequestVerificationToken"]').val();

    $.ajax({
        url: messages.urlGerar,
        type: "POST",
        data: {
            unidade: $('#unidade').val(),
            data_inicio: mes.inicio,
            data_termino: mes.termino,
            capacidade: $('#capacidade').val() || 0,
            __RequestVerificationToken: token
        },
        success: function (resp) {
            if (resp && resp.success) {
                if (window.rfAlert) { rfAlert({ title: "", message: messages.gerarOk + " (" + resp.total + ")", icon: "success" }); }
                calendar.refetchEvents();
            } else if (window.rfAlert) {
                rfAlert({ title: "", message: (resp && resp.message) || "Erro", icon: "error" });
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

    var mes = mesVisivel();
    var token = $('input[name="__RequestVerificationToken"]').val();

    $.ajax({
        url: messages.urlLimpar,
        type: "POST",
        data: {
            unidade: $('#unidade').val(),
            data_inicio: mes.inicio,
            data_termino: mes.termino,
            __RequestVerificationToken: token
        },
        success: function (resp) {
            if (resp && resp.success) { calendar.refetchEvents(); }
        }
    });
}

// -------------------------------------------------------------------
// Helpers de data
// -------------------------------------------------------------------
function fmt(d) {
    return ('0' + d.getDate()).slice(-2) + '/' + ('0' + (d.getMonth() + 1)).slice(-2) + '/' + d.getFullYear();
}
function fmtIso(d) {
    return d.getFullYear() + '-' + ('0' + (d.getMonth() + 1)).slice(-2) + '-' + ('0' + d.getDate()).slice(-2);
}
function hoje(iso) {
    return iso === fmtIso(new Date());
}
