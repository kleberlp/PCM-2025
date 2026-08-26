/* manual-panel.js
   Manual integrado: o "?" do cabeçalho abre o manual da tela atual.

   O conteúdo só é buscado quando o painel abre — carregar manual em toda tela
   seria peso à toa, e a maioria das visitas não abre a ajuda. Buscado uma vez,
   fica em memória: reabrir na mesma tela não volta ao servidor. */
(function () {
    'use strict';

    var $btn = jQuery('#btnManual');
    if (!$btn.length) return;

    var cfg = {
        url:        $btn.attr('data-url'),
        urlEdit:    $btn.attr('data-url-edit'),
        urlNew:     $btn.attr('data-url-new'),
        controller: $btn.attr('data-controller'),
        action:     $btn.attr('data-action'),
        msgEmpty:   $btn.attr('data-msg-empty'),
        msgNoMatch: $btn.attr('data-msg-nomatch'),
        msgProcess: $btn.attr('data-msg-process'),
        msgEdit:    $btn.attr('data-msg-edit'),
        msgCreate:  $btn.attr('data-msg-create')
    };

    var manual = null;   // o que veio do servidor, guardado entre aberturas
    var canEdit = false;
    var buscou = false;

    function escapar(txt) {
        return jQuery('<div>').text(txt == null ? '' : txt).html();
    }

    /* O conteúdo é escrito por gente, não por programa: aceita quebra de linha e
       uma marcação mínima (**negrito** e - lista), e nada de HTML solto — o
       texto vem do banco e não pode virar script na tela de quem lê. */
    function formatar(txt) {
        var seguro = escapar(txt).replace(/\r/g, '');

        // Lista: linhas seguidas começando por "- ".
        seguro = seguro.replace(/(^|\n)((?:- [^\n]*(?:\n|$))+)/g, function (todo, antes, bloco) {
            var itens = bloco.replace(/\n+$/, '').split('\n').map(function (l) {
                return '<li>' + l.replace(/^- /, '') + '</li>';
            }).join('');
            return antes + '<ul>' + itens + '</ul>';
        });

        return seguro
            .replace(/\*\*([^*]+)\*\*/g, '<b>$1</b>')
            .replace(/\n{2,}/g, '</p><p>')
            .replace(/\n/g, '<br>')
            .replace(/^/, '<p>')
            .replace(/$/, '</p>')
            .replace(/<p>(<ul>)/g, '$1')
            .replace(/(<\/ul>)<\/p>/g, '$1');
    }

    /* ── vídeo ──
       YouTube e Vimeo viram player incorporado; qualquer outra URL vira link.
       O player só carrega quando a seção abre (data-src): abrir o painel não
       pode baixar meia dúzia de iframes de vídeo de uma vez. */
    function videoEmbed(url) {
        var m = url.match(/(?:youtube\.com\/(?:watch\?[^#]*v=|shorts\/|embed\/|live\/)|youtu\.be\/)([A-Za-z0-9_-]{5,20})/);
        if (m) return 'https://www.youtube.com/embed/' + m[1];
        m = url.match(/vimeo\.com\/(?:video\/)?(\d+)/);
        if (m) return 'https://player.vimeo.com/video/' + m[1];
        return null;
    }

    function montarVideo(url, aberta) {
        if (!url || !/^https?:\/\//i.test(url)) return '';

        var embed = videoEmbed(url);
        if (!embed) {
            return '<a class="manual-video-link" href="' + escapar(url) + '" target="_blank" rel="noopener noreferrer">' +
                   '<i class="fa fa-play-circle"></i> ' + escapar(url) + '</a>';
        }

        return '<div class="manual-video">' +
               '<iframe ' + (aberta ? 'src' : 'data-src') + '="' + escapar(embed) + '" ' +
               'allow="accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture" ' +
               'allowfullscreen loading="lazy"></iframe></div>';
    }

    function montarSecoes(itens) {

        if (!itens || !itens.length) {
            return '<div class="manual-vazio">' + escapar(cfg.msgEmpty) + '</div>';
        }

        var html = '';

        itens.forEach(function (s, i) {
            // A primeira seção abre; as demais ficam recolhidas, para o painel
            // caber na tela e o índice ser lido de uma vez.
            var aberta = i === 0;

            var nota = '';
            if (s.nota) {
                nota = '<div class="manual-nota ' + (s.tipo_nota === 'A' ? 'aviso' : 'dica') + '">' +
                       formatar(s.nota) + '</div>';
            }

            var imagem = s.imagem
                ? '<img class="manual-img" src="' + escapar(s.imagem) + '" alt="' + escapar(s.titulo) + '">'
                : '';

            html +=
                '<div class="manual-sec" data-idx="' + i + '">' +
                    '<div class="manual-sec-h' + (aberta ? '' : ' fechada') + '">' +
                        '<span><span class="n">' + (i + 1) + '</span>' + escapar(s.titulo) + '</span>' +
                        '<i class="fa fa-chevron-' + (aberta ? 'down' : 'right') + '"></i>' +
                    '</div>' +
                    '<div class="manual-sec-b' + (aberta ? '' : ' js-hidden') + '">' +
                        formatar(s.conteudo) + nota + imagem + montarVideo(s.video, aberta) +
                    '</div>' +
                '</div>';
        });

        return html;
    }

    function render() {

        jQuery('#manualTitulo').text((manual && manual.titulo) || $btn.attr('title'));
        jQuery('#manualSubtitulo').text((manual && manual.subtitulo) || '');
        jQuery('#manualBusca').val('');
        jQuery('#manualCorpo').html(montarSecoes(manual && manual.itens));

        var $proc = jQuery('#manualProcesso').empty();
        if (manual && manual.processo_codigo > 0) {
            $proc.html(escapar(cfg.msgProcess) + ' ' +
                '<a href="#" class="manual-processo" data-id="' + manual.processo_codigo + '">' +
                escapar(manual.processo_titulo) + '</a>');
        }

        // Quem pode manter o manual edita o existente ou cria o da tela sem manual.
        var $editar = jQuery('#manualEditar').toggleClass('js-hidden', !canEdit);
        if (canEdit) {
            if (manual && manual.codigo > 0) {
                $editar.html('<i class="fa fa-pencil"></i> ' + escapar(cfg.msgEdit))
                       .attr('href', cfg.urlEdit + '?codigo=' + manual.codigo);
            } else {
                $editar.html('<i class="fa fa-plus"></i> ' + escapar(cfg.msgCreate))
                       .attr('href', cfg.urlNew + '?tela_controller=' + encodeURIComponent(cfg.controller || '') +
                                                '&tela_action=' + encodeURIComponent(cfg.action || ''));
            }
        }
    }

    function buscar(pronto) {
        jQuery('#manualCorpo').html('<div class="manual-vazio"><i class="fa fa-spinner fa-spin"></i></div>');

        jQuery.ajax({
            url: cfg.url, type: 'GET',
            data: { screenController: cfg.controller, screenAction: cfg.action },
            success: function (r) {
                buscou = true;
                manual = (r && r.found && r.manual) || null;
                canEdit = !!(r && r.canEdit);
                pronto();
            },
            error: function () {
                jQuery('#manualCorpo').html('<div class="manual-vazio">' + escapar(cfg.msgEmpty) + '</div>');
            }
        });
    }

    /* ── abrir / fechar ── */

    function abrir() {
        jQuery('#manualPainel').addClass('aberto');
        jQuery('#manualBackdrop').addClass('aberto');
    }

    function fechar() {
        jQuery('#manualPainel').removeClass('aberto');
        jQuery('#manualBackdrop').removeClass('aberto');
    }

    $btn.on('click', function (e) {
        e.preventDefault();
        abrir();
        if (buscou) { render(); return; }
        buscar(render);
    });

    jQuery('#manualFechar, #manualBackdrop').on('click', fechar);

    jQuery(document).on('keydown', function (e) {
        if (e.key === 'Escape') fechar();
    });

    /* ── seções ── */
    jQuery(document).on('click', '#manualCorpo .manual-sec-h', function () {
        var $h = jQuery(this).toggleClass('fechada');
        var $b = $h.next('.manual-sec-b').toggleClass('js-hidden', $h.hasClass('fechada'));
        $h.find('i').toggleClass('fa-chevron-down', !$h.hasClass('fechada'))
                    .toggleClass('fa-chevron-right', $h.hasClass('fechada'));

        // O player de vídeo só carrega na primeira vez em que a seção abre.
        if (!$h.hasClass('fechada')) {
            $b.find('iframe[data-src]').each(function () {
                jQuery(this).attr('src', jQuery(this).attr('data-src')).removeAttr('data-src');
            });
        }
    });

    /* ── busca dentro do manual ──
       Filtra as seções pelo título e pelo texto, e abre o que sobrou: procurar
       e ainda ter que clicar em cada seção para ver se está lá não ajuda. */
    jQuery('#manualBusca').on('input', function () {

        var termo = (jQuery(this).val() || '').trim().toLowerCase();
        var achou = 0;

        jQuery('#manualCorpo .manual-sec').each(function () {
            var $s = jQuery(this);
            var texto = $s.text().toLowerCase();
            var bate = termo === '' || texto.indexOf(termo) >= 0;

            $s.toggleClass('js-hidden', !bate);
            if (bate) achou++;

            if (termo !== '') {
                $s.find('.manual-sec-h').removeClass('fechada')
                  .find('i').addClass('fa-chevron-down').removeClass('fa-chevron-right');
                $s.find('.manual-sec-b').removeClass('js-hidden');
            }
        });

        jQuery('#manualCorpo .manual-semnada').remove();
        if (achou === 0) {
            jQuery('#manualCorpo').append('<div class="manual-vazio manual-semnada">' +
                escapar(cfg.msgNoMatch) + '</div>');
        }
    });

    /* ── manual do processo (link "ver também") ── */
    jQuery(document).on('click', '.manual-processo', function (e) {
        e.preventDefault();
        // Troca o conteúdo do painel sem sair da tela: o processo é leitura de
        // apoio, não outra página.
        jQuery.ajax({
            url: cfg.url, type: 'GET',
            data: { screenController: cfg.controller, screenAction: cfg.action, codigo: jQuery(this).data('id') },
            success: function (r) {
                if (r && r.found && r.manual) { manual = r.manual; render(); }
            }
        });
    });

}());
