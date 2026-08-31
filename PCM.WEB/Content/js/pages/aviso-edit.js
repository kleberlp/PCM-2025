/* aviso-edit.js — cadastro de Avisos aos Clientes (AvisoInsert/AvisoEdit).

   Cabeçalho e seções gravados de uma vez (JSON), como o manual: salvar em
   partes deixaria meio caminho gravado se o navegador caísse no meio. */
jQuery(function () {

    'use strict';

    var $dados = jQuery('#aviso-data');
    var aviso = JSON.parse($dados.attr('data-aviso') || '{}');
    var molde = document.getElementById('moldeSecaoAviso');

    /* ── unidades da empresa escolhida ──
       Empresa TODAS obriga unidade TODAS (o alvo por unidade só faz sentido
       dentro de uma empresa). */
    function carregarUnidades(empresa, selecionada) {
        var $u = jQuery('#unidadeAviso').empty()
            .append(jQuery('<option>').attr('value', '-1').text('TODAS'));

        if (Number(empresa) <= 0) {
            $u.prop('disabled', true).val('-1');
            return;
        }

        $u.prop('disabled', false);

        jQuery.getJSON($dados.attr('data-url-unidades'), { empresa: empresa })
            .done(function (unidades) {
                (unidades || []).forEach(function (u) {
                    $u.append(jQuery('<option>').attr('value', u.codigo).text(u.descricao));
                });
                if (selecionada && $u.find('option[value="' + selecionada + '"]').length) {
                    $u.val(String(selecionada));
                }
            });
    }

    jQuery('#empresaAviso').on('change', function () {
        carregarUnidades(jQuery(this).val(), -1);
    });

    /* ── seções 1:N ── */
    function renumerar() {
        jQuery('#secoesAviso .av-secao-ordem').each(function (i) {
            jQuery(this).text(i + 1);
        });
    }

    function addSecao(titulo, conteudo) {
        var novo = molde.content.cloneNode(true);
        jQuery('#secoesAviso').append(novo);

        var $bloco = jQuery('#secoesAviso .av-secao').last();
        $bloco.find('.av-secao-titulo').val(titulo || '');
        $bloco.find('.av-secao-conteudo').val(conteudo || '');
        renumerar();
    }

    jQuery('#btnAddSecao').on('click', function () { addSecao('', ''); });

    jQuery('#secoesAviso').on('click', '.av-secao-remover', function () {
        if (jQuery('#secoesAviso .av-secao').length <= 1) { return; }
        jQuery(this).closest('.av-secao').remove();
        renumerar();
    });

    /* ── pré-visualização ──
       Mostra o aviso no MESMO popup do login (aviso-popup.js, no layout):
       trilha, carrossel e estrelas de verdade, montados com o que está no
       formulário — e sem registrar visualização/avaliação/dispensa. */

    // Espelho leve da sanitização do servidor: a prévia não deve mostrar o
    // que o salvar vai remover (script/style/invólucro de documento colado).
    function limparHtmlPrevia(html) {
        return (html || '')
            .replace(/<!--[\s\S]*?-->/g, '')
            .replace(/<!DOCTYPE[^>]*>/gi, '')
            .replace(/<\s*(script|style|title)\b[^>]*>[\s\S]*?<\s*\/\s*\1\s*>/gi, '')
            .replace(/<\s*\/?\s*(html|head|body)\b[^>]*>/gi, '')
            .replace(/<\s*(meta|link|base)\b[^>]*\/?\s*>/gi, '');
    }

    jQuery('#btnPreviaAviso').on('click', function () {

        if (!window.PcmAvisoPopup) { return; }

        var secoes = jQuery('#secoesAviso .av-secao').map(function (i) {
            var $s = jQuery(this);
            return {
                titulo: jQuery.trim($s.find('.av-secao-titulo').val() || ''),
                conteudo: limparHtmlPrevia($s.find('.av-secao-conteudo').val() || '')
            };
        }).get().filter(function (s) { return s.titulo !== '' || s.conteudo !== ''; });

        if (!secoes.length) {
            secoes = [{ titulo: 'Sem conteúdo', conteudo: '<p>Inclua ao menos uma seção para pré-visualizar.</p>' }];
        }

        var termino = (jQuery('#dataTerminoAviso').val() || '').split('-'); // yyyy-mm-dd

        window.PcmAvisoPopup.exibir({
            codigo: 0,
            titulo: jQuery('#tituloAviso').val() || '(sem título)',
            data_termino: termino.length === 3 ? termino[2] + '/' + termino[1] + '/' + termino[0] : '',
            auditado: false,
            avaliado: jQuery('#avaliadoAviso').prop('checked'),
            avaliacao: 0,
            secoes: secoes
        });
    });

    /* ── salvar ── */
    jQuery('#btnSalvarAviso').on('click', function () {

        var secoes = jQuery('#secoesAviso .av-secao').map(function (i) {
            var $s = jQuery(this);
            return {
                sequencia: i + 1,
                titulo: jQuery.trim($s.find('.av-secao-titulo').val() || ''),
                conteudo: $s.find('.av-secao-conteudo').val() || ''
            };
        }).get().filter(function (s) { return s.titulo !== ''; });

        if (!(jQuery('#tituloAviso').val() || '').trim() ||
            !jQuery('#dataInicioAviso').val() || !jQuery('#dataTerminoAviso').val() ||
            secoes.length === 0) {
            Swal.fire({ text: $dados.attr('data-msg-requerido'), icon: 'warning' });
            return;
        }

        var $btn = jQuery(this).prop('disabled', true);

        jQuery.ajax({
            url: $dados.attr('data-url-save'), type: 'POST',
            data: {
                codigo: aviso.codigo || 0,
                titulo: jQuery('#tituloAviso').val(),
                data_inicio: jQuery('#dataInicioAviso').val(),
                data_termino: jQuery('#dataTerminoAviso').val(),
                empresa: jQuery('#empresaAviso').val(),
                unidade: jQuery('#unidadeAviso').val() || -1,
                auditado: jQuery('#auditadoAviso').prop('checked'),
                avaliado: jQuery('#avaliadoAviso').prop('checked'),
                ativo: jQuery('#ativoAviso').val() === '1',
                secoesJson: JSON.stringify(secoes)
            },
            success: function (r) {
                $btn.prop('disabled', false);
                if (r.success) { window.location.href = $dados.attr('data-url-index'); }
                else { Swal.fire({ text: r.message, icon: 'error' }); }
            },
            error: function (xhr) {
                $btn.prop('disabled', false);
                // O código HTTP diz onde morreu (500 validação/erro no servidor,
                // 404 rota, 413 tamanho): sem ele o suporte fica no escuro.
                Swal.fire({ text: 'Não foi possível salvar o aviso (HTTP ' + (xhr && xhr.status || 0) + ').', icon: 'error' });
            }
        });
    });

    /* ── carga ── */
    jQuery('#tituloAviso').val(aviso.titulo || '');
    jQuery('#dataInicioAviso').val(aviso.data_inicio || '');
    jQuery('#dataTerminoAviso').val(aviso.data_termino || '');
    jQuery('#ativoAviso').val(aviso.codigo > 0 && !aviso.ativo ? '0' : '1');
    jQuery('#empresaAviso').val(String(aviso.codigo_empresa != null ? aviso.codigo_empresa : -1));
    jQuery('#auditadoAviso').prop('checked', !!aviso.auditado);
    jQuery('#avaliadoAviso').prop('checked', !!aviso.avaliado);

    carregarUnidades(aviso.codigo_empresa, aviso.codigo_unidade);

    if (aviso.secoes && aviso.secoes.length) {
        aviso.secoes.forEach(function (s) { addSecao(s.titulo, s.conteudo); });
    } else {
        addSecao('', '');
    }
});
