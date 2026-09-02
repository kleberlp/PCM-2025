// ============================================================
//  ordemservico-kanban.js — quadro Kanban de Ordens de Serviço
//  (OrdemServico/OrdemServicoKanban)
//
//  Cartão (aprovado no mockup):
//    [origem] [prioridade] [colaboradores vinculados]
//    Nº O.S. (destaque)          relógio desde a abertura · aberta dd/MM/yyyy HH:mm
//    setor - local
//    descrição
//  (na O.S. concluída o relógio não corre — mostra só "aberta dd/MM/yyyy HH:mm")
//
//  O.S. vinculada não tem coluna própria: vira o chip roxo com o nome dos
//  colaboradores (o próprio nome já diz que é vinculada). O cartão cai em
//  Em Aberto ou Atrasadas conforme o prazo (data_necessidade) já venceu ou não.
//
//  Cartao: fundo vermelho quando atrasada (hoje > data_necessidade), azul
//  quando em andamento, verde quando concluida, pulso quando passa do tempo
//  maximo de atendimento. O relogio em si fica sempre cinza.
//
//  Clique: concluida (2) e em andamento (4) abrem OrdemServicoView; pendente,
//  atrasada e vinculada vao para ApontamentoOS.
//
//  Status reais da tb_pcm_ordem_servico: 1 Em Aberto, 2 Concluída,
//  3 Atrasada (derivado pela SP — coluna não aceita drop), 4 Em Andamento,
//  5 Vinculada (substitui o status de trabalho; no quadro entra em Em Aberto
//  ou Atrasadas, conforme o prazo).
//
//  Colunas do quadro: 1 Em Aberto, 3 Atrasadas, 4 Em Andamento, 2 Concluídas.
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
        urlView: $root.data('url-view'),
        urlCorPrioridade: $root.data('url-cor-prioridade'),
        empresa: $root.data('empresa'),
        usuario: $root.data('usuario'),
        unidade: $root.data('unidade'),
        dataInicio: $root.data('data-inicio'),
        dataTermino: $root.data('data-termino'),
        editar: String($root.data('editar')) === '1'
    };

    // Colunas do quadro, na ordem de exibição (chave = status de destino no drop)
    var COLUNAS = [1, 3, 4, 2];

    var ordens = [];   // última carga do servidor
    var busca = '';
    var LIMITE_PADRAO = 30;  // minutos — vale até o usuário informar o dele
    var limiteMin = LIMITE_PADRAO; // tempo máx. de atendimento (0 = sem limite)

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
        if (os.status === 2) { return 'kb-rel-neutro'; }
        var prazo = parseData(os.data_necessidade);
        if (!prazo) { return 'kb-rel-neutro'; }
        prazo.setHours(0, 0, 0, 0);
        var hoje = new Date();
        hoje.setHours(0, 0, 0, 0);
        if (hoje > prazo) { return 'kb-rel-vermelho'; }
        if (hoje.getTime() === prazo.getTime()) { return 'kb-rel-amarelo'; }
        return 'kb-rel-verde';
    }

    // Prazo de execução já venceu? (hoje > data_necessidade)
    function prazoVencido(os) {
        return classePrazo(os) === 'kb-rel-vermelho';
    }

    // Coluna do quadro em que a O.S. aparece. A SP já deriva a 3 (Atrasada) para
    // as pendentes vencidas, mas não para as vinculadas (5) — essas são
    // separadas aqui entre Em Aberto e Atrasadas conforme o prazo.
    function colunaDaOS(os) {
        switch (os.status) {
            case 1: return 1;   // Em Aberto
            case 3: return 3;   // Atrasada
            case 4: return 4;   // Em Andamento
            case 2: return 2;   // Concluída
            case 5: return prazoVencido(os) ? 3 : 1;   // Vinculada
            default: return 0;  // demais status não entram no quadro
        }
    }

    // Passou do tempo máximo de atendimento? (só para O.S. ainda não concluída)
    function estourouLimite(abertoEm, status) {
        if (!limiteMin || !abertoEm || status === 2) { return false; }
        return (Date.now() - abertoEm) > (limiteMin * 60000);
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

    // ---------- cor da prioridade ----------
    //
    // A cor vem do cadastro (tb_cad_prioridade.cor). Sem cor cadastrada — ou com o
    // script 2026-08-28 ainda nao rodado — cai na paleta padrao abaixo, para a tela
    // nunca ficar sem cor nenhuma.
    var coresPrioridade = {};   // codigo -> "#rrggbb"

    // So cor literal segura entra no style: o valor vem do banco.
    function corValida(cor) {
        return /^#[0-9a-fA-F]{3,8}$/.test(String(cor || '').trim()) ? String(cor).trim() : '';
    }

    // Texto escuro sobre fundo claro, branco sobre escuro (luminancia aproximada).
    function tintaSobre(cor) {
        var hex = cor.replace('#', '');
        if (hex.length === 3) { hex = hex[0] + hex[0] + hex[1] + hex[1] + hex[2] + hex[2]; }
        if (hex.length < 6) { return '#fff'; }
        var r = parseInt(hex.substr(0, 2), 16),
            g = parseInt(hex.substr(2, 2), 16),
            b = parseInt(hex.substr(4, 2), 16);
        return (0.299 * r + 0.587 * g + 0.114 * b) > 165 ? '#3b3200' : '#fff';
    }

    // Paleta padrao: 0 critico vermelho · 1 alta laranja · 2 media mostarda
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
        card.className = 'kb-card'
            + (colunaDaOS(os) === 1 ? ' kb-aberta' : '')
            + (os.status === 2 ? ' kb-concluida' : '')
            + (os.status === 4 ? ' kb-andamento' : '')
            + (os.status !== 4 && prazoCls === 'kb-rel-vermelho' ? ' kb-atrasada' : '')
            + (estourouLimite(abertoEm ? abertoEm.getTime() : 0, os.status) ? ' kb-estourada' : '');
        card.setAttribute('data-codigo', os.codigo);
        card.setAttribute('data-unidade', os.codigo_unidade);
        card.setAttribute('data-status', os.status);
        // Atrasada (3) não recebe drop nem sai arrastada — vale também para a
        // vinculada (5) que caiu na coluna Atrasadas por prazo vencido.
        if (cfg.editar && colunaDaOS(os) !== 3) { card.setAttribute('draggable', 'true'); }

        // linha 1: origem + prioridade + colaboradores vinculados
        var chips = document.createElement('div');
        chips.className = 'kb-card-chips';
        if (os.origem) { chips.appendChild(chip(os.origem)); }
        if (os.prioridade) {
            var cor = corValida(coresPrioridade[os.codigo_prioridade]);
            var chipPri = chip(os.prioridade, cor ? '' : classePrioridade(os));
            if (cor) {
                // cor do cadastro: CSSOM, nao atributo style inline (CSP)
                chipPri.style.background = cor;
                chipPri.style.borderColor = cor;
                chipPri.style.color = tintaSobre(cor);
            }
            chips.appendChild(chipPri);
        }
        // O.S. vinculada: o nome dos colaboradores é o próprio "badge" de vinculada.
        if (os.executor && os.executor !== '-') {
            chips.appendChild(chip(os.executor, 'kb-chip-vinculada'));
        }
        card.appendChild(chips);

        // linha 2: nº da O.S. em destaque (esq.) + relógio desde a abertura (dir.)
        var titulo = document.createElement('div');
        titulo.className = 'kb-card-titulo';

        if (os.numero_documento) {
            titulo.appendChild(linhaIcone('kb-card-local', 'fa-tag', os.numero_documento));
        }

        if (abertoEm) {
            // Sempre cinza: quem sinaliza atraso agora e o fundo do cartao
            // (vermelho) e o pulso do tempo maximo — colorir o relogio tambem
            // so competiria com eles.
            var rel = document.createElement('div');
            rel.className = 'kb-card-relogio kb-rel-neutro';

            var icone = document.createElement('i');
            icone.className = 'fa fa-clock-o';
            rel.appendChild(icone);

            // O.S. concluida nao tem "tempo decorrido" correndo — so a data de
            // abertura. Sem data-aberto-em o cronometro por segundo a ignora.
            if (os.status !== 2) {
                rel.setAttribute('data-aberto-em', abertoEm.getTime());

                var tempo = document.createElement('span');
                tempo.className = 'kb-rel-tempo';
                tempo.textContent = fmtDecorrido(Date.now() - abertoEm.getTime());
                rel.appendChild(tempo);
            }

            var abertura = document.createElement('span');
            abertura.className = 'kb-card-abertura';
            abertura.textContent = (os.status === 2 ? 'aberta ' : '· aberta ') + os.data;
            rel.appendChild(abertura);

            titulo.appendChild(rel);
        }

        if (titulo.childNodes.length) { card.appendChild(titulo); }

        // linha 3: setor - local
        var setorLocal = [os.setor, os.local].filter(function (v) { return v; }).join(' - ');
        if (setorLocal) { card.appendChild(linhaIcone('kb-card-numero', 'fa-map-marker', setorLocal)); }

        // linha 4: descrição
        if (os.descricao) {
            var desc = document.createElement('div');
            desc.className = 'kb-card-descricao';
            desc.textContent = os.descricao;
            card.appendChild(desc);
        }

        return card;
    }

    function render() {
        var ordenacao = $('#kb-ordenar').val();
        var visiveis = 0;
        var contagem = {};

        $('.kb-cards').empty();
        COLUNAS.forEach(function (st) { contagem[st] = 0; });

        COLUNAS.forEach(function (st) {
            var itens = ordens.filter(function (os) { return colunaDaOS(os) === st && combina(os); });

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

    // relógios correm por segundo — e o pulso entra sozinho quando o tempo
    // estoura, sem depender de recarregar a página
    setInterval(function () {
        $('.kb-card-relogio[data-aberto-em]').each(function () {
            var abertoEm = Number($(this).attr('data-aberto-em'));
            $(this).find('.kb-rel-tempo').text(fmtDecorrido(Date.now() - abertoEm));

            var $card = $(this).closest('.kb-card');
            var status = Number($card.attr('data-status'));
            $card.toggleClass('kb-estourada', estourouLimite(abertoEm, status));
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

    // ---------- tempo máximo de atendimento ----------
    // Começa em 30 min; assim que o usuário troca, o valor dele passa a ser o
    // fixo do quadro — fica no navegador, como o refresh, por ser ajuste de
    // quem opera a tela. Campo vazio = sem limite (ninguém pulsa).
    function aplicarLimite() {
        var informado = $('#kb-limite').val();

        limiteMin = (informado === '' || informado === null)
            ? 0
            : Math.max(0, Number(informado) || 0);

        try { localStorage.setItem('kb-limite', String(limiteMin)); } catch (e) { /* modo privado */ }
        render();
    }

    $('#kb-limite').on('change input', aplicarLimite);

    try {
        var limiteSalvo = localStorage.getItem('kb-limite');
        // já escolhido antes vence o padrão — inclusive o zero (sem limite)
        limiteMin = (limiteSalvo === null) ? LIMITE_PADRAO : Math.max(0, Number(limiteSalvo) || 0);
    } catch (e) {
        limiteMin = LIMITE_PADRAO;
    }

    $('#kb-limite').val(limiteMin > 0 ? limiteMin : '');

    try {
        var salvo = localStorage.getItem('kb-refresh');
        if (salvo && $('#kb-refresh option[value="' + salvo + '"]').length) { $('#kb-refresh').val(salvo); }
    } catch (e) { /* modo privado */ }

    aplicarRefresh();

    // Clique no cartão: concluída (2) e em andamento (4) abrem a visualização;
    // pendente (1), atrasada (3) e vinculada (5) vão para o apontamento e
    // voltam para o Kanban.
    $root.on('click', '.kb-card', function () {
        var codigo = encodeURIComponent($(this).data('codigo'));
        var unidade = encodeURIComponent($(this).data('unidade'));
        var st = String($(this).attr('data-status'));

        if (st === '2' || st === '4') {
            window.location.href = cfg.urlView + '?codigo=' + codigo + '&unidade=' + unidade;
            return;
        }

        window.location.href = cfg.urlApontamento +
            '?codigo_pcm_ordem_servico=' + codigo +
            '&codigo_unidade=' + unidade +
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
            if (!os || colunaDaOS(os) === novoStatus) { return; }

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

    // primeira carga: o mapa de cores vem antes, para o primeiro desenho ja sair pintado
    if (cfg.urlCorPrioridade) {
        $.getJSON(cfg.urlCorPrioridade)
            .done(function (lista) {
                (lista || []).forEach(function (p) { coresPrioridade[p.codigo] = p.cor; });
            })
            .always(carregar);
    } else {
        carregar();
    }

})(jQuery);
