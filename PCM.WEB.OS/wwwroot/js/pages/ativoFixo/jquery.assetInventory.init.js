// ============================================================
//  PCM.WEB.OS - jquery.assetInventory.init.js
// ============================================================

$(document).ready(function () {

    // ---- Filtro: setor -> recarrega apartamentos ----
    $('#codigoSetor').on('change', function () {
        const unidade = $('#codigoUnidade').val();
        const setor = $(this).val() || -1;

        $.post(urlLoadListApartamento, { unidade: unidade, setor: setor }, function (data) {
            const $sel = $('#codigoApartamento');
            $sel.empty().append('<option value="">Todos</option>');
            (data || []).forEach(function (item) {
                $sel.append($('<option>', { value: item.codigo, text: item.descricao }));
            });
            reloadGrid();
        });
    });

    // ---- Filtro: apartamento -> recarrega grid ----
    $('#codigoApartamento').on('change', function () {
        reloadGrid();
    });

    // ---- Botão confirmar / Enter no barcode ----
    $('#btnConfirm').on('click', function () {
        processBarcode();
    });

    $('#barcode').on('keydown', function (e) {
        if (e.key === 'Enter') {
            e.preventDefault();
            processBarcode();
        }
    });

    // ---- Toggle OK / Não OK no modal ----
    $('input[name="statusOk"]').on('change', function () {
        const isOk = $(this).val() === 'true';
        $('#areaNok').toggleClass('show', !isOk);
        if (isOk) {
            $('#modalObservacao').val('');
            $('#inputFoto').val('');
            $('#fotoPreview').attr('src', '#').removeClass('show');
        }
    });

    // ---- Preview da foto ----
    $('#inputFoto').on('change', function () {
        const file = this.files[0];
        if (!file) return;
        const reader = new FileReader();
        reader.onload = function (e) {
            $('#fotoPreview').attr('src', e.target.result).addClass('show');
        };
        reader.readAsDataURL(file);
    });

    // ---- Cancelar modal ----
    $('#btnModalCancelar').on('click', function () {
        fecharModal();
    });

    // ---- Confirmar modal ----
    $('#btnModalConfirmar').on('click', function () {
        confirmarRegistro();
    });

    // ---- Focus inicial ----
    $('#barcode').focus();

});

// ============================================================
//  Processa leitura do barcode
// ============================================================
async function processBarcode() {
    const barcode = $('#barcode').val().trim();

    if (!barcode) {
        $('#barcode').focus();
        return;
    }

    // 1) Valida se o ativo existe no cadastro
    const validacao = await validarAsset(barcode);

    if (!validacao.success) {
        // Ativo não encontrado — confirma se deve registrar assim mesmo
        const confirmar = await Swal.fire({
            title: 'Ativo não encontrado',
            text: 'Deseja registrar este ativo no inventário mesmo assim?',
            icon: 'warning',
            showCancelButton: true,
            confirmButtonText: 'Sim',
            cancelButtonText: 'Não'
        });

        // compatível com SweetAlert2 v6 (retorna bool) e v7+ (retorna objeto)
        const confirmado = confirmar === true || (confirmar && confirmar.isConfirmed);
        if (!confirmado) {
            $('#barcode').val('').focus();
            return;
        }

        // Solicita descrição para o novo ativo
        const descResult = await Swal.fire({
            title: 'Informe a descrição do ativo',
            input: 'text',
            inputPlaceholder: 'Descrição do ativo fixo',
            showCancelButton: true,
            confirmButtonText: 'Salvar',
            cancelButtonText: 'Cancelar'
        });

        const cancelado = descResult === false ||
            (descResult && descResult.dismiss === Swal.DismissReason.cancel) ||
            !descResult.value;
        if (cancelado) {
            $('#barcode').val('').focus();
            return;
        }

        const desc = typeof descResult === 'string' ? descResult : descResult.value;
        abrirModal(barcode, false, desc);
        return;
    }

    // 2) Ativo existe — abre modal de status
    abrirModal(barcode, true, null);
}

