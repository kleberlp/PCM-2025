// ============================================================
//  ordemservico-kanban.js — quadro Kanban de Ordens de Serviço
//  (OrdemServico/OrdemServicoKanban)
//
//  Cartão (aprovado no mockup):
//    [origem] [prioridade]  Nº O.S. (destaque)
//    setor - local
//    descrição
//    relógio desde a abertura · aberta dd/MM/yyyy HH:mm
//
//  Relógio colorido pela data_necessidade: vermelho hoje > prazo,
//  amarelo hoje = prazo, verde hoje < prazo (neutro em concluída).
//
//  Status reais da tb_pcm_ordem_servico: 1 Em Aberto, 3 Atrasada
//  (derivado pela SP — coluna não aceita drop), 5 Vinculada, 4 Concluída.
//
//  Padrão CSP: nenhum handler/estilo inline; parâmetros via data-* do
//  #kb-root; DOM via createElement + textContent (nada de HTML
//  concatenado com dado do banco).
// ============================================================

(function ($) {
    'use strict';

    var $root = $('#kb-root');
    if (!$root.length) { return; }

    var cfg = {
        urlLista: $root.data('url-lista'),
        urlStatus: $root.data('url-status'),
        urlApontamento: $root.data('url-apontamento'),
        empresa: $root.data('empresa'),
        usuario: $root.data('usuario'),
        unidade: $root.data('unidade'),
        dataInicio: $root.data('data-inicio'),
        dataTermino: $root.data('data-termino'),
        editar: String($root.data('editar')) === '1'
    };

    var STATUS_COLUNAS = [1, 3, 5, 4];

    var ordens = [];   // última carga do servidor
    var busca = '';

    // ---------- datas ----------

    // "dd/MM/yyyy" ou "dd/MM/yyyy HH:mm" -> Date; null se inválida
    function parseData(sData) {
        if (!sData) { return null; }
        var m = String(sData).match(/^(\d{2})\/(\d{2})\/(\d{4})(?:\s+(\d{2}):(\d{2}))?/);
        if (!m) { return null; }
        var d = new Date(Number(m[3]), Number(m[2]) - 1, Number(m[1]),
                         Number(m[4] || 0), Number(m[5] || 0));
        return isNaN(d.getTime()) ? null : d;
    }

    function dois(n) { return (n < 10 ? '0' : '') + n; }

    function fmtDecorrido(ms) {
        var s = Math.max(0, Math.floor(ms / 1000));
        return dois(Math.floor(s / 3600)) + ':' + dois(Math.floor(s / 60) % 60) + ':' + dois(s % 60);
    }

    // vermelho: hoje > data_necessidade | amarelo: hoje = | verde: hoje <
    function classePrazo(os) {
        if (os.status === 4) { return 'kb-rel-neutro'; }
        var prazo = parseData(os.data_necessidade);
        if (!prazo) { return 'kb-rel-neutro'; }
        prazo.setHours(0, 0, 0, 0);
        var hoje = new Date();
        hoje.setHours(0, 0, 0, 0);
        if (hoje > prazo) { return 'kb-rel-vermelho'; }
        if (hoje.getTime() === prazo.getTime()) { return 'kb-rel-amarelo'; }
        return 'kb-rel-verde';
    }

    function combina(os) {
        if (!busca) { return true; }
        var alvo = [os.origem, os.setor, os.local, os.descricao,
                    os.numero_documento, os.executor, os.equipamento]
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

    // Cor da prioridade: 0 critico vermelho · 1 alta laranja · 2 media mostarda
    // · 3 baixa amarelo claro.
    //
    // A descricao cadastrada ja carrega o numero ("0- CRITICA", "1- ALTA"), e ele
    // e a fonte da verdade do negocio — o codigo da tb_cad_prioridade e identity e
    // pode nao coincidir. Por isso: numero da descricao > nome > codigo.
    var CLASSES_PRI = ['kb-pri-critica', 'kb-pri-alta', 'kb-pri-media', 'kb-pri-baixa'];

    function classePrioridade(os) {
        var nome = String(os.prioridade || '').toUpperCase();

        var numero = nome.match(/^\s*([0-3])\b/);
        if (numero) { return CLASSES_PRI[Number(numero[1])]; }

        if (nome.indexOf('CRITIC') >= 0 || nome.indexOf('CRÍTIC') >= 0) { return 'kb-pri-critica'; }
        if (nome.indexOf('ALTA') >= 0 || nome.indexOf('URGEN') >= 0) { return 'kb-pri-alta'; }
        if (nome.indexOf('MEDIA') >= 0 || nome.indexOf('MÉDIA') >= 0) { return 'kb-pri-media'; }
        if (nome.indexOf('BAIXA') >= 0) { return 'kb-pri-baixa'; }

        var codigo = Number(os.codigo_prioridade);
        return (codigo >= 0 && codigo <= 3) ? CLASSES_PRI[codigo] : '';
    }

    // Ordem de prioridade para a ordenacao, pela mesma regra da cor
    function pesoPrioridade(os) {
        var cls = classePrioridade(os);
        var idx = CLASSES_PRI.indexOf(cls);
        return idx >= 0 ? idx : 99;
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
        var prazoCls = classePrazo(os);
        var abertoEm = parseData(os.data);

        var card = document.createElement('article');
        card.className = 'kb-card' + (prazoCls === 'kb-rel-vermelho' ? ' kb-atrasada' : '');
        card.setAttribute('data-codigo', os.codigo);
        card.setAttribute('data-unidade', os.codigo_unidade);
        if (cfg.editar && os.status !== 3) { card.setAttribute('draggable', 'true'); }

        // linha 1: origem + prioridade + nº da O.S. em destaque
        var chips = document.createElement('div');
        chips.className = 'kb-card-chips';
        if (os.origem) { chips.appendChild(chip(os.origem)); }
        if (os.prioridade) { chips.appendChild(chip(os.prioridade, classePrioridade(os))); }
        card.appendChild(chips);

        if (os.numero_documento) {
            card.appendChild(linhaIcone('kb-card-local', 'fa-tag', os.numero_documento));
        }

        // linha 2: setor - local
        var setorLocal = [os.setor, os.local].filter(function (v) { return v; }).join(' - ');
        if (setorLocal) { card.appendChild(linhaIcone('kb-card-numero', 'fa-map-marker', setorLocal)); }

        // linha 3: descrição
        if (os.descricao) {
            var desc = document.createElement('div');
            desc.className = 'kb-card-descricao';
            desc.textContent = os.descricao;
            card.appendChild(desc);
        }

        // linha 4: relógio desde a abertura + data/hora da abertura
        if (abertoEm) {
            var rel = document.createElement('div');
            rel.className = 'kb-card-relogio ' + prazoCls;
            rel.setAttribute('data-aberto-em', abertoEm.getTime());

            var icone = document.createElement('i');
            icone.className = 'fa fa-clock-o';
            rel.appendChild(icone);

            var tempo = document.createElement('span');
            tempo.className = 'kb-rel-tempo';
            tempo.textContent = fmtDecorrido(Date.now() - abertoEm.getTime());
            rel.appendChild(tempo);

            var abertura = document.createElement('span');
            abertura.className = 'kb-card-abertura';
            abertura.textContent = '· aberta ' + os.data;
            rel.appendChild(abertura);

            card.appendChild(rel);
        }

        if (os.executor && os.executor !== '-') {
            var exec = document.createElement('div');
            exec.className = 'kb-card-executor';
            exec.textContent = os.executor;
            card.appendChild(exec);
        }

        return card;
    }

    function render() {
        var ordenacao = $('#kb-ordenar').val();
        var visiveis = 0;
        var contagem = {};

        $('.kb-cards').empty();
        STATUS_COLUNAS.forEach(function (st) { contagem[st] = 0; });

        STATUS_COLUNAS.forEach(function (st) {
            var itens = ordens.filter(function (os) { return os.status === st && combina(os); });

            itens.sort(function (a, b) {
                switch (ordenacao) {
                    case 'abertura':
                        // abertura mais recente primeiro
                        return (parseData(b.data) || 0) - (parseData(a.data) || 0);
                    case 'data':
                        // menor prazo primeiro (necessidade mais próxima/estourada no topo)
                        return (parseData(a.data_necessidade) || 0) - (parseData(b.data_necessidade) || 0) ||
                               (parseData(a.data) || 0) - (parseData(b.data) || 0);
                    case 'prioridade':
                        // crítica primeiro; empate desempata pela mais antiga
                        return pesoPrioridade(a) - pesoPrioridade(b) ||
                               (parseData(a.data) || 0) - (parseData(b.data) || 0);
                    default:
                        // 'tempo': mais tempo aberta primeiro
                        return (parseData(a.data) || 0) - (parseData(b.data) || 0);
                }
            });

            var wrap = $('[data-cards="' + st + '"]');
            itens.forEach(function (os) { wrap.append(montaCartao(os)); });

            contagem[st] = itens.length;
            visiveis += itens.length;
        });

        $.each(contagem, function (status, n) {
            $('[data-count="' + status + '"]').text(n);
        });

        $('#kb-vazio').toggleClass('kb-oculto', visiveis > 0);
    }

    // relógios correm por segundo
    setInterval(function () {
        $('.kb-card-relogio[data-aberto-em]').each(function () {
            var abertoEm = Number($(this).attr('data-aberto-em'));
            $(this).find('.kb-rel-tempo').text(fmtDecorrido(Date.now() - abertoEm));
        });
    }, 1000);

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
    $('#kb-ordenar').on('change', render);
    $('#kb-atualizar').on('click', carregar);

    // ---------- atualização automática ----------
    // A escolha fica no navegador de quem usa: o quadro costuma ficar aberto
    // em telão da manutenção, e reconfigurar a cada abertura seria trabalho à toa.
    var timerRefresh = null;

    function aplicarRefresh() {
        var seg = Number($('#kb-refresh').val()) || 0;

        if (timerRefresh) { clearInterval(timerRefresh); timerRefresh = null; }
        if (seg > 0) { timerRefresh = setInterval(carregar, seg * 1000); }

        try { localStorage.setItem('kb-refresh', String(seg)); } catch (e) { /* modo privado */ }
    }

    $('#kb-refresh').on('change', aplicarRefresh);

    try {
        var salvo = localStorage.getItem('kb-refresh');
        if (salvo && $('#kb-refresh option[value="' + salvo + '"]').length) { $('#kb-refresh').val(salvo); }
    } catch (e) { /* modo privado */ }

    aplicarRefresh();

    // clique no cartão abre o apontamento da O.S. (volta para o Kanban ao concluir)
    $root.on('click', '.kb-card', function () {
        window.location.href = cfg.urlApontamento +
            '?codigo_pcm_ordem_servico=' + encodeURIComponent($(this).data('codigo')) +
            '&codigo_unidade=' + encodeURIComponent($(this).data('unidade')) +
            '&model=OrdemServico&page=OrdemServicoKanban';
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
            if ($(this).attr('data-drop') === 'nao') { return; }
            e.preventDefault();
            e.originalEvent.dataTransfer.dropEffect = 'move';
            $(this).addClass('kb-drop-alvo');
        });

        $root.on('dragleave', '.kb-col', function () {
            $(this).removeClass('kb-drop-alvo');
        });

        $root.on('drop', '.kb-col', function (e) {
            if ($(this).attr('data-drop') === 'nao') { return; }
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
