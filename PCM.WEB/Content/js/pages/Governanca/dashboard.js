// Obtendo URL da API do atributo data
var apiUrls = document.getElementById("api-urls");
var chartArrumadoxVistoriadoUrl = apiUrls.getAttribute("data-chart-arrumado-vistoriado");
var chartArrumacaoDiaUrl = apiUrls.getAttribute("data-chart-arrumacao-dia");
var chartVistoriaDiaUrl = apiUrls.getAttribute("data-chart-vistoria-dia");
//var chartProdutividadeCamareiraUrl = apiUrls.getAttribute("data-chart-produtividade-camareira");
//var chartProdutividadeVistoriadorUrl = apiUrls.getAttribute("data-chart-produtividade-vistoriador");
var chartNaoConformidadeTipoUrl = apiUrls.getAttribute("data-chart-nao-conformidade-tipo");
var chartNaoConformidadeDiaUrl = apiUrls.getAttribute("data-chart-nao-conformidade-dia");
var table;
var day = 30;

// Inicializando os gráficos
var cArrumadoxVistoriado = document.getElementById('chartArrumadoxVistoriado').getContext('2d');
var cArrumacaoDia = document.getElementById('chartArrumacaoDia').getContext('2d');
var cVistoriaDia = document.getElementById('chartVistoriaDia').getContext('2d');
//var cProdutividadeCamareira = document.getElementById('chartProdutividadeCamareira').getContext('2d');
//var cProdutividadeVistoriador = document.getElementById('chartProdutividadeVistoriador').getContext('2d');
var cNCDia = document.getElementById('chartNCDia').getContext('2d'); 

document.addEventListener("DOMContentLoaded", function () {

    Codebase.helpers(['datepicker', 'maxlength', 'select2', 'easy-pie-chart']);

    if ($("#unidade").val() == "-1") {
        $("#arrumacaoCamareira").hide();
    }

    // Inicialização do gráfico Arrumado x Vistoriado
    if (cArrumadoxVistoriado) {
        window.ArrumadoxVistoriado = new Chart(cArrumadoxVistoriado, getChartConfigArrumadoxVistoriado());
        console.log("Gráfico Arrumado x Vistoriado:", window.ArrumadoxVistoriado);
        loadChartData(chartArrumadoxVistoriadoUrl, window.ArrumadoxVistoriado.config, "arrumadoxvistoriado");
    } else {
        console.error("Erro: Elemento de gráfico Arrumado x Vistoriado não encontrado.");
    }

    // Inicialização do gráfico Arrumação Dia
    if (cArrumacaoDia) {
        window.ArrumacaoDia = new Chart(cArrumacaoDia, getChartConfigArrumacaoDia());
        console.log("Gráfico Nº Arrumação Dia:", window.ArrumacaoDia);
        loadChartData(chartArrumacaoDiaUrl, window.ArrumacaoDia.config, "arrumacaoDia");
    } else {
        console.error("Erro: Elemento de gráfico Arrumação Dia não encontrado.");
    }

    // Inicialização do gráfico Vistoria Dia
    if (cVistoriaDia) {
        window.VistoriaDia = new Chart(cVistoriaDia, getChartConfigVistoriaDia());
        console.log("Gráfico Nº Vistoria Dia:", window.VistoriaDia);
        loadChartData(chartVistoriaDiaUrl, window.VistoriaDia.config, "vistoriaDia");
    } else {
        console.error("Erro: Elemento de gráfico Vistoria Dia não encontrado.");
    }

    //// Inicialização do gráfico Produtividade Camareira
    //if (cProdutividadeCamareira) {
    //    window.ProdutividadeCamareira = new Chart(cProdutividadeCamareira, getChartConfigProdutividadeCamareira());
    //    console.log("Gráfico Produtividade (Camareira):", window.ProdutividadeCamareira);
    //    loadChartData(chartProdutividadeCamareiraUrl, window.ProdutividadeCamareira.config, "produtividadeCamareira");
    //} else {
    //    console.error("Erro: Elemento de gráfico Produtividade Camareira não encontrado.");
    //}

    //// Inicialização do gráfico Produtividade Vistoriador
    //if (cProdutividadeVistoriador) {
    //    window.ProdutividadeVistoriador = new Chart(cProdutividadeVistoriador, getChartConfigProdutividadeVistoriador());
    //    console.log("Gráfico Produtividade (Vistoriador):", window.ProdutividadeVistoriador);
    //    loadChartData(chartProdutividadeVistoriadorUrl, window.ProdutividadeVistoriador.config, "produtividadeVistoriador");
    //} else {
    //    console.error("Erro: Elemento de gráfico Produtividade Vistoriador não encontrado.");
    //}

    // Inicialização do gráfico NC Dia
    if (cNCDia) {
        window.NCDia = new Chart(cNCDia, getChartConfigNCDia());
        console.log("Gráfico NC Dia:", window.NCDia);
        loadChartData(chartNaoConformidadeDiaUrl, window.NCDia.config, "ncDia");
    } else {
        console.error("Erro: Elemento de gráfico NC Dia não encontrado.");
    }


    //// Inicialização do gráfico Não Conformidade
    //if (cNaoConformidadeTipo) {
    //    window.NaoConformidadeTipo = new Chart(cNaoConformidadeTipo, getChartConfigNaoConformidadeTipo());
    //    console.log("Gráfico Camareira criado:", window.NaoConformidadeTipo);
    //    loadChartData(chartNaoConformidadeTipoUrl, window.NaoConformidadeTipo.config, "nao-conformidade-tipo");
    //} else {
    //    console.error("Erro: Elemento de gráfico Não Conformidade não encontrado.");
    //}

    //loadNaoConformidade(30)

});

