
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
        customAction: true,
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
        },
        customButtons: [
            {
                action: "ficha",
                icon: "fa fa-eye",
                class: "btn btn-sm btn-outline-secondary",
                title: messages.clickToView,
                onClick: (row) => {
                    abrirFicha(row);
                }
            }
        ],
        cellRender: {
            assetCode: function (valor) {
                return valor ? "<span class='af-code'>" + valor + "</span>" : "";
            },
            status: function (valor) {
                if (!valor) return "";

                var mapa = {
                    "ATIVO": "af-ok",
                    "MANUTENÇÃO": "af-warn",
                    "MANUTENCAO": "af-warn",
                    "BAIXADO": "af-crit",
                    "TRANSFERIDO": "af-brand"
                };

                var classe = mapa[String(valor).toUpperCase()] || "";
                return "<span class='af-chip " + classe + "'>" + valor + "</span>";
            },
            ultimaMovimentacao: function (valor) {
                return valor && valor !== "—" ? "<span class='af-code'>" + valor + "</span>" : "—";
            },
            valorCompra: function (valor) {
                return valor && valor !== "—" ? "<span class='af-code'>" + valor + "</span>" : "—";
            }
        },
        onLoaded: function (response, rows) {
            var total = (rows || []).length;
            $("#contagemAtivos").text(total === 1 ? "1 resultado" : total + " resultados");
            $("#resumoAtivos").text(total === 1 ? "1 ativo listado" : total + " ativos listados");
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

// ============================================================
//  Ficha do ativo (painel lateral)
//  Abre ao lado da lista, sem trocar de página.
// ============================================================
function escaparHtml(valor) {
    return $("<div>").text(valor == null ? "" : valor).html();
}

function linhaFicha(rotulo, valor) {
    if (valor == null || valor === "" || valor === "0" || valor === 0) return "";

    return "<div class='af-dl-row'>" +
           "<span class='af-k'>" + escaparHtml(rotulo) + "</span>" +
           "<span class='af-v'>" + escaparHtml(valor) + "</span>" +
           "</div>";
}

function abrirFicha(row) {

    // Cabeçalho e status saem da própria linha já carregada: o painel abre na hora
    $("#fichaDescricao").text(row.descricao || "");
    $("#fichaCodigo").text(row.assetCode || "");
    $("#fichaEditar").attr("href", messages.urlAssetEdit + "?codigo=" + row.codigo);

    $("#fichaDados").html(
        linhaFicha(messages.status, row.status) +
        linhaFicha(messages.localizacao, row.local)
    );

    $("#fichaHistorico").html("<div class='af-empty'>Carregando...</div>");
    $("#afSplit").addClass("af-open");

    $.ajax({
        type: "POST",
        url: messages.urlLoadAssetFicha,
        data: { codigo: row.codigo },
        success: function (ficha) {

            if (!ficha || !ficha.success) {
                $("#fichaHistorico").html("<div class='af-empty'>Não foi possível carregar a ficha.</div>");
                return;
            }

            $("#fichaDados").html(
                linhaFicha(messages.status, row.status) +
                linhaFicha(messages.localizacao, row.local) +
                linhaFicha(messages.numeroSerie, ficha.numeroSerie) +
                linhaFicha(messages.tag, ficha.tag) +
                linhaFicha(messages.contaContabil, ficha.contaContabil) +
                linhaFicha(messages.notaFiscal, ficha.notaFiscal) +
                linhaFicha(messages.dataCompra, ficha.dataCompra) +
                linhaFicha(messages.valorCompra, row.valorCompra) +
                linhaFicha(messages.tempoDepreciacaoMes, ficha.tempoDepreciacaoMes)
            );

            var historico = ficha.historico || [];

            if (historico.length === 0) {
                $("#fichaHistorico").html("<div class='af-empty'>Nenhum registro para este ativo.</div>");
                return;
            }

            var html = "";

            historico.forEach(function (h) {

                var marcador = (h.marcador === "ok" || h.marcador === "warn") ? h.marcador : "neutro";
                var rodape = h.data + (h.usuario ? " · " + h.usuario : "");

                html += "<div class='af-tl-item'>" +
                        "<span class='af-dot af-" + marcador + "'></span>" +
                        "<div>" +
                        "<div class='af-tl-txt'>" + escaparHtml(h.titulo) + "</div>" +
                        (h.detalhe ? "<div class='af-tl-det'>" + escaparHtml(h.detalhe) + "</div>" : "") +
                        "<div class='af-tl-when'>" + escaparHtml(rodape) + "</div>" +
                        "</div></div>";
            });

            $("#fichaHistorico").html(html);
        },
        error: function () {
            $("#fichaHistorico").html("<div class='af-empty'>Não foi possível carregar a ficha.</div>");
        }
    });
}

$(document).on("click", "#fichaFechar", function () {
    $("#afSplit").removeClass("af-open");
});
