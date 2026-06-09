const messagesData = document.getElementById('resource-messages').getAttribute('data-messages');
const messages = JSON.parse(messagesData);

// Obtendo URL da API do atributo data
var apiUrls = document.getElementById("api-urls");
var chartArrumadoxVistoriadoUrl = apiUrls.getAttribute("data-chart-arrumado-vistoriado");
var chartNaoConformidadeDiaUrl = apiUrls.getAttribute("data-chart-nao-conformidade-dia");
var table;
var day = 30;

let chartArr = null;
let chartNC = null;

// Inicializando os gráficos
const ctxArr = document.getElementById('chartArrumadoxVistoriado')?.getContext('2d');
const ctxNC = document.getElementById('chartNCDia')?.getContext('2d');

// Chamado pelo codebase.js após popular os selects do header
window.onHeaderReady = function () {

    Codebase.helpers(['datepicker', 'maxlength', 'select2', 'easy-pie-chart']);

    if ($("#header-unidade").val() == "-1") {
        $("#arrumacaoCamareira").hide();
    }

    // Arrumado x Vistoriado
    if (ctxArr) {
        loadChartData(chartArrumadoxVistoriadoUrl, "arrumadoxvistoriado");
    } else {
        console.error("Erro: Elemento de gráfico Arrumado x Vistoriado não encontrado.");
    }

    // NC DIA
    if (ctxNC) {
        loadChartData(chartNaoConformidadeDiaUrl, "ncDia");
    } else {
        console.error("Erro: Elemento de gráfico NC Dia não encontrado.");
    }

    var tableProdutividade = $('#tbProdutividadeCamareira').DataTable({
        select: {
            selector: 'td:not(:first-child)',
            style: 'os'
        },
        searching: false,
        fixedColumns: {
            start: 0,
            end: 1
        },
        scrollX: false,
        autoWidth: false,
        lengthChange: false,
        pageLength: 15,
        processing: true,
        scrollCollapse: true,
        serverSide: false,
        ajax: {
            url: "LoadProdutividadeCamareira",
            type: "POST",
            datatype: "json",
            data: function (d) {
                d.empresa = $('#empresa').val(),
                    d.unidade = ($('#header-unidade').val() == "") ? -1 : $('#header-unidade').val(),
                    d.data = $('#data').val()
            },
            dataSrc: ""
        },
        columns: [
            { data: "unidade" },
            { data: "totalUHArrumada" },
            { data: "totalUHSaida" },
            { data: "totalUHPermanencia" },
            { data: "totalUHManutencao" },
            { data: "percentual" }
        ],
        language: {
            emptyTable: messages.emptyTable,
            info: "",
            infoEmpty: "",
            infoFiltered: "",
        },
        columnDefs: [
            { className: 'text-center', targets: [1, 2, 3, 4, 5] },
            { width: '150px', targets: [1, 2, 3, 4, 5] },
        ],

    });

    var tbProdutividadeVistoriador = $('#tbProdutividadeVistoriador').DataTable({
        select: {
            selector: 'td:not(:first-child)',
            style: 'os'
        },
        searching: false,
        fixedColumns: {
            start: 0,
            end: 1
        },
        scrollX: false,
        autoWidth: false,
        lengthChange: false,
        pageLength: 15,
        processing: true,
        scrollCollapse: true,
        serverSide: false,
        ajax: {
            url: "LoadProdutividadeVistoriador",
            type: "POST",
            datatype: "json",
            data: function (d) {
                d.empresa = $('#empresa').val(),
                    d.unidade = ($('#header-unidade').val() == "") ? -1 : $('#header-unidade').val(),
                    d.data = $('#data').val()
            },
            dataSrc: ""
        },
        columns: [
            { data: "unidade" },
            { data: "totalUHSaida" },
            { data: "totalUHVistoriada" },
            { data: "percentualTotal" },
            { data: "percentualMeta" }
        ],
        language: {
            emptyTable: messages.emptyTable,
            info: "",
            infoEmpty: "",
            infoFiltered: "",
        },
        columnDefs: [
            { className: 'text-center', targets: [1, 2, 3, 4] },
            { width: '150px', targets: [1, 2, 3, 4] },
        ],

    });

    var tbNCDetalhado = $('#tbNCDetalhado').DataTable({
        select: {
            selector: 'td:not(:first-child)',
            style: 'os'
        },
        searching: false,
        fixedColumns: {
            start: 0,
            end: 1
        },
        scrollX: false,
        autoWidth: false,
        lengthChange: false,
        pageLength: 15,
        processing: true,
        scrollCollapse: true,
        serverSide: false,
        ajax: {
            url: "LoadNCDetalhado",
            type: "POST",
            datatype: "json",
            data: function (d) {
                d.empresa = $('#empresa').val(),
                    d.unidade = ($('#header-unidade').val() == "") ? -1 : $('#header-unidade').val(),
                    d.data = $('#data').val()
            },
            dataSrc: ""
        },
        columns: [
            { data: "ocorrencia" },
            { data: "quantidadeNC" },
            { data: "mediaMovel30Dias" },
            { data: "tendencia" }
        ],
        order: [[1, 'desc']],
        language: {
            emptyTable: messages.emptyTable,
            info: "",
            infoEmpty: "",
            infoFiltered: "",
        },
        columnDefs: [
            { className: 'text-center', targets: [1, 2, 3] },
            { width: '150px', targets: [1, 2, 3] },
            {
                targets: 3,
                className: 'text-center',
                render: function (data) {
                    if (!data) return '-';

                    const value = data.toLowerCase();

                    if (value.includes('aumento'))
                        return '<span class="text-danger"><i class="fa fa-arrow-up"></i>' + value + '</span>';

                    if (value.includes('queda'))
                        return '<span class="text-success"><i class="fa fa-arrow-down"></i> ' + value + '</span>';

                    if (value.includes('estável'))
                        return '<span class="text-secondary"><i class="fa fa-minus"></i> ' + value + '</span>';

                    return data;
                }
            }
        ],

    });

    var tbNCCamareira = $('#tbNCCamareira').DataTable({
        select: {
            selector: 'td:not(:first-child)',
            style: 'os'
        },
        searching: false,
        fixedColumns: {
            start: 0,
            end: 1
        },
        scrollX: false,
        autoWidth: false,
        lengthChange: false,
        pageLength: 15,
        processing: true,
        scrollCollapse: true,
        serverSide: false,
        ajax: {
            url: "LoadNCCamareira",
            type: "POST",
            datatype: "json",
            data: function (d) {
                d.empresa = $('#empresa').val(),
                    d.unidade = ($('#header-unidade').val() == "") ? -1 : $('#header-unidade').val(),
                    d.data = $('#data').val()
            },
            dataSrc: ""
        },
        columns: [
            { data: "camareira" },
            { data: "quantidadeNC" },
            { data: "mediaMovel30Dias" },
            { data: "tendencia" }
        ],
        order: [[1, 'desc']],
        language: {
            emptyTable: messages.emptyTable,
            info: "",
            infoEmpty: "",
            infoFiltered: "",
        },
        columnDefs: [
            { className: 'text-center', targets: [1, 2, 3] },
            { width: '150px', targets: [1, 2, 3] },
            {
                targets: 3,
                className: 'text-center',
                render: function (data) {
                    if (!data) return '-';

                    const value = data.toLowerCase();

                    if (value.includes('aumento'))
                        return '<span class="text-danger"><i class="fa fa-arrow-up"></i> ' + value + '</span>';

                    if (value.includes('queda'))
                        return '<span class="text-success"><i class="fa fa-arrow-down"></i> ' + value + '</span>';

                    if (value.includes('estável'))
                        return '<span class="text-secondary"><i class="fa fa-minus"></i> ' + value + '</span>';

                    return data;
                }
            }
        ],

    });

    var tbRankingCamareira = $('#tbRankingCamareira').DataTable({
        select: {
            selector: 'td:not(:first-child)',
            style: 'os'
        },
        searching: false,
        fixedColumns: {
            start: 0,
            end: 1
        },
        scrollX: false,
        autoWidth: false,
        lengthChange: false,
        pageLength: 15,
        processing: true,
        scrollCollapse: true,
        serverSide: false,
        ajax: {
            url: "LoadRankingCamareira",
            type: "POST",
            datatype: "json",
            data: function (d) {
                d.empresa = $('#empresa').val(),
                    d.unidade = ($('#header-unidade').val() == "") ? -1 : $('#header-unidade').val(),
                    d.data = $('#data').val()
            },
            dataSrc: ""
        },
        columns: [
            { data: "ranking" },
            { data: "camareira" },
            { data: "qtdeNC" },
            { data: "qtdeNCRetrabalho" },
            { data: "pesoNCRetrabalho" },
            { data: "qtdeUH" },
            { data: "percentualNC" }
        ],
        order: [[0, 'asc']],
        language: {
            emptyTable: messages.emptyTable,
            info: "",
            infoEmpty: "",
            infoFiltered: "",
        },
        columnDefs: [
            { className: 'text-center', targets: [0, 2, 3, 4, 5, 6] },
            { width: '150px', targets: [2, 3, 4, 5, 6] },
            { width: '20px', targets: [0] },
            {
                createdCell: function (td, cellData, rowData, row, col) {
                    $(td).addClass(rowData.cssClass);
                }, targets: [0]
            }
        ],

    });

});

