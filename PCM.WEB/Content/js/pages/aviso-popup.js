/* aviso-popup.js — popup de Avisos aos Clientes no login.

   Busca os avisos pendentes (o servidor entrega uma vez por sessão) e mostra
   um popup por aviso, em sequência: cada aviso tem seu carrossel de seções,
   as 5 estrelas quando pede avaliação e o "não mostrar novamente".

   Fechar sem marcar = o aviso volta no próximo login. Marcar = dispensa
   definitiva (gravada). A exibição é registrada quando o aviso é auditado.

   O conteúdo das seções é HTML cadastrado pela administração — sanitizado
   na gravação (server-side) e inserido aqui via innerHTML; a CSP bloqueia
   qualquer script inline que sobreviva. */
jQuery(function () {

    'use strict';

    var $root = jQuery('#aviso-popup-root');
    if (!$root.length) { return; }

    var urls = {
        avisos: $root.attr('data-url-avisos'),
        visualizado: $root.attr('data-url-visualizado'),
        avaliar: $root.attr('data-url-avaliar'),
        dispensar: $root.attr('data-url-dispensar')
    };

    var fila = [];      // avisos pendentes, mostrados um por vez
    var avisoAtual = null;
    var slideAtual = 0;

    /* ── carrossel ──
       A navegação vive na trilha de seções: cada seção é uma etapa numerada
       com nome, para ninguém fechar o aviso sem perceber que há outras. */
    function irPara(i) {
        var $slides = $root.find('.avp-slide');
        slideAtual = Math.max(0, Math.min($slides.length - 1, i));

        $slides.removeClass('avp-ativo').eq(slideAtual).addClass('avp-ativo');

        $root.find('.avp-trilha button').each(function (idx) {
            jQuery(this).toggleClass('avp-atual', idx === slideAtual)
                        .toggleClass('avp-vista', idx < slideAtual);
        });

        $root.find('.avp-prev').prop('disabled', slideAtual === 0);
        $root.find('.avp-next').prop('disabled', slideAtual === $slides.length - 1);

        // barra de progresso acompanha o avanço
        $root.find('.avp-progresso > div')
             .css('width', (($slides.length ? (slideAtual + 1) / $slides.length : 0) * 100) + '%');

        // na última seção o caminho natural é encerrar
        $root.find('.avp-prox').text(slideAtual === $slides.length - 1 ? 'Concluir' : 'Próxima seção ›');

        $root.find('.avp-carrossel').scrollTop(0);
    }

    $root.on('click', '.avp-prev', function () { irPara(slideAtual - 1); });
    $root.on('click', '.avp-next', function () { irPara(slideAtual + 1); });
    $root.on('click', '.avp-trilha button', function () { irPara(jQuery(this).index()); });

    $root.on('click', '.avp-prox', function () {
        if (slideAtual < $root.find('.avp-slide').length - 1) { irPara(slideAtual + 1); }
        else { fechar(); }
    });

    /* ── estrelas ── */
    function pintarEstrelas(nota) {
        $root.find('.avp-estrela').each(function () {
            jQuery(this).toggleClass('avp-cheia', Number(jQuery(this).attr('data-v')) <= nota);
        });
    }

    $root.on('click', '.avp-estrela', function () {
        var nota = Number(jQuery(this).attr('data-v'));
        pintarEstrelas(nota);
        $root.find('.avp-obrigado').removeClass('js-hidden');

        if (avisoAtual) {
            jQuery.post(urls.avaliar, { codigo: avisoAtual.codigo, avaliacao: nota });
        }
    });

    /* ── monta um aviso no popup ── */
    function mostrar(aviso) {

        avisoAtual = aviso;

        $root.find('.avp-titulo').text(aviso.titulo);
        $root.find('.avp-periodo').text('Aviso válido até ' + aviso.data_termino);

        var $slides = $root.find('.avp-slides').empty();
        var $trilha = $root.find('.avp-trilha').empty();

        (aviso.secoes || []).forEach(function (secao, i) {
            var slide = document.createElement('section');
            slide.className = 'avp-slide' + (i === 0 ? ' avp-ativo' : '');

            var h = document.createElement('h4');
            h.textContent = secao.titulo;
            slide.appendChild(h);

            var corpo = document.createElement('div');
            corpo.className = 'avp-slide-conteudo';
            // HTML da administração, sanitizado na gravação
            corpo.innerHTML = secao.conteudo || '';
            slide.appendChild(corpo);

            $slides.append(slide);

            // etapa da trilha: número + nome da seção (textContent: título é dado)
            var etapa = document.createElement('button');
            etapa.type = 'button';

            var num = document.createElement('span');
            num.className = 'avp-num';
            num.textContent = i + 1;
            etapa.appendChild(num);
            etapa.appendChild(document.createTextNode(secao.titulo || ('Seção ' + (i + 1))));

            $trilha.append(etapa);
        });

        // Aviso de seção única dispensa trilha, setas e progresso.
        var umaSecao = (aviso.secoes || []).length <= 1;
        $root.find('.avp-prev, .avp-next').toggleClass('js-hidden', umaSecao);
        $root.find('.avp-trilha, .avp-progresso').toggleClass('js-hidden', umaSecao);

        $root.find('.avp-avaliacao').toggleClass('js-hidden', !aviso.avaliado);
        $root.find('.avp-obrigado').toggleClass('js-hidden', !(aviso.avaliacao > 0));
        pintarEstrelas(aviso.avaliacao || 0);

        $root.find('.avp-nao-ver-chk').prop('checked', false);

        irPara(0);
        $root.addClass('avp-aberto');

        // log de auditoria: o aviso foi exibido para este usuário
        if (aviso.auditado) {
            jQuery.post(urls.visualizado, { codigo: aviso.codigo });
        }
    }

    /* ── fechar / próximo da fila ── */
    function fechar() {

        if (avisoAtual && $root.find('.avp-nao-ver-chk').prop('checked')) {
            jQuery.post(urls.dispensar, { codigo: avisoAtual.codigo });
        }

        avisoAtual = null;

        if (fila.length) {
            mostrar(fila.shift());
        } else {
            $root.removeClass('avp-aberto');
        }
    }

    $root.on('click', '.avp-ok, .avp-fechar', fechar);

    /* ── carga: uma vez por sessão (o servidor controla) ── */
    jQuery.getJSON(urls.avisos).done(function (avisos) {
        if (!avisos || !avisos.length) { return; }
        fila = avisos;
        mostrar(fila.shift());
    });
});
