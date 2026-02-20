
const messagesData = document.getElementById('resource-messages').getAttribute('data-messages');
const messages = JSON.parse(messagesData);

const { origin, pathname } = window.location;

const segments = pathname.split('/').filter(Boolean);

const basePath = document.baseURI.replace(/\/$/, '');

var table = null;
var currentConfig = null;
var currentStateKey = null;
var groupDefs = null;

//const basePath = window.location.pathname.split("/").slice(0, 2).join("/");

$(document).ready(function () {

    var inputFile = $("#inputFile"); 

    $('#modWarningInfo').on('shown.bs.modal', function () {
        $('#tableWarning').DataTable().columns.adjust().draw();
    });

    $(".custom-file-input").on('change', function () {
        var filename = document.getElementById("inputFile").files[0].name;
        $(this).next(".custom-file-label").addClass("selected").html(filename);
    });

    inputFile.change(function () {

        if ( inputFile.val() != "") {
            $("#btnUpload").prop('disabled', false);
        } else {
            $("#btnUpload").prop('disabled', true);
        }

    });

    // Download Excel
    $('#btnDownload').click(function () {
        var form = $('<form>', {
            method: 'POST',
            action: messages.downloadChecklistExcel
        });

        form.append($('<input>', {
            type: 'hidden',
            name: 'tipoChecklist',
            value: $('#tipoChecklist').val()
        }));

        form.append($('<input>', {
            type: 'hidden',
            name: 'codigoUnidade',
            value: $('#unidade').val()
        }));

        form.append($('<input>', {
            type: 'hidden',
            name: 'uniqueId',
            value: $('#uniqueId').val()
        }));

        $('body').append(form);
        form.submit();
        form.remove();
    });

    // Download Excel
    $('#btnUpload').on('click', function (e) {

        e.preventDefault();

        // Pegar o arquivo do input
        var fileInput = $('#inputFile')[0].files[0];

        // Verifica se um arquivo foi selecionado
        if (!fileInput) {
            Swal.fire(messages.uploadError, messages.pleaseSelectFile, 'error');
            return;
        }

        // Criar um objeto FormData
        var formData = new FormData();
        formData.append('file', fileInput);
        formData.append('codigoUnidade', ($('#unidade').val() === "" ? -1 : $('#unidade').val()));
        formData.append('tipoChecklist', $("#tipoChecklist option:selected").text());

        // Exibe o SweetAlert2 de loading
        Swal.fire({
            title: 'Uploading...',
            html: '<i class="la la-refresh text-secondary la-spin progress-icon-spin"></i>',
            allowOutsideClick: false,
            allowEscapeKey: false,
            showConfirmButton: false,
            didOpen: () => {
                Swal.showLoading();
            }
        });

        // Envio Ajax
        $.ajax({
            url: $('#btnUpload').data('url'),
            type: 'POST',
            data: formData,
            processData: false,
            contentType: false,
            success: function (response) {
                if (response.success) {
                    Swal.close();
                    $("#uniqueId").val(response.uniqueId);
                    loadGrid();
                } else {
                    Swal.close();
                    Swal.fire(messages.uploadError, response.message, 'error');
                }
            },
            error: function (xhr, status, error) {
                Swal.fire(messages.uploadError, messages.errorOnSentFile, 'error');
            }
        });
    });

    $.validator.addMethod("validPK", function (value, element) {

        var result = false;

        jQuery.ajax({
            method: "POST",
            url: "ValidaChecklist",
            async: false,
            data: {
                "descricao": value,
                "codigo": $("#codigo").val()
            },
            dataType: "json",
            success: function (response) {
                result = response;
            }
        });

        return result;

    }, messages.validPK);

    $('#form').validate({
        ignore: ".ignore-validation",
        errorClass: 'invalid-feedback animated fadeInDown',
        errorElement: 'div',
        onkeyup: false,
        onclick: false,
        errorPlacement: function (error, e) {
            jQuery(e).parents('.form-group > div').append(error);
        },
        highlight: function (e) {
            jQuery(e).closest('.form-group > div').removeClass('is-invalid').addClass('is-invalid');
        },
        success: function (e) {
            jQuery(e).closest('.form-group > div').removeClass('is-invalid');
            jQuery(e).remove();
        },
        rules: {
            'tipoChecklist': {
                required: true
            },
            'descricao': {
                required: true,
                validPK: true
            }
        },
        messages: {
            'tipoChecklist': {
                required: messages.campoObrigatorio
            },
            'descricao': {
                required: messages.campoObrigatorio,
                validPK: messages.validaBancoDados
            }
        },
        submitHandler: function (form) {
            if (gridHasErrors()) {

                Swal.fire({
                    icon: 'warning',
                    title: "Atenção",
                    text: "Este Checklist possui inconsistência.", // crie essa mensagem no resx
                    confirmButtonText: 'OK'
                });

                return false; 
            }

            return true;
        },
        invalidHandler: function (e, validation) {
            $("#btnSave").prop('disabled', false);
        }
    });

    function loadGrid() {

        var data = {
            uniqueId: $("#uniqueId").val(),
            tipoChecklist: $("#tipoChecklist").val()
        }

        loadGridMain(table, data, messages.loadChecklist, false, false, true);
    }

    loadGrid();

    $("#tipoChecklist").prop("disabled", true)
});
