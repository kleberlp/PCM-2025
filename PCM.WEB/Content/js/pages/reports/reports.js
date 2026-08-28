/*
 * Comportamento compartilhado para relatórios HTML do PCM (Relatorios/*).
 * Par do reports.css. Escrito sem handlers inline (onclick/onerror) para seguir
 * o padrão de CSP do projeto (script-src sem 'unsafe-inline'). Cada bloco checa
 * se os elementos existem antes de ligar os listeners, já que nem todo relatório
 * usa todos os recursos (filtro, grupos recolhíveis, lightbox de fotos).
 */
(function () {
    'use strict';

    function ligarFiltro() {
        var filtroTexto = document.getElementById('filtroTexto');
        var filtroResultado = document.getElementById('filtroResultado');
        var contador = document.getElementById('contadorVisivel');
        var grupos = Array.prototype.slice.call(document.querySelectorAll('.grupo'));

        if (!filtroTexto && !filtroResultado) { return; }

        function aplicarFiltro() {
            var termo = filtroTexto ? filtroTexto.value.trim().toLowerCase() : '';
            var status = filtroResultado ? filtroResultado.value : '';
            var visiveis = 0;

            grupos.forEach(function (grupo) {
                var itens = grupo.querySelectorAll('.item-checklist');
                var algumVisivelNoGrupo = false;

                itens.forEach(function (item) {
                    var correspondeTexto = !termo || (item.getAttribute('data-busca') || '').indexOf(termo) !== -1;
                    var correspondeStatus = !status || item.getAttribute('data-status') === status;
                    var corresponde = correspondeTexto && correspondeStatus;

                    item.classList.toggle('oculto-por-filtro', !corresponde);
                    if (corresponde) {
                        algumVisivelNoGrupo = true;
                        if (item.getAttribute('data-status')) { visiveis++; }
                    }
                });

                grupo.classList.toggle('oculto-por-filtro', !algumVisivelNoGrupo);
            });

            if (contador) { contador.textContent = visiveis; }
        }

        if (filtroTexto) { filtroTexto.addEventListener('input', aplicarFiltro); }
        if (filtroResultado) { filtroResultado.addEventListener('change', aplicarFiltro); }
    }

    function ligarExpandirTudo() {
        var btnExpandir = document.getElementById('btnExpandir');
        var grupos = Array.prototype.slice.call(document.querySelectorAll('.grupo'));
        if (!btnExpandir) { return; }

        var expandido = false;
        btnExpandir.addEventListener('click', function () {
            expandido = !expandido;
            grupos.forEach(function (grupo) { grupo.classList.toggle('recolhido', !expandido); });
            btnExpandir.textContent = expandido ? 'Recolher tudo' : 'Expandir tudo';
        });
    }

    function ligarToggleGrupo() {
        document.querySelectorAll('.grupo-cabecalho').forEach(function (cabecalho) {
            cabecalho.addEventListener('click', function () {
                var grupo = cabecalho.closest('.grupo');
                if (grupo) { grupo.classList.toggle('recolhido'); }
            });
        });
    }

    function ligarBotaoImprimir() {
        var btn = document.getElementById('btnImprimir');
        if (btn) { btn.addEventListener('click', function () { window.print(); }); }
    }

    function ligarLightbox() {
        var lightbox = document.getElementById('lightbox');
        var lightboxImg = document.getElementById('lightboxImg');
        if (!lightbox || !lightboxImg) { return; }

        document.querySelectorAll('.foto-card[data-foto]').forEach(function (card) {
            card.addEventListener('click', function () {
                lightboxImg.src = card.getAttribute('data-foto');
                lightbox.classList.add('aberto');
            });
        });

        lightbox.addEventListener('click', function () {
            lightbox.classList.remove('aberto');
        });
    }

    // Esconde uma imagem (ex.: logo do timbre) caso o arquivo não exista/não carregue,
    // em vez de deixar o ícone de imagem quebrada. Marque a tag com data-fallback-hide.
    function ligarFallbackImagens() {
        document.querySelectorAll('img[data-fallback-hide]').forEach(function (img) {
            img.addEventListener('error', function () { img.style.visibility = 'hidden'; });
        });
    }

    document.addEventListener('DOMContentLoaded', function () {
        ligarFiltro();
        ligarExpandirTudo();
        ligarToggleGrupo();
        ligarBotaoImprimir();
        ligarLightbox();
        ligarFallbackImagens();
    });
})();