// unidade e modulo são gerenciados pelo header global (codebase.js)
// apenas o filtro de data é local a esta tela
$("#data").change(function () {
    $("#form").submit();
});

async function loadChartData(url, type) {

    const unidade = document.getElementById('header-unidade').value;
    const data = document.getElementById("data").value;

    const dataInicio = inicioFimMes(data).primeiroDia;
    const dataTermino = inicioFimMes(data).ultimoDia;

    try {

        if (type === "arrumadoxvistoriado") {

            const payload = await fetchChartArrumadoxVistoriado(url, { unidade, dataInicio, dataTermino });
            console.log(payload);
            const cfg = normalizeChartConfig(getChartConfigArrumadoxVistoriado());
            applyArrumadoxVistoriadoData(cfg, payload);

            //ajustarLayoutGraficoArrumado(payload.length);

            if (chartArr) chartArr.destroy();

            chartArr = new Chart(ctxArr, cfg);

        } else if (type === "ncDia") {

            const ds = await fetchChartData(url, { unidade, dataInicio, dataTermino });

            const cfg = getChartConfigNCDia();
            cfg.data.labels = ds.labels;
            cfg.data.datasets = ds.datasets;

            if (chartNC) chartNC.destroy();
            chartNC = new Chart(ctxNC, cfg);
        }

    } catch (error) {
        console.error("Erro ao carregar dados do gráfico:", error);
    }
}

