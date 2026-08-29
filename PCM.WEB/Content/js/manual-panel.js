/* manual-panel.js
   Manual integrado: o "?" do cabeçalho abre o manual da tela atual.

   O manual é buscado uma vez na carga da página: é o que diz se o "?" aparece
   (tela sem manual não tem por que mostrar o botão) e já deixa o conteúdo
   pronto para o clique. Fica em memória: reabrir não volta ao servidor. */
(function () {
    'use strict';

    function escapar(txt) {
        return jQuery('<div>').text(txt == null ? '' : txt).html();
    }

    /* ── Markdown ──
       O conteúdo é escrito por gente, em Markdown, e chega do banco: tudo é escapado
       ANTES de virar marcação, então nenhum texto do manual pode virar script na tela
       de quem lê. O subconjunto é o que um manual usa de verdade: títulos, negrito,
       itálico, código, listas, tabelas, citação, imagem e link. */

    // Só http(s) e caminho do próprio site. Fecha javascript:, data: e afins, que num
    // href transformariam um link do manual em execução de código.
    function urlSegura(u) {
        u = (u || '').trim();
        return (/^https?:\/\//i.test(u) || /^[/.#]/.test(u)) ? u : '';
    }

    function emLinha(t) {
        return t
            // Imagem antes do link: a sintaxe do link é a mesma sem o "!".
            .replace(/!\[([^\]]*)\]\(([^)\s]+)[^)]*\)/g, function (todo, alt, url) {
                var src = urlSegura(url);
                return src ? '<img class="manual-img" src="' + src + '" alt="' + alt + '">' : alt;
            })
            .replace(/\[([^\]]+)\]\(([^)\s]+)[^)]*\)/g, function (todo, texto, url) {
                var href = urlSegura(url);
                return href ? '<a href="' + href + '" target="_blank" rel="noopener noreferrer">' + texto + '</a>' : texto;
            })
            .replace(/`([^`]+)`/g, '<code>$1</code>')
            .replace(/\*\*([^*]+)\*\*/g, '<b>$1</b>')
            .replace(/(^|[\s(])\*([^*\n]+)\*/g, '$1<i>$2</i>')
            .replace(/(^|[\s(])_([^_\n]+)_/g, '$1<i>$2</i>');
    }

    function celulas(linha) {
        return linha.replace(/^\s*\|/, '').replace(/\|\s*$/, '').split('|').map(function (c) {
            return c.trim();
        });
    }

    function ehSeparadorTabela(linha) {
        return /^\s*\|?[\s:-]*-[\s|:-]*$/.test(linha) && linha.indexOf('-') >= 0 && linha.indexOf('|') >= 0;
    }

    function tabelaHtml(cab, corpo) {
        return '<div class="manual-tabela"><table><thead><tr>' +
               cab.map(function (c) { return '<th>' + emLinha(c) + '</th>'; }).join('') +
               '</tr></thead><tbody>' +
               corpo.map(function (l) {
                   return '<tr>' + l.map(function (c) { return '<td>' + emLinha(c) + '</td>'; }).join('') + '</tr>';
               }).join('') +
               '</tbody></table></div>';
    }

    /* Tabela espremida numa linha só — conteúdo colado sem as quebras de linha:
       "| A | B | | :--- | :--- | | a1 | b1 | | a2 | b2 |". O separador embutido
       diz quantas colunas a tabela tem; o que vem antes é o cabeçalho e as
       células seguintes são fatiadas em linhas dessa largura. */
    function tabelaEmLinhaUnica(linha) {

        if (!/^\s*\|/.test(linha)) return null;

        var mSep = /\|\s*:?-{2,}:?\s*(?:\|\s*:?-{2,}:?\s*)*\|/.exec(linha);
        if (!mSep || mSep.index <= 0) return null;

        var cab = celulas(linha.slice(0, mSep.index));
        var n = celulas(mSep[0]).length;
        if (!cab.length || !n) return null;

        var corpo = [];
        var atual = [];
        linha.slice(mSep.index + mSep[0].length).split('|').forEach(function (tok) {
            var t = tok.trim();
            // o "| |" entre uma linha e outra vira token vazio na borda: pula
            if (t === '' && atual.length === 0) return;
            atual.push(t);
            if (atual.length === n) { corpo.push(atual); atual = []; }
        });
        if (atual.length && atual.join('') !== '') corpo.push(atual);

        return tabelaHtml(cab, corpo);
    }

    // O texto chega escapado: o ">" do Markdown já é "&gt;" quando o bloco é lido.
    var CITACAO = /^\s*&gt;\s?/;

    function formatar(txt) {

        var linhas = escapar(txt == null ? '' : txt).replace(/\r/g, '').split('\n');
        var html = [];
        var i = 0;

        function paragrafo(bloco) {
            if (bloco.length) html.push('<p>' + emLinha(bloco.join('<br>')) + '</p>');
        }

        while (i < linhas.length) {

            var linha = linhas[i];

            // Bloco de código: sai literal, sem passar pela marcação de linha.
            if (/^\s*```/.test(linha)) {
                var codigo = [];
                i++;
                while (i < linhas.length && !/^\s*```/.test(linhas[i])) { codigo.push(linhas[i]); i++; }
                i++;
                html.push('<pre><code>' + codigo.join('\n') + '</code></pre>');
                continue;
            }

            // Tabela colada numa linha só (sem quebras de linha).
            var comprimida = tabelaEmLinhaUnica(linha);
            if (comprimida) { html.push(comprimida); i++; continue; }

            // Tabela: linha de células seguida da linha de traços — ou, sem os
            // traços, duas linhas seguidas no formato |...|: quem escreveu assim
            // queria uma tabela, e pipes crus no painel não ajudam ninguém.
            var proxima = i + 1 < linhas.length ? linhas[i + 1] : '';
            var comSeparador = linha.indexOf('|') >= 0 && ehSeparadorTabela(proxima);
            var semSeparador = !comSeparador &&
                               /^\s*\|.*\|\s*$/.test(linha) &&
                               /^\s*\|.*\|\s*$/.test(proxima) && !ehSeparadorTabela(proxima);
            if (comSeparador || semSeparador) {
                var cab = celulas(linha);
                i += comSeparador ? 2 : 1;
                var corpo = [];
                while (i < linhas.length && linhas[i].indexOf('|') >= 0 && linhas[i].trim() !== '') {
                    corpo.push(celulas(linhas[i])); i++;
                }
                html.push(tabelaHtml(cab, corpo));
                continue;
            }

            // Título. O manual já tem o título da seção no cabeçalho dela, então aqui
            // tudo desce um nível: o "##" do texto vira subtítulo, e não outro topo.
            var titulo = /^(#{1,6})\s+(.*)$/.exec(linha);
            if (titulo) {
                var nivel = Math.min(titulo[1].length + 3, 6);
                html.push('<h' + nivel + '>' + emLinha(titulo[2]) + '</h' + nivel + '>');
                i++;
                continue;
            }

            if (/^\s*([-*_])\s*\1\s*\1[\s-*_]*$/.test(linha)) { html.push('<hr>'); i++; continue; }

            // Citação. O texto já veio escapado, então o ">" do Markdown chega aqui
            // como "&gt;" — procurar pelo sinal cru nunca acharia citação nenhuma.
            if (CITACAO.test(linha)) {
                var cita = [];
                while (i < linhas.length && CITACAO.test(linhas[i])) {
                    cita.push(linhas[i].replace(CITACAO, '')); i++;
                }
                html.push('<blockquote>' + emLinha(cita.join('<br>')) + '</blockquote>');
                continue;
            }

            // Lista, com ou sem número.
            var marcador = /^\s*([-*+]|\d+[.)])\s+/;
            if (marcador.test(linha)) {
                var ordenada = /^\s*\d/.test(linha);
                var itens = [];
                while (i < linhas.length && marcador.test(linhas[i])) {
                    itens.push('<li>' + emLinha(linhas[i].replace(marcador, '')) + '</li>');
                    i++;
                }
                html.push((ordenada ? '<ol>' : '<ul>') + itens.join('') + (ordenada ? '</ol>' : '</ul>'));
                continue;
            }

            // Parágrafo: até a linha em branco.
            var bloco = [];
            while (i < linhas.length && linhas[i].trim() !== '' &&
                   !marcador.test(linhas[i]) && !/^\s*#/.test(linhas[i]) &&
                   !CITACAO.test(linhas[i]) &&
                   !/^\s*```/.test(linhas[i])) {
                bloco.push(linhas[i]); i++;
            }
            if (bloco.length) { paragrafo(bloco); continue; }

            i++;
        }

        return html.join('');
    }

    // O cadastro do manual (HelpInsert/HelpEdit) pré-visualiza a seção com ESTE
    // formatador: o autor vê a tabela, o código e a lista exatamente como o
    // painel vai mostrar, e não uma segunda implementação que divergiria dele.
    window.PcmManualPreview = { formatar: formatar };

    // Daqui para baixo é o painel em si: sem o "?" no cabeçalho (tela de login,
    // layout sem header), só o formatador acima fica disponível.
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

    /* ── vídeo ──
       YouTube, Vimeo e Google Drive viram player incorporado; arquivo de vídeo solto
       (.mp4 e afins) toca no player do próprio navegador; o resto vira link.
       O player só carrega quando a seção abre (data-src): abrir o painel não
       pode baixar meia dúzia de iframes de vídeo de uma vez. */
    function videoEmbed(url) {

        var m = url.match(/(?:youtube\.com\/(?:watch\?[^#]*v=|shorts\/|embed\/|live\/)|youtu\.be\/)([A-Za-z0-9_-]{5,20})/);
        if (m) return 'https://www.youtube.com/embed/' + m[1];

        m = url.match(/vimeo\.com\/(?:video\/)?(\d+)/);
        if (m) return 'https://player.vimeo.com/video/' + m[1];

        // Google Drive: o link que a pessoa copia costuma ser o /view, que só abre a
        // página do Drive. Quem toca embutido é o /preview — o id é o mesmo.
        m = url.match(/drive\.google\.com\/file\/d\/([A-Za-z0-9_-]+)/);
        if (m) return 'https://drive.google.com/file/d/' + m[1] + '/preview';

        m = url.match(/drive\.google\.com\/(?:open|uc)\?[^#]*id=([A-Za-z0-9_-]+)/);
        if (m) return 'https://drive.google.com/file/d/' + m[1] + '/preview';

        return null;
    }

    // Arquivo de vídeo servido direto (o /Files do próprio PCM, um CDN): não precisa
    // de iframe, o <video> do navegador dá conta e ainda respeita o clique para tocar.
    function ehArquivoVideo(url) {
        return /\.(mp4|webm|ogv|ogg|m4v|mov)(\?|#|$)/i.test(url);
    }

    function montarVideo(url, aberta) {
        if (!url || !/^https?:\/\//i.test(url)) return '';

        if (ehArquivoVideo(url)) {
            return '<div class="manual-video"><video controls preload="none" ' +
                   'src="' + escapar(url) + '"></video></div>';
        }

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

    /* ── presença do botão ──
       O "?" só existe onde há manual cadastrado: a busca da carga decide, para
       todo mundo — quem mantém o manual cria o da tela pela manutenção
       (HelpIndex), não por aqui. Se a busca falhar (rede, banco), o botão fica
       e o clique tenta de novo, como antes. */
    buscar(function () {
        $btn.toggleClass('js-hidden', !manual);
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
