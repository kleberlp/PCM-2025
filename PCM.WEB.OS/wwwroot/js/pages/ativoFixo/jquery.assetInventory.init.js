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

        $('#areaNok').toggleClass('show', !isOk);

        // Regra da foto reavaliada: Não OK sempre exige; no OK, só se o ativo não tiver foto
        atualizarAreaFoto(!isOk, $('#modalConfirmacao').data('semFoto') === true);

        if (isOk) {
            $('#modalObservacao').val('');
        }
    });

    // ---- Cadastro de novo ativo: busca do nome (mínimo 3 caracteres) ----
    $('#novoBusca').on('input', function () {

        var termo = $(this).val().trim();

        novoAtivo.descricao = '';
        $('#btnNovoAvancar1').prop('disabled', true);

        clearTimeout(buscaTimer);

        if (termo.length < 3) {
            $('#novoSugestoes').empty().removeClass('show');
            $('#novoBuscaHint').text('Digite ao menos 3 caracteres para buscar.');
            return;
        }

        // Pequeno atraso para não consultar a cada tecla
        buscaTimer = setTimeout(function () {
            buscarDescricao(termo);
        }, 300);
    });

    // ---- Seleção do equipamento na lista ----
    $(document).on('click', '#novoSugestoes li', function () {
        $('#novoSugestoes li').removeClass('selected');
        $(this).addClass('selected');

        novoAtivo.descricao = $(this).attr('data-descricao') || $(this).text();
        $('#novoBusca').val(novoAtivo.descricao);
        $('#btnNovoAvancar1').prop('disabled', false);
    });

    // ---- Navegação entre as etapas ----
    $('#btnNovoAvancar1').on('click', function () {
        if (!novoAtivo.descricao) return;
        irParaEtapa(2);
    });

    $('#btnNovoVoltar2').on('click', function () {
        irParaEtapa(1);
    });

    $('#btnNovoAvancar2').on('click', function () {
        if (!novoAtivo.foto) return;

        $('#novoResumoCodigo').text(novoAtivo.barcode);
        $('#novoResumoNome').text(novoAtivo.descricao);
        $('#novoResumoLocal').text(
            ($('#codigoSetor option:selected').text() || 'Todos') +
            ' / ' + ($('#codigoApartamento option:selected').text() || 'Todos')
        );
        $('#novoResumoFoto').attr('src', $('#novoFotoPreview').attr('src')).addClass('show');

        irParaEtapa(3);
    });

    $('#btnNovoVoltar3').on('click', function () {
        irParaEtapa(2);
    });

    $('#btnNovoCancelar').on('click', function () {
        cancelarNovoAtivo();
    });

    $('#btnNovoConfirmar').on('click', function () {
        confirmarNovoAtivo();
    });

    // ---- Foto do novo ativo (obrigatória) ----
    $('#novoInputFoto').on('change', function () {
        const file = this.files[0];
        if (!file) return;

        novoAtivo.foto = file;

        const reader = new FileReader();
        reader.onload = function (e) {
            $('#novoFotoPreview').attr('src', e.target.result).addClass('show');
            $('#btnNovoAvancar2').prop('disabled', false);
        };
        reader.readAsDataURL(file);
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

        // Cadastro do novo ativo em etapas (nome -> foto -> confirmação)
        abrirNovoAtivo(barcode);
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
//  Cadastro de novo ativo (código não encontrado na base)
//  Etapas: nome (cadastro pré-definido) -> foto -> confirmação
// ============================================================
var novoAtivo = { barcode: '', descricao: '', foto: null };
var buscaTimer = null;

function abrirNovoAtivo(barcode) {

    // O modal vive na view (compilada na DLL). Se o publish levou só o wwwroot,
    // o elemento não existe e a tela ficaria em branco sem nenhum aviso.
    if ($('#modalNovoAtivo').length === 0) {
        Swal.fire({
            title: 'Versão desatualizada',
            text: 'A tela publicada não tem o cadastro de novo ativo. Recarregue a página; se continuar, republique a aplicação (não apenas os arquivos estáticos).',
            icon: 'error'
        });
        return;
    }

    novoAtivo = { barcode: barcode, descricao: '', foto: null };

    $('#novoAssetCode').text('Código: ' + barcode);
    $('#novoBusca').val('');
    $('#novoSugestoes').empty().removeClass('show');
    $('#novoBuscaHint').text('Digite ao menos 3 caracteres para buscar.');
    $('#novoInputFoto').val('');
    $('#novoFotoPreview').attr('src', '#').removeClass('show');
    $('#novoResumoFoto').attr('src', '#').removeClass('show');
    $('#btnNovoAvancar1').prop('disabled', true);
    $('#btnNovoAvancar2').prop('disabled', true);

    irParaEtapa(1);
    $('#modalNovoAtivo').addClass('show');
    $('#novoBusca').trigger('focus');
}

function irParaEtapa(n) {
    $('.novo-step').removeClass('show');
    $('#novoStep' + n).addClass('show');
}

function fecharNovoAtivo() {
    $('#modalNovoAtivo').removeClass('show');
    novoAtivo = { barcode: '', descricao: '', foto: null };
}

// Etapa 1: busca no cadastro pré-definido a partir de 3 caracteres
function buscarDescricao(termo) {

    $.ajax({
        type: 'POST',
        url: urlBuscarDescricaoAtivo,
        data: { termo: termo },
        success: function (lista) {

            var $ul = $('#novoSugestoes').empty();

            // Nada no cadastro pré-definido: permite seguir com o texto digitado,
            // para o operador não ficar travado em campo
            if (!lista || lista.length === 0) {

                var $usar = $('<li>')
                    .addClass('novo-usar-digitado')
                    .attr('data-descricao', termo);

                $usar.append(document.createTextNode('Usar '));
                $usar.append($('<strong>').text('"' + termo + '"'));
                $usar.appendTo($ul);

                $ul.addClass('show');
                $('#novoBuscaHint').text('Nenhum equipamento encontrado. Você pode seguir com o nome digitado.');
                return;
            }

            lista.forEach(function (nome) {
                $('<li>').text(nome).attr('data-descricao', nome).appendTo($ul);
            });

            $ul.addClass('show');
            $('#novoBuscaHint').text('Selecione o equipamento na lista.');
        },
        error: function () {
            $('#novoSugestoes').empty().removeClass('show');
            $('#novoBuscaHint').text('Não foi possível buscar agora. Tente novamente.');
        }
    });

}

// Etapa 4 do fluxo: confirma o cadastro
function confirmarNovoAtivo() {

    var $btn = $('#btnNovoConfirmar');
    $btn.prop('disabled', true).text('Salvando...');

    var fd = new FormData();
    fd.append('uniqueId', $('#uniqueId').val());
    fd.append('codigoInventario', $('#codigoInventario').val());
    fd.append('unidade', $('#codigoUnidade').val());
    fd.append('setor', $('#codigoSetor').val() || -1);
    fd.append('apartamento', $('#codigoApartamento').val() || -1);
    fd.append('assetCode', novoAtivo.barcode);
    fd.append('ativoCadastrado', false);
    fd.append('descricaoInformada', novoAtivo.descricao);
    fd.append('statusOk', true);
    fd.append('observacao', '');
    if (novoAtivo.foto) fd.append('foto', novoAtivo.foto);

    $.ajax({
        type: 'POST',
        url: urlInsertAssetInventory,
        data: fd,
        processData: false,
        contentType: false,
        success: function (response) {

            $btn.prop('disabled', false).text('Confirmar cadastro');

            if (response && response.success) {
                fecharNovoAtivo();
                limparParaNovaBipagem();
                reloadGrid();
                Swal.fire({ title: 'Cadastro concluído com sucesso!', icon: 'success' });
                return;
            }

            Swal.fire({ title: (response && response.message) || 'Não foi possível cadastrar.', icon: 'error' });
        },
        error: function () {
            $btn.prop('disabled', false).text('Confirmar cadastro');
            Swal.fire({ title: 'Erro ao cadastrar o ativo.', icon: 'error' });
        }
    });

}

// Cancelamento: pergunta se descarta o código informado
async function cancelarNovoAtivo() {

    const result = await Swal.fire({
        title: 'Descartar o código informado?',
        text: 'Código: ' + novoAtivo.barcode,
        icon: 'question',
        showCancelButton: true,
        confirmButtonText: 'Sim, descartar',
        cancelButtonText: 'Continuar cadastro'
    });

    const descartar = result === true || (result && result.isConfirmed);

    if (descartar) {
        fecharNovoAtivo();
        limparParaNovaBipagem();
    }

}

// Deixa a tela livre para um novo cadastro ou bipagem
function limparParaNovaBipagem() {
    $('#barcode').val('').trigger('focus');
}

// ============================================================
//  Regra da foto: permitida somente em 3 casos
//   1) ativo bipado que ainda não possui foto
//   2) novo cadastro (fluxo próprio, acima)
//   3) avaliação Não OK
//  Fora deles o bloco nem aparece.
// ============================================================
function atualizarAreaFoto(naoOk, semFoto) {

    var exigir = naoOk || semFoto;

    $('#areaFoto').toggleClass('show', exigir);
    $('#fotoObrigatoria').toggle(exigir);

    $('#fotoHint').text(naoOk
        ? 'Foto obrigatória para registrar o item como Não OK.'
        : 'Este ativo ainda não possui foto: é necessário tirar uma foto para concluir.');
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

    $('#stOk').prop('checked', statusOk);
    $('#stNok').prop('checked', !statusOk);
    $('#areaNok').toggleClass('show', !statusOk);
    $('#modalObservacao').val(!statusOk && temAvaliacao ? (avaliacao.observacao || '') : '');

    // Caso 1 da regra da foto: ativo bipado que ainda não possui foto
    const semFoto = !(avaliacao && avaliacao.possuiFoto);
    $('#modalConfirmacao').data('semFoto', semFoto);

    atualizarAreaFoto(!statusOk, semFoto);

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

    // Foto obrigatória no Não OK e quando o ativo ainda não possui foto
    const semFoto = modal.data('semFoto') === true;

    if ((!statusOk || semFoto) && !fotoFile) {
        Swal.fire({
            title: 'Foto obrigatória',
            text: !statusOk
                ? 'É necessário tirar uma foto para registrar o item como Não OK.'
                : 'Este ativo ainda não possui foto. Tire uma foto para concluir.',
            icon: 'warning'
        });
        return;
    }

    $('#btnModalConfirmar').prop('disabled', true);

    // Monta FormData para suportar envio de arquivo
    const fd = new FormData();
    fd.append('uniqueId', $('#uniqueId').val());
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