/* manual-edit.js — manutenção do manual: cabeçalho e seções (HelpInsert/HelpEdit).

   As seções são editadas em bloco e gravadas de uma vez, junto do cabeçalho:
   mexer no manual é mexer em texto e ordem ao mesmo tempo, e salvar seção a
   seção deixaria meio caminho gravado se o navegador caísse no meio. */
jQuery(function () {

    'use strict';

    var $dados = jQuery('#manual-data');
    var manual = JSON.parse($dados.attr('data-manual') || '{}');
    var telas = JSON.parse($dados.attr('data-telas') || '{}');
    var molde = document.getElementById('moldeSecao');

    /* ── tela (controller/action) ──
       As listas vêm por reflexão dos controllers: um mapa em tabela seria
       cadastro para alguém esquecer de manter. Action vazia = manual do módulo
       inteiro (qualquer tela do controller sem manual próprio cai nele). */

    function montarControllers(atual) {
        var $c = jQuery('#controllerManual').empty();
        var nomes = Object.keys(telas);
        if (atual && nomes.indexOf(atual) < 0) nomes.push(atual);
        nomes.sort();
        nomes.forEach(function (n) {
            $c.append(jQuery('<option>').attr('value', n).text(n));
        });
        if (atual) $c.val(atual);
    }

    function montarActions(controller, atual) {
        var $a = jQuery('#actionManual').empty();
        $a.append(jQuery('<option>').attr('value', '').text('— módulo inteiro —'));
        var acoes = (telas[controller] || []).slice();
        if (atual && acoes.indexOf(atual) < 0) acoes.push(atual);
        acoes.sort();
        acoes.forEach(function (a) {
            $a.append(jQuery('<option>').attr('value', a).text(a));
        });
        $a.val(atual || '');
    }

    jQuery('#controllerManual').on('change', function () {
        montarActions(jQuery(this).val(), '');
    });

    /* ── telas irmas ──
       O mesmo manual pode atender varias telas (ex.: as listas de O.S. e o
       Kanban): cadastra-se o texto uma vez e o "?" mostra em todas. A tela
       principal continua nos campos acima; estas sao as adicionais. */

    var telasExtras = (manual.telas || []).map(function (t) {
        return { controller: t.controller || '', action: t.action || '' };
    });

    function rotuloTela(t) {
        return t.controller + (t.action ? '/' + t.action : ' (módulo inteiro)');
    }

    function pintarTelasExtras() {
        var $lista = jQuery('#listaTelasExtras').empty();
        if (!telasExtras.length) {
            $lista.append(jQuery('<span>').addClass('text-muted font-size-sm').text('—'));
            return;
        }
        telasExtras.forEach(function (t, idx) {
            var $chip = jQuery('<span>').addClass('badge badge-secondary mr-5 mb-5 js-tela-chip').text(rotuloTela(t));
            jQuery('<a>').attr({ href: '#', 'data-idx': idx, title: 'Remover' })
                .addClass('ml-5 js-remove-tela').text('×').appendTo($chip);
            $lista.append($chip);
        });
    }

    function montarComboExtra() {
        var $c = jQuery('#controllerTelaExtra').empty();
        Object.keys(telas).sort().forEach(function (n) {
            $c.append(jQuery('<option>').attr('value', n).text(n));
        });
        montarActionsExtra($c.val());
    }

    function montarActionsExtra(controller) {
        var $a = jQuery('#actionTelaExtra').empty();
        $a.append(jQuery('<option>').attr('value', '').text('— módulo inteiro —'));
        (telas[controller] || []).slice().sort().forEach(function (acao) {
            $a.append(jQuery('<option>').attr('value', acao).text(acao));
        });
    }

    jQuery('#controllerTelaExtra').on('change', function () {
        montarActionsExtra(jQuery(this).val());
    });

    jQuery('#addTelaExtra').on('click', function () {
        var nova = {
            controller: jQuery('#controllerTelaExtra').val() || '',
            action: jQuery('#actionTelaExtra').val() || ''
        };
        if (!nova.controller) { return; }

        // a tela principal ja e atendida pelo proprio cabecalho
        var principal = (jQuery('#controllerManual').val() || '') + '/' + (jQuery('#actionManual').val() || '');
        var repetida = telasExtras.some(function (t) {
            return t.controller === nova.controller && t.action === nova.action;
        });

        if (nova.controller + '/' + nova.action === principal || repetida) {
            Swal.fire({ text: 'Essa tela já está associada a este manual.', icon: 'info' });
            return;
        }

        telasExtras.push(nova);
        pintarTelasExtras();
    });

    jQuery('#listaTelasExtras').on('click', '.js-remove-tela', function (e) {
        e.preventDefault();
        telasExtras.splice(parseInt(jQuery(this).attr('data-idx'), 10), 1);
        pintarTelasExtras();
    });

    /* ── tela x processo ──
       Um manual é de uma coisa só: processo não tem tela nem "ver também". */
    jQuery('#tipoManual').on('change', function () {
        var processo = jQuery(this).val() === 'P';
        jQuery('.js-box-tela').toggleClass('js-hidden', processo);
    });

    /* ── seções ── */

    function renumerar() {
        jQuery('#secoes .secao').each(function (i) {
            jQuery(this).find('.secao-num').text($dados.attr('data-msg-secao') + ' ' + (i + 1));
        });
    }

    function novaSecao(dados) {

        var $s = jQuery(molde.content.cloneNode(true).firstElementChild);
        dados = dados || {};

        $s.find('.secao-titulo').val(dados.titulo || '');
        $s.find('.secao-texto').val(dados.conteudo || '');
        $s.find('.secao-tipo-nota').val(dados.tipo_nota || '');
        $s.find('.secao-nota').val(dados.nota || '');
        $s.find('.secao-video').val(dados.video || '');

        if (dados.imagem) {
            $s.find('.secao-img').attr('src', dados.imagem);
            $s.find('.secao-img-box').removeClass('js-hidden');
        }

        jQuery('#secoes').append($s);
        renumerar();
        return $s;
    }

    jQuery('#btnAddSecao').on('click', function (e) {
        e.preventDefault();
        novaSecao().find('.secao-titulo').trigger('focus');
    });

    jQuery(document).on('click', '.secao-remove', function (e) {
        e.preventDefault();
        jQuery(this).closest('.secao').remove();
        renumerar();
    });

    jQuery(document).on('click', '.secao-sobe', function (e) {
        e.preventDefault();
        var $s = jQuery(this).closest('.secao');
        $s.prev('.secao').before($s);
        renumerar();
    });

    jQuery(document).on('click', '.secao-desce', function (e) {
        e.preventDefault();
        var $s = jQuery(this).closest('.secao');
        $s.next('.secao').after($s);
        renumerar();
    });

    /* ── imagem ──
       Sobe na hora de escolher, e não junto do salvar: assim o autor vê a
       imagem que escolheu antes de gravar o manual. */
    jQuery(document).on('change', '.secao-arquivo', function () {

        var arquivo = (this.files || [])[0];
        if (!arquivo) return;

        var $s = jQuery(this).closest('.secao');
        var dados = new FormData();
        dados.append('imagem', arquivo);

        jQuery.ajax({
            url: $dados.attr('data-url-upload'), type: 'POST',
            data: dados, processData: false, contentType: false,
            success: function (r) {
                if (!r.success) { Swal.fire({ text: r.message, icon: 'error' }); return; }
                $s.find('.secao-img').attr('src', r.path);
                $s.find('.secao-img-box').removeClass('js-hidden');
            },
            error: function () { Swal.fire({ text: 'Não foi possível enviar a imagem.', icon: 'error' }); }
        });
    });

    jQuery(document).on('click', '.secao-img-remove', function (e) {
        e.preventDefault();
        var $s = jQuery(this).closest('.secao');
        $s.find('.secao-img').attr('src', '');
        $s.find('.secao-arquivo').val('');
        $s.find('.secao-img-box').addClass('js-hidden');
    });

    /* ── salvar ── */
    jQuery('#btnSalvarManual').on('click', function (e) {

        e.preventDefault();

        var processo = jQuery('#tipoManual').val() === 'P';

        if (!(jQuery('#tituloManual').val() || '').trim() || (!processo && !jQuery('#controllerManual').val())) {
            Swal.fire({ text: $dados.attr('data-msg-requerido'), icon: 'warning' });
            return;
        }

        var secoes = jQuery('#secoes .secao').map(function (i) {
            var $s = jQuery(this);
            return {
                sequencia: i + 1,
                titulo: (jQuery.trim($s.find('.secao-titulo').val() || '')),
                conteudo: $s.find('.secao-texto').val() || '',
                tipo_nota: $s.find('.secao-tipo-nota').val() || '',
                nota: $s.find('.secao-nota').val() || '',
                imagem: $s.find('.secao-img-box').hasClass('js-hidden') ? '' : ($s.find('.secao-img').attr('src') || ''),
                video: jQuery.trim($s.find('.secao-video').val() || '')
            };
        }).get().filter(function (s) { return s.titulo !== ''; });

        var $btn = jQuery(this).prop('disabled', true);

        jQuery.ajax({
            url: $dados.attr('data-url-save'), type: 'POST',
            data: {
                codigo: manual.codigo || 0,
                tipo: processo ? 'P' : 'S',
                tela_controller: processo ? '' : jQuery('#controllerManual').val(),
                tela_action: processo ? '' : (jQuery('#actionManual').val() || ''),
                processo: processo ? 0 : parseInt(jQuery('#processoManual').val(), 10) || 0,
                titulo: jQuery('#tituloManual').val(),
                subtitulo: jQuery('#subtituloManual').val(),
                ativo: jQuery('#ativoManual').val() === '1',
                itensJson: JSON.stringify(secoes),
                telasJson: JSON.stringify(processo ? [] : telasExtras)
            },
            success: function (r) {
                $btn.prop('disabled', false);
                if (r.success) { window.location.href = $dados.attr('data-url-index'); }
                else { Swal.fire({ text: r.message, icon: 'error' }); }
            },
            error: function () {
                $btn.prop('disabled', false);
                Swal.fire({ text: 'Não foi possível salvar o manual.', icon: 'error' });
            }
        });
    });

    /* ── carga ── */
    montarControllers(manual.controller || '');
    montarActions(jQuery('#controllerManual').val(), manual.action || '');
    montarComboExtra();
    pintarTelasExtras();

    jQuery('#tipoManual').val(manual.tipo === 'P' ? 'P' : 'S').trigger('change');
    jQuery('#processoManual').val(String(manual.processo_codigo || 0));
    jQuery('#ativoManual').val(manual.codigo > 0 && !manual.ativo ? '0' : '1');
    jQuery('#tituloManual').val(manual.titulo || '');
    jQuery('#subtituloManual').val(manual.subtitulo || '');

    (manual.itens || []).forEach(novaSecao);

    // Manual novo já abre com uma seção: a tela vazia não diz o que fazer.
    if (!(manual.itens || []).length) novaSecao();

});
