
const messagesData = document.getElementById('resource-messages').getAttribute('data-messages');
const messages = JSON.parse(messagesData);
var table;

$(document).ready(function () {

    // Basic
    $('.dropify').dropify();

    $("#cardSave").hide();

    $('#modWarningInfo').on('shown.bs.modal', function () {
        $('#tableWarning').DataTable().columns.adjust().draw();
    });
    
    // Quando o botão for clicado
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
                    loadTable(response.data)
                    $("#cardSave").show();
                    $("#cardUpload").hide();
                } else {
                    Swal.close();
                    Swal.fire(messages.uploadError, messages.errorOnSentFile, 'error');
                }
            },
            error: function (xhr, status, error) {
                Swal.fire(messages.uploadError, messages.errorOnSentFile, 'error');
            }
        });
    });

    // Quando o botão for clicado
    $('#btnSave').on('click', function (e) {

        e.preventDefault();

        swal.fire({
            title: messages.msgQuestionSaveDelivery,
            icon: 'question',
            showCancelButton: true,
            confirmButtonText: messages.yes,
            cancelButtonText: messages.no,
            reverseButtons: true
        }).then(function (result) {
            if (result.value) {
                jQuery.ajax({
                    method: "POST",
                    url: "saveDelivery",
                    async: false,
                    data: {
                        "id": $("#uniqueId").val()
                    },
                    dataType: "json",
                    success: function (response) {

                        if (response.success) {
                            Swal.fire({
                                title: response.message,
                                icon: "success",
                                showDenyButton: false,
                                showCancelButton: false,
                            }).then((result) => {
                                if (result) {

                                    var drEvent = $('#inputFile').dropify();
                                    drEvent = drEvent.data('dropify');
                                    drEvent.clearElement();

                                    $("#cardUpload").show();
                                    $("#cardSave").hide();
                                }
                            });

                        } else {
                            Swal.fire({
                                title: response.message,
                                icon: "error",
                                showDenyButton: false,
                                showCancelButton: false,
                            });
                        }
                    }
                });
            }
        });

    });

    // Quando o botão for clicado
    $('#btnCancel').on('click', function (e) {

        var drEvent = $('#inputFile').dropify();
        drEvent = drEvent.data('dropify');
        drEvent.clearElement();

        $("#cardUpload").show();
        $("#cardSave").hide();

    });

    $('#tableMain tbody').on('click', 'button', function () {

        var data = table.row($(this).closest('tr')).data();

        if (this.id == "btnWarning") {
            showWarning(data);
        }

    });

});

function loadTable(data) {

    table = $('#tableMain').DataTable({
        "searching": false,
        "destroy": true,
        "lengthChange": false,
        "pageLength": 15,
        "processing": true,
        "serverSide": false,
        "data": data,
        "autoWidth": false, 
        "buttons": [
            { extend: 'copy', text: "Copiar" },
            { extend: 'excel', text: "Excel" },
            { extend: 'pdf', text: "PDF" },
            { extend: 'colvis', text: "Config" }
        ],
        "columns": [
            { data: "deliveryNumber" },
            { data: "deliveryCreateDate"},
            { data: "deliveryDate" },
            { data: "soldToParty" },
            { data: "shipToParty" },
            { data: "grossWeight" },
            {
                orderable: false,
                data: null,
                defaultContent: "<div class='btn-group'> " +
                    "<button type='button' class='btn btn-sm btn-outline-secondary waves-light waves-effect dropdown-toggle tippy-btn' title='" + messages.clickToViewAlert + "' data-tippy-arrow='true' data-tippy-arrowTransform='scale(0.75)' data-tippy-animation='fade' id='btnWarning' data-bstoggle='dropdown' aria-expanded='false'><i class='dripicons-warning text-warning'></i></button>" +
                    "</div> "
            }
        ],
        "language": {
            emptyTable: messages.emptyTable,
            "info": "",
            "infoEmpty": "",
            "infoFiltered": "",
        },
        order: [[1, 'asc']],
        columnDefs: [
            { className: 'text-center', targets: [0, 1, 2, 5, 6] },
            { width: '20px', targets: [6] },
            { width: '150px', targets: [0, 1, 2, 6] }
        ],
        createdRow: function (row, data, dataIndex) {
            if (!data.errorData || data.errorData.length === 0) {
                $(row).find('#btnWarning').hide();
            } else {
                $(row).addClass("text-danger");
            }
        }
    });

}

function showWarning(data) {

    tableWarning = $('#tableWarning').DataTable({
        fixedColumns: {
            start: 0,
            end: 1
        },
        lengthChange: false,
        pageLength: 15,
        processing: true,
        paging: false,
        searching: false,
        scrollX: true,
        scrollCollapse: true,
        serverSide: false,
        data: data.errorData,
        destroy: true,
        columns: [
            { data: "message" }
        ],
        language: {
            emptyTable: messages.emptyTable,
            info: "",
            infoEmpty: "",
            infoFiltered: "",
        },
        order: [[0, 'asc']]
    });

    $("#modWarningInfo").modal("show");

}
