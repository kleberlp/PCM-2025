const messagesData = document.getElementById('resource-messages').getAttribute('data-messages');
const messages = JSON.parse(messagesData);

//Gerenciamento de contadores

let contadores = [];
let contadorIndex = 0;

function renderContadores() {
    const container = $('#contadores-container');
    const empty = $('#contadores-empty');
    container.empty();

    if (contadores.length === 0) {
        empty.show();
        return;
    }

    empty.hide();

    contadores.forEach(function (c, idx) {
        const row = `
            <div class="card mb-10 border" id="contador-row-${idx}" style="border-radius:6px;">
                <div class="card-body py-10 px-15">
                    <div class="form-group row mb-0 align-items-center">
                        <div class="col-md-4">
                            <label class="mb-5" style="font-size:12px;font-weight:600;color:#555;">Nome <span class="text-danger">*</span></label>
                            <input type="text" class="form-control form-control-sm contador-nome"
                                   data-idx="${idx}" value="${escapeHtml(c.nome)}" maxlength="100"
                                   placeholder="Nome completo">
                        </div>
                        <div class="col-md-4">
                            <label class="mb-5" style="font-size:12px;font-weight:600;color:#555;">E-mail <span class="text-danger">*</span></label>
                            <input type="email" class="form-control form-control-sm contador-email"
                                   data-idx="${idx}" value="${escapeHtml(c.email)}"
                                   placeholder="email@exemplo.com">
                        </div>
                        <div class="col-md-3">
                            <label class="mb-5" style="font-size:12px;font-weight:600;color:#555;">Celular</label>
                            <input type="text" class="form-control form-control-sm contador-celular"
                                   data-idx="${idx}" value="${escapeHtml(c.celular)}"
                                   placeholder="(00) 00000-0000" maxlength="20">
                        </div>
                        <div class="col-md-1 text-right" style="padding-top:22px;">
                            <button type="button" class="btn btn-sm btn-danger btn-remove-contador" data-idx="${idx}" title="Remover">
                                <i class="fa fa-trash"></i>
                            </button>
                        </div>
                    </div>
                </div>
            </div>`;
        container.append(row);
    });
}

function escapeHtml(str) {
    if (!str) return '';
    return str.replace(/&/g, '&amp;').replace(/"/g, '&quot;').replace(/</g, '&lt;').replace(/>/g, '&gt;');
}

function syncContadoresFromDOM() {
    contadores.forEach(function (c, idx) {
        c.nome = $(`#contadores-container .contador-nome[data-idx="${idx}"]`).val() || '';
        c.email = $(`#contadores-container .contador-email[data-idx="${idx}"]`).val() || '';
        c.celular = $(`#contadores-container .contador-celular[data-idx="${idx}"]`).val() || '';
    });
}

$(document).on('change blur', '.contador-nome, .contador-email, .contador-celular', function () {
    const idx = parseInt($(this).data('idx'));
    if ($(this).hasClass('contador-nome')) contadores[idx].nome = $(this).val();
    if ($(this).hasClass('contador-email')) contadores[idx].email = $(this).val();
    if ($(this).hasClass('contador-celular')) contadores[idx].celular = $(this).val();
});

$('#btn-add-contador').on('click', function () {
    contadores.push({ nome: '', email: '', celular: '' });
    renderContadores();
    // foca no nome do novo item
    $(`#contadores-container .contador-nome[data-idx="${contadores.length - 1}"]`).focus();
});

$(document).on('click', '.btn-remove-contador', function () {
    const idx = parseInt($(this).data('idx'));
    contadores.splice(idx, 1);
    renderContadores();
});

// Validação e submit

jQuery(function () {
    Codebase.helpers(['datepicker', 'maxlength', 'select2']);
});

$('#form').validate({

    onkeyup: false,
    onclick: false,
    ignore: ":hidden:not(select)",

    errorClass: 'invalid-feedback animated fadeInDown',
    errorElement: 'div',

    errorPlacement: function (error, e) {
        $(e).parents('.form-group > div').append(error);
    },

    highlight: function (e) {
        $(e).closest('.form-group > div').addClass('is-invalid');
    },

    success: function (e) {
        $(e).closest('.form-group > div').removeClass('is-invalid');
        $(e).remove();
    },

    rules: {
        'unidade': { required: true },
        'descricao': { required: true }
    },

    messages: {
        'unidade': { required: messages.requiredField },
        'descricao': { required: messages.requiredField }
    },

    submitHandler: function (form) {

        // Valida contadores antes de submeter
        syncContadoresFromDOM();

        let valid = true;
        let errMsg = '';

        contadores.forEach(function (c, idx) {
            if (!c.nome.trim()) {
                valid = false;
                errMsg = `Contador ${idx + 1}: o campo Nome é obrigatório.`;
            } else if (!c.email.trim()) {
                valid = false;
                errMsg = `Contador ${idx + 1}: o campo E-mail é obrigatório.`;
            } else if (!/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(c.email.trim())) {
                valid = false;
                errMsg = `Contador ${idx + 1}: e-mail inválido.`;
            }
        });

        if (!valid) {
            Swal.fire({ icon: 'warning', title: 'Atenção', text: errMsg });
            return false;
        }

        // Serializa contadores como JSON e coloca no campo hidden
        const json = contadores.length > 0 ? JSON.stringify(contadores) : '';
        $('#contadoresJson').val(json);

        form.submit();
    }

});