// ============================================================
//  Abre o modal de confirmação de status
// ============================================================
function abrirModal(barcode, ativoCadastrado, descricaoInformada) {
    // Guarda contexto no modal via data attributes
    $('#modalConfirmacao')
        .data('barcode', barcode)
        .data('ativoCadastrado', ativoCadastrado)
        .data('descricaoInformada', descricaoInformada);

    // Reset estado
    $('#stOk').prop('checked', true);
    $('#areaNok').removeClass('show');
    $('#modalObservacao').val('');
    $('#inputFoto').val('');
    $('#fotoPreview').attr('src', '#').removeClass('show');
    $('#modalAssetCode').text('Ativo: ' + barcode);

    $('#modalConfirmacao').addClass('show');
    $('#btnModalConfirmar').prop('disabled', false);
}

function fecharModal() {
    $('#modalConfirmacao').removeClass('show');
    $('#barcode').val('').focus();
}

// ============================================================
//  Confirma e envia para o servidor
// ============================================================
async function confirmarRegistro() {
    $('#btnModalConfirmar').prop('disabled', true);

    const modal = $('#modalConfirmacao');
    const barcode = modal.data('barcode');
    const ativoCadastrado = modal.data('ativoCadastrado');
    const descricaoInformada = modal.data('descricaoInformada') || '';
    const statusOk = $('input[name="statusOk"]:checked').val() === 'true';
    const observacao = statusOk ? '' : $('#modalObservacao').val().trim();
    const fotoFile = statusOk ? null : ($('#inputFoto')[0].files[0] || null);

    // Monta FormData para suportar envio de arquivo
    const fd = new FormData();
    fd.append('codigoInventario', $('#codigoInventario').val());
    fd.append('unidade', $('#codigoUnidade').val());
    fd.append('setor', $('#codigoSetor').val() || -1);
    fd.append('apartamento', $('#codigoApartamento').val() || -1);
    fd.append('assetCode', barcode);
    fd.append('ativoCadastrado', ativoCadastrado);
    fd.append('descricaoInformada', descricaoInformada);
    fd.append('statusOk', statusOk);
    fd.append('observacao', observacao);
    if (fotoFile) fd.append('foto', fotoFile);

    try {
        const response = await $.ajax({
            type: 'POST',
            url: urlInsertAssetInventory,
            data: fd,
            processData: false,
            contentType: false
        });

        if (response.success) {
            fecharModal();
            reloadGrid();
        } else {
            $('#btnModalConfirmar').prop('disabled', false);
            Swal.fire({ title: response.message, icon: 'error' });
        }
    } catch (e) {
        $('#btnModalConfirmar').prop('disabled', false);
        Swal.fire({ title: 'Erro ao registrar ativo.', icon: 'error' });
    }
}

// ============================================================
//  Valida asset via AJAX
// ============================================================
function validarAsset(assetCode) {
    return $.ajax({
        type: 'POST',
        url: urlValidaAsset,
        data: {
            unidade: $('#codigoUnidade').val(),
            assetCode: assetCode
        }
    });
}

// ============================================================
//  Recarrega o grid de itens inventariados
// ============================================================
function reloadGrid() {
    $.ajax({
        type: 'POST',
        url: urlLoadAssetInventory,
        data: {
            codigoInventario: $('#codigoInventario').val(),
            unidade: $('#codigoUnidade').val(),
            setor: $('#codigoSetor').val() || -1,
            apartamento: $('#codigoApartamento').val() || -1
        },
        success: function (data) {
            const $tbody = $('#tbInventario tbody');
            $tbody.empty();

            (data || []).forEach(function (item) {
                let statusBadge = '';
                if (item.statusOk === true) statusBadge = '<span class="badge bg-success" style="font-size:.72rem">OK</span>';
                else if (item.statusOk === false) statusBadge = '<span class="badge bg-danger"  style="font-size:.72rem" title="' + (item.observacao || '') + '">N/OK</span>';

                $tbody.append(
                    `<tr class="${item.cssClass || ''}">
                        <td class="text-center">${item.asset || ''}</td>
                        <td>${item.descricao || ''}</td>
                        <td class="text-center">${statusBadge}</td>
                    </tr>`
                );
            });
        }
    });
}