function ajustarLayoutGraficoArrumado(labelsCount) {

    const colArrumado = document.getElementById("col-chart-arrumado");
    const colNC = document.getElementById("col-chart-nc");

    if (labelsCount > 6 && $("#header-unidade").val() === "-1") {
        // Gráfico principal ocupa a linha inteira
        colArrumado.classList.remove("col-md-6");
        colArrumado.classList.add("col-md-12");

        // Segundo gráfico desce para a linha de baixo
        colNC.classList.remove("col-md-6");
        colNC.classList.add("col-md-12");
    } else {
        // Volta ao layout 50/50
        colArrumado.classList.remove("col-md-12");
        colArrumado.classList.add("col-md-6");

        colNC.classList.remove("col-md-12");
        colNC.classList.add("col-md-6");
    }
}

function normalizeChartConfig(cfg) {
    if (!cfg || typeof cfg !== 'object') {
        throw new Error('Chart config inválido');
    }

    // GARANTE estrutura mínima exigida pelo Chart.js
    cfg.options ??= {};
    cfg.options.plugins ??= {};
    cfg.options.scales ??= {};

    return cfg;
}

function applyArrumadoxVistoriadoData(cfg, payload) {

    const labels = payload.map(x => x.data);

    const saida = payload.map(x => Number(x.qtdeSaida ?? 0));
    const permanencia = payload.map(x => Number(x.qtdePermanencia ?? 0));
    const manutencao = payload.map(x => Number(x.qtdeManutencao ?? 0));
    const meta = payload.map(x => Number(x.meta ?? 0));
    const qtdeVistoriado = payload.map(x => Number(x.percentualVistoriado ?? 0));

    cfg.data.labels = labels;

    cfg.data.datasets = [
        {
            type: 'line',
            label: 'Meta',
            data: meta,
            borderColor: '#2c3e50',
            borderDash: [1, 1],
            borderWidth: 2,
            pointRadius: 0,
            fill: false
        },
        {
            type: 'line',
            label: 'Qtde. Vistoriado',
            data: qtdeVistoriado,
            borderColor: '#27ae60',
            borderWidth: 2,
            pointRadius: 3,
            fill: false
        },
        {
            type: 'bar',
            label: 'Qtde. Saída',
            data: saida,
            backgroundColor: '#e74c3c',
            stack: 'total'
        },
        {
            type: 'bar',
            label: 'Qtde. Permanência',
            data: permanencia,
            backgroundColor: '#3498db',
            stack: 'total'
        },
        {
            type: 'bar',
            label: 'Qtde. Manutenção',
            data: manutencao,
            backgroundColor: '#f1c40f',
            stack: 'total'
        }

    ];
}

