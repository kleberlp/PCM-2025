// ============================================================
//  PCM.WEB.OS - jquery.assetInventory.init.js
// ============================================================

$(document).ready(function () {

    // ---- Filtro: setor -> recarrega apartamentos ----
    $('#codigoSetor').on('change', function () {
        const unidade = $('#codigoUnidade').val();
        const setor   = $(this).val() || -1;

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

        if (!confirmar.isConfirmed) {
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

        if (descResult.dismiss === Swal.DismissReason.cancel || !descResult.value) {
            $('#barcode').val('').focus();
            return;
        }

        await registrarInventario(false, descResult.value);
        return;
    }

    // 2) Ativo existe — registra diretamente
    await registrarInventario(true, null);
}

// ============================================================
//  Valida asset via AJAX (retorna Promise)
// ============================================================
function validarAsset(assetCode) {
    return $.ajax({
        type: 'POST',
        url: urlValidaAsset,
        data: {
            unidade:   $('#codigoUnidade').val(),
            assetCode: assetCode
        }
    });
}

// ============================================================
//  Registra no inventário via AJAX
// ============================================================
async function registrarInventario(isValidAsset, descricao) {
    const response = await $.ajax({
        type: 'POST',
        url: urlInsertAssetInventory,
        data: {
            codigoInventario:  $('#codigoInventario').val(),
            unidade:           $('#codigoUnidade').val(),
            setor:             $('#codigoSetor').val()      || -1,
            apartamento:       $('#codigoApartamento').val() || -1,
            assetCode:         $('#barcode').val().trim(),
            ativoCadastrado:   isValidAsset,
            descricaoInformada: descricao
        }
    });

    if (response.success) {
        $('#barcode').val('').focus();
        reloadGrid();
    } else {
        Swal.fire({ title: response.message, icon: 'error' });
    }
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
            unidade:          $('#codigoUnidade').val(),
            setor:            $('#codigoSetor').val()      || -1,
            apartamento:      $('#codigoApartamento').val() || -1
        },
        success: function (data) {
            const $tbody = $('#tbInventario tbody');
            $tbody.empty();

            (data || []).forEach(function (item) {
                $tbody.append(
                    `<tr class="${item.cssClass}">
                        <td class="text-center">${item.asset}</td>
                        <td>${item.descricao}</td>
                    </tr>`
                );
            });
        }
    });
}
