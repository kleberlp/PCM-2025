/* manual-guia.js — Guia do PCM (menu próprio, no lugar da plataforma externa).

   Navegação por trilha à esquerda, artigo à direita. A lista de trilhas e
   artigos vem enxuta (GuiaLista); o conteúdo de cada artigo é buscado sob
   demanda pelo mesmo ManualTela que o painel "?" usa. O texto é formatado
   pelo PcmManualPreview.formatar (exposto por manual-panel.js), então tabela,
   lista e código saem idênticos ao painel — uma implementação só. */
jQuery(function () {

    'use strict';

    var $corpo = jQuery('.guia-corpo');
    if (!$corpo.length) { return; }

    var urlLista = $corpo.attr('data-url-lista');
    var urlConteudo = $corpo.attr('data-url-conteudo');

    var itens = [];          // tudo que veio do servidor
    var cache = {};          // conteúdo já buscado, por código
    var atual = 0;

    function escapar(txt) {
        return jQuery('<div>').text(txt == null ? '' : txt).html();
    }

    function formatar(txt) {
        return (window.PcmManualPreview && window.PcmManualPreview.formatar)
            ? window.PcmManualPreview.formatar(txt)
            : escapar(txt).replace(/\n/g, '<br>');
    }

    /* ── vídeo (mesma proteção do painel: sem link exposto, tampa na faixa de
          cima que leva para fora) ── */
    function ehArquivoVideo(url) {
        return /\.(mp4|webm|ogv|ogg|m4v|mov)(\?|#|$)/i.test(url);
    }

    function videoEmbed(url) {
        var m = url.match(/(?:youtube\.com\/(?:watch\?[^#]*v=|shorts\/|embed\/|live\/)|youtu\.be\/)([A-Za-z0-9_-]{5,20})/);
        if (m) { return 'https://www.youtube.com/embed/' + m[1] + '?rel=0'; }
        m = url.match(/vimeo\.com\/(?:video\/)?(\d+)/);
        if (m) { return 'https://player.vimeo.com/video/' + m[1]; }
        m = url.match(/drive\.google\.com\/file\/d\/([A-Za-z0-9_-]+)/);
        if (m) { return 'https://drive.google.com/file/d/' + m[1] + '/preview'; }
        m = url.match(/drive\.google\.com\/(?:open|uc)\?[^#]*id=([A-Za-z0-9_-]+)/);
        if (m) { return 'https://drive.google.com/file/d/' + m[1] + '/preview'; }
        return null;
    }

    function montarVideo(url) {
        if (!url || !/^https?:\/\//i.test(url)) { return ''; }

        var player;
        if (ehArquivoVideo(url)) {
            player = '<video controls controlsList="nodownload noremoteplayback" ' +
                     'disablepictureinpicture preload="none" src="' + escapar(url) + '"></video>';
        } else {
            var embed = videoEmbed(url);
            if (!embed) {
                return '<a class="manual-video-link" href="' + escapar(url) + '" target="_blank" rel="noopener noreferrer">' +
                       '<i class="fa fa-play-circle"></i> Assistir vídeo</a>';
            }
            player = '<iframe src="' + escapar(embed) + '" ' +
                     'allow="accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture" ' +
                     'allowfullscreen loading="lazy"></iframe><div class="manual-video-tampa"></div>';
        }

        return '<div class="manual-video-box"><div class="manual-video">' + player + '</div>' +
               '<div class="manual-video-acoes"><button type="button" class="manual-video-cheia">' +
               '<i class="fa fa-expand"></i> Tela cheia</button></div></div>';
    }

    /* ── índice (trilhas com artigos embaixo) ── */
    function montarIndice() {

        var trilhas = itens.filter(function (i) { return i.tipo === 'P'; });
        var artigos = itens.filter(function (i) { return i.tipo === 'S'; });

        var html = '';

        function linkItem(it) {
            return '<a href="#" class="guia-item" data-codigo="' + it.codigo + '">' +
                   escapar(it.titulo) +
                   (it.secoes > 0 ? '<span class="guia-secoes">' + it.secoes + '</span>' : '') +
                   '</a>';
        }

        trilhas.forEach(function (t) {
            var filhos = artigos.filter(function (a) { return a.processo_codigo === t.codigo; });
            html += '<div class="guia-grupo">' +
                        '<a href="#" class="guia-trilha" data-codigo="' + t.codigo + '">' + escapar(t.titulo) + '</a>' +
                        '<div class="guia-filhos">' + filhos.map(linkItem).join('') + '</div>' +
                    '</div>';
        });

        // Artigos sem trilha (ou cuja trilha não veio): não podem sumir.
        var codigosTrilha = trilhas.map(function (t) { return t.codigo; });
        var soltos = artigos.filter(function (a) { return codigosTrilha.indexOf(a.processo_codigo) < 0; });
        if (soltos.length) {
            html += '<div class="guia-grupo">' +
                        '<div class="guia-trilha guia-trilha-fixa">Outros</div>' +
                        '<div class="guia-filhos">' + soltos.map(linkItem).join('') + '</div>' +
                    '</div>';
        }

        if (!html) { html = '<div class="guia-vazio">Nenhum manual cadastrado ainda.</div>'; }

        jQuery('#guiaIndice').html(html);
    }

    /* ── artigo ── */
    function montarArtigo(m) {

        var html = '<h2 class="guia-titulo">' + escapar(m.titulo) + '</h2>';
        if (m.subtitulo) { html += '<p class="guia-subtitulo">' + escapar(m.subtitulo) + '</p>'; }

        (m.itens || []).forEach(function (s) {
            html += '<section class="guia-secao">';
            if (s.titulo) { html += '<h3>' + escapar(s.titulo) + '</h3>'; }
            html += formatar(s.conteudo);

            if (s.nota) {
                html += '<div class="manual-nota ' + (s.tipo_nota === 'A' ? 'aviso' : 'dica') + '">' +
                        formatar(s.nota) + '</div>';
            }
            if (s.imagem) {
                html += '<img class="manual-img" src="' + escapar(s.imagem) + '" alt="' + escapar(s.titulo) + '">';
            }
            html += montarVideo(s.video);
            html += '</section>';
        });

        if (m.processo_codigo > 0) {
            html += '<div class="guia-vertambem">' +
                    '<a href="#" class="guia-item" data-codigo="' + m.processo_codigo + '">' +
                    '<i class="fa fa-sitemap mr-5"></i>Ver a trilha completa: ' + escapar(m.processo_titulo || '') + '</a></div>';
        }

        jQuery('#guiaArtigo').html(html).scrollTop(0);
        jQuery('#guiaArtigo')[0].scrollTop = 0;
    }

    function abrir(codigo) {
        atual = codigo;
        jQuery('.guia-item, .guia-trilha').removeClass('ativo');
        jQuery('.guia-item[data-codigo="' + codigo + '"], .guia-trilha[data-codigo="' + codigo + '"]').first().addClass('ativo');

        if (cache[codigo]) { montarArtigo(cache[codigo]); return; }

        jQuery('#guiaArtigo').html('<div class="guia-vazio"><i class="fa fa-spinner fa-spin"></i></div>');
        jQuery.getJSON(urlConteudo, { codigo: codigo }).done(function (r) {
            if (r && r.found && r.manual) {
                cache[codigo] = r.manual;
                if (atual === codigo) { montarArtigo(r.manual); }
            } else {
                jQuery('#guiaArtigo').html('<div class="guia-vazio">Não foi possível carregar este manual.</div>');
            }
        }).fail(function () {
            jQuery('#guiaArtigo').html('<div class="guia-vazio">Não foi possível carregar este manual.</div>');
        });
    }

    $corpo.on('click', '.guia-item, .guia-trilha[data-codigo]', function (e) {
        e.preventDefault();
        abrir(parseInt(jQuery(this).attr('data-codigo'), 10));
    });

    /* ── tela cheia do vídeo ── */
    $corpo.on('click', '.manual-video-cheia', function () {
        var alvo = jQuery(this).closest('.manual-video-box').find('.manual-video')[0];
        if (!alvo) { return; }
        if (alvo.requestFullscreen) { alvo.requestFullscreen(); }
        else if (alvo.webkitRequestFullscreen) { alvo.webkitRequestFullscreen(); }
    });
    $corpo.on('contextmenu', '.manual-video', function (e) { e.preventDefault(); });

    /* ── busca ── */
    jQuery('#guiaBusca').on('input', function () {
        var termo = (jQuery(this).val() || '').trim().toLowerCase();

        jQuery('#guiaIndice .guia-item').each(function () {
            var bate = termo === '' || jQuery(this).text().toLowerCase().indexOf(termo) >= 0;
            jQuery(this).toggleClass('js-hidden', !bate);
        });

        // Trilha some quando nenhum filho seu casa a busca.
        jQuery('#guiaIndice .guia-grupo').each(function () {
            var visiveis = jQuery(this).find('.guia-item:not(.js-hidden)').length;
            jQuery(this).toggleClass('js-hidden', termo !== '' && visiveis === 0);
        });
    });

    /* ── carga ── */
    jQuery.getJSON(urlLista).done(function (r) {
        itens = (r && r.itens) || [];
        montarIndice();

        // Abre o primeiro artigo (ou a primeira trilha) para a tela não nascer vazia.
        var primeiro = itens.filter(function (i) { return i.tipo === 'S'; })[0] || itens[0];
        if (primeiro) { abrir(primeiro.codigo); }
        else { jQuery('#guiaArtigo').html('<div class="guia-vazio">Nenhum manual cadastrado ainda.</div>'); }
    }).fail(function () {
        jQuery('#guiaArtigo').html('<div class="guia-vazio">Não foi possível carregar o guia.</div>');
    });

});