function normalizeForChart(payload) {
    // 1) Datas em ISO (se seus labels vierem como dd/MM/yyyy)
    const labels = (payload.labels || []).map(l => {
        if (typeof l === 'string' && /^\d{2}\/\d{2}\/\d{4}$/.test(l)) {
            const [dd, mm, yyyy] = l.split('/');
            return `${dd}/${mm}/${yyyy}`; // ISO para o eixo time
        }
        return l;
    });

    // 2) Coagir valores para Number, mantendo null
    const datasets = (payload.datasets || []).map(ds => ({
        ...ds,
        data: (ds.data || []).map(v =>
            v === null || v === undefined || v === '' ? null : Number(v)
        )
    }));

    return { labels, datasets };
}

function formatDate(dateStr) {
    const [day, month, year] = dateStr.split('/');
    return `${year}-${month}-${day}`;
}

async function fetchChartData(url, params) {
    try {
        const response = await fetch(url, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(params)
        });

        if (!response.ok) {
            throw new Error(`Erro ao buscar dados do gráfico: ${response.statusText}`);
        }
        const data = await response.json();
        return normalizeForChart(data);
    } catch (error) {
        console.error("Erro ao buscar dados do gráfico:", error);
        throw error; // Relançar o erro para ser tratado onde a função foi chamada
    }
}

async function fetchChartArrumadoxVistoriado(url, params) {
    const response = await fetch(url, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(params)
    });

    if (!response.ok) {
        throw new Error(`Erro ao buscar dados: ${response.statusText}`);
    }

    return await response.json();
}

function getChartConfigArrumadoxVistoriado() {
    return {
        type: 'bar',
        data: {
            labels: [],
            datasets: []
        },
        options: {
            responsive: true,
            maintainAspectRatio: false,

            tooltips: {
                mode: 'index',
                intersect: false
            },

            legend: {
                position: 'top'
            },
            scales: {
                xAxes: [{
                    stacked: true
                }],
                yAxes: [{
                    stacked: true,
                    ticks: {
                        beginAtZero: true,
                        precision: 0
                    }
                }]
            }

        }
    };
}

function getChartConfigNCDia() {
    return {
        type: 'line',
        options: {
            responsive: true,
            maintainAspectRatio: false,

            interaction: { mode: 'index', intersect: false },

            plugins: {
                legend: {
                    position: 'top',
                    labels: {
                        boxWidth: 12,
                        boxHeight: 12,
                        font: { family: 'Inter, Segoe UI, Arial, sans-serif', size: 12, weight: '600' }
                    }
                },
                tooltip: { mode: 'index', intersect: false }
            },

            scales: {
                x: {
                    grid: { display: false },
                    ticks: {
                        autoSkip: true,
                        maxTicksLimit: 8,
                        maxRotation: 0,
                        minRotation: 0,
                        padding: 8,
                        font: { family: 'Inter, Segoe UI, Arial, sans-serif', size: 11 }
                    }
                },
                y: {
                    beginAtZero: true,
                    ticks: {
                        precision: 0,
                        font: { family: 'Inter, Segoe UI, Arial, sans-serif', size: 11 }
                    },
                    grid: { color: 'rgba(0,0,0,.05)' }
                }
            }
        },
        data: {
            labels: [],
            datasets: []
        }
    };
}

