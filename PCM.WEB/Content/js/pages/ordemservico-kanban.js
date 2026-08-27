// ============================================================
//  ordemservico-kanban.js — quadro Kanban de Ordens de Serviço
//  (OrdemServico/OrdemServicoKanban)
//
//  Padrão CSP: nenhum handler/estilo inline; parâmetros do servidor
//  chegam pelos data-* do #kb-root; DOM montado via createElement +
//  textContent (nunca HTML concatenado com dado do banco).
// ============================================================

(function ($) {
    'use strict';

    var $root = $('#kb-root');
    if (!$root.length) { return; }

    var cfg = {
        urlLista: $root.data('url-lista'),
        urlStatus: $root.data('url-status'),
        urlView: $root.data('url-view'),
        empresa: $root.data('empresa'),
        usuario: $root.data('usuario'),
        unidade: $root.data('unidade'),
        dataInicio: $root.data('data-inicio'),
        dataTermino: $root.data('data-termino'),
        editar: String($root.data('editar')) === '1'
    };

    var ordens = [];   // última carga do servidor
    var busca = '';

    // ---------- utilidades ----------

    // "dd/MM/yyyy" -> Date (meia-noite local); null se inválida
    function parseData(sData) {
        if (!sData) { return null; }
        var p = String(sData).split('/');
        if (p.length !== 3) { return null; }
        var d = new Date(Number(p[2]), Number(p[1]) - 1, Number(p[0]));
        return isNaN(d.getTime()) ? null : d;
    }

    function diasAtraso(os) {
        var alvo = parseData(os.data_necessidade);
        if (!alvo) { return 0; }
        var hoje = new Date();
        hoje.setHours(0, 0, 0, 0);
        var diff = Math.floor((hoje - alvo) / 86400000);
        return diff > 0 ? diff : 0;
    }

    function classePrioridade(nome) {
        var s = (nome || '').toUpperCase();
        if (s.indexOf('ALTA') >= 0 || s.indexOf('URGEN') >= 0) { return 'kb-chip-prioridade'; }
        if (s.indexOf('MÉDIA') >= 0 || s.indexOf('MEDIA') >= 0) { return 'kb-chip-prioridade-media'; }
        if (s.indexOf('BAIXA') >= 0) { return 'kb-chip-prioridade-baixa'; }
        return '';
    }

    function combina(os) {
        if (!busca) { return true; }
        var alvo = [os.executor, os.local, os.setor, os.equipamento,
                    os.numero_documento, os.descricao, os.categoria]
            .join(' ').toUpperCase();
        return alvo.indexOf(busca) >= 0;
    }

    // ---------- montagem dos cartões ----------

    function chip(texto, classeExtra) {
        var el = document.createElement('span');
        el.className = 'kb-chip' + (classeExtra ? ' ' + classeExtra : '');
        el.textContent = texto;
        return el;
    }

    function linhaIcone(classeLinha, classeIcone, texto) {
        var linha = document.createElement('div');
        linha.className = classeLinha;
        var icone = document.createElement('i');
        icone.className = 'fa ' + classeIcone;
        linha.appendChild(icone);
        linha.appendChild(document.createTextNode(' ' + texto));
        return linha;
    }

    function montaCartao(os) {
        var atraso = (os.status !== 4) ? diasAtraso(os) : 0;

        var card = document.createElement('article');
        card.className = 'kb-card' + (atraso ? ' kb-atrasada' : '');
        card.setAttribute('data-codigo', os.codigo);
        card.setAttribute('data-unidade', os.codigo_unidade);
        if (cfg.editar) { card.setAttribute('draggable', 'true'); }

        var chips = document.createElement('div');
        chips.className = 'kb-card-chips';
        if (os.categoria) { chips.appendChild(chip(os.categoria)); }
        if (os.prioridade) { chips.appendChild(chip(os.prioridade, classePrioridade(os.prioridade))); }
        card.appendChild(chips);

        var local = os.local || os.setor || os.unidade || '';
        if (local) { card.appendChild(linhaIcone('kb-card-local', 'fa-bed', local)); }

        if (os.descricao) {
            var desc = document.createElement('div');
            desc.className = 'kb-card-descricao';
            desc.textContent = os.descricao;
            card.appendChild(desc);
        }

        if (os.numero_documento) {
            card.appendChild(linhaIcone('kb-card-numero', 'fa-tag', os.numero_documento));
        }

        if (atraso) {
            card.appendChild(linhaIcone('kb-card-atraso', 'fa-exclamation-circle',
                'Atraso + ' + atraso + (atraso === 1 ? ' dia' : ' dias')));
        }

        if (os.executor) {
            var exec = document.createElement('div');
            exec.className = 'kb-card-executor';
            exec.textContent = os.executor;
            card.appendChild(exec);
        }

        return card;
    }

    function render() {
        var visiveis = 0;

        $('.kb-cards').empty();

        var contagem = { 1: 0, 2: 0, 3: 0, 4: 0 };

        for (var i = 0; i < ordens.length; i++) {
            var os = ordens[i];
            if (!contagem.hasOwnProperty(os.status)) { continue; }
            if (!combina(os)) { continue; }

            contagem[os.status]++;
            visiveis++;
            $('[data-cards="' + os.status + '"]').append(montaCartao(os));
        }

        $.each(contagem, function (status, n) {
            $('[data-count="' + status + '"]').text(n);
        });

        $('#kb-vazio').toggleClass('kb-oculto', visiveis > 0);
    }

    // ---------- carga ----------

    function carregar() {
        $.getJSON(cfg.urlLista, {
            empresa: cfg.empresa,
            usuario: cfg.usuario,
            unidade: $('#kb-unidade').val() || cfg.unidade,
            departamento: -1,
            ordem_servico: '',
            ordem_servico_cliente: '',
            data_inicio: cfg.dataInicio,
            data_termino: cfg.dataTermino,
            status: -1
        }).done(function (dados) {
            ordens = dados || [];
            render();
        }).fail(function () {
            Swal.fire({ text: 'Falha ao carregar as ordens de serviço.', icon: 'error' });
        });
    }

    // ---------- eventos ----------

    $('#kb-busca').on('input', function () {
        busca = String($(this).val() || '').toUpperCase();
        render();
    });

    $('#kb-unidade').on('change', carregar);
    $('#kb-atualizar').on('click', carregar);

    // abrir a O.S. (clique simples no cartão)
    $root.on('click', '.kb-card', function () {
        window.location.href = cfg.urlView +
            '?codigo=' + encodeURIComponent($(this).data('codigo')) +
            '&unidade=' + encodeURIComponent($(this).data('unidade'));
    });

    // coluna Concluídas recolhida <-> aberta
    $('#kb-col-concluidas').on('click', function (e) {
        if ($(this).hasClass('kb-col-aberta') && !$(e.target).closest('.kb-col-header').length) { return; }
        $(this).toggleClass('kb-col-fechada kb-col-aberta');
    });

    // ---------- arrastar e soltar (só com direito de edição) ----------

    if (cfg.editar) {

        var arrastando = null;

        $root.on('dragstart', '.kb-card', function (e) {
            arrastando = this;
            $(this).addClass('kb-arrastando');
            e.originalEvent.dataTransfer.effectAllowed = 'move';
            e.originalEvent.dataTransfer.setData('text/plain', $(this).data('codigo'));
        });

        $root.on('dragend', '.kb-card', function () {
            $(this).removeClass('kb-arrastando');
            $('.kb-col').removeClass('kb-drop-alvo');
            arrastando = null;
        });

        $root.on('dragover', '.kb-col', function (e) {
            e.preventDefault();
            e.originalEvent.dataTransfer.dropEffect = 'move';
            $(this).addClass('kb-drop-alvo');
        });

        $root.on('dragleave', '.kb-col', function () {
            $(this).removeClass('kb-drop-alvo');
        });

        $root.on('drop', '.kb-col', function (e) {
            e.preventDefault();
            $(this).removeClass('kb-drop-alvo');

            if (!arrastando) { return; }

            var novoStatus = Number($(this).attr('data-status'));
            var codigo = $(arrastando).data('codigo');
            var unidade = $(arrastando).data('unidade');

            var os = null;
            for (var i = 0; i < ordens.length; i++) {
                if (ordens[i].codigo === codigo) { os = ordens[i]; break; }
            }
            if (!os || os.status === novoStatus) { return; }

            var statusAnterior = os.status;
            os.status = novoStatus;
            render();

            $.getJSON(cfg.urlStatus, { codigo: codigo, unidade: unidade, status: novoStatus })
                .fail(function () {
                    os.status = statusAnterior;
                    render();
                    Swal.fire({ text: 'Não foi possível mover a O.S.', icon: 'error' });
                });
        });
    }

    // primeira carga
    carregar();

})(jQuery);
