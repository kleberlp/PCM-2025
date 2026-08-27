// ============================================================
//  csp-actions.js — degrau 2 da CSP (docs/MANUAL-MIGRACAO-DOTNET10-SEGURANCA.md)
//
//  Substitui os handlers inline (onclick= / onchange= / ...) por atributos
//  data-* com delegação no document. Com a CSP em enforce, handler inline é
//  bloqueado pelo navegador; este dispatcher é o caminho permitido.
//
//  Atributos suportados (executados nesta ordem no disparo):
//    data-setval='{"#campo": "valor"}'   -> $(sel).val(valor)
//    data-select2val='{"#campo": "3"}'   -> $(sel).select2('val', valor)
//    data-act="funcao" ou "Obj.metodo"   -> chama a função global
//    data-args='[1, "x", "__self__"]'    -> argumentos do data-act
//                                           ("__self__" vira o elemento clicado)
//    data-clear-file-label               -> limpa o rótulo do input de arquivo
//    data-show="#sel" / data-hide="#sel" -> $(sel).show() / $(sel).hide()
//    data-disable="self" ou "#sel"       -> desabilita o botão/elemento
//    data-form-action="Url"              -> troca a action do form do data-submit
//    data-submit="#form"                 -> $(form).trigger('submit')
//
//    data-on="change|blur|keyup|input"   -> evento que dispara (padrão: click)
//
//    data-enter-focus="#campo"           -> Enter move o foco (e não submete)
//    data-enter-act="funcao"             -> Enter chama a função (e não submete)
// ============================================================

(function ($) {
    'use strict';

    // "Codebase.helpers" -> { fn, ctx } preservando o this do objeto dono
    function resolver(nome) {
        var partes = String(nome).split('.');
        var dono = window;

        for (var i = 0; i < partes.length - 1; i++) {
            dono = dono ? dono[partes[i]] : null;
        }

        var fn = dono ? dono[partes[partes.length - 1]] : null;

        return (typeof fn === 'function') ? { fn: fn, ctx: dono } : null;
    }

    function executar(el) {

        var $el = $(el);

        var setval = $el.data('setval');
        if (setval) {
            $.each(setval, function (sel, valor) { $(sel).val(valor); });
        }

        var s2 = $el.data('select2val');
        if (s2) {
            $.each(s2, function (sel, valor) { $(sel).select2('val', String(valor)); });
        }

        var act = $el.attr('data-act');
        if (act) {
            var alvo = resolver(act);
            if (alvo) {
                var brutos = $el.data('args') || [];
                var args = [];
                for (var i = 0; i < brutos.length; i++) {
                    args.push(brutos[i] === '__self__' ? el : brutos[i]);
                }
                // this = elemento, como no handler inline original
                alvo.fn.apply(el, args);
            } else if (window.console) {
                console.error('csp-actions: função não encontrada:', act);
            }
        }

        if ($el.is('[data-clear-file-label]')) {
            $el.next('.custom-file-label').addClass('selected').html('');
        }

        var mostrar = $el.attr('data-show');
        if (mostrar) { $(mostrar).show(); }

        var esconder = $el.attr('data-hide');
        if (esconder) { $(esconder).hide(); }

        var desabilitar = $el.attr('data-disable');
        if (desabilitar) {
            (desabilitar === 'self' ? $el : $(desabilitar)).prop('disabled', true);
        }

        var form = $el.attr('data-submit');
        if (form) {
            var $form = $(form);

            var acaoForm = $el.attr('data-form-action');
            if (acaoForm) { $form.attr('action', acaoForm); }

            $form.trigger('submit');
        }
    }

    var SELETOR = '[data-act],[data-submit],[data-show],[data-hide],' +
                  '[data-setval],[data-select2val],[data-clear-file-label],[data-disable]';

    // focusout é a versão que borbulha do blur
    $(document).on('click change focusout keyup input', SELETOR, function (e) {

        var quando = $(this).attr('data-on') || 'click';
        var tipo = (e.type === 'focusout') ? 'blur' : e.type;

        if (tipo !== quando) { return; }

        executar(this);
    });

    // Enter em campos: move o foco ou chama função, sem submeter o form
    $(document).on('keydown', '[data-enter-focus],[data-enter-act]', function (e) {

        if (e.key !== 'Enter' && e.keyCode !== 13) { return; }

        e.preventDefault();

        var foco = $(this).attr('data-enter-focus');
        if (foco) {
            $(foco).focus();
            return;
        }

        var alvo = resolver($(this).attr('data-enter-act'));
        if (alvo) { alvo.fn.call(this); }
    });

})(jQuery);