function updateChartConfig(chart, labels, newDatasets) {

    // labels OK
    chart.data.labels = labels;

    // se ainda não existem datasets, cria
    if (!chart.data.datasets || chart.data.datasets.length === 0) {
        chart.data.datasets = newDatasets;
        return;
    }

    // atualiza dataset por dataset
    newDatasets.forEach((ds, i) => {
        if (!chart.data.datasets[i]) {
            chart.data.datasets[i] = ds;
        } else {
            chart.data.datasets[i].data = ds.data;
            chart.data.datasets[i].label = ds.label;
        }
    });

    // remove datasets extras
    chart.data.datasets.length = newDatasets.length;

    // eixo Y seguro
    if (chart.options?.scales?.y) {
        chart.options.scales.y.beginAtZero = true;
        chart.options.scales.y.min = 0;
        chart.options.scales.y.ticks ??= {};
        chart.options.scales.y.ticks.precision = 0;
        chart.options.scales.y.ticks.stepSize = 1;
    }
}

function format(data) {

    var info = '';

    $.ajax({
        type: "POST",
        url: "LoadGovernancaHistoricoDetails",
        async: false,
        data: {
            "empresa": data.codigoEmpresa,
            "unidade": data.codigoUnidade,
            "codigo": data.codigo
        },
        dataType: "json",
        success: function (response) {

            if (response.grupo.length) {

                info = info + '<table class="table table-bordered table-vcenter table-striped table-sm js-dataTable-full" id="tbDetails"> ';
                info = info + '<thead> ';
                info = info + '<tr class="table-secondary"> ';
                info = info + '<th class="font-size-sm">' + apiUrls.getAttribute("data-resource-checklist") + '</th> ';
                info = info + '<th class="text-center font-size-sm">' + apiUrls.getAttribute("data-resource-checklist") + '</th> ';
                info = info + '<th class="font-size-sm">' + apiUrls.getAttribute("data-resource-checklist") + '</th> ';
                info = info + '</tr> ';
                info = info + '</thead> ';
                info = info + '<tbody>';

                for (var i = 0; i < response.grupo.length; i++) {

                    info = info + '<tr class="bg-primary-light"> ';
                    info = info + '<td class="font-w600" colspan="3">' + response.grupo[i].grupo + '</td> ';
                    info = info + '</tr> ';

                    for (var j = 0; j < response.grupo[i].subgrupo.length; j++) {

                        if (response.grupo[i].subgrupo[j].subgrupo != "") {
                            info = info + '<tr class="bg-secondary-light"> ';
                            info = info + '<td class="font-w600" colspan="3">' + response.grupo[i].subgrupo[j].subgrupo + '</td> ';
                            info = info + '</tr> ';
                        }

                        for (var k = 0; k < response.grupo[i].subgrupo[j].checklist.length; k++) {

                            info = info + '<tr class="bg-white"> ';
                            info = info + '<td class="font-size-sm">' + response.grupo[i].subgrupo[j].checklist[k].checklist + '</td> ';
                            info = info + '<td class="text-center font-size-sm ' + response.grupo[i].subgrupo[j].checklist[k].cssClass + '">' + response.grupo[i].subgrupo[j].checklist[k].resposta + '</td> ';
                            info = info + '<td class="font-size-sm">' + response.grupo[i].subgrupo[j].checklist[k].observacao + '</td> ';
                            info = info + '</tr> ';
                        }

                    }

                }

                info = info + '</tbody> ';

                info = info + '</table> ';

            }

            if (response.enxoval.length) {

                info = info + '<table class="table table-bordered table-vcenter table-striped table-sm js-dataTable-full" id="tbDetails"> ';
                info = info + '<thead> ';
                info = info + '<tr class="table-secondary"> ';
                info = info + '<th class="font-size-sm">' + apiUrls.getAttribute("data-resource-enxoval") + '</th> ';
                info = info + '<th class="text-center font-size-sm">' + apiUrls.getAttribute("data-resource-quantidade") + '</th> ';
                info = info + '<th class="text-center font-size-sm">' + apiUrls.getAttribute("data-resource-peso") + '</th> ';
                info = info + '<th class="text-center font-size-sm">' + apiUrls.getAttribute("data-resource-total") + '</th> ';
                info = info + '</tr> ';
                info = info + '</thead> ';
                info = info + '<tbody>';

                for (var i = 0; i < response.enxoval.length; i++) {

                    info = info + '<tr class="bg-white"> ';
                    info = info + '<td class="font-size-sm">' + response.enxoval[i].enxoval + '</td> ';
                    info = info + '<td class="text-center font-size-sm" style="width:100px">' + response.enxoval[i].quantidade + '</td> ';
                    info = info + '<td class="text-center font-size-sm" style="width:100px">' + response.enxoval[i].peso + '</td> ';
                    info = info + '<td class="text-center font-size-sm" style="width:100px">' + response.enxoval[i].totalPeso + '</td> ';
                    info = info + '</tr> ';
                }

                info = info + '</tbody> ';

                info = info + '</table> ';

            }

            return info;

        },
        error: function (data) {
            Codebase.layout("header_loader_off");
        }
    });

    return info;

}

