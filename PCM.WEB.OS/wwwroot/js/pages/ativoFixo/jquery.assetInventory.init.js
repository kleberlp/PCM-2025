// ============================================================
//  PCM.WEB.OS - jquery.assetInventory.init.js
// ============================================================

$(document).ready(function () {

    // ---- App aberto sem uniqueId (atalho do PWA / link expirado) ----
    if ($('#solicitarEmail').val() === '1') {
        $('#modalEmail').addClass('show');
        $('#emailContador').trigger('focus');
    }

    $('#btnEmailEntrar').on('click', function () {
        identificarContador();
    });

    $('#emailContador').on('keydown', function (e) {
        if (e.key === 'Enter') {
            e.preventDefault();
            identificarContador();
        }
    });

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
        const novoCadastro = $('#modalConfirmacao').data('ativoCadastrado') === false;

        $('#areaNok').toggleClass('show', !isOk);

        // Foto é exigida em avaliação N/OK e em novos cadastros
        $('#areaFoto').toggleClass('show', !isOk || novoCadastro);

        if (isOk) {
            $('#modalObservacao').val('');

            // Em novo cadastro a foto continua obrigatória, então é preservada
            if (!novoCadastro) {
                $('#inputFoto').val('');
                $('#fotoPreview').attr('src', '#').removeClass('show');
            }
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
//  Identificação por e-mail: localiza o inventário em aberto
//  vinculado ao contador e entra na aplicação
// ============================================================
function identificarContador() {

    var email = ($('#emailContador').val() || '').trim();
    var $erro = $('#emailErro');

    $erro.removeClass('show').text('');

    if (!email || !/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(email)) {
        $erro.text('Informe um e-mail válido.').addClass('show');
        $('#emailContador').trigger('focus');
        return;
    }

    var $btn = $('#btnEmailEntrar');
    $btn.prop('disabled', true).text('Enviando...');

    $.ajax({
        type: 'POST',
        url: urlIdentificarContador,
        data: { email: email },
        success: function (response) {

            if (response && response.success) {
                // O link de acesso é enviado ao e-mail cadastrado
                $('#emailFormArea').hide();
                $('#emailEnviado').text(response.message).addClass('show');
                return;
            }

            $erro.text((response && response.message) || 'Não foi possível concluir.').addClass('show');
            $btn.prop('disabled', false).text('Enviar link de acesso');
        },
        error: function () {
            $erro.text('Erro ao consultar. Tente novamente.').addClass('show');
            $btn.prop('disabled', false).text('Enviar link de acesso');
        }
    });

}

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

    // 2) Ativo existe — verifica se já foi contado neste inventário
    const check = await checkInventoried(barcode);

    // 2a) Já contado em OUTRO local: pergunta se deseja movimentar
    if (check.alreadyCounted && !check.sameLocation) {

        const movimentarResult = await Swal.fire({
            title: 'Ativo já inventariado' + (check.localAtual ? ' em: ' + check.localAtual : ''),
            text: 'Deseja movimentar o item para o local atual?',
            icon: 'question',
            showCancelButton: true,
            confirmButtonText: 'Sim',
            cancelButtonText: 'Não'
        });

        const movimentar = movimentarResult === true || (movimentarResult && movimentarResult.isConfirmed);
        if (!movimentar) {
            $('#barcode').val('').focus();
            return;
        }

        abrirModal(barcode, true, null, true, validacao); // conta e movimenta para o local atual
        return;
    }

    // 2b) Já contado NESTE local: apenas avisa
    if (check.alreadyCounted && check.sameLocation) {
        await Swal.fire({
            title: 'Ativo já inventariado neste local.',
            icon: 'info'
        });
        $('#barcode').val('').focus();
        return;
    }

    // 2c) Primeira contagem — abre modal de status
    abrirModal(barcode, true, null, false, validacao);
}

// ============================================================
//  Abre o modal de confirmação de status
// ============================================================
function abrirModal(barcode, ativoCadastrado, descricaoInformada, movimentar, avaliacao) {
    // Guarda contexto no modal via data attributes
    $('#modalConfirmacao')
        .data('barcode', barcode)
        .data('ativoCadastrado', ativoCadastrado)
        .data('descricaoInformada', descricaoInformada)
        .data('movimentar', movimentar === true);

    // Ponto 4: item encontrado vem pré-classificado com a última avaliação (edição opcional)
    const temAvaliacao = !!(avaliacao && avaliacao.possuiAvaliacao);
    const statusOk = temAvaliacao ? avaliacao.statusOk === true : true;

    const novoCadastro = ativoCadastrado === false;

    $('#stOk').prop('checked', statusOk);
    $('#stNok').prop('checked', !statusOk);
    $('#areaNok').toggleClass('show', !statusOk);
    $('#modalObservacao').val(!statusOk && temAvaliacao ? (avaliacao.observacao || '') : '');

    // Foto obrigatória: avaliação N/OK ou novo cadastro
    $('#areaFoto').toggleClass('show', !statusOk || novoCadastro);
    $('#fotoHint').text(novoCadastro
        ? 'Foto obrigatória para concluir o cadastro do novo ativo.'
        : 'Foto obrigatória para registrar o item como Não OK.');

    $('#inputFoto').val('');
    $('#fotoPreview').attr('src', '#').removeClass('show');
    $('#modalAssetCode').text('Ativo: ' + barcode);

    // Mensagens do modal: encontrado na base / pré-classificado
    $('#modalEncontrado').toggle(ativoCadastrado === true);
    $('#modalPreClassificado').toggle(temAvaliacao);

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

    const modal = $('#modalConfirmacao');
    const barcode = modal.data('barcode');
    const ativoCadastrado = modal.data('ativoCadastrado');
    const descricaoInformada = modal.data('descricaoInformada') || '';
    const statusOk = $('input[name="statusOk"]:checked').val() === 'true';
    const novoCadastro = ativoCadastrado === false;
    const observacao = statusOk ? '' : $('#modalObservacao').val().trim();
    const fotoFile = $('#inputFoto')[0].files[0] || null;

    // ---- Validações obrigatórias ----

    // Observação é obrigatória quando o item é apontado como Não OK
    if (!statusOk && !observacao) {
        Swal.fire({
            title: 'Observação obrigatória',
            text: 'Descreva o problema encontrado para registrar o item como Não OK.',
            icon: 'warning'
        }).then(function () { $('#modalObservacao').focus(); });
        return;
    }

    // Foto é obrigatória em avaliação Não OK e em novos cadastros
    if ((!statusOk || novoCadastro) && !fotoFile) {
        Swal.fire({
            title: 'Foto obrigatória',
            text: novoCadastro
                ? 'É necessário tirar uma foto para concluir o cadastro do novo ativo.'
                : 'É necessário tirar uma foto para registrar o item como Não OK.',
            icon: 'warning'
        });
        return;
    }

    $('#btnModalConfirmar').prop('disabled', true);

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
    fd.append('movimentar', modal.data('movimentar') === true);
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
//  Consulta se o ativo já foi contado neste inventário e onde
// ============================================================
function checkInventoried(assetCode) {
    return $.ajax({
        type: 'POST',
        url: urlCheckAssetInventoried,
        data: {
            codigoInventario: $('#codigoInventario').val(),
            assetCode: assetCode,
            setor: $('#codigoSetor').val() || -1,
            apartamento: $('#codigoApartamento').val() || -1
        }
    }).catch(function () {
        // Falha na verificação não bloqueia a contagem
        return { alreadyCounted: false, sameLocation: false, localAtual: '' };
    });
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