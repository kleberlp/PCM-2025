/* aviso-index.js — lista de Avisos aos Clientes (exclusão com confirmação). */
jQuery(function () {

    'use strict';

    var $dados = jQuery('#aviso-index-data');

    jQuery('.av-excluir').on('click', function () {

        var codigo = jQuery(this).data('codigo');
        var titulo = jQuery(this).data('titulo');
        var $linha = jQuery(this).closest('tr');

        Swal.fire({
            title: 'Excluir aviso?',
            text: '"' + titulo + '" — as visualizações e avaliações registradas também serão apagadas.',
            icon: 'warning',
            showCancelButton: true,
            confirmButtonText: 'Excluir',
            cancelButtonText: 'Cancelar'
        }).then(function (r) {

            if (!r.value) { return; }

            jQuery.post($dados.attr('data-url-delete'), { codigo: codigo })
                .done(function (resp) {
                    if (resp.success) { $linha.remove(); }
                    else { Swal.fire({ text: resp.message, icon: 'error' }); }
                })
                .fail(function () {
                    Swal.fire({ text: 'Não foi possível excluir o aviso.', icon: 'error' });
                });
        });
    });
});