function openModal(data) {

    // Init simple DataTable, for more examples you can check out https://www.datatables.net/
    table = $('#tb_main').DataTable({
        select: {
            selector: 'td:not(:first-child)',
            style: 'os'
        },
        //scrollY: '400px',
        //scrollCollapse: true,
        paging: true,
        destroy: true,
        //fixedHeader: true,
        searching: true,
        lengthChange: false,
        pageLength: 15,
        autoWidth: false,
        scrollX: false,
        scrollColapse: true,
        ajax: {
            url: "LoadApontamentoHistorico",
            type: "POST",
            datatype: "json",
            data: {
                "unidade": $("#header-unidade").val() == "" ? "-1" : $("#header-unidade").val(),
                "data": data.data,
                "tipoGovernanca": data.tipoGovernanca,
                "camareira": data.camareira,
                "naoConformidade": data.naoConformidade
            },
            dataSrc: ""
        },
        columns: [
            {
                class: "details-control",
                orderable: false,
                data: null,
                defaultContent: "",
                render: function () {
                    return '<i class="fa fa-plus-square" aria-hidden="true"></i>';
                },
                width: "15px"
            },
            { data: "data" },
            { data: "apartamento" },
            { data: "camareira" },
            { data: "horaInicio" },
            { data: "horaTermino" },
            { data: "tempoGasto" },
            { data: "tipoGovernanca" },
            { data: "naoConformidade" },
            { data: "responsavelVistoria" },
            { data: "horaVistoria" }
        ],
        columnDefs: [
            { className: 'text-center', targets: [0, 1, 2, 4, 5, 6, 8, 10] },
            { width: '120px', targets: [1, 2, 4, 5, 6, 8, 10] }
        ],
        dom: 'Bfrtip',
        buttons: [
            { extend: 'copy', text: 'Copiar' },
            { extend: 'print', text: 'Imprimir' },
            { extend: 'excel', text: 'Excel' },
            { extend: 'pdf', orientation: 'portrait' },
        ],
        language: {
            "search": "Procurar:",
            "lengthMenu": "Exibir _MENU_ ",
            "info": "_MAX_ registro(s)",
            "infoFiltered": "(filtrado de _MAX_ registro(s))"
        }
    });


    jQuery('#modal-extra-large').modal('show');

    table.on("user-select", function (e, dt, type, cell, originalEvent) {
        if ($(cell.node()).hasClass("details-control")) {
            e.preventDefault();
        }
    });

    // Add event listener for opening and closing details
    $('#tb_main tbody').on('click', 'td.details-control', function () {
        var tr = $(this).closest('tr');
        var tdi = tr.find("i.fa");
        var row = table.row(tr);

        if (row.child.isShown()) {
            // This row is already open - close it
            row.child.hide();
            tr.removeClass('shown');
            tdi.first().removeClass('fa-minus-square');
            tdi.first().addClass('fa-plus-square');
            tr.removeClass('bg-warning-light font-italic font-b');
            // tr.find('svg').attr('data-icon', 'plus-square');    // FontAwesome 5
        }
        else {
            // Open this row
            row.child(format(row.data())).show();
            tr.addClass('shown');
            tdi.first().removeClass('fa-plus-square');
            tdi.first().addClass('fa-minus-square');
            tr.addClass('bg-warning-light font-italic font-b');
            // tr.find('svg').attr('data-icon', 'minus-circle'); // FontAwesome 5
        }
    });

}