$("#unidade").change(function () {
    $("#form").submit();
});

$("#modulo").change(function () {
    $("#form").submit();
});

$("#unidade").change(function () {
    $("#form").submit();
});

$("#modulo").change(function () {
    $("#form").submit();
});

$("#data").change(function () {
    $("#form").submit();
});

async function loadChartData(url, config, type) {

    const unidade = document.getElementById("unidade").value;
    const data = document.getElementById("data").value;

    const dataInicio = inicioFimMes(data).primeiroDia;
    const dataTermino = inicioFimMes(data).ultimoDia;

    try {

        const datasets = await fetchChartData(url, { unidade, dataInicio, dataTermino });

        console.log(`Dados recebidos para o gráfico ${type}:`, datasets);

        if (type === "arrumadoxvistoriado") {

            //datasets.datasets.forEach(dataset => {
            //    dataset.stack = 'Stack 0';
            //});
            updateChartConfig(config, datasets.labels, datasets.datasets);
            if (window.ArrumadoxVistoriado) {
                window.ArrumadoxVistoriado.update();
            } else {
                console.error("Erro: Arrumado x Vistoriado não foi inicializado corretamente.");
            }

        } else if (type === "arrumacaoDia") {

            updateChartConfig(config, datasets.labels, datasets.datasets);

            if (window.ArrumacaoDia) {
                window.ArrumacaoDia.update();
            } else {
                console.error("Erro: Arrumação x Dia não foi inicializado corretamente.");
            }

        } else if (type === "vistoriaDia") {

            updateChartConfig(config, datasets.labels, datasets.datasets);

            if (window.VistoriaDia) {
                window.VistoriaDia.update();
            } else {
                console.error("Erro: Gráfico de Vistoria x Dia não foi inicializado corretamente.");
            }

        } else if (type === "produtividadeCamareira") {

            updateChartConfig(config, datasets.labels, datasets.datasets);

            if (window.ProdutividadeCamareira) {
                window.ProdutividadeCamareira.update();
            } else {
                console.error("Erro: Gráfico de Produtividade x Camareira não foi inicializado corretamente.");
            }

        } else if (type === "produtividadeVistoriador") {

            updateChartConfig(config, datasets.labels, datasets.datasets);

            if (window.ProdutividadeVistoriador) {
                window.ProdutividadeVistoriador.update();
            } else {
                console.error("Erro: Gráfico de Produtividade x Vistoriador não foi inicializado corretamente.");
            }

        } else if (type === "ncDia") {

            updateChartConfig(config, datasets.labels, datasets.datasets);

            if (window.NCDia) {
                window.NCDia.update();
            } else {
                console.error("Erro: Gráfico de NC Dia não foi inicializado corretamente.");
            }

        }

    } catch (error) {
        console.error("Erro ao carregar dados do gráfico:", error);
    }
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

function processChartDataNaoConformidadeTipo(datasets) {
    const labels = datasets.map(item => item.naoConformidadeTipo);
    const dataSeries = datasets.map(item => Number(item.quantidade));

    return {
        labels: labels,
        dataSeries: [{
            data: dataSeries,
            backgroundColor: [
                'rgba(255, 99, 132, 0.2)',
                'rgba(54, 162, 235, 0.2)',
                'rgba(255, 206, 86, 0.2)',
                'rgba(75, 192, 192, 0.2)',
                'rgba(153, 102, 255, 0.2)',
                'rgba(255, 159, 64, 0.2)'
            ],
            borderColor: [
                'rgba(255, 99, 132, 1)',
                'rgba(54, 162, 235, 1)',
                'rgba(255, 206, 86, 1)',
                'rgba(75, 192, 192, 1)',
                'rgba(153, 102, 255, 1)',
                'rgba(255, 159, 64, 1)'
            ],
            borderWidth: 1
        }]
    };
}

function getChartConfigArrumadoxVistoriado() {

    return {
        type: 'bar',
        options: {
            responsive: true,
            scales: {
                y: {
                    type: 'number',
                    beginAtZero: true
                },
                x: {
                    type: 'time',
                    time: {
                        unit: 'day',
                        displayFormats: {
                            day: 'DD/MM/YYYY'
                        },
                        tooltipFormat: 'DD/MM/YYYY'
                    }
                }
            },
            plugins: {
                legend: {
                    display: true,
                    position: 'bottom' // Aqui também
                },
                tooltip: {
                    mode: 'nearest', // Alterado para 'nearest' em vez de 'index'
                    intersect: true  // Certifique-se de que esteja definido para true
                }
            },
            onClick: function (evt) {
                const elements = this.getElementsAtEventForMode(evt, 'nearest', { intersect: true }, true);

                if (elements.length > 0) {
                    const element = elements[0];
                    const datasetIndex = element._datasetIndex;
                    const index = element._index;

                    const label = this.data.labels[index];
                    const datasetLabel = this.data.datasets[datasetIndex].label;
                    const value = this.data.datasets[datasetIndex].data[index];

                    if (label && datasetLabel && typeof value !== 'undefined' && value != "0") {
                        openModal({
                            data: label,
                            tipoGovernanca: datasetLabel,
                            camareira: "",
                            naoConformidade: ""
                        });
                    } else {
                        console.error("Erro ao acessar os dados do ponto do gráfico.");
                    }
                } else {
                    console.error("Nenhum elemento clicado foi encontrado.");
                }
            }

        },
        data: {
            labels: [],
            datasets: []
        }
    };
}

function getChartConfigArrumacaoDia() {
    return {
        type: 'bar',
        options: {
            responsive: true,
            scales: {
                y: {
                    beginAtZero: true
                },
                x: {
                    type: 'time',
                    time: {
                        unit: 'day',
                        displayFormats: {
                            day: 'DD/MM/YYYY'
                        },
                        tooltipFormat: 'DD/MM/YYYY'
                    }
                }
            },
            plugins: {
                legend: {
                    display: true,
                    position: 'bottom' // Aqui também
                },
                tooltip: {
                    mode: 'nearest', // Alterado para 'nearest' em vez de 'index'
                    intersect: true  // Certifique-se de que esteja definido para true
                }
            },
            onClick: function (evt) {
                const elements = this.getElementsAtEventForMode(evt, 'nearest', { intersect: true }, true);

                if (elements.length > 0) {
                    const element = elements[0];
                    const datasetIndex = element._datasetIndex;
                    const index = element._index;

                    const label = this.data.labels[index];
                    const datasetLabel = this.data.datasets[datasetIndex].label;
                    const value = this.data.datasets[datasetIndex].data[index];

                    if (label && datasetLabel && typeof value !== 'undefined' && value != "0") {
                        openModal({
                            data: label,
                            tipoGovernanca: datasetLabel,
                            camareira: "",
                            naoConformidade: ""
                        });
                    } else {
                        console.error("Erro ao acessar os dados do ponto do gráfico.");
                    }
                } else {
                    console.error("Nenhum elemento clicado foi encontrado.");
                }
            }

        },
        data: {
            labels: [],
            datasets: []
        }
    };
}

function getChartConfigVistoriaDia() {
    return {
        type: 'bar',
        options: {
            responsive: true,
            scales: {
                y: {
                    beginAtZero: true
                },
                x: {
                    type: 'time',
                    time: {
                        unit: 'day',
                        displayFormats: {
                            day: 'DD/MM/YYYY'
                        },
                        tooltipFormat: 'DD/MM/YYYY'
                    }
                }
            },
            plugins: {
                legend: {
                    display: true
                },
                tooltip: {
                    mode: 'nearest', // Alterado para 'nearest' em vez de 'index'
                    intersect: true  // Certifique-se de que esteja definido para true
                }
            },
            onClick: function (evt) {
                const elements = this.getElementsAtEventForMode(evt, 'nearest', { intersect: true }, true);

                if (elements.length > 0) {
                    const element = elements[0];
                    const datasetIndex = element._datasetIndex;
                    const index = element._index;

                    const label = this.data.labels[index];
                    const datasetLabel = this.data.datasets[datasetIndex].label;
                    const value = this.data.datasets[datasetIndex].data[index];

                    if (label && datasetLabel && typeof value !== 'undefined' && value != "0") {
                        openModal({
                            data: label,
                            tipoGovernanca: datasetLabel,
                            camareira: "",
                            naoConformidade: ""
                        });
                    } else {
                        console.error("Erro ao acessar os dados do ponto do gráfico.");
                    }
                } else {
                    console.error("Nenhum elemento clicado foi encontrado.");
                }
            }

        },
        data: {
            labels: [],
            datasets: []
        }
    };
}

function getChartConfigProdutividadeCamareira() {
    return {
        type: 'bar',
        options: {
            responsive: true,
            scales: {
                y: {
                    beginAtZero: true
                },
                x: {
                    type: 'time',
                    time: {
                        unit: 'day',
                        displayFormats: {
                            day: 'DD/MM/YYYY'
                        },
                        tooltipFormat: 'DD/MM/YYYY'
                    }
                }
            },
            plugins: {
                legend: {
                    display: true
                },
                tooltip: {
                    mode: 'nearest', // Alterado para 'nearest' em vez de 'index'
                    intersect: true  // Certifique-se de que esteja definido para true
                }
            },
            onClick: function (evt) {
                const elements = this.getElementsAtEventForMode(evt, 'nearest', { intersect: true }, true);

                if (elements.length > 0) {
                    const element = elements[0];
                    const datasetIndex = element._datasetIndex;
                    const index = element._index;

                    const label = this.data.labels[index];
                    const datasetLabel = this.data.datasets[datasetIndex].label;
                    const value = this.data.datasets[datasetIndex].data[index];

                    if (label && datasetLabel && typeof value !== 'undefined' && value != "0") {
                        openModal({
                            data: label,
                            tipoGovernanca: datasetLabel,
                            camareira: "",
                            naoConformidade: ""
                        });
                    } else {
                        console.error("Erro ao acessar os dados do ponto do gráfico.");
                    }
                } else {
                    console.error("Nenhum elemento clicado foi encontrado.");
                }
            }

        },
        data: {
            labels: [],
            datasets: []
        }
    };
}

function getChartConfigProdutividadeVistoriador() {
    return {
        type: 'bar',
        options: {
            responsive: true,
            scales: {
                y: {
                    beginAtZero: true
                },
                x: {
                    type: 'time',
                    time: {
                        unit: 'day',
                        displayFormats: {
                            day: 'DD/MM/YYYY'
                        },
                        tooltipFormat: 'DD/MM/YYYY'
                    }
                }
            },
            plugins: {
                legend: {
                    display: true
                },
                tooltip: {
                    mode: 'nearest', // Alterado para 'nearest' em vez de 'index'
                    intersect: true  // Certifique-se de que esteja definido para true
                }
            },
            onClick: function (evt) {
                const elements = this.getElementsAtEventForMode(evt, 'nearest', { intersect: true }, true);

                if (elements.length > 0) {
                    const element = elements[0];
                    const datasetIndex = element._datasetIndex;
                    const index = element._index;

                    const label = this.data.labels[index];
                    const datasetLabel = this.data.datasets[datasetIndex].label;
                    const value = this.data.datasets[datasetIndex].data[index];

                    if (label && datasetLabel && typeof value !== 'undefined' && value != "0") {
                        openModal({
                            data: label,
                            tipoGovernanca: datasetLabel,
                            camareira: "",
                            naoConformidade: ""
                        });
                    } else {
                        console.error("Erro ao acessar os dados do ponto do gráfico.");
                    }
                } else {
                    console.error("Nenhum elemento clicado foi encontrado.");
                }
            }

        },
        data: {
            labels: [],
            datasets: []
        }
    };
}

function getChartConfigNCDia() {
    return {
        type: 'line',
        options: {
            responsive: true,
            scales: {
                y: {
                    beginAtZero: true
                },
                x: {
                    type: 'time',
                    time: {
                        unit: 'day',
                        displayFormats: {
                            day: 'DD/MM/YYYY'
                        },
                        tooltipFormat: 'DD/MM/YYYY'
                    }
                }
            },
            plugins: {
                legend: {
                    display: true,
                    position: 'bottom'
                },
                tooltip: {
                    mode: 'nearest', // Alterado para 'nearest' em vez de 'index'
                    intersect: true  // Certifique-se de que esteja definido para true
                }
            },
            onClick: function (evt) {
                const elements = this.getElementsAtEventForMode(evt, 'nearest', { intersect: true }, true);

                if (elements.length > 0) {
                    const element = elements[0];
                    const datasetIndex = element._datasetIndex;
                    const index = element._index;

                    const label = this.data.labels[index];
                    const datasetLabel = this.data.datasets[datasetIndex].label;
                    const value = this.data.datasets[datasetIndex].data[index];

                    if (label && datasetLabel && typeof value !== 'undefined' && value != "0") {
                        openModal({
                            data: label,
                            tipoGovernanca: datasetLabel,
                            camareira: "",
                            naoConformidade: ""
                        });
                    } else {
                        console.error("Erro ao acessar os dados do ponto do gráfico.");
                    }
                } else {
                    console.error("Nenhum elemento clicado foi encontrado.");
                }
            }

        },
        data: {
            labels: [],
            datasets: []
        }
    };
}

function getChartConfigNaoConformidadeTipo() {
    return {
        type: 'doughnut',
        options: {
            responsive: true,
            plugins: {
                legend: {
                    display: true,
                    position: 'top'
                },
                tooltip: {
                    callbacks: {
                        label: function (context) {
                            const label = context.label || '';
                            const value = context.raw || 0;
                            return `${label}: ${value}`;
                        }
                    }
                }
            },
            onClick: function (evt) {
                const elements = this.getElementsAtEventForMode(evt, 'nearest', { intersect: true }, true);

                if (elements.length > 0) {
                    const element = elements[0];
                    const datasetIndex = element._datasetIndex;
                    const index = element._index;

                    const label = this.data.labels[index];
                    const datasetLabel = this.data.datasets[datasetIndex].label;
                    const value = this.data.datasets[datasetIndex].data[index];

                    if (label && datasetLabel && typeof value !== 'undefined' && value != "0") {
                        openModal({
                            data: label,
                            tipoGovernanca: "",
                            camareira: "",
                            naoConformidade: datasetLabel
                        });
                    } else {
                        console.error("Erro ao acessar os dados do ponto do gráfico.");
                    }
                } else {
                    console.error("Nenhum elemento clicado foi encontrado.");
                }
            }
        },
        data: {
            labels: [], // Tipos de Não Conformidade
            datasets: [{
                data: [], // Quantidades
                backgroundColor: [
                    'rgba(255, 99, 132, 0.2)',
                    'rgba(54, 162, 235, 0.2)',
                    'rgba(255, 206, 86, 0.2)',
                    'rgba(75, 192, 192, 0.2)',
                    'rgba(153, 102, 255, 0.2)',
                    'rgba(255, 159, 64, 0.2)'
                ],
                borderColor: [
                    'rgba(255, 99, 132, 1)',
                    'rgba(54, 162, 235, 1)',
                    'rgba(255, 206, 86, 1)',
                    'rgba(75, 192, 192, 1)',
                    'rgba(153, 102, 255, 1)',
                    'rgba(255, 159, 64, 1)'
                ],
                borderWidth: 1
            }]
        }
    };
}

function updateChartConfig(config, labels, dataSeries) {
    config.data.labels = labels;
    config.data.datasets = dataSeries;
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
                "unidade": $("#unidade").val() == "" ? "-1" : $("#unidade").val(),
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

    const unidade = document.getElementById("unidade").value;
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