function loadNaoConformidade() {

    const unidade = document.getElementById('header-unidade').value;
    const dataInicio = document.getElementById("dataInicio").value;
    const dataTermino = document.getElementById("dataTermino").value;

    // Init simple DataTable, for more examples you can check out https://www.datatables.net/
    table = $('#tbNaoConformidade').DataTable({
        select: {
            selector: 'td:not(:first-child)',
            style: 'os'
        },
        //scrollY: '400px',
        //scrollCollapse: true,
        paging: true,
        destroy: true,
        //fixedHeader: true,
        searching: false,
        lengthChange: false,
        pageLength: 15,
        autoWidth: false,
        scrollX: false,
        scrollColapse: true,
        ajax: {
            url: "LoadGovernancaNaoConformidade",
            type: "POST",
            datatype: "json",
            data: {
                "unidade": unidade == "" ? "-1" : unidade,
                "dataInicio": dataInicio,
                "dataTermino": dataTermino
            },
            dataSrc: function (json) {
                // Verificar se a resposta contém dados
                if (json.length === 0) {
                    // Ocultar a div quando não houver dados
                    document.getElementById("divNaoConformidade").style.display = "none";
                } else {
                    // Mostrar a div caso haja dados
                    document.getElementById("divNaoConformidade").style.display = "block";
                }
                return json;
            }
        },
        columns: [
            { data: "item" },
            { data: "descricao" },
            { data: "quantidade" }
        ],
        columnDefs: [
            { className: 'text-center', targets: [0, 2] },
            { width: '120px', targets: [2] },
            { width: '50px', targets: [0] }
        ],
        buttons: [
            { extend: 'copy', text: 'Copiar' },
            { extend: 'print', text: 'Imprimir' },
            { extend: 'excel', text: 'Excel' },
            { extend: 'pdf', orientation: 'portrait' },
        ],
        language: {
            "search": "",
            "lengthMenu": "",
            "info": "",
            "infoFiltered": ""
        }
    });

}

function buildArrumadoxVistoriadoChart(chart, payload) {

    console.group('DEBUG PAYLOAD');

    console.log('Payload length:', payload.length);
    console.log('Sample row:', payload[0]);

    console.log('Saída:', payload.map(x => x.qtdeSaida));
    console.log('Permanência:', payload.map(x => x.qtdePermanencia));
    console.log('Manutenção:', payload.map(x => x.qtdeManutencao));
    console.log('Meta:', payload.map(x => x.meta));
    console.log('% Vistoriado:', payload.map(x => x.percentualVistoriado));

    console.groupEnd();
    const labels = payload.map(x => x.data);

    const saida = payload.map(x => x.qtdeSaida);
    const permanencia = payload.map(x => x.qtdePermanencia);
    const manutencao = payload.map(x => x.qtdeManutencao);
    const meta = payload.map(x => x.meta);
    const percentualVistoriado = payload.map(x => x.percentualVistoriado);

    chart.data.labels = labels;
    chart.data.datasets = [

        {
            type: 'bar',
            label: 'Qtde. Saída',
            data: saida,
            backgroundColor: '#e74c3c',
            stack: 'total',
            yAxisID: 'y'
        },

        {
            type: 'bar',
            label: 'Qtde. Permanência',
            data: permanencia,
            backgroundColor: '#3498db',
            stack: 'total',
            yAxisID: 'y'
        },

        {
            type: 'bar',
            label: 'Qtde. Manutenção',
            data: manutencao,
            backgroundColor: '#f1c40f',
            stack: 'total',
            yAxisID: 'y'
        },

        {
            type: 'line',
            label: 'Meta',
            data: meta,
            borderColor: '#2c3e50',
            borderWidth: 2,
            pointRadius: 0,
            fill: false,
            yAxisID: 'yMeta'
        },

        {
            type: 'line',
            label: '% Vistoriado',
            data: percentualVistoriado,
            borderColor: '#27ae60',
            borderDash: [6, 4],
            borderWidth: 2,
            pointRadius: 3,
            yAxisID: 'yPercent'
        }
    ];